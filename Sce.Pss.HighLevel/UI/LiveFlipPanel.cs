using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveFlipPanel : Widget
	{
		private enum AnimationState
		{
			None,
			Flip,
			FlickFlip,
			Stopping,
			Drag
		}

		private LiveFlipPanel.AnimationState animationState;

		private int flipCount = 16;

		private bool touchEnabled = true;

		private float bounceTime = 750f;

		private float acceleration = -0.0001f;

		private float flipTime;

		private float firstOmega;

		private float secondOmega;

		private float angle;

		private float startAngle;

		private float flipEndAngle;

		private float endAngle;

		private float totalTime;

		private bool isStopAnimate;

		public bool isUseSprite;

		private FlickGestureDetector flickGesture;

		private DragGestureDetector dragGesture;

		private Panel frontPanel;

		private Panel backPanel;

		private UISprite frontSprt;

		private UISprite backSprt;

		private Texture2D frontTexture;

		private Texture2D backTexture;

		private bool animation;

		public int FlipCount
		{
			get
			{
				return this.flipCount;
			}
			set
			{
				this.flipCount = value;
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
					base.HookChildTouchEvent = true;
					base.AddGestureDetector(this.flickGesture);
					return;
				}
				base.HookChildTouchEvent = false;
				base.RemoveGestureDetector(this.flickGesture);
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
				base.Width = value;
				if (this.frontPanel != null && this.backPanel != null && this.frontSprt != null && this.frontSprt != null)
				{
					this.frontPanel.Width = value;
					this.backPanel.Width = value;
					if (!this.isUseSprite)
					{
						this.frontPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
						this.backPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
					}
					this.updateSprtSize();
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
				base.Height = value;
				if (this.frontPanel != null && this.backPanel != null && this.frontSprt != null && this.frontSprt != null)
				{
					this.frontPanel.Height = value;
					this.backPanel.Height = value;
					if (!this.isUseSprite)
					{
						this.frontPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
						this.backPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
					}
					this.updateSprtSize();
				}
			}
		}

		public Panel FrontPanel
		{
			get
			{
				return this.frontPanel;
			}
			set
			{
				if (value != null)
				{
					base.RemoveChild(this.frontPanel);
					this.frontPanel = value;
					this.frontPanel.Width = base.Width;
					this.frontPanel.Height = base.Height;
					if (!this.isUseSprite)
					{
						this.frontPanel.PivotType = PivotType.MiddleCenter;
						this.frontPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
					}
					base.AddChildFirst(this.frontPanel);
				}
			}
		}

		public Panel BackPanel
		{
			get
			{
				return this.backPanel;
			}
			set
			{
				if (value != null)
				{
					base.RemoveChild(this.backPanel);
					this.backPanel = value;
					this.backPanel.Width = base.Width;
					this.backPanel.Height = base.Height;
					this.backPanel.Visible = false;
					if (!this.isUseSprite)
					{
						this.backPanel.PivotType = PivotType.MiddleCenter;
						this.backPanel.SetPosition(this.frontPanel.Width / 2f, this.frontPanel.Height / 2f);
					}
					base.AddChildLast(this.backPanel);
				}
			}
		}

		public LiveFlipPanel()
		{
			this.frontPanel = new Panel();
			if (!this.isUseSprite)
			{
				this.frontPanel.PivotType = PivotType.MiddleCenter;
			}
			base.AddChildLast(this.frontPanel);
			this.backPanel = new Panel();
			if (!this.isUseSprite)
			{
				this.backPanel.PivotType = PivotType.MiddleCenter;
			}
			base.AddChildLast(this.backPanel);
			this.backPanel.Visible = false;
			this.frontSprt = new UISprite(1);
			this.frontSprt.ShaderType = ShaderType.OffscreenTexture;
			this.frontSprt.Culling = true;
			this.frontSprt.Visible = false;
			base.RootUIElement.AddChildLast(this.frontSprt);
			this.backSprt = new UISprite(1);
			this.backSprt.ShaderType = ShaderType.OffscreenTexture;
			this.backSprt.Culling = true;
			this.backSprt.Visible = false;
			base.RootUIElement.AddChildLast(this.backSprt);
			base.HookChildTouchEvent = true;
			this.flickGesture = new FlickGestureDetector();
			this.flickGesture.Direction = FlickDirection.Horizontal;
			this.flickGesture.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(this.flickGesture);
			this.dragGesture = new DragGestureDetector();
			this.dragGesture.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(this.dragGesture);
			this.Width = 100f;
			this.Height = 100f;
		}

		protected override void DisposeSelf()
		{
			this.removeTextures();
			base.DisposeSelf();
		}

		private void updateSprtSize()
		{
			this.removeTextures();
			this.frontSprt.X = base.Width / 2f;
			this.frontSprt.Y = base.Height / 2f;
			UISpriteUnit unit = this.frontSprt.GetUnit(0);
			unit.Width = base.Width;
			unit.Height = base.Height;
			unit.X = -this.frontSprt.X;
			unit.Y = -this.frontSprt.Y;
			this.backSprt.X = this.frontSprt.X;
			this.backSprt.Y = this.frontSprt.Y;
			unit = this.backSprt.GetUnit(0);
			unit.Width = base.Width;
			unit.Height = base.Height;
			unit.X = -this.backSprt.X;
			unit.Y = -this.backSprt.Y;
		}

		private void removeTextures()
		{
			if (this.frontSprt != null && this.frontSprt.Image != null)
			{
				this.frontSprt.Image.Dispose();
				this.frontSprt.Image = null;
			}
			if (this.backSprt != null && this.backSprt.Image != null)
			{
				this.backSprt.Image.Dispose();
				this.backSprt.Image = null;
			}
			if (this.frontTexture != null)
			{
				this.frontTexture.Dispose();
				this.frontTexture = null;
			}
			if (this.backTexture != null)
			{
				this.backTexture.Dispose();
				this.backTexture = null;
			}
		}

		public void Start()
		{
			this.Start(this.flipCount, false);
		}

		private void Start(int revo, bool isFlick)
		{
			if (revo == 0)
			{
				return;
			}
			this.startAngle = this.angle;
			this.endAngle = this.startAngle + (float)revo * 3.14159274f;
			this.endAngle = (float)Math.Round((double)(this.endAngle / 3.14159274f)) * 3.14159274f;
			if (revo > 0)
			{
				this.flipEndAngle = this.endAngle - 1.57079637f;
				this.acceleration = -1f * Math.Abs(this.acceleration);
				this.secondOmega = 10.88f / this.bounceTime;
				this.firstOmega = (float)Math.Sqrt((double)(this.secondOmega * this.secondOmega + 2f * this.acceleration * (this.startAngle - this.flipEndAngle)));
			}
			else
			{
				this.flipEndAngle = this.endAngle + 1.57079637f;
				this.acceleration = Math.Abs(this.acceleration);
				this.secondOmega = -10.88f / this.bounceTime;
				this.firstOmega = -(float)Math.Sqrt((double)(this.secondOmega * this.secondOmega + 2f * this.acceleration * (this.startAngle - this.flipEndAngle)));
			}
			this.totalTime = 0f;
			this.flipTime = (this.secondOmega - this.firstOmega) / this.acceleration;
			this.isStopAnimate = false;
			if (this.isUseSprite)
			{
				this.setupSprite();
			}
			this.animation = true;
			if (isFlick)
			{
				this.animationState = LiveFlipPanel.AnimationState.FlickFlip;
				return;
			}
			this.animationState = LiveFlipPanel.AnimationState.Flip;
		}

		private void setupSprite()
		{
			if (this.frontTexture == null || this.backTexture == null)
			{
				int num;
				int num2;
				if (UISystem.Scaled)
				{
					num = (int)this.Width;
					num2 = (int)this.Height;
				}
				else
				{
					num = (int)(this.Width * UISystem.Scale);
					num2 = (int)(this.Height * UISystem.Scale);
				}
				this.frontTexture = new Texture2D(num, num2, false, (PixelFormat)1, (PixelBufferOption)1);
				this.frontSprt.Image = new ImageAsset(this.frontTexture);
				this.frontSprt.Image.AdjustScaledSize = true;
				this.backTexture = new Texture2D(num, num2, false, (PixelFormat)1, (PixelBufferOption)1);
				this.backSprt.Image = new ImageAsset(this.backTexture);
				this.backSprt.Image.AdjustScaledSize = true;
			}
			this.frontPanel.Visible = true;
			this.backPanel.Visible = true;
			this.frontPanel.RenderToTexture(this.frontTexture);
			this.backPanel.RenderToTexture(this.backTexture);
			this.frontPanel.Visible = false;
			this.backPanel.Visible = false;
			this.frontSprt.Visible = true;
			this.frontSprt.BlendMode = BlendMode.Premultiplied;
			this.backSprt.Visible = true;
			this.backSprt.BlendMode = BlendMode.Premultiplied;
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
				this.UpdateRotateMatrix();
				if (this.frontPanel.Visible)
				{
					this.angle = 0f;
				}
				else
				{
					this.angle = 3.14159274f;
				}
				this.frontSprt.Visible = false;
				this.backSprt.Visible = false;
				this.animation = false;
				this.animationState = LiveFlipPanel.AnimationState.None;
			}
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (primaryTouchEvent.Type == TouchEventType.Down)
			{
				this.animation = false;
			}
			else if (primaryTouchEvent.Type == TouchEventType.Up)
			{
				if (this.animationState == LiveFlipPanel.AnimationState.Drag)
				{
					this.totalTime = 0f;
					this.endAngle = (float)Math.Round((double)(this.angle / 3.14159274f)) * 3.14159274f;
					this.flipEndAngle = this.angle;
					if (this.isUseSprite)
					{
						this.setupSprite();
					}
					this.isStopAnimate = true;
					this.animation = true;
				}
				else if (this.animationState == LiveFlipPanel.AnimationState.Flip)
				{
					this.animation = true;
					this.Stop(true);
				}
				else if (this.animationState == LiveFlipPanel.AnimationState.FlickFlip)
				{
					this.animationState = LiveFlipPanel.AnimationState.Flip;
				}
			}
			if (this.animationState != LiveFlipPanel.AnimationState.None)
			{
				touchEvents.Forward = true;
			}
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			int num = 1;
			int num2 = this.flipCount;
			float num3 = (float)(num2 - num) / ((this.flickGesture.MaxSpeed - this.flickGesture.MinSpeed) * (this.flickGesture.MaxSpeed - this.flickGesture.MinSpeed));
			float num4 = num3 * (Math.Abs(e.Speed.X) - this.flickGesture.MinSpeed) * (Math.Abs(e.Speed.X) - this.flickGesture.MinSpeed) + (float)num;
			this.Start((e.Speed.X > 0f) ? (-(int)num4) : ((int)num4), true);
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			if (this.animationState != LiveFlipPanel.AnimationState.Drag)
			{
				if (this.animationState != LiveFlipPanel.AnimationState.Flip)
				{
					base.ResetState(false);
					if (this.isUseSprite)
					{
						this.setupSprite();
					}
				}
				this.animationState = LiveFlipPanel.AnimationState.Drag;
				return;
			}
			this.angle += -e.Distance.X * 3.14159274f / 180f;
			this.UpdateRotateMatrix();
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (!this.animation)
			{
				return;
			}
			switch (this.animationState)
			{
			case LiveFlipPanel.AnimationState.Flip:
			case LiveFlipPanel.AnimationState.FlickFlip:
				this.totalTime += elapsedTime;
				if (this.isStopAnimate)
				{
					if (this.totalTime < this.flipTime)
					{
						this.totalTime = this.flipTime;
						this.endAngle = (float)Math.Round((double)(this.angle / 3.14159274f)) * 3.14159274f;
						this.flipEndAngle = this.angle;
						return;
					}
					if (this.totalTime < this.flipTime + this.bounceTime)
					{
						this.angle = LiveFlipPanel.ElasticInterpolator(this.flipEndAngle, this.endAngle, (this.totalTime - this.flipTime) / this.bounceTime);
						this.UpdateRotateMatrix();
						return;
					}
					this.Stop();
					this.isStopAnimate = false;
					return;
				}
				else
				{
					if (this.totalTime < this.flipTime)
					{
						this.angle = this.acceleration / 2f * this.totalTime * this.totalTime + this.firstOmega * this.totalTime + this.startAngle;
						this.UpdateRotateMatrix();
						return;
					}
					if (this.totalTime < this.flipTime + this.bounceTime)
					{
						this.angle = LiveFlipPanel.ElasticInterpolator(this.flipEndAngle, this.endAngle, (this.totalTime - this.flipTime) / this.bounceTime);
						this.UpdateRotateMatrix();
						return;
					}
					this.Stop();
					return;
				}
//				break;
				
			case LiveFlipPanel.AnimationState.Stopping:
				break;
			case LiveFlipPanel.AnimationState.Drag:
				if (this.isStopAnimate)
				{
					this.totalTime += elapsedTime;
					if (this.totalTime < this.bounceTime)
					{
						this.angle = LiveFlipPanel.ElasticInterpolator(this.flipEndAngle, this.endAngle, this.totalTime / this.bounceTime);
						this.UpdateRotateMatrix();
						return;
					}
					this.Stop();
					this.isStopAnimate = false;
				}
				break;
			default:
				return;
			}
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
			if (this.isUseSprite)
			{
				Matrix4 transform3D = Matrix4.RotationY(this.angle);
				transform3D.ColumnW = (this.frontSprt.Transform3D.ColumnW);
				this.frontSprt.Transform3D = transform3D;
				transform3D = Matrix4.RotationY(this.angle + 3.14159274f);
				transform3D.ColumnW = (this.backSprt.Transform3D.ColumnW);
				this.backSprt.Transform3D = transform3D;
				return;
			}
			Matrix4 transform3D2 = Matrix4.RotationY(this.angle);
			transform3D2.ColumnW = (this.frontPanel.Transform3D.ColumnW);
			this.frontPanel.Transform3D = transform3D2;
			transform3D2 = Matrix4.RotationY(this.angle + 3.14159274f);
			transform3D2.ColumnW = (this.backPanel.Transform3D.ColumnW);
			this.backPanel.Transform3D = transform3D2;
			Matrix4 matrix = this.frontPanel.Transform3D;
			Widget parent = this.frontPanel.Parent;
			while (parent != null)
			{
				Matrix4 transform3D3 = parent.Transform3D;
				PivotType pivotType = parent.PivotType;
				switch (pivotType)
				{
				case PivotType.TopCenter:
					goto IL_145;
				case PivotType.TopRight:
					goto IL_161;
				default:
					switch (pivotType)
					{
					case PivotType.MiddleCenter:
						goto IL_145;
					case PivotType.MiddleRight:
						goto IL_161;
					default:
						switch (pivotType)
						{
						case PivotType.BottomCenter:
							goto IL_145;
						case PivotType.BottomRight:
							goto IL_161;
						}
						break;
					}
					break;
				}
				IL_175:
				PivotType pivotType2 = parent.PivotType;
				switch (pivotType2)
				{
				case PivotType.MiddleLeft:
				case PivotType.MiddleCenter:
				case PivotType.MiddleRight:
					transform3D3.M42 -= parent.Height / 2f;
					break;
				default:
					switch (pivotType2)
					{
					case PivotType.BottomLeft:
					case PivotType.BottomCenter:
					case PivotType.BottomRight:
						transform3D3.M42 -= parent.Height;
						break;
					}
					break;
				}
				matrix = transform3D3 * matrix;
				parent = parent.Parent;
				continue;
				IL_145:
				transform3D3.M41 -= parent.Width / 2f;
				goto IL_175;
				IL_161:
				transform3D3.M41 -= parent.Width;
				goto IL_175;
			}
			Vector3 vector = new Vector3((float)UISystem.FramebufferWidth / 2f, (float)UISystem.FramebufferHeight / 2f, -1000f);
			Vector3 xyz = matrix.ColumnW.Xyz;
			float num = matrix.ColumnZ.Xyz.Dot(vector - xyz);
			if (num < 0f)
			{
				this.frontPanel.Visible = true;
				this.backPanel.Visible = false;
				return;
			}
			this.frontPanel.Visible = false;
			this.backPanel.Visible = true;
		}
	}
}
