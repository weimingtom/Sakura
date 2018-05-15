using System;

namespace Sce.Pss.HighLevel.UI
{
	public class Scene
	{
		private string title;

		private RootWidget rootWidget;

		private float designWidth;

		private float designHeight;

		private bool needDesignSizeReflect;

		public event EventHandler<UpdateEventArgs> Updated;

		public event EventHandler Showing;

		public event EventHandler Shown;

		public event EventHandler Hiding;

		public event EventHandler Hidden;

		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				if (value != null)
				{
					this.title = value;
					return;
				}
				this.title = "";
			}
		}

		public RootWidget RootWidget
		{
			get
			{
				return this.rootWidget;
			}
		}

		public Transition Transition
		{
			get;
			set;
		}

		public bool ShowNavigationBar
		{
			get;
			set;
		}

		public bool Visible
		{
			get
			{
				return this.RootWidget != null && this.RootWidget.Visible;
			}
			set
			{
				if (this.RootWidget != null)
				{
					this.RootWidget.Visible = value;
				}
			}
		}

		public float DesignWidth
		{
			get
			{
				return this.designWidth;
			}
			set
			{
				this.designWidth = value;
				this.needDesignSizeReflect = true;
			}
		}

		public float DesignHeight
		{
			get
			{
				return this.designHeight;
			}
			set
			{
				this.designHeight = value;
				this.needDesignSizeReflect = true;
			}
		}

		internal bool ConsumeAllTouchEvent
		{
			get;
			set;
		}

		public Scene()
		{
			this.rootWidget = new RootWidget(this);
			this.Transition = null;
			this.ShowNavigationBar = false;
			this.ConsumeAllTouchEvent = false;
		}

		protected virtual void OnUpdate(float elapsedTime)
		{
			if (this.Updated != null)
			{
				this.Updated.Invoke(this, new UpdateEventArgs(elapsedTime));
			}
		}

		internal void Update(float elapsedTime)
		{
			this.OnUpdate(elapsedTime);
			this.RootWidget.updateForEachDescendant(elapsedTime);
		}

		protected internal virtual void OnShowing()
		{
			if (this.needDesignSizeReflect)
			{
				this.RootWidget.updateWidth(this.designWidth, (float)UISystem.FramebufferWidth);
				this.RootWidget.updateHeight(this.designHeight, (float)UISystem.FramebufferHeight);
				this.needDesignSizeReflect = false;
			}
			if (this.Showing != null)
			{
				this.Showing.Invoke(this, EventArgs.Empty);
			}
		}

		protected internal virtual void OnShown()
		{
			if (this.Shown != null)
			{
				this.Shown.Invoke(this, EventArgs.Empty);
			}
		}

		protected internal virtual void OnHiding()
		{
			if (this.Hiding != null)
			{
				this.Hiding.Invoke(this, EventArgs.Empty);
			}
		}

		protected internal virtual void OnHidden()
		{
			if (this.Hidden != null)
			{
				this.Hidden.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
