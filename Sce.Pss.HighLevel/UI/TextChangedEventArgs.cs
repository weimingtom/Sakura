using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TextChangedEventArgs : EventArgs
	{
		private string newText;

		private string oldText;

		public string NewText
		{
			get
			{
				return this.newText;
			}
		}

		public string OldText
		{
			get
			{
				return this.oldText;
			}
		}

		public TextChangedEventArgs(string oldText, string newText)
		{
			this.newText = newText;
			this.oldText = oldText;
		}
	}
}
