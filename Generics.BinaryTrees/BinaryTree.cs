using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees {
    class BinaryTree<T> : IEnumerable<T> {
        public T Value { get; private set; }
        public BinaryTree<T> Left { get; private set; }
        public BinaryTree<T> Right { get; private set; }
        private bool isEmpty = true;

        public BinaryTree() {
        }

        public BinaryTree(T value) {
            Value = value;
            isEmpty = false;
        }

        public void Add(T e) {
            if (isEmpty) {
                Value = e;
            }
            else if (Comparer<T>.Default.Compare(e, Value) <= 0) {
                if (Left == null) {
                    Left = new BinaryTree<T>(e);
                }
                else {
                    Left.Add(e);
                }
            }
            else if (Comparer<T>.Default.Compare(e, Value) > 0) {
                if (Right == null) {
                    Right = new BinaryTree<T>(e);
                }
                else {
                    Right.Add(e);
                }
            }
            isEmpty = false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator() {
            return GetEnumerable().GetEnumerator();
        }

        public IEnumerable<T> GetEnumerable() {
            return GetEnumerable(this);
        }

        List<T> elems = new List<T>();
        public IEnumerable<T> GetEnumerable(BinaryTree<T> node) {
            if (isEmpty) {
                return new T[0];
            }
            if (node.Left != null) {
                GetEnumerable(node.Left);
            }
            elems.Add(node.Value);
            if (node.Right != null) {
                GetEnumerable(node.Right);
            }
            return elems;
        }
    }

    class BinaryTree {
        public static BinaryTree<T> Create<T>(params T[] elems){
            var tree = new BinaryTree<T>();
            foreach (var e in elems) {
                tree.Add(e);
            }
            return tree;
        }
    }
}
