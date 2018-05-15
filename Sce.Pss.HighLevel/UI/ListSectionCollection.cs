using System;
using System.Collections;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class ListSectionCollection : IList<ListSection>, ICollection<ListSection>, IEnumerable<ListSection>, IEnumerable
	{
		private List<ListSection> items;

		public event EventHandler<EventArgs> ItemsChanged;

		public int AllItemCount
		{
			get
			{
				int num = 0;
				using (List<ListSection>.Enumerator enumerator = this.items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ListSection current = enumerator.Current;
						num += current.ItemCount;
					}
				}
				return num;
			}
		}

		public ListSection this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				this.items[index].ItemChanged += new EventHandler<EventArgs>(this.SectionItemChanged);
				this.items[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public ListSectionCollection()
		{
			this.items = new List<ListSection>();
		}

		public int IndexOf(ListSection section)
		{
			return this.items.IndexOf(section);
		}

		public void Insert(int index, ListSection section)
		{
			section.ItemChanged += new EventHandler<EventArgs>(this.SectionItemChanged);
			this.items.Insert(index, section);
			this.SectionItemChanged(this, EventArgs.Empty);
		}

		public void RemoveAt(int index)
		{
			this.items.RemoveAt(index);
			this.SectionItemChanged(this, EventArgs.Empty);
		}

		public void Add(ListSection section)
		{
			section.ItemChanged += new EventHandler<EventArgs>(this.SectionItemChanged);
			this.items.Add(section);
			this.SectionItemChanged(this, EventArgs.Empty);
		}

		public void Clear()
		{
			this.items.Clear();
			this.SectionItemChanged(this, EventArgs.Empty);
		}

		public bool Contains(ListSection section)
		{
			return this.items.Contains(section);
		}

		public void CopyTo(ListSection[] array, int arrayIndex)
		{
			this.items.CopyTo(array, arrayIndex);
		}

		public bool Remove(ListSection section)
		{
			bool result = this.items.Remove(section);
			this.SectionItemChanged(this, EventArgs.Empty);
			return result;
		}

		IEnumerator<ListSection> IEnumerable<ListSection>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		private void SectionItemChanged(object sender, EventArgs e)
		{
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged.Invoke(sender, e);
			}
		}
	}
}
