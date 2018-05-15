using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Diagnostics;

namespace Sce.Pss.HighLevel.UI
{
	internal static class ShaderProgramManager
	{
		internal class ShaderProgramUnit
		{
			public ShaderProgram ShaderProgram;

			public Dictionary<string, int> Uniforms;

			public List<string> OtherUniformNames;

			public int UniformIndexOfModelViewProjection = -1;

			public int UniformIndexOfAlpha = -1;

			public int AttributeIndexOfPosition = -1;

			public int AttributeIndexOfColor = -1;

			public int AttributeIndexOfTexcoord = -1;
		}

		private const string aPosition = "a_Position";

		private const string aColor = "a_Color";

		private const string aTexcoord = "a_TexCoord";

		private const string uModelViewProjection = "u_WorldMatrix";

		private const string uAlpha = "u_Alpha";

		private const string sTexture = "Texture0";//"s_Texture"; //FIXME:

		private static string shaderFileNamePrifix = "Sce.Pss.HighLevel.UI.shaders.";

		private static string[,] shaderFileName;

		private static ShaderProgramManager.ShaderProgramUnit[] shaderPrograms;

		internal static void Initialize()
		{
			ShaderProgramManager.shaderPrograms = new ShaderProgramManager.ShaderProgramUnit[7];
			for (int i = 0; i < ShaderProgramManager.shaderPrograms.Length; i++)
			{
				ShaderProgramManager.loadShaderProgramUnit(i);
			}
		}

		internal static ShaderProgramManager.ShaderProgramUnit GetShaderProgramUnit(InternalShaderType type)
		{
			return ShaderProgramManager.shaderPrograms[(int)type];
		}

		private static void loadShaderProgramUnit(int index)
		{
			ShaderProgramManager.ShaderProgramUnit shaderProgramUnit = new ShaderProgramManager.ShaderProgramUnit();
#if false
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			byte[] array = null;
			byte[] array2 = null;
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(ShaderProgramManager.shaderFileNamePrifix + ShaderProgramManager.shaderFileName[index, 0]))
			{
				if (manifestResourceStream == null)
				{
					throw new FileNotFoundException("Shader file not found.", ShaderProgramManager.shaderFileName[index, 0]);
				}
				array = new byte[manifestResourceStream.Length];
				manifestResourceStream.Read(array, 0, array.Length);
			}
			using (Stream manifestResourceStream2 = executingAssembly.GetManifestResourceStream(ShaderProgramManager.shaderFileNamePrifix + ShaderProgramManager.shaderFileName[index, 1]))
			{
				if (manifestResourceStream2 == null)
				{
					throw new FileNotFoundException("Shader file not found.", ShaderProgramManager.shaderFileName[index, 1]);
				}
				array2 = new byte[manifestResourceStream2.Length];
				manifestResourceStream2.Read(array2, 0, array2.Length);
			}
			shaderProgramUnit.ShaderProgram = new ShaderProgram(array, array2);
#else
			Debug.WriteLine("shaderFileNamePrifix == " + shaderFileNamePrifix);
			shaderProgramUnit.ShaderProgram = new ShaderProgram(
				"/Application/Sce.Pss.HighLevel/UI/shaders/" + ShaderProgramManager.shaderFileName[index, 0], 
				"/Application/Sce.Pss.HighLevel/UI/shaders/" + ShaderProgramManager.shaderFileName[index, 1]);
#endif
			shaderProgramUnit.ShaderProgram.SetAttributeBinding(0, "a_Position");
			shaderProgramUnit.AttributeIndexOfPosition = 0;
			shaderProgramUnit.ShaderProgram.SetAttributeBinding(1, "a_Color");
			shaderProgramUnit.AttributeIndexOfColor = 1;
//			int testIndex = shaderProgramUnit.ShaderProgram.FindAttribute("a_Position");
//			Debug.WriteLine(">>>>>>testIndex == " + testIndex);
			if (shaderProgramUnit.ShaderProgram.FindAttribute("a_TexCoord") >= 0)
			{
				shaderProgramUnit.ShaderProgram.SetAttributeBinding(2, "a_TexCoord");
				shaderProgramUnit.AttributeIndexOfTexcoord = 2;
			}
//			else
//			{
//				//FIXME:
//				Debug.Assert(false);//???
//			}
			int uniformCount = shaderProgramUnit.ShaderProgram.UniformCount;
			shaderProgramUnit.Uniforms = new Dictionary<string, int>(uniformCount);
			shaderProgramUnit.OtherUniformNames = new List<string>(Math.Min(uniformCount - 2, 0));
			for (int i = 0; i < shaderProgramUnit.ShaderProgram.UniformCount; i++)
			{
				string uniformName = shaderProgramUnit.ShaderProgram.GetUniformName(i);
				//Debug.WriteLine(">>>>>>>>>uuu>>>>>>>>>uniformName:" + uniformName);
				if (!shaderProgramUnit.Uniforms.ContainsKey(uniformName))
				{
					shaderProgramUnit.Uniforms.Add(uniformName, i);
					if (uniformName == "u_WorldMatrix")
					{
						shaderProgramUnit.UniformIndexOfModelViewProjection = i;
					}
					else if (uniformName == "u_Alpha")
					{
						shaderProgramUnit.UniformIndexOfAlpha = i;
					}
					else if (uniformName != "s_Texture" && uniformName != "Texture0")  //FIXME:
					{
						shaderProgramUnit.OtherUniformNames.Add(uniformName);
					}
				}
			}
			ShaderProgramManager.shaderPrograms[index] = shaderProgramUnit;
		}

		internal static void Terminate(GraphicsContext graphics)
		{
			ShaderProgram shaderProgram = null;
			if (graphics != null)
			{
				shaderProgram = graphics.GetShaderProgram();
			}
			ShaderProgramManager.ShaderProgramUnit[] array = ShaderProgramManager.shaderPrograms;
			for (int i = 0; i < array.Length; i++)
			{
				ShaderProgramManager.ShaderProgramUnit shaderProgramUnit = array[i];
				if (shaderProgramUnit != null)
				{
					if (shaderProgramUnit.ShaderProgram == shaderProgram)
					{
						graphics.SetShaderProgram(null);
					}
					shaderProgramUnit.ShaderProgram.Dispose();
				}
			}
		}

		public static ShaderProgram GetShaderProgram(InternalShaderType type)
		{
			return ShaderProgramManager.GetShaderProgramUnit(type).ShaderProgram;
		}

		public static Dictionary<string, int> GetUniforms(InternalShaderType type)
		{
			return ShaderProgramManager.GetShaderProgramUnit(type).Uniforms;
		}

		static ShaderProgramManager()
		{
			// Note: this type is marked as 'beforefieldinit'.
			string[,] array = new string[7, 2];
			array[0, 0] = "basic.vp.cgx"; array[0, 1] = "solid_fill.fp.cgx";
			array[1, 0] = "basic.vp.cgx"; array[1, 1] = "texture_rgba.fp.cgx";
			array[2, 0] = "basic.vp.cgx"; array[2, 1] = "texture_a8.fp.cgx";
			array[3, 0] = "premultiplied.vp.cgx"; array[3, 1] = "texture_rgba_offscreen.fp.cgx";
			array[4, 0] = "basic.vp.cgx"; array[4, 1] = "texture_a8_shadow.fp.cgx";
			array[5, 0] = "premultiplied.vp.cgx"; array[5, 1] = "live_scroll.fp.cgx";
			array[6, 0] = "live_sphere.vp.cgx"; array[6, 1] = "live_sphere.fp.cgx";
			ShaderProgramManager.shaderFileName = array;
		}
	}
}
