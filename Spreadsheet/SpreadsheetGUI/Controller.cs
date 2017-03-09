using System;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace SpreadsheetGUI
{
    /// <summary>
    /// 
    /// </summary>
    class Controller
    { // The window being controlled
        private ISpreadsheet window;

        // The model being used
        private Spreadsheet SSModel;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
      public Controller(ISpreadsheet window)
        {
            this.window = window;
            this.SSModel = new Spreadsheet();
            window.OpenFileEvent += OpenFile;
            window.CloseEvent += HandleClose;
            window.NewFileEvent += NewFile ;
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void OpenFile(String filename)
        {
            try
            {
                TextReader file = new StreamReader(filename);
                SSModel = new Spreadsheet(file, new Regex(""));
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles request to make a new spreadsheet.
        /// </summary>
        private void NewFile()
        {
            window.OpenNew();
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
