using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class ScaleTo : ActionTweenGenericVector2Scale
	{
		public ScaleTo(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = false;
			this.Get = (() => base.Target.Scale);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Scale = value;
			};
		}
	}
}
