using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class RootWidget : ContainerWidget
	{
		internal Scene parentScene;

		public override float X
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override float Y
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override float Width
		{
			get
			{
				return (float)UISystem.FramebufferWidth;
			}
			set
			{
			}
		}

		public override float Height
		{
			get
			{
				return (float)UISystem.FramebufferHeight;
			}
			set
			{
			}
		}

		public override Matrix4 Transform3D
		{
			get
			{
				return base.Transform3D;
			}
			set
			{
			}
		}

		internal RootWidget(Scene scene)
		{
			this.parentScene = scene;
		}

		public override bool HitTest(Vector2 screenPoint)
		{
			return false;
		}
	}
}
