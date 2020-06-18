using GitHubTelemetry.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace GitHubTelemetry.Services
{
    public class ExcelService : IDisposable
    {
        /// <summary>
        /// Writes out to an excel file contained in the runtimes folder
        /// </summary>
        /// <param name="pullRequests"></param>
        public string ExportPullRequestData(List<PullRequest> pullRequests)
        {
            // instantiating NPOI objects
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Telemetry");

            DataTable table = new DataTable();

            // Can make this dynamic 
            table.Columns.Add("Date");
            table.Columns.Add("GitHub Handle");
            table.Columns.Add("MSFT/External");
            table.Columns.Add("Total Contribution");

            foreach (PullRequest pr in pullRequests)
                table.Rows.Add(pr.Date, pr.User.Login, pr.Group, pr.Files.Count);

            // Create the sheet
            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(table.Columns[i].ColumnName);

                IFont fontStyle = workbook.CreateFont();
                fontStyle.IsBold = true;

                cell.CellStyle.SetFont(fontStyle);
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                IRow sheetRow = sheet.CreateRow(i + 1);

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    ICell cell = sheetRow.CreateCell(j);
                    string cellValue = table.Rows[i][j].ToString();
                    cell.SetCellValue(cellValue);
                }
            }

            // Writing out to the same directory as the app
            string fileName = $"Telemetry_{DateTime.Now.ToString("MMddyyyyHHmmss")}.xlsx";
            using (System.IO.FileStream file = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                workbook.Write(file);

            Console.WriteLine($"Successfully saved file: {fileName}");
            return fileName;
        }

        public void Dispose()
        {

        }
    }
}
