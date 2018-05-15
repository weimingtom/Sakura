using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class Slider : Widget
	{
		private enum SliderState
		{
			Normal,
			Pressed,
			Disable
		}

		private const float defaultSliderHorizontalWidth = 362f;

		private const float defaultSliderHorizontalHeight = 58f;

		private const float defaultSliderVerticalWidth = 58f;

		private const float defaultSliderVerticalHeight = 232f;

		private const float handleExtraMargin = 5f;

		private SliderOrientation orientation;

		private float value;

		private float minValue;

		private float maxValue;

		private float step;

		private ImageBox baseImage;

		private ImageAsset[] baseImageAssets;

		private NinePatchMargin[] baseImageNinePatchMargins;

		private ImageBox barImage;

		private ImageAsset[] barImageAssets;

		private NinePatchMargin[] barImageNinePatchMargins;

		private ImageBox handleImage;

		private ImageAsset[,] handleImageAssets;

		private Slider.SliderState state;

		private Vector2 handleClickPos;

		public event EventHandler<SliderValueChangeEventArgs> ValueChanging;

		public event EventHandler<SliderValueChangeEventArgs> ValueChanged;

		public SliderOrientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				this.orientation = value;
				this.baseImage.Image = this.baseImageAssets[(int)this.orientation];
				this.baseImage.NinePatchMargin = this.baseImageNinePatchMargins[(int)this.orientation];
				this.barImage.Image = this.barImageAssets[(int)this.orientation];
				this.barImage.NinePatchMargin = this.barImageNinePatchMargins[(int)this.orientation];
				this.handleImage.Image = this.handleImageAssets[(int)this.orientation, (int)this.state];
				this.handleImage.Width = (float)this.handleImage.Image.Width;
				this.handleImage.Height = (float)this.handleImage.Image.Height;
				this.handleImage.NinePatchMargin = NinePatchMargin.Zero;
				switch (this.orientation)
				{
				case SliderOrientation.Horizontal:
					base.Width = 362f;
					base.Height = 58f;
					this.baseImage.Width = 362f;
					this.barImage.Width = this.baseImage.Width;
					this.baseImage.X = 0f;
					this.barImage.X = 0f;
					this.baseImage.Height = (float)this.baseImageAssets[(int)this.orientation].Height;
					this.barImage.Height = this.baseImage.Height;
					this.baseImage.Y = (58f - this.baseImage.Height - 1f) / 2f;
					this.barImage.Y = this.baseImage.Y;
					break;
				case SliderOrientation.Vertical:
					base.Width = 58f;
					base.Height = 232f;
					this.baseImage.Width = (float)this.baseImageAssets[(int)this.orientation].Width;
					this.barImage.Width = this.baseImage.Width;
					this.baseImage.X = (58f - this.baseImage.Width - 1f) / 2f;
					this.barImage.X = this.baseImage.X;
					this.baseImage.Height = 232f;
					this.barImage.Height = this.barImage.Height;
					this.baseImage.Y = 0f;
					this.barImage.Y = 0f;
					break;
				}
				this.UpdateView();
			}
		}

		public bool FillValue
		{
			get
			{
				return this.barImage.Visible;
			}
			set
			{
				this.barImage.Visible = value;
			}
		}

		public float Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.step > 1.401298E-45f)
				{
					this.value = value - ((value + this.step * 0.5f - this.minValue) % this.step - this.step * 0.5f);
					if (this.value > this.maxValue - (this.maxValue - this.minValue) % this.step / 2f)
					{
						this.value = this.maxValue;
					}
				}
				else
				{
					this.value = value;
				}
				if (this.value < this.minValue)
				{
					this.value = this.minValue;
				}
				else if (this.value > this.maxValue)
				{
					this.value = this.maxValue;
				}
				this.UpdateView();
			}
		}

		public float MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue = value;
				if (this.minValue > this.maxValue)
				{
					this.maxValue = this.minValue;
				}
				if (this.value < this.minValue)
				{
					this.value = this.minValue;
				}
				this.UpdateView();
			}
		}

		public float MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue = value;
				if (this.minValue > this.maxValue)
				{
					this.minValue = this.maxValue;
				}
				if (this.value > this.maxValue)
				{
					this.value = this.maxValue;
				}
				this.UpdateView();
			}
		}

		public bool ValueChangeEventEnabled
		{
			get;
			set;
		}

		public bool Enabled
		{
			get
			{
				return this.state != Slider.SliderState.Disable;
			}
			set
			{
				if (value)
				{
					if (this.state == Slider.SliderState.Disable)
					{
						this.state = Slider.SliderState.Normal;
						this.UpdateView();
						return;
					}
				}
				else if (this.state != Slider.SliderState.Disable)
				{
					this.state = Slider.SliderState.Disable;
					this.UpdateView();
				}
			}
		}

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				if (this.Orientation == SliderOrientation.Horizontal)
				{
					base.Width = value;
					if (this.baseImage != null)
					{
						this.baseImage.Width = value;
						this.UpdateView();
					}
				}
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
				if (this.Orientation == SliderOrientation.Vertical)
				{
					base.Height = value;
					if (this.baseImage != null)
					{
						this.baseImage.Height = value;
						this.UpdateView();
					}
				}
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
				if (this.handleImage != null)
				{
					this.handleImage.PriorityHit = value;
				}
			}
		}

		public float Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
			}
		}

		public Slider()
		{
			this.baseImageAssets = new ImageAsset[Enum.GetValues(typeof(SliderOrientation)).Length];
			this.baseImageAssets[1] = new ImageAsset(SystemImageAsset.SliderVerticalBaseNormal);
			this.baseImageAssets[0] = new ImageAsset(SystemImageAsset.SliderHorizontalBaseNormal);
			this.baseImageNinePatchMargins = new NinePatchMargin[Enum.GetValues(typeof(SliderOrientation)).Length];
			this.baseImageNinePatchMargins[1] = AssetManager.GetNinePatchMargin(SystemImageAsset.SliderVerticalBaseNormal);
			this.baseImageNinePatchMargins[0] = AssetManager.GetNinePatchMargin(SystemImageAsset.SliderHorizontalBaseNormal);
			this.barImageAssets = new ImageAsset[Enum.GetValues(typeof(SliderOrientation)).Length];
			this.barImageAssets[1] = new ImageAsset(SystemImageAsset.SliderVerticalBarNormal);
			this.barImageAssets[0] = new ImageAsset(SystemImageAsset.SliderHorizontalBarNormal);
			this.barImageNinePatchMargins = new NinePatchMargin[Enum.GetValues(typeof(SliderOrientation)).Length];
			this.barImageNinePatchMargins[1] = AssetManager.GetNinePatchMargin(SystemImageAsset.SliderVerticalBarNormal);
			this.barImageNinePatchMargins[0] = AssetManager.GetNinePatchMargin(SystemImageAsset.SliderHorizontalBarNormal);
			this.handleImageAssets = new ImageAsset[Enum.GetValues(typeof(SliderOrientation)).Length, Enum.GetValues(typeof(Slider.SliderState)).Length];
			this.handleImageAssets[1, 0] = new ImageAsset(SystemImageAsset.SliderVerticalHandleNormal);
			this.handleImageAssets[1, 1] = new ImageAsset(SystemImageAsset.SliderVerticalHandlePressed);
			this.handleImageAssets[1, 2] = new ImageAsset(SystemImageAsset.SliderVerticalHandleDisabled);
			this.handleImageAssets[0, 0] = new ImageAsset(SystemImageAsset.SliderHorizontalHandleNormal);
			this.handleImageAssets[0, 1] = new ImageAsset(SystemImageAsset.SliderHorizontalHandlePressed);
			this.handleImageAssets[0, 2] = new ImageAsset(SystemImageAsset.SliderHorizontalHandleDisabled);
			this.baseImage = new ImageBox();
			this.baseImage.ImageScaleType = ImageScaleType.NinePatch;
			this.baseImage.TouchResponse = false;
			base.AddChildLast(this.baseImage);
			this.barImage = new ImageBox();
			this.barImage.ImageScaleType = ImageScaleType.NinePatch;
			this.barImage.TouchResponse = false;
			base.AddChildLast(this.barImage);
			this.handleImage = new ImageBox();
			this.handleImage.ImageScaleType = ImageScaleType.NinePatch;
			base.AddChildLast(this.handleImage);
			this.value = 0f;
			this.minValue = 0f;
			this.maxValue = 1f;
			this.state = Slider.SliderState.Normal;
			this.ValueChangeEventEnabled = true;
			this.ValueChanging = null;
			this.ValueChanged = null;
			this.handleClickPos = Vector2.Zero;
			this.Orientation = SliderOrientation.Horizontal;
			base.HookChildTouchEvent = true;
			this.PriorityHit = true;
			this.UpdateView();
		}

		protected override void DisposeSelf()
		{
			ImageAsset[] array = this.baseImageAssets;
			for (int i = 0; i < array.Length; i++)
			{
				ImageAsset imageAsset = array[i];
				imageAsset.Dispose();
			}
			ImageAsset[] array2 = this.barImageAssets;
			for (int j = 0; j < array2.Length; j++)
			{
				ImageAsset imageAsset2 = array2[j];
				imageAsset2.Dispose();
			}
			ImageAsset[,] array3 = this.handleImageAssets;
			int upperBound = array3.GetUpperBound(0);
			int upperBound2 = array3.GetUpperBound(1);
			for (int k = array3.GetLowerBound(0); k <= upperBound; k++)
			{
				for (int l = array3.GetLowerBound(1); l <= upperBound2; l++)
				{
					ImageAsset imageAsset3 = array3[k, l];
					imageAsset3.Dispose();
				}
			}
			base.DisposeSelf();
		}

		public override bool HitTest(Vector2 screenPoint)
		{
			return false;
		}

		private void GetHandleImagePos(float value, out Vector2 position)
		{
			position = Vector2.Zero;
			float num = value - this.MinValue;
			switch (this.Orientation)
			{
			case SliderOrientation.Horizontal:
			{
				float num2 = -5f;
				float num3 = Math.Max(num2, this.Width - this.handleImage.Width + 5f);
				position.Y = (this.Height - this.handleImage.Height) / 2f;
				if (this.MaxValue - this.MinValue > 0f)
				{
					position.X = (num3 - num2) / (this.MaxValue - this.MinValue) * num + num2 + 0.5f;
					return;
				}
				position.X = num2 + 0.5f;
				return;
			}
			case SliderOrientation.Vertical:
			{
				num = this.MaxValue - value;
				float num4 = -5f;
				float num5 = Math.Max(num4, this.Height - this.handleImage.Height + 5f);
				position.X = (this.Width - this.handleImage.Width) / 2f;
				if (this.MaxValue - this.MinValue > 0f)
				{
					position.Y = (num5 - num4) / (this.MaxValue - this.MinValue) * num + num4 + 0.5f;
					return;
				}
				position.Y = num4 + 0.5f;
				return;
			}
			default:
				return;
			}
		}

		private void SetHandleImagePos(Vector2 position)
		{
			this.handleImage.X = position.X;
			this.handleImage.Y = position.Y;
		}

		private void SetBarImagePos(Vector2 position)
		{
			switch (this.Orientation)
			{
			case SliderOrientation.Horizontal:
				this.barImage.Width = position.X + this.handleImage.Width / 2f;
				return;
			case SliderOrientation.Vertical:
				this.barImage.Y = position.Y + this.handleImage.Height / 2f;
				this.barImage.Height = this.Height - this.barImage.Y;
				return;
			default:
				return;
			}
		}

		private void UpdateHandlePos(Vector2 localTouchPos)
		{
			Vector2 vector = new Vector2(this.handleImage.X, this.handleImage.Y);
			Vector2 vector2 = localTouchPos;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			bool flag = false;
			switch (this.Orientation)
			{
			case SliderOrientation.Horizontal:
				num = 0f;
				num2 = Math.Max(num, this.Width - this.handleImage.Width);
				num3 = MathUtility.Clamp<float>(this.handleClickPos.X + vector2.X, num, num2);
				vector.X = num3;
				break;
			case SliderOrientation.Vertical:
				flag = true;
				num = 0f;
				num2 = Math.Max(num, this.Height - this.handleImage.Height);
				num3 = MathUtility.Clamp<float>(this.handleClickPos.Y + vector2.Y, num, num2);
				vector.Y = num3;
				break;
			}
			this.SetHandleImagePos(vector);
			this.SetBarImagePos(vector);
			float num4 = (num3 - num) / (num2 - num) * (this.MaxValue - this.MinValue) + this.MinValue;
			if (flag)
			{
				num4 = this.MaxValue - num4 + this.MinValue;
			}
			this.Value = num4;
		}

		private void UpdateView()
		{
			this.handleImage.Image = this.handleImageAssets[(int)this.Orientation, (int)this.state];
			Vector2 vector;
			this.GetHandleImagePos(this.value, out vector);
			this.SetHandleImagePos(vector);
			this.SetBarImagePos(vector);
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (!this.Enabled)
			{
				return;
			}
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			switch (primaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.state == Slider.SliderState.Pressed)
				{
					this.state = Slider.SliderState.Normal;
					this.UpdateView();
					if (this.ValueChangeEventEnabled && this.ValueChanged != null)
					{
						this.ValueChanged.Invoke(this, new SliderValueChangeEventArgs(this.Value));
					}
				}
				break;
			case TouchEventType.Down:
				this.handleClickPos.X = this.handleImage.X - primaryTouchEvent.LocalPosition.X;
				this.handleClickPos.Y = this.handleImage.Y - primaryTouchEvent.LocalPosition.Y;
				this.state = Slider.SliderState.Pressed;
				this.UpdateView();
				return;
			case TouchEventType.Move:
				if (this.state == Slider.SliderState.Pressed)
				{
					this.UpdateHandlePos(primaryTouchEvent.LocalPosition);
					if (this.ValueChangeEventEnabled && this.ValueChanging != null)
					{
						this.ValueChanging.Invoke(this, new SliderValueChangeEventArgs(this.Value));
						return;
					}
				}
				break;
			default:
				return;
			}
		}
	}
}
