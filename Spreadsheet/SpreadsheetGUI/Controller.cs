using System;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SSGui;
using Formulas;
using System.Text.RegularExpressions;

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
        private void HandleCell()
        {
            int row, col;
            String value;
            window.SP.GetSelection(out col, out row);
            window.SP.GetValue(col, row, out value);
           
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

        /// <summary>
        /// Whenever the contents of a cell are changed, the values are stored to be updated to the GUI.
        /// </summary>
        private void HandleCellChanged()
        {
            int col;
            int row;
            //Get the current cell
            window.SP.GetSelection(out col, out row);

            window.Content = "";//If tbe cell is empty, the content box should also be empty.

            string val;
            if (window.SP.GetValue(col, row, out val))
            {
                    window.Content = "";//If tbe cell is empty, the content box should also be empty.
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleOpen()
        {
            //LoadFile will need a file to create a spreadsheet out of.
          //  LoadFile();
        }

        /// <summary>
        /// Helper method from adding items from the spreadsheet to the GUI.
        /// </summary>
        /// <param name="list"></param>
        private void AddAll(IEnumerable<string> list)
        {
            list = new List<string>(list);

            foreach (string cell in list)
            {
                int r, c;
                CellNameToLocation(cell, out r, out c);
                window.SP.SetValue(c, r - 1, model.GetCellValue(cell).ToString()); //WATCH THIS. MIGHT CAUSE ISSUES.
            }

        }

        /// <summary>
        ///  A method that converts the contents of an xml file to the spreadsheet.
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFile(string filepath)
        {
            TextReader reader = new StreamReader(filepath);
            model = new Spreadsheet(reader, new Regex(""));
            List<string> list = new List<string>(model.GetNamesOfAllNonemptyCells());

            //Convert each non-empty cell of the spreadsheet to the SpreadsheetGUI
            AddAll(list);
        }

        /// <summary>
        /// Saves file to a destination
        /// </summary>
        /// <param name="T"></param>
        private void SaveFile(TextWriter T)
        {
            if (model.Changed)
            {
                try
                {
                    model.Save(T);
                }
                catch
                {
                    window.Message = "Unable to save file.";
                }
            }
        }

        /// <summary>
        /// A help message to explain how to use the SpreadsheetGUI.
        /// </summary>
        private void DisplayHelp()
        {
            window.Message =
            "Welcome the the Ghetto Spreadsheet! \n " +
            "Sorry in advance. \n" +
            "\n\n" +
            "To Edit Cell Contents: \n" +
            "Click on the cell you would like to change." +
            "Edit its contents in the \"Contents\" box at the top of the page." +
            "\n\n" +
            "To close the program: File > Close \n" +
            "To open an existing spreadsheet: File > Open \n" +
            "To create a new Spreadsheet: File > New \n"

            ;

        }

    }
}
