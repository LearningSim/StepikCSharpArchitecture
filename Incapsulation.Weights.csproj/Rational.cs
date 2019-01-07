using System;

namespace Incapsulation.RationalNumbers {
	public struct Rational {

		public Rational(int numerator) {
			this.numerator = numerator;
			denominator = 1;
		}

		public Rational(int numerator, int denominator) {
			this.numerator = numerator;
			this.denominator = denominator;
			if (numerator == 0) {
				this.denominator = 1;
				return;
			}

			if(denominator < 0) {
				numerator *= -1;
				denominator *= -1;
			}
			int gcd = GetGCD(numerator, denominator);
			numerator /= gcd;
			denominator /= gcd;
			this.numerator = numerator;
			this.denominator = denominator;
		}

		private static int GetGCD(int a, int b) {
			a = Math.Abs(a);
			b = Math.Abs(b);
			while (b > 0) {
				int rem = a % b;
				a = b;
				b = rem;
			}
			return a;
		}

		private static int GetLCM(int a, int b) => (a / GetGCD(a, b)) * b;

		private int denominator;
		public int Denominator => denominator;

		private int numerator;
		public int Numerator => numerator;

		public bool IsNan => denominator == 0;

		public static Rational operator +(Rational r1, Rational r2) {
			if(r1.IsNan || r2.IsNan) {
				return new Rational(1, 0);
			}

			int d = GetLCM(r1.denominator, r2.denominator);
			int summand1 = r1.numerator * (d / r1.denominator);
			int summand2 = r2.numerator * (d / r2.denominator);
			return new Rational(summand1 + summand2, d);
		}

		public static Rational operator -(Rational r1, Rational r2) {
			return r1 + new Rational(-r2.numerator, r2.denominator);
		}

		public static Rational operator *(Rational r1, Rational r2) {
			return new Rational(r1.numerator * r2.numerator, r1.denominator * r2.denominator);
		}

		public static Rational operator /(Rational r1, Rational r2) {
			if (r1.IsNan || r2.IsNan) {
				return new Rational(1, 0);
			}

			return new Rational(r1.numerator * r2.denominator, r1.denominator * r2.numerator);
		}

		public static implicit operator Rational(int val) {
			return new Rational(val);
		}

		public static explicit operator int(Rational val) {
			if(val.denominator != 1) {
				throw new ArgumentException();
			}
			return val.numerator / val.denominator;
		}

		public static implicit operator double(Rational val) {
			return val.numerator / (double)val.denominator;
		}
	}
}
