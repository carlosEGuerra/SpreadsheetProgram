using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    static class Launch
    {
        static void Main()
        {
            //Citation: FileAnalyzers5 by Joe Zachary from CS 3500 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Get the application context and run one form inside it.
            var context = SpreadsheetApplicationContext.GetContext();
            SpreadsheetApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
