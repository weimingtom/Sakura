using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TimePicker : Widget
	{
		private const float bgWidthGap = 30f;

		private float[] unitPosX;

		private float[] bgWidth;

		private Label separatorLabelLeft;

		private InternalSpinBox spinLeft;

		private InternalSpinBox spinMiddle;

		private InternalSpinBox spinRight;

		private TextRenderHelper textRenderer;

		private static string[] noonText = new string[]
		{
			"AM",
			"PM"
		};

		private static string separatorCharactor = ":";

		private int lastHour;

		private int lastMinute;

		private int hour;

		private int minute;

		public event EventHandler<TimePickerValueChangedEventArgs> ValueChanged;

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

		public int Hour
		{
			get
			{
				if (this.spinLeft != null && this.spinRight != null)
				{
					this.hour = this.spinLeft.spinList.ScrollAreaFirstLine + 2;
					if (this.hour > 11)
					{
						this.hour -= 12;
					}
					this.hour += this.spinRight.spinList.ScrollAreaFirstLine * 12;
				}
				return this.hour;
			}
			set
			{
				this.hour = value;
				this.UpdateListTime();
			}
		}

		public int Minute
		{
			get
			{
				if (this.spinMiddle != null)
				{
					this.minute = this.spinMiddle.spinList.ScrollAreaFirstLine + 2;
					if (this.minute > 59)
					{
						this.minute -= 60;
					}
				}
				return this.minute;
			}
			set
			{
				this.minute = value;
				this.UpdateListTime();
			}
		}

		public DateTime Time
		{
			get
			{
				return new DateTime(1, 1, 1, this.Hour, this.Minute, 0);
			}
			set
			{
				this.Hour = value.Hour;
				this.Minute = value.Minute;
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

		public TimePicker()
		{
			this.unitPosX = new float[]
			{
				default(float),
				110f,
				200f
			};
			this.bgWidth = new float[]
			{
				80f,
				80f,
				80f
			};
			this.Time = DateTime.Now;
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
			this.separatorLabelLeft.Text = TimePicker.separatorCharactor;
			base.AddChildLast(this.separatorLabelLeft);
			this.spinMiddle = new InternalSpinBox();
			this.spinMiddle.Width = this.bgWidth[1];
			this.spinMiddle.X = this.unitPosX[1];
			base.AddChildLast(this.spinMiddle);
			this.spinRight = new InternalSpinBox();
			this.spinRight.Width = this.bgWidth[2];
			this.spinRight.X = this.unitPosX[2];
			base.AddChildLast(this.spinRight);
			this.spinLeft.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction0Origin);
			this.spinMiddle.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction0Origin);
			this.spinRight.spinList.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionAmPm);
			this.UpdateListTime();
			this.spinLeft.spinList.StartItemRequest();
			this.spinMiddle.spinList.StartItemRequest();
			this.spinRight.spinList.StartItemRequest();
			this.spinLeft.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.spinMiddle.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.spinRight.spinList.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.PriorityHit = true;
			this.lastHour = this.Hour;
			this.lastMinute = this.Minute;
		}

		public override bool HitTest(Vector2 screenPoint)
		{
			return false;
		}

		private void UpdateListTime()
		{
			if (this.spinLeft == null || this.spinMiddle == null || this.spinRight == null)
			{
				return;
			}
			this.hour = MathUtility.Clamp<int>(this.hour, 0, 23);
			this.spinLeft.spinList.ListItemNum = 12;
			int num = this.hour % 12;
			this.spinLeft.spinList.ScrollTo(num - InternalSpinBox.visibleCount / 2, 0f, false);
			this.spinLeft.spinList.FocusIndex = num;
			this.minute = MathUtility.Clamp<int>(this.minute, 0, 59);
			this.spinMiddle.spinList.ListItemNum = 60;
			num = this.minute;
			this.spinMiddle.spinList.ScrollTo(num - InternalSpinBox.visibleCount / 2, 0f, false);
			this.spinMiddle.spinList.FocusIndex = num;
			this.spinRight.spinList.ListItemNum = 2 + (InternalSpinBox.visibleCount - 1);
			num = ((this.hour < 12) ? 0 : 1);
			this.spinRight.spinList.ScrollTo(num, 0f, false);
			this.spinRight.spinList.FocusIndex = num + InternalSpinBox.visibleCount / 2;
			this.spinLeft.spinList.IsLoop = true;
			this.spinMiddle.spinList.IsLoop = true;
			this.spinRight.spinList.IsLoop = false;
		}

		private void GridListItemRequestAction0Origin(object sender, InternalSpinBox.SpinItemRequestEventArgs e)
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
					string text = num.ToString("00");
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

		private void GridListItemRequestActionAmPm(object sender, InternalSpinBox.SpinItemRequestEventArgs e)
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
						text = TimePicker.noonText[e.Index - InternalSpinBox.visibleCount / 2];
					}
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

		private void ItemFocusChanged(object sender, EventArgs e)
		{
			int oldH = this.lastHour;
			int oldM = this.lastMinute;
			this.lastHour = this.Hour;
			this.lastMinute = this.Minute;
			if (this.ValueChanged != null)
			{
				this.ValueChanged.Invoke(this, new TimePickerValueChangedEventArgs(this.lastHour, this.lastMinute, oldH, oldM));
			}
		}
	}
}
