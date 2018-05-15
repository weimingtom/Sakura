using System;

namespace Sce.Pss.HighLevel.UI
{
	public class PopupSelectionChangedEventArgs : EventArgs
	{
		private int oldIndex;

		private int newIndex;

		public int OldIndex
		{
			get
			{
				return this.oldIndex;
			}
		}

		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		public PopupSelectionChangedEventArgs(int oldIndex, int newIndex)
		{
			this.oldIndex = oldIndex;
			this.newIndex = newIndex;
		}
	}
}
