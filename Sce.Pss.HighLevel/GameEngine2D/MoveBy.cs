using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class MoveBy : ActionTweenGenericVector2
	{
		public MoveBy(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = true;
			this.Get = (() => base.Target.Position);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Position = value;
			};
		}
	}
}
