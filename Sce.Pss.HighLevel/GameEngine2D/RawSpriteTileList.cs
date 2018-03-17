using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class RawSpriteTileList : Node
	{
		public List<RawSpriteTile> Sprites = new List<RawSpriteTile>();

		public Vector4 Color = Colors.White;

		public BlendMode BlendMode = BlendMode.Normal;

		public TextureInfo TextureInfo;

		public SpriteRenderer.ISpriteShader Shader = Director.Instance.SpriteRenderer.DefaultShader;

		public RawSpriteTileList(TextureInfo texture_info)
		{
			this.TextureInfo = texture_info;
		}

		public override void Draw()
		{
			Director.Instance.GL.SetBlendMode(this.BlendMode);
			this.Shader.SetColor(ref this.Color);
			this.Shader.SetUVTransform(ref Math.UV_TransformFlipV);
			Director.Instance.SpriteRenderer.BeginSprites(this.TextureInfo, this.Shader, this.Sprites.Count);
			foreach (RawSpriteTile current in this.Sprites)
			{
				Director.Instance.SpriteRenderer.FlipU = current.FlipU;
				Director.Instance.SpriteRenderer.FlipV = current.FlipV;
				TRS quad = current.Quad;
				Director.Instance.SpriteRenderer.AddSprite(ref quad, current.TileIndex2D);
			}
			Director.Instance.SpriteRenderer.EndSprites();
		}

		public Vector2 CalcSizeInPixels()
		{
			return this.TextureInfo.TileSizeInPixelsf;
		}
	}
}
