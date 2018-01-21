using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

using Sakura.OpenTK;

namespace Sce.Pss.Core.Graphics
{
	public class GraphicsContext : IDisposable
	{
		//see https://stackoverflow.com/questions/250404/where-does-console-writeline-go-in-debug
		public class __DebugTextWriter : TextWriter
	    {
	        public override Encoding Encoding
	        {
	            get { return Encoding.UTF8; }
	        }
	
	        //Required
	        public override void Write(char value)
	        {
	            Debug.Write(value);
	        }
	
	        //Added for efficiency
	        public override void Write(string value)
	        {
	            Debug.Write(value);
	        }
	
	        //Added for efficiency
	        public override void WriteLine(string value)
	        {
	            Debug.WriteLine(value);
	        }
	    }
		
		
		
		
		//see http://blog.csdn.net/hb707934728/article/details/52044702
		//http://www.opengl-tutorial.org
		//https://github.com/opengl-tutorials/ogl
		//https://github.com/ynztlxdeai/GLproject/tree/master/app/src/main/assets
		//http://doc.qt.io/qt-5/qtgui-openglwindow-example.html
		
		public static Dictionary<int, bool> __isUsedProgram = new Dictionary<int, bool>();
		public static Dictionary<int, VertexBuffer> __vertexBuffer = new Dictionary<int, VertexBuffer>();
		private FrameBuffer __frameBuffer;
		private FrameBuffer __screen;
	    private static readonly float[] __vVertices = {  
        	0.0f,  0.5f, 0.0f,
	        -0.5f, -0.5f, 0.0f,
	        0.5f, -0.5f, 0.0f 
        };		
		
		public GraphicsContext()
		{
			Console.SetOut(new __DebugTextWriter());
			
			MyGameWindow.Init();
			
			Color4 color = Color4.Black;//FIXME:
            color = Color4.MidnightBlue;
            GL.ClearColor(color.R, color.G, color.B, color.A);
            GL.Enable(EnableCap.DepthTest);
            
            this.__screen = FrameBuffer.__getScreen();
            this.__frameBuffer = this.__screen;
		}
		
		public void Dispose ()
		{
			
		}
		
		public FrameBuffer Screen
		{
			get
			{
				return __screen;//this.screen;
			}
		}

		public void SetViewport(int x, int y, int w, int h)
		{
			GL.Viewport(0, 0, w, h);
        }
		
		public void SetClearColor(float r, float g, float b, float a)
		{
			GL.ClearColor(r, g, b, a);
		}
		
		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}
		
		private int __curProgramObject = 0;
		public void SetShaderProgram(ShaderProgram program)
		{
			program.__linkProgram();
			__curProgramObject = program.__programObject;
			if (program.__programObject != 0)
			{
				__isUsedProgram[program.__programObject] = true;
				__vertexBuffer.Clear();
				GL.Clear(ClearBufferMask.DepthBufferBit); //FIXME:???
	        	GL.UseProgram(program.__programObject);
	        	foreach (int location in program.__uniformMatrix4.Keys)
	        	{
	        		OpenTK.Matrix4 v = program.__uniformMatrix4[location];
	        		GL.UniformMatrix4 (location, false, ref v);
	        	}
	        	program.__afterUseProgram();
			}
			else
			{
				//FIXME:
			}
		}
		
		public void SetVertexBuffer(int index, VertexBuffer buffer)
		{
			//FIXME:index not unsed
			__vertexBuffer[index] = buffer;
		}
		
		public void DrawArrays(DrawMode mode, int first, int count)
		{
			//VertexAttribPointer
			//EnableVertexAttribArray
			foreach (VertexBuffer buffer in __vertexBuffer.Values)
			{
				for (int i = 0; i < buffer.__verticesArr.Length; ++i)
				{
					if (buffer.__verticesArr[i] == null)
					{
						break;
					}
					if (buffer.__formatsArr[i] == VertexFormat.Float3 || 
					    buffer.__formatsArr[i] == VertexFormat.Float4 ||
					    buffer.__formatsArr[i] == VertexFormat.Float2)
					{
						int size = 4;
						switch(buffer.__formatsArr[i])
						{
						case VertexFormat.Float3:
							size = 3;
							break;
							
						case VertexFormat.Float4:
							size = 4;
							break;
							
						case VertexFormat.Float2:
							size = 2;
							break;
							
						default:
							Debug.Assert(false);
							break;
						}
						VertexAttribPointerType vType = VertexAttribPointerType.Float;
						float[] vert = (float[])buffer.__verticesArr[i];
						GL.VertexAttribPointer (i, size, vType, false, 0/*size * 4*/, vert);
						GL.EnableVertexAttribArray ( i );
					}
					else
					{
						Debug.Assert(false);
					}
				}
				
				//DrawArrays
				PrimitiveType type = PrimitiveType.Triangles;
				switch (mode)
				{
					case DrawMode.TriangleStrip:
						type = PrimitiveType.TriangleStrip;
						break;
						
					case DrawMode.Triangles:
						type = PrimitiveType.Triangles;
						break;
						
					case DrawMode.TriangleFan:
						type = PrimitiveType.TriangleFan;
						break;
						
					case DrawMode.Points:
						type = PrimitiveType.Points;
						break;
						
					case DrawMode.Lines:
						type = PrimitiveType.Lines;
						break;
						
					case DrawMode.LineStrip:
						type = PrimitiveType.LineStrip;
						break;
						
					default:
						Debug.Assert(false);
						break;
				}
				ushort[] indics = buffer.__indices;
				int indexCount = buffer.__indexCount;
				if (indexCount <= 0)
				{
					GL.DrawArrays(type, first, count);
				}
				else
				{
					GL.DrawElements(type, count, DrawElementsType.UnsignedShort, indics);
				}
				
				if (buffer != null)
				{
					for (int i = 0; i < buffer.__verticesArr.Length; ++i)
					{
						if (buffer.__verticesArr[i] == null)
						{
							break;
						}
						if (buffer.__formatsArr[i] == VertexFormat.Float3 || 
						    buffer.__formatsArr[i] == VertexFormat.Float4 ||
						    buffer.__formatsArr[i] == VertexFormat.Float2)
						{
							GL.DisableVertexAttribArray ( i );
						}
						else
						{
							Debug.Assert(false);
						}
					}
				}
			}
   		}
		
		public void SwapBuffers()
		{  
#if true
			MyGameWindow.OnResize();
#endif
			MyGameWindow.SwapBuffers();
            List<int> akeys = new List<int>(__isUsedProgram.Keys);  
            foreach (int key in akeys)
            {
            	__isUsedProgram[key] = false;
            }
		}
		
		public FrameBuffer GetFrameBuffer()
		{
			return __frameBuffer;
		}
		
		public void SetTexture(int index, Texture texture)
		{
			//FIXME:tex
		   //glActiveTexture ( GL_TEXTURE0 );
		   //glBindTexture ( GL_TEXTURE_2D, userData->textureId );
			
		   // Set the sampler texture unit to 0
		   //glUniform1i ( userData->samplerLoc, 0 );
		   TextureUnit textureUnit = TextureUnit.Texture0;
		   switch (index)
		   {
		    case 0:
		   		textureUnit = TextureUnit.Texture0;
		   		break;
		   
		   	default:
		   		Debug.Assert(false);
		   		break;
		   }
		   Debug.Assert(((Texture2D)texture).__textureId >= 0);
		   GL.ActiveTexture(textureUnit);
		   GL.BindTexture (TextureTarget.Texture2D, ((Texture2D)texture).__textureId); //FIXME:
		   int samplerLoc = GL.GetUniformLocation(__curProgramObject, "Texture" + index );
		   //Debug.WriteLine("uniform['" + "Texture0" + "':].location == " + samplerLoc);
		   GL.Uniform1(samplerLoc, 0);
		}
		
		public void Enable(EnableMode mode)
		{
			switch (mode)
			{
				case EnableMode.Blend:
					GL.Enable(EnableCap.Blend);
					break;
					
				case EnableMode.DepthTest:
					GL.Enable(EnableCap.DepthTest);
					break;
				
				case EnableMode.CullFace:
					GL.Enable(EnableCap.CullFace);
					break;
					
				default:
					Debug.Assert(false);
					break;
			}
		}
		
		public void Enable (EnableMode mode, bool status)
		{
			//Debug.Assert(false);
			EnableCap mode_ = EnableCap.AlphaTest; //FIXME:
			switch (mode)
			{
				case EnableMode.CullFace:
					mode_ = EnableCap.CullFace;
					break;
					
				case EnableMode.DepthTest:
					mode_ = EnableCap.DepthTest;
					break;
				
				default:
					Debug.Assert(false);
					break;
			}
			if (status)
			{
				GL.Enable(mode_);
			}
			else
			{
				GL.Disable(mode_);
			}
		}
		
		public void SetBlendFunc(BlendFuncMode mode, BlendFuncFactor srcFactor, BlendFuncFactor dstFactor)
		{
			BlendEquationMode _mode = BlendEquationMode.FuncAdd;
			switch (mode)
			{
				case BlendFuncMode.Add:
					_mode = BlendEquationMode.FuncAdd;
					break;

				default:
					Debug.Assert(false);
					break;
			}
			BlendingFactorSrc _src = BlendingFactorSrc.SrcAlpha;
			BlendingFactorDest _dst = BlendingFactorDest.OneMinusSrcAlpha;
			switch (srcFactor)
			{
				case BlendFuncFactor.SrcAlpha:
					_src = BlendingFactorSrc.SrcAlpha;
					break;
					 
				case BlendFuncFactor.OneMinusSrcAlpha:
					_src = BlendingFactorSrc.OneMinusSrcAlpha;
				  	break;
				  	
				default:
					Debug.Assert(false);
					break;
			}
			switch (dstFactor)
			{
				case BlendFuncFactor.SrcAlpha:
					_dst = BlendingFactorDest.SrcAlpha;
					break;
					 
				case BlendFuncFactor.OneMinusSrcAlpha:
					_dst = BlendingFactorDest.OneMinusSrcAlpha;
				  	break;
				  	
				default:
					Debug.Assert(false);
					break;
			}
			GL.BlendEquation(_mode);
			GL.BlendFunc(_src, _dst);
		}
		
		public void SetFrameBuffer (FrameBuffer buffer)
		{
			//Debug.Assert(false);
			Debug.Assert(buffer.__framebufferId >= 0);
			this.__frameBuffer = buffer;
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, buffer.__framebufferId);
		}
		
		public void SetCullFace (CullFaceMode mode, CullFaceDirection direction)
		{
			OpenTK.Graphics.ES20.CullFaceMode mode_ = 0;
			OpenTK.Graphics.ES20.FrontFaceDirection mode2_ = 0;
			switch (mode)
			{
				case CullFaceMode.Back:
					mode_ = OpenTK.Graphics.ES20.CullFaceMode.Back;
					break;
			}
			switch (direction)
			{
				case CullFaceDirection.Ccw:
					mode2_ = OpenTK.Graphics.ES20.FrontFaceDirection.Ccw;	
					break;
			}
			GL.CullFace(mode_);
			GL.FrontFace(mode2_);
			//Debug.Assert(false);
		}
	}
}
