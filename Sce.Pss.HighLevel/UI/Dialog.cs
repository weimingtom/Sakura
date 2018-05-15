using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class Dialog : ContainerWidget
	{
		private Effect showEffect;

		private Effect hideEffect;

		private DialogBackgroundStyle backgroundStyle;

		private ImageAsset customBackgroundImage;

		private NinePatchMargin customBackgroundNinePatchMargin;

		private UIColor customBackgroundColor = default(UIColor);

		private UIColor backgroundFilterColor = new UIColor(1f, 1f, 1f, 1f);

		private bool hideOnTouchOutside;

		private bool needUpdateSprite = true;

		private UISprite bgImageSprt;

		private UISprite bgColorSprt;

		private ImageAsset defaultImageAsset;

		public event EventHandler Showing;

		public event EventHandler Shown;

		public event EventHandler Hiding;

		public event EventHandler Hidden;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				this.needUpdateSprite = true;
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
				base.Height = value;
				this.needUpdateSprite = true;
			}
		}

		public Effect ShowEffect
		{
			get
			{
				return this.showEffect;
			}
			set
			{
				if (this.showEffect != null)
				{
					this.showEffect.EffectStopped -= new EventHandler<EventArgs>(this.ShowEffectStopped);
				}
				this.showEffect = value;
				if (this.showEffect != null)
				{
					this.showEffect.EffectStopped += new EventHandler<EventArgs>(this.ShowEffectStopped);
				}
			}
		}

		public Effect HideEffect
		{
			get
			{
				return this.hideEffect;
			}
			set
			{
				if (this.hideEffect != null)
				{
					this.hideEffect.EffectStopped -= new EventHandler<EventArgs>(this.HideEffectStopped);
				}
				this.hideEffect = value;
				if (this.hideEffect != null)
				{
					this.hideEffect.EffectStopped += new EventHandler<EventArgs>(this.HideEffectStopped);
				}
			}
		}

		public DialogBackgroundStyle BackgroundStyle
		{
			get
			{
				return this.backgroundStyle;
			}
			set
			{
				this.backgroundStyle = value;
				this.needUpdateSprite = true;
			}
		}

		public ImageAsset CustomBackgroundImage
		{
			get
			{
				return this.customBackgroundImage;
			}
			set
			{
				this.customBackgroundImage = value;
				this.needUpdateSprite = true;
			}
		}

		public NinePatchMargin CustomBackgroundNinePatchMargin
		{
			get
			{
				return this.customBackgroundNinePatchMargin;
			}
			set
			{
				this.customBackgroundNinePatchMargin = value;
				this.needUpdateSprite = true;
			}
		}

		public UIColor CustomBackgroundColor
		{
			get
			{
				return this.customBackgroundColor;
			}
			set
			{
				this.customBackgroundColor = value;
				this.needUpdateSprite = true;
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
				this.needUpdateSprite = true;
			}
		}

		public bool HideOnTouchOutside
		{
			get
			{
				return this.hideOnTouchOutside;
			}
			set
			{
				this.hideOnTouchOutside = value;
			}
		}

		public Dialog()
		{
			this.init(new BunjeeJumpEffect(), new TiltDropEffect());
		}

		public Dialog(Effect showEffect, Effect hideEffect)
		{
			this.init(showEffect, hideEffect);
		}

		private void init(Effect showEffect, Effect hideEffect)
		{
			this.Width = (float)UISystem.FramebufferWidth;
			this.Height = (float)UISystem.FramebufferHeight;
			this.bgColorSprt = new UISprite(1);
			this.bgColorSprt.ShaderType = ShaderType.SolidFill;
			base.RootUIElement.AddChildLast(this.bgColorSprt);
			this.bgImageSprt = new UISprite(9);
			this.bgImageSprt.ShaderType = ShaderType.Texture;
			base.RootUIElement.AddChildLast(this.bgImageSprt);
			this.ShowEffect = showEffect;
			this.HideEffect = hideEffect;
			this.needUpdateSprite = true;
		}

		protected override void DisposeSelf()
		{
			if (this.defaultImageAsset != null)
			{
				this.defaultImageAsset.Dispose();
			}
			base.DisposeSelf();
		}

		private void updateSprite()
		{
			if (this.bgImageSprt != null && this.bgColorSprt != null)
			{
				this.needUpdateSprite = false;
				if (this.backgroundStyle == DialogBackgroundStyle.Default)
				{
					this.bgColorSprt.Visible = false;
					this.bgImageSprt.Visible = true;
					if (this.defaultImageAsset == null)
					{
						this.defaultImageAsset = new ImageAsset(SystemImageAsset.DialogBackground);
					}
					this.bgImageSprt.Image = this.defaultImageAsset;
					UISpriteUtility.SetupNinePatch(this.bgImageSprt, this.Width, this.Height, 0f, 0f, AssetManager.GetNinePatchMargin(SystemImageAsset.DialogBackground));
				}
				else
				{
					UISpriteUnit unit = this.bgColorSprt.GetUnit(0);
					unit.Color = this.customBackgroundColor;
					unit.SetSize(this.Width, this.Height);
					this.bgColorSprt.Visible = true;
					if (this.customBackgroundImage == null)
					{
						this.bgImageSprt.Image = null;
						this.bgImageSprt.Visible = false;
					}
					else if (this.customBackgroundImage.Ready)
					{
						this.bgImageSprt.Image = this.customBackgroundImage;
						this.bgImageSprt.Visible = true;
						UISpriteUtility.SetupNinePatch(this.bgImageSprt, this.Width, this.Height, 0f, 0f, this.customBackgroundNinePatchMargin);
					}
					else
					{
						this.bgImageSprt.Visible = false;
						this.needUpdateSprite = true;
					}
				}
				for (int i = 0; i < this.bgImageSprt.UnitCount; i++)
				{
					UISpriteUnit unit2 = this.bgImageSprt.GetUnit(i);
					unit2.Color = this.backgroundFilterColor;
				}
			}
		}

		public void Show()
		{
			UISystem.IsDialogEffect = true;
			if (this.Showing != null)
			{
				this.Showing.Invoke(this, EventArgs.Empty);
			}
			UISystem.PushModalWidget(this);
			this.Transform3D = Matrix4.Identity;
			this.PivotType = PivotType.TopLeft;
			this.X = ((float)UISystem.FramebufferWidth - this.Width) / 2f;
			this.Y = ((float)UISystem.FramebufferHeight - this.Height) / 2f;
			this.Alpha = 1f;
			this.Visible = true;
			if (this.ShowEffect == null)
			{
				this.ShowEffectStopped(this, null);
				return;
			}
			base.ResetState(false);
			this.ShowEffect.Widget = this;
			this.ShowEffect.Start();
		}

		public void Show(Effect effect)
		{
			this.ShowEffect = effect;
			this.Show();
		}

		private void ShowEffectStopped(object sender, EventArgs e)
		{
			if (this.Shown != null)
			{
				this.Shown.Invoke(this, EventArgs.Empty);
			}
			UISystem.IsDialogEffect = false;
		}

		public void Hide()
		{
			UISystem.IsDialogEffect = true;
			if (this.Hiding != null)
			{
				this.Hiding.Invoke(this, EventArgs.Empty);
			}
			if (this.HideEffect == null)
			{
				this.HideEffectStopped(this, null);
				return;
			}
			base.ResetState(false);
			this.HideEffect.Widget = this;
			this.HideEffect.Start();
		}

		public void Hide(Effect effect)
		{
			this.HideEffect = effect;
			this.Hide();
		}

		private void HideEffectStopped(object sender, EventArgs e)
		{
			this.Visible = false;
			this.Transform3D = Matrix4.Identity;
			UISystem.PopModalWidget();
			if (this.Hidden != null)
			{
				this.Hidden.Invoke(this, EventArgs.Empty);
			}
			UISystem.IsDialogEffect = false;
		}

		protected internal override void Render()
		{
			if (this.needUpdateSprite)
			{
				this.updateSprite();
			}
			base.Render();
		}
	}
}
