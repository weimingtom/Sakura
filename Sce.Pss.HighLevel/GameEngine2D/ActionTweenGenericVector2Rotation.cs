using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class ActionTweenGenericVector2Rotation : ActionTweenGeneric<Vector2>
	{
		public override void lerp(float alpha)
		{
			if (!this.IsRelative)
			{
				this.Set(Math.LerpUnitVectors(this.m_start_value, this.TargetValue, alpha));
			}
			else
			{
				this.Set(this.m_start_value.Rotate(Math.Angle(this.TargetValue) * alpha));
			}
		}
	}
}
