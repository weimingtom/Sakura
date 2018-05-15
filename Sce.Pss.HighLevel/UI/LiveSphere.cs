using Sce.Pss.Core;
using System;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveSphere : Widget
	{
		private enum AnimationState
		{
			None,
			Flip,
			FlickFlip,
			Stopping,
			Drag
		}

		private LiveSphere.AnimationState animationState;

		private bool animation;

		private int turnCount = 16;

		private float zAxis;

		private float yAxis;

		private bool touchEnabled = true;

		private bool toggleEnabled;

		private float bounceTime = 1500f;

		private float acceleration = -5E-06f;

		private int flick = 100;

		private float flipTime;

		private float firstOmega;

		private float secondOmega;

		private float startAngle;

		private float flipEndAngle;

		private float endAngle;

		private float totalTime;

		private bool isStopAnimate;

		private FlickGestureDetector flickGesture;

		private DragGestureDetector dragGesture;

		private int sphereDiv = 16;

		private float radius = 50f;

		private UIPrimitive spherePrim;

		private Vector4 LightDirection;

		private float LightZAngle = 3.8f;

		private float LightYAngle = 0.44f;

		private float Shininess = 100f;

		private float Specular = 0.6f;

		public event EventHandler<TouchEventArgs> ButtonAction;

		public int TurnCount
		{
			get
			{
				return this.turnCount;
			}
			set
			{
				this.turnCount = value;
			}
		}

		public float ZAxis
		{
			get
			{
				return this.zAxis;
			}
			set
			{
				this.zAxis = value;
				this.UpdateRotateMatrix();
			}
		}

		public float YAxis
		{
			get
			{
				return this.yAxis;
			}
			set
			{
				this.Stop();
				this.yAxis = value;
				this.UpdateRotateMatrix();
			}
		}

		public bool TouchEnabled
		{
			get
			{
				return this.touchEnabled;
			}
			set
			{
				this.touchEnabled = value;
				if (this.touchEnabled)
				{
					base.AddGestureDetector(this.flickGesture);
					base.AddGestureDetector(this.dragGesture);
					return;
				}
				base.RemoveGestureDetector(this.flickGesture);
				base.RemoveGestureDetector(this.dragGesture);
			}
		}

		public bool ToggleEnabled
		{
			get
			{
				return this.toggleEnabled;
			}
			set
			{
				this.toggleEnabled = value;
			}
		}

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				if (value > 0f)
				{
					base.Width = value;
					this.updateRadius();
				}
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				if (value > 0f)
				{
					base.Height = value;
					this.updateRadius();
				}
			}
		}

		public ImageAsset Image
		{
			get
			{
				return this.spherePrim.Image;
			}
			set
			{
				this.spherePrim.Image = value;
			}
		}

		public bool FrontFace
		{
			get
			{
				return (int)Math.Round((double)(this.yAxis / 3.14159274f)) % 2 == 0;
			}
		}

		public LiveSphere()
		{
			int num = this.sphereDiv;
			int num2 = this.sphereDiv * 2;
			this.spherePrim = new UIPrimitive((DrawMode)4, (num + 1) * (num2 + 1), num * 2 * (num2 + 1));
			this.spherePrim.Culling = true;
			this.spherePrim.InternalShaderType = InternalShaderType.LiveSphere;
			float num3 = 3.14159274f / (float)num;
			float num4 = 6.28318548f / (float)num2;
			float num5 = 1f / (float)num2;
			float num6 = 1f / (float)num;
			for (int i = 0; i < num + 1; i++)
			{
				for (int j = 0; j < num2 + 1; j++)
				{
					float num7 = -(float)Math.Sin((double)((float)i * num3)) * (float)Math.Cos((double)((float)j * num4));
					float num8 = -(float)Math.Cos((double)((float)i * num3));
					float num9 = -(float)Math.Sin((double)((float)i * num3)) * (float)Math.Sin((double)((float)j * num4));
					UIPrimitiveVertex vertex = this.spherePrim.GetVertex(i * (num2 + 1) + j);
					vertex.Position3D = new Vector3(num7, num8, num9);
					vertex.U = num5 * (float)j;
					vertex.V = num6 * (float)i;
				}
			}
			ushort[] array = new ushort[num * 2 * (num2 + 1)];
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num2 + 1; l++)
				{
					array[k * (num2 + 1) * 2 + l * 2] = (ushort)(k * (num2 + 1) + l);
					array[k * (num2 + 1) * 2 + l * 2 + 1] = (ushort)((k + 1) * (num2 + 1) + l);
				}
			}
			this.spherePrim.SetIndices(array);
			base.RootUIElement.AddChildLast(this.spherePrim);
			this.LightDirection = new Vector4((float)(Math.Sin((double)this.LightZAngle) * Math.Cos((double)this.LightYAngle)), (float)Math.Cos((double)this.LightZAngle), (float)(Math.Sin((double)this.LightZAngle) * Math.Sin((double)this.LightYAngle)), 0f);
			this.spherePrim.ShaderUniforms["Shininess"] = new float[]
			{
				this.Shininess
			};
			this.spherePrim.ShaderUniforms["Specular"] = new float[]
			{
				this.Specular
			};
			this.SetSize(this.radius * 2f, this.radius * 2f);
			this.flickGesture = new FlickGestureDetector();
			this.flickGesture.Direction = FlickDirection.All;
			this.flickGesture.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(this.flickGesture);
			this.dragGesture = new DragGestureDetector();
			this.dragGesture.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(this.dragGesture);
			this.PriorityHit = true;
		}

		public override void SetSize(float width, float height)
		{
			base.Width = width;
			base.Height = height;
			this.updateRadius();
		}

		private void updateRadius()
		{
			this.spherePrim.X = base.Width / 2f;
			this.spherePrim.Y = base.Height / 2f;
			this.radius = ((this.spherePrim.X < this.spherePrim.Y) ? this.spherePrim.X : this.spherePrim.Y);
			this.UpdateRotateMatrix();
		}

		public void Start()
		{
			if (!this.toggleEnabled)
			{
				this.Start(this.turnCount, false);
				return;
			}
			if (this.FrontFace)
			{
				this.Start(1, false);
				return;
			}
			this.Start(-1, false);
		}

		private void Start(int revo, bool isFlick)
		{
			if (revo == 0)
			{
				return;
			}
			this.startAngle = this.yAxis;
			this.endAngle = this.startAngle + (float)revo * 3.14159274f;
			this.endAngle = (float)Math.Round((double)(this.endAngle / 3.14159274f)) * 3.14159274f;
			if (this.toggleEnabled)
			{
				this.acceleration *= 3f;
			}
			if (revo > 0)
			{
				this.flipEndAngle = this.endAngle - 1.57079637f;
				this.acceleration = -5E-06f;
				this.secondOmega = 10.88f / this.bounceTime;
				this.firstOmega = (float)Math.Sqrt((double)(this.secondOmega * this.secondOmega + 2f * this.acceleration * (this.startAngle - this.flipEndAngle)));
			}
			else
			{
				this.flipEndAngle = this.endAngle + 1.57079637f;
				this.acceleration = 5E-06f;
				this.secondOmega = -10.88f / this.bounceTime;
				this.firstOmega = -(float)Math.Sqrt((double)(this.secondOmega * this.secondOmega + 2f * this.acceleration * (this.startAngle - this.flipEndAngle)));
			}
			this.totalTime = 0f;
			this.flipTime = (this.secondOmega - this.firstOmega) / this.acceleration;
			this.isStopAnimate = false;
			this.animation = true;
			if (isFlick)
			{
				this.animationState = LiveSphere.AnimationState.FlickFlip;
				return;
			}
			this.animationState = LiveSphere.AnimationState.Flip;
		}

		public void Stop()
		{
			this.Stop(false);
		}

		public void Stop(bool withAnimate)
		{
			if (withAnimate)
			{
				this.isStopAnimate = true;
				return;
			}
			if (this.animation)
			{
				if ((int)Math.Round((double)(this.yAxis / 3.14159274f)) % 2 == 0)
				{
					this.yAxis = 0f;
				}
				else
				{
					this.yAxis = 3.14159274f;
				}
				this.animation = false;
				this.animationState = LiveSphere.AnimationState.None;
				this.UpdateRotateMatrix();
			}
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (primaryTouchEvent.Type == TouchEventType.Down)
			{
				this.animation = false;
				Matrix4 transform3D = this.spherePrim.Transform3D;
				transform3D.M43 = this.radius * 0.2f;
				this.spherePrim.Transform3D = transform3D;
				return;
			}
			if (primaryTouchEvent.Type == TouchEventType.Enter)
			{
				Matrix4 transform3D2 = this.spherePrim.Transform3D;
				transform3D2.M43 = this.radius * 0.2f;
				this.spherePrim.Transform3D = transform3D2;
				return;
			}
			if (primaryTouchEvent.Type == TouchEventType.Leave)
			{
				Matrix4 transform3D3 = this.spherePrim.Transform3D;
				transform3D3.M43 = 0f;
				this.spherePrim.Transform3D = transform3D3;
				return;
			}
			if (primaryTouchEvent.Type == TouchEventType.Up)
			{
				Matrix4 transform3D4 = this.spherePrim.Transform3D;
				transform3D4.M43 = 0f;
				this.spherePrim.Transform3D = transform3D4;
				if (this.animationState == LiveSphere.AnimationState.None && !this.animation)
				{
					if (this.ButtonAction != null)
					{
						this.ButtonAction.Invoke(this, new TouchEventArgs(touchEvents));
						return;
					}
				}
				else
				{
					if (this.animationState == LiveSphere.AnimationState.Drag)
					{
						this.totalTime = 0f;
						this.endAngle = (float)Math.Round((double)(this.yAxis / 3.14159274f)) * 3.14159274f;
						this.flipEndAngle = this.yAxis;
						this.isStopAnimate = true;
						this.animation = true;
						return;
					}
					if (this.animationState == LiveSphere.AnimationState.Flip)
					{
						this.animation = true;
						this.Stop(true);
						return;
					}
					if (this.animationState == LiveSphere.AnimationState.FlickFlip)
					{
						this.animationState = LiveSphere.AnimationState.Flip;
					}
				}
			}
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			if (this.toggleEnabled)
			{
				if (e.Speed.X > 0f && !this.FrontFace)
				{
					this.Start(-1, true);
				}
				else if (e.Speed.X < 0f && this.FrontFace)
				{
					this.Start(1, true);
				}
			}
			else
			{
				int revo = -(int)(e.Speed.X * (float)Math.Cos((double)this.zAxis) / (float)this.flick + e.Speed.Y * (float)Math.Sin((double)this.zAxis) / (float)this.flick);
				this.Start(revo, true);
			}
			Matrix4 transform3D = this.spherePrim.Transform3D;
			transform3D.M43 = 0f;
			this.spherePrim.Transform3D = transform3D;
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			if (this.animationState != LiveSphere.AnimationState.Drag)
			{
				this.animationState = LiveSphere.AnimationState.Drag;
				return;
			}
			this.yAxis += -e.Distance.X * 3.14159274f / 180f;
			if (this.toggleEnabled)
			{
				if (this.yAxis < 0f)
				{
					this.yAxis = 0f;
				}
				else if (this.yAxis > 3.14159274f)
				{
					this.yAxis = 3.14159274f;
				}
			}
			this.UpdateRotateMatrix();
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (this.animation)
			{
				switch (this.animationState)
				{
				case LiveSphere.AnimationState.Flip:
				case LiveSphere.AnimationState.FlickFlip:
					this.totalTime += elapsedTime;
					if (this.isStopAnimate)
					{
						if (this.totalTime < this.flipTime)
						{
							this.totalTime = this.flipTime;
							this.endAngle = (float)Math.Round((double)(this.yAxis / 3.14159274f)) * 3.14159274f;
							this.flipEndAngle = this.yAxis;
						}
						else if (this.totalTime < this.flipTime + this.bounceTime)
						{
							this.yAxis = LiveSphere.ElasticInterpolator(this.flipEndAngle, this.endAngle, (this.totalTime - this.flipTime) / this.bounceTime);
							this.UpdateRotateMatrix();
						}
						else
						{
							this.Stop();
							this.isStopAnimate = false;
						}
					}
					else if (this.totalTime < this.flipTime)
					{
						this.yAxis = this.acceleration / 2f * this.totalTime * this.totalTime + this.firstOmega * this.totalTime + this.startAngle;
						this.UpdateRotateMatrix();
					}
					else if (this.totalTime < this.flipTime + this.bounceTime)
					{
						this.yAxis = LiveSphere.ElasticInterpolator(this.flipEndAngle, this.endAngle, (this.totalTime - this.flipTime) / this.bounceTime);
						this.UpdateRotateMatrix();
					}
					else
					{
						this.Stop();
					}
					break;
				case LiveSphere.AnimationState.Drag:
					if (this.isStopAnimate)
					{
						this.totalTime += elapsedTime;
						if (this.totalTime < this.bounceTime)
						{
							this.yAxis = LiveSphere.ElasticInterpolator(this.flipEndAngle, this.endAngle, this.totalTime / this.bounceTime);
							this.UpdateRotateMatrix();
						}
						else
						{
							this.Stop();
							this.isStopAnimate = false;
						}
					}
					break;
				}
			}
			this.updateShaderUniform();
		}

		private static float ElasticInterpolator(float from, float to, float ratio)
		{
			float num = 0.3f;
			float num2 = to - from;
			if (ratio == 0f)
			{
				return from;
			}
			if (ratio == 1f)
			{
				return to;
			}
			return to - (float)((double)num2 * Math.Pow(2.0, (double)(-10f * ratio)) * Math.Cos((double)ratio * 6.2831853071795862 / (double)num));
		}

		private void UpdateRotateMatrix()
		{
			Matrix4 transform3D = this.spherePrim.Transform3D;
			Matrix4 matrix = Matrix4.Scale(new Vector3(this.radius, this.radius, this.radius));
			matrix.M41 = transform3D.M41;
			matrix.M42 = transform3D.M42;
			matrix.M43 = transform3D.M43;
			Matrix4 matrix2 = Matrix4.RotationZ(this.zAxis);
			matrix.Multiply(ref matrix2, out transform3D);
			matrix = transform3D;
			matrix2 = Matrix4.RotationY(this.yAxis);
			matrix.Multiply(ref matrix2, out transform3D);
			this.spherePrim.Transform3D = transform3D;
		}

		private void updateShaderUniform()
		{
			Matrix4 matrix = this.spherePrim.Transform3D;
			Widget widget = this;
			while (widget != null)
			{
				Matrix4 transform3D = widget.Transform3D;
				PivotType pivotType = widget.PivotType;
				switch (pivotType)
				{
				case PivotType.TopCenter:
					goto IL_59;
				case PivotType.TopRight:
					goto IL_75;
				default:
					switch (pivotType)
					{
					case PivotType.MiddleCenter:
						goto IL_59;
					case PivotType.MiddleRight:
						goto IL_75;
					default:
						switch (pivotType)
						{
						case PivotType.BottomCenter:
							goto IL_59;
						case PivotType.BottomRight:
							goto IL_75;
						}
						break;
					}
					break;
				}
				IL_89:
				PivotType pivotType2 = widget.PivotType;
				switch (pivotType2)
				{
				case PivotType.MiddleLeft:
				case PivotType.MiddleCenter:
				case PivotType.MiddleRight:
					transform3D.M42 -= widget.Height / 2f;
					break;
				default:
					switch (pivotType2)
					{
					case PivotType.BottomLeft:
					case PivotType.BottomCenter:
					case PivotType.BottomRight:
						transform3D.M42 -= widget.Height;
						break;
					}
					break;
				}
				matrix = transform3D * matrix;
				widget = widget.Parent;
				continue;
				IL_59:
				transform3D.M41 -= widget.Width / 2f;
				goto IL_89;
				IL_75:
				transform3D.M41 -= widget.Width;
				goto IL_89;
			}
			Matrix4 matrix2 = matrix.Inverse();
			Vector4 vector = (matrix2 * this.LightDirection).Normalize();
			this.spherePrim.ShaderUniforms["LightDirection"] = new float[]
			{
				vector.X,
				vector.Y,
				vector.Z
			};
			Vector4 vector2 = new Vector4((float)UISystem.FramebufferWidth / 2f, (float)UISystem.FramebufferHeight / 2f, -1000f, 1f);
			vector2 -= matrix.ColumnW;
			vector2 = matrix2 * vector2;
			this.spherePrim.ShaderUniforms["EyePosition"] = new float[]
			{
				vector2.X,
				vector2.Y,
				vector2.Z
			};
			this.LightDirection = new Vector4((float)(Math.Sin((double)this.LightZAngle) * Math.Cos((double)this.LightYAngle)), (float)Math.Cos((double)this.LightZAngle), (float)(Math.Sin((double)this.LightZAngle) * Math.Sin((double)this.LightYAngle)), 0f);
			this.spherePrim.ShaderUniforms["Shininess"] = new float[]
			{
				this.Shininess
			};
			this.spherePrim.ShaderUniforms["Specular"] = new float[]
			{
				this.Specular
			};
		}
	}
}
