using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public static class DebugFlags
	{
		public static uint DrawTransform = 1u;

		public static uint DrawPivot = 2u;

		public static uint DrawContentLocalBounds = 4u;

		public static uint DrawContentWorldBounds = 8u;

		public static uint DrawGrid = 16u;

		public static uint Navigate = 32u;

		internal static uint Log = 64u;
	}
}
