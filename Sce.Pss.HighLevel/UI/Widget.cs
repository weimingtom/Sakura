using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class Widget : IDisposable
	{
		private const int PivotHorizontalMask = 15;

		private const int PivotVerticalMask = 240;

		private string name = "";

		private RootUIElement rootUIElement;

		private float width;

		private float height;

		private PivotType pivotType;

		private bool touchMode;

		private bool clip;

		private LinkedTree<Widget> linkedTree;

		private bool _needUpdateLocalToWorld = true;

		internal float finalClipX;

		internal float finalClipY;

		internal float finalClipWidth;

		internal float finalClipHeight;

		private Matrix4 localToWorld;

		public event EventHandler<TouchEventArgs> TouchEventReceived;

		public event EventHandler<KeyEventArgs> KeyEventReceived;

		public event EventHandler<MotionEventArgs> MotionEventReceived;

		internal bool Disposed
		{
			get;
			private set;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		protected internal RootUIElement RootUIElement
		{
			get
			{
				return this.rootUIElement;
			}
		}

		public virtual float X
		{
			get
			{
				return this.RootUIElement.X;
			}
			set
			{
				this.RootUIElement.X = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public virtual float Y
		{
			get
			{
				return this.RootUIElement.Y;
			}
			set
			{
				this.RootUIElement.Y = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public virtual float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if ((this.pivotType & (PivotType)15) != PivotType.TopLeft)
				{
					this.NeedUpdateLocalToWorld = true;
				}
				this.width = value;
			}
		}

		public virtual float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if ((this.pivotType & (PivotType)240) != PivotType.TopLeft)
				{
					this.NeedUpdateLocalToWorld = true;
				}
				this.height = value;
			}
		}

		public virtual Matrix4 Transform3D
		{
			get
			{
				return this.RootUIElement.transform3D;
			}
			set
			{
				this.RootUIElement.transform3D = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public virtual float Alpha
		{
			get
			{
				return this.RootUIElement.Alpha;
			}
			set
			{
				this.RootUIElement.Alpha = value;
			}
		}

		public virtual PivotType PivotType
		{
			get
			{
				return this.pivotType;
			}
			set
			{
				this.X -= this.PivotAlignmentX;
				this.Y -= this.PivotAlignmentY;
				this.pivotType = value;
				this.X += this.PivotAlignmentX;
				this.Y += this.PivotAlignmentY;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public virtual bool ZSort
		{
			get
			{
				return this.RootUIElement.ZSort;
			}
			set
			{
				this.RootUIElement.ZSort = value;
			}
		}

		public virtual float ZSortOffset
		{
			get
			{
				return this.RootUIElement.ZSortOffset;
			}
			set
			{
				this.RootUIElement.ZSortOffset = value;
			}
		}

		public virtual Anchors Anchors
		{
			get;
			set;
		}

		public virtual bool Visible
		{
			get;
			set;
		}

		public virtual bool TouchResponse
		{
			get
			{
				return this.touchMode;
			}
			set
			{
				this.touchMode = value;
				if (!this.touchMode)
				{
					this.ResetState(true);
				}
			}
		}

		protected internal bool Clip
		{
			get
			{
				return this.clip;
			}
			set
			{
				this.clip = value;
			}
		}

		protected internal bool HookChildTouchEvent
		{
			get;
			set;
		}

		internal LinkedTree<Widget> LinkedTree
		{
			get
			{
				return this.linkedTree;
			}
		}

		public Widget Parent
		{
			get
			{
				LinkedTree<Widget> parent = this.linkedTree.Parent;
				if (parent != null)
				{
					return parent.Value;
				}
				return null;
			}
		}

		protected IEnumerable<Widget> Children
		{
			get
			{
				LinkedTree<Widget> linkedTree = this.linkedTree.FirstChild;
				while (linkedTree != null && linkedTree.Value != null)
				{
					yield return linkedTree.Value;
					linkedTree = linkedTree.NextSibling;
				}
				yield break;
			}
		}

		public Widget LocalKeyReceiverWidget
		{
			get;
			set;
		}

		internal bool NeedUpdateLocalToWorld
		{
			get
			{
				return this._needUpdateLocalToWorld;
			}
			set
			{
				if (!this._needUpdateLocalToWorld && value)
				{
					LinkedTree<Widget> nextAsList = this.linkedTree.LastDescendant.NextAsList;
					for (LinkedTree<Widget> nextAsList2 = this.linkedTree.NextAsList; nextAsList2 != nextAsList; nextAsList2 = nextAsList2.NextAsList)
					{
						nextAsList2.Value._needUpdateLocalToWorld = true;
						nextAsList2.Value.RootUIElement.NeedUpdateLocalToWorld = true;
					}
					this.RootUIElement.NeedUpdateLocalToWorld = true;
				}
				this._needUpdateLocalToWorld = value;
			}
		}

		public Matrix4 LocalToWorld
		{
			get
			{
				this.updateLocalToWorld();
				return this.localToWorld;
			}
		}

		private float PivotAlignmentX
		{
			get
			{
				switch (this.pivotType & (PivotType)15)
				{
				case PivotType.TopLeft:
					return 0f;
				case PivotType.TopCenter:
					return this.Width / 2f;
				case PivotType.TopRight:
					return this.Width;
				default:
					return 0f;
				}
			}
		}

		private float PivotAlignmentY
		{
			get
			{
				int num = (int)(this.pivotType & (PivotType)240);
				if (num == 0)
				{
					return 0f;
				}
				if (num == 16)
				{
					return this.Height / 2f;
				}
				if (num != 32)
				{
					return 0f;
				}
				return this.Height;
			}
		}

		public virtual bool PriorityHit
		{
			get;
			set;
		}

		internal List<GestureDetector> GestureDetectors
		{
			get;
			private set;
		}

		public Widget()
		{
			this.linkedTree = new LinkedTree<Widget>(this);
			this.rootUIElement = new RootUIElement(this);
			this.PivotType = PivotType.TopLeft;
			this.Anchors = (Anchors.Top | Anchors.Height | Anchors.Left | Anchors.Width);
			this.TouchResponse = true;
			this.Visible = true;
			this.Width = 0f;
			this.Height = 0f;
			this.TouchEventReceived = null;
			this.KeyEventReceived = null;
			this.MotionEventReceived = null;
			this.Clip = false;
			this.HookChildTouchEvent = false;
			this.finalClipX = 0f;
			this.finalClipY = 0f;
			this.finalClipWidth = 0f;
			this.finalClipHeight = 0f;
			this.PriorityHit = false;
			this.GestureDetectors = new List<GestureDetector>();
			this.LocalKeyReceiverWidget = null;
		}

		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.linkedTree != null)
			{
				this.linkedTree.RemoveChild();
				for (LinkedTree<Widget> nextAsList = this.linkedTree; nextAsList != null; nextAsList = nextAsList.NextAsList)
				{
					if (nextAsList.Value != null)
					{
						nextAsList.Value.DisposeSelf();
					}
				}
			}
		}

		protected virtual void DisposeSelf()
		{
			if (this.rootUIElement != null)
			{
				this.rootUIElement.Dispose();
			}
		}

		public virtual void SetPosition(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public virtual void SetSize(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		protected void AddChildFirst(Widget child)
		{
			this.linkedTree.AddChildFirst(child.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		protected void AddChildLast(Widget child)
		{
			this.linkedTree.AddChildLast(child.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		protected void InsertChildBefore(Widget child, Widget nextChild)
		{
			child.linkedTree.InsertChildBefore(nextChild.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		protected void InsertChildAfter(Widget child, Widget prevChild)
		{
			child.linkedTree.InsertChildAfter(prevChild.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		protected void RemoveChild(Widget child)
		{
			child.linkedTree.RemoveChild();
			child.NeedUpdateLocalToWorld = true;
		}

		protected internal virtual void OnTouchEvent(TouchEventCollection touchEvents)
		{
			this.SendTouchEventToGestureDetectors(touchEvents);
			if (this.TouchEventReceived != null)
			{
				this.TouchEventReceived.Invoke(this, new TouchEventArgs(touchEvents));
			}
		}

		private void SendTouchEventToGestureDetectors(TouchEventCollection touchEvents)
		{
			using (List<GestureDetector>.Enumerator enumerator = this.GestureDetectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GestureDetector current = enumerator.Current;
					bool flag = false;
					foreach (TouchEvent touchEvent in touchEvents)
					{
						flag |= (touchEvent.Type == TouchEventType.Down);
					}
					if (flag || current.State != GestureDetectorResponse.None)
					{
						current.State = current.OnTouchEvent(touchEvents);
						if (current.State == GestureDetectorResponse.None)
						{
							current.State = GestureDetectorResponse.FailedAndStop;
						}
					}
				}
			}
		}

		protected internal virtual void OnKeyEvent(KeyEvent keyEvent)
		{
			if (this.KeyEventReceived != null)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs(keyEvent);
				keyEventArgs.KeyType = keyEvent.KeyType;
				keyEventArgs.KeyEventType = keyEvent.KeyEventType;
				this.KeyEventReceived.Invoke(this, keyEventArgs);
				keyEvent.Forward = keyEventArgs.Forward;
			}
		}

		protected internal virtual void OnMotionEvent(MotionEvent motionEvent)
		{
			if (this.MotionEventReceived != null)
			{
				this.MotionEventReceived.Invoke(this, new MotionEventArgs(motionEvent));
			}
		}

		protected internal virtual void OnResetState()
		{
			using (List<GestureDetector>.Enumerator enumerator = this.GestureDetectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GestureDetector current = enumerator.Current;
					current.OnResetState();
					current.State = GestureDetectorResponse.None;
				}
			}
		}

		protected virtual void OnUpdate(float elapsedTime)
		{
		}

		internal void updateForEachDescendant(float elapsedTime)
		{
			LinkedTree<Widget> nextAsList = this.linkedTree.LastDescendant.NextAsList;
			for (LinkedTree<Widget> linkedTree = this.linkedTree; linkedTree != nextAsList; linkedTree = linkedTree.NextAsList)
			{
				Widget value = linkedTree.Value;
				if (value.Visible)
				{
					value.OnUpdate(elapsedTime);
				}
				else
				{
					linkedTree = linkedTree.LastDescendant;
				}
			}
		}

		public virtual bool HitTest(Vector2 screenPoint)
		{
			if (this.Width <= 0f || this.Height <= 0f)
			{
				return false;
			}
			this.updateLocalToWorld();
			float num = this.localToWorld.M41;
			float num2 = this.localToWorld.M42;
			float num3 = num + this.Width;
			float num4 = num2 + this.Height;
			num = ((this.finalClipX > num) ? this.finalClipX : num);
			num2 = ((this.finalClipY > num2) ? this.finalClipY : num2);
			num3 = ((this.finalClipX + this.finalClipWidth < num3) ? (this.finalClipX + this.finalClipWidth) : num3);
			num4 = ((this.finalClipY + this.finalClipHeight < num4) ? (this.finalClipY + this.finalClipHeight) : num4);
			return screenPoint.X >= num && screenPoint.Y >= num2 && screenPoint.X < num3 && screenPoint.Y < num4;
		}

		internal virtual void AddZSortUIElements(ref UIElement zSortList)
		{
			for (LinkedTree<UIElement> linkedTree = this.rootUIElement.linkedTree; linkedTree != null; linkedTree = linkedTree.NextAsList)
			{
				UIElement value = linkedTree.Value;
				if (value.Visible)
				{
					value.SetupFinalAlpha();
					if (value.ZSort)
					{
						value.SetupSortValue();
						value.nextZSortElement = zSortList;
						zSortList = value;
					}
				}
				else
				{
					linkedTree = linkedTree.LastDescendant;
				}
			}
		}

		protected internal virtual void Render()
		{
			for (LinkedTree<UIElement> linkedTree = this.rootUIElement.linkedTree; linkedTree != null; linkedTree = linkedTree.NextAsList)
			{
				UIElement value = linkedTree.Value;
				if (value.Visible && !value.ZSort)
				{
					value.SetupFinalAlpha();
					value.SetupDrawState();
					value.Render();
				}
				else
				{
					linkedTree = linkedTree.LastDescendant;
				}
			}
		}

		protected internal void ResetState(bool includeSelf)
		{
			LinkedTree<Widget> nextAsList = this.LinkedTree.LastDescendant.NextAsList;
			for (LinkedTree<Widget> nextAsList2 = this.LinkedTree; nextAsList2 != nextAsList; nextAsList2 = nextAsList2.NextAsList)
			{
				if (includeSelf || !nextAsList2.Value.Equals(this))
				{
					nextAsList2.Value.OnResetState();
				}
			}
		}

		internal void updateLocalToWorld()
		{
			if (this.NeedUpdateLocalToWorld)
			{
				if (this.Parent == null)
				{
					this.localToWorld = this.Transform3D;
				}
				else
				{
					this.Parent.updateLocalToWorld();
					this.Parent.localToWorld.Multiply(ref this.rootUIElement.transform3D, out this.localToWorld);
					float pivotAlignmentX = this.PivotAlignmentX;
					float pivotAlignmentY = this.PivotAlignmentY;
					this.localToWorld.M41 = this.localToWorld.M41 - (this.localToWorld.M11 * pivotAlignmentX + this.localToWorld.M21 * pivotAlignmentY);
					this.localToWorld.M42 = this.localToWorld.M42 - (this.localToWorld.M12 * pivotAlignmentX + this.localToWorld.M22 * pivotAlignmentY);
					this.localToWorld.M43 = this.localToWorld.M43 - (this.localToWorld.M13 * pivotAlignmentX + this.localToWorld.M23 * pivotAlignmentY);
				}
				this.NeedUpdateLocalToWorld = false;
			}
		}

		public Vector2 ConvertScreenToLocal(Vector2 screenPoint)
		{
			return this.ConvertScreenToLocal(screenPoint.X, screenPoint.Y);
		}

		public Vector2 ConvertScreenToLocal(float screenX, float screenY)
		{
			float num = 1000f;
			this.updateLocalToWorld();
			Matrix4 matrix = this.localToWorld.Inverse();
			Vector4 vector = matrix * new Vector4((float)UISystem.FramebufferWidth, (float)UISystem.FramebufferHeight, -num, 1f);
			Vector4 vector2 = matrix * new Vector4(screenX, screenY, 0f, 1f);
			vector /= vector.W;
			vector2 /= vector2.W;
			float num2 = 1f - vector2.Z / vector.Z;
			return vector.Xy + (vector2.Xy - vector.Xy) / num2;
		}

		public Vector2 ConvertLocalToScreen(Vector2 localPoint)
		{
			return this.ConvertLocalToScreen(localPoint.X, localPoint.Y);
		}

		public Vector2 ConvertLocalToScreen(float localX, float localY)
		{
			Vector4 vector = new Vector4(localX, localY, 0f, 1f);
			this.updateLocalToWorld();
			Vector4 vector2;
			this.localToWorld.Transform(ref vector, out vector2);
			Vector4 vector3;
			UISystem.viewProjectionMatrix.Transform(ref vector2, out vector3);
			UISystem.screenMatrix.Transform(ref vector3, out vector2);
			return new Vector2(vector2.X / vector2.W, vector2.Y / vector2.W);
		}

		public Vector2 ConvertLocalToOtherWidget(Vector2 localPoint, Widget targetWidget)
		{
			return this.ConvertLocalToOtherWidget(localPoint.X, localPoint.Y, targetWidget);
		}

		public Vector2 ConvertLocalToOtherWidget(float x, float y, Widget targetWidget)
		{
			Vector4 vector = targetWidget.LocalToWorld.Inverse() * (this.LocalToWorld * new Vector4(x, y, 0f, 1f));
			return (vector / vector.W).Xy;
		}

		internal bool SetupClipArea(bool isRootWidget)
		{
			if (isRootWidget || this.Parent == null)
			{
				this.finalClipX = 0f;
				this.finalClipY = 0f;
				this.finalClipWidth = this.Width;
				this.finalClipHeight = this.Height;
			}
			else
			{
				this.updateLocalToWorld();
				if (this.Clip)
				{
					ImageRect rect = new ImageRect((int)this.localToWorld.M41, (int)this.localToWorld.M42, (int)this.Width, (int)this.Height);
					ImageRect rect2 = new ImageRect((int)this.Parent.finalClipX, (int)this.Parent.finalClipY, (int)this.Parent.finalClipWidth, (int)this.Parent.finalClipHeight);
					ImageRect imageRect = this.Intersect(rect, rect2);
					this.finalClipX = (float)imageRect.X;
					this.finalClipY = (float)imageRect.Y;
					this.finalClipWidth = (float)imageRect.Width;
					this.finalClipHeight = (float)imageRect.Height;
				}
				else
				{
					this.finalClipX = this.Parent.finalClipX;
					this.finalClipY = this.Parent.finalClipY;
					this.finalClipWidth = this.Parent.finalClipWidth;
					this.finalClipHeight = this.Parent.finalClipHeight;
				}
				if (this.finalClipWidth <= 0f || this.finalClipHeight <= 0f)
				{
					return false;
				}
			}
			UISystem.SetClipRegion((int)this.finalClipX, (int)this.finalClipY, (int)this.finalClipWidth, (int)this.finalClipHeight);
			return true;
		}

		private ImageRect Intersect(ImageRect rect1, ImageRect rect2)
		{
			int num = Math.Max(rect1.X, rect2.X);
			int num2 = Math.Max(rect1.Y, rect2.Y);
			int num3 = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
			int num4 = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
			return new ImageRect
			{
				X = num,
				Y = num2,
				Width = ((num3 > num) ? (num3 - num) : 0),
				Height = ((num4 > num2) ? (num4 - num2) : 0)
			};
		}

		protected internal virtual void SetupFinalAlpha()
		{
			if (this.Parent != null)
			{
				this.RootUIElement.finalAlpha = this.Alpha * this.Parent.RootUIElement.finalAlpha;
				return;
			}
			this.RootUIElement.finalAlpha = this.Alpha;
		}

		public void RenderToTexture(Texture2D texture)
		{
			this.RenderToTexture(texture, Matrix4.Identity);
		}

		public void RenderToTexture(Texture2D texture, Matrix4 transform)
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}
			if (!texture.IsRenderable)
			{
				throw new ArgumentException("Texture is not renderable.", "texture");
			}
			FrameBuffer offScreenFramebufferCache = UISystem.offScreenFramebufferCache;
			offScreenFramebufferCache.SetColorTarget(texture, 0);
			this.RenderToFrameBuffer(offScreenFramebufferCache, ref transform, true);
		}

		public void RenderToFrameBuffer(FrameBuffer frameBuffer)
		{
			Matrix4 identity = Matrix4.Identity;
			this.RenderToFrameBuffer(frameBuffer, ref identity, true);
		}

		public void RenderToFrameBuffer(FrameBuffer frameBuffer, Matrix4 transform)
		{
			this.RenderToFrameBuffer(frameBuffer, ref transform, true);
		}

		internal void RenderToFrameBuffer(FrameBuffer frameBuffer, ref Matrix4 transform, bool useOrthoProjection)
		{
			if (frameBuffer == null)
			{
				throw new ArgumentNullException("frameBuffer");
			}
			if (!frameBuffer.Status)
			{
				throw new ArgumentException("Frame buffer is not renderable.", "frameBuffer");
			}
			UISystem.IsOffScreenRendering = true;
			GraphicsContext graphicsContext = UISystem.GraphicsContext;
			int num = frameBuffer.Width;
			int num2 = frameBuffer.Height;
			Matrix4 viewProjectionMatrix = UISystem.ViewProjectionMatrix;
			Matrix4 transform3D = this.Transform3D;
			PivotType pivotType = this.PivotType;
			this.Transform3D = transform;
			this.PivotType = PivotType.TopLeft;
			LinkedTree<Widget> parent = this.linkedTree.Parent;
			LinkedTree<Widget> linkedTree = null;
			if (parent != null)
			{
				linkedTree = this.LinkedTree.NextSibling;
				this.linkedTree.RemoveChild();
			}
			graphicsContext.SetFrameBuffer(frameBuffer);
			graphicsContext.SetColorMask((ColorMask)15);
			UISystem.SetClipRegionFull();
			graphicsContext.SetViewport(0, 0, num, num2);
			graphicsContext.SetClearColor(0f, 0f, 0f, 1f);
			graphicsContext.Clear();
			if (useOrthoProjection)
			{
				float num3 = 100000f;
				float num4 = -100000f;
				UISystem.ViewProjectionMatrix = new Matrix4(2f / (float)num * UISystem.Scale, 0f, 0f, 0f, 0f, -2f / (float)num2 * UISystem.Scale, 0f, 0f, 0f, 0f, -2f / (num3 - num4), 0f, -1f, 1f, (num3 + num4) / (num3 - num4), 1f);
			}
			UISystem.Render(this);
			graphicsContext.Enable((EnableMode)4u, true);
			graphicsContext.SetBlendFuncRgb((BlendFuncMode)0, (BlendFuncFactor)0, (BlendFuncFactor)1);
			graphicsContext.SetBlendFuncAlpha((BlendFuncMode)0, (BlendFuncFactor)9, (BlendFuncFactor)0);
			UISystem.SetClipRegionFull();
			UISprite uISprite = new UISprite(1);
			UISpriteUnit unit = uISprite.GetUnit(0);
			unit.SetPosition(-1f, -1f);
			unit.Width = (float)frameBuffer.Width + 2f;
			unit.Height = (float)frameBuffer.Height + 2f;
			unit.Color = new UIColor(0f, 0f, 0f, 1f);
			uISprite.Render();
			graphicsContext.SetFrameBuffer(null);
			UISystem.SetClipRegionFull();
			graphicsContext.SetColorMask((ColorMask)7);
			UISystem.ViewProjectionMatrix = viewProjectionMatrix;
			if (linkedTree != null)
			{
				this.linkedTree.InsertChildBefore(linkedTree);
			}
			else if (parent != null)
			{
				parent.AddChildLast(this.linkedTree);
			}
			this.pivotType = pivotType;
			this.Transform3D = transform3D;
			UISystem.IsOffScreenRendering = false;
		}

		public bool AddGestureDetector(GestureDetector gestureDetector)
		{
			if (gestureDetector == null)
			{
				throw new ArgumentNullException("gestureDetector");
			}
			if (gestureDetector.TargetWidget == null)
			{
				this.GestureDetectors.Add(gestureDetector);
				gestureDetector.TargetWidget = this;
				return true;
			}
			return false;
		}

		public bool RemoveGestureDetector(GestureDetector gestureDetector)
		{
			if (gestureDetector == null)
			{
				throw new ArgumentNullException("gestureDetector");
			}
			if (gestureDetector.TargetWidget == this && this.GestureDetectors.Remove(gestureDetector))
			{
				gestureDetector.TargetWidget = null;
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name + " : " + base.GetType().Name;
		}
	}
}
