using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class SpriteTile : SpriteBase
	{
		public Vector2i TileIndex2D = new Vector2i(0, 0);

		public int TileIndex1D
		{
			get
			{
				Common.Assert(this.TextureInfo != null);
				return this.TileIndex2D.X + this.TileIndex2D.Y * this.TextureInfo.NumTiles.X;
			}
			set
			{
				Common.Assert(this.TextureInfo != null);
				this.TileIndex2D = new Vector2i(value % this.TextureInfo.NumTiles.X, value / this.TextureInfo.NumTiles.X);
			}
		}

		public SpriteTile()
		{
		}

		public SpriteTile(TextureInfo texture_info) : base(texture_info)
		{
		}

		public SpriteTile(TextureInfo texture_info, Vector2i index) : base(texture_info)
		{
			this.TileIndex2D = index;
		}

		public SpriteTile(TextureInfo texture_info, int index) : base(texture_info)
		{
			this.TileIndex1D = index;
		}

		public override Vector2 CalcSizeInPixels()
		{
			return this.TextureInfo.TileSizeInPixelsf;
		}

		internal override void internal_draw()
		{
			Director.Instance.SpriteRenderer.FlipU = this.FlipU;
			Director.Instance.SpriteRenderer.FlipV = this.FlipV;
			Director.Instance.SpriteRenderer.AddSprite(ref this.Quad, this.TileIndex2D);
		}

		internal override void internal_draw_cpu_transform()
		{
			Director.Instance.SpriteRenderer.FlipU = this.FlipU;
			Director.Instance.SpriteRenderer.FlipV = this.FlipV;
			Matrix3 transform = base.GetTransform();
			Director.Instance.SpriteRenderer.AddSprite(ref this.Quad, this.TileIndex2D, ref transform);
		}
	}
}
