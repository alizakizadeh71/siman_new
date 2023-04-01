﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace OpenXml
{
    public class XLSWorker
    {
        public string WorkBookPath { get; set; }

        public XLWorkbook WorkBook { get; set; }
        public IXLWorksheets WorkSheets { get; set; }

        public IXLWorksheet CurrentSheet { get; set; }
        private int headerRowNum;
        public int HeaderRowNum
        {
            get { return headerRowNum; }
            set
            {
                //this.CurrentSheet.SheetView.FreezeRows(value);
                headerRowNum = value;
            }
        }

        #region Constructors

        public XLSWorker()
        {
            this.WorkBook = new XLWorkbook();

            //this.WorkBook.Worksheets.Add("Sheet1");
            this.WorkSheets = this.WorkBook.Worksheets;
            this.CurrentSheet = this.WorkSheets.FirstOrDefault();
        }

        /// <summary>
        /// Open an Existing xls file for Reading and Modfification
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XLSWorker Open(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File Not Found", path);
            }

            var xWorker = new XLSWorker
                              {
                                  WorkBookPath = path,
                                  WorkBook = new XLWorkbook(path)
                              };
            xWorker.WorkSheets = xWorker.WorkBook.Worksheets;
            xWorker.CurrentSheet = xWorker.WorkSheets.FirstOrDefault();

            return xWorker;
        }

        /// <summary>
        /// Creates a new Excel Workbook
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XLSWorker Create(string path)
        {
            return Create(path, true);
        }

        /// <summary>
        /// Creates a new Excel Workbook - Overwrites Existing File
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>        
        public static XLSWorker Create(string path, bool overwrite)
        {
            if (File.Exists(path))
            {
                if (overwrite)
                {
                    File.Delete(path);
                }
                else
                {
                    throw new Exception("File Already Exists : " + path);
                }
            }

            var xWorker = new XLSWorker()
            {
                WorkBookPath = path,
                WorkBook = new XLWorkbook()
            };

            xWorker.WorkBook.Worksheets.Add("Sheet1");

            xWorker.WorkSheets = xWorker.WorkBook.Worksheets;
            xWorker.CurrentSheet = xWorker.WorkSheets.FirstOrDefault();

            return xWorker;
        }

        /// <summary>
        /// Checks to see if document exists - creates if not - opens if it does exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XLSWorker CreateOrOpen(string path)
        {
            if(File.Exists(path))
            {
                return Open(path);
            }
            else
            {
                return Create(path);
            }
        }

        #endregion

        #region Set Active Sheet

        public void SetActiveSheet(int sheetNum, int headerRowNum = 1, bool noHeaderRow = false)
        {
            this.SetActiveSheet(this.WorkSheets.ElementAt(sheetNum - 1), headerRowNum, noHeaderRow);
        }
        public void SetActiveSheet(string sheetName, int headerRowNum = 1, bool noHeaderRow = false)
        {
            this.SetActiveSheet(this.WorkSheets.FirstOrDefault(x => x.Name == sheetName), headerRowNum, noHeaderRow);
        }
        public void SetActiveSheet(IXLWorksheet sheet, int headerRowNum = 1, bool noHeaderRow = false)
        {
            this.CurrentSheet = sheet;
            this.HeaderRowNum = headerRowNum;
            this.CurrentSheet.ShowRowColHeaders = !noHeaderRow;
            this._columns = null;
            this._rows = null;
        }

        #endregion

        #region Add / Remove Sheet
        public void AddNewSheet(string sheetName)
        {
            this.AddNewSheet(sheetName, this.WorkSheets.Count() + 1);
        }
        public void AddNewSheet(string sheetName, int position)
        {
            var xSheet = this.WorkSheets.Add(sheetName);
            xSheet.Position = position;
            this.SetActiveSheet(sheetName);
        }

        #endregion

        #region Styles

        private XLSStyle _headerStyle;
        public XLSStyle HeaderStyle
        {
            get
            {
                if (_headerStyle == null)
                {
                    _headerStyle = new XLSStyle
                    {
                        BackgroundColor = XLColor.DarkSlateBlue,
                        HorizintalAlign = XLAlignmentHorizontalValues.Center,
                        FontColor = XLColor.White,
                        Bold = true
                    };
                }
                return _headerStyle;
            }
            set { _headerStyle = value; }
        }

        private XLSStyle _rowStyle;
        public XLSStyle RowStyle
        {
            get
            {
                if(_rowStyle == null)
                {
                    _rowStyle = new XLSStyle
                    {
                        BackgroundColor = XLColor.Ivory,
                        FontColor = XLColor.Black
                    };                    
                }
                return _rowStyle;
            }
            set { _rowStyle = value; }
        }

        private XLSStyle _altRowStyle;
        public XLSStyle AltRowStyle
        {
            get
            {
                if (_altRowStyle == null)
                {
                    _altRowStyle = new XLSStyle
                    {
                        BackgroundColor = XLColor.LightBlue,
                        FontColor = XLColor.Black
                    };
                }
                return _altRowStyle;
            }
            set { _altRowStyle = value; }
        }

        #endregion

        #region Header / Columns

        private List<string> _columns;
        public List<string> Columns
        {
            get
            {
                if (this._columns == null)
                {
                    this._columns = new List<string>();
                    if (HeaderRowNum > 0)
                    {
                        var header_row = this.CurrentSheet.Row(this.HeaderRowNum);

                        foreach (var cell in header_row.CellsUsed())
                        {
                            this._columns.Add(cell.GetValue<string>());
                        }
                    }
                }
                return this._columns;
            }
        }

        /// <summary>
        /// Write a New Header Row - Sets the Header Row of the current sheet to the row specified
        /// </summary>
        public void AddHeaderRow(List<string> colNames, int rowNum, List<string> resourcesName)
        {
            var hRow = this.InsertNewRow(rowNum);
            for (int i = 0; i < colNames.Count(); i++)
            {
                IXLCell xlCell = hRow[i + 1];
				xlCell.Value = resourcesName==null?colNames[i]:resourcesName[i];
                this.HeaderStyle.ApplyTo(xlCell.Style);
            }

            this.HeaderRowNum = rowNum;
            this._columns = colNames;
        }

        #endregion

        #region Rows

        private List<XLSRow> _rows;
        public List<XLSRow> Rows
        {
            get
            {
                if (this._rows != null) return this._rows;

                this._rows = this.CurrentSheet.RowsUsed()
                    .Skip(this.HeaderRowNum)
                    .Select(x=> new XLSRow(this.Columns, x))
                    .ToList();

                return this._rows;
            }
            set { this._rows = value; }
        }

        public XLSRow AddNewRow()
        {
            int lastRow = (this.CurrentSheet.LastRowUsed() != null)
                ? this.CurrentSheet.LastRowUsed().RowNumber()
                : 0;
            XLSRow xRow = this.InsertNewRow(lastRow + 1);


            //Style the new row
            for (int i = 1; i <= Columns.Count(); i++)
            {
                if (lastRow % 2 == 0)
                {
                    RowStyle.ApplyTo(xRow[i].Style);
                }
                else
                {
                    AltRowStyle.ApplyTo(xRow[i].Style);
                }

                xRow[i].Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            return xRow;
        }

        public XLSRow InsertNewRow(int rowNumber)
        {
            var xRow = this.CurrentSheet.Row(rowNumber).InsertRowsAbove(1).FirstOrDefault();
            var xlsRow = new XLSRow(this.Columns, xRow);
            this.Rows.Add(xlsRow);
            return xlsRow;            
        }
        
        /// <summary>
        /// Filter Row Collection on a specific Column
        /// </summary>
        public void ApplyFilter(string columnName, string filterString)
        {
            this.Rows = this.Rows.Where(x => x[columnName].GetValue<string>().Contains(filterString)).ToList();
        }

        #endregion   
 
        #region Writing Data

        public void WriteToCell(string CellID, object Value)
        {
            this.CurrentSheet.Cell(CellID).Value = Value;
        }

        #endregion

        #region Save And Close

        public void Save()
        {
            this.SaveAs(this.WorkBookPath);
        }

        public void SaveAs(string path)
        {
           // this.WorkSheets.ForEach(x => x.Rows().AdjustToContents());
           // this.WorkSheets.ForEach(x => x.Columns().AdjustToContents());

            this.WorkBook.SaveAs(path);
            this.WorkBookPath = path;
        }

        public Stream SaveToMemoryStream()
        {
            Stream outPutStream = new MemoryStream();
            this.WorkBook.SaveAs(outPutStream);
            return outPutStream;
        }

        public void WriteToHttpResponse(string fileName)
        {
            //TODO: Make this not use a real file!  -- Looser.
            var tempDir = HttpContext.Current.Server.MapPath("/temp/");
            this.SaveAs(tempDir + fileName);
          //  HttpContext.Current.Response.ContentType = "application/ms-excel";
           // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            //HttpContext.Current.Response.WriteFile(tempDir + fileName);
        }

        #endregion
    }
}
