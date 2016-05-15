using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using ExpediteTool.Model.DataTransfer;
using System.Data.Entity;
using Ninject;
using ExpediteTool.Model.Abstracts;

namespace ExpediteTool.Web
{
    public class BulkLoader
    {
        private const string SHEETNAMELOT = "Lot data";
        private const string SHEETNAMEBU = "BUs";
        private const int ROWINIT = 4;

        private List<LotExpediteDto> lots;
        private int colTotal = 9;
        private string filePath = string.Empty;

        private Workbook appWorkBook;
        private Worksheet appWorkSheetLots;
        private Worksheet appWorkSheetBu;
        private Range UseRange;

        public BulkLoader(string fileName)
        {
            filePath = fileName;
            lots = new List<LotExpediteDto>();
            Read();
        }

        private void Read()
        {
            Application appExcel = new Application();
            try
            {
                appExcel.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                true, false, 0, true, false, false);
                appExcel.Visible = false;
                appExcel.Calculation = XlCalculation.xlCalculationManual;
                appExcel.DisplayAlerts = false;
                appExcel.DisplayFormulaBar = false;
                appExcel.EnableAutoComplete = false;
                appExcel.EnableSound = false;
                appWorkBook = appExcel.ActiveWorkbook;
                appWorkSheetLots = appWorkBook.Sheets[SHEETNAMELOT];
                UseRange = appWorkSheetLots.UsedRange;

                colTotal = UseRange.Columns.Count;
                int row = UseRange.Rows.Count;
                int rCount = row - 4;
                UseRange = appWorkSheetLots.get_Range("B4", String.Format("J{0}", row));
                object[,] arrLots = UseRange.Value as object[,];

                for (int r = 1; r <= rCount; r++)
                {
                    int iCol = 1;
                    string lotId = arrLots[r, iCol] + "";
                    iCol++;
                    string reason = arrLots[r, iCol] + "";
                    iCol++;
                    DateTime? requestOutDate = (DateTime?)arrLots[r, iCol];
                    iCol++;
                    string bu = arrLots[r, iCol] + "";
                    iCol++;
                    string owner = arrLots[r, iCol] + "";
                    iCol++;
                    string comment = arrLots[r, iCol] + "";
                    iCol++;
                    string platform = arrLots[r, iCol] + "";
                    iCol++;
                    string currentOperation = arrLots[r, iCol] + "";
                    iCol++;
                    string device = arrLots[r, iCol] + "";

                    if (!string.IsNullOrEmpty(lotId) &&
                        !string.IsNullOrEmpty(reason) &&
                        requestOutDate != null &&
                        !string.IsNullOrEmpty(bu) &&
                        !string.IsNullOrEmpty(owner) &&
                        !string.IsNullOrEmpty(currentOperation) &&
                        !string.IsNullOrEmpty(device))
                    {

                        LotExpediteDto result = new LotExpediteDto();
                        result.LotId = lotId;
                        result.Reason = reason;
                        result.RequestOutDate = requestOutDate.Value;
                        result.BUId = GetBuId(bu);
                        result.Owner = owner;
                        result.Comment = comment;
                        result.Platform = platform;
                        result.CurrentOperation = currentOperation;
                        result.Device = device;

                        lots.Add(result);
                    }
                    else//one or the more than nothing data
                    {
                        if (!string.IsNullOrEmpty(lotId) ||
                   !string.IsNullOrEmpty(reason) ||
                   requestOutDate != null ||
                   !string.IsNullOrEmpty(bu) ||
                   !string.IsNullOrEmpty(owner) ||
                   !string.IsNullOrEmpty(currentOperation) ||
                   !string.IsNullOrEmpty(device))
                        {
                            throw new Exception();
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                appWorkBook.Close(false, null, null);
                appExcel.Quit();

                ReleaseObject(appWorkSheetLots);
                ReleaseObject(appWorkSheetBu);
                ReleaseObject(appWorkBook);
                ReleaseObject(appExcel);
            }
        }

        private int GetBuId(string nameBu)
        {
            var result = 1;
            switch (nameBu)
            {
                case "MEM":
                    result = 1;
                    break;
                case "MXT":
                    result = 2;
                    break;
                case "MCU":
                    result = 3; break;
                case "APG":
                    result = 4;
                    break;
                case "ASIC":
                    result = 5;
                    break;
                case "RFA":
                    result = 6;
                    break;
                case "Foundry":
                    result = 7; break;
            }
            return result;
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public List<LotExpediteDto> Lots { get { return lots; } }
    }
}