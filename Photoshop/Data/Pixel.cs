using System;

namespace MyPhotoshop.Data {
	public struct Pixel {
		public Pixel(double r, double g, double b) {
			this.r = this.g = this.b = 0;
			R = r;
			G = g;
			B = b;
		}

		public static Pixel operator *(Pixel px, double multiplier) {
			return new Pixel(
				Trim(px.r * multiplier),
				Trim(px.g * multiplier),
				Trim(px.b * multiplier)
			);
		}

		public static Pixel operator *(double multiplier, Pixel px) {
			return px * multiplier;
		}

		private double r;
		public double R {
			get { return r; }
			set {
				r = Check(value);
			}
		}

		private double g;
		public double G {
			get { return g; }
			set {
				g = Check(value);
			}
		}

		private double b;
		public double B {
			get { return b; }
			set {
				b = Check(value);
			}
		}

		public static double Trim(double value) {
			value = Math.Max(value, 0);
			value = Math.Min(1, value);
			return value;
		}

		private double Check(double value) {
			if (value < 0 || value > 1) {
				throw new ArgumentException();
			}
			return value;
		}
	}
}
