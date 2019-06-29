namespace TodoApplication
{
	public class LimitedSizeStack<T>
	{
		public int Limit { get; }
		private StackItem<T> head;
		private StackItem<T> tail;

		public LimitedSizeStack(int limit)
		{
			Limit = limit;
		}

		public void Push(T item)
		{
			head = new StackItem<T>(item, head);
			if (Count == 0)
			{
				tail = head;
			}

			var second = head.Prev;
			if (second != null)
			{
				second.Next = head;
			}

			if (Count == Limit)
			{
				CutTail();
			}
			else if (Count < Limit)
			{
				Count++;
			}
		}

		private void CutTail()
		{
			tail = tail.Next;
			tail.Prev = null;
		}

		public T Pop()
		{
			if (Count == 0)
			{
				return default(T);
			}

			var item = head;
			if (Count == 1)
			{
				tail = null;
			}

			head = head.Prev;
			if (head != null)
			{
				head.Next = null;
			}

			Count--;

			return item.Value;
		}

		public int Count { get; private set; }
	}

	internal class StackItem<T>
	{
		public T Value { get; set; }
		public StackItem<T> Next { get; set; }
		public StackItem<T> Prev { get; set; }

		public StackItem(T value, StackItem<T> prev = null, StackItem<T> next = null)
		{
			Value = value;
			Next = next;
			Prev = prev;
		}
	}
}