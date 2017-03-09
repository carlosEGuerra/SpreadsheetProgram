using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// The View where events are fired.
    /// </summary>
    public partial class SpreadsheetWindow : Form, ISpreadsheet
    {

        public event Action CloseEvent;
        public event Action HelpEvent;
        public event Action NewFileEvent;
        public event Action<string> OpenFileEvent;
        public event Action SaveEvent;  
        public event Action WarningEvent;
        public event Action<string> SaveFile;

        public SpreadsheetWindow()
        {
            InitializeComponent();
        }

        public object cellContents
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public object cellVal
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        string ISpreadsheet.cellName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Message
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
           SpreadsheetApplicationContext.GetContext().RunNew();
        }

        
    }
}
