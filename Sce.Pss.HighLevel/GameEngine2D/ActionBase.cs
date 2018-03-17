using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class ActionBase
	{
		private bool m_running = false;

		private Node m_target_node;

		public int Tag;

		public bool IsRunning
		{
			get
			{
				return this.m_running;
			}
		}

		public Node Target
		{
			get
			{
				return this.m_target_node;
			}
		}

		internal void set_target(Node value)
		{
			Common.Assert(!this.IsRunning);
			this.m_target_node = value;
		}

		public virtual void Run()
		{
			this.m_running = true;
		}

		public virtual void Stop()
		{
			this.m_running = false;
		}

		public virtual void Update(float dt)
		{
		}
	}
}
