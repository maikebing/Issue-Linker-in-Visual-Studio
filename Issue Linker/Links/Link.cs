using Issue_Linker.Core;
using Issue_Linker.Visuals;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker
{
    public abstract class Link
    {
        private int tagNumber;


        private double x;
        private double y;
        private string state = string.Empty;
        private List<Label> labels;

        public int TagNumber { get => tagNumber; set => tagNumber = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public string State { get => state; set => state = value; }
        internal List<Label> Labels { get => labels; set => labels = value; }

        public Link(int tagNumber) { }
        public abstract Task<bool> CallAPIAsync();

    }
}
