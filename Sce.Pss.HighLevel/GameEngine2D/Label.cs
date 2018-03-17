using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class Label : Node
	{
		public string Text = "";

		public Vector4 Color = Colors.White;

		public BlendMode BlendMode = BlendMode.Normal;

		public float HeightScale = 1f;

		public FontMap FontMap;

		public SpriteRenderer.ISpriteShader Shader = Director.Instance.SpriteRenderer.DefaultFontShader;

		private float FontHeight
		{
			get
			{
				float result;
				if (this.FontMap == null)
				{
					result = (float)EmbeddedDebugFontData.CharSizei.Y;
				}
				else
				{
					result = this.FontMap.CharPixelHeight;
				}
				return result;
			}
		}

		public float CharWorldHeight
		{
			get
			{
				return this.FontHeight * this.HeightScale;
			}
		}

		public Label()
		{
		}

		public Label(string text, FontMap fontmap = null)
		{
			this.Text = text;
			this.FontMap = fontmap;
		}

		public override void Draw()
		{
			base.Draw();
			Director.Instance.GL.SetBlendMode(this.BlendMode);
			this.Shader.SetColor(ref this.Color);
			if (this.FontMap == null)
			{
				Director.Instance.SpriteRenderer.DrawTextDebug(this.Text, Math._00, this.CharWorldHeight, true, null);
			}
			else
			{
				Director.Instance.SpriteRenderer.DrawTextWithFontMap(this.Text, Math._00, this.CharWorldHeight, true, this.FontMap, this.Shader);
			}
		}

		public override bool GetlContentLocalBounds(ref Bounds2 bounds)
		{
			bounds = this.GetlContentLocalBounds();
			return true;
		}

		public Bounds2 GetlContentLocalBounds()
		{
			Bounds2 result;
			if (this.FontMap == null)
			{
				result = Director.Instance.SpriteRenderer.DrawTextDebug(this.Text, Math._00, this.CharWorldHeight, false, null);
			}
			else
			{
				result = Director.Instance.SpriteRenderer.DrawTextWithFontMap(this.Text, Math._00, this.CharWorldHeight, false, this.FontMap, this.Shader);
			}
			return result;
		}
	}
}
