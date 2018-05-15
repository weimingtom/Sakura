using Sce.Pss.Core.Imaging;
using System;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.UI
{
	public class Label : Widget
	{
		[Flags]
		private enum UpdateFlags
		{
			Background = 1,
			Text = 2
		}

		private const float defaultLabelWidth = 214f;

		private const float defaultLabelHeight = 27f;

		private string text;

		private Font font;

		private UIColor textColor;

		private TextShadowSettings textShadow;

		private UIColor backgroundColor;

		private HorizontalAlignment horizontalAlignment;

		private VerticalAlignment verticalAlignment;

		private LineBreak lineBreak;

		private TextTrimming textTrimming;

		private float lineGap;

		private UISprite backgroundSprt;

		private UISprite textSprt;

		private Label.UpdateFlags updateFlags;

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
					this.updateFlags = (Label.UpdateFlags.Background | Label.UpdateFlags.Text);
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
					this.updateFlags = (Label.UpdateFlags.Background | Label.UpdateFlags.Text);
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
				}
			}
		}

		public UIColor BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				if (this.backgroundColor.R != value.R || this.backgroundColor.G != value.G || this.backgroundColor.B != value.B || this.backgroundColor.A != value.A)
				{
					this.backgroundColor = value;
					this.updateFlags |= Label.UpdateFlags.Background;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
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
					this.updateFlags |= Label.UpdateFlags.Text;
				}
			}
		}

		public float TextHeight
		{
			get
			{
				return new TextRenderHelper
				{
					Font = this.Font,
					HorizontalAlignment = this.HorizontalAlignment,
					VerticalAlignment = this.VerticalAlignment,
					LineBreak = this.LineBreak,
					TextTrimming = this.TextTrimming,
					LineGap = this.LineGap
				}.GetTotalHeight(this.Text, this.Width);
			}
		}

		public Label()
		{
			this.backgroundSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.backgroundSprt);
			this.backgroundSprt.ShaderType = ShaderType.SolidFill;
			this.BackgroundColor = TextRenderHelper.DefaultBackgroundColor;
			this.textSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.textSprt);
			this.textSprt.ShaderType = ShaderType.TextTexture;
			this.Text = "";
			this.Font = UISystem.DefaultFont;
			this.TextColor = TextRenderHelper.DefaultTextColor;
			this.TextShadow = null;
			this.HorizontalAlignment = HorizontalAlignment.Left;
			this.VerticalAlignment = VerticalAlignment.Middle;
			this.LineBreak = LineBreak.Character;
			this.TextTrimming = TextTrimming.EllipsisCharacter;
			this.LineGap = 0f;
			this.Width = 214f;
			this.Height = 27f;
			this.updateFlags = (Label.UpdateFlags.Background | Label.UpdateFlags.Text);
		}

		protected internal override void Render()
		{
			if (this.Width != 0f && this.Height != 0f)
			{
				this.UpdateBackgroundSprite();
				if (string.IsNullOrEmpty(this.Text))
				{
					this.textSprt.Visible = false;
				}
				else
				{
					this.UpdateTextSprite();
				}
			}
			this.updateFlags = (Label.UpdateFlags)0;
//			Debug.WriteLine("=====================Render " + this.GetHashCode());
			base.Render();
		}

		private void UpdateBackgroundSprite()
		{
			if ((this.updateFlags & Label.UpdateFlags.Background) == Label.UpdateFlags.Background)
			{
				UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				unit.Color = this.BackgroundColor;
				this.updateFlags &= ~Label.UpdateFlags.Background;
			}
		}

		private void UpdateTextSprite()
		{
			if ((this.updateFlags & Label.UpdateFlags.Text) == Label.UpdateFlags.Text)
			{
				//FIXME:refresh text texture
				UISpriteUnit unit = this.textSprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				unit.Color = this.TextColor;
				TextRenderHelper textRenderHelper = new TextRenderHelper();
				textRenderHelper.Font = this.Font;
				textRenderHelper.HorizontalAlignment = this.HorizontalAlignment;
				textRenderHelper.VerticalAlignment = this.VerticalAlignment;
				textRenderHelper.LineBreak = this.LineBreak;
				textRenderHelper.TextTrimming = this.TextTrimming;
				textRenderHelper.LineGap = this.LineGap;
				this.textSprt.Visible = true;
				this.textSprt.ShaderType = ShaderType.TextTexture;
				this.textSprt.__name = "see UpdateTextSprite";
				if (this.textSprt.Image != null)
				{
					this.textSprt.Image.Dispose();
				}
				//FIXME:write text to memory bitmap
				this.textSprt.Image = textRenderHelper.DrawText(ref this.text, (int)unit.Width, (int)unit.Height);
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
				this.updateFlags &= ~Label.UpdateFlags.Text;
			}
		}
	}
}
