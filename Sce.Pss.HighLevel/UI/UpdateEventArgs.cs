using System;

namespace Sce.Pss.HighLevel.UI
{
	public class UpdateEventArgs : EventArgs
	{
		private float elapsedTime;

		public float ElapsedTime
		{
			get
			{
				return this.elapsedTime;
			}
		}

		public UpdateEventArgs(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}
	}
}
