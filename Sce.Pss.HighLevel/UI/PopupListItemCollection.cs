using System;
using System.Collections;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class PopupListItemCollection : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
	{
		private List<string> list;

		internal event EventHandler ItemChanged;

		public string this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
				if (this.ItemChanged != null)
				{
					this.ItemChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public PopupListItemCollection()
		{
			this.list = new List<string>();
		}

		public PopupListItemCollection(IEnumerable<string> items)
		{
			this.list = new List<string>(items);
		}

		[Obsolete("use PopupListItemCollection")]
		public static implicit operator PopupListItemCollection(List<string> list)
		{
			return new PopupListItemCollection(list);
		}

		public void AddRange(IEnumerable<string> items)
		{
			this.list.AddRange(items);
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public void InsertRange(int index, IEnumerable<string> items)
		{
			this.list.InsertRange(index, items);
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public int IndexOf(string item)
		{
			return this.list.IndexOf(item);
		}

		public void Insert(int index, string item)
		{
			this.list.Insert(index, item);
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			this.RemoveAt(index);
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public IEnumerator<string> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		public void Add(string item)
		{
			this.list.Add(item);
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			this.list.Clear();
			if (this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
		}

		public bool Contains(string item)
		{
			return this.list.Contains(item);
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		public bool Remove(string item)
		{
			bool flag = this.list.Remove(item);
			if (flag && this.ItemChanged != null)
			{
				this.ItemChanged.Invoke(this, EventArgs.Empty);
			}
			return flag;
		}
	}
}
