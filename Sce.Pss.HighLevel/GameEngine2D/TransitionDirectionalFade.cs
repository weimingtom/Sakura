using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class TransitionDirectionalFade : TransitionFadeBase
	{
		public class SpriteShaderDirFade : SpriteRenderer.ISpriteShader, IDisposable
		{
			public ShaderProgram m_shader_program;

			public SpriteShaderDirFade()
			{
				this.m_shader_program = Common.CreateShaderProgram("cg/sprite_directional_fade.cgx");
				this.m_shader_program.SetUniformBinding(0, "MVP");
				this.m_shader_program.SetUniformBinding(1, "UVTransform");
				this.m_shader_program.SetUniformBinding(2, "Plane");
				this.m_shader_program.SetUniformBinding(3, "OffsetRcp");
				this.m_shader_program.SetAttributeBinding(0, "vin_data");
				Matrix4 identity = Matrix4.Identity;
				this.SetMVP(ref identity);
				this.SetUVTransform(ref Math._0011);
			}

			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					Common.DisposeAndNullify<ShaderProgram>(ref this.m_shader_program);
				}
			}

			public ShaderProgram GetShaderProgram()
			{
				return this.m_shader_program;
			}

			public void SetMVP(ref Matrix4 value)
			{
				this.m_shader_program.SetUniformValue(0, ref value);
			}

			public void SetUVTransform(ref Vector4 value)
			{
				this.m_shader_program.SetUniformValue(1, ref value);
			}

			public void SetPlane(ref Vector4 value)
			{
				this.m_shader_program.SetUniformValue(2, ref value);
			}

			public void SetOffsetRcp(float value)
			{
				this.m_shader_program.SetUniformValue(3, value);
			}

			public void SetColor(ref Vector4 value)
			{
			}
		}

		public float Width = 0.75f;

		public Vector2 Direction = Math._10;

		public DTween Tween = (float x) => Math.PowEaseOut(x, 4f);

		private static TransitionDirectionalFade.SpriteShaderDirFade m_shader;

		public TransitionDirectionalFade(Scene next_scene) : base(next_scene)
		{
			if (TransitionDirectionalFade.m_shader == null)
			{
				TransitionDirectionalFade.m_shader = new TransitionDirectionalFade.SpriteShaderDirFade();
			}
		}

		public static new void Terminate()
		{
			Common.DisposeAndNullify<TransitionDirectionalFade.SpriteShaderDirFade>(ref TransitionDirectionalFade.m_shader);
		}

		public override void Draw()
		{
			base.Draw();
			float x = this.Tween(base.FadeCompletion);
			TRS tRS = new TRS(Director.Instance.CurrentScene.Camera.CalcBounds());
			TRS quad0_ = TRS.Quad0_1;
			TransitionDirectionalFade.m_shader.SetUVTransform(ref Math.UV_TransformIdentity);
			Director.Instance.GL.Context.SetTexture(1, TransitionFadeBase.m_next_scene_render.Texture);
			Director.Instance.GL.SetBlendMode(BlendMode.None);
			Vector2 vector = -this.Direction.Normalize();
			Vector2 a = new Vector2(0f, 0f);
			Vector2 vector2 = new Vector2(1f, 1f);
			if (vector.X > 0f)
			{
				a.X = 1f;
				vector2.X = 0f;
			}
			if (vector.Y > 0f)
			{
				a.Y = 1f;
				vector2.Y = 0f;
			}
			vector2 -= vector * this.Width;
			Vector4 vector3 = new Vector4(Math.Lerp(a, vector2, x), vector);
			TransitionDirectionalFade.m_shader.SetPlane(ref vector3);
			TransitionDirectionalFade.m_shader.SetOffsetRcp(1f / this.Width);
			Director.Instance.SpriteRenderer.BeginSprites(TransitionFadeBase.m_previous_scene_render, TransitionDirectionalFade.m_shader, 1);
			Director.Instance.SpriteRenderer.AddSprite(ref tRS, ref quad0_);
			Director.Instance.SpriteRenderer.EndSprites();
		}
	}
}
