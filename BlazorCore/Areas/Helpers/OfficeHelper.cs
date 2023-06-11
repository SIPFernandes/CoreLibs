using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BlazorCore.Areas.Helpers
{
    public static class OfficeHelper
    {
        public static class ExcelExtension
        {
            //public static void UploadExcel(FileUploadModel upload,
            //    Action<Dictionary<int, string>, Row> func)
            //{                
            //    using (var doc = SpreadsheetDocument.Open(upload.Stream, false))
            //    {
            //        var workbookPart = doc.WorkbookPart;

            //        var worksheetPart = workbookPart.WorksheetParts.First();

            //        var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            //        var stringTable = LoadSharedStringDictionarySax(workbookPart.SharedStringTablePart);

            //        var rows = sheetData.Elements<Row>();

            //        var totalRows = rows.Count();

            //        upload.TotalUploadSteps = totalRows;

            //        foreach (var r in rows)
            //        {
            //            if (upload.CancellationToken != null && 
            //                upload.CancellationToken.IsCancellationRequested)
            //            {
            //                throw new Exception(nameof(upload.CancellationToken));
            //            }

            //            upload.UploadCompletion = FilesHelper.GetUploadProgress(upload.TotalUploadSteps, (int)r.RowIndex.Value);

            //            func(stringTable, r);
            //        }
            //    }

            //    upload.Stream.Close();

            //    FilesHelper.GetUploadProgress(upload.TotalUploadSteps, upload.TotalUploadSteps);                                
            //}

            public static void DownloadExcel(MemoryStream ms, List<ExcelCell> list)
            {
                using var spreadSheet = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook);
                var workBookPart = spreadSheet.AddWorkbookPart();
                workBookPart.Workbook = new();

                // Get the SharedStringTablePart. If it does not exist, create a new one.
                var shareStringPart = workBookPart.AddNewPart<SharedStringTablePart>();

                //Style
                var stylesPart = workBookPart.AddNewPart<WorkbookStylesPart>();

                stylesPart.Stylesheet = new Stylesheet
                {
                    Fonts = new Fonts(new Font()),
                    Fills = new Fills(new Fill()),
                    Borders = new Borders(new Border()),
                    CellStyleFormats = new CellStyleFormats(new CellFormat()),
                    CellFormats = new CellFormats(
                        new CellFormat(),
                        new CellFormat
                        {
                            NumberFormatId = 14,
                            ApplyNumberFormat = true
                        })
                };

                // Insert a new worksheet.
                var worksheetPart = InsertWorksheet(spreadSheet.WorkbookPart);

                foreach (var val in list)
                {
                    // Insert cell A1 into the new worksheet.
                    Cell cell = InsertCellInWorksheet(val.Col, (uint)val.Row, worksheetPart);

                    string cellValue = string.Empty;

                    if (val.Type == ExcelCell.TypeEnum.Date)
                    {
                        cellValue = val.Value;

                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);

                        cell.StyleIndex = 1;
                    }
                    else
                    {
                        // Insert the text into the SharedStringTablePart.
                        var index = InsertSharedStringItem(val.Value, shareStringPart);

                        cellValue = index.ToString();

                        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                    }

                    // Set the value of cell A1.
                    cell.CellValue = new CellValue(cellValue);
                }


                // Save the new worksheet.
                worksheetPart.Worksheet.Save();
            }                        

            public class ExcelCell
            {
                public int Row { get; set; }
                public string Col { get; set; }
                public TypeEnum Type { get; set; }
                public string Value { get; set; }

                public enum TypeEnum
                {
                    Text,
                    Date
                }
            }

            // Given a WorkbookPart, inserts a new worksheet.
            private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
            {
                // Add a new worksheet part to the workbook.
                var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());
                newWorksheetPart.Worksheet.Save();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                // Append the new worksheet and associate it with the workbook.
                Sheet sheet = new()
                {
                    Id = workbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = 1,
                    Name = "Sheet" + 1
                };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                return newWorksheetPart;
            }

            // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
            // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
            private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
            {
                // If the part does not contain a SharedStringTable, create one.
                if (shareStringPart.SharedStringTable == null)
                {
                    shareStringPart.SharedStringTable = new SharedStringTable();
                }

                int i = 0;

                // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
                {
                    if (item.InnerText == text)
                    {
                        return i;
                    }

                    i++;
                }

                // The text does not exist in the part. Create the SharedStringItem and return its index.
                shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
                shareStringPart.SharedStringTable.Save();

                return i;
            }

            // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
            // If the cell already exists, returns it. 
            private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                string cellReference = columnName + rowIndex;

                // If the worksheet does not contain a row with the specified row index, insert one.
                Row row;
                if (sheetData.Elements<Row>().Any(r => r.RowIndex == rowIndex))
                {
                    row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
                }
                else
                {
                    row = new Row() { RowIndex = rowIndex };
                    sheetData.Append(row);
                }

                // If there is not a cell with the specified column name, insert one.  
                if (row.Elements<Cell>().Any(c => c.CellReference.Value == columnName + rowIndex))
                {
                    return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
                }
                else
                {
                    // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                    Cell refCell = null;
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        if (cell.CellReference.Value.Length == cellReference.Length)
                        {
                            if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                            {
                                refCell = cell;
                                break;
                            }
                        }
                    }

                    Cell newCell = new() { CellReference = cellReference };
                    row.InsertBefore(newCell, refCell);

                    worksheet.Save();
                    return newCell;
                }
            }

            private static Dictionary<int, string> LoadSharedStringDictionarySax(SharedStringTablePart sharedStringTablePart)
            {
                Dictionary<int, string> dictionary = new Dictionary<int, string>();

                using (OpenXmlReader reader = OpenXmlReader.Create(sharedStringTablePart))
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(SharedStringItem))
                        {
                            SharedStringItem ssi = (SharedStringItem)reader.LoadCurrentElement();

                            dictionary.Add(i++, ssi.InnerText);
                        }
                    }
                }

                return dictionary;
            }
        }
    }
}
