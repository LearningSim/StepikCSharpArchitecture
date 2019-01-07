using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Robots
{
    [TestFixture]
    public class Architecture_Should
    {
        [Test]
        public void BeCorrectForShooter()
        {
            var robot = Robot.Create(new ShooterAI(), new Mover());
            var result = robot.Start(5);
            var dueResult = Enumerable.Range(1, 5).Select(z => string.Format("MOV {0}, {1}", z * 2, z * 3));
            CollectionAssert.AreEqual(dueResult, result);
        }

        [Test]
        public void BeCorrectForBuilder()
        {
            var robot = Robot.Create(new BuilderAI(), new Mover());
            var result = robot.Start(5).ToArray();
            var dueResult = Enumerable.Range(1, 5).Select(z => string.Format("MOV {0}, {0}", z));
            CollectionAssert.AreEqual(dueResult, result);
        }
    }
}
