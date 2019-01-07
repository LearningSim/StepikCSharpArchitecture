using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
	public class Traversal2 {
		static readonly List<Product> products = new List<Product>();
		static readonly List<Job> jobs = new List<Job>();
		static readonly List<int> values = new List<int>();
		internal static IEnumerable<Product> GetProducts(ProductCategory data) {
			return Get(
				products,
				prods => {
					products.AddRange(data.Products);
				},
				() => {
					foreach (var category in data.Categories) {
						GetProducts(category);
					}
				}
			);
		}

		internal static IEnumerable<Job> GetEndJobs(Job data) {
			return Get(
				jobs, 
				jbs => {
					if (data.Subjobs.Count == 0) {
						jbs.Add(data);
					}
				},
				() => {
					foreach (var job in data.Subjobs) {
						GetEndJobs(job);
					}
				}
			);
		}

		internal static IEnumerable<int> GetBinaryTreeValues(BinaryTree<int> data) {
			return Get(
				values,
				vals => {
					values.Add(data.Value);
				},
				() => {
					if (data.Left != null) {
						GetBinaryTreeValues(data.Left);
					}

					if (data.Right != null) {
						GetBinaryTreeValues(data.Right);
					}
				}
			);
		}

		internal static IEnumerable<T> Get<T>(List<T> list, Action<List<T>> addTo, Action traverse) {
			addTo(list);
			traverse();
			return list;
		}
	}
}
