using System;

namespace func_rocket {
	public class ControlTask
	{
		public static Vector GuessPoint = new Vector(0, 0);
		public static Turn ControlRocket(Rocket rocket, Vector target) {
			var p = GetGuessedPoint(rocket);
			var guessAngle = (p - rocket.Location).Angle;
			var targetAngle = (target - rocket.Location).Angle;
			if (Math.Abs(targetAngle - guessAngle) < 1 * Math.PI / 180)
			{
				return Turn.None;
			}

			return targetAngle > guessAngle ? Turn.Right : Turn.Left;
		}

		private static Vector GetGuessedPoint(Rocket rocket) {
			var dt = .3;
			var p = rocket.Location;
			var f = ForcesTask.GetThrustForce(1)(rocket);
			while ((p - rocket.Location).Length < 400) {
				var v = rocket.Velocity + f * dt;
				p += v * dt;
			}

			GuessPoint = p;

			return p;
		}
	}
}