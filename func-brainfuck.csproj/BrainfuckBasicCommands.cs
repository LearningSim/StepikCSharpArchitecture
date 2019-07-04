using System;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand(',', b => b.SetCell(read()));
			vm.RegisterCommand('.', b => write(Convert.ToChar(b.GetCell())));
			vm.RegisterCommand('+', b => b.SetCell(b.GetCell() + 1));
			vm.RegisterCommand('-', b => b.SetCell(b.GetCell() - 1));
			vm.RegisterCommand('>', b => b.ShiftMemory(1));
			vm.RegisterCommand('<', b => b.ShiftMemory(-1));

			var syms = Enumerable.Range('A', 26).Concat(Enumerable.Range('a', 26)).Concat(Enumerable.Range('0', 10))
				.Select(Convert.ToChar).ToList();
			foreach (var sym in syms)
			{
				vm.RegisterCommand(sym, b => b.SetCell(sym));
			}
		}
	}

	static partial class VirtualMachineExtensions
	{
		public static void SetCell(this IVirtualMachine vm, int val)
		{
			vm.Memory[vm.MemoryPointer] = (byte) ((val + 256) % 256);
		}

		public static byte GetCell(this IVirtualMachine vm)
		{
			return vm.Memory[vm.MemoryPointer];
		}

		public static int ShiftMemory(this IVirtualMachine vm, int shift)
		{
			return vm.MemoryPointer = (vm.MemoryPointer + shift + vm.Memory.Length) % vm.Memory.Length;
		}
	}
}