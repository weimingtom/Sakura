using System;

namespace Sce.Pss.HighLevel.UI
{
	public class RootUIElement : UIElement
	{
		internal Widget parentWidget;

		public RootUIElement(Widget parentWidget)
		{
			this.parentWidget = parentWidget;
		}

		protected internal override void SetupFinalAlpha()
		{
		}

		internal override void updateLocalToWorld()
		{
			if (base.NeedUpdateLocalToWorld)
			{
				this.localToWorld = this.parentWidget.LocalToWorld;
				base.NeedUpdateLocalToWorld = false;
			}
		}
	}
}
