using System.Collections.Generic;

namespace TodoApplication
{
	public class ListModel<TItem>
	{
		public List<TItem> Items { get; }
		public int Limit;
		private readonly LimitedSizeStack<Command<TItem>> commands;

		public ListModel(int limit)
		{
			Items = new List<TItem>();
			Limit = limit;
			commands = new LimitedSizeStack<Command<TItem>>(limit);
		}

		public void AddItem(TItem item)
		{
			Items.Add(item);
			commands.Push(new AddCommand<TItem>(Items));
		}

		public void RemoveItem(int index)
		{
			commands.Push(new RemoveCommand<TItem>(Items, index, Items[index]));
			Items.RemoveAt(index);
		}

		public bool CanUndo()
		{
			return commands.Count > 0;
		}

		public void Undo()
		{
			commands.Pop().Undo();
		}
	}

	abstract class Command<T>
	{
		protected readonly List<T> items;

		protected Command(List<T> items)
		{
			this.items = items;
		}

		public abstract void Undo();
	}

	class AddCommand<T> : Command<T>
	{
		public AddCommand(List<T> items) : base(items)
		{
		}

		public override void Undo()
		{
			items.RemoveAt(items.Count - 1);
		}
	}

	class RemoveCommand<T> : Command<T>
	{
		private readonly int index;
		private readonly T item;

		public RemoveCommand(List<T> items, int index, T item) : base(items)
		{
			this.index = index;
			this.item = item;
		}

		public override void Undo()
		{
			items.Insert(index, item);
		}
	}
}