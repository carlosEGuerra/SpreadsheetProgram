using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of the SpreadsheetGUI.
    /// </summary>
    public interface ISpreadsheet
    {
        event Action CloseEvent;

        event Action<string> SaveFile;

        event Action NewFileEvent;

        event Action<string> OpenFileEvent;

        event Action SaveEvent;

        event Action HelpEvent;

        event Action WarningEvent;

        //To get and set the cell contents.
        object cellContents { get; set; }

        //For getting and setting the cell value.
        object cellVal { get; set; }

        //For access to the cell name for the display
        string cellName { get; }
        string Message { get; set; }

        void DoClose();

        /// <summary>
        /// Happens immediately when we open the spreadsheet. 
        /// </summary>
        void OpenNew();
    }
}
