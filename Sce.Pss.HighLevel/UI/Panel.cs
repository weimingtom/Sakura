using System;

namespace Sce.Pss.HighLevel.UI
{
	public class Panel : ContainerWidget
	{
		private UISprite sprt;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.sprt != null)
				{
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.Width = value;
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
				base.Height = value;
				if (this.sprt != null)
				{
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.Height = value;
				}
			}
		}

		public virtual UIColor BackgroundColor
		{
			get
			{
				UISpriteUnit unit = this.sprt.GetUnit(0);
				return unit.Color;
			}
			set
			{
				if (this.sprt != null)
				{
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.Color = value;
				}
			}
		}

		public new bool Clip
		{
			get
			{
				return base.Clip;
			}
			set
			{
				base.Clip = value;
			}
		}

		internal UISprite BackgroundUISprite
		{
			get
			{
				return this.sprt;
			}
		}

		public Panel()
		{
			this.sprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.sprt);
			this.sprt.ShaderType = ShaderType.SolidFill;
			this.Width = 0f;
			this.Height = 0f;
			this.BackgroundColor = new UIColor(0f, 0f, 0f, 0f);
			this.Clip = false;
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			touchEvents.Forward = true;
		}
	}
}
