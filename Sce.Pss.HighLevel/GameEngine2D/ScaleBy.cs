using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class ScaleBy : ActionTweenGenericVector2Scale
	{
		public ScaleBy(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = true;
			this.Get = (() => base.Target.Scale);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Scale = value;
			};
		}
	}
}
