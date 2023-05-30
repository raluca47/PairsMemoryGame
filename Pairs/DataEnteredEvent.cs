using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs
{
    public class DataEnteredEvent : EventArgs
    {
        public int rows { get; set; }
        public int columns { get; set; }

        public DataEnteredEvent(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
        }   
    }
}
