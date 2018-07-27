using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;

namespace Issue_Linker
{
    /// <summary>
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    internal sealed class CreateVisuals
    {
        /// <summary>
        /// The width of the square box.
        /// </summary>
        private const double AdornmentWidth = 30;

        /// <summary>
        /// The height of the square box.
        /// </summary>
        private const double AdornmentHeight = 30;

        private double x = 10;
        private double y = 10;
        /// <summary>
        /// Text view to add the adornment on.
        /// </summary>
        private readonly IWpfTextView view;

        /// <summary>
        /// Adornment image
        /// </summary>
        private List<Image> images = new List<Image>();

        /// <summary>
        /// The layer for the adornment.
        /// </summary>
        private readonly IAdornmentLayer adornmentLayer;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public List<Image> Images { get => images; set => images = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVisuals"/> class.
        /// Creates a square image and attaches an event handler to the layout changed event that
        /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
        public CreateVisuals(IWpfTextView view, double x, double y)
        {
            this.view = view ?? throw new ArgumentNullException("view");
            SetCoords(x, y);
            DrawingImage drawingImage = null;
            ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                drawingImage = await new ButtonShape().CreateGeometryAsync(10, 10);
            });
            this.Images.Add(new Image{Source = drawingImage});

            this.adornmentLayer = view.GetAdornmentLayer("CreateVisuals");
            this.view.LayoutChanged += this.OnSizeChanged;
        }

        /// <summary>
        /// Event handler for viewport layout changed event. Adds adornment at the top right corner of the viewport.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            // Clear the adornment layer of previous adornments
            this.adornmentLayer.RemoveAllAdornments();

            foreach (var image in Images)
            {
                // Place the image in the top right hand corner of the Viewport
                Canvas.SetLeft(image, this.view.ViewportRight - x - AdornmentWidth);
                Canvas.SetTop(image, this.view.ViewportTop + y);
                // Add the image to the adornment layer and make it relative to the viewport
                this.adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, image, null);
            }
            



        }

        internal void Redraw(double x, double y)
        {
            SetCoords(x, y);

            this.adornmentLayer.RemoveAllAdornments();

            foreach (var image in Images)
            {
                // Place the image in the top right hand corner of the Viewport
                Canvas.SetLeft(image, this.view.ViewportRight - x - AdornmentWidth);
                Canvas.SetTop(image, this.view.ViewportTop + y);
                this.adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, image, null);
            }


        }

        internal void SetCoords(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
