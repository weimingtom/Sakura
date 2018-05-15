using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public sealed class ImageAsset : IDisposable
	{
		private Texture2D unsharedTexture;

		private string filename;

		private int width;

		private int height;

		public bool Ready
		{
			get
			{
				if (this.filename != null)
				{
					return AssetManager.IsLoadedTexture(this.filename);
				}
				return this.unsharedTexture != null;
			}
		}

		internal bool AdjustScaledSize
		{
			get;
			set;
		}

		public int Width
		{
			get
			{
				if (this.width <= 0)
				{
					ImageSize imageSize = AssetManager.GetImageSize(this.filename);
					this.width = imageSize.Width;
					this.height = imageSize.Height;
				}
				if (UISystem.Scaled && this.AdjustScaledSize)
				{
					return (int)((float)this.width / UISystem.Scale);
				}
				return this.width;
			}
		}

		public int Height
		{
			get
			{
				if (this.height <= 0 && this.filename != null)
				{
					ImageSize imageSize = AssetManager.GetImageSize(this.filename);
					this.width = imageSize.Width;
					this.height = imageSize.Height;
				}
				if (UISystem.Scaled && this.AdjustScaledSize)
				{
					return (int)((float)this.height / UISystem.Scale);
				}
				return this.height;
			}
		}

		public ImageAsset(string filename, bool asyncLoad)
		{
			this.filename = filename;
			AssetManager.LoadTexture(filename, asyncLoad);
		}

		public ImageAsset(string filename) : this(filename, false)
		{
		}

		public ImageAsset(SystemImageAsset name)
		{
			this.filename = AssetManager.GetSystemFileName(name);
			AssetManager.LoadTextureFromAssembly(this.filename, true);
			ImageSize imageSize = AssetManager.GetImageSize(name);
			this.width = imageSize.Width;
			this.height = imageSize.Height;
		}

		public ImageAsset(Texture2D texture)
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}
			this.unsharedTexture = texture.ShallowCopy();
			this.width = texture.Width;
			this.height = texture.Height;
		}

		public ImageAsset(Image image, PixelFormat format)
		{
			this.unsharedTexture = new Texture2D(image.Size.Width, image.Size.Height, false, format);
			byte[] array = image.ToBuffer();
			this.unsharedTexture.SetPixels(0, array);
			this.width = this.unsharedTexture.Width;
			this.height = this.unsharedTexture.Height;
		}

		public Texture2D CloneTexture()
		{
			Texture2D texture = this.getTexture();
			if (texture != null)
			{
				return texture.ShallowCopy();
			}
			return null;
		}

		private Texture2D getTexture()
		{
			if (this.filename != null)
			{
				return AssetManager.GetTexture(this.filename);
			}
			return this.unsharedTexture;
		}

		public void WaitForLoad()
		{
			if (this.filename != null)
			{
				AssetManager.WaitForLoad(this.filename);
			}
		}

		public void Dispose()
		{
			if (this.unsharedTexture != null)
			{
				this.unsharedTexture.Dispose();
				this.unsharedTexture = null;
			}
		}

		public bool UnloadFromCache()
		{
			return this.filename != null && AssetManager.UnloadTexture(this.filename);
		}

		public static void WaitForLoadAll()
		{
			AssetManager.WaitForLoadAll();
		}

		public static bool UnloadFromCache(string filename)
		{
			return AssetManager.UnloadTexture(filename);
		}
	}
}
