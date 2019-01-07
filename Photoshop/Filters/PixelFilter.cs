using MyPhotoshop.Data;
using System;

namespace MyPhotoshop {
	public class PixelFilter<TParameters> : ParametrizedFilter<TParameters> where TParameters : IParameters, new() {
        private Func<Pixel, TParameters, Pixel> processor;

        public PixelFilter(string name, Func<Pixel, TParameters, Pixel> processor) : base(name) {
            this.processor = processor;
        }

		protected override Photo Process(Photo original, TParameters parameters) {
			var result = new Photo(original.width, original.height);

			for (int x = 0; x < result.width; x++) {
				for (int y = 0; y < result.height; y++) {
					result[x, y] = processor(original[x, y], parameters);
				}
			}
			return result;
		}
	}
}
