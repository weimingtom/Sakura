using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public struct RawSpriteTile
	{
		public TRS Quad;

		public Vector2i TileIndex2D;

		public bool FlipU;

		public bool FlipV;

		public RawSpriteTile(TRS positioning, Vector2i tile_index, bool flipu = false, bool flipv = false)
		{
			this.Quad = positioning;
			this.TileIndex2D = tile_index;
			this.FlipU = flipu;
			this.FlipV = flipv;
		}
	}
}
