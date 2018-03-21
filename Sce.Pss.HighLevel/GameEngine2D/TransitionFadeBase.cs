using Sce.Pss.Core.Graphics;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class TransitionFadeBase : TransitionScene
	{
		private static bool m_graphics_resources_init = false;

		private static FrameBuffer m_fbuf1;

		private static FrameBuffer m_fbuf2;

		private static uint m_last_update_scenes_render = 4294967295u;

		protected static TextureInfo m_previous_scene_render;

		protected static TextureInfo m_next_scene_render;

		public TransitionFadeBase(Scene next_scene) : base(next_scene)
		{
			Vector2i value = new Vector2i(Director.Instance.GL.Context.GetViewport().Width, Director.Instance.GL.Context.GetViewport().Height);
			if (!TransitionFadeBase.m_graphics_resources_init)
			{
				Texture2D t1 = new Texture2D(value.X, value.Y, false, PixelFormat.Rgba, PixelBufferOption.Renderable, true); //FIXME:added???
				//t1.__supportNPOT = true; //FIXME:added???
				TransitionFadeBase.m_previous_scene_render = new TextureInfo(t1);
				Texture2D t2 = new Texture2D(value.X, value.Y, false, PixelFormat.Rgba, PixelBufferOption.Renderable, true); //FIXME:added???
				//t2.__supportNPOT = true; //FIXME:added???
				TransitionFadeBase.m_next_scene_render = new TextureInfo(t2);
				TransitionFadeBase.m_fbuf1 = new FrameBuffer();
				TransitionFadeBase.m_fbuf2 = new FrameBuffer();
				TransitionFadeBase.m_fbuf1.SetColorTarget(TransitionFadeBase.m_previous_scene_render.Texture, 0);
				TransitionFadeBase.m_fbuf2.SetColorTarget(TransitionFadeBase.m_next_scene_render.Texture, 0);
				TransitionFadeBase.m_graphics_resources_init = true;
			}
			else
			{
				Common.Assert(TransitionFadeBase.m_previous_scene_render.TextureSizei == value);
				Common.Assert(TransitionFadeBase.m_next_scene_render.TextureSizei == value);
			}
		}

		public static void Terminate()
		{
			Common.DisposeAndNullify<FrameBuffer>(ref TransitionFadeBase.m_fbuf1);
			Common.DisposeAndNullify<FrameBuffer>(ref TransitionFadeBase.m_fbuf2);
			Common.DisposeAndNullify<TextureInfo>(ref TransitionFadeBase.m_previous_scene_render);
			Common.DisposeAndNullify<TextureInfo>(ref TransitionFadeBase.m_next_scene_render);
		}

		protected void update_scenes_render()
		{
			this.m_render_count += 1u;
			if (TransitionFadeBase.m_last_update_scenes_render != Common.FrameCount)
			{
				TransitionFadeBase.m_last_update_scenes_render = Common.FrameCount;
				Director.Instance.GL.Context.SetFrameBuffer(TransitionFadeBase.m_fbuf1);
				this.PreviousScene.render();
				Director.Instance.GL.Context.SetFrameBuffer(TransitionFadeBase.m_fbuf2);
				this.NextScene.render();
				Director.Instance.GL.Context.SetFrameBuffer(null);
			}
		}

		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void Draw()
		{
			if (this.KeepRendering || this.m_render_count == 0u)
			{
				this.update_scenes_render();
			}
		}
	}
}
