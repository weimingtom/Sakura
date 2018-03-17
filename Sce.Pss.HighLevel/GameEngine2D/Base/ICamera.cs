using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public interface ICamera
	{
		void SetAspectFromViewport();

		void Push();

		void Pop();

		Matrix4 GetTransform();

		void DebugDraw(float step);

		void Navigate(int control);

		void SetViewFromViewport();

		Vector2 NormalizedToWorld(Vector2 bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos);

		Vector2 GetTouchPos(int nth = 0, bool prev = false);

		Bounds2 CalcBounds();

		float GetPixelSize();

		void SetTouchPlaneMatrix(Matrix4 mat);
	}
}
