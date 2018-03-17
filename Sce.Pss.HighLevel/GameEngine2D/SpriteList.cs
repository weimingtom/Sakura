using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class SpriteList : Node
	{
		public bool EnableLocalTransform = true;

		public Vector4 Color = Colors.White;

		public BlendMode BlendMode = BlendMode.Normal;

		public TextureInfo TextureInfo;

		public SpriteRenderer.ISpriteShader Shader = Director.Instance.SpriteRenderer.DefaultShader;

		public SpriteList(TextureInfo texture_info)
		{
			this.TextureInfo = texture_info;
		}

		public override void DrawHierarchy()
		{
			if (this.Visible)
			{
				if ((Director.Instance.DebugFlags & DebugFlags.DrawTransform) != 0u)
				{
					base.DebugDrawTransform();
				}
				this.PushTransform();
				Director.Instance.GL.SetBlendMode(this.BlendMode);
				this.Shader.SetColor(ref this.Color);
				this.Shader.SetUVTransform(ref Math.UV_TransformFlipV);
				Director.Instance.SpriteRenderer.BeginSprites(this.TextureInfo, this.Shader, base.Children.Count);
				int i;
				for (i = 0; i < base.Children.Count; i++)
				{
					if (base.Children[i].Order >= 0)
					{
						break;
					}
					if (!this.EnableLocalTransform)
					{
						((SpriteBase)base.Children[i]).internal_draw();
					}
					else
					{
						((SpriteBase)base.Children[i]).internal_draw_cpu_transform();
					}
				}
				this.Draw();
				while (i < base.Children.Count)
				{
					if (!this.EnableLocalTransform)
					{
						((SpriteBase)base.Children[i]).internal_draw();
					}
					else
					{
						((SpriteBase)base.Children[i]).internal_draw_cpu_transform();
					}
					i++;
				}
				Director.Instance.SpriteRenderer.EndSprites();
				if ((Director.Instance.DebugFlags & DebugFlags.DrawPivot) != 0u)
				{
					base.DebugDrawPivot();
				}
				if ((Director.Instance.DebugFlags & DebugFlags.DrawContentLocalBounds) != 0u)
				{
					this.DebugDrawContentLocalBounds();
				}
				this.PopTransform();
			}
		}
	}
}
