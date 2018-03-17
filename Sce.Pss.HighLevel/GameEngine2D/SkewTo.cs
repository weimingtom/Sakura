using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class SkewTo : ActionTweenGenericVector2
	{
		public SkewTo(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = false;
			this.Get = (() => base.Target.Skew);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Skew = value;
			};
		}
	}
}
