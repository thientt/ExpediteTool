using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;

namespace ExpediteTool.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportExcel
    {
        private const string nameExpediteLotList = "ACTIVE Lots";
        private const string namePendingLotList = "PENDING Lots";
        private const string nameClosedLotList = "CLOSED Lots";
        private string _pathRoot = String.Empty;
        private object misValue = System.Reflection.Missing.Value;

        private ILogService _logService = null;

        private Excel.Application excelApp;
        private Excel.Workbook workBook;
        private Excel.Worksheet workSheetActivedLotList;
        private Excel.Worksheet workSheetPendingLotList;
        private Excel.Worksheet workSheetClosedLotList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathRoot"></param>
        public ExportExcel(string pathRoot, ILogService logService)
        {
            try
            {
                _logService = logService;

                _pathRoot = pathRoot;
                excelApp = new Excel.Application();
                workBook = excelApp.Workbooks.Add(Type.Missing);
                //
                workSheetClosedLotList = workBook.Sheets.Add(misValue, misValue, 1, misValue);
                workSheetClosedLotList.Name = nameClosedLotList;
                //
                workSheetPendingLotList = workBook.Sheets.Add(misValue, misValue, 1, misValue);
                workSheetPendingLotList.Name = namePendingLotList;
                //
                workSheetActivedLotList = workBook.Sheets.Add(misValue, misValue, 1, misValue);
                workSheetActivedLotList.Name = nameExpediteLotList;

                //Delete sheet default
                for (int i = excelApp.ActiveWorkbook.Worksheets.Count; i > 0; i--)
                {
                    Excel.Worksheet wkSheet = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[i];
                    if (wkSheet.Name.Contains("Sheet"))
                    {
                        wkSheet.Delete();
                    }
                }
                excelApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                excelApp.Application.ScreenUpdating = false;
                excelApp.DisplayAlerts = false;
            }
            catch (Exception ex)
            {
                NAR(workSheetActivedLotList);
                NAR(workSheetPendingLotList);
                NAR(workSheetClosedLotList);
                NAR(workBook);
                excelApp.Quit();
                NAR(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateHeader()
        {
            var cell1 = workSheetActivedLotList.Cells[2, 2];
            var cell2 = workSheetActivedLotList.Cells[2, 11];
            Excel.Range rangeHeader = workSheetActivedLotList.Range[cell1, cell2] as Excel.Range;
            FormatHeader(rangeHeader, nameExpediteLotList.ToUpper());
            //
            cell1 = workSheetPendingLotList.Cells[2, 2];
            cell2 = workSheetPendingLotList.Cells[2, 11];
            rangeHeader = workSheetPendingLotList.Range[cell1, cell2] as Excel.Range;
            FormatHeader(rangeHeader, namePendingLotList.ToUpper());
            //
            cell1 = workSheetClosedLotList.Cells[2, 2];
            cell2 = workSheetClosedLotList.Cells[2, 11];
            rangeHeader = workSheetClosedLotList.Range[cell1, cell2] as Excel.Range;
            FormatHeader(rangeHeader, nameClosedLotList.ToUpper());
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateTitle()
        {
            var cell1 = workSheetActivedLotList.Cells[4, 2];
            var cell2 = workSheetActivedLotList.Cells[4, 11];
            Excel.Range rangeTitle = workSheetActivedLotList.Range[cell1, cell2] as Excel.Range;
            WriteTitle(rangeTitle);
            //
            cell1 = workSheetPendingLotList.Cells[4, 2];
            cell2 = workSheetPendingLotList.Cells[4, 11];
            rangeTitle = workSheetPendingLotList.Range[cell1, cell2] as Excel.Range;
            WriteTitle(rangeTitle);
            //
            cell1 = workSheetClosedLotList.Cells[4, 2];
            cell2 = workSheetClosedLotList.Cells[4, 11];
            rangeTitle = workSheetClosedLotList.Range[cell1, cell2] as Excel.Range;
            WriteTitle(rangeTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateContent()
        {
            var items = HotLotsList.Where(x => x.Status == Model.StatusType.ACTIVED).ToList();
            WriteContent(workSheetActivedLotList, items);
            Excel.Range rangeFilter = (workSheetActivedLotList.Cells[4, 2] as Excel.Range);
            rangeFilter.AutoFilter(1, BuList, Excel.XlAutoFilterOperator.xlFilterValues, misValue, true);

            items = HotLotsList.Where(x => x.Status == Model.StatusType.PENDING).ToList();
            WriteContent(workSheetPendingLotList, items);
            rangeFilter = workSheetPendingLotList.Cells[4, 2];
            rangeFilter.AutoFilter(1, BuList, Excel.XlAutoFilterOperator.xlFilterValues, misValue, true);

            items = HotLotsList.Where(x => x.Status == Model.StatusType.CLOSED).ToList();
            WriteContent(workSheetClosedLotList, items);
            rangeFilter = workSheetClosedLotList.Cells[4, 2];
            rangeFilter.AutoFilter(1, BuList, Excel.XlAutoFilterOperator.xlFilterValues, misValue, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="title"></param>
        private void FormatHeader(Excel.Range range, string title)
        {
            range.Merge(true);
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            range.Value2 = title;
            range.RowHeight = 35;
            range.Font.Name = "Arial Unicode MS";
            range.Font.Bold = true;
            range.Font.Size = 20;
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        private void WriteTitle(Excel.Range range)
        {
            int startCol = 1;
            int startRow = 1;

            range.EntireRow.AutoFit();
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            range.Font.Bold = true;
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            //
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Bu's";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 10;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "LotID";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 15;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Device";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 15;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Reason";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 50;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Requestor";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 10;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Owner";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 10;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Request Out Date";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 15;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "SCM End Date";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 15;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Comment";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 10;
            //
            startCol++;
            (range.Cells[startRow, startCol] as Excel.Range).Value2 = "Current Operation";
            (range.Cells[startRow, startCol] as Excel.Range).ColumnWidth = 20;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="items"></param>
        private void WriteContent(Excel.Worksheet sheet, List<LotExpediteDto> items)
        {
            int startRow = 4;
            int startCol = 2;
            sheet.EnableCalculation = false;

            foreach (LotExpediteDto item in items)
            {
                startCol = 2;
                startRow++;
                Excel.Range rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.Bu.BuName;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.LotId;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.Device;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.Reason;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.Requestor;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value2 = item.Owner;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "mm/dd/yyyy";
                rangeContent.Value2 = item.RequestOutDate.ToString("MM/dd/yyyy");
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "mm/dd/yyyy";
                rangeContent.Value2 = item.ScmEndDate.HasValue ? item.ScmEndDate.Value.ToString("MM/dd/yyyy") : "";

                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value = item.Comment;
                //
                startCol++;
                rangeContent = (sheet.Cells[startRow, startCol] as Excel.Range);
                rangeContent.NumberFormat = "@";
                rangeContent.Value = item.CurrentOperation;
            }
            var cell1 = sheet.Cells[4, 2];
            var cell2 = sheet.Cells[startRow, startCol];
            Excel.Range range = sheet.Range[cell1, cell2];
            SetBorder(range, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        private void SetBorder(Excel.Range range, int color)
        {
            Excel.Borders borders = range.Borders;
            borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            borders.Color = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveFile()
        {
            string fileName = String.Empty;
            try
            {
                fileName = Guid.NewGuid().ToString() + ".xlsx";
                string folderReport = Path.Combine(_pathRoot, "Reports");
                if (!Directory.Exists(folderReport))
                    Directory.CreateDirectory(folderReport);

                string filePath = System.IO.Path.Combine(folderReport, fileName);
                workBook.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                fileName = String.Empty;
                _logService.Error(ex.Message, ex);
            }

            finally
            {
                NAR(workSheetActivedLotList);
                NAR(workSheetPendingLotList);
                NAR(workSheetClosedLotList);
                workBook.Close(false);
                NAR(workBook);
                excelApp.Quit();
                NAR(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void NAR(object obj)
        {
            try
            {
                while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj) > 0) ;
            }
            catch { }
            finally
            {
                obj = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<LotExpediteDto> HotLotsList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String[] BuList { get; set; }
    }
}