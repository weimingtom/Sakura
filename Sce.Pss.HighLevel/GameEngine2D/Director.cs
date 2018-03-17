using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Director : IDisposable
	{
		private delegate void DSceneEvent();

		private List<Scene> m_scenes_stack = new List<Scene>();

		private Timer m_frame_timer;

		private bool m_paused;

		private bool m_run_with_scene_called;

		private double m_elapsed;

		public GraphicsContextAlpha GL;

		public SpriteRenderer SpriteRenderer;

		public DrawHelpers DrawHelpers;

		public uint DebugFlags = 0u;

		private static Director m_instance;

		private HashSet<Scene> m_canceled_replace_scene;

		private event Director.DSceneEvent m_scene_events;

		public double DirectorTime
		{
			get
			{
				return this.m_elapsed;
			}
		}

		public static Director Instance
		{
			get
			{
				return Director.m_instance;
			}
		}

		public Scene CurrentScene
		{
			get
			{
				return this.get_top_scene();
			}
		}

		public static void Initialize(uint sprites_capacity = 500u, uint draw_helpers_capacity = 400u, GraphicsContext context = null)
		{
			Director.m_instance = new Director(sprites_capacity, draw_helpers_capacity, context);
			Scheduler.m_instance = new Scheduler();
			ActionManager.m_instance = new ActionManager();
		}

		public static void Terminate()
		{
			while (Director.m_instance.m_scenes_stack.Count != 0)
			{
				Director.m_instance.pop_scene();
			}
			Common.DisposeAndNullify<Director>(ref Director.m_instance);
			Scheduler.m_instance = null;
			ActionManager.m_instance = null;
			TransitionFadeBase.Terminate();
			TransitionDirectionalFade.Terminate();
			ParticleSystem.Terminate();
		}

		private Director(uint sprites_capacity, uint draw_helpers_capacity, GraphicsContext context)
		{
			this.m_paused = false;
			this.m_frame_timer = new Timer();
			this.m_run_with_scene_called = false;
			this.m_elapsed = 0.0;
			this.GL = new GraphicsContextAlpha(context);
			this.DrawHelpers = new DrawHelpers(this.GL, draw_helpers_capacity);
			this.SpriteRenderer = new SpriteRenderer(this.GL, 6u * sprites_capacity);
			this.m_canceled_replace_scene = new HashSet<Scene>();
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
				Common.DisposeAndNullify<GraphicsContextAlpha>(ref this.GL);
				Common.DisposeAndNullify<DrawHelpers>(ref this.DrawHelpers);
				Common.DisposeAndNullify<SpriteRenderer>(ref this.SpriteRenderer);
			}
		}

		public void DebugLog(string text)
		{
			if ((Director.Instance.DebugFlags & Sce.Pss.HighLevel.GameEngine2D.DebugFlags.Log) != 0u)
			{
				Console.WriteLine(string.Format("{0:00000}", Common.FrameCount) + " " + text);
			}
		}

		private void on_scene_exit(ref Scene scene)
		{
			if (scene != null)
			{
				scene.OnExit();
			}
		}

		private void on_scene_enter(ref Scene scene)
		{
			if (scene != null)
			{
				scene.OnEnter();
			}
		}

		private void on_scene_change(Scene previous_scene, Scene next_scene)
		{
			Common.Assert(previous_scene != next_scene);
			bool flag = previous_scene != null && previous_scene.IsTransitionScene();
			bool flag2 = next_scene != null && next_scene.IsTransitionScene();
			if (!flag && !flag2)
			{
				this.on_scene_exit(ref previous_scene);
				this.on_scene_enter(ref next_scene);
			}
			else if (!flag && flag2)
			{
				((TransitionScene)next_scene).PreviousScene = previous_scene;
				this.on_scene_enter(ref next_scene);
				this.on_scene_enter(ref ((TransitionScene)next_scene).NextScene);
			}
			else if (flag && !flag2)
			{
				Common.Assert(next_scene == ((TransitionScene)previous_scene).NextScene);
				this.on_scene_exit(ref ((TransitionScene)previous_scene).PreviousScene);
				this.on_scene_exit(ref previous_scene);
			}
			else if (flag && flag2)
			{
				((TransitionScene)next_scene).PreviousScene = ((TransitionScene)previous_scene).NextScene;
				((TransitionScene)previous_scene).cancel_replace_scene();
				this.m_canceled_replace_scene.Add(((TransitionScene)previous_scene).NextScene);
				this.on_scene_exit(ref ((TransitionScene)previous_scene).PreviousScene);
				this.on_scene_exit(ref previous_scene);
				this.on_scene_enter(ref next_scene);
				this.on_scene_enter(ref ((TransitionScene)next_scene).NextScene);
			}
		}

		private Scene get_top_scene()
		{
			Scene result;
			if (this.m_scenes_stack.Count == 0)
			{
				result = null;
			}
			else
			{
				result = this.m_scenes_stack[this.m_scenes_stack.Count - 1];
			}
			return result;
		}

		private bool is_transitionning()
		{
			Scene top_scene = this.get_top_scene();
			return top_scene != null && top_scene.IsTransitionScene();
		}

		private void replace_scene(Scene new_scene)
		{
			if (!this.m_canceled_replace_scene.Contains(new_scene))
			{
				Common.Assert(this.m_scenes_stack.Count > 0, "Can't ReplaceScene: scene stack is empty");
				Scene top_scene = this.get_top_scene();
				this.m_scenes_stack[this.m_scenes_stack.Count - 1] = new_scene;
				Common.Assert(top_scene != new_scene);
				this.on_scene_change(top_scene, new_scene);
			}
		}

		private void push_scene(Scene new_scene)
		{
			Scene top_scene = this.get_top_scene();
			this.m_scenes_stack.Add(new_scene);
			this.on_scene_change(top_scene, new_scene);
		}

		private void pop_scene()
		{
			Common.Assert(this.m_scenes_stack.Count > 0, "Can't PopScene: scene stack is empty");
			Scene top_scene = this.get_top_scene();
			this.m_scenes_stack.RemoveAt(this.m_scenes_stack.Count - 1);
			this.on_scene_change(top_scene, this.get_top_scene());
		}

		public void ReplaceScene(Scene new_scene)
		{
			this.m_scene_events += delegate
			{
				this.replace_scene(new_scene);
			};
		}

		public void PushScene(Scene new_scene)
		{
			this.m_scene_events += delegate
			{
				this.push_scene(new_scene);
			};
		}

		public void PopScene()
		{
			this.m_scene_events += delegate
			{
				this.pop_scene();
			};
		}

		public void Pause()
		{
			this.m_paused = true;
		}

		public void Resume()
		{
			this.m_paused = false;
			this.m_frame_timer.Reset();
		}

		public void Dump()
		{
			this.DebugLog("CurrentScene.m_elapsed time = " + this.CurrentScene.m_elapsed);
			this.CurrentScene.Traverse(delegate(Node node, int depth)
			{
				this.DebugLog(" " + new string(' ', depth * 2) + node.DebugInfo());
				return true;
			}, 0);
		}

		private void internal_step(float dt)
		{
			if ((this.DebugFlags & Sce.Pss.HighLevel.GameEngine2D.DebugFlags.Navigate) != 0u && this.CurrentScene != null)
			{
				bool down = Input2.GamePad0.Triangle.Down;
				bool down2 = Input2.GamePad0.Square.Down;
				this.CurrentScene.Camera.Navigate(down2 ? 1 : (down ? 2 : 0));
			}
			Scheduler.Instance.Update(dt);
			ActionManager.Instance.Update(dt);
			this.CurrentScene.m_elapsed += (double)dt;
			this.m_elapsed += (double)dt;
		}

		public void Update()
		{
			Common.Assert(this.m_run_with_scene_called, "RunWithScene hasn't been called");
			this.m_canceled_replace_scene.Clear();
			if (this.m_scene_events != null)
			{
				this.m_scene_events();
			}
			this.m_scene_events = null;
			if (this.CurrentScene != null)
			{
				if (!this.m_paused)
				{
					float dt = (float)this.m_frame_timer.Seconds();
					this.m_frame_timer.Reset();
					this.internal_step(dt);
				}
			}
		}

		public void Render()
		{
			if (this.CurrentScene != null)
			{
				this.CurrentScene.render();
			}
			else
			{
				this.DebugLog("no scene has been set, please call RunWithScene");
			}
		}

		public void PostSwap()
		{
			Common.OnSwap();
		}

		public void RunWithScene(Scene scene, bool manual_loop = false)
		{
			Common.Assert(this.CurrentScene == null);
			Common.Assert(!this.m_run_with_scene_called, "You can't call RunWithScene more than once.");
			this.PushScene(scene);
			this.m_run_with_scene_called = true;
			if (manual_loop)
			{
				return;
			}
			while (true)
			{
				SystemEvents.CheckEvents();
				this.Update();
				this.Render();
				Director.Instance.GL.Context.SwapBuffers();
				this.PostSwap();
			}
		}
	}
}
