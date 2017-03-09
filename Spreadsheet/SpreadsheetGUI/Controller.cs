using System;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SSGui;

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
        /// The primary method that updates the cell if the value is clicked.
        /// </summary>
        private void HandleCell(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            //string cellName;//Panel coordinates

            string content = window.Content; //Pull the current cell contents from the interface.
            //model.SetContentsOfCell(cellName, content);//Modify the model cell contents.

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

    }
}
