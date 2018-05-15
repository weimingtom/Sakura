using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class ScrollPanel : ContainerWidget
	{
		private enum AnimationState
		{
			None,
			Drag,
			Flick
		}

		private const float toleranceSizeAsScrollable = 0.999f;

		private ScrollBarVisibility scrollBarVisibility = ScrollBarVisibility.ScrollableVisible;

		private bool horizontalScroll = true;

		private bool verticalScroll = true;

		private Panel panel;

		private ScrollBar scrollBarH;

		private ScrollBar scrollBarV;

		private ScrollPanel.AnimationState animationState;

		private bool animation;

		private Vector2 flickDistance;

		private Vector2 startFlickDistance;

		private Vector2 startPanelPos;

		private float animationElapsedTime;

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
				return this.panel.X;
			}
			set
			{
				if (this.Width < this.panel.Width)
				{
					this.panel.X = MathUtility.Clamp<float>(value, this.Width - this.panel.Width, 0f);
					this.scrollBarH.BarPosition = -this.panel.X;
				}
			}
		}

		public float PanelY
		{
			get
			{
				return this.panel.Y;
			}
			set
			{
				if (this.Height < this.panel.Height)
				{
					this.panel.Y = MathUtility.Clamp<float>(value, this.Height - this.panel.Height, 0f);
					this.scrollBarV.BarPosition = -this.panel.Y;
				}
			}
		}

		public float PanelWidth
		{
			get
			{
				return this.panel.Width;
			}
			set
			{
				this.panel.Width = value;
				this.UpdateView();
			}
		}

		public float PanelHeight
		{
			get
			{
				return this.panel.Height;
			}
			set
			{
				this.panel.Height = value;
				this.UpdateView();
			}
		}

		public UIColor PanelColor
		{
			get
			{
				return this.panel.BackgroundColor;
			}
			set
			{
				this.panel.BackgroundColor = value;
			}
		}

		public ScrollBarVisibility ScrollBarVisibility
		{
			get
			{
				return this.scrollBarVisibility;
			}
			set
			{
				this.scrollBarVisibility = value;
				this.UpdateScrollBarVisible();
			}
		}

		public bool HorizontalScroll
		{
			get
			{
				return this.horizontalScroll;
			}
			set
			{
				this.horizontalScroll = value;
				this.UpdateScrollBarVisible();
			}
		}

		public bool VerticalScroll
		{
			get
			{
				return this.verticalScroll;
			}
			set
			{
				this.verticalScroll = value;
				this.UpdateScrollBarVisible();
			}
		}

		public override IEnumerable<Widget> Children
		{
			get
			{
				return this.panel.Children;
			}
		}

		public ScrollPanel()
		{
			this.panel = new Panel();
			this.panel.Clip = true;
			base.AddChildLast(this.panel);
			this.scrollBarH = new ScrollBar(ScrollBarOrientation.Horizontal);
			this.scrollBarV = new ScrollBar(ScrollBarOrientation.Vertical);
			base.AddChildLast(this.scrollBarH);
			base.AddChildLast(this.scrollBarV);
			this.animationState = ScrollPanel.AnimationState.None;
			base.Clip = true;
			base.HookChildTouchEvent = true;
			DragGestureDetector dragGestureDetector = new DragGestureDetector();
			dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(dragGestureDetector);
			FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
			flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(flickGestureDetector);
			this.UpdateView();
		}

		public override void AddChildFirst(Widget child)
		{
			this.panel.AddChildFirst(child);
		}

		public override void AddChildLast(Widget child)
		{
			this.panel.AddChildLast(child);
		}

		public override void InsertChildBefore(Widget child, Widget nextChild)
		{
			this.panel.InsertChildBefore(child, nextChild);
		}

		public override void InsertChildAfter(Widget child, Widget prevChild)
		{
			this.panel.InsertChildAfter(child, prevChild);
		}

		public override void RemoveChild(Widget child)
		{
			this.panel.RemoveChild(child);
		}

		private void UpdateView()
		{
			if (this.scrollBarH == null || this.scrollBarV == null)
			{
				return;
			}
			if (this.Width > this.PanelWidth)
			{
				this.PanelX = 0f;
				this.scrollBarH.Length = this.Width;
			}
			else
			{
				this.panel.X = FMath.Clamp(this.panel.X, this.Width - this.panel.Width, 0f);
				this.scrollBarH.Length = this.panel.Width;
			}
			this.scrollBarH.Y = this.Height - this.scrollBarH.Height;
			this.scrollBarH.Width = this.Width - this.scrollBarV.Width;
			this.scrollBarH.BarLength = this.Width;
			if (this.Height > this.PanelHeight)
			{
				this.PanelY = 0f;
				this.scrollBarV.Length = this.Height;
			}
			else
			{
				this.panel.Y = FMath.Clamp(this.panel.Y, this.Height - this.panel.Height, 0f);
				this.scrollBarV.Length = this.panel.Height;
			}
			this.scrollBarV.X = this.Width - this.scrollBarV.Width;
			this.scrollBarV.Height = this.Height - this.scrollBarH.Height;
			this.scrollBarV.BarLength = this.Height;
			this.UpdateScrollBarPos();
			this.UpdateScrollBarVisible();
		}

		private void UpdateScrollBarPos()
		{
			this.scrollBarH.BarPosition = -this.panel.X;
			this.scrollBarV.BarPosition = -this.panel.Y;
		}

		private void UpdateScrollBarVisible()
		{
			switch (this.scrollBarVisibility)
			{
			case ScrollBarVisibility.Visible:
				this.scrollBarH.Visible = this.HorizontalScroll;
				this.scrollBarV.Visible = this.VerticalScroll;
				return;
			case ScrollBarVisibility.ScrollableVisible:
				this.scrollBarH.Visible = this.isHorizontalScrollable();
				this.scrollBarV.Visible = this.isVerticalScrollable();
				return;
			case ScrollBarVisibility.ScrollingVisible:
			case ScrollBarVisibility.Invisible:
				this.scrollBarH.Visible = false;
				this.scrollBarV.Visible = false;
				return;
			default:
				return;
			}
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (!this.animation)
			{
				return;
			}
			if (Math.Abs(this.flickDistance.X) < 0.5f && Math.Abs(this.flickDistance.Y) < 0.5f)
			{
				this.flickDistance = Vector2.Zero;
				this.animationState = ScrollPanel.AnimationState.None;
				this.animation = false;
				this.UpdateView();
				return;
			}
			this.animationElapsedTime += elapsedTime;
			float num = (float)Math.Exp((double)(-(double)this.animationElapsedTime * 0.00539892027f));
			this.flickDistance = this.startFlickDistance * num;
			if (Math.Abs(this.flickDistance.X) > 0.5f)
			{
				this.panel.X = this.startPanelPos.X + (this.startFlickDistance.X - this.flickDistance.X) / 0.09f;
				this.panel.X = FMath.Clamp(this.panel.X, this.Width - this.panel.Width, 0f);
			}
			if (Math.Abs(this.flickDistance.Y) > 0.5f)
			{
				this.panel.Y = this.startPanelPos.Y + (this.startFlickDistance.Y - this.flickDistance.Y) / 0.09f;
				this.panel.Y = FMath.Clamp(this.panel.Y, this.Height - this.panel.Height, 0f);
			}
			this.UpdateScrollBarPos();
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (touchEvents.PrimaryTouchEvent.Type == TouchEventType.Up && !this.animation)
			{
				if (this.ScrollBarVisibility == ScrollBarVisibility.ScrollingVisible)
				{
					this.scrollBarH.Visible = false;
					this.scrollBarV.Visible = false;
				}
				this.animationState = ScrollPanel.AnimationState.None;
			}
			if (this.animationState != ScrollPanel.AnimationState.Drag && this.animationState != ScrollPanel.AnimationState.Flick)
			{
				touchEvents.Forward = true;
			}
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			base.ResetState(false);
			if (this.animation)
			{
				this.flickDistance = Vector2.Zero;
				this.animation = false;
			}
			this.animationState = ScrollPanel.AnimationState.Drag;
			if (this.ScrollBarVisibility == ScrollBarVisibility.ScrollingVisible)
			{
				this.scrollBarH.Visible = this.isHorizontalScrollable();
				this.scrollBarV.Visible = this.isVerticalScrollable();
			}
			if (this.isHorizontalScrollable())
			{
				this.panel.X += e.Distance.X;
				this.panel.X = MathUtility.Clamp<float>(this.panel.X, this.Width - this.panel.Width, 0f);
			}
			if (this.isVerticalScrollable())
			{
				this.panel.Y += e.Distance.Y;
				this.panel.Y = MathUtility.Clamp<float>(this.panel.Y, this.Height - this.panel.Height, 0f);
			}
			this.UpdateScrollBarPos();
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			base.ResetState(false);
			this.animationState = ScrollPanel.AnimationState.Flick;
			if (this.ScrollBarVisibility == ScrollBarVisibility.ScrollingVisible)
			{
				this.scrollBarH.Visible = this.isHorizontalScrollable();
				this.scrollBarV.Visible = this.isVerticalScrollable();
			}
			if (this.isHorizontalScrollable())
			{
				this.flickDistance.X = e.Speed.X * 0.03f;
			}
			if (this.isVerticalScrollable())
			{
				this.flickDistance.Y = e.Speed.Y * 0.03f;
			}
			this.startFlickDistance = this.flickDistance;
			this.startPanelPos.X = this.panel.X;
			this.startPanelPos.Y = this.panel.Y;
			this.animationElapsedTime = 0f;
			this.animation = true;
		}

		private bool isHorizontalScrollable()
		{
			return this.HorizontalScroll && this.Width < this.PanelWidth - 0.999f;
		}

		private bool isVerticalScrollable()
		{
			return this.VerticalScroll && this.Height < this.PanelHeight - 0.999f;
		}
	}
}
