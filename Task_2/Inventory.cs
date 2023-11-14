using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Inventory
    {
        public List<Tool> Tools { get; set; }
        public Inventory() {
            Random rnd = new Random();
            Tools = new List<Tool>();

            for (int i = 0; i < 100000; i++)
            {
                Tool tool = new Tool(
                    rnd.Next(0,100),
                    rnd.Next(0,int.MaxValue),
                    "description"
                    );

                Tools.Add(tool);
            }
        
        }

    }
}
