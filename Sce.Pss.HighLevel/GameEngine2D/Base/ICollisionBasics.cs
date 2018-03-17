using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public interface ICollisionBasics
	{
		bool IsInside(Vector2 point);

		void ClosestSurfacePoint(Vector2 point, out Vector2 ret, out float sign);

		float SignedDistance(Vector2 point);

		bool NegativeClipSegment(ref Vector2 A, ref Vector2 B);
	}
}
