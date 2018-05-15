using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TextShadowSettings
	{
		internal delegate void TextShadowSettingsChanged();

		internal TextShadowSettings.TextShadowSettingsChanged ValueChanged;

		private UIColor color;

		private float horizontalOffset;

		private float verticalOffset;

		public UIColor Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public float HorizontalOffset
		{
			get
			{
				return this.horizontalOffset;
			}
			set
			{
				this.horizontalOffset = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public float VerticalOffset
		{
			get
			{
				return this.verticalOffset;
			}
			set
			{
				this.verticalOffset = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public TextShadowSettings()
		{
			this.color = new UIColor(0.5f, 0.5f, 0.5f, 0.5f);
			this.horizontalOffset = 2f;
			this.verticalOffset = 2f;
			this.ValueChanged = null;
		}
	}
}
