using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using NPOI.SS.Formula;

namespace Snow.Tools.Excel;

public class NpoiExcel : IExcel
{
    public NpoiExcel()
    {
        Options = new ExcelOptions()
        {
            DateTimeFormat = "yyyy-MM-dd"
        };
    }

    public ExcelType ExcelType { get; set; }
    public ExcelOptions Options { get; set; }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="table">数据</param>
    /// <param name="cell">单元格</param>
    /// <returns>数据留</returns>
    public MemoryStream Export(DataTable table, string cell)
    {
        using var workbook = ExportData(table, cell);
        using var stream = new MemoryStream();
        workbook.Write(stream, true);
        stream.Flush();
        stream.Position = 0;

        return stream;
    }

    /// <summary>
    /// 导出文件
    /// </summary>
    /// <param name="table">数据</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="cell">单元格</param>
    public void ExportFile(DataTable table, string filePath, string cell)
    {
        Check.NotNullOrEmpty(filePath, nameof(filePath));
        using var workbook = ExportData(table, cell);
        using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        workbook.Write(fs, false);
    }

    private IWorkbook ExportData(DataTable table, string cell)
    {
        Check.NotNull(table, nameof(table));
        Check.NotNullOrEmpty(table.TableName, nameof(table.TableName));
        if (table.Rows.Count <= 0)
        {
            throw new ArgumentException($"{nameof(table)} is empty");
        }

        var workbook = CreateWorkbook();
        var coordinate = CellToCoordinate(cell);
        var startRowIndex = coordinate.Row - 1;
        var startColumnIndex = coordinate.Column - 1;

        var sheet = workbook.CreateSheet(table.TableName);
        CreateHeader(table, sheet, startRowIndex++, startColumnIndex);
        SetData(table, sheet, startRowIndex, startColumnIndex);
        return workbook;
    }

    private IWorkbook CreateWorkbook()
    {
        return ExcelType switch
        {
            ExcelType.Xls => new HSSFWorkbook(),
            ExcelType.Xlsx => new XSSFWorkbook(),
            _ => new XSSFWorkbook()
        };
    }

    private IWorkbook CreateWorkbook(Stream stream)
    {
        return ExcelType switch
        {
            ExcelType.Xls => new HSSFWorkbook(stream),
            ExcelType.Xlsx => new XSSFWorkbook(stream),
            _ => new XSSFWorkbook(stream)
        };
    }

    /// <summary>
    /// 导入第一个sheet
    /// </summary>
    /// <param name="stream">数据流</param>
    /// <returns>数据</returns>
    public DataTable ImportToTable(Stream stream)
    {
        var workbook = CreateWorkbook(stream);
        ISheet sheet = workbook.GetSheetAt(0);
        return ToTable(sheet);
    }

    /// <summary>
    /// 导入DataSet
    /// </summary>
    /// <param name="stream">数据流</param>
    /// <returns>数据集</returns>
    public DataSet ImportToDataSet(Stream stream)
    {
        var workbook = CreateWorkbook(stream);
        DataSet ds = new DataSet();
        for (int i = 0; i < workbook.NumberOfSheets; i++)
        {
            ISheet sheet = workbook.GetSheetAt(i);
            ds.Tables.Add(ToTable(sheet));
        }
        return ds;
    }

    /// <summary>
    /// 导入第一个sheet
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>数据</returns>
    public DataTable ImportToTable(string filePath)
    {
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return ImportToTable(fs);
    }

    /// <summary>
    /// 导入DataSet
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>数据集</returns>
    public DataSet ImportToDataSet(string filePath)
    {
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return ImportToDataSet(fs);
    }

    private DataTable ToTable(ISheet sheet)
    {
        DataTable table = new DataTable();

        var header = sheet.GetRow(sheet.FirstRowNum);
        var firstRowFirstCellNumber = header.FirstCellNum;
        for (int i = firstRowFirstCellNumber; i < header.LastCellNum; i++)
        {
            object obj = GetValue(header.GetCell(i));
            if (obj == null || obj.ToString() == string.Empty)
            {
                table.Columns.Add(new DataColumn("Columns" + i));
            }
            else
            {
                table.Columns.Add(new DataColumn(obj.ToString()));
            }
        }

        //数据
        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            DataRow dr = table.NewRow();
            bool hasValue = false;
            IRow row = sheet.GetRow(i);
            for (int j = firstRowFirstCellNumber; j < row.LastCellNum; j++)
            {
                var tableColumnIndex = j - firstRowFirstCellNumber;
                dr[tableColumnIndex] = GetValue(sheet.GetRow(i).GetCell(j));
                if (!hasValue && !dr[tableColumnIndex].ToString().IsNullOrWhiteSpace())
                {
                    hasValue = true;
                }
            }

            if (hasValue)
            {
                table.Rows.Add(dr);
            }
        }

        return table;
    }

    private object GetValue(ICell cell)
    {
        if (cell == null)
            return null;
        switch (cell.CellType)
        {
            case CellType.Blank:
                return null;

            case CellType.Boolean:
                return cell.BooleanCellValue;

            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue;
                }

                return cell.NumericCellValue;

            case CellType.String:
                return cell.StringCellValue;

            case CellType.Error:
                return cell.ErrorCellValue;

            case CellType.Formula:
                {
                    BaseFormulaEvaluator evaluator;
                    if (cell is XSSFCell)
                    {
                        evaluator = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                    }
                    else
                    {
                        evaluator = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                    }

                    var formulaValue = evaluator.Evaluate(cell);
                    if (formulaValue.CellType == CellType.Numeric)
                    {
                        return formulaValue.NumberValue;
                    }
                    else if (formulaValue.CellType == CellType.String)
                    {
                        return formulaValue.StringValue;
                    }

                    return cell.ToString();
                }
            default:
                return cell.ToString();
        }
    }

    /// <summary>
    /// 单元格转坐标
    /// </summary>
    /// <param name="cell">单元格</param>
    /// <returns></returns>
    private ExcelCoordinate CellToCoordinate(string cell)
    {
        var regex = new Regex("[A-Z]+");
        var column = regex.Match(cell).Value;
        var row = cell.Replace(column, "");

        var chars = column.ToCharArray();
        int columnIndex = chars.Select((t, i) => (t - 'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1)).Sum();
        if (!int.TryParse(row, out int rowIndex))
        {
            rowIndex = 0;
        }

        return new ExcelCoordinate(rowIndex, columnIndex);
    }

    /// <summary>
    /// 创建表头
    /// </summary>
    /// <param name="table">表</param>
    /// <param name="sheet"></param>
    /// <param name="rowIndex">行号</param>
    /// <param name="columnIndex">列号</param>
    private void CreateHeader(DataTable table, ISheet sheet, int rowIndex, int columnIndex)
    {
        var columns = new List<string>();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            columns.Add(table.Columns[i].ColumnName);
        }

        CreateHeader(columns.ToArray(), sheet, rowIndex, columnIndex);
    }

    /// <summary>
    /// 创建表头
    /// </summary>
    /// <param name="columns">标题</param>
    /// <param name="sheet"></param>
    /// <param name="rowIndex">行号</param>
    /// <param name="columnIndex">列号</param>
    private void CreateHeader(string[] columns, ISheet sheet, int rowIndex, int columnIndex)
    {
        IRow row = sheet.CreateRow(rowIndex);
        for (int i = 0; i < columns.Length; i++)
        {
            ICell cell = row.CreateCell(columnIndex + i);
            SetCellValue(typeof(string), cell, columns[i]);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="sheet"></param>
    /// <param name="startRowIndex">行号</param>
    /// <param name="columnIndex">列号</param>
    private void SetData(KeyValuePair<Type, string>[][] data, ISheet sheet, int startRowIndex, int columnIndex)
    {
        for (int i = 0; i < data.Length; i++)
        {
            var dataRow = sheet.CreateRow(startRowIndex + i);
            for (int j = 0; j < data[i].Length; j++)
            {
                var cell = dataRow.CreateCell(columnIndex + j);

                string drValue = data[i][j].Value;
                SetCellValue(data[i][j].Key, cell, drValue);
            }
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="table">表格</param>
    /// <param name="sheet"></param>
    /// <param name="startRowIndex">行号</param>
    /// <param name="columnIndex">列号</param>
    private void SetData(DataTable table, ISheet sheet, int startRowIndex, int columnIndex)
    {
        var data = new List<KeyValuePair<Type, string>[]>();
        foreach (DataRow row in table.Rows)
        {
            var item = new List<KeyValuePair<Type, string>>();
            foreach (DataColumn column in table.Columns)
            {
                item.Add(new KeyValuePair<Type, string>(column.DataType, row[column].ToString()));
            }

            data.Add(item.ToArray());
        }

        SetData(data.ToArray(), sheet, startRowIndex, columnIndex);
    }

    /// <summary>
    /// 设置单元格值
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="cell"></param>
    /// <param name="value"></param>
    /// <param name="dateStyle"></param>
    protected virtual void SetCellValue(Type dataType, ICell cell, string value)
    {
        switch (dataType.ToString())
        {
            case "System.String":
                cell.SetCellValue(value);
                break;

            case "System.DateTime": //日期类型
                DateTime.TryParse(value, out var dateValue);
                cell.SetCellValue(dateValue);
                var dateStyle = CreateDateStyle(cell.Sheet.Workbook);
                cell.CellStyle = dateStyle;
                break;

            case "System.Boolean": //布尔型
                bool.TryParse(value, out var boolValue);
                cell.SetCellValue(boolValue);
                break;

            case "System.Int16": //整型
            case "System.Int32":
            case "System.Int64":
            case "System.Byte":
                int.TryParse(value, out var intValue);
                cell.SetCellValue(intValue);
                break;

            case "System.Decimal": //浮点型
            case "System.Double":
                double.TryParse(value, out var doubleValue);
                cell.SetCellValue(doubleValue);
                break;

            case "System.DBNull": //空值处理
                cell.SetCellType(CellType.Blank);
                break;

            default:
                cell.SetCellType(CellType.Blank);
                break;
        }
    }

    private ICellStyle CreateDateStyle(IWorkbook workbook)
    {
        var dateStyle = workbook.CreateCellStyle();
        var format = workbook.CreateDataFormat();
        dateStyle.DataFormat = format.GetFormat(Options.DateTimeFormat);
        return dateStyle;
    }
}