using System;

namespace Sce.Pss.HighLevel.UI
{
	public class CustomButtonImageSettings
	{
		internal delegate void CustomButtonImageSettingsChanged();

		internal CustomButtonImageSettings.CustomButtonImageSettingsChanged ValueChanged;

		private ImageAsset backgroundNormalImage;

		private ImageAsset backgroundPressedImage;

		private ImageAsset backgroundDisabledImage;

		private NinePatchMargin backgroundNinePatchMargin;

		public ImageAsset BackgroundNormalImage
		{
			get
			{
				return this.backgroundNormalImage;
			}
			set
			{
				this.backgroundNormalImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset BackgroundPressedImage
		{
			get
			{
				return this.backgroundPressedImage;
			}
			set
			{
				this.backgroundPressedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset BackgroundDisabledImage
		{
			get
			{
				return this.backgroundDisabledImage;
			}
			set
			{
				this.backgroundDisabledImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public NinePatchMargin BackgroundNinePatchMargin
		{
			get
			{
				return this.backgroundNinePatchMargin;
			}
			set
			{
				this.backgroundNinePatchMargin = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public CustomButtonImageSettings()
		{
			this.backgroundNormalImage = null;
			this.backgroundPressedImage = null;
			this.backgroundDisabledImage = null;
			this.backgroundNinePatchMargin = NinePatchMargin.Zero;
			this.ValueChanged = null;
		}
	}
}
