using System;
using System.Diagnostics;

using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

using Sakura.OpenTK;

namespace Sce.Pss.Core.Graphics
{
	public class FrameBuffer : IDisposable
	{
		public int __framebufferId = -1;
		private int __width = 0;
		private int __height = 0;
		private int __textureIndex = 0;
		private Texture[] __textureArr = new Texture[100];
		
		private static FrameBuffer screenFrameBuffer;
		public static FrameBuffer __getScreen()
		{
			if (screenFrameBuffer == null)
			{
				screenFrameBuffer = new FrameBuffer(true);
			}
			return screenFrameBuffer;
		}
		
		private FrameBuffer(bool isScreen)
		{
			if (isScreen)
			{
				this.__framebufferId = (int)OpenTK.Graphics.ES20.All.None;
				this.__width = MyGameWindow.getWidth();
				this.__height = MyGameWindow.getHeight();
			}
		}
		
		public FrameBuffer()
		{
			this.__framebufferId = GL.GenFramebuffer();
			this.__width = 0;
			this.__height = 0;
		}
		
		public float AspectRatio
		{
			get
			{
				return (float)Width / (float)Height;//(float)this.state.width / (float)this.state.height;
			}
		}
		
		public int Width
		{
			get
			{
				return __width;//this.state.width;
			}
		}

		public int Height
		{
			get
			{
				return __height;//this.state.height;
			}
		}
		
		public void Dispose ()
		{
			
		}
		
		public void SetColorTarget (Texture2D texture, int level)
		{
			//Debug.Assert(false);
			//FIXME:????
			//see http://blog.csdn.net/hoytgm/article/details/38174875
			//glFramebufferTexture2D
			//http://blog.csdn.net/u012501459/article/details/12945167
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, __framebufferId);			
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, 
			                        //All.ColorAttachment0Ext + this.__textureIndex, //FIXME:??? //GL_COLOR_ATTACHMENT0_EXT 
			                        All.ColorAttachment0 + this.__textureIndex, //FIXME:???
			                        TextureTarget2d.Texture2D, texture.__textureId, level);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			
			this.__width = texture.Width;
			this.__height = texture.Height;
			this.__textureArr[this.__textureIndex] = texture;
			this.__textureIndex++;
		}
		
		public void SetDepthTarget (DepthBuffer buffer)
		{
			//Debug.Assert(false);
			//FIXME:not used
			//see E:\github2\angle\samples\gles2_book\MultipleRenderTargets
			//see http://blog.csdn.net/u012501459/article/details/12945167
			// 创建一个渲染缓冲区对象来存储深度信息  
//			GLuint rboId;  
//			glGenRenderbuffers(1, &rboId);  
//			glBindRenderbuffer(GL_RENDERBUFFER, rboId);  
//			glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT,  
//			                      TEXTURE_WIDTH, TEXTURE_HEIGHT);  
//			glBindRenderbuffer(GL_RENDERBUFFER, 0);  
			
			// 将渲染缓冲区对象附加到FBO的深度附加点上  
//			glFramebufferRenderbuffer(GL_FRAMEBUFFER,      // 1. fbo target: GL_FRAMEBUFFER  
//			                          GL_DEPTH_ATTACHMENT, // 2. attachment point  
//			                          GL_RENDERBUFFER,     // 3. rbo target: GL_RENDERBUFFER  
//			                          rboId);              // 4. rbo ID  
		}
	}
}
