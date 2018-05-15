using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics.ES20;

namespace Sce.Pss.Core.Graphics
{
	public class ShaderProgram : IDisposable
	{
//		public ShaderProgram(byte[] fileImage)
//		{
//			
//		}
		
		public class __uniform_data
		{
			public string name;
			public int location = -1;
			
			public __uniform_data(string name_, int location_)
			{
				this.name = name_;
				this.location = location_;
			}
		}
		public Dictionary<int, __uniform_data> __uniformDic = new Dictionary<int, __uniform_data>();
		public Dictionary<int, string> __attribDic = new Dictionary<int, string>();
		public Dictionary<int, OpenTK.Matrix4> __uniformMatrix4 = new Dictionary<int, OpenTK.Matrix4>();
		public Dictionary<int, OpenTK.Vector4> __uniform4 = new Dictionary<int, OpenTK.Vector4>();
		public Dictionary<int, OpenTK.Vector3> __uniform3 = new Dictionary<int, OpenTK.Vector3>();
		public Dictionary<int, OpenTK.Vector2> __uniform2 = new Dictionary<int, OpenTK.Vector2>();
		public Dictionary<int, float> __uniform1 = new Dictionary<int, float>();
		public string __filename_vcg = null;
        public string __filename_fcg = null;
		public int __programObject = 0;
        public bool __isLinked = false;
        
        public bool __begin_GetUniformName = false;
        public bool __begin_FindAttribute = false;
        
        private static int __LoadShader ( ShaderType type, string shaderSrc )
		{
		   int shader;
		   // Create the shader object
		   shader = GL.CreateShader ( type );
		
		   if ( shader == 0 )
		   {
		   		return 0;
		   }
		
		   // Load the shader source
		   GL.ShaderSource ( shader, 1, new string[]{shaderSrc}, (int[])null);
		   
		   // Compile the shader
		   GL.CompileShader ( shader );
		
		   // Check the compile status
#if DEBUG
			int logLength;
			GL.GetShader (shader, ShaderParameter.InfoLogLength, out logLength);
			if (logLength > 0) 
			{
				string data = GL.GetShaderInfoLog (shader);
				Debug.WriteLine ("Shader compile log:\n" + data);
			}
#endif
			int status;
			GL.GetShader (shader, ShaderParameter.CompileStatus, out status);
			if (status == 0) 
			{
				GL.DeleteShader (shader);
				return 0;
			}
		   	return shader;
		}
        
        public string __ReadString(string path)
        {
        	StringBuilder sb = new StringBuilder();
        	if (File.Exists(path))
        	{
	        	using (StreamReader sr = new StreamReader(path, Encoding.Default))
	        	{
		            String line;
		            while ((line = sr.ReadLine()) != null) 
		            {
		               	sb.Append(line.ToString());
		               	sb.Append("\n");
		            }
	        	}
        	}
        	return sb.ToString();
        }
        
        public void __linkProgram()
        {
        	if (__filename_vcg == null || __filename_fcg == null || __isLinked)
        	{
        		return;
        	}
        	__isLinked = true;
			string vShaderStr =  __ReadString(__filename_vcg);
		   	string fShaderStr =  __ReadString(__filename_fcg);
			int vertexShader, fragmentShader;

			// Load the vertex/fragment shaders
			Debug.WriteLine("=============begin load shader : " + __filename_vcg + ", " + __filename_fcg);
			vertexShader = __LoadShader ( ShaderType.VertexShader, vShaderStr );
			fragmentShader = __LoadShader ( ShaderType.FragmentShader, fShaderStr );			
			Debug.WriteLine("=============end load shader : " + __filename_vcg + ", " + __filename_fcg);
			
			// Create shader program.
			__programObject = GL.CreateProgram ();

			// Attach vertex shader to program.
			GL.AttachShader (__programObject, vertexShader);
			GL.AttachShader (__programObject, fragmentShader);

			// Bind vPosition to attribute 0   
//   			GL.BindAttribLocation ( __programObject, 0, "a_Position");
//   			GL.BindAttribLocation ( __programObject, 1, "a_Color0");


        	foreach (int key in __attribDic.Keys)
            {
                GL.BindAttribLocation ( __programObject, key, __attribDic[key]);
            }
        
      	
        	
			//https://github.com/infinitespace-studios/Blog/blob/master/Etc1ContentPipeline/Etc1Alpha.Test/GLSupport.cs
			
		   	// Link the program
		   	GL.LinkProgram ( __programObject );


		   	
		   	
		   	int linked = 0;
		   	GL.GetProgram (__programObject, GetProgramParameterName.LinkStatus, out linked);
			
			// Link program.
			if (linked == 0) 
			{
				System.Diagnostics.Debug.WriteLine ("Failed to link program: {0:x}", __programObject);

				if (vertexShader != 0)
				{
					GL.DeleteShader (vertexShader);
				}

				if (fragmentShader != 0)
				{
					GL.DeleteShader (fragmentShader);
				}
				
				if (__programObject != 0) 
				{
					GL.DeleteProgram (__programObject);
					__programObject = 0;
				}
			}
			else
			{
				// Release vertex and fragment shaders.
				if (vertexShader != 0) 
				{
					GL.DetachShader (__programObject, vertexShader);
					GL.DeleteShader (vertexShader);
				}
	
				if (fragmentShader != 0) 
				{
					GL.DetachShader (__programObject, fragmentShader);
					GL.DeleteShader (fragmentShader);
				}
			}
			
			
			//http://blog.csdn.net/hjimce/article/details/51475644
			
			
			{
				Debug.WriteLine(">>>>>compiled summary: " + __filename_vcg + ", " + __filename_fcg);
				foreach (string v in __attribDic.Values)
            	{
					int location = GL.GetAttribLocation(__programObject, v);
					Debug.WriteLine(">attrib['" + v + "'].location == " + location);
				}
			}
			{
				foreach (int index in __uniformDic.Keys)
				{
					__uniform_data data = __uniformDic[index];
					data.location = GL.GetUniformLocation(__programObject, data.name);
					Debug.WriteLine(">uniform['" + data.name + "':" + index + "].location == " + data.location);
				}
				//FIXME:tex
				for (int i = 0; i < 32; ++i)
				{
					string name = "Texture" + i;
					int location = GL.GetUniformLocation(__programObject, name);
					if (location >= 0)
					{
						Debug.WriteLine(">__programObject=" + __programObject + ", texture.uniform['" + name + "'].location == " + location);	
					}
				}
			}  
        }
        
        public void __afterUseProgram()
        {

        }
		
        public ShaderProgram(byte[] fileImage)
		{
        	Debug.Assert(false);
		}
        
        public ShaderProgram(byte[] fileImage, byte[] fileImage2)
		{
        	Debug.Assert(false);
		}

        public ShaderProgram(String filename1, String filename2)
		{
			string[] whiteList = new string[] {
			    "/Application/Sce.Pss.HighLevel/UI/shaders/"
			};
			bool isMatch1 = false;
			bool isMatch2 = false;
			foreach (string name in whiteList)
			{
				if (filename1.Equals(name) || filename1.StartsWith(name))
				{
					isMatch1 = true;
					break;
				}
			}
			foreach (string name in whiteList)
			{
				if (filename2.Equals(name) || filename2.StartsWith(name))
				{
					isMatch2 = true;
					break;
				}
			}
			if (filename1 == null || filename2 == null || !isMatch1 || !isMatch2)
			{
				Debug.Assert(false);
				return;
			}
			string cgname1 = filename1.Replace("/Application/", "./");
			string cgname2 = filename2.Replace("/Application/", "./");
			__filename_vcg = cgname1.Replace(".cgx", ".vcg");
		   	__filename_fcg = cgname2.Replace(".cgx", ".fcg");
		}
        
		public ShaderProgram(String filename)
		{
			string[] whiteList = new string[] {
				"/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/default.cgx",
				"/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/sprite.cgx",
				"/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/font.cgx",
				"/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/particles.cgx",
				"/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/sprite_directional_fade.cgx",	
				
				"/Application/Sample/GameEngine2D/ActionGameDemo/assets/",
				"/Application/Sample/GameEngine2D/PuzzleGameDemo/shaders/",
				
				"/Application/Sample/Lib/SampleLib/shaders/Test.cgx",
			    "/Application/Sample/Graphics/TriangleSample/shaders/VertexColor.cgx",
			    "/Application/Sample/Lib/SampleLib/shaders/Simple.cgx",
			    "/Application/Sample/Lib/SampleLib/shaders/Texture.cgx",
			    "/Application/Sample/Graphics/PrimitiveSample/shaders/VertexColor.cgx",
			    "/Application/Sample/Graphics/PixelBufferSample/shaders/VertexColor.cgx",
			    "/Application/Sample/Graphics/PixelBufferSample/shaders/Texture.cgx",
			    "/Application/Sample/Graphics/ShaderCatalogSample/shaders/"
			};
			bool isMatch = false;
			foreach (string name in whiteList)
			{
				if (filename.Equals(name) || filename.StartsWith(name))
				{
					isMatch = true;
					break;
				}
			}
			if (filename == null || !isMatch)
			{
				Debug.Assert(false);
				return;
			}
			string cgname = filename.Replace("/Application/", "./");
			__filename_vcg = cgname.Replace(".cgx", ".vcg");
		   	__filename_fcg = cgname.Replace(".cgx", ".fcg");
		}
		
		public void SetUniformBinding (int index, string name)
		{
			__uniformDic[index] = new __uniform_data(name, -1);
			
			if (__programObject != 0)
			{	
				if ((GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				     GraphicsContext.__isUsedProgram[__programObject]) || __begin_GetUniformName)
				{
					__uniform_data data = __uniformDic[index];
					data.location = GL.GetUniformLocation(__programObject, data.name);
					Debug.WriteLine("uniform['" + data.name + "':" + index + "].location == " + data.location);
				}
			}
		}
		
		public void SetAttributeBinding (int index, string name)
		{
			__attribDic[index] = name;
			if (__programObject != 0)
			{	
				if ((GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				     GraphicsContext.__isUsedProgram[__programObject]) || __begin_FindAttribute)
				{
					//Debug.Assert(false);
					string name2 = __attribDic[index];
					GL.BindAttribLocation ( __programObject, index, name2);
					Debug.WriteLine("attrib['" + name2 + "':" + index + "].index == " + index);
				}
			}
		}
		
		public void Dispose ()
		{
			
		}
		
		public void SetUniformValue(int index, float[] value)
		{
			Debug.Assert(false);
		}
		
		public void SetUniformValue(int index, ref Matrix4 value)
		{
			//FIXME:???
			this.__linkProgram();
			OpenTK.Matrix4 v = new OpenTK.Matrix4(
				value.M11, value.M12, value.M13, value.M14,
				value.M21, value.M22, value.M23, value.M24,
				value.M31, value.M32, value.M33, value.M34,
				value.M41, value.M42, value.M43, value.M44);
			int location = __uniformDic[index].location;
			if (location < 0)
			{
				Debug.Assert(false);
			}
			__uniformMatrix4[location] = v;
//			if (__filename.Equals("/Application/Sample/Graphics/ShaderCatalogSample/shaders/Simple.cgx"))
//			{
//				Debug.WriteLine("======================location:" + location + " : " + v.ToString());
//			}
			if (__programObject != 0)
			{				
				if (GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				    GraphicsContext.__isUsedProgram[__programObject])
				{
					GL.UniformMatrix4 (location, false, ref v);	
				}
			}
		}
		
		public void SetUniformValue(int index, ref Vector4 value)
		{
			this.__linkProgram();
			//TODO:
			//Debug.Assert(false);
			OpenTK.Vector4 v = new OpenTK.Vector4(value.X, value.Y, value.Z, value.W);
			int location = __uniformDic[index].location;
			__uniform4[location] = v;
			
			if (__programObject != 0)
			{				
				if (GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				    GraphicsContext.__isUsedProgram[__programObject])
				{
					GL.Uniform4 (location, v.X, v.Y, v.Z, v.W);	
				}
			}
		}
		
		private int __FindUniform_nextId = 1001;
		public int FindUniform(string name)
		{
			//TODO:
			//Debug.Assert(false);
			
			foreach (int index in __uniformDic.Keys)
			{
				__uniform_data data = __uniformDic[index];
				if (data.name.Equals(name))
				{
					return index; 
				}
			}
			int newId = __FindUniform_nextId;
			__FindUniform_nextId++;
			SetUniformBinding(newId, name);
			this.__linkProgram();
			foreach (int index in __uniformDic.Keys)
			{
				if (index == newId)
				{
					__uniform_data data = __uniformDic[index];
					data.location = GL.GetUniformLocation(__programObject, data.name);
					Debug.WriteLine(">>>FindUniform:uniform['" + data.name + "':" + index + "].location == " + data.location);
					return newId; 
				}
			}
			return -1;
		}
		
		public void SetUniformValue(int index, ref Vector3 value)
		{
			this.__linkProgram();
			//TODO:
			//Debug.Assert(false);
			OpenTK.Vector3 v = new OpenTK.Vector3(value.X, value.Y, value.Z);
			int location = __uniformDic[index].location;
			__uniform3[location] = v;
			
			if (__programObject != 0)
			{				
				if (GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				    GraphicsContext.__isUsedProgram[__programObject])
				{
					GL.Uniform3 (location, v.X, v.Y, v.Z);	
				}
			}
		}
		
		
		public void SetUniformValue(int index, float value)
		{
			this.__linkProgram();
			//TODO:
			//Debug.Assert(false);
			float v = value;
			int location = __uniformDic[index].location;
			__uniform1[location] = v;
			
			if (__programObject != 0)
			{				
				if (GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				    GraphicsContext.__isUsedProgram[__programObject])
				{
					GL.Uniform1 (location, v);	
				}
			}
		}
		
		public void SetUniformValue(int index, int offset, float[] value)
		{
			Debug.Assert(false);
		}
		
		public void SetUniformValue(int index, ref Vector2 value)
		{
			this.__linkProgram();
			//TODO:
			//Debug.Assert(false);
			OpenTK.Vector2 v = new OpenTK.Vector2(value.X, value.Y);
			int location = __uniformDic[index].location;
			__uniform2[location] = v;
			
			if (__programObject != 0)
			{				
				if (GraphicsContext.__isUsedProgram.ContainsKey(__programObject) &&
				    GraphicsContext.__isUsedProgram[__programObject])
				{
					GL.Uniform2 (location, v.X, v.Y);	
				}
			}
		}
		
		//see https://stackoverflow.com/questions/440144/in-opengl-is-there-a-way-to-get-a-list-of-all-uniforms-attribs-used-by-a-shade
		public string GetUniformName(int i)
		{
//			Debug.Assert(false);
//			return null;
			int size = 0;
			ActiveUniformType type;
			string name = GL.GetActiveUniform(__programObject, i, out size, out type);
			this.__begin_GetUniformName = true;
			SetUniformBinding(i, name); //FIXME:auto bind??? for

			return name;			
		}
		
		//see https://stackoverflow.com/questions/440144/in-opengl-is-there-a-way-to-get-a-list-of-all-uniforms-attribs-used-by-a-shade
		public int UniformCount
		{
			get
			{
//				Debug.Assert(false);
//				return 0;
//				glGetProgramiv(program, GL_ACTIVE_UNIFORMS, &count);
				
				this.__linkProgram();
				int count = 0;
				GL.GetProgram(__programObject, GetProgramParameterName.ActiveUniforms, out count);
				return count;
			}
		}
		
		public int FindAttribute(string name)
		{
			this.__linkProgram(); //如果没有编译，__programObject将无效果
			this.__begin_FindAttribute = true;
			//FIXME:???
			return GL.GetAttribLocation(__programObject, name);
//			Debug.Assert(false);
//			return 0;
		}
	}
}
