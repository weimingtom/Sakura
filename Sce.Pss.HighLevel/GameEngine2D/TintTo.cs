using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class TintTo : ActionTweenGenericVector4
	{
		public TintTo(Vector4 target, float duration)
		{
			this.TargetValue = target;
			this.Duration = duration;
			this.IsRelative = false;
			this.Get = (() => ((SpriteBase)base.Target).Color);
			this.Set = delegate(Vector4 value)
			{
				((SpriteBase)base.Target).Color = value;
			};
		}
	}
}
