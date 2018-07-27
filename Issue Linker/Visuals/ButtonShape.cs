using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Issue_Linker
{
    public class ButtonShape
    {
        public ButtonShape()
        {
        }

        public async Task<DrawingImage> CreateGeometryAsync(double radiusX, double radiusY)
        {
            // _____
            // |    )
            // -----
            DrawingGroup drawingGroup = new DrawingGroup();

            var blackBrush = new SolidColorBrush(Colors.Black);
            blackBrush.Freeze();

            var penBrush = new SolidColorBrush(Colors.Gray);
            penBrush.Freeze();

            var blackPen = new Pen(blackBrush, 0.5);
            blackPen.Freeze();

            var ellipseGeometry = new EllipseGeometry(new System.Windows.Point(), radiusX, radiusY);
            var ellipseDrawing = new GeometryDrawing(blackBrush, blackPen, ellipseGeometry);
            ellipseDrawing.Freeze();

            var rectangleGeometry = new RectangleGeometry(new System.Windows.Rect(0, -5d, 15d, radiusY*2));
            var rectangleDrawing = new GeometryDrawing(blackBrush, blackPen, rectangleGeometry);
            rectangleDrawing.Freeze();

            drawingGroup.Children.Add(ellipseDrawing);
            drawingGroup.Children.Add(rectangleDrawing);

            return new DrawingImage(drawingGroup);

        }
    }
}
