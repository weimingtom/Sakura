using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class Scene : Node
	{
		internal double m_elapsed = 0.0;

		public bool NoClear = false;

		public float DrawGridStep = -1f;

		public float DrawGridAutoStepMaxCells = 32f;

		public double SceneTime
		{
			get
			{
				return this.m_elapsed;
			}
		}

		public Scene()
		{
			Camera2D camera2D = new Camera2D(Director.Instance.GL, Director.Instance.DrawHelpers);
			camera2D.SetViewFromHeightAndCenter(16f, Math._00);
			this.Camera = camera2D;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			this.m_elapsed = 0.0;
		}

		public override void OnExit()
		{
			this.Cleanup();
			base.OnExit();
		}

		public virtual bool IsTransitionScene()
		{
			return false;
		}

		public void render()
		{
			this.Camera.SetAspectFromViewport();
			if (!this.NoClear)
			{
				Director.Instance.GL.Context.Clear();
			}
			this.Camera.Push();
			if ((Director.Instance.DebugFlags & DebugFlags.DrawGrid) != 0u)
			{
				float num = this.DrawGridStep;
				Bounds2 bounds = this.Camera.CalcBounds();
				float num2 = (bounds.Size.X > bounds.Size.Y) ? bounds.Size.X : bounds.Size.Y;
				if (num2 == 0f)
				{
					num = 0f;
				}
				else if (num == -1f)
				{
					int num3 = (int)FMath.Floor(FMath.Log(num2 / this.DrawGridAutoStepMaxCells) / FMath.Log(2f)) + 1;
					num3 = ((num3 < 1) ? 0 : num3);
					num = (float)(1 << num3);
					if (num3 > 15)
					{
						num = 0f;
					}
				}
				if (num != 0f)
				{
					this.Camera.DebugDraw(num);
				}
			}
			this.DrawHierarchy();
			if ((Director.Instance.DebugFlags & DebugFlags.DrawContentWorldBounds) != 0u)
			{
				this.draw_content_world_bounds();
			}
			this.Camera.Pop();
		}

		internal void draw_content_world_bounds()
		{
			this.Traverse(delegate(Node node, int depth)
			{
				Bounds2 bounds = default(Bounds2);
				if (node.GetContentWorldBounds(ref bounds))
				{
					Director.Instance.GL.SetBlendMode(BlendMode.Additive);
					Director.Instance.DrawHelpers.SetColor(Colors.Grey20);
					Director.Instance.DrawHelpers.DrawBounds2(bounds);
				}
				return true;
			}, 0);
		}
	}
}
