using Sce.Pss.Core;
using System;
using System.Collections.Generic;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	[Obsolete("Use DatePicker or TimePicker instead of SpinBox.")]
	public class SpinBox : Widget
	{
		private const float defaultHeight = 204f;

		private const float centerHeight = 54f;

		private const float bgWidthGap = 30f;

		private const float listVerticalMargin = 2f;

		private float[] unitPosX;

		private float[] bgWidth;

		private int minYear;

		private int maxYear;

		private int year;

		private int month;

		private int day;

		private int hour;

		private int minute;

		private UIPrimitive[] backPrimitives;

		private UIPrimitive[] centerPrimitives;

		private Label separatorLabelLeft;

		private Label separatorLabelRight;

		private InternalSpinBox.SpinList listLeft;

		private InternalSpinBox.SpinList listMiddle;

		private InternalSpinBox.SpinList listRight;

		private TextRenderHelper textRenderer;

		private readonly float unitWidth = 80f;

		private readonly float unitHeight = 40f;

		private readonly float unitWidthPower = 1.6f;

		internal static readonly int visibleCount;

		private static ImageAsset backgraundImage;

		private static ImageAsset centerImage;

		private static NinePatchMargin backgroundImageNinePatch;

		private static NinePatchMargin centerImageNinePatch;

		private static string[] noonText;

		private static string[] separatorCharactor;

		public event EventHandler<SpinBoxValueChangedEventArgs> ValueChanged;

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
				if (this.listLeft != null)
				{
					this.listLeft.PriorityHit = value;
				}
				if (this.listMiddle != null)
				{
					this.listMiddle.PriorityHit = value;
				}
				if (this.listRight != null)
				{
					this.listRight.PriorityHit = value;
				}
			}
		}

		public SpinBoxStyle Style
		{
			get;
			private set;
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
				if (this.Style == SpinBoxStyle.Date && this.listRight != null)
				{
					this.year = this.listRight.ScrollAreaFirstLine + this.MinYear;
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
				if (this.Style == SpinBoxStyle.Date && this.listLeft != null)
				{
					this.month = this.listLeft.ScrollAreaFirstLine + 3;
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
				if (this.Style == SpinBoxStyle.Date && this.listMiddle != null)
				{
					this.day = this.listMiddle.ScrollAreaFirstLine + 3;
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

		public int Hour
		{
			get
			{
				if (this.Style == SpinBoxStyle.Time && this.listLeft != null && this.listRight != null)
				{
					this.hour = this.listLeft.ScrollAreaFirstLine + 2;
					if (this.hour > 11)
					{
						this.hour -= 12;
					}
					this.hour += this.listRight.ScrollAreaFirstLine * 12;
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
				if (this.Style == SpinBoxStyle.Time && this.listMiddle != null)
				{
					this.minute = this.listMiddle.ScrollAreaFirstLine + 2;
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

		public DateTime DateTime
		{
			get
			{
				return new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, 0);
			}
			set
			{
				this.Year = value.Year;
				this.Month = value.Month;
				this.Day = value.Day;
				this.Hour = value.Hour;
				this.Minute = value.Minute;
			}
		}

		private float flickInterpRate
		{
			get
			{
				return this.listLeft.flickInterpRate;
			}
			set
			{
				this.listLeft.flickInterpRate = value;
				this.listMiddle.flickInterpRate = value;
				this.listRight.flickInterpRate = value;
			}
		}

		private float flickSpeedCoeff
		{
			get
			{
				return this.listLeft.flickSpeedCoeff;
			}
			set
			{
				this.listLeft.flickSpeedCoeff = value;
				this.listMiddle.flickSpeedCoeff = value;
				this.listRight.flickSpeedCoeff = value;
			}
		}

		private float fitInterpRate
		{
			get
			{
				return this.listLeft.fitInterpRate;
			}
			set
			{
				this.listLeft.fitInterpRate = value;
				this.listMiddle.fitInterpRate = value;
				this.listRight.fitInterpRate = value;
			}
		}

		public SpinBox(SpinBoxStyle style)
		{
			this.Style = style;
			if (style == SpinBoxStyle.Date)
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
			}
			else
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
			}
			this.MinYear = DateTime.Now.Year - 20;
			this.MaxYear = DateTime.Now.Year + 20;
			this.DateTime = DateTime.Now;
			this.textRenderer = new TextRenderHelper();
			this.backPrimitives = new UIPrimitive[3];
			this.centerPrimitives = new UIPrimitive[3];
			for (int i = 0; i < 3; i++)
			{
				UIPrimitive uIPrimitive = new UIPrimitive((DrawMode)4, 16, 28);
				uIPrimitive.Image = SpinBox.backgraundImage;
				uIPrimitive.ShaderType = ShaderType.Texture;
				UIPrimitiveUtility.SetupNinePatch(uIPrimitive, this.bgWidth[i], 204f, 0f, 0f, SpinBox.backgroundImageNinePatch);
				uIPrimitive.SetPosition(this.unitPosX[i], 0f);
				base.RootUIElement.AddChildLast(uIPrimitive);
				this.backPrimitives[i] = uIPrimitive;
			}
			for (int j = 0; j < 3; j++)
			{
				UIPrimitive uIPrimitive2 = new UIPrimitive((DrawMode)4, 16, 28);
				uIPrimitive2.Image = SpinBox.centerImage;
				uIPrimitive2.ShaderType = ShaderType.Texture;
				UIPrimitiveUtility.SetupNinePatch(uIPrimitive2, this.bgWidth[j], 54f, 0f, 0f, SpinBox.centerImageNinePatch);
				uIPrimitive2.SetPosition(this.unitPosX[j], 75f);
				base.RootUIElement.AddChildLast(uIPrimitive2);
				this.centerPrimitives[j] = uIPrimitive2;
			}
			base.Width = this.bgWidth[2] + this.unitPosX[2];
			base.Height = 204f;
			this.listLeft = new InternalSpinBox.SpinList();
			this.listLeft.Width = this.bgWidth[0];
			this.listLeft.Height = this.unitHeight * (float)SpinBox.visibleCount;
			this.listLeft.X = this.unitPosX[0];
			this.listLeft.Y = 2f;
			this.listLeft.ItemGapLine = this.unitHeight;
			this.listLeft.ScrollAreaLineNum = SpinBox.visibleCount;
			base.AddChildLast(this.listLeft);
			this.separatorLabelLeft = new Label();
			this.separatorLabelLeft.SetPosition(this.bgWidth[0], 0f);
			this.separatorLabelLeft.SetSize(30f, 204f);
			this.separatorLabelLeft.Font = this.textRenderer.Font;
			this.separatorLabelLeft.VerticalAlignment = VerticalAlignment.Middle;
			this.separatorLabelLeft.HorizontalAlignment = HorizontalAlignment.Center;
			this.separatorLabelLeft.Text = SpinBox.separatorCharactor[(int)this.Style];
			base.AddChildLast(this.separatorLabelLeft);
			this.listMiddle = new InternalSpinBox.SpinList();
			this.listMiddle.Width = this.bgWidth[1];
			this.listMiddle.Height = this.unitHeight * (float)SpinBox.visibleCount;
			this.listMiddle.X = this.unitPosX[1];
			this.listMiddle.Y = this.listLeft.Y;
			this.listMiddle.ItemGapLine = this.unitHeight;
			this.listMiddle.ScrollAreaLineNum = SpinBox.visibleCount;
			base.AddChildLast(this.listMiddle);
			this.separatorLabelRight = new Label();
			this.separatorLabelRight.SetPosition(this.unitPosX[1] + this.bgWidth[1], 0f);
			this.separatorLabelRight.SetSize(30f, 204f);
			this.separatorLabelRight.VerticalAlignment = VerticalAlignment.Middle;
			this.separatorLabelRight.HorizontalAlignment = HorizontalAlignment.Center;
			this.separatorLabelRight.Font = this.textRenderer.Font;
			this.separatorLabelRight.Text = ((this.Style == SpinBoxStyle.Date) ? SpinBox.separatorCharactor[(int)this.Style] : "");
			base.AddChildLast(this.separatorLabelRight);
			this.listRight = new InternalSpinBox.SpinList();
			this.listRight.Width = this.bgWidth[2];
			this.listRight.Height = this.unitHeight * (float)SpinBox.visibleCount;
			this.listRight.X = this.unitPosX[2];
			this.listRight.Y = this.listLeft.Y;
			this.listRight.ItemGapLine = this.unitHeight;
			this.listRight.ScrollAreaLineNum = SpinBox.visibleCount;
			base.AddChildLast(this.listRight);
			if (this.Style == SpinBoxStyle.Date)
			{
				this.listLeft.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction1Origin);
				this.listMiddle.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionDate);
				this.listRight.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionYear);
				this.UpdateListDate();
			}
			else
			{
				this.listLeft.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction0Origin);
				this.listMiddle.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestAction0Origin);
				this.listRight.ItemRequestAction += new EventHandler<InternalSpinBox.SpinItemRequestEventArgs>(this.GridListItemRequestActionAmPm);
				this.UpdateListTime();
			}
			this.listLeft.StartItemRequest();
			this.listMiddle.StartItemRequest();
			this.listRight.StartItemRequest();
			this.listLeft.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.listMiddle.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.listRight.FocusChanged += new EventHandler<EventArgs>(this.ItemFocusChanged);
			this.PriorityHit = true;
		}

		static SpinBox()
		{
			SpinBox.visibleCount = 5;
			SpinBox.noonText = new string[]
			{
				"AM",
				"PM"
			};
			SpinBox.separatorCharactor = new string[]
			{
				"/",
				":"
			};
			SpinBox.backgraundImage = new ImageAsset(SystemImageAsset.SpinBoxBase);
			SpinBox.centerImage = new ImageAsset(SystemImageAsset.SpinBoxCenter);
			SpinBox.backgroundImageNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.SpinBoxBase);
			SpinBox.centerImageNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.SpinBoxCenter);
		}

		public override bool HitTest(Vector2 screenPoint)
		{
			return false;
		}

		private void UpdateListDate()
		{
			if (this.Style != SpinBoxStyle.Date || this.listLeft == null || this.listMiddle == null || this.listRight == null)
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
			this.listLeft.ListItemNum = 12;
			int num2 = this.month - 1;
			this.listLeft.ScrollTo(num2 - SpinBox.visibleCount / 2, 0f, false);
			this.listLeft.FocusIndex = num2;
			this.day = MathUtility.Clamp<int>(this.day, 1, 31);
			this.listMiddle.ListItemNum = 31;
			num2 = this.day - 1;
			this.listMiddle.ScrollTo(num2 - SpinBox.visibleCount / 2, 0f, false);
			this.listMiddle.FocusIndex = num2;
			this.year = MathUtility.Clamp<int>(this.year, this.MinYear, this.MaxYear);
			this.listRight.ListItemNum = this.MaxYear - this.MinYear + 1 + (SpinBox.visibleCount - 1);
			num2 = this.year - this.MinYear;
			this.listRight.ScrollTo(num2, 0f, false);
			this.listRight.FocusIndex = num2 + SpinBox.visibleCount / 2;
			this.listLeft.IsLoop = true;
			this.listMiddle.IsLoop = true;
			this.listRight.IsLoop = false;
		}

		private void UpdateListTime()
		{
			if (this.Style != SpinBoxStyle.Time || this.listLeft == null || this.listMiddle == null || this.listRight == null)
			{
				return;
			}
			this.hour = MathUtility.Clamp<int>(this.hour, 0, 23);
			this.listLeft.ListItemNum = 12;
			int num = this.hour % 12;
			this.listLeft.ScrollTo(num - SpinBox.visibleCount / 2, 0f, false);
			this.listLeft.FocusIndex = num;
			this.minute = MathUtility.Clamp<int>(this.minute, 0, 59);
			this.listMiddle.ListItemNum = 60;
			num = this.minute;
			this.listMiddle.ScrollTo(num - SpinBox.visibleCount / 2, 0f, false);
			this.listMiddle.FocusIndex = num;
			this.listRight.ListItemNum = 2 + (SpinBox.visibleCount - 1);
			num = ((this.hour < 12) ? 0 : 1);
			this.listRight.ScrollTo(num, 0f, false);
			this.listRight.FocusIndex = num + SpinBox.visibleCount / 2;
			this.listLeft.IsLoop = true;
			this.listMiddle.IsLoop = true;
			this.listRight.IsLoop = false;
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
					if (e.Index < SpinBox.visibleCount / 2 || e.Index >= spinList.ListItemNum - SpinBox.visibleCount / 2)
					{
						text = "";
					}
					else
					{
						text = (e.Index + this.MinYear - SpinBox.visibleCount / 2).ToString("0000");
					}
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, (int)(this.unitWidth * this.unitWidthPower), (int)this.unitHeight);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
				}
			}
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
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, (int)this.unitWidth, (int)this.unitHeight);
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
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, (int)this.unitWidth, (int)this.unitHeight);
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
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, (int)this.unitWidth, (int)this.unitHeight);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
					listItem.Enabled = (listItem.ItemId + 1 <= this.GetDaysInMonth());
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
					if (e.Index < SpinBox.visibleCount / 2 || e.Index >= spinList.ListItemNum - SpinBox.visibleCount / 2)
					{
						text = "";
					}
					else
					{
						text = SpinBox.noonText[e.Index - SpinBox.visibleCount / 2];
					}
					listItem.ImageAsset = this.textRenderer.DrawText(ref text, (int)this.unitWidth, (int)this.unitHeight);
					listItem.ShaderType = ShaderType.TextTexture;
					listItem.Width = spinList.ItemGapStep;
					listItem.Height = spinList.ItemGapLine;
				}
			}
		}

		private int GetDaysInMonth()
		{
			int num = this.listRight.ScrollAreaFirstLine + this.MinYear;
			int num2 = this.listLeft.ScrollAreaFirstLine + SpinBox.visibleCount / 2 + 1;
			if (num2 > this.listLeft.TotalLineNum)
			{
				num2 -= this.listLeft.TotalLineNum;
			}
			return DateTime.DaysInMonth(num, num2);
		}

		private void ItemFocusChanged(object sender, EventArgs e)
		{
			if (this.Style == SpinBoxStyle.Date)
			{
				int daysInMonth = this.GetDaysInMonth();
				int num = this.listMiddle.ScrollAreaFirstLine + SpinBox.visibleCount / 2 + 1;
				if (num > this.listMiddle.TotalLineNum)
				{
					num -= this.listMiddle.TotalLineNum;
				}
				if (num > daysInMonth)
				{
					if (daysInMonth < 30 && num > 30)
					{
						this.listMiddle.ScrollTo(29, 0f, true);
					}
					else
					{
						int num2 = daysInMonth - 1;
						this.listMiddle.ScrollTo(num2 - SpinBox.visibleCount / 2, 0f, true);
					}
				}
				else if (this.ValueChanged != null)
				{
					this.ValueChanged.Invoke(this, new SpinBoxValueChangedEventArgs());
				}
				using (List<InternalSpinBox.SpinList.ListContainer>.Enumerator enumerator = this.listMiddle.listItem.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InternalSpinBox.SpinList.ListContainer current = enumerator.Current;
						current.listItem.Enabled = (current.listItem.ItemId + 1 <= daysInMonth);
					}
					return;
				}
			}
			if (this.ValueChanged != null)
			{
				this.ValueChanged.Invoke(this, new SpinBoxValueChangedEventArgs());
			}
		}
	}
}
