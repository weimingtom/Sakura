using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class RotateBy : ActionTweenGenericVector2Rotation
	{
		public RotateBy(Vector2 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = true;
			this.Get = (() => base.Target.Rotation);
			this.Set = delegate(Vector2 value)
			{
				base.Target.Rotation = value;
			};
		}
	}
}
