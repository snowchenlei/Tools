using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using JetBrains.Annotations;
using MathNet.Numerics.Distributions;
using Microsoft.VisualBasic.CompilerServices;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Snow.Tools.Excel;

public class NpoiExcelOperation
{
    public NpoiExcelOperation()
    {
        ExcelType = ExcelType.Xlsx;
        Options = new ExcelOptions()
        {
            DateTimeFormat = "yyyy-MM-dd"
        };
    }

    public ExcelType ExcelType { get; set; }
    public ExcelOptions Options { get; set; }

    public MemoryStream Export(DataTable table, int row = 1, int column = 1)
    {
        Check.NotNull(table, nameof(table));
        Check.NotNullOrEmpty(table.TableName, nameof(table.TableName));
        Check.Range(row, nameof(row), 1);
        if (table.Rows.Count <= 0)
        {
            throw new ArgumentException($"{nameof(table)} is empty");
        }

        int startRowIndex = --row;
        int startColumnIndex = --column;

        using IWorkbook workbook = ExcelType switch
        {
            ExcelType.Xls => new HSSFWorkbook(),
            ExcelType.Xlsx => new XSSFWorkbook(),
            _ => new XSSFWorkbook()
        };
        var sheet = workbook.CreateSheet(table.TableName);
        //格式
        var dateStyle = workbook.CreateCellStyle();
        var format = workbook.CreateDataFormat();
        dateStyle.DataFormat = format.GetFormat(Options.DateTimeFormat);//日期格式

        CreateHeader(table, sheet, startRowIndex);
        SetData(table, sheet, startRowIndex, dateStyle);

        using var stream = new MemoryStream();
        workbook.Write(stream, true);
        stream.Flush();
        stream.Position = 0;

        return stream;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="sheet"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="dateStyle"></param>
    private static void SetData(KeyValuePair<Type, string>[][] data, ISheet sheet, int startRowIndex, ICellStyle dateStyle)
    {
        for (int i = 0; i < data.Length; i++)
        {
            var dataRow = sheet.CreateRow(startRowIndex + i);
            for (int j = 0; j < data[i].Length; j++)
            {
                var cell = dataRow.CreateCell(j);

                string drValue = data[i][j].Value;
                SetCellValue(data[i][j].Key, cell, drValue, dateStyle);
            }
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="table"></param>
    /// <param name="sheet"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="dateStyle"></param>
    private static void SetData(DataTable table, ISheet sheet, int startRowIndex, ICellStyle dateStyle)
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

        SetData(data.ToArray(), sheet, startRowIndex, dateStyle);
    }

    /// <summary>
    /// 设置单元格值
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="cell"></param>
    /// <param name="drValue"></param>
    /// <param name="dateStyle"></param>
    private static void SetCellValue(Type dataType, ICell cell, string drValue, ICellStyle dateStyle)
    {
        switch (dataType.ToString())
        {
            case "System.String":
                cell.SetCellValue(drValue);
                break;

            case "System.DateTime": //日期类型
                DateTime.TryParse(drValue, out var dateValue);
                cell.SetCellValue(dateValue);
                cell.CellStyle = dateStyle;
                break;

            case "System.Boolean": //布尔型
                bool.TryParse(drValue, out var boolValue);
                cell.SetCellValue(boolValue);
                break;

            case "System.Int16": //整型
            case "System.Int32":
            case "System.Int64":
            case "System.Byte":
                int.TryParse(drValue, out var intValue);
                cell.SetCellValue(intValue);
                break;

            case "System.Decimal": //浮点型
            case "System.Double":
                double.TryParse(drValue, out var doubleValue);
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

    /// <summary>
    /// 创建表头
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <param name="sheet"></param>
    /// <param name="rowIndex">行号</param>
    private static void CreateHeader(Type entityType, ISheet sheet, int rowIndex)
    {
        PropertyInfo[] entityProperties = entityType.GetProperties();

        var columns = new List<string>();
        foreach (var property in entityProperties)
        {
            string name;
            var description = property.GetCustomAttribute<DescriptionAttribute>();
            if (description != null && !description.Description.IsNullOrWhiteSpace())
            {
                name = description.Description;
            }
            else
            {
                name = property.Name;
            }
            columns.Add(name);
        }

        CreateHeader(columns.ToArray(), sheet, rowIndex);
    }

    /// <summary>
    /// 创建表头
    /// </summary>
    /// <param name="table">表</param>
    /// <param name="sheet"></param>
    /// <param name="rowIndex">行号</param>
    private static void CreateHeader(DataTable table, ISheet sheet, int rowIndex)
    {
        var columns = new List<string>();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            columns.Add(table.Columns[i].ColumnName);
        }

        CreateHeader(columns.ToArray(), sheet, rowIndex);
    }

    /// <summary>
    /// 创建表头
    /// </summary>
    /// <param name="columns">标题</param>
    /// <param name="sheet"></param>
    /// <param name="rowIndex">行号</param>
    private static void CreateHeader(string[] columns, ISheet sheet, int rowIndex)
    {
        IRow row = sheet.CreateRow(rowIndex);
        for (int i = 0; i < columns.Length; i++)
        {
            ICell cell = row.CreateCell(i);
            cell.SetCellValue(columns[i]);
        }
    }
}