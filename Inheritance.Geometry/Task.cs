using System;

namespace Inheritance.Geometry {
	public abstract class Body {
		public abstract double GetVolume();
		public abstract void Accept(IVisitor visitor);
	}

	public class Ball : Body {
		public double Radius { get; set; }

		public override double GetVolume() => 4.0 * Math.PI * Math.Pow(Radius, 3) / 3;

		public override void Accept(IVisitor visitor) {
			visitor.Visit(this);
		}
	}

	public class Cube : Body {
		public double Size { get; set; }

		public override double GetVolume() => Math.Pow(Size, 3);

		public override void Accept(IVisitor visitor) {
			visitor.Visit(this);
		}
	}

	public class Cyllinder : Body {
		public double Height { get; set; }
		public double Radius { get; set; }

		public override double GetVolume() => Math.PI * Math.Pow(Radius, 2) * Height;

		public override void Accept(IVisitor visitor) {
			visitor.Visit(this);
		}
	}

	public interface IVisitor {
		void Visit(Ball ball);
		void Visit(Cube cube);
		void Visit(Cyllinder cyllinder);
	}

	public class SurfaceAreaVisitor : IVisitor {
		public double SurfaceArea { get; private set; }

		public void Visit(Ball ball) {
			SurfaceArea = 4 * Math.PI * Math.Pow(ball.Radius, 2);
		}

		public void Visit(Cube cube) {
			SurfaceArea = 6 * cube.Size * cube.Size;
		}

		public void Visit(Cyllinder cyllinder) {
			SurfaceArea = 2 * Math.PI * cyllinder.Radius * cyllinder.Height;
			SurfaceArea += 2 * Math.PI * Math.Pow(cyllinder.Radius, 2);
		}
	}

	public class DimensionsVisitor : IVisitor {
		public Dimensions Dimensions { get; private set; }

		public void Visit(Ball ball) {
			Dimensions = new Dimensions(2 * ball.Radius, 2 * ball.Radius);
		}

		public void Visit(Cube cube) {
			Dimensions = new Dimensions(cube.Size, cube.Size);
		}

		public void Visit(Cyllinder cyllinder) {
			Dimensions = new Dimensions(2 * cyllinder.Radius, cyllinder.Height);
		}
	}

	public class DimensionsCalculator {
		public Dimensions Calc(Ball ball) {
			return new Dimensions(2 * ball.Radius, 2 * ball.Radius);
		}

		public Dimensions Calc(Cube cube) {
			return new Dimensions(cube.Size, cube.Size);
		}

		public Dimensions Calc(Cyllinder cyllinder) {
			return new Dimensions(2 * cyllinder.Radius, cyllinder.Height);
		}
	}
}