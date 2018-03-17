using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class SpriteUV : SpriteBase
	{
		public TRS UV = TRS.Quad0_1;

		public SpriteUV()
		{
		}

		public SpriteUV(TextureInfo texture_info) : base(texture_info)
		{
		}

		public override Vector2 CalcSizeInPixels()
		{
			Common.Assert(this.TextureInfo != null);
			Common.Assert(this.TextureInfo.Texture != null);
			return new Vector2(this.UV.S.X * (float)this.TextureInfo.Texture.Width, this.UV.S.Y * (float)this.TextureInfo.Texture.Height);
		}

		internal override void internal_draw()
		{
			Director.Instance.SpriteRenderer.FlipU = this.FlipU;
			Director.Instance.SpriteRenderer.FlipV = this.FlipV;
			Director.Instance.SpriteRenderer.AddSprite(ref this.Quad, ref this.UV);
		}

		internal override void internal_draw_cpu_transform()
		{
			Director.Instance.SpriteRenderer.FlipU = this.FlipU;
			Director.Instance.SpriteRenderer.FlipV = this.FlipV;
			Matrix3 transform = base.GetTransform();
			Director.Instance.SpriteRenderer.AddSprite(ref this.Quad, ref this.UV, ref transform);
		}
	}
}
