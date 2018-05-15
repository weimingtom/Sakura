using Sce.Pss.Core.Imaging;
using System;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	public class Button : Widget
	{
		private enum ButtonState
		{
			Normal,
			Pressed,
			Disabled
		}

		[Flags]
		private enum UpdateFlags
		{
			Background = 1,
			Text = 2,
			Icon = 4
		}

		private const float defaultButtonWidth = 214f;

		private const float defaultButtonHeight = 56f;

		private string text;

		private ButtonStyle style;

		private ImageAsset iconImage;

		private Font textFont;

		private TextTrimming textTrimming;

		private UIColor textColor;

		private TextShadowSettings textShadow;

		private HorizontalAlignment horizontalAlignment;

		private VerticalAlignment verticalAlignment;

		private CustomButtonImageSettings customImage;

		private Button.ButtonState state;

		private UIColor backgroundFilterColor = new UIColor(1f, 1f, 1f, 1f);

		private UISprite iconSprt;

		private UISprite textSprt;

		private UIPrimitive backgroundPrim;

		private ImageAsset[,] backgroundImages;

		private NinePatchMargin[] backgroundNinePatchs;

		private Button.UpdateFlags updateFlags;

		public event EventHandler<TouchEventArgs> ButtonAction;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				if (base.Width != value)
				{
					base.Width = value;
					this.updateFlags |= Button.UpdateFlags.Background;
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= Button.UpdateFlags.Text;
					}
					if (this.IconImage != null)
					{
						this.updateFlags |= Button.UpdateFlags.Icon;
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
				if (base.Height != value)
				{
					base.Height = value;
					this.updateFlags |= Button.UpdateFlags.Background;
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= Button.UpdateFlags.Text;
					}
					if (this.IconImage != null)
					{
						this.updateFlags |= Button.UpdateFlags.Icon;
					}
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this.State != Button.ButtonState.Disabled;
			}
			set
			{
				if (value)
				{
					if (this.State == Button.ButtonState.Disabled)
					{
						this.State = Button.ButtonState.Normal;
						return;
					}
				}
				else
				{
					this.State = Button.ButtonState.Disabled;
				}
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text != value)
				{
					this.text = value;
					this.updateFlags |= Button.UpdateFlags.Text;
				}
			}
		}

		public ButtonStyle Style
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
					this.updateFlags |= Button.UpdateFlags.Background;
				}
			}
		}

		public ImageAsset IconImage
		{
			get
			{
				return this.iconImage;
			}
			set
			{
				if (this.iconImage != value)
				{
					this.iconImage = value;
					this.updateFlags |= Button.UpdateFlags.Icon;
				}
			}
		}

		public Font TextFont
		{
			get
			{
				return this.textFont;
			}
			set
			{
				if (this.textFont != value)
				{
					this.textFont = value;
					this.updateFlags |= Button.UpdateFlags.Text;
				}
			}
		}

		public TextTrimming TextTrimming
		{
			get
			{
				return this.textTrimming;
			}
			set
			{
				if (this.textTrimming != value)
				{
					this.textTrimming = value;
					this.updateFlags |= Button.UpdateFlags.Text;
				}
			}
		}

		public UIColor TextColor
		{
			get
			{
				return this.textColor;
			}
			set
			{
				if (this.textColor.R != value.R || this.textColor.G != value.G || this.textColor.B != value.B || this.textColor.A != value.A)
				{
					this.textColor = value;
					this.updateFlags |= Button.UpdateFlags.Text;
				}
			}
		}

		public TextShadowSettings TextShadow
		{
			get
			{
				return this.textShadow;
			}
			set
			{
				if (this.textShadow != value)
				{
					if (this.textShadow != null)
					{
						TextShadowSettings expr_17 = this.textShadow;
						expr_17.ValueChanged = (TextShadowSettings.TextShadowSettingsChanged)Delegate.Remove(expr_17.ValueChanged, new TextShadowSettings.TextShadowSettingsChanged(this.textShadowChanged));
					}
					this.textShadow = value;
					if (this.textShadow != null)
					{
						TextShadowSettings expr_4D = this.textShadow;
						expr_4D.ValueChanged = (TextShadowSettings.TextShadowSettingsChanged)Delegate.Combine(expr_4D.ValueChanged, new TextShadowSettings.TextShadowSettingsChanged(this.textShadowChanged));
					}
					this.updateFlags |= Button.UpdateFlags.Text;
				}
			}
		}

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignment;
			}
			set
			{
				if (this.horizontalAlignment != value)
				{
					this.horizontalAlignment = value;
					this.updateFlags |= (Button.UpdateFlags.Text | Button.UpdateFlags.Icon);
				}
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return this.verticalAlignment;
			}
			set
			{
				if (this.verticalAlignment != value)
				{
					this.verticalAlignment = value;
					this.updateFlags |= (Button.UpdateFlags.Text | Button.UpdateFlags.Icon);
				}
			}
		}

		public CustomButtonImageSettings CustomImage
		{
			get
			{
				return this.customImage;
			}
			set
			{
				if (this.customImage != value)
				{
					if (this.customImage != null)
					{
						CustomButtonImageSettings expr_17 = this.customImage;
						expr_17.ValueChanged = (CustomButtonImageSettings.CustomButtonImageSettingsChanged)Delegate.Remove(expr_17.ValueChanged, new CustomButtonImageSettings.CustomButtonImageSettingsChanged(this.customImageChanged));
					}
					this.customImage = value;
					if (this.customImage != null)
					{
						this.customImageChanged();
						CustomButtonImageSettings expr_53 = this.customImage;
						expr_53.ValueChanged = (CustomButtonImageSettings.CustomButtonImageSettingsChanged)Delegate.Combine(expr_53.ValueChanged, new CustomButtonImageSettings.CustomButtonImageSettingsChanged(this.customImageChanged));
					}
					this.updateFlags |= Button.UpdateFlags.Background;
				}
			}
		}

		private Button.ButtonState State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					if (this.state == Button.ButtonState.Disabled || value == Button.ButtonState.Disabled)
					{
						this.updateFlags |= (Button.UpdateFlags.Background | Button.UpdateFlags.Text | Button.UpdateFlags.Icon);
					}
					else
					{
						this.updateFlags |= Button.UpdateFlags.Background;
					}
					this.state = value;
				}
			}
		}

		public UIColor BackgroundFilterColor
		{
			get
			{
				return this.backgroundFilterColor;
			}
			set
			{
				this.backgroundFilterColor = value;
				for (int i = 0; i < this.backgroundPrim.VertexCount; i++)
				{
					UIPrimitiveVertex vertex = this.backgroundPrim.GetVertex(i);
					vertex.Color = value;
				}
				this.updateFlags |= Button.UpdateFlags.Background;
			}
		}

		[Obsolete("Use BackgroundFilterColor")]
		public UIColor BackgroundColor
		{
			get
			{
				return this.BackgroundFilterColor;
			}
			set
			{
				this.BackgroundFilterColor = value;
			}
		}

		public Button()
		{
			this.backgroundPrim = new UIPrimitive((DrawMode)4, 16, 28);
			base.RootUIElement.AddChildLast(this.backgroundPrim);
			this.backgroundPrim.ShaderType = ShaderType.Texture;
			this.iconSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.iconSprt);
			this.iconSprt.ShaderType = ShaderType.SolidFill;
			this.iconSprt.Visible = false;
			this.textSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.textSprt);
			this.textSprt.ShaderType = ShaderType.TextTexture;
			this.textSprt.Visible = false;
			this.State = Button.ButtonState.Normal;
			this.updateFlags = Button.UpdateFlags.Background;
			this.backgroundImages = new ImageAsset[Enum.GetValues(typeof(ButtonStyle)).Length, Enum.GetValues(typeof(Button.ButtonState)).Length];
			this.backgroundImages[0, 0] = new ImageAsset(SystemImageAsset.ButtonBackgroundNormal);
			this.backgroundImages[0, 1] = new ImageAsset(SystemImageAsset.ButtonBackgroundPressed);
			this.backgroundImages[0, 2] = new ImageAsset(SystemImageAsset.ButtonBackgroundDisabled);
			this.backgroundNinePatchs = new NinePatchMargin[Enum.GetValues(typeof(ButtonStyle)).Length];
			this.backgroundNinePatchs[0] = AssetManager.GetNinePatchMargin(SystemImageAsset.ButtonBackgroundNormal);
			this.CustomImage = null;
			this.IconImage = null;
			this.Text = "";
			this.TextFont = UISystem.DefaultFont;
			this.TextTrimming = TextTrimming.EllipsisCharacter;
			this.TextColor = new UIColor(1f, 1f, 1f, 1f);
			this.TextShadow = null;
			this.HorizontalAlignment = HorizontalAlignment.Center;
			this.VerticalAlignment = VerticalAlignment.Middle;
			this.Style = ButtonStyle.Default;
			this.Enabled = true;
			this.Width = 214f;
			this.Height = 56f;
			this.PriorityHit = true;
		}

		protected override void DisposeSelf()
		{
			for (int i = 0; i < this.backgroundImages.GetLength(1); i++)
			{
				if (this.backgroundImages[0, i] != null)
				{
					this.backgroundImages[0, i].Dispose();
				}
			}
			base.DisposeSelf();
		}

		private void textShadowChanged()
		{
			this.updateFlags |= Button.UpdateFlags.Text;
		}

		private void customImageChanged()
		{
			this.backgroundImages[1, 0] = this.customImage.BackgroundNormalImage;
			this.backgroundImages[1, 1] = this.customImage.BackgroundPressedImage;
			this.backgroundImages[1, 2] = this.customImage.BackgroundDisabledImage;
			this.backgroundNinePatchs[1] = this.customImage.BackgroundNinePatchMargin;
			this.updateFlags |= Button.UpdateFlags.Background;
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (this.State == Button.ButtonState.Disabled)
			{
				return;
			}
			switch (touchEvents.PrimaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.State == Button.ButtonState.Pressed && this.ButtonAction != null)
				{
					this.ButtonAction.Invoke(this, new TouchEventArgs(touchEvents));
				}
				if (this.Enabled)
				{
					this.State = Button.ButtonState.Normal;
					return;
				}
				break;
			case TouchEventType.Down:
				this.State = Button.ButtonState.Pressed;
				return;
			case TouchEventType.Move:
				break;
			case TouchEventType.Enter:
				this.State = Button.ButtonState.Pressed;
				return;
			case TouchEventType.Leave:
				this.State = Button.ButtonState.Normal;
				break;
			default:
				return;
			}
		}

		protected internal override void OnResetState()
		{
			base.OnResetState();
			if (this.State != Button.ButtonState.Disabled)
			{
				this.State = Button.ButtonState.Normal;
			}
		}

		protected override void OnUpdate(float elapsedTime)
		{
			if (this.Width != 0f && this.Height != 0f)
			{
				this.UpdateBackgroundSprite();
				this.UpdateTextSprite();
				this.UpdateIconSprite();
			}
			base.OnUpdate(elapsedTime);
		}

		private void UpdateBackgroundSprite()
		{
			if ((this.updateFlags & Button.UpdateFlags.Background) == Button.UpdateFlags.Background)
			{
				this.updateFlags &= ~Button.UpdateFlags.Background;
				ImageAsset imageAsset = this.backgroundImages[(int)this.Style, (int)this.State];
				if (imageAsset == null)
				{
					this.backgroundPrim.Image = null;
					this.backgroundPrim.Visible = true;
					return;
				}
				if (imageAsset.Ready)
				{
					this.backgroundPrim.Image = imageAsset;
					this.backgroundPrim.VertexCount = 9;
					this.backgroundPrim.Visible = true;
					UIPrimitiveUtility.SetupNinePatch(this.backgroundPrim, this.Width, this.Height, 0f, 0f, this.backgroundNinePatchs[(int)this.Style]);
					return;
				}
				this.backgroundPrim.Visible = false;
				this.updateFlags |= Button.UpdateFlags.Background;
			}
		}

		private void UpdateTextSprite()
		{
			if (string.IsNullOrEmpty(this.Text))
			{
				this.textSprt.Visible = false;
				this.updateFlags &= ~Button.UpdateFlags.Text;
			}
			if ((this.updateFlags & Button.UpdateFlags.Text) == Button.UpdateFlags.Text)
			{
				TextRenderHelper textRenderHelper = new TextRenderHelper();
				textRenderHelper.LineBreak = LineBreak.AtCode;
				textRenderHelper.HorizontalAlignment = this.HorizontalAlignment;
				textRenderHelper.VerticalAlignment = this.VerticalAlignment;
				textRenderHelper.Font = this.TextFont;
				textRenderHelper.TextTrimming = this.TextTrimming;
				UISpriteUnit unit = this.textSprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				unit.Color = this.TextColor;
				if (this.textSprt.Image != null)
				{
					this.textSprt.Image.Dispose();
				}
				this.textSprt.Image = textRenderHelper.DrawText(ref this.text, (int)unit.Width, (int)unit.Height);
				this.textSprt.Alpha = ((this.State == Button.ButtonState.Disabled) ? 0.3f : 1f);
				this.textSprt.ShaderType = ShaderType.TextTexture;
				this.textSprt.Visible = true;
				this.iconSprt.Visible = false;
				if (this.TextShadow != null)
				{
					this.textSprt.InternalShaderType = InternalShaderType.TextureAlphaShadow;
					this.textSprt.ShaderUniforms["u_ShadowColor"] = new float[]
					{
						this.TextShadow.Color.R,
						this.TextShadow.Color.G,
						this.TextShadow.Color.B,
						this.TextShadow.Color.A
					};
					this.textSprt.ShaderUniforms["u_ShadowOffset"] = new float[]
					{
						this.TextShadow.HorizontalOffset / (float)this.textSprt.Image.Width,
						this.TextShadow.VerticalOffset / (float)this.textSprt.Image.Height
					};
				}
				this.updateFlags &= ~Button.UpdateFlags.Text;
			}
		}

		private void UpdateIconSprite()
		{
			if ((this.updateFlags & Button.UpdateFlags.Icon) == Button.UpdateFlags.Icon)
			{
				this.updateFlags &= ~Button.UpdateFlags.Icon;
				if (this.IconImage == null)
				{
					this.iconSprt.Visible = false;
					return;
				}
				if (this.IconImage.Ready)
				{
					UISpriteUnit unit = this.iconSprt.GetUnit(0);
					unit.Width = (float)this.IconImage.Width;
					unit.Height = (float)this.IconImage.Height;
					switch (this.HorizontalAlignment)
					{
					case HorizontalAlignment.Left:
						unit.X = 0f;
						break;
					case HorizontalAlignment.Center:
						unit.X = (this.Width - unit.Width) / 2f;
						break;
					case HorizontalAlignment.Right:
						unit.X = this.Width - unit.Width;
						break;
					}
					switch (this.VerticalAlignment)
					{
					case VerticalAlignment.Top:
						unit.Y = 0f;
						break;
					case VerticalAlignment.Middle:
						unit.Y = (this.Height - unit.Height) / 2f;
						break;
					case VerticalAlignment.Bottom:
						unit.Y = this.Height - unit.Height;
						break;
					}
					this.iconSprt.ShaderType = ShaderType.Texture;
					this.iconSprt.Image = this.IconImage;
					this.iconSprt.Visible = true;
					this.textSprt.Visible = false;
					return;
				}
				this.iconSprt.Visible = false;
				this.textSprt.Visible = false;
				this.updateFlags |= Button.UpdateFlags.Icon;
			}
		}
	}
}
