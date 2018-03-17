using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public abstract class ActionWithDuration : ActionBase
	{
		protected float m_elapsed = 0f;

		public float Duration = 0f;

		public override void Run()
		{
			base.Run();
			this.m_elapsed = 0f;
		}
	}
}
