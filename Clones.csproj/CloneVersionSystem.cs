using System;
using System.Collections.Generic;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private List<Clone> clones = new List<Clone>();
		private Dictionary<string, Func<int, string, string>> commands;

		public CloneVersionSystem()
		{
			commands = new Dictionary<string, Func<int, string, string>>
			{
				{"learn", Learn},
				{"rollback", Rollback},
				{"relearn", Relearn},
				{"clone", Clone},
				{"check", Check},
			};

			// default clone
			clones.Add(new Clone());
		}

		public string Execute(string query)
		{
			var args = query.Split();
			if (args.Length == 3)
			{
				return commands[args[0]](int.Parse(args[1]) - 1, args[2]);
			}

			return commands[args[0]](int.Parse(args[1]) - 1, null);
		}

		private string Learn(int cloneNumber, string program)
		{
			clones[cloneNumber].Learn(program);
			return null;
		}

		private string Rollback(int cloneNumber, string _)
		{
			clones[cloneNumber].Rollback();
			return null;
		}

		private string Relearn(int cloneNumber, string _)
		{
			clones[cloneNumber].Relearn();
			return null;
		}

		private string Clone(int cloneNumber, string _)
		{
			clones.Add(clones[cloneNumber].MakeClone());
			return null;
		}

		private string Check(int cloneNumber, string _)
		{
			return clones[cloneNumber].Programs.Peek();
		}
	}

	class Clone
	{
		public Stack<string> Programs { get; } = new Stack<string>(new[] {"basic"});
		public Stack<string> Rollbacks { get; } = new Stack<string>();

		public Clone(Stack<string> programs, Stack<string> rollbacks)
		{
			Programs = programs;
			Rollbacks = rollbacks;
		}

		public Clone()
		{
		}

		public void Learn(string program)
		{
			Programs.Push(program);
		}

		public void Rollback()
		{
			Rollbacks.Push(Programs.Pop());
		}

		public void Relearn()
		{
			Learn(Rollbacks.Pop());
		}

		public Clone MakeClone()
		{
			var programs = new Stack<string>(Programs.Head);
			var rollbacks = new Stack<string>(Rollbacks.Head);
			return new Clone(programs, rollbacks);
		}
	}

	class Node<T>
	{
		public T Value { get; set; }
		public Node<T> Next { get; set; }
	}

	class Stack<T>
	{
		public Node<T> Head { get; private set; }

		public Stack()
		{
		}

		public Stack(Node<T> head)
		{
			Head = head;
		}

		public Stack(IEnumerable<T> values)
		{
			foreach (var value in values)
			{
				Push(value);
			}
		}

		public void Push(T value)
		{
			Node<T> newHead = new Node<T>();
			newHead.Value = value;

			newHead.Next = Head;
			Head = newHead;
		}

		public T Pop()
		{
			var lastHead = Head;
			Head = Head.Next;
			return lastHead.Value;
		}

		public T Peek()
		{
			return Head.Value;
		}
	}
}