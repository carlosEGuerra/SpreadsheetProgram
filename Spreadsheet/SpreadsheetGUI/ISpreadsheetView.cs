using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface ISpreadsheetView
    {
        event Action<string> FileChosenEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Action OpenEvent;

        event Action UpdateCell;

        event Action CellClicked;

        string CellName { get; set; }

        SpreadsheetPanel SP { get; set; }

        string Title { set; }

        string Message { set; }

        string Content { get; set; }

        string Value { get; set; }

        void DoClose();

        void OpenNew();
    }
}
