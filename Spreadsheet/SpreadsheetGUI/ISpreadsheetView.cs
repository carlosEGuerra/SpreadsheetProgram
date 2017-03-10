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

        event Action<string> CountEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Action<SpreadsheetPanel> UpdateCell;

        string Title { set; }

        string Message { set; }

        string Content { get; set; }

        string Value { get; set; }

        void DoClose();

        void OpenNew();
    }
}
