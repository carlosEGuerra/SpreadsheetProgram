using SSGui;
using System;
using System.Collections.Generic;
using System.IO;
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

        event Action UpdateCell;

        event Action CellClicked;

        event Action HelpEvent;

        event Action CheckChanged;

        event Action<TextWriter> SaveEvent;

        SpreadsheetPanel SP { get; set; }

        bool Changed { get; set; }

        string Title { set; }

        string HelpMessage { set; }

        string CellName { get; set; }

        string Message { set; }

        string Content { get; set; }

        string Value { get; set; }

        void DoClose();

        void OpenNew();
    }
}
