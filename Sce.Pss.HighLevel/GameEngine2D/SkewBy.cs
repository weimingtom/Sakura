using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class SkewBy : ActionTweenGenericVector2
	{
		public SkewBy(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = true;
			this.Get = (() => base.Target.Skew);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Skew = value;
			};
		}
	}
}
