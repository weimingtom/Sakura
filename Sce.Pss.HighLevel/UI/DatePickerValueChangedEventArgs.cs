using System;

namespace Sce.Pss.HighLevel.UI
{
	public class DatePickerValueChangedEventArgs : EventArgs
	{
		public int NewYear
		{
			get;
			set;
		}

		public int NewMonth
		{
			get;
			set;
		}

		public int NewDay
		{
			get;
			set;
		}

		public int OldYear
		{
			get;
			set;
		}

		public int OldMonth
		{
			get;
			set;
		}

		public int OldDay
		{
			get;
			set;
		}

		public DatePickerValueChangedEventArgs()
		{
		}

		internal DatePickerValueChangedEventArgs(DateTime newDate, DateTime oldDate)
		{
			this.NewYear = newDate.Year;
			this.NewMonth = newDate.Month;
			this.NewDay = newDate.Day;
			this.OldYear = oldDate.Year;
			this.OldMonth = oldDate.Month;
			this.OldDay = oldDate.Day;
		}

		public override string ToString()
		{
			return string.Format("New = {1:00}/{2:00}/{0:0000}, Old = {4:00}/{5:00}/{3:0000}", new object[]
			{
				this.NewYear,
				this.NewMonth,
				this.NewDay,
				this.OldYear,
				this.OldMonth,
				this.OldDay
			});
		}
	}
}
