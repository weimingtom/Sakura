using System;

namespace Sce.Pss.HighLevel.UI
{
	public class CustomCheckBoxImageSettings
	{
		internal delegate void CustomCheckBoxImageSettingsChanged();

		internal CustomCheckBoxImageSettings.CustomCheckBoxImageSettingsChanged ValueChanged;

		private ImageAsset normalUncheckedImage;

		private ImageAsset normalCheckedImage;

		private ImageAsset pressedUncheckedImage;

		private ImageAsset pressedCheckedImage;

		private ImageAsset disabledUncheckedImage;

		private ImageAsset disabledCheckedImage;

		public ImageAsset NormalUncheckedImage
		{
			get
			{
				return this.normalUncheckedImage;
			}
			set
			{
				this.normalUncheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset NormalCheckedImage
		{
			get
			{
				return this.normalCheckedImage;
			}
			set
			{
				this.normalCheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset PressedUncheckedImage
		{
			get
			{
				return this.pressedUncheckedImage;
			}
			set
			{
				this.pressedUncheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset PressedCheckedImage
		{
			get
			{
				return this.pressedCheckedImage;
			}
			set
			{
				this.pressedCheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset DisabledUncheckedImage
		{
			get
			{
				return this.disabledUncheckedImage;
			}
			set
			{
				this.disabledUncheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public ImageAsset DisabledCheckedImage
		{
			get
			{
				return this.disabledCheckedImage;
			}
			set
			{
				this.disabledCheckedImage = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged();
				}
			}
		}

		public CustomCheckBoxImageSettings()
		{
			this.NormalUncheckedImage = null;
			this.NormalCheckedImage = null;
			this.PressedUncheckedImage = null;
			this.PressedCheckedImage = null;
			this.DisabledUncheckedImage = null;
			this.DisabledCheckedImage = null;
			this.ValueChanged = null;
		}
	}
}
