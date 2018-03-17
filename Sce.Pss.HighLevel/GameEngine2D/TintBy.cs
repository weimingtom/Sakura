using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class TintBy : ActionTweenGenericVector4
	{
		public TintBy(Vector4 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = true;
			this.Get = (() => ((SpriteBase)base.Target).Color);
			this.Set = delegate(Vector4 value)
			{
				((SpriteBase)base.Target).Color = value;
			};
		}
	}
}
