using System;
using Dependencies;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;

namespace SS
{
    /// <summary>
    /// A cell class to represent cells of a spreadsheet.
    /// </summary>
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
            }
            //If the object is a double
            if(c is double)
            {
                Sheet[s][0] = (double) c;//Set contents of cell.

            }
            if(v is double)
            {
                Sheet[s][1] = (double) v;//Set the value of the cell. 
            }
            if(c is Formula)
            {
                Sheet[s][0] = (Formula) c;//Set contents of cell.
            }
            if (v is FormulaError)
            {
                Sheet[s][1] = (FormulaError) v;//Set value of cell.
            }
            if (c is string)
            {
                Sheet[s][0] = (string) c;//Set contents of cell.
            }
            if (c is string)
            {
                Sheet[s][1] = (string) v;//Set the value of the cell. 
            }
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
