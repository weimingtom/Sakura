using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class DelayTime : ActionWithDuration
	{
		public DelayTime()
		{
		}

		public DelayTime(float duration)
		{
			this.Duration = duration;
		}

		public override void Run()
		{
			base.Run();
		}

		public override void Update(float dt)
		{
			if (base.IsRunning)
			{
				if (this.Duration <= this.m_elapsed)
				{
					this.Stop();
				}
				this.m_elapsed += dt;
			}
		}
	}
}
