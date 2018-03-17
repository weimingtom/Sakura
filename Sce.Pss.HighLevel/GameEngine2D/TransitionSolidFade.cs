using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class TransitionSolidFade : TransitionFadeBase
	{
		public DTween Tween = (float x) => Math.PowEaseInOut(x, 4f);

		public TransitionSolidFade(Scene next_scene) : base(next_scene)
		{
		}

		public override void Draw()
		{
			base.Draw();
			TRS tRS = new TRS(Director.Instance.CurrentScene.Camera.CalcBounds());
			TRS quad0_ = TRS.Quad0_1;
			Director.Instance.SpriteRenderer.DefaultShader.SetUVTransform(ref Math.UV_TransformIdentity);
			Director.Instance.GL.SetBlendMode(BlendMode.None);
			if (base.FadeCompletion < 0.5f)
			{
				float num = this.Tween(base.FadeCompletion / 0.5f);
				Vector4 vector = new Vector4(1f - num);
				Director.Instance.SpriteRenderer.DefaultShader.SetColor(ref vector);
				Director.Instance.SpriteRenderer.BeginSprites(TransitionFadeBase.m_previous_scene_render, 1);
				Director.Instance.SpriteRenderer.AddSprite(ref tRS, ref quad0_);
				Director.Instance.SpriteRenderer.EndSprites();
			}
			else
			{
				float num = this.Tween((base.FadeCompletion - 0.5f) / 0.5f);
				Vector4 vector = new Vector4(num);
				Director.Instance.SpriteRenderer.DefaultShader.SetColor(ref vector);
				Director.Instance.SpriteRenderer.BeginSprites(TransitionFadeBase.m_next_scene_render, 1);
				Director.Instance.SpriteRenderer.AddSprite(ref tRS, ref quad0_);
				Director.Instance.SpriteRenderer.EndSprites();
			}
		}
	}
}
