using System;

namespace Sce.Pss.HighLevel.UI
{
	public class MessageDialogButtonEventArgs : EventArgs
	{
		private MessageDialogResult result;

		public MessageDialogResult Result
		{
			get
			{
				return this.result;
			}
		}

		public MessageDialogButtonEventArgs(MessageDialogResult result)
		{
			this.result = result;
		}
	}
}
