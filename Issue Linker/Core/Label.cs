using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Issue_Linker.Core
{
    class Label
    {
        string name = string.Empty;
        string color = string.Empty;
        Rectangle rect;
        public Label(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        public string Name { get => name; set => name = value; }
        public string Color { get => color; set => color = value; }
    }
}
