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

        private IWpfTextView view;

        private List<CreateVisuals> visuals;

        private double x;
        private double y;

        public int TagNumber { get => tagNumber; set => tagNumber = value; }
        public IWpfTextView View { get => view; set => view = value; }
        internal List<CreateVisuals> Visuals { get => visuals; set => visuals = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Link(int tagNumber, IWpfTextView view) { }
        public abstract Task<bool> CallAPIAsync();
        public abstract void CreateVisuals(double x, double y);
        public abstract void Redraw(double x, double y);

    }
}
