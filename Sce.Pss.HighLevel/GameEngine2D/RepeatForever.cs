using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class RepeatForever : ActionBase
	{
		public ActionBase InnerAction;

		public override void Run()
		{
			base.Run();
			if (this.InnerAction == null)
			{
				this.Stop();
			}
			else
			{
				base.Target.RunAction(this.InnerAction);
			}
		}

		public override void Stop()
		{
			base.Stop();
			if (this.InnerAction != null)
			{
				this.InnerAction.Stop();
			}
		}

		public override void Update(float dt)
		{
			if (!base.IsRunning || !this.InnerAction.IsRunning)
			{
				this.Run();
			}
		}
	}
}
