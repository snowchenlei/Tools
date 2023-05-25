namespace Snow.Tools.Excel;

/// <summary>
/// Excel坐标
/// </summary>
/// <param name="Row">行</param>
/// <param name="Column">列</param>
public readonly record struct ExcelCoordinate(int Row, int Column)
{
}