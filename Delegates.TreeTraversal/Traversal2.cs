using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal {
	public static class Traversal {
		private static IEnumerable<T> Get<T>(T data, Func<T, bool> predicate, Func<T, IList<T>> childs) {
			{
				if (data == null)
					yield break;

				if (predicate(data))
					yield return data;

				foreach (var child in childs(data))
				foreach (var item in Get(child, predicate, childs))
					yield return item;
			}
		}

		public static IEnumerable<int> GetBinaryTreeValues(BinaryTree<int> data) {
			return Get(data,
				d => true,
				d => new List<BinaryTree<int>>() { d.Left, d.Right }
			).Select(d => d.Value);
		}

		public static IEnumerable<Job> GetEndJobs(Job data) {
			return Get(data,
				d => d.Subjobs.Count == 0,
				d => d.Subjobs
			);
		}

		public static IEnumerable<Product> GetProducts(ProductCategory data) {
			return Get(data,
				d => true,
				d => d.Categories
			).SelectMany(d => d.Products);
		}
	}
}