using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class CallFunc : ActionBase
	{
		public Action Func;

		public CallFunc(Action func)
		{
			this.Func = func;
		}

		public override void Update(float dt)
		{
			if (base.IsRunning)
			{
				if (this.Func != null)
				{
					this.Func.Invoke();
				}
				this.Stop();
			}
		}
	}
}
