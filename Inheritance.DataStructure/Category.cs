using System;

namespace Inheritance.DataStructure {
	class Category : IComparable {
		private MessageType type;
		private MessageTopic topic;
		private string product;

		public Category(string product, MessageType type, MessageTopic topic) {
			this.product = product;
			this.type = type;
			this.topic = topic;
		}

		public int CompareTo(object obj) {
			if (Equals(obj)) {
				return 0;
			}
			Category cat = obj as Category;
			if(cat == null) {
				return 1;
			}
			return this < cat ? -1 : 1;
		}

		public override bool Equals(object obj) {
			if (this == obj) {
				return true;
			}

			Category cat = obj as Category;
			if(cat == null) {
				return false;
			}

			return ToString() == cat.ToString();
		}

		public override string ToString() {
			return string.Format("{0}.{1}.{2}", product, type, topic);
		}

		public override int GetHashCode() {
			string enumsHash = ((int)type * 10).ToString() + (int)topic;
			return (product + enumsHash).GetHashCode();
		}

		public static bool operator<(Category a, Category b) {
			if(a.product.CompareTo(b.product) != 0) {
				return a.product.CompareTo(b.product) < 0;
			}
			if(a.type != b.type) {
				return a.type < b.type;
			}
			return a.topic < b.topic;
		}

		public static bool operator >(Category a, Category b) {
			return b < a;
		}

		public static bool operator <=(Category a, Category b) {
			return a.Equals(b) || a < b;
		}

		public static bool operator >=(Category a, Category b) {
			return a.Equals(b) || a > b;
		}
	}
}
