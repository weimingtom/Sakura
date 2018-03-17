using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class RotateTo : ActionTweenGenericVector2Rotation
	{
		public RotateTo(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = false;
			this.Get = (() => base.Target.Rotation);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Rotation = value;
			};
		}
	}
}
