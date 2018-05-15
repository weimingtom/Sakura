using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class DatePicker : Widget
	{
		private const float bgWidthGap = 30f;

		private float[] unitPosX;

		private float[] bgWidth;

		private Label separatorLabelLeft;

		private Label separatorLabelRight;

		private InternalSpinBox spinLeft;

		private InternalSpinBox spinMiddle;

		private InternalSpinBox spinRight;

		private TextRenderHelper textRenderer;

		private static string separatorCharactor = "/";

		private DateTime lastDate;

		private int minYear;

		private int maxYear;

		private int year;

		private int month;

		private int day;

		public event EventHandler<DatePickerValueChangedEventArgs> ValueChanged;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
			}
		}

		public override bool PriorityHit
		{
			get
			{
				return base.PriorityHit;
			}
			set
			{
				base.PriorityHit = value;
				if (this.spinLeft != null)
				{
					this.spinLeft.PriorityHit = value;
				}
				if (this.spinMiddle != null)
				{
					this.spinMiddle.PriorityHit = value;
				}
				if (this.spinRight != null)
				{
					this.spinRight.PriorityHit = value;
				}
			}
		}

		public int MinYear
		{
			get
			{
				return this.minYear;
			}
			set
			{
				this.minYear = value;
				this.UpdateListDate();
			}
		}

		public int MaxYear
		{
			get
			{
				return this.maxYear;
			}
			set
			{
				this.maxYear = value;
				this.UpdateListDate();
			}
		}

		public int Year
		{
			get
			{
				if (this.spinRight != null)
				{
					this.year = this.spinRight.spinList.ScrollAreaFirstLine + this.MinYear;
				}
				return this.year;
			}
			set
			{
				this.year = value;
				this.UpdateListDate();
			}
		}

		public int Month
		{
			get
			{
				if (this.spinLeft != null)
				{
					this.month = this.spinLeft.spinList.ScrollAreaFirstLine + 3;
					if (this.month > 12)
					{
						this.month -= 12;
					}
				}
				return this.month;
			}
			set
			{
				this.month = value;
				this.UpdateListDate();
			}
		}

		public int Day
		{
			get
			{
				if (this.spinMiddle != null)
				{
					this.day = this.spinMiddle.spinList.ScrollAreaFirstLine + 3;
					if (this.day > 31)
					{
						this.day -= 31;
					}
				}
				return this.day;
			}
			set
			{
				this.day = value;
				this.UpdateListDate();
			}
		}

		public DateTime Date
		{
			get
			{
				return new DateTime(this.Year, this.Month, this.Day);
			}
			set
			{
				this.Year = value.Year;
				this.Month = value.Month;
				this.Day = value.Day;
			}
		}

		private float flickInterpRate
		{
			get
			{
				return this.spinLeft.spinList.flickInterpRate;
			}
			set
			{
				this.spinLeft.spinList.flickInterpRate = value;
				this.spinMiddle.spinList.flickInterpRate = value;
				this.spinRight.spinList.flickInterpRate = value;
			}
		}

		private float flickSpeedCoeff
		{
			get
			{
				return this.spinLeft.spinList.flickSpeedCoeff;
			}
			set
			{
				this.spinLeft.spinList.flickSpeedCoeff = value;
				this.spinMiddle.spinList.flickSpeedCoeff = value;
				this.spinRight.spinList.flickSpeedCoeff = value;
			}
		}

		private float fitInterpRate
		{
			get
			{
				return this.spinLeft.spinList.fitInterpRate;
			}
			set
			{
				this.spinLeft.spinList.fitInterpRate = value;
				this.spinMiddle.spinList.fitInterpRate = value;
				this.spinRight.spinList.fitInterpRate = value;
			}
		}

		public DatePicker()
		{
			this.unitPosX = new float[]
			{
				default(float),
				110f,
				220f
			};
			this.bgWidth = new float[]
			{
				80f,
				80f,
				120f
			};
			this.Date = DateTime.Now;
			this.MinYear = this.Date.Year - 20;
			this.MaxYear = this.Date.Year + 20;
			this.textRenderer = new TextRenderHelper();
			base.Width = this.bgWidth[2] + this.unitPosX[2];
			base.Height = 204f;
			this.spinLeft = new InternalSpinBox();
			this.spinLeft.Width = this.bgWidth[0];
			this.spinLeft.X = this.unitPosX[0];
			base.AddChildLast(this.spinLeft);
			this.separatorLabelLeft = new Label();
			this.separatorLabelLeft.SetPosition(this.bgWidth[0], 0f);
			this.separatorLabelLeft.SetSize(30f, 204f);
			this.separatorLabelLeft.Font = this.textRenderer.Font;
			this.separatorLabelLeft.VerticalAlignment = VerticalAlignment.Middle;
			this.separatorLabelLeft.HorizontalAlignment = HorizontalAlignment.Center;
			this.separatorLabelLeft.Text = DatePicker.separatorCharactor;
			base.AddChildLast(this.separatorLabelLeft);
			this.spinMiddle = new InternalSpinBox();
			this.spinMiddle.Width = this.bgWidth[1];
			this.spinMiddle.X = this.unitPosX[1];
			base.AddChildLast(this.spinMiddle);
			this.separatorLabelRight = new Label();
			this.separatorLabelRight.SetPosition(this.unitPosX[1] + this.bgWidth[1], 0f);
			this.separatorLabelRight.SetSize(30f, 204f);
			this.separatorLabelRight.VerticalAlignment = VerticalAlignment.Middle;
			this.separatorLabelRight.HorizontalAlignment = HorizontalAlignment.Center;
			this.separatorLabelRight.Font = this.textRenderer.Font;
			this.separatorLabelRight.Text = DatePicker.separatorCharactor;
			base.AddChildLast(this.separatorLabelRight);
			this.spinRight = new InternalSpinBox();
			this.spinRight.Width = this.bgWidth[2];
			this.spinRight.X = this.unitPosX[2];
			base.AddChildLast(this.spinRight);
			this.spinLeft.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction1Origin);
			this.spinMiddle.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionDate);
			this.spinRight.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionYear);
			this.UpdateListDate();
			this.spinLeft.spinList.StartItemRequest();
			this.spinMiddle.spinList.StartItemRequest();
			this.spinRight.spinList.StartItemRequest();
			this.spinLeft.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.spinMiddle.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.spinRight.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.PriorityHit = true;
			this.lastDate = this.Date;
		}

		public override bool HitTest(Vector2 screenPoint)
		{
			return false;
		}

		private void UpdateListDate()
		{
			if (this.spinLeft == null || this.spinMiddle == null || this.spinRight == null)
			{
				return;
			}
			if (this.MaxYear < this.MinYear)
			{
				int num = this.MaxYear;
				this.maxYear = this.MinYear;
				this.minYear = num;
			}
			this.month = MathUtility.Clamp<int>(this.month, 1, 12);
			this.spinLeft.spinList.ListItemNum = 12;
			int num2 = this.month - 1;
			this.spinLeft.spinList.ScrollTo(num2 - InternalSpinBox.visibleCount / 2, 0f, false);
			this.spinLeft.spinList.FocusIndex = num2;
			this.day = MathUtility.Clamp<int>(this.day, 1, 31);
			this.spinMiddle.spinList.ListItemNum = 31;
			num2 = this.day - 1;
			this.spinMiddle.spinList.ScrollTo(num2 - InternalSpinBox.visibleCount / 2, 0f, false);
			this.spinMiddle.spinList.FocusIndex = num2;
			this.year = MathUtility.Clamp<int>(this.year, this.MinYear, this.MaxYear);
			this.spinRight.spinList.ListItemNum = this.MaxYear - this.MinYear + 1 + (InternalSpinBox.visibleCount - 1);
			num2 = this.year - this.MinYear;
			this.spinRight.spinList.ScrollTo(num2, 0f, false);
			this.spinRight.spinList.FocusIndex = num2 + InternalSpinBox.visibleCount / 2;
			this.spinLeft.spinList.IsLoop = true;
			this.spinMiddle.spinList.IsLoop = true;
			this.spinRight.spinList.IsLoop = false;
		}

		private void GridListItemRequestActionYear(object sender, InternalSpinBox.SpinItemRequestEventArgs e)
		{
			if (sender is InternalSpinBox.SpinList)
			{
				InternalSpinBox.SpinList spinList = (InternalSpinBox.SpinList)sender;
				InternalSpinBox.SpinItem listItem = spinList.GetListItem(e.Index);
				if (listItem != null && 0 <= e.Index && e.Index < spinList.ListItemNum)
				{
					string text;
					if (e.Index < InternalSpinBox.visibleCount / 2 || e.Index >= spinList.ListItemNum - InternalSpinBox.visibleCount / 2)
					{
						text = "";
					}
					else
					{
						text = (e.Index + this.MinYear - InternalSpinBox.visibleCount / 2).ToString("0000");
					}
					if (listItem.ImageAsset != null)
					{
						listItem.ImageAsset.Dispose();
					}
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, 128, 40);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
				}
			}
		}

		private void GridListItemRequestAction1Origin(object sender, InternalSpinBox.SpinItemRequestEventArgs e)
		{
			if (sender is InternalSpinBox.SpinList)
			{
				InternalSpinBox.SpinList spinList = (InternalSpinBox.SpinList)sender;
				int num = e.Index % spinList.ListItemNum;
				if (num < 0)
				{
					num += spinList.ListItemNum;
				}
				InternalSpinBox.SpinItem listItem = spinList.GetListItem(num);
				if (listItem != null)
				{
					string text = (num + 1).ToString("00");
					if (listItem.ImageAsset != null)
					{
						listItem.ImageAsset.Dispose();
					}
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, 80, 40);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
				}
			}
		}

		private void GridListItemRequestActionDate(object sender, InternalSpinBox.SpinItemRequestEventArgs e)
		{
			if (sender is InternalSpinBox.SpinList)
			{
				InternalSpinBox.SpinList spinList = (InternalSpinBox.SpinList)sender;
				int num = e.Index % spinList.ListItemNum;
				if (num < 0)
				{
					num += spinList.ListItemNum;
				}
				InternalSpinBox.SpinItem listItem = spinList.GetListItem(num);
				if (listItem != null)
				{
					string text = (num + 1).ToString("00");
					if (listItem.ImageAsset != null)
					{
						listItem.ImageAsset.Dispose();
					}
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, 80, 40);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
					listItem.Enabled = (listItem.ItemId + 1 <= this.GetDaysInMonth());
				}
			}
		}

		private int GetDaysInMonth()
		{
			int num = this.spinRight.spinList.ScrollAreaFirstLine + this.MinYear;
			int num2 = this.spinLeft.spinList.ScrollAreaFirstLine + InternalSpinBox.visibleCount / 2 + 1;
			if (num2 > this.spinLeft.spinList.TotalLineNum)
			{
				num2 -= this.spinLeft.spinList.TotalLineNum;
			}
			return DateTime.DaysInMonth(num, num2);
		}

		private void ItemFocusChanged(object sender, EventArgs e)
		{
			int daysInMonth = this.GetDaysInMonth();
			int num = this.spinMiddle.spinList.ScrollAreaFirstLine + InternalSpinBox.visibleCount / 2 + 1;
			if (num > this.spinMiddle.spinList.TotalLineNum)
			{
				num -= this.spinMiddle.spinList.TotalLineNum;
			}
			if (num > daysInMonth)
			{
				if (daysInMonth < 30 && num > 30)
				{
					this.spinMiddle.spinList.ScrollTo(29, 0f, true);
				}
				else
				{
					int num2 = daysInMonth - 1;
					this.spinMiddle.spinList.ScrollTo(num2 - InternalSpinBox.visibleCount / 2, 0f, true);
				}
			}
			else if (this.ValueChanged != null)
			{
				DateTime oldDate = this.lastDate;
				this.lastDate = this.Date;
				this.ValueChanged.Invoke(this, new DatePickerValueChangedEventArgs(this.lastDate, oldDate));
			}
			using (List<InternalSpinBox.SpinList.ListContainer>.Enumerator enumerator = this.spinMiddle.spinList.listItem.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InternalSpinBox.SpinList.ListContainer current = enumerator.Current;
					current.listItem.Enabled = (current.listItem.ItemId + 1 <= daysInMonth);
				}
			}
		}
	}
}
