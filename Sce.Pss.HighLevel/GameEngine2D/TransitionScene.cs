using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class TransitionScene : Scene
	{
		public Scene PreviousScene;

		public Scene NextScene;

		public float Duration = 0f;

		private Sequence m_seq = null;

		public bool KeepRendering = true;

		protected uint m_render_count = 0u;

		public float FadeCompletion
		{
			get
			{
				return FMath.Clamp((float)base.SceneTime / this.Duration, 0f, 1f);
			}
		}

		public override bool IsTransitionScene()
		{
			return true;
		}

		public TransitionScene(Scene next_scene)
		{
			this.PreviousScene = null;
			this.NextScene = next_scene;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			this.m_seq = new Sequence();
			this.m_seq.Add(new DelayTime(this.Duration));
			this.m_seq.Add(new CallFunc(delegate
			{
				Director.Instance.ReplaceScene(this.NextScene);
			}));
			ActionManager.Instance.AddAction(this.m_seq, this);
			this.m_seq.Run();
		}

		internal void cancel_replace_scene()
		{
			base.StopAction(this.m_seq);
			this.m_seq = null;
		}
	}
}
