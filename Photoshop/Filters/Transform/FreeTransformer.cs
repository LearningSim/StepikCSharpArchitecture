using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyPhotoshop {
    public class FreeTransformer : ITransformAlgorithm<EmptyParameters> {
        public Func<Size, Size> sizeTransformer;
        public Func<Point, Size, Point> pointTransformer;
        private Size originalSize;
        public Size TransformedSize { get; private set; }

        public FreeTransformer(Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer) {
            this.sizeTransformer = sizeTransformer;
            this.pointTransformer = pointTransformer;
        }

        public Size PrepareSize(Size size, EmptyParameters parameters) {
            originalSize = size;
            TransformedSize = sizeTransformer(size);
            return TransformedSize;
        }
        public Point TransformPoint(Point point) {
            return pointTransformer(point, originalSize);
        }
    }
}
