using System;

namespace Sce.Pss.HighLevel.UI
{
	public class SliderValueChangeEventArgs : EventArgs
	{
		private float value;

		public float Value
		{
			get
			{
				return this.value;
			}
		}

		public SliderValueChangeEventArgs(float value)
		{
			this.value = value;
		}
	}
}
