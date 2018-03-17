using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Particles : Node, IDisposable
	{
		public ParticleSystem ParticleSystem;

		public Particles(int max_particles)
		{
			this.ParticleSystem = new ParticleSystem(max_particles);
			base.ScheduleUpdate(0);
		}

		public override void Update(float dt)
		{
			base.Update(dt);
			this.ParticleSystem.Update(dt);
		}

		public override void Draw()
		{
			base.Draw();
			this.ParticleSystem.Draw();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Common.DisposeAndNullify<ParticleSystem>(ref this.ParticleSystem);
			}
		}
	}
}
