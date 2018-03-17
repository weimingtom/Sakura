using Sce.Pss.Core.Graphics;
using System;
using System.IO;
using System.Reflection;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public static class Common
	{
		public class AssertFailed : Exception
		{
		}

		public delegate int IndexWrapMode(int i, int n);

		private static uint g_frame_counter = 0u;

		public static Profiler Profiler = new Profiler();

		public static uint FrameCount
		{
			get
			{
				return Common.g_frame_counter;
			}
		}

		public static void Assert(bool cond, string message)
		{
			if (!cond)
			{
				Console.WriteLine(" *** error: " + message);
				throw new Common.AssertFailed();
			}
		}

		public static void Assert(bool cond)
		{
			if (!cond)
			{
				throw new Common.AssertFailed();
			}
		}

		public static void OnSwap()
		{
			Common.g_frame_counter += 1u;
		}

		public static int WrapIndex(int i, int n)
		{
			return ((i %= n) < 0) ? (i + n) : i;
		}

		public static int ClampIndex(int i, int n)
		{
			return (i < 0) ? 0 : ((i > n - 1) ? (n - 1) : i);
		}

		public static int Clamp(int i, int min, int max)
		{
			return (i < min) ? min : ((i > max) ? max : i);
		}

		public static int Min(int i, int value)
		{
			return (i < value) ? i : value;
		}

		public static int Max(int i, int value)
		{
			return (i > value) ? i : value;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		public static byte[] GetEmbeddedResource(string filename)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text = "Sce.Pss.HighLevel.GameEngine2D." + filename.Replace("/", ".");
			if (executingAssembly.GetManifestResourceInfo(text) == null)
			{
				string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
				for (int i = 0; i < manifestResourceNames.Length; i++)
				{
					Console.WriteLine("all_embedded_names[i]=" + manifestResourceNames[i]);
				}
				Console.WriteLine("embedded filename=" + executingAssembly);
				Common.Assert(false, filename);
				throw new FileNotFoundException("File not found.", filename);
			}
			Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, array.Length);
			return array;
		}

		public static ShaderProgram CreateShaderProgram(string filename)
		{
#if false
			byte[] embeddedResource = Common.GetEmbeddedResource(filename);
			return new ShaderProgram(embeddedResource);
#else
			string cgname = filename.Replace("cg/", "/Application/Sce.Pss.HighLevel/GameEngine2D/Base/cg/");
		   	return new ShaderProgram(cgname);
#endif
		}

		public static void DisposeAndNullify<T>(ref T a)
		{
			if (a != null)
			{
				((IDisposable)((object)a)).Dispose();
				a = default(T);
			}
		}
	}
}
