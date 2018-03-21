using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class GraphicsContextAlpha : IDisposable
	{
		public class DebugStats_
		{
			private uint m_current_frame = 0u;

			public uint DrawArraysCount = 0u;

			public void OnDrawArray()
			{
				if (this.m_current_frame != Common.FrameCount)
				{
					this.m_current_frame = Common.FrameCount;
					this.DrawArraysCount = 0u;
				}
				this.DrawArraysCount += 1u;
			}
		}

		private GraphicsContext m_context;

		private bool m_context_must_be_disposed;

		private Texture2D m_white_texture;

		private TextureInfo m_white_texture_info;

		private bool m_disposed = false;

		public MatrixStack ModelMatrix;

		public MatrixStack ViewMatrix;

		public MatrixStack ProjectionMatrix;

		public GraphicsContextAlpha.DebugStats_ DebugStats;

		public GraphicsContext Context
		{
			get
			{
				return this.m_context;
			}
		}

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public TextureInfo WhiteTextureInfo
		{
			get
			{
				return this.m_white_texture_info;
			}
		}

		public GraphicsContextAlpha(GraphicsContext context = null)
		{
			this.m_context = context;
			this.m_context_must_be_disposed = false;
			if (this.m_context == null)
			{
				this.m_context = new GraphicsContext();
				this.m_context_must_be_disposed = true;
			}
			this.ModelMatrix = new MatrixStack(16u);
			this.ViewMatrix = new MatrixStack(16u);
			this.ProjectionMatrix = new MatrixStack(8u);
			this.m_white_texture = GraphicsContextAlpha.CreateTextureUnicolor(4294967295u);
			this.m_white_texture_info = new TextureInfo(this.m_white_texture);
			this.DebugStats = new GraphicsContextAlpha.DebugStats_();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_context_must_be_disposed)
				{
					Common.DisposeAndNullify<GraphicsContext>(ref this.m_context);
				}
				Common.DisposeAndNullify<Texture2D>(ref this.m_white_texture);
				this.m_disposed = true;
			}
		}

		public Matrix4 GetMVP()
		{
			return this.ProjectionMatrix.Get() * this.ViewMatrix.Get() * this.ModelMatrix.Get();
		}

		public float GetAspect()
		{
			ImageRect viewport = this.Context.GetViewport();
			Common.Assert(viewport.Width != 0 && viewport.Height != 0);
			return (float)viewport.Width / (float)viewport.Height;
		}

		public Bounds2 GetViewportf()
		{
			ImageRect viewport = this.Context.GetViewport();
			return new Bounds2(new Vector2((float)viewport.X, (float)viewport.Y), new Vector2((float)(viewport.X + viewport.Width), (float)(viewport.Y + viewport.Height)));
		}

		public void SetDepthMask(bool write)
		{
			DepthFunc depthFunc = this.Context.GetDepthFunc();
			depthFunc.WriteMask = write;
			this.Context.SetDepthFunc(depthFunc);
		}

		public void SetBlendMode(BlendMode mode)
		{
			if (!mode.Enabled)
			{
				this.Context.Disable(EnableMode.Blend);//(EnableMode)4);
			}
			else
			{
				this.Context.Enable(EnableMode.Blend);//(EnableMode)4);
				this.Context.SetBlendFunc(mode.BlendFunc);
			}
		}

		public static Texture2D CreateTextureUnicolor(uint color)
		{
			int num = 16;
			int num2 = 16;
			uint[] array = new uint[num * num2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(num, num2, false, (PixelFormat)1);
			texture2D.SetPixels(0, array);
			return texture2D;
		}

		public static Texture2D CreateTextureFromFont(string text, Font font, uint color = 4294967295u)
		{
			int textWidth = font.GetTextWidth(text, 0, text.Length);
			int height = font.Metrics.Height;
			Image image = new Image(0, new ImageSize(textWidth, height), new ImageColor(0, 0, 0, 0));
			image.DrawText(text, new ImageColor((int)(color >> 16 & 255u), (int)(color >> 8 & 255u), (int)(color & 255u), (int)(color >> 24 & 255u)), font, new ImagePosition(0, 0));
			Texture2D texture2D = new Texture2D(textWidth, height, false, (PixelFormat)1);
			texture2D.SetPixels(0, image.ToBuffer());
			image.Dispose();
			return texture2D;
		}
	}
}
