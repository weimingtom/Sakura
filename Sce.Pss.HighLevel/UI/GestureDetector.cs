using System;

namespace Sce.Pss.HighLevel.UI
{
	public abstract class GestureDetector
	{
		private Widget targetWidget;

		public GestureDetectorResponse State
		{
			get;
			internal set;
		}

		internal Widget TargetWidget
		{
			get
			{
				return this.targetWidget;
			}
			set
			{
				this.targetWidget = value;
			}
		}

		public GestureDetector()
		{
			this.State = GestureDetectorResponse.None;
			this.TargetWidget = null;
		}

		protected internal abstract GestureDetectorResponse OnTouchEvent(TouchEventCollection touchEvents);

		protected internal abstract void OnResetState();
	}
}
