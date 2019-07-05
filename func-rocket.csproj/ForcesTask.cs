using System;
using System.Drawing;
using System.Linq;

namespace func_rocket
{
    public class ForcesTask
    {
        private static readonly Vector oneVector = new Vector(1, 0);

        /// <summary>
        /// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
        /// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
        /// </summary>
        public static RocketForce GetThrustForce(double forceValue)
        {
            return r => oneVector.Rotate(r.Direction) * forceValue;
        }

        /// <summary>
        /// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
        /// </summary>
        public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
        {
            return r => gravity(spaceSize, r.Location);
        }

        /// <summary>
        /// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
        /// </summary>
        public static RocketForce Sum(params RocketForce[] forces)
        {
            return r => forces.Select(f => f(r)).Aggregate(Vector.Zero, (sum, f) => sum + f);
        }
    }
}