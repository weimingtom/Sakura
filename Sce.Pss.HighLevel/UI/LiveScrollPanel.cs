using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveScrollPanel : ContainerWidget
	{
		private enum AnimationState
		{
			None,
			Drag,
			Up,
			Migrate
		}

		private const float textureMarginRatio = 0.25f;

		private UISprite animationSprt;

		private Panel frontPanel;

		private LiveScrollPanel.AnimationState animationState;

		private ImageAsset texImage;

		private Texture2D cacheTexture;

		private Vector2 touchPos;

		private Vector2 touchDownPanelPos;

		private Vector2 panelVelocity;

		private Vector2 touchVelocity;

		private float maxTextureWidth;

		private float maxTextureHeight;

		private bool needPartialTexture;

		private float textureOffsetX;

		private float textureOffsetY;

		private float staticFrictionSq = 5000f;

		private float kineticFriction = 10f;

		private float viscousFriction = 75f;

		private float forceCoeff = 0.0002f;

		private float flickCoeff = 1f;

		private float tensionCoeff = 0.5f;

		private float panelTouchRatio = 0.1f;

		private float touchFrictionRatio = 0.5f;

		private float flickElapsedTime;

		private float defaultMaxTextureWidth = (float)UISystem.GraphicsContext.Caps.MaxTextureSize / UISystem.Scale;

		private float defaultMaxtextureHeight = (float)UISystem.GraphicsContext.Caps.MaxTextureSize / UISystem.Scale;

		private Vector2 migrateDownPanelPos;

		private Vector2 migratePos;

		private bool isFlick;

		private bool isDrag;

		private Vector2 touchPanelPos
		{
			get
			{
				return this.touchPos - new Vector2(this.frontPanel.X, this.frontPanel.Y);
			}
		}

		private Vector2 touchDownPos
		{
			get
			{
				return this.touchDownPanelPos + new Vector2(this.frontPanel.X, this.frontPanel.Y);
			}
			set
			{
				this.touchDownPanelPos = value - new Vector2(this.frontPanel.X, this.frontPanel.Y);
			}
		}

		private Vector2 tension
		{
			get
			{
				return this.touchPos - this.touchDownPos;
			}
		}

		private Vector2 migrateDownPos
		{
			get
			{
				return this.migrateDownPanelPos + new Vector2(this.frontPanel.X, this.frontPanel.Y);
			}
			set
			{
				this.migrateDownPanelPos = value - new Vector2(this.frontPanel.X, this.frontPanel.Y);
			}
		}

		public float Elasticity
		{
			get
			{
				return 0.5f / this.touchFrictionRatio;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("Elasticity");
				}
				this.touchFrictionRatio = 0.5f / value;
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
				this.UpdateView();
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
				this.UpdateView();
			}
		}

		public float PanelX
		{
			get
			{
				return this.frontPanel.X;
			}
			set
			{
				if (this.frontPanel != null)
				{
					this.frontPanel.X = MathUtility.Clamp<float>(value, base.Width - this.frontPanel.Width, 0f);
				}
			}
		}

		public float PanelY
		{
			get
			{
				return this.frontPanel.Y;
			}
			set
			{
				if (this.frontPanel != null)
				{
					this.frontPanel.Y = MathUtility.Clamp<float>(value, base.Height - this.frontPanel.Height, 0f);
				}
			}
		}

		public float PanelWidth
		{
			get
			{
				return this.frontPanel.Width;
			}
			set
			{
				if (this.frontPanel != null)
				{
					this.frontPanel.Width = value;
					this.updateNeedPartialTexture();
				}
				this.UpdateView();
			}
		}

		public float PanelHeight
		{
			get
			{
				return this.frontPanel.Height;
			}
			set
			{
				if (this.frontPanel != null)
				{
					this.frontPanel.Height = value;
					this.updateNeedPartialTexture();
				}
				this.UpdateView();
			}
		}

		public float MaxTextureWidth
		{
			get
			{
				return this.maxTextureWidth;
			}
			set
			{
				this.maxTextureWidth = value;
				if (this.frontPanel != null)
				{
					this.updateNeedPartialTexture();
				}
			}
		}

		public float MaxTextureHeight
		{
			get
			{
				return this.maxTextureHeight;
			}
			set
			{
				this.maxTextureHeight = value;
				if (this.frontPanel != null)
				{
					this.updateNeedPartialTexture();
				}
			}
		}

		public UIColor PanelColor
		{
			get
			{
				return this.frontPanel.BackgroundColor;
			}
			set
			{
				if (this.frontPanel != null)
				{
					this.frontPanel.BackgroundColor = value;
				}
			}
		}

		public bool HorizontalScroll
		{
			get;
			set;
		}

		public bool VerticalScroll
		{
			get;
			set;
		}

		public override IEnumerable<Widget> Children
		{
			get
			{
				return this.frontPanel.Children;
			}
		}

		public LiveScrollPanel()
		{
			this.animationSprt = new UISprite(1);
			this.animationSprt.InternalShaderType = InternalShaderType.LiveScrollPanel;
			this.animationSprt.TextureWrapMode = (TextureWrapMode)1;
			this.animationSprt.BlendMode = BlendMode.Premultiplied;
			this.animationSprt.Visible = false;
			base.RootUIElement.AddChildLast(this.animationSprt);
			this.MaxTextureWidth = this.defaultMaxTextureWidth;
			this.MaxTextureHeight = this.defaultMaxtextureHeight;
			this.needPartialTexture = false;
			this.textureOffsetX = 0f;
			this.textureOffsetY = 0f;
			this.frontPanel = new Panel();
			base.AddChildLast(this.frontPanel);
			this.frontPanel.Clip = true;
			this.frontPanel.X = 0f;
			this.frontPanel.Y = 0f;
			DragGestureDetector dragGestureDetector = new DragGestureDetector();
			dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(dragGestureDetector);
			FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
			flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(flickGestureDetector);
			base.Clip = true;
			this.HorizontalScroll = true;
			this.VerticalScroll = true;
			this.animationState = LiveScrollPanel.AnimationState.None;
			base.HookChildTouchEvent = true;
			base.Width = 600f;
			base.Height = 300f;
			this.UpdateView();
		}

		public override void AddChildFirst(Widget child)
		{
			this.frontPanel.AddChildFirst(child);
		}

		public override void AddChildLast(Widget child)
		{
			this.frontPanel.AddChildLast(child);
		}

		public override void InsertChildBefore(Widget child, Widget nextChild)
		{
			this.frontPanel.InsertChildBefore(child, nextChild);
		}

		public override void InsertChildAfter(Widget child, Widget prevChild)
		{
			this.frontPanel.InsertChildAfter(child, prevChild);
		}

		public override void RemoveChild(Widget child)
		{
			this.frontPanel.RemoveChild(child);
		}

		private void UpdateView()
		{
			if (this.animationSprt == null || this.frontPanel == null)
			{
				return;
			}
			this.animationSprt.GetUnit(0).Width = base.Width;
			this.animationSprt.GetUnit(0).Height = base.Height;
		}

		protected internal override void OnResetState()
		{
			this.animationState = LiveScrollPanel.AnimationState.None;
			this.unsetTexture();
			this.touchPos = default(Vector2);
			this.panelVelocity = default(Vector2);
			this.touchVelocity = default(Vector2);
			this.flickElapsedTime = 0f;
		}

		protected override void DisposeSelf()
		{
			if (this.animationSprt.Image != null)
			{
				this.animationSprt.Image.Dispose();
			}
			if (this.cacheTexture != null)
			{
				this.cacheTexture.Dispose();
			}
			base.DisposeSelf();
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (elapsedTime > 10000f)
			{
				return;
			}
			while (elapsedTime > 35f)
			{
				this.internalUpdate(35f);
				elapsedTime -= 35f;
			}
			this.internalUpdate(elapsedTime);
			this.updateSprt();
			if (this.touchVelocity.LengthSquared() + this.panelVelocity.LengthSquared() > 100f || this.flickElapsedTime > 10000f)
			{
				this.OnResetState();
			}
		}

		private void internalUpdate(float elapsedTime)
		{
			if (this.animationState == LiveScrollPanel.AnimationState.Drag || this.animationState == LiveScrollPanel.AnimationState.Migrate)
			{
				float num;
				float num2;
				if (this.needPartialTexture)
				{
					num = this.CalculateTextureOffsetX();
					num2 = this.CalculateTextureOffsetY();
				}
				else
				{
					num = 0f;
					num2 = 0f;
				}
				if (this.texImage == null)
				{
					this.textureOffsetX = num;
					this.textureOffsetY = num2;
					this.setTexture();
				}
				else if (this.textureOffsetX != num || this.textureOffsetY != num2)
				{
					this.textureOffsetX = num;
					this.textureOffsetY = num2;
					this.unsetTexture();
					this.setTexture();
				}
				if (this.animationState == LiveScrollPanel.AnimationState.Migrate)
				{
					bool flag = true;
					Vector2 vector = this.migratePos - this.touchPos;
					if (vector.LengthSquared() > 4f)
					{
						this.touchPos += vector * (elapsedTime / 100f) + vector.Normalize() * 2f;
						flag = false;
					}
					else
					{
						this.touchPos = this.migratePos;
					}
					Vector2 vector2 = this.migrateDownPos - this.touchDownPos;
					if (vector2.LengthSquared() > 4f)
					{
						this.touchDownPos += vector2 * (elapsedTime / 100f) + vector2.Normalize() * 2f;
						flag = false;
					}
					else
					{
						this.touchDownPos = this.migrateDownPos;
					}
					if (flag)
					{
						this.animationState = LiveScrollPanel.AnimationState.Drag;
					}
				}
				if (this.panelVelocity.LengthSquared() > 1E-09f)
				{
					Vector2 vector3 = -this.panelVelocity.Normalize() * this.kineticFriction - this.panelVelocity * this.viscousFriction;
					this.panelVelocity += (this.tension + vector3) * elapsedTime * this.forceCoeff;
				}
				else if (this.tension.LengthSquared() > (this.isFlick ? (this.staticFrictionSq / 100f) : this.staticFrictionSq))
				{
					this.panelVelocity += this.tension * elapsedTime * this.forceCoeff;
				}
				if (this.HorizontalScroll)
				{
					this.frontPanel.X += this.panelVelocity.X * elapsedTime;
				}
				if (this.VerticalScroll)
				{
					this.frontPanel.Y += this.panelVelocity.Y * elapsedTime;
				}
				this.adjustPanelEdge();
				return;
			}
			if (this.animationState == LiveScrollPanel.AnimationState.Up)
			{
				if (this.needPartialTexture)
				{
					float num3 = this.CalculateTextureOffsetX();
					float num4 = this.CalculateTextureOffsetY();
					if (this.textureOffsetX != num3 || this.textureOffsetY != num4)
					{
						this.textureOffsetX = num3;
						this.textureOffsetY = num4;
						this.unsetTexture();
						this.setTexture();
					}
				}
				if (this.texImage == null || (this.tension.LengthSquared() < 4f && this.touchVelocity.LengthSquared() < 0.01f && this.panelVelocity.LengthSquared() < 0.01f))
				{
					this.OnResetState();
					return;
				}
				if (this.panelVelocity.LengthSquared() > 0.0001f)
				{
					Vector2 vector4 = -this.panelVelocity.Normalize() * this.kineticFriction - this.panelVelocity * this.viscousFriction;
					this.panelVelocity += (this.tension + vector4) * elapsedTime * this.forceCoeff * this.panelTouchRatio;
				}
				else if (this.tension.LengthSquared() > this.staticFrictionSq || this.isFlick)
				{
					this.panelVelocity += this.tension * elapsedTime * this.forceCoeff * this.panelTouchRatio;
				}
				if (!this.isFlick)
				{
					if (this.frontPanel.X > -10f && this.panelVelocity.X < 0f)
					{
						this.panelVelocity.X = (this.frontPanel.X + 10f) * elapsedTime * this.forceCoeff;
					}
					else if (this.frontPanel.X < this.Width - this.frontPanel.Width + 10f && this.panelVelocity.X > 0f)
					{
						this.panelVelocity.X = (this.frontPanel.X - (this.Width - this.frontPanel.Width + 10f)) * elapsedTime * this.forceCoeff;
					}
					if (this.frontPanel.Y > -10f && this.panelVelocity.Y < 0f)
					{
						this.panelVelocity.Y = (this.frontPanel.Y + 10f) * elapsedTime * this.forceCoeff;
					}
					else if (this.frontPanel.Y < this.Height - this.frontPanel.Height + 10f && this.panelVelocity.Y > 0f)
					{
						this.panelVelocity.Y = (this.frontPanel.Y - (this.Height - this.frontPanel.Height + 10f)) * elapsedTime * this.forceCoeff;
					}
				}
				this.isFlick = false;
				Vector2 vector5 = default(Vector2);
				if (this.touchVelocity.LengthSquared() > 0.0001f)
				{
					vector5 = -this.touchVelocity.Normalize() * this.kineticFriction - this.touchVelocity * this.viscousFriction;
					vector5 *= this.touchFrictionRatio;
				}
				if (this.touchVelocity.LengthSquared() < 0.01f && this.tension.LengthSquared() < 50f)
				{
					this.touchVelocity += (vector5 - this.tension * (1f + 100f / this.tension.LengthSquared())) * elapsedTime * this.forceCoeff * (1f - this.panelTouchRatio);
					if (this.tension.LengthSquared() < 1.401298E-45f || this.touchVelocity.Dot(this.tension) / this.tension.LengthSquared() * elapsedTime < -1f)
					{
						this.touchVelocity = -this.tension / elapsedTime;
					}
				}
				else
				{
					this.touchVelocity += (vector5 - this.tension) * elapsedTime * this.forceCoeff * (1f - this.panelTouchRatio);
				}
				if (this.tension.LengthSquared() > 1.401298E-45f)
				{
					this.touchVelocity = this.tension * this.touchVelocity.Dot(this.tension) / this.tension.LengthSquared();
				}
				if (this.HorizontalScroll)
				{
					this.frontPanel.X += this.panelVelocity.X * elapsedTime;
				}
				if (this.VerticalScroll)
				{
					this.frontPanel.Y += this.panelVelocity.Y * elapsedTime;
				}
				this.touchPos += this.touchVelocity * elapsedTime;
				this.adjustPanelEdge();
			}
		}

		private void adjustPanelEdge()
		{
			if (this.frontPanel.X > 0f)
			{
				this.frontPanel.X = 0f;
				if (this.panelVelocity.X > 0f)
				{
					this.panelVelocity.X = 0f;
				}
			}
			if (this.frontPanel.X < this.Width - this.frontPanel.Width)
			{
				this.frontPanel.X = this.Width - this.frontPanel.Width;
				if (this.panelVelocity.X < 0f)
				{
					this.panelVelocity.X = 0f;
				}
			}
			if (this.frontPanel.Y > 0f)
			{
				this.frontPanel.Y = 0f;
				if (this.panelVelocity.Y > 0f)
				{
					this.panelVelocity.Y = 0f;
				}
			}
			if (this.frontPanel.Y < this.Height - this.frontPanel.Height)
			{
				this.frontPanel.Y = this.Height - this.frontPanel.Height;
				if (this.panelVelocity.Y < 0f)
				{
					this.panelVelocity.Y = 0f;
				}
			}
		}

		private void setTexture()
		{
			float num;
			float num2;
			if (this.needPartialTexture)
			{
				num = FMath.Min(this.frontPanel.Width, this.MaxTextureWidth) * UISystem.Scale;
				num2 = FMath.Min(this.frontPanel.Height, this.MaxTextureHeight) * UISystem.Scale;
			}
			else
			{
				num = this.frontPanel.Width * UISystem.Scale;
				num2 = this.frontPanel.Height * UISystem.Scale;
			}
			if (this.cacheTexture == null || this.cacheTexture.Width != (int)num || this.cacheTexture.Height != (int)num2)
			{
				if (this.cacheTexture != null)
				{
					this.cacheTexture.Dispose();
				}
				this.cacheTexture = new Texture2D((int)num, (int)num2, false, (PixelFormat)1, (PixelBufferOption)1);
			}
			Matrix4 matrix = new Matrix4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, -this.textureOffsetX, -this.textureOffsetY, 0f, 1f);
			FrameBuffer frameBuffer = new FrameBuffer();
			frameBuffer.SetColorTarget(this.cacheTexture, 0);
			this.frontPanel.RenderToFrameBuffer(frameBuffer, ref matrix, true);
			frameBuffer.Dispose();
			if (this.animationSprt.Image != null)
			{
				this.animationSprt.Image.Dispose();
			}
			this.animationSprt.Image = new ImageAsset(this.cacheTexture);
			this.animationSprt.Image.AdjustScaledSize = true;
			this.texImage = this.animationSprt.Image;
			this.animationSprt.Visible = true;
			this.frontPanel.Visible = false;
		}

		private void unsetTexture()
		{
			this.animationSprt.Visible = false;
			this.frontPanel.Visible = true;
			this.texImage = null;
			Dictionary<string, float[]> arg_37_0 = this.animationSprt.ShaderUniforms;
			string arg_37_1 = "u_touchCoord";
			float[] array = new float[2];
			arg_37_0[arg_37_1] = array;
			Dictionary<string, float[]> arg_54_0 = this.animationSprt.ShaderUniforms;
			string arg_54_1 = "u_tension";
			float[] array2 = new float[2];
			arg_54_0[arg_54_1] = array2;
		}

		private void updateSprt()
		{
			if (this.texImage != null)
			{
				UISpriteUnit unit = this.animationSprt.GetUnit(0);
				unit.Width = this.Width;
				unit.Height = this.Height;
				unit.U1 = (-this.frontPanel.X - this.textureOffsetX) / (float)this.texImage.Width;
				unit.V1 = (-this.frontPanel.Y - this.textureOffsetY) / (float)this.texImage.Height;
				unit.U2 = (-this.frontPanel.X - this.textureOffsetX + this.Width) / (float)this.texImage.Width;
				unit.V2 = (-this.frontPanel.Y - this.textureOffsetY + this.Height) / (float)this.texImage.Height;
				this.animationSprt.ShaderUniforms["u_touchCoord"] = new float[]
				{
					(this.touchPos.X - this.frontPanel.X - this.textureOffsetX) / (float)this.texImage.Width,
					(this.touchPos.Y - this.frontPanel.Y - this.textureOffsetY) / (float)this.texImage.Height
				};
				this.animationSprt.ShaderUniforms["u_tension"] = new float[]
				{
					this.tension.X / (float)this.texImage.Width * this.tensionCoeff,
					this.tension.Y / (float)this.texImage.Height * this.tensionCoeff
				};
				this.animationSprt.ShaderUniforms["u_clipRect"] = new float[]
				{
					this.Width / (float)this.texImage.Width,
					this.Height / (float)this.texImage.Height
				};
			}
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (primaryTouchEvent.Type == TouchEventType.Down)
			{
				this.isFlick = false;
				if (this.animationState == LiveScrollPanel.AnimationState.Up)
				{
					this.animationState = LiveScrollPanel.AnimationState.Migrate;
					this.migrateDownPos = primaryTouchEvent.LocalPosition;
					this.migratePos = this.migrateDownPos;
				}
				else
				{
					this.touchDownPos = primaryTouchEvent.LocalPosition;
					this.touchPos = this.touchDownPos;
					touchEvents.Forward = true;
				}
			}
			else if (primaryTouchEvent.Type == TouchEventType.Up)
			{
				this.isDrag = false;
			}
			if (!this.isDrag)
			{
				touchEvents.Forward = true;
			}
			if (primaryTouchEvent.Type == TouchEventType.Up)
			{
				this.animationState = LiveScrollPanel.AnimationState.Up;
				this.flickElapsedTime = 0f;
			}
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			base.ResetState(false);
			this.isDrag = true;
			if (this.animationState == LiveScrollPanel.AnimationState.None)
			{
				this.animationState = LiveScrollPanel.AnimationState.Migrate;
				this.migrateDownPos = e.LocalPosition;
				this.migratePos = this.migrateDownPos;
				return;
			}
			if (this.animationState == LiveScrollPanel.AnimationState.Migrate)
			{
				this.migratePos = e.LocalPosition;
				return;
			}
			this.animationState = LiveScrollPanel.AnimationState.Drag;
			this.touchPos = e.LocalPosition;
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			base.ResetState(false);
			this.animationState = LiveScrollPanel.AnimationState.Up;
			this.isFlick = true;
			this.touchVelocity = e.Speed / 1000f * this.flickCoeff;
			this.panelVelocity += e.Speed / 1000f * this.flickCoeff;
		}

		private void updateNeedPartialTexture()
		{
			if (this.frontPanel.Width > this.MaxTextureWidth || this.frontPanel.Height > this.MaxTextureHeight)
			{
				this.needPartialTexture = true;
				return;
			}
			this.needPartialTexture = false;
		}

		private float CalculateTextureOffsetX()
		{
			float spritePosition = -this.frontPanel.X;
			float textureMarginSize = this.Width * 0.25f;
			return LiveScrollPanel.CalculateTextureOffset(spritePosition, this.Width, this.textureOffsetX, textureMarginSize, this.MaxTextureWidth, this.frontPanel.Width);
		}

		private float CalculateTextureOffsetY()
		{
			float spritePosition = -this.frontPanel.Y;
			float textureMarginSize = this.Height * 0.25f;
			return LiveScrollPanel.CalculateTextureOffset(spritePosition, this.Height, this.textureOffsetY, textureMarginSize, this.MaxTextureHeight, this.frontPanel.Height);
		}

		private static float CalculateTextureOffset(float spritePosition, float spriteSize, float offsetPosition, float textureMarginSize, float maxTextureSize, float frontPanelSize)
		{
			float result;
			if (spritePosition < maxTextureSize - textureMarginSize - spriteSize)
			{
				result = 0f;
			}
			else if (spritePosition > frontPanelSize - spriteSize - textureMarginSize)
			{
				result = FMath.Max(frontPanelSize - maxTextureSize, 0f);
			}
			else if (spritePosition > offsetPosition + textureMarginSize && spritePosition < offsetPosition + maxTextureSize - textureMarginSize - spriteSize)
			{
				result = offsetPosition;
			}
			else
			{
				result = spritePosition + spriteSize / 2f - maxTextureSize / 2f;
			}
			return result;
		}
	}
}
