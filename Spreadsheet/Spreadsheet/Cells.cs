using System;
using Dependencies;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    /// <summary>
    /// A cell class to represent cells of a spreadsheet.
    /// /summary>
    public class Cells
    {
        private Dictionary<string, object[]> Sheet; 
        /// <summary>
        /// Zero constructor for an empty cell.
        /// </summary>
        public Cells()
        {
            Sheet = new Dictionary<string, object[]>();
        
        }

        /// <summary>
        /// Assign contents to a cell. Does nothing if the cell name is null.
        /// First index of the Value denotes the contents of the cell. 
        /// Second index of Value denotes the value of the cell (FormulaError, double, string).
        /// <param name="c"></param>
        /// <param name="s"></param>
        ///<param name="v"></param>
        /// </summary>
        public void SetCell(string s, object c, object v)
        {
            if(s == null)
            {
                return;
            }

            if (!Sheet.ContainsKey(s)) //If we've not explicitly added the cell to our set.
            {
                Sheet.Add(s, new object[2]);
                return;
            }

            Sheet[s][0] = c;//Set contents of cell.
            Sheet[s][1] = v;//Set the value of the cell. 
            return;
        }

        /// <summary>
        /// Returns the entire contents of the represented spreadsheet.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object[]> GetSheet()
        {
            return this.Sheet;
        }
    }
}
