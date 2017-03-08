using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// 
    /// </summary>
    class Controller
    { // The window being controlled
        private I  window;

        // The model being used
        private Model model;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(IAnalysisView window)
        {
            this.window = window;
            this.model = new Model();
            window.FileChosenEvent += HandleFileChosen;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.CountEvent += HandleCount;
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            try
            {
                model.ReadFile(filename);
                window.CharCount = model.CountChars();
                window.WordCount = model.CountWords();
                window.LineCount = model.CountLines();
                window.SubstringCount = 0;
                window.SearchString = "";
                window.Title = filename;
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
