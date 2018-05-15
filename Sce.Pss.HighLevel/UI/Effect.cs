using System;

namespace Sce.Pss.HighLevel.UI
{
	public abstract class Effect
	{
		public event EventHandler<EventArgs> EffectStopped;

		public float TotalElapsedTime
		{
			get;
			private set;
		}

		public bool Playing
		{
			get;
			private set;
		}

		[Obsolete("Use Playing")]
		public bool IsPlaying
		{
			get
			{
				return this.Playing;
			}
		}

		public bool Paused
		{
			get;
			private set;
		}

		public bool Repeating
		{
			get;
			set;
		}

		public Widget Widget
		{
			get;
			set;
		}

		public Effect()
		{
			this.Playing = false;
			this.Paused = false;
			this.Repeating = false;
			this.Widget = null;
		}

		public void Start()
		{
			if (!this.Playing && !this.Paused)
			{
				this.TotalElapsedTime = 0f;
				UISystem.RegisterEffect(this);
				this.Playing = true;
				this.Paused = false;
				this.OnStart();
			}
		}

		internal void Update(float elapsedTime)
		{
			this.TotalElapsedTime += elapsedTime;
			EffectUpdateResponse effectUpdateResponse = this.OnUpdate(elapsedTime);
			if (effectUpdateResponse == EffectUpdateResponse.Finish)
			{
				if (this.Repeating)
				{
					this.OnRepeat();
					this.TotalElapsedTime = 0f;
					return;
				}
				this.Stop();
			}
		}

		public void Stop()
		{
			if (this.Playing || this.Paused)
			{
				this.OnStop();
				if (this.Playing)
				{
					UISystem.UnregisterEffect(this);
				}
				this.Playing = false;
				this.Paused = false;
				if (this.EffectStopped != null)
				{
					this.EffectStopped.Invoke(this, EventArgs.Empty);
				}
				this.TotalElapsedTime = 0f;
			}
		}

		public void Pause()
		{
			if (this.Playing)
			{
				this.OnPause();
				this.Paused = true;
				this.Playing = false;
				UISystem.UnregisterEffect(this);
			}
		}

		public void Resume()
		{
			if (this.Paused)
			{
				this.OnResume();
				this.Paused = false;
				this.Playing = true;
				UISystem.RegisterEffect(this);
			}
		}

		protected abstract void OnStart();

		protected abstract EffectUpdateResponse OnUpdate(float elapsedTime);

		protected abstract void OnStop();

		protected virtual void OnPause()
		{
		}

		protected virtual void OnResume()
		{
		}

		protected virtual void OnRepeat()
		{
		}
	}
}
