using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Issue_Linker.Visuals;

namespace Issue_Linker.Core
{
    class LinkAdornment : Button
    {
        private Tag dataTag;
        private Rectangle rect;
        private List<Rectangle> labels = new List<Rectangle>();
        private TextBlock text;
        private Image image;
        public LinkAdornment(Tag dataTag)
        {
            this.dataTag = dataTag;

            rect = new Rectangle()
            {
                Stroke = Brushes.Black,
                Width = 20,
                Height = 10
            };

            foreach (Label label in dataTag.Link.Labels)
            {
                labels.Add(new Rectangle()
                {
                    Width = 20,
                    Height = 10
                });
            }

            Update(dataTag);

            Content = rect;
        }

        private Brush MakeBrush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

        internal void Update(Tag linkTag)
        {
            foreach (var label in linkTag.Link.Labels)
            {

            }
        }
    }
}
