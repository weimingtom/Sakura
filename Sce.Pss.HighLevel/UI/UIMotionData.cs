using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using System.Diagnostics;

namespace Sce.Pss.HighLevel.UI
{
	public class UIMotionData
	{
		public struct Header
		{
			public uint timeScale;

			public uint duration;

			public bool orientToPath;

			public float xformPtXOffsetPct;

			public float xformPtYOffsetPct;

			public float xformPtZOffsetPixels;

			public uint propertyNum;

			public uint timeMapNum;
		}

		public class TimeMap
		{
			public struct TimeMapHeader
			{
				public EaseType easeType;

				public int strength;

				public uint customPointNum;
			}

			public struct CustomPoint
			{
				public float anchorX;

				public float anchorY;

				public float nextX;

				public float nextY;

				public float previousX;

				public float previousY;
			}

			public UIMotionData.TimeMap.TimeMapHeader header;

			public List<UIMotionData.TimeMap.CustomPoint> customPoints = new List<UIMotionData.TimeMap.CustomPoint>();

			public void Read(FileStream fs)
			{
				this.header = (UIMotionData.TimeMap.TimeMapHeader)UIMotionData.ReadObject(fs, typeof(UIMotionData.TimeMap.TimeMapHeader));
				if (this.header.easeType == EaseType.Custom)
				{
					int num = 0;
					while ((long)num < (long)((ulong)this.header.customPointNum))
					{
						UIMotionData.TimeMap.CustomPoint customPoint = (UIMotionData.TimeMap.CustomPoint)UIMotionData.ReadObject(fs, typeof(UIMotionData.TimeMap.CustomPoint));
						this.customPoints.Add(customPoint);
						num++;
					}
				}
			}
		}

		public class Property
		{
			public struct PropertyHeader
			{
				public PropertyType propertyType;

				public bool useTimeMap;

				public uint timeMapIndex;

				public uint keyframeNum;
			}

			public struct Keyframe
			{
				public float anchorX;

				public float anchorY;

				public float nextX;

				public float nextY;

				public float previousX;

				public float previousY;

				public bool roving;

				public uint time;
			}

			public UIMotionData.Property.PropertyHeader header = default(UIMotionData.Property.PropertyHeader);

			public List<UIMotionData.Property.Keyframe> keyframes = new List<UIMotionData.Property.Keyframe>();

			public void Read(FileStream fs)
			{
				this.header = (UIMotionData.Property.PropertyHeader)UIMotionData.ReadObject(fs, typeof(UIMotionData.Property.PropertyHeader));
				int num = 0;
				while ((long)num < (long)((ulong)this.header.keyframeNum))
				{
					UIMotionData.Property.Keyframe keyframe = (UIMotionData.Property.Keyframe)UIMotionData.ReadObject(fs, typeof(UIMotionData.Property.Keyframe));
					this.keyframes.Add(keyframe);
					num++;
				}
			}
		}

		private const string SIGNATURE = "_uim";

		private const string VERSION = "0001";

		internal UIMotionData.Header header;

		public List<UIMotionData.TimeMap> timeMaps = new List<UIMotionData.TimeMap>();

		public List<UIMotionData.Property> properties = new List<UIMotionData.Property>();

		public void Read(string fileName)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(fileName, (FileMode)3, (FileAccess)1);
				if (fileStream != null)
				{
					this.Read(fileStream);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		public void Read(FileStream fs)
		{
			if (fs != null)
			{
				this.CheckSignatureAndVersion(fs);
				this.ReadHeader(fs);
				this.ReadTimeMaps(fs);
				this.ReadProperties(fs);
			}
		}

		private void CheckSignatureAndVersion(FileStream fs)
		{
			byte[] array = new byte[4];
			string text = "";
			if (fs.Read(array, 0, array.Length) == array.Length)
			{
				text = Encoding.ASCII.GetString(array);
			}
			string text2 = "";
			if (fs.Read(array, 0, array.Length) == array.Length)
			{
				text2 = Encoding.ASCII.GetString(array);
			}
			if (!text.Equals("_uim") || !text2.Equals("0001"))
			{
				throw new FileLoadException("Invalid file format");
			}
		}

		private void ReadHeader(FileStream fs)
		{
			this.header = (UIMotionData.Header)UIMotionData.ReadObject(fs, typeof(UIMotionData.Header));
		}

		private void ReadTimeMaps(FileStream fs)
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.header.timeMapNum))
			{
				UIMotionData.TimeMap timeMap = new UIMotionData.TimeMap();
				timeMap.Read(fs);
				this.timeMaps.Add(timeMap);
				num++;
			}
		}

		private void ReadProperties(FileStream fs)
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.header.propertyNum))
			{
				UIMotionData.Property property = new UIMotionData.Property();
				property.Read(fs);
				this.properties.Add(property);
				num++;
			}
		}

		//FIXME:???decompile failed
		private static object ReadObject(FileStream fs, Type type)
		{
			object obj = type.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				switch (fieldInfo.FieldType.Name)
				{
				case "UInt32":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, BitConverter.ToUInt32(array, 0));
					}
					break;
				}
				case "Int32":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, BitConverter.ToInt32(array, 0));
					}
					break;
				}
				case "Char":
				{
					byte[] array = new byte[2];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, BitConverter.ToChar(array, 0));
					}
					break;
				}
				case "Single":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, BitConverter.ToSingle(array, 0));
					}
					break;
				}
				case "EaseType":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, (EaseType)BitConverter.ToInt32(array, 0));
					}
					break;
				}
				case "PropertyType":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) == array.Length)
					{
						fieldInfo.SetValue(obj, (PropertyType)BitConverter.ToInt32(array, 0));
					}
					break;
				}
				case "Boolean":
				{
					byte[] array = new byte[4];
					if (fs.Read(array, 0, array.Length) >= array.Length)
					{
						fieldInfo.SetValue(obj, BitConverter.ToBoolean(array, 0));
					}
					break;
				}
				}
			}
			return obj;
		}
	}
}
