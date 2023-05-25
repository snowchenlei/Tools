using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Table;

namespace Snow.Tools.Excel;

public class EpplusExcelOperation
{
    public EpplusExcelOperation()
    {
        ExcelPackage.LicenseContext = LicenseContext.Commercial;
    }

    public MemoryStream Export(DataTable table, string cell)
    {
        using ExcelPackage pck = new ExcelPackage();
        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(table.TableName);
        ws.Cells[cell].LoadFromDataTable(table, true);
        using var ms = new MemoryStream();
        pck.SaveAs(ms);
        return ms;
    }
}