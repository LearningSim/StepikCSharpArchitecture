using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPhotoshop.Data;
using System.Drawing;

namespace MyPhotoshop {
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters> where TParameters : IParameters, new() {
        private ITransformAlgorithm<TParameters> transformer;

        public TransformFilter(string name, ITransformAlgorithm<TParameters> transformer) : base(name) {
            this.transformer = transformer;
        }

        protected override Photo Process(Photo original, TParameters parameters) {
            var oldSize = new Size(original.width, original.height);
            var newSize = transformer.PrepareSize(oldSize, parameters);
            var result = new Photo(newSize.Width, newSize.Height);

            for (int x = 0; x < original.width; x++) {
                for (int y = 0; y < original.height; y++) {
                    var newPoint = transformer.TransformPoint(new Point(x, y));
                    result[newPoint.X, newPoint.Y] = original[x, y];
                }
            }
            return result;
        }
    }

    public class TransformFilter : TransformFilter<EmptyParameters> {
        public TransformFilter(string name, Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer) 
        : base(name, new FreeTransformer(sizeTransformer, pointTransformer)) {
        }
    }
}
