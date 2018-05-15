using System;

namespace Sce.Pss.HighLevel.UI
{
	public class CheckBox : Widget
	{
		private enum CheckBoxState
		{
			Normal,
			Pressed,
			Disabled
		}

		private const float defaultCheckBoxWidth = 56f;

		private const float defaultCheckBoxHeight = 56f;

		private const float defaultRadioButtonWidth = 39f;

		private const float defaultRadioButtonHeight = 39f;

		private bool enabled;

		private bool checkedValue;

		private CheckBoxStyle style;

		private CustomCheckBoxImageSettings customCheckBoxImage;

		private CheckBox.CheckBoxState buttonState;

		private UISprite sprt;

		private ImageAsset[,,] images;

		private bool needUpdateFlag;

		public event EventHandler<TouchEventArgs> CheckedChanged;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				if (this.Style == CheckBoxStyle.Custom)
				{
					base.Width = value;
					this.needUpdateFlag = true;
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
				if (this.Style == CheckBoxStyle.Custom)
				{
					base.Height = value;
					this.needUpdateFlag = true;
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					this.ButtonState = (this.enabled ? CheckBox.CheckBoxState.Normal : CheckBox.CheckBoxState.Disabled);
				}
			}
		}

		public bool Checked
		{
			get
			{
				return this.checkedValue;
			}
			set
			{
				if (this.checkedValue != value)
				{
					this.checkedValue = value;
					this.needUpdateFlag = true;
				}
			}
		}

		public CheckBoxStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (this.style != value)
				{
					this.style = value;
					switch (this.style)
					{
					case CheckBoxStyle.CheckBox:
						base.Width = 56f;
						base.Height = 56f;
						break;
					case CheckBoxStyle.RadioButton:
						base.Width = 39f;
						base.Height = 39f;
						break;
					}
					this.needUpdateFlag = true;
				}
			}
		}

		public CustomCheckBoxImageSettings CustomCheckBoxImage
		{
			get
			{
				return this.customCheckBoxImage;
			}
			set
			{
				if (this.customCheckBoxImage != value)
				{
					if (this.customCheckBoxImage != null)
					{
						CustomCheckBoxImageSettings expr_17 = this.customCheckBoxImage;
						expr_17.ValueChanged = (CustomCheckBoxImageSettings.CustomCheckBoxImageSettingsChanged)Delegate.Remove(expr_17.ValueChanged, new CustomCheckBoxImageSettings.CustomCheckBoxImageSettingsChanged(this.customImageChanged));
					}
					this.customCheckBoxImage = value;
					if (this.customCheckBoxImage != null)
					{
						this.customImageChanged();
						CustomCheckBoxImageSettings expr_53 = this.customCheckBoxImage;
						expr_53.ValueChanged = (CustomCheckBoxImageSettings.CustomCheckBoxImageSettingsChanged)Delegate.Combine(expr_53.ValueChanged, new CustomCheckBoxImageSettings.CustomCheckBoxImageSettingsChanged(this.customImageChanged));
					}
					this.needUpdateFlag = true;
				}
			}
		}

		private CheckBox.CheckBoxState ButtonState
		{
			get
			{
				return this.buttonState;
			}
			set
			{
				if (this.buttonState != value)
				{
					this.buttonState = value;
					this.needUpdateFlag = true;
				}
			}
		}

		public CheckBox()
		{
			base.Width = 56f;
			base.Height = 56f;
			this.sprt = new UISprite(1);
			this.sprt.ShaderType = ShaderType.Texture;
			base.RootUIElement.AddChildLast(this.sprt);
			this.ButtonState = CheckBox.CheckBoxState.Normal;
			this.Enabled = true;
			this.Checked = false;
			this.CustomCheckBoxImage = null;
			this.Style = CheckBoxStyle.CheckBox;
			this.needUpdateFlag = true;
			this.PriorityHit = true;
			this.images = new ImageAsset[Enum.GetValues(typeof(CheckBoxStyle)).Length, Enum.GetValues(typeof(CheckBox.CheckBoxState)).Length, 2];
			this.images[0, 0, 0] = new ImageAsset(SystemImageAsset.CheckBoxUncheckedNormal);
			this.images[0, 0, 1] = new ImageAsset(SystemImageAsset.CheckBoxCheckedNormal);
			this.images[0, 1, 0] = new ImageAsset(SystemImageAsset.CheckBoxUncheckedPressed);
			this.images[0, 1, 1] = new ImageAsset(SystemImageAsset.CheckBoxCheckedPressed);
			this.images[0, 2, 0] = new ImageAsset(SystemImageAsset.CheckBoxUncheckedDisabled);
			this.images[0, 2, 1] = new ImageAsset(SystemImageAsset.CheckBoxCheckedDisabled);
			this.images[1, 0, 0] = new ImageAsset(SystemImageAsset.RadioButtonUncheckedNormal);
			this.images[1, 0, 1] = new ImageAsset(SystemImageAsset.RadioButtonCheckedNormal);
			this.images[1, 1, 0] = new ImageAsset(SystemImageAsset.RadioButtonUncheckedPressed);
			this.images[1, 1, 1] = new ImageAsset(SystemImageAsset.RadioButtonCheckedPressed);
			this.images[1, 2, 0] = new ImageAsset(SystemImageAsset.RadioButtonUncheckedDisabled);
			this.images[1, 2, 1] = new ImageAsset(SystemImageAsset.RadioButtonCheckedDisabled);
		}

		protected override void DisposeSelf()
		{
			for (int i = 0; i < this.images.GetLength(1); i++)
			{
				for (int j = 0; j < 2; j++)
				{
					if (this.images[0, i, j] != null)
					{
						this.images[0, i, j].Dispose();
					}
					if (this.images[1, i, j] != null)
					{
						this.images[1, i, j].Dispose();
					}
				}
			}
			base.DisposeSelf();
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (this.ButtonState == CheckBox.CheckBoxState.Disabled)
			{
				return;
			}
			switch (touchEvents.PrimaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.buttonState == CheckBox.CheckBoxState.Pressed)
				{
					if (this.style == CheckBoxStyle.RadioButton && this.Checked)
					{
						this.ButtonState = CheckBox.CheckBoxState.Normal;
						this.needUpdateFlag = true;
						return;
					}
					this.ButtonState = CheckBox.CheckBoxState.Normal;
					this.Checked = !this.Checked;
					this.needUpdateFlag = true;
					if (this.CheckedChanged != null)
					{
						this.CheckedChanged.Invoke(this, new TouchEventArgs(touchEvents));
						return;
					}
				}
				break;
			case TouchEventType.Down:
				this.ButtonState = CheckBox.CheckBoxState.Pressed;
				return;
			case TouchEventType.Move:
				break;
			case TouchEventType.Enter:
				this.ButtonState = CheckBox.CheckBoxState.Pressed;
				return;
			case TouchEventType.Leave:
				this.ButtonState = CheckBox.CheckBoxState.Normal;
				break;
			default:
				return;
			}
		}

		private void customImageChanged()
		{
			this.images[2, 0, 0] = this.customCheckBoxImage.NormalUncheckedImage;
			this.images[2, 0, 1] = this.customCheckBoxImage.NormalCheckedImage;
			this.images[2, 1, 0] = this.customCheckBoxImage.PressedUncheckedImage;
			this.images[2, 1, 1] = this.customCheckBoxImage.PressedCheckedImage;
			this.images[2, 2, 0] = this.customCheckBoxImage.DisabledUncheckedImage;
			this.images[2, 2, 1] = this.customCheckBoxImage.DisabledCheckedImage;
			this.needUpdateFlag = true;
		}

		protected internal override void OnResetState()
		{
			base.OnResetState();
			this.ButtonState = (this.Enabled ? CheckBox.CheckBoxState.Normal : CheckBox.CheckBoxState.Disabled);
		}

		protected internal override void Render()
		{
			if (this.needUpdateFlag)
			{
				UISpriteUnit unit = this.sprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				this.sprt.Image = this.images[(int)this.Style, (int)this.buttonState, this.Checked ? 1 : 0];
				this.needUpdateFlag = false;
			}
			base.Render();
		}
	}
}
