using System;
using System.Collections.Generic;
using System.Linq;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();
        private static Vector holeLoc = new Vector(600, 200);
        private static Vector holeRocketLoc = new Vector(200, 500);

        private static Func<Level>[] levels =
        {
            () => CreateLevel("Zero", CreateRocket(), new Vector(600, 200), (size, v) => Vector.Zero),
            () => CreateLevel("Heavy", CreateRocket(), new Vector(600, 200), (size, v) => new Vector(0, .9)),
            () => CreateLevel("Up", CreateRocket(), new Vector(700, 500), (size, v) =>
            {
                var distToBottom = size.Height - v.Y;
                return new Vector(0, -300 / (distToBottom + 300.0));
            }),
            () => CreateLevel("WhiteHole", CreateRocket(holeRocketLoc), holeLoc, WhiteHoleGravity),
            () => CreateLevel("BlackHole", CreateRocket(holeRocketLoc), holeLoc, BlackHoleGravity),
            () => CreateLevel(
                "BlackAndWhite", CreateRocket(holeRocketLoc), holeLoc,
                (size, v) => (WhiteHoleGravity(size, v) + BlackHoleGravity(size, v)) / 2
            )
        };


        public static IEnumerable<Level> CreateLevels() => levels.Select(level => level());

        private static Level CreateLevel(string name, Rocket rocket, Vector target, Gravity gravity) =>
            new Level(name, rocket, target, gravity, standardPhysics);

        private static Rocket CreateRocket(Vector location = null, double direction = -0.5 * Math.PI, Vector velocity = null)
        {
            if (location == null)
            {
                location = new Vector(200, 500);
            }

            if (velocity == null)
            {
                velocity = Vector.Zero;
            }

            return new Rocket(location, velocity, direction);
        }

        private static Gravity WhiteHoleGravity => (size, v) =>
        {
            var distToTarget = (v - holeLoc).Length;
            var gravLen = 140 * distToTarget / (distToTarget * distToTarget + 1);
            return (v - holeLoc).Normalize() * gravLen;
        };

        private static Gravity BlackHoleGravity => (size, v) =>
        {
            var anomalyLoc = (holeRocketLoc + holeLoc) / 2;
            var distToAnomaly = (v - anomalyLoc).Length;
            var gravLen = 300 * distToAnomaly / (distToAnomaly * distToAnomaly + 1);
            return (anomalyLoc - v).Normalize() * gravLen;
        };
    }
}