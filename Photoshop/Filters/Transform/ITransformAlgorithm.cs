using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyPhotoshop {
    public interface ITransformAlgorithm<TParameters> where TParameters : IParameters, new() {
        Size TransformedSize { get;}
        Size PrepareSize(Size size, TParameters parameters);
        Point TransformPoint(Point point);
    }
}
