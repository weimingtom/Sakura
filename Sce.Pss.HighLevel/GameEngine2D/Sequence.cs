using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Sequence : ActionBase
	{
		private List<ActionBase> m_actions = new List<ActionBase>();

		private int m_current = 0;

		public void Add(ActionBase action)
		{
			Common.Assert(!base.IsRunning);
			this.m_actions.Add(action);
		}

		public override void Run()
		{
			base.Run();
			this.m_current = 0;
			if (this.m_actions.Count == 0)
			{
				this.Stop();
			}
			else
			{
				base.Target.RunAction(this.m_actions[this.m_current]);
			}
		}

		public override void Stop()
		{
			base.Stop();
			foreach (ActionBase current in this.m_actions)
			{
				if (current != null)
				{
					current.Stop();
				}
			}
		}

		public override void Update(float dt)
		{
			if (base.IsRunning)
			{
				if (!this.m_actions[this.m_current].IsRunning)
				{
					if (this.m_current == this.m_actions.Count - 1)
					{
						this.Stop();
					}
					else
					{
						this.m_current++;
						base.Target.RunAction(this.m_actions[this.m_current]);
					}
				}
			}
		}
	}
}
