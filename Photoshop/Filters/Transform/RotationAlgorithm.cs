using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyPhotoshop {
    class RotationAlgorithm : ITransformAlgorithm<RotationParameters> {
        public Size OriginalSize { get; private set; }
        public Size TransformedSize { get; private set; }
        public double Radians { get; private set; }

        public Size PrepareSize(Size size, RotationParameters parameters) {
            OriginalSize = size;
            Radians = parameters.Angle / 180.0 * Math.PI;

            TransformedSize = new Size(
                (int)(size.Height * Math.Abs(Math.Sin(Radians)) + size.Width * Math.Abs(Math.Cos(Radians))),
                (int)(size.Height * Math.Abs(Math.Cos(Radians)) + size.Width * Math.Abs(Math.Sin(Radians))) 
            );
            return TransformedSize;
        }

        public Point TransformPoint(Point point) {
            var newSize = TransformedSize;
            var relativeX = point.X - OriginalSize.Width / 2.0;
            var relativeY = point.Y - OriginalSize.Height / 2.0;
            return new Point(
                Restrict(Math.Round(newSize.Width / 2.0 + relativeX * Math.Cos(Radians) - relativeY * Math.Sin(Radians)), 0, newSize.Width - 1),
                Restrict(Math.Round(newSize.Height / 2.0 + relativeX * Math.Sin(Radians) + relativeY * Math.Cos(Radians)), 0, newSize.Height - 1)
            );
        }

        private int Restrict(double n, int min, int max) {
            return (int)(Math.Min(max, Math.Max(min, n)));
        }
    }
}
