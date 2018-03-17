using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public abstract class ActionTweenGeneric<T> : ActionWithDuration
	{
		public delegate void DSet(T value);

		public delegate T DGet();

		public T TargetValue;

		public bool IsRelative = false;

		protected T m_start_value;

		public DTween Tween = (float t) => Math.PowEaseOut(t, 4f);

		public ActionTweenGeneric<T>.DSet Set;

		public ActionTweenGeneric<T>.DGet Get;

		public abstract void lerp(float alpha);

		public ActionTweenGeneric()
		{
		}

		public ActionTweenGeneric(ActionTweenGeneric<T>.DGet dget, ActionTweenGeneric<T>.DSet dset)
		{
			this.Get = dget;
			this.Set = dset;
		}

		public override void Run()
		{
			base.Run();
			this.m_start_value = this.Get();
		}

		public override void Update(float dt)
		{
			if (base.IsRunning)
			{
				float t = FMath.Clamp(this.m_elapsed / this.Duration, 0f, 1f);
				float alpha = this.Tween(t);
				this.lerp(alpha);
				if (this.m_elapsed / this.Duration > 1f)
				{
					this.Stop();
				}
				this.m_elapsed += dt;
			}
		}
	}
}
