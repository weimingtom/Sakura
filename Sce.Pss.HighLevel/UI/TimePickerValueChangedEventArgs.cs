using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TimePickerValueChangedEventArgs : EventArgs
	{
		public int NewHour
		{
			get;
			set;
		}

		public int NewMinute
		{
			get;
			set;
		}

		public int OldHour
		{
			get;
			set;
		}

		public int OldMinute
		{
			get;
			set;
		}

		public TimePickerValueChangedEventArgs()
		{
		}

		internal TimePickerValueChangedEventArgs(int newH, int newM, int oldH, int oldM)
		{
			this.NewHour = newH;
			this.NewMinute = newM;
			this.OldHour = oldH;
			this.OldMinute = oldM;
		}

		public override string ToString()
		{
			return string.Format("New = {0:00}:{1:00}, Old = {2:00}:{3:00}", new object[]
			{
				this.NewHour,
				this.NewMinute,
				this.OldHour,
				this.OldMinute
			});
		}
	}
}
