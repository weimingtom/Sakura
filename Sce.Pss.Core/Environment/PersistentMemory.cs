using System;
using System.Diagnostics;
using System.IO;

namespace Sce.Pss.Core.Environment
{
	public static class PersistentMemory
	{
		private const int __maxLength = 64 * 1024; //64KB
		private const string __filename = "persistent_memory.dat";
		
		public static void Write(byte[] data)
		{
			using (FileStream stream = new FileStream(__filename, FileMode.OpenOrCreate))
			{
				stream.Write(data, 0, data.Length);
				stream.Flush();
			}
		}

		public static byte[] Read()
		{
			using (FileStream stream = new FileStream(__filename, FileMode.OpenOrCreate))
			{
				byte[] bytes = new byte[__maxLength];
				stream.Read(bytes, 0, bytes.Length);
				return bytes;
			}
		}
	}
}
