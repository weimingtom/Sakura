using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sce.Pss.HighLevel.UI
{
	public class UIAnimationPlayer : Panel
	{
		internal class Layer : ContainerWidget
		{
			public UIAnimationPlayer.TimeLine timeLineForChildren;

			public ContainerWidget transformRoot;

			public List<UIAnimationPlayer.Layer> childLayers;

			public string instanceName = "";

			private List<Effect> motions;

			private List<Effect> startedMotions;

			private Dictionary<Effect, float> motionStartTimes;

			private Matrix4 initialTransform3Dtroot;

			private Matrix4 initialTransform3Droot;

			public event EventHandler<EventArgs> AnimationStopped;

			public bool Playing
			{
				get
				{
					return this.timeLineForChildren.Playing;
				}
			}

			public bool Paused
			{
				get
				{
					return this.timeLineForChildren.Paused;
				}
			}

			public bool Repeating
			{
				get
				{
					return this.timeLineForChildren.Repeating;
				}
				set
				{
					this.timeLineForChildren.Repeating = value;
				}
			}

			public Layer()
			{
				this.timeLineForChildren = new UIAnimationPlayer.TimeLine();
				this.timeLineForChildren.TimeLineRepeated += new EventHandler<EventArgs>(this.OnTimeLineRepeated);
				this.timeLineForChildren.TimeLineStopped += new EventHandler<EventArgs>(this.OnTimeLineStoped);
				this.childLayers = new List<UIAnimationPlayer.Layer>();
				this.motions = new List<Effect>();
				this.startedMotions = new List<Effect>();
				this.motionStartTimes = new Dictionary<Effect, float>();
				this.transformRoot = new ContainerWidget();
				this.initialTransform3Dtroot = this.transformRoot.Transform3D;
				this.initialTransform3Droot = this.Transform3D;
				this.AddChildLast(this.transformRoot);
				this.Repeating = true;
			}

			protected override void OnUpdate(float elapsedTime)
			{
				base.OnUpdate(elapsedTime);
				if (!this.Playing)
				{
					return;
				}
				this.timeLineForChildren.OnUpdate(elapsedTime);
				using (List<UIAnimationPlayer.Layer>.Enumerator enumerator = this.childLayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIAnimationPlayer.Layer current = enumerator.Current;
						using (List<Effect>.Enumerator enumerator2 = current.motions.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Effect current2 = enumerator2.Current;
								if (!current.startedMotions.Contains(current2))
								{
									float num = current.motionStartTimes[current2];
									if (num <= this.timeLineForChildren.CurrentTime)
									{
										current2.Start();
										if (current.startedMotions.Count == 0)
										{
											current.timeLineForChildren.Start(this.timeLineForChildren.CurrentTime - num);
										}
										current.startedMotions.Add(current2);
									}
								}
							}
						}
					}
				}
			}

			public uint InitLayer(UIAnimationPlayer.Layer parent, UIAnimationPlayer.UIAnimationData.Layer layerData, float x, float y, UIAnimationPlayer.UIAnimationData data)
			{
				uint num = 0u;
				List<uint> list = new List<uint>();
				List<uint> list2 = new List<uint>();
				UIMotion uIMotion = null;
				UIMotion uIMotion2 = null;
				this.timeLineForChildren.FrameRate = data.header.frameRate;
				this.SetPosition(x, y);
				using (List<UIAnimationPlayer.UIAnimationData.Frame>.Enumerator enumerator = layerData.frames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIAnimationPlayer.UIAnimationData.Frame current = enumerator.Current;
						if (current.instance != null)
						{
							UIAnimationPlayer.UIAnimationData.Instance instance = current.instance;
							if (instance.header.hasBitmap && !list.Contains(instance.header.bitmapId))
							{
								this.SetPosition(x + instance.header.translationX + instance.header.matrix41, y + instance.header.translationY + instance.header.matrix42);
								Widget widget = this.CreateBitmap(data.bitmaps[(int)instance.header.bitmapId]);
								widget.SetSize(widget.Width * instance.header.scaleX, widget.Height * instance.header.scaleY);
								this.transformRoot.AddChildLast(widget);
								list.Add(instance.header.bitmapId);
							}
							if (instance.header.hasSymbol && !list2.Contains(instance.header.symbolId))
							{
								this.transformRoot.SetPosition(instance.header.transPointX, instance.header.transPointY);
								this.SetPosition(x + instance.header.translationX + instance.header.matrix41, y + instance.header.translationY + instance.header.matrix42);
								UIAnimationPlayer.UIAnimationData.Symbol symbol = data.symbols[(int)instance.header.symbolId];
								using (List<UIAnimationPlayer.UIAnimationData.Layer>.Enumerator enumerator2 = symbol.timeLines[0].layers.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										UIAnimationPlayer.UIAnimationData.Layer current2 = enumerator2.Current;
										UIAnimationPlayer.Layer layer = new UIAnimationPlayer.Layer();
										uint num2 = layer.InitLayer(this, current2, -instance.header.transPointX, -instance.header.transPointY, data);
										this.transformRoot.AddChildLast(layer);
										this.childLayers.Add(layer);
										if (this.timeLineForChildren.Duration < num2)
										{
											this.timeLineForChildren.Duration = num2;
										}
									}
								}
								list2.Add(instance.header.symbolId);
							}
							if (current.header.hasMotion)
							{
								UIMotion uIMotion3 = new UIMotion(this.transformRoot, current.motion);
								this.motions.Add(uIMotion3);
								if (uIMotion != null)
								{
									uIMotion.EffectStopped -= new EventHandler<EventArgs>(this.OnEffectStopped);
								}
								uIMotion3.EffectStopped += new EventHandler<EventArgs>(this.OnEffectStopped);
								this.motionStartTimes.Add(uIMotion3, this.timeLineForChildren.GetTimeFromFrameIndex(current.header.index));
								uIMotion2 = uIMotion3;
								uIMotion = uIMotion3;
							}
							else
							{
								uIMotion = null;
							}
							this.instanceName = instance.name;
							this.timeLineForChildren.Repeating = instance.header.repeating;
							uint num3 = current.header.index + current.header.duration;
							if (num < num3)
							{
								num = num3;
							}
						}
					}
				}
				if (uIMotion2 != null)
				{
					uIMotion2.EffectStopped -= new EventHandler<EventArgs>(this.OnEffectStopped);
				}
				this.initialTransform3Droot = this.Transform3D;
				this.initialTransform3Dtroot = this.transformRoot.Transform3D;
				return num;
			}

			public bool ReplaceWidget(string name, Widget widget)
			{
				if (this.instanceName.Equals(name))
				{
					float childrenLeft = this.GetChildrenLeft();
					float childrenRight = this.GetChildrenRight();
					float childrenTop = this.GetChildrenTop();
					float childrenBottom = this.GetChildrenBottom();
					widget.SetPosition(childrenLeft, childrenTop);
					widget.SetSize(childrenRight - childrenLeft, childrenBottom - childrenTop);
					using (List<UIAnimationPlayer.Layer>.Enumerator enumerator = this.childLayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							UIAnimationPlayer.Layer current = enumerator.Current;
							this.transformRoot.RemoveChild(current);
						}
					}
					this.childLayers.Clear();
					this.transformRoot.AddChildLast(widget);
					return true;
				}
				using (List<UIAnimationPlayer.Layer>.Enumerator enumerator2 = this.childLayers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UIAnimationPlayer.Layer current2 = enumerator2.Current;
						bool flag = current2.ReplaceWidget(name, widget);
						if (flag)
						{
							return true;
						}
					}
				}
				return false;
			}

			private float GetChildrenLeft()
			{
				float num = 3.40282347E+38f;
				foreach (Widget current in this.transformRoot.Children)
				{
					float num2 = current.X;
					if (current is UIAnimationPlayer.Layer)
					{
						num2 += (current as UIAnimationPlayer.Layer).GetChildrenLeft();
					}
					if (num2 < num)
					{
						num = num2;
					}
				}
				return num;
			}

			private float GetChildrenRight()
			{
				float num = -3.40282347E+38f;
				foreach (Widget current in this.transformRoot.Children)
				{
					float num2 = current.X + current.Width;
					if (current is UIAnimationPlayer.Layer)
					{
						num2 += (current as UIAnimationPlayer.Layer).GetChildrenRight();
					}
					if (num < num2)
					{
						num = num2;
					}
				}
				return num;
			}

			private float GetChildrenTop()
			{
				float num = 3.40282347E+38f;
				foreach (Widget current in this.transformRoot.Children)
				{
					float num2 = current.Y;
					if (current is UIAnimationPlayer.Layer)
					{
						num2 += (current as UIAnimationPlayer.Layer).GetChildrenTop();
					}
					if (num2 < num)
					{
						num = num2;
					}
				}
				return num;
			}

			private float GetChildrenBottom()
			{
				float num = -3.40282347E+38f;
				foreach (Widget current in this.transformRoot.Children)
				{
					float num2 = current.Y + current.Height;
					if (current is UIAnimationPlayer.Layer)
					{
						num2 += (current as UIAnimationPlayer.Layer).GetChildrenBottom();
					}
					if (num < num2)
					{
						num = num2;
					}
				}
				return num;
			}

			private ImageBox CreateBitmap(UIAnimationPlayer.UIAnimationData.Bitmap bitmapData)
			{
				Texture2D texture2D = new Texture2D((int)bitmapData.header.width, (int)bitmapData.header.height, false, (PixelFormat)1);
				texture2D.SetPixels(0, bitmapData.rawData);
				ImageAsset imageAsset = new ImageAsset(texture2D);
				return new ImageBox
				{
					Image = imageAsset,
					Width = (float)imageAsset.Width,
					Height = (float)imageAsset.Height
				};
			}

			public void Start(float offset)
			{
				this.Stop();
				this.timeLineForChildren.Start(offset);
			}

			public void Stop()
			{
				this.timeLineForChildren.Stop();
				using (List<Effect>.Enumerator enumerator = this.motions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect current = enumerator.Current;
						current.Stop();
					}
				}
				using (List<UIAnimationPlayer.Layer>.Enumerator enumerator2 = this.childLayers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UIAnimationPlayer.Layer current2 = enumerator2.Current;
						current2.Stop();
					}
				}
				this.startedMotions.Clear();
				this.transformRoot.Transform3D = this.initialTransform3Dtroot;
			}

			public void Pause()
			{
				this.timeLineForChildren.Pause();
				using (List<Effect>.Enumerator enumerator = this.motions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect current = enumerator.Current;
						current.Pause();
					}
				}
				using (List<UIAnimationPlayer.Layer>.Enumerator enumerator2 = this.childLayers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UIAnimationPlayer.Layer current2 = enumerator2.Current;
						current2.Pause();
					}
				}
			}

			public void Resume()
			{
				this.timeLineForChildren.Resume();
				using (List<Effect>.Enumerator enumerator = this.motions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect current = enumerator.Current;
						current.Resume();
					}
				}
				using (List<UIAnimationPlayer.Layer>.Enumerator enumerator2 = this.childLayers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UIAnimationPlayer.Layer current2 = enumerator2.Current;
						current2.Resume();
					}
				}
			}

			private void OnEffectStopped(object sender, EventArgs e)
			{
				if (sender is Effect)
				{
					Effect effect = sender as Effect;
					effect.Widget.Visible = false;
				}
			}

			private void OnTimeLineRepeated(object sender, EventArgs e)
			{
				if (sender is UIAnimationPlayer.TimeLine)
				{
					using (List<UIAnimationPlayer.Layer>.Enumerator enumerator = this.childLayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							UIAnimationPlayer.Layer current = enumerator.Current;
							current.Start((sender as UIAnimationPlayer.TimeLine).CurrentTime);
						}
					}
				}
			}

			private void OnTimeLineStoped(object sender, EventArgs e)
			{
				if (sender is UIAnimationPlayer.TimeLine && this.AnimationStopped != null)
				{
					this.AnimationStopped.Invoke(this, EventArgs.Empty);
				}
			}
		}

		internal class TimeLine
		{
			private Dictionary<string, uint> labels;

			private uint frameRate;

			private uint durationFrame;

			private float durationTime;

			private float frameToTime;

			public event EventHandler<EventArgs> TimeLineRepeated;

			public event EventHandler<EventArgs> TimeLineStopped;

			public uint Duration
			{
				get
				{
					return this.durationFrame;
				}
				set
				{
					this.durationFrame = value;
					this.durationTime = value * this.frameToTime;
				}
			}

			public uint FrameRate
			{
				get
				{
					return this.frameRate;
				}
				set
				{
					this.frameRate = value;
					this.frameToTime = 1000f / value;
				}
			}

			public float CurrentTime
			{
				get;
				private set;
			}

			public uint CurrentFrame
			{
				get
				{
					return (uint)(this.CurrentTime / this.durationTime * this.durationFrame);
				}
				private set
				{
					this.CurrentTime = value / this.durationFrame * this.durationTime;
				}
			}

			public bool Playing
			{
				get;
				private set;
			}

			public bool Paused
			{
				get;
				private set;
			}

			public bool Repeating
			{
				get;
				set;
			}

			public TimeLine()
			{
				this.CurrentTime = 0f;
				this.Repeating = false;
				this.Playing = false;
				this.Paused = false;
				this.labels = new Dictionary<string, uint>();
			}

			public void OnUpdate(float elapsedTime)
			{
				if (this.Playing)
				{
					this.CurrentTime += elapsedTime;
					if (this.CurrentTime > this.durationTime)
					{
						if (this.Repeating)
						{
							this.CurrentTime -= this.durationTime;
							if (this.TimeLineRepeated != null)
							{
								this.TimeLineRepeated.Invoke(this, EventArgs.Empty);
								return;
							}
						}
						else
						{
							this.Playing = false;
							this.Paused = false;
							if (this.TimeLineStopped != null)
							{
								this.TimeLineStopped.Invoke(this, EventArgs.Empty);
							}
						}
					}
				}
			}

			public void Reset()
			{
				this.CurrentTime = 0f;
				this.Playing = false;
				this.Paused = false;
			}

			public void Start(float offsetTime)
			{
				this.CurrentTime = offsetTime;
				this.Playing = true;
				this.Paused = false;
			}

			public void Stop()
			{
				if (this.TimeLineStopped != null)
				{
					this.TimeLineStopped.Invoke(this, EventArgs.Empty);
				}
				this.Playing = false;
				this.Paused = false;
			}

			public void Pause()
			{
				if (this.Playing)
				{
					this.Playing = false;
					this.Paused = true;
				}
			}

			public void Resume()
			{
				if (this.Paused)
				{
					this.Playing = true;
					this.Paused = false;
				}
			}

			public void SetLabel(string labelName, uint frameIndex)
			{
				if (this.labels.ContainsKey(labelName))
				{
					this.labels.Remove(labelName);
				}
				this.labels.Add(labelName, frameIndex);
			}

			public void Goto(string labelName)
			{
				if (this.labels.ContainsKey(labelName))
				{
					this.CurrentFrame = this.labels[labelName];
				}
			}

			public float GetTimeFromFrameIndex(uint frameIndex)
			{
				return frameIndex * this.frameToTime;
			}
		}

		public class UIAnimationData
		{
			public struct Header
			{
				public uint width;

				public uint height;

				public uint backgroundColorR;

				public uint backgroundColorG;

				public uint backgroundColorB;

				public uint frameRate;

				public bool repeating;

				public uint bitmapNum;

				public uint symbolNum;
			}

			public class Bitmap
			{
				public struct BitmapHeader
				{
					public uint width;

					public uint height;

					public uint byteSize;
				}

				public UIAnimationPlayer.UIAnimationData.Bitmap.BitmapHeader header;

				public byte[] rawData;

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.Bitmap.BitmapHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Bitmap.BitmapHeader));
					this.rawData = UIAnimationPlayer.UIAnimationData.ReadRawImageFromFileStream(fs, (int)this.header.byteSize);
				}
			}

			public class Symbol
			{
				public struct SymbolHeader
				{
					public uint timeLineNum;
				}

				private UIAnimationPlayer.UIAnimationData.Symbol.SymbolHeader header;

				public List<UIAnimationPlayer.UIAnimationData.TimeLine> timeLines = new List<UIAnimationPlayer.UIAnimationData.TimeLine>();

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.Symbol.SymbolHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Symbol.SymbolHeader));
					int num = 0;
					while ((long)num < (long)((ulong)this.header.timeLineNum))
					{
						UIAnimationPlayer.UIAnimationData.TimeLine timeLine = new UIAnimationPlayer.UIAnimationData.TimeLine();
						timeLine.Read(fs);
						this.timeLines.Add(timeLine);
						num++;
					}
				}
			}

			public class TimeLine
			{
				public struct TimeLienHeader
				{
					public uint layerNum;
				}

				public UIAnimationPlayer.UIAnimationData.TimeLine.TimeLienHeader header;

				public List<UIAnimationPlayer.UIAnimationData.Layer> layers = new List<UIAnimationPlayer.UIAnimationData.Layer>();

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.TimeLine.TimeLienHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.TimeLine.TimeLienHeader));
					int num = 0;
					while ((long)num < (long)((ulong)this.header.layerNum))
					{
						UIAnimationPlayer.UIAnimationData.Layer layer = new UIAnimationPlayer.UIAnimationData.Layer();
						layer.Read(fs);
						this.layers.Add(layer);
						num++;
					}
				}
			}

			public class Layer
			{
				public struct LayerHeader
				{
					public uint frameNum;
				}

				public UIAnimationPlayer.UIAnimationData.Layer.LayerHeader header;

				public List<UIAnimationPlayer.UIAnimationData.Frame> frames = new List<UIAnimationPlayer.UIAnimationData.Frame>();

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.Layer.LayerHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Layer.LayerHeader));
					int num = 0;
					while ((long)num < (long)((ulong)this.header.frameNum))
					{
						UIAnimationPlayer.UIAnimationData.Frame frame = new UIAnimationPlayer.UIAnimationData.Frame();
						frame.Read(fs);
						this.frames.Add(frame);
						num++;
					}
				}
			}

			public class Frame
			{
				public struct FrameHeader
				{
					public uint index;

					public uint duration;

					public bool hasMotion;

					public bool hasInstance;

					public uint nameByteSize;
				}

				public UIAnimationPlayer.UIAnimationData.Frame.FrameHeader header = default(UIAnimationPlayer.UIAnimationData.Frame.FrameHeader);

				public string name;

				public UIMotionData motion;

				public UIAnimationPlayer.UIAnimationData.Instance instance;

				public Frame()
				{
					this.header.duration = 0u;
					this.header.hasMotion = false;
					this.header.hasInstance = false;
				}

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.Frame.FrameHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Frame.FrameHeader));
					if (this.header.nameByteSize > 0u)
					{
						byte[] array = new byte[this.header.nameByteSize];
						if (fs.Read(array, 0, array.Length) == array.Length)
						{
							this.name = Encoding.Unicode.GetString(array);
						}
					}
					if (this.header.hasMotion)
					{
						this.motion = new UIMotionData();
						this.motion.Read(fs);
					}
					if (this.header.hasInstance)
					{
						this.instance = new UIAnimationPlayer.UIAnimationData.Instance();
						this.instance.Read(fs);
					}
				}
			}

			public class Instance
			{
				public struct InstanceHeader
				{
					public bool hasSymbol;

					public uint symbolId;

					public bool hasBitmap;

					public uint bitmapId;

					public bool repeating;

					public float transPointX;

					public float transPointY;

					public float translationX;

					public float translationY;

					public float scaleX;

					public float scaleY;

					public float rotationX;

					public float rotationY;

					public float rotationZ;

					public float centerPointX;

					public float centerPointY;

					public float centerPointZ;

					public float matrix11;

					public float matrix12;

					public float matrix13;

					public float matrix14;

					public float matrix21;

					public float matrix22;

					public float matrix23;

					public float matrix24;

					public float matrix31;

					public float matrix32;

					public float matrix33;

					public float matrix34;

					public float matrix41;

					public float matrix42;

					public float matrix43;

					public float matrix44;

					public uint nameByteSize;
				}

				public UIAnimationPlayer.UIAnimationData.Instance.InstanceHeader header;

				public string name = "";

				public Instance()
				{
					this.header.hasSymbol = false;
					this.header.hasBitmap = false;
					this.header.repeating = true;
				}

				public void Read(FileStream fs)
				{
					this.header = (UIAnimationPlayer.UIAnimationData.Instance.InstanceHeader)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Instance.InstanceHeader));
					if (this.header.nameByteSize > 0u)
					{
						byte[] array = new byte[this.header.nameByteSize];
						if (fs.Read(array, 0, array.Length) == array.Length)
						{
							this.name = Encoding.Unicode.GetString(array);
						}
					}
				}
			}

			private string SIGNATURE = "_uia";

			private string VERSION = "0001";

			public UIAnimationPlayer.UIAnimationData.Header header;

			public List<UIAnimationPlayer.UIAnimationData.Bitmap> bitmaps = new List<UIAnimationPlayer.UIAnimationData.Bitmap>();

			public List<UIAnimationPlayer.UIAnimationData.Symbol> symbols = new List<UIAnimationPlayer.UIAnimationData.Symbol>();

			public UIAnimationPlayer.UIAnimationData.Symbol rootSymbol = new UIAnimationPlayer.UIAnimationData.Symbol();

			public void ReadFromFile(string fileName)
			{
				FileStream fileStream = new FileStream(fileName, (FileMode)3, (FileAccess)1);
				if (fileStream != null)
				{
					this.CheckSignatureAndVersion(fileStream);
					this.ReadHeader(fileStream);
					this.ReadBitmaps(fileStream);
					this.ReadSymbols(fileStream);
					this.rootSymbol = new UIAnimationPlayer.UIAnimationData.Symbol();
					this.rootSymbol.Read(fileStream);
					fileStream.Close();
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
				if (!text.Equals(this.SIGNATURE) || !text2.Equals(this.VERSION))
				{
					throw new FileLoadException("Invalid file format");
				}
			}

			private void ReadHeader(FileStream fs)
			{
				this.header = (UIAnimationPlayer.UIAnimationData.Header)UIAnimationPlayer.UIAnimationData.ReadObject(fs, typeof(UIAnimationPlayer.UIAnimationData.Header));
			}

			private void ReadBitmaps(FileStream fs)
			{
				int num = 0;
				while ((long)num < (long)((ulong)this.header.bitmapNum))
				{
					UIAnimationPlayer.UIAnimationData.Bitmap bitmap = new UIAnimationPlayer.UIAnimationData.Bitmap();
					bitmap.Read(fs);
					this.bitmaps.Add(bitmap);
					num++;
				}
			}

			private void ReadSymbols(FileStream fs)
			{
				int num = 0;
				while ((long)num < (long)((ulong)this.header.symbolNum))
				{
					UIAnimationPlayer.UIAnimationData.Symbol symbol = new UIAnimationPlayer.UIAnimationData.Symbol();
					symbol.Read(fs);
					this.symbols.Add(symbol);
					num++;
				}
			}

			private static byte[] ReadRawImageFromFileStream(FileStream fs, int size)
			{
				byte[] array = new byte[size];
				if (fs.Read(array, 0, array.Length) == array.Length)
				{
					return array;
				}
				return null;
			}

			private static object ReadObject(FileStream fs, Type type)
			{
				object obj = type.InvokeMember(null, (BindingFlags)512, null, null, null);
				FieldInfo[] fields = type.GetFields();
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					string name;
					if ((name = fieldInfo.FieldType.Name) != null)
					{
						if (!(name == "UInt32"))
						{
							if (!(name == "Int32"))
							{
								if (!(name == "Char"))
								{
									if (!(name == "Single"))
									{
										if (name == "Boolean")
										{
											byte[] array = new byte[4];
											if (fs.Read(array, 0, array.Length) == array.Length)
											{
												fieldInfo.SetValue(obj, BitConverter.ToBoolean(array, 0));
											}
										}
									}
									else
									{
										byte[] array = new byte[4];
										if (fs.Read(array, 0, array.Length) == array.Length)
										{
											fieldInfo.SetValue(obj, BitConverter.ToSingle(array, 0));
										}
									}
								}
								else
								{
									byte[] array = new byte[2];
									if (fs.Read(array, 0, array.Length) == array.Length)
									{
										fieldInfo.SetValue(obj, BitConverter.ToChar(array, 0));
									}
								}
							}
							else
							{
								byte[] array = new byte[4];
								if (fs.Read(array, 0, array.Length) == array.Length)
								{
									fieldInfo.SetValue(obj, BitConverter.ToInt32(array, 0));
								}
							}
						}
						else
						{
							byte[] array = new byte[4];
							if (fs.Read(array, 0, array.Length) == array.Length)
							{
								fieldInfo.SetValue(obj, BitConverter.ToUInt32(array, 0));
							}
						}
					}
				}
				return obj;
			}
		}

		private UIAnimationPlayer.Layer rootLayer;

		public event EventHandler<EventArgs> AnimationStopped;

		public bool Playing
		{
			get
			{
				return this.rootLayer.Playing;
			}
		}

		public bool Paused
		{
			get
			{
				return this.rootLayer.Paused;
			}
		}

		public bool Repeating
		{
			get
			{
				return this.rootLayer.Repeating;
			}
			set
			{
				this.rootLayer.Repeating = value;
			}
		}

		private UIAnimationPlayer() : this(null)
		{
		}

		public UIAnimationPlayer(string filePath)
		{
			base.Clip = true;
			if (filePath != null)
			{
				UIAnimationPlayer.UIAnimationData uIAnimationData = new UIAnimationPlayer.UIAnimationData();
				uIAnimationData.ReadFromFile(filePath);
				this.SetSize(uIAnimationData.header.width, uIAnimationData.header.height);
				this.BackgroundColor = new UIColor(uIAnimationData.header.backgroundColorR / 255f, uIAnimationData.header.backgroundColorG / 255f, uIAnimationData.header.backgroundColorB / 255f, 1f);
				if (uIAnimationData.rootSymbol != null && uIAnimationData.rootSymbol.timeLines != null && uIAnimationData.rootSymbol.timeLines[0] != null && uIAnimationData.rootSymbol.timeLines[0].layers != null)
				{
					this.rootLayer = new UIAnimationPlayer.Layer();
					this.rootLayer.timeLineForChildren.FrameRate = uIAnimationData.header.frameRate;
					this.rootLayer.AnimationStopped += new EventHandler<EventArgs>(this.OnRootLayoutAnimationStoped);
					using (List<UIAnimationPlayer.UIAnimationData.Layer>.Enumerator enumerator = uIAnimationData.rootSymbol.timeLines[0].layers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							UIAnimationPlayer.UIAnimationData.Layer current = enumerator.Current;
							UIAnimationPlayer.Layer layer = new UIAnimationPlayer.Layer();
							uint num = layer.InitLayer(this.rootLayer, current, 0f, 0f, uIAnimationData);
							this.rootLayer.transformRoot.AddChildLast(layer);
							this.rootLayer.childLayers.Add(layer);
							if (this.rootLayer.timeLineForChildren.Duration < num)
							{
								this.rootLayer.timeLineForChildren.Duration = num;
							}
						}
					}
					this.rootLayer.Repeating = uIAnimationData.header.repeating;
					this.Width = uIAnimationData.header.width;
					this.Height = uIAnimationData.header.height;
					this.AddChildLast(this.rootLayer);
				}
			}
		}

		public static UIAnimationPlayer CreateAndPlay(string filePath)
		{
			UIAnimationPlayer uIAnimationPlayer = new UIAnimationPlayer(filePath);
			uIAnimationPlayer.Play();
			return uIAnimationPlayer;
		}

		public void Play()
		{
			this.rootLayer.Start(0f);
		}

		public void Stop()
		{
			this.rootLayer.Stop();
		}

		public void Pause()
		{
			this.rootLayer.Pause();
		}

		public void Resume()
		{
			this.rootLayer.Resume();
		}

		private void GotoAndPlay(string label)
		{
		}

		public void ReplaceWidget(string name, Widget widget)
		{
			if (name == null || name.Equals(""))
			{
				return;
			}
			this.rootLayer.ReplaceWidget(name, widget);
		}

		private void OnRootLayoutAnimationStoped(object sender, EventArgs e)
		{
			if (sender is UIAnimationPlayer.Layer && this.AnimationStopped != null)
			{
				this.AnimationStopped.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
