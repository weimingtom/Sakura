using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class MoveTo : ActionTweenGenericVector2
	{
		public MoveTo(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = false;
			this.Get = (() => base.Target.Position);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Position = value;
			};
		}
	}
}
