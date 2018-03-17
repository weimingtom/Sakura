using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class ActionTweenGenericVector2 : ActionTweenGeneric<Vector2>
	{
		public override void lerp(float alpha)
		{
			if (!this.IsRelative)
			{
				this.Set(Math.Lerp(this.m_start_value, this.TargetValue, alpha));
			}
			else
			{
				this.Set(this.m_start_value + this.TargetValue * alpha);
			}
		}
	}
}
