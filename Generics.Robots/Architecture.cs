using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots {
    public interface RobotAI<TCommand> {
        TCommand GetCommand();
    }

    public class ShooterAI : RobotAI<ShooterCommand> {
        int counter = 1;

        public ShooterCommand GetCommand() {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : RobotAI<BuilderCommand> {
        int counter = 1;
        public BuilderCommand GetCommand() {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface Device<in TCommand> {
         string ExecuteCommand(TCommand command);
    }

    public class Mover : Device<IMoveCommand> {
        public string ExecuteCommand(IMoveCommand command) {
            if (command == null)
                throw new ArgumentException();
            return string.Format("MOV {0}, {1}", command.Destination.X, command.Destination.Y);
        }
    }

    public class Robot<TCommand> {
        RobotAI<TCommand> ai;
        Device<TCommand> device;

        public Robot(RobotAI<TCommand> ai, Device<TCommand> executor) {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps) {
            for (int i = 0; i < steps; i++) {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }

        }
    }

    public class Robot {
        public static Robot<TCommand> Create<TCommand>(RobotAI<TCommand> ai, Device<TCommand> executor) {
            return new Robot<TCommand>(ai, executor);
        }
    }
}
