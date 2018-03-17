using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public abstract class SpriteBase : Node
	{
		public TRS Quad = TRS.Quad0_1;

		public bool FlipU = false;

		public bool FlipV = false;

		public Vector4 Color = Colors.White;

		public BlendMode BlendMode = BlendMode.Normal;

		public TextureInfo TextureInfo;

		public SpriteRenderer.ISpriteShader Shader = Director.Instance.SpriteRenderer.DefaultShader;

		public abstract Vector2 CalcSizeInPixels();

		public SpriteBase()
		{
		}

		public SpriteBase(TextureInfo texture_info)
		{
			this.TextureInfo = texture_info;
		}

		public override void Draw()
		{
			Common.Assert(this.TextureInfo != null, "Sprite's TextureInfo is null");
			Common.Assert(this.Shader != null, "Sprite's Shader is null");
			Director.Instance.GL.SetBlendMode(this.BlendMode);
			this.Shader.SetColor(ref this.Color);
			this.Shader.SetUVTransform(ref Math.UV_TransformFlipV);
			Director.Instance.SpriteRenderer.BeginSprites(this.TextureInfo, this.Shader, 1);
			this.internal_draw();
			Director.Instance.SpriteRenderer.EndSprites();
		}

		public override bool GetlContentLocalBounds(ref Bounds2 bounds)
		{
			bounds = this.GetlContentLocalBounds();
			return true;
		}

		public Bounds2 GetlContentLocalBounds()
		{
			return this.Quad.Bounds2();
		}

		public void MakeFullScreen()
		{
			this.Quad = new TRS(Director.Instance.CurrentScene.Camera.CalcBounds());
		}

		public void CenterSprite()
		{
			this.Quad.Centering(new Vector2(0.5f, 0.5f));
		}

		public void CenterSprite(Vector2 new_center)
		{
			this.Quad.Centering(new_center);
		}

		internal abstract void internal_draw();

		internal abstract void internal_draw_cpu_transform();
	}
}
