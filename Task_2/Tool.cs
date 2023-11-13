using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Tool
    {
        public int Type { get; set; }
        public int Barcode { get; set; }
        public string Description { get; set; }

        public Tool(int type, int barcode, string description)
        {
            Type = type;
            Barcode = barcode;
            Description = description;
        }
    }
}
