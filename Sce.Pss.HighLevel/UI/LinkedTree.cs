using System;

namespace Sce.Pss.HighLevel.UI
{
	internal class LinkedTree<T>
	{
		private T value;

		private LinkedTree<T> parent;

		private LinkedTree<T> lastChild;

		private LinkedTree<T> previous;

		private LinkedTree<T> next;

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public LinkedTree<T> Parent
		{
			get
			{
				return this.parent;
			}
		}

		public LinkedTree<T> PreviousAsList
		{
			get
			{
				return this.previous;
			}
		}

		public LinkedTree<T> NextAsList
		{
			get
			{
				return this.next;
			}
		}

		public LinkedTree<T> PreviousSibling
		{
			get
			{
				if (this.previous != this.parent)
				{
					LinkedTree<T> linkedTree = this.previous;
					while (linkedTree.parent != this.parent)
					{
						linkedTree = linkedTree.parent;
					}
					return linkedTree;
				}
				return null;
			}
		}

		public LinkedTree<T> NextSibling
		{
			get
			{
				LinkedTree<T> lastDescendant = this.LastDescendant;
				LinkedTree<T> linkedTree = lastDescendant.next;
				if (linkedTree == null || linkedTree.parent != this.parent)
				{
					return null;
				}
				return linkedTree;
			}
		}

		public LinkedTree<T> FirstChild
		{
			get
			{
				if (this.lastChild == null)
				{
					return null;
				}
				return this.next;
			}
		}

		public LinkedTree<T> LastChild
		{
			get
			{
				return this.lastChild;
			}
		}

		public LinkedTree<T> LastDescendant
		{
			get
			{
				LinkedTree<T> linkedTree = this;
				while (linkedTree.lastChild != null)
				{
					linkedTree = linkedTree.lastChild;
				}
				return linkedTree;
			}
		}

		public LinkedTree(T value)
		{
			this.value = value;
			this.parent = null;
			this.lastChild = null;
			this.previous = (this.next = null);
		}

		public void AddChildFirst(LinkedTree<T> child)
		{
			for (LinkedTree<T> linkedTree = this.parent; linkedTree != null; linkedTree = linkedTree.parent)
			{
			}
			child.RemoveChild();
			LinkedTree<T> lastDescendant = child.LastDescendant;
			child.parent = this;
			child.previous = this;
			lastDescendant.next = this.next;
			child.previous.next = child;
			if (lastDescendant.next != null)
			{
				lastDescendant.next.previous = lastDescendant;
			}
			if (this.lastChild == null)
			{
				this.lastChild = child;
			}
		}

		public void AddChildLast(LinkedTree<T> child)
		{
			for (LinkedTree<T> linkedTree = this.parent; linkedTree != null; linkedTree = linkedTree.parent)
			{
			}
			child.RemoveChild();
			LinkedTree<T> lastDescendant = this.LastDescendant;
			LinkedTree<T> lastDescendant2 = child.LastDescendant;
			child.parent = this;
			child.previous = lastDescendant;
			lastDescendant2.next = lastDescendant.next;
			child.previous.next = child;
			if (lastDescendant2.next != null)
			{
				lastDescendant2.next.previous = lastDescendant2;
			}
			this.lastChild = child;
		}

		public void InsertChildBefore(LinkedTree<T> tree)
		{
			for (LinkedTree<T> linkedTree = tree.parent; linkedTree != null; linkedTree = linkedTree.parent)
			{
			}
			this.RemoveChild();
			LinkedTree<T> lastDescendant = this.LastDescendant;
			this.parent = tree.parent;
			this.previous = tree.previous;
			lastDescendant.next = tree;
			this.previous.next = this;
			lastDescendant.next.previous = lastDescendant;
		}

		public void InsertChildAfter(LinkedTree<T> tree)
		{
			for (LinkedTree<T> linkedTree = tree.parent; linkedTree != null; linkedTree = linkedTree.parent)
			{
			}
			this.RemoveChild();
			LinkedTree<T> lastDescendant = tree.LastDescendant;
			LinkedTree<T> lastDescendant2 = this.LastDescendant;
			this.parent = tree.parent;
			this.previous = lastDescendant;
			lastDescendant2.next = lastDescendant.next;
			this.previous.next = this;
			if (lastDescendant2.next != null)
			{
				lastDescendant2.next.previous = lastDescendant2;
			}
			if (this.parent.lastChild == tree)
			{
				this.parent.lastChild = this;
			}
		}

		public void RemoveChild()
		{
			if (this.parent != null)
			{
				LinkedTree<T> lastDescendant = this.LastDescendant;
				if (this.parent.lastChild == this)
				{
					this.parent.lastChild = this.PreviousSibling;
				}
				this.previous.next = lastDescendant.next;
				if (lastDescendant.next != null)
				{
					lastDescendant.next.previous = this.previous;
				}
				this.parent = null;
				this.previous = (lastDescendant.next = null);
			}
		}

		public void Clear()
		{
			while (this.lastChild != null)
			{
				this.FirstChild.RemoveChild();
			}
		}
	}
}
