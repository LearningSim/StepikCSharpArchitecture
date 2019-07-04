using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var pairs = vm.FindLoopPairs();

			vm.RegisterCommand('[', b =>
			{
				if (b.GetCell() == 0)
				{
					b.InstructionPointer = pairs[b.InstructionPointer];
				}
			});
			vm.RegisterCommand(']', b =>
			{
				if (b.GetCell() != 0)
				{
					b.InstructionPointer = pairs[b.InstructionPointer];
				}
			});
		}
	}

	static partial class VirtualMachineExtensions
	{
		public static Dictionary<int, int> FindLoopPairs(this IVirtualMachine vm)
		{
			var pairs = new Dictionary<int, int>();
			var opens = new Stack<int>();
			for (int i = 0; i < vm.Instructions.Length; i++)
			{
				var instruction = vm.Instructions[i];
				if (instruction == '[')
				{
					opens.Push(i);
				}
				else if (instruction == ']')
				{
					var open = opens.Pop();
					pairs[open] = i;
					pairs[i] = open;
				}
			}

			return pairs;
		}
	}
}