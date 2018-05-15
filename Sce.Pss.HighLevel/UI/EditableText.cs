using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class EditableText : Widget
	{
		[Flags]
		private enum UpdateFlags
		{
			Background = 1,
			Text = 2
		}

		private const float defaultEditableTextWidth = 360f;

		private const float defaultEditableTextHeight = 56f;

		private const float textVerticalOffset = 4f;

		private const float textHorizontalOffset = 10f;

		private string text;

		private Font font;

		private UIColor textColor;

		private string defaultText;

		private Font defaultFont;

		private UIColor defaultTextColor;

		private TextShadowSettings textShadow;

		private HorizontalAlignment horizontalAlignment;

		private VerticalAlignment verticalAlignment;

		private LineBreak lineBreak;

		private TextTrimming textTrimming;

		private float lineGap;

		private TextInputMode textInputMode;

		private UISprite textSprt;

		private UISprite backgroundSprt;

		private NinePatchMargin backgroundNinePatchMargin;

		private TextInputDialog dialog;

		private EditableText.UpdateFlags updateFlags;

		public event EventHandler<TextChangedEventArgs> TextChanged;

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
					this.updateFlags = (EditableText.UpdateFlags.Background | EditableText.UpdateFlags.Text);
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
					this.updateFlags = (EditableText.UpdateFlags.Background | EditableText.UpdateFlags.Text);
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
					TextChangedEventArgs textChangedEventArgs = new TextChangedEventArgs(this.text, value);
					this.text = value;
					this.updateFlags |= EditableText.UpdateFlags.Text;
					if (this.TextChanged != null)
					{
						this.TextChanged.Invoke(this, textChangedEventArgs);
					}
				}
			}
		}

		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				if (this.font != value)
				{
					this.font = value;
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= EditableText.UpdateFlags.Text;
					}
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
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= EditableText.UpdateFlags.Text;
					}
				}
			}
		}

		public string DefaultText
		{
			get
			{
				return this.defaultText;
			}
			set
			{
				if (this.defaultText != value)
				{
					this.defaultText = value;
					if (string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= EditableText.UpdateFlags.Text;
					}
				}
			}
		}

		public Font DefaultFont
		{
			get
			{
				return this.defaultFont;
			}
			set
			{
				if (this.defaultFont != value)
				{
					this.defaultFont = value;
					if (string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= EditableText.UpdateFlags.Text;
					}
				}
			}
		}

		public UIColor DefaultTextColor
		{
			get
			{
				return this.defaultTextColor;
			}
			set
			{
				if (this.defaultTextColor.R != value.R || this.defaultTextColor.G != value.G || this.defaultTextColor.B != value.B || this.defaultTextColor.A != value.A)
				{
					this.defaultTextColor = value;
					if (string.IsNullOrEmpty(this.Text))
					{
						this.updateFlags |= EditableText.UpdateFlags.Text;
					}
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
					this.textShadow = value;
					this.updateFlags |= EditableText.UpdateFlags.Text;
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
					this.updateFlags |= EditableText.UpdateFlags.Text;
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
					this.updateFlags |= EditableText.UpdateFlags.Text;
				}
			}
		}

		public LineBreak LineBreak
		{
			get
			{
				return this.lineBreak;
			}
			set
			{
				if (this.lineBreak != value)
				{
					this.lineBreak = value;
					this.updateFlags |= EditableText.UpdateFlags.Text;
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
					this.updateFlags |= EditableText.UpdateFlags.Text;
				}
			}
		}

		public float LineGap
		{
			get
			{
				return this.lineGap;
			}
			set
			{
				if (this.lineGap != value)
				{
					this.lineGap = value;
					this.updateFlags |= EditableText.UpdateFlags.Text;
				}
			}
		}

		public TextInputMode TextInputMode
		{
			get
			{
				return this.textInputMode;
			}
			set
			{
				if (this.textInputMode != value)
				{
					this.textInputMode = value;
					this.updateFlags |= EditableText.UpdateFlags.Text;
				}
			}
		}

		public EditableText()
		{
			this.backgroundSprt = new UISprite(9);
			base.RootUIElement.AddChildLast(this.backgroundSprt);
			this.backgroundSprt.ShaderType = ShaderType.Texture;
			this.backgroundSprt.Image = new ImageAsset(SystemImageAsset.EditableTextBackgroundNormal);
			this.backgroundNinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.EditableTextBackgroundNormal);
			this.textSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.textSprt);
			this.textSprt.ShaderType = ShaderType.TextTexture;
			this.DefaultText = "Please input the text.";
			this.DefaultFont = UISystem.DefaultFont;
			this.DefaultTextColor = new UIColor(0.75f, 0.75f, 0.75f, 0.75f);
			this.Text = "";
			this.Font = this.DefaultFont;
			this.TextColor = TextRenderHelper.DefaultTextColor;
			this.TextShadow = null;
			this.HorizontalAlignment = HorizontalAlignment.Left;
			this.VerticalAlignment = VerticalAlignment.Middle;
			this.LineBreak = LineBreak.Character;
			this.TextTrimming = TextTrimming.EllipsisCharacter;
			this.LineGap = 0f;
			this.Width = 360f;
			this.Height = 56f;
			this.PriorityHit = true;
			this.TextInputMode = (TextInputMode)0;
			this.updateFlags = (EditableText.UpdateFlags.Background | EditableText.UpdateFlags.Text);
		}

		protected override void DisposeSelf()
		{
			if (this.backgroundSprt != null && this.backgroundSprt.Image != null)
			{
				this.backgroundSprt.Image.Dispose();
			}
			base.DisposeSelf();
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (touchEvents.PrimaryTouchEvent.Type == TouchEventType.Down)
			{
				this.dialog = new TextInputDialog();
				this.dialog.Mode = this.TextInputMode;
				this.dialog.Text = this.Text;
				this.dialog.Open();
			}
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (this.dialog != null && this.dialog.State == (CommonDialogState)2)
			{
				if (this.dialog.Result == (CommonDialogResult)0)
				{
					this.Text = this.dialog.Text;
				}
				this.dialog.Dispose();
				this.dialog = null;
			}
		}

		protected internal override void Render()
		{
			if (this.Width != 0f && this.Height != 0f)
			{
				this.UpdateBackgroundSprite();
				this.UpdateTextSprite();
			}
			this.updateFlags = (EditableText.UpdateFlags)0;
			base.Render();
		}

		private void UpdateBackgroundSprite()
		{
			if ((this.updateFlags & EditableText.UpdateFlags.Background) == EditableText.UpdateFlags.Background)
			{
				UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				UISpriteUtility.SetupNinePatch(this.backgroundSprt, this.Width, this.Height, 0f, 0f, this.backgroundNinePatchMargin);
				this.updateFlags &= ~EditableText.UpdateFlags.Background;
			}
		}

		private void UpdateTextSprite()
		{
			if ((this.updateFlags & EditableText.UpdateFlags.Text) == EditableText.UpdateFlags.Text)
			{
				UISpriteUnit unit = this.textSprt.GetUnit(0);
				unit.X = 10f;
				unit.Y = 4f;
				unit.Width = this.Width - 20f;
				unit.Height = this.Height - 8f;
				TextRenderHelper textRenderHelper = new TextRenderHelper();
				textRenderHelper.HorizontalAlignment = this.HorizontalAlignment;
				textRenderHelper.VerticalAlignment = this.VerticalAlignment;
				textRenderHelper.LineBreak = this.LineBreak;
				textRenderHelper.TextTrimming = this.TextTrimming;
				textRenderHelper.LineGap = this.LineGap;
				if (string.IsNullOrEmpty(this.Text))
				{
					unit.Color = this.DefaultTextColor;
					textRenderHelper.Font = this.DefaultFont;
					this.textSprt.ShaderType = ShaderType.TextTexture;
					if (this.textSprt.Image != null)
					{
						this.textSprt.Image.Dispose();
					}
					this.textSprt.Image = textRenderHelper.DrawText(ref this.defaultText, (int)unit.Width, (int)unit.Height);
				}
				else
				{
					string text;
					if (this.TextInputMode == (TextInputMode)2)
					{
						text = new string('*', this.Text.Length);
					}
					else
					{
						text = this.Text;
					}
					unit.Color = this.TextColor;
					textRenderHelper.Font = this.Font;
					this.textSprt.ShaderType = ShaderType.TextTexture;
					if (this.textSprt.Image != null)
					{
						this.textSprt.Image.Dispose();
					}
					this.textSprt.Image = textRenderHelper.DrawText(ref text, (int)unit.Width, (int)unit.Height);
				}
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
				this.updateFlags &= ~EditableText.UpdateFlags.Text;
			}
		}
	}
}
