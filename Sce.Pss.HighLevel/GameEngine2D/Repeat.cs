using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Repeat : ActionBase
	{
		public ActionBase InnerAction;

		public int Times = 0;

		private int m_count = 0;

		public Repeat(ActionBase inner_action, int times)
		{
			this.InnerAction = inner_action;
			this.Times = times;
		}

		public override void Run()
		{
			base.Run();
			if (this.InnerAction == null)
			{
				this.Stop();
			}
			else
			{
				this.m_count = 0;
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
			if ((!base.IsRunning || !this.InnerAction.IsRunning) && this.m_count < this.Times)
			{
				base.Target.RunAction(this.InnerAction);
				this.m_count++;
			}
		}
	}
}
