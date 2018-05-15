using System;

namespace Sce.Pss.HighLevel.UI
{
	public class ListSection
	{
		private string title;

		private int itemCount;

		public event EventHandler<EventArgs> ItemChanged;

		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				if (this.ItemChanged != null)
				{
					this.ItemChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public int ItemCount
		{
			get
			{
				return this.itemCount;
			}
			set
			{
				this.itemCount = value;
				if (this.ItemChanged != null)
				{
					this.ItemChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public ListSection()
		{
			this.Title = "";
			this.ItemCount = 0;
		}

		public ListSection(string title, int itemCount)
		{
			this.Title = title;
			this.ItemCount = itemCount;
		}
	}
}
