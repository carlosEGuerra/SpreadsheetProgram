using System;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SSGui;
using Formulas;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controls the operation of an ISpreadsheedView.
    /// </summary>
    public class Controller
    {
        // The window being controlled
        private ISpreadsheetView window;

        // The model being used
        private Spreadsheet model;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            this.model = new Spreadsheet();
            window.FileChosenEvent += HandleFileChosen;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.UpdateCell += HandleCell;

        }

        /// <summary>
        /// The primary method that updates the cell if the value in the text box is updated.
        /// </summary>
        private void HandleCell(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
           
            //Convert row value to a cell name.
            string cellName = this.LocationToCellName(row, col);
            string content = window.Content; //Pull the current cell contents from the interface.

            try
            {
                //Modify the model cell contents.
                HashSet<string> list = new HashSet<string>(model.SetContentsOfCell(cellName, content));
            }
            catch (Exception e)
            {
                //CircularExecption
                if (e is CircularException)
                {
                    window.Message = "Circular Exception detected";
                    window.Value = "1";
                }
                //FormulaFormatError
                else if (e is FormulaFormatException)
                {
                    window.Message = "Sorry, but that formula is in the incorrect format!";
                    window.Value = "00";

                }
                //FormulaEvaluationException
                else if (e is FormulaEvaluationException)
                {
                    window.Message = "Formula Evaluation Exception Error Detected with new formula";
                    window.Value = "000";
                }
            }

            finally
            {
                object o = model.GetCellContents(cellName);
                window.Content = o.ToString();

                object val = model.GetCellValue(cellName);
                if (val is FormulaError)
                {
                    window.Value = "FORMULAERROR";
                }
                else
                {
                    window.Value = val.ToString();
                    window.Content = model.GetCellContents(cellName).ToString();
                }
            }
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            try
            {
                TextReader file = new StringReader(filename);
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }
        /// <summary>
        /// Take in the row and column of the value to convert it to a cell name
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private string LocationToCellName(int row, int col)
        {
            char colName = (char)(col + 65); //CELLS START INDEXING AT 0,0.
            return colName + (row + 1).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private int CellNameToLocation(string cellName)
        {
            int colLocation = cellName[0] - 65;//CELLS START INDEXING AT ZERO.
            return colLocation;
        }
    }
}
