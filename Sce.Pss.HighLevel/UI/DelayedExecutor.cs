using System;

namespace Sce.Pss.HighLevel.UI
{
	public class DelayedExecutor : Effect
	{
		private TimeSpan startTime;

		private float time;

		private Action action;

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public Action Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		public DelayedExecutor()
		{
			base.Widget = null;
			this.Time = 0f;
			this.Action = null;
		}

		public DelayedExecutor(float time, Action action)
		{
			base.Widget = null;
			this.Time = time;
			this.action = action;
		}

		public static DelayedExecutor CreateAndStart(float time, Action action)
		{
			DelayedExecutor delayedExecutor = new DelayedExecutor(time, action);
			delayedExecutor.Start();
			return delayedExecutor;
		}

		protected override void OnStart()
		{
			if (this.action == null)
			{
				base.Stop();
			}
			this.startTime = UISystem.CurrentTime;
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (this.startTime == UISystem.CurrentTime)
			{
				return EffectUpdateResponse.Continue;
			}
			if (base.TotalElapsedTime > this.Time)
			{
				if (this.action != null)
				{
					this.action.Invoke();
				}
				return EffectUpdateResponse.Finish;
			}
			return EffectUpdateResponse.Continue;
		}

		protected override void OnStop()
		{
		}
	}
}
