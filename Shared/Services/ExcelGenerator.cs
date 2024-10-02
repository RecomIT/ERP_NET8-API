using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;


namespace Shared.Services
{
    public class ExcelGenerator
    {

        public byte[] GenerateExcelAsset(DataTable dataTable, string sheetName)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                var modelCells = worksheet.Cells["A1"];
                int fromRow = 1;
                int fromColumn = 1;
                int lastRowNo = dataTable.Rows.Count + 1;
                int lastColumnNo = dataTable.Columns.Count;

                modelCells.LoadFromDataTable(dataTable, true).AutoFitColumns();

                var range = worksheet.Cells[fromRow, fromColumn, lastRowNo, lastColumnNo];

                // Write column headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // Write data rows
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        //worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Auto fit columns for better appearance
                worksheet.Cells.AutoFitColumns();

                // Formatting the data

                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                worksheet.DefaultRowHeight = 16;

                // Assign borders
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Column Header formatting
                var columnHeader = worksheet.Cells[fromRow, fromColumn, fromRow, lastColumnNo];
                columnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                columnHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                columnHeader.Style.WrapText = true;
                columnHeader.Style.Font.Bold = true;
                columnHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Height = 24.60;

                // Convert the Excel package to a byte array
                package.Save();
                return package.GetAsByteArray();
            }
        }
        public byte[] GenerateExcel(DataTable dataTable, string sheetName)
        {
            using (ExcelPackage package = new ExcelPackage()) {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                var modelCells = worksheet.Cells["A1"];
                int fromRow = 1;
                int fromColumn = 1;
                int lastRowNo = dataTable.Rows.Count + 1;
                int lastColumnNo = dataTable.Columns.Count;

                modelCells.LoadFromDataTable(dataTable, true).AutoFitColumns();

                var range = worksheet.Cells[fromRow, fromColumn, lastRowNo, lastColumnNo];

                // Write column headers
                for (int i = 0; i < dataTable.Columns.Count; i++) {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // Write data rows
                for (int row = 0; row < dataTable.Rows.Count; row++) {
                    for (int col = 0; col < dataTable.Columns.Count; col++) {
                        //worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Auto fit columns for better appearance
                worksheet.Cells.AutoFitColumns();

                // Formatting the data

                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                worksheet.DefaultRowHeight = 16;

                // Assign borders
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Column Header formatting
                var columnHeader = worksheet.Cells[fromRow, fromColumn, fromRow, lastColumnNo];
                columnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                columnHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                columnHeader.Style.WrapText = true;
                columnHeader.Style.Font.Bold = true;
                columnHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Height = 24.60;

                // Convert the Excel package to a byte array
                package.Save();
                return package.GetAsByteArray();
            }
        }
        public byte[] GenerateExcel(List<string> columns, string sheetName)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                var modelCells = worksheet.Cells["A1"];
                int fromRow = 1;
                int fromColumn = 1;
                int lastRowNo = 6;
                int lastColumnNo = columns.Count;

                //var dt = columns.ToDataTable();
                int tableRow = 5;
                DataTable dt = new DataTable();
                dt.AddColumns(columns.ToArray(), tableRow);
                modelCells.LoadFromDataTable(dt, true).AutoFitColumns();

                var range = worksheet.Cells[fromRow, fromColumn, lastRowNo, lastColumnNo];

                // Write column headers
                for (int i = 0; i < columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columns[i];
                }

                // Auto fit columns for better appearance
                worksheet.Cells.AutoFitColumns();

                // Formatting the data
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                worksheet.DefaultRowHeight = 16;

                // Assign borders
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Column Header formatting
                var columnHeader = worksheet.Cells[fromRow, fromColumn, fromRow, lastColumnNo];
                columnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                columnHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                columnHeader.Style.WrapText = true;
                columnHeader.Style.Font.Bold = true;
                columnHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Height = 24.60;

                // Convert the Excel package to a byte array
                package.Save();
                return package.GetAsByteArray();
            }
        }
        public byte[] GenerateExcel(DataTable dataTable, string sheetName, Color? color= null, Dictionary<string,string> columnTypes=null)
        {
            using (ExcelPackage package = new ExcelPackage()) {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                var modelCells = worksheet.Cells["A1"];
                int fromRow = 1;
                int fromColumn = 1;
                int lastRowNo = dataTable.Rows.Count + 1;
                int lastColumnNo = dataTable.Columns.Count;

                modelCells.LoadFromDataTable(dataTable, true).AutoFitColumns();

                var range = worksheet.Cells[fromRow, fromColumn, lastRowNo, lastColumnNo];

                // Write column headers
                for (int i = 0; i < dataTable.Columns.Count; i++) {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // Write data rows
                for (int row = 0; row < dataTable.Rows.Count; row++) {
                    for (int col = 0; col < dataTable.Columns.Count; col++) {
                        //worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "0.00";
                        var currentColumnName = dataTable.Columns[col].ColumnName;
                        if (columnTypes != null) {
                           var columnInfo = columnTypes.Where(col => col.Key == currentColumnName).FirstOrDefault();
                            if(columnInfo.Value == "Text") {
                                worksheet.Cells[row + 2, col + 1].Value = string.Format("'{0}", dataTable.Rows[row][col].ToString());
                            }
                            if (columnInfo.Value == "Number") {
                                worksheet.Cells[row + 2, col + 1].Value = Utility.TryParseLong(dataTable.Rows[row][col].ToString());
                            }
                        }
                        else {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }
                }

                // Auto fit columns for better appearance
                worksheet.Cells.AutoFitColumns();

                // Formatting the data

                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                worksheet.DefaultRowHeight = 16;

                // Assign borders
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Column Header formatting
                var columnHeader = worksheet.Cells[fromRow, fromColumn, fromRow, lastColumnNo];
                columnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                if(color == null) {
                    columnHeader.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                }
                else {
                    columnHeader.Style.Fill.BackgroundColor.SetColor(color?? Color.LightSkyBlue);
                }
                
                columnHeader.Style.WrapText = true;
                columnHeader.Style.Font.Bold = true;
                columnHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Height = 24.60;

                // Convert the Excel package to a byte array
                package.Save();
                return package.GetAsByteArray();
            }
        }
        public byte[] SalarySheetGenerateExcel(DataTable dataTable)
        {
            using (ExcelPackage package = new ExcelPackage()) {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("SalarySheet");

                var modelCells = worksheet.Cells["A1"];
                int fromRow = 1;
                int fromColumn = 1;
                int lastRowNo = dataTable.Rows.Count + 1;
                int lastColumnNo = dataTable.Columns.Count;

                modelCells.LoadFromDataTable(dataTable, true).AutoFitColumns();

                var range = worksheet.Cells[fromRow, fromColumn, lastRowNo, lastColumnNo];

                // Write column headers
                for (int i = 0; i < dataTable.Columns.Count; i++) {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // Group data by EmployeeCode
                var groupedData = dataTable.AsEnumerable().GroupBy(row => row.Field<string>("EmployeeCode"));

                int currentRow = 2;

                foreach (var group in groupedData) {
                    int rowCount = group.Count();

                    // Merge cells for each column in the current group
                    for (int col = 0; col < dataTable.Columns.Count; col++) {
                        if (col < 6) {
                            var mergeRange = worksheet.Cells[currentRow, col + 1, currentRow + rowCount - 1, col + 1];
                            mergeRange.Merge = true;
                            mergeRange.Value = group.First()[col];  // Use the value from the first row of the group
                        }
                        else {
                            for (int row = 0; row < rowCount; row++) {
                                worksheet.Cells[currentRow + row, col + 1].Value = group.ElementAt(row)[col];
                                //if (col > 7) {

                                //}
                                //else {
                                //    worksheet.Cells[currentRow + row, col + 1].Value = group.ElementAt(row)[col];
                                //}
                            }
                        }
                    }

                    currentRow += rowCount;
                }

                // Auto fit columns for better appearance
                worksheet.Cells.AutoFitColumns();

                // Formatting the data
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                worksheet.DefaultRowHeight = 16;

                // Assign borders
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Column Header formatting
                var columnHeader = worksheet.Cells[fromRow, fromColumn, fromRow, lastColumnNo];
                columnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                columnHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                columnHeader.Style.WrapText = true;
                columnHeader.Style.Font.Bold = true;
                columnHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Height = 24.60;

                // Save or perform other actions as needed


                // Save or perform other actions as needed


                // Convert the Excel package to a byte array
                package.Save();
                return package.GetAsByteArray();
            }
        }
    }
}
