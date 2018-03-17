using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Scheduler
	{
		internal class Entry
		{
			internal Node m_node;

			internal DSchedulerFunc m_func;

			internal float m_interval;

			internal float m_interval_counter;

			internal bool Valid
			{
				get
				{
					return this.m_func != null;
				}
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"m_interval=",
					this.m_interval,
					" m_node=",
					(this.m_node != null) ? this.m_node.ToString() : "",
					" m_func=",
					(this.m_func != null) ? this.m_func.ToString() : "",
					" SchedulerPaused=",
					this.m_node.SchedulerPaused
				});
			}

			internal void invalidate()
			{
				this.m_node = null;
				this.m_func = null;
			}
		}

		private const int max_priority = 3;

		public const int PriorityGroups = 7;

		public const int DefaultPriority = 0;

		private List<Scheduler.Entry>[] m_groups;

		private List<Scheduler.Entry> m_cache;

		private HashSet<Node> m_nodes;

		private List<Node> m_nodes_to_remove;

		internal static Scheduler m_instance;

		public static Scheduler Instance
		{
			get
			{
				return Scheduler.m_instance;
			}
		}

		private Scheduler.Entry add_entry(Node node, DSchedulerFunc func, float interval)
		{
			Scheduler.Entry entry = new Scheduler.Entry
			{
				m_node = node,
				m_func = func,
				m_interval = interval
			};
			if (!this.m_nodes.Contains(node))
			{
				this.m_nodes.Add(node);
			}
			if (node.m_scheduler_entries == null)
			{
				node.m_scheduler_entries = new List<Scheduler.Entry>();
			}
			node.m_scheduler_entries.Add(entry);
			return entry;
		}

		private void invalidate_entry(Node node, DSchedulerFunc func)
		{
			if (this.m_nodes.Contains(node))
			{
				foreach (Scheduler.Entry current in node.m_scheduler_entries)
				{
					if (current.m_func == func)
					{
						current.invalidate();
					}
				}
			}
		}

		private void invalidate_all_entries(Node node)
		{
			if (this.m_nodes.Contains(node))
			{
				this.invalidate_all_entries(ref node.m_scheduler_entries);
			}
		}

		private void invalidate_all_entries(ref List<Scheduler.Entry> list)
		{
			foreach (Scheduler.Entry current in list)
			{
				current.invalidate();
			}
		}

		public Scheduler()
		{
			this.m_nodes = new HashSet<Node>();
			this.m_groups = new List<Scheduler.Entry>[7];
			this.m_cache = new List<Scheduler.Entry>();
			this.m_nodes_to_remove = new List<Node>();
			for (int i = 0; i < 7; i++)
			{
				this.m_groups[i] = new List<Scheduler.Entry>();
			}
		}

		private void schedule_internal(Node target, DSchedulerFunc func, float interval, int priority)
		{
			Scheduler.Entry entry = this.add_entry(target, func, interval);
			this.m_groups[3 + priority].Add(entry);
		}

		public void Schedule(Node target, DSchedulerFunc func, float interval, bool paused, int priority = 0)
		{
			this.schedule_internal(target, func, interval, priority);
			target.SchedulerPaused = paused;
		}

		public void Unschedule(Node target, DSchedulerFunc func)
		{
			this.invalidate_entry(target, func);
		}

		public void UnscheduleAll(Node target)
		{
			this.invalidate_all_entries(target);
		}

		public void UnscheduleAll()
		{
			foreach (Node current in this.m_nodes)
			{
				this.invalidate_all_entries(current);
			}
		}

		public void ScheduleUpdateForTarget(Node target, int priority, bool paused)
		{
			this.schedule_internal(target, new DSchedulerFunc(target.Update), 0f, priority);
			target.SchedulerPaused = paused;
		}

		public void UnscheduleUpdateForTarget(Node target)
		{
			this.Unschedule(target, new DSchedulerFunc(target.Update));
		}

		public void Dump()
		{
			string text = Common.FrameCount + " Scheduler: ";
			Console.WriteLine(text + "Node set");
			foreach (Node current in this.m_nodes)
			{
				foreach (Scheduler.Entry current2 in current.m_scheduler_entries)
				{
					Common.Assert(current2 != null);
					Console.WriteLine(text + current2);
				}
			}
			int num = 0;
			List<Scheduler.Entry>[] groups = this.m_groups;
			for (int i = 0; i < groups.Length; i++)
			{
				List<Scheduler.Entry> list = groups[i];
				if (list.Count != 0)
				{
					Console.WriteLine(string.Concat(new object[]
					{
						text,
						"group ",
						num,
						"  entries:",
						list.Count
					}));
					foreach (Scheduler.Entry current2 in list)
					{
						Common.Assert(current2 != null);
						Console.WriteLine(text + current2);
					}
					num++;
				}
			}
		}

		internal void Update(float dt)
		{
			this.m_nodes_to_remove.Clear();
			foreach (Node current in this.m_nodes)
			{
				List<Scheduler.Entry> list = current.m_scheduler_entries;
				int i = 0;
				while (i < list.Count)
				{
					if (!list[i].Valid)
					{
						list[i] = list[list.Count - 1];
						list.RemoveAt(list.Count - 1);
					}
					else
					{
						i++;
					}
				}
				if (list.Count == 0)
				{
					this.m_nodes_to_remove.Add(current);
				}
			}
			foreach (Node current in this.m_nodes_to_remove)
			{
				this.m_nodes.Remove(current);
			}
			List<Scheduler.Entry>[] groups = this.m_groups;
			for (int j = 0; j < groups.Length; j++)
			{
				List<Scheduler.Entry> list = groups[j];
				this.m_cache.Clear();
				int i = 0;
				while (i < list.Count)
				{
					if (!list[i].Valid)
					{
						list[i] = list[list.Count - 1];
						list.RemoveAt(list.Count - 1);
					}
					else
					{
						this.m_cache.Add(list[i]);
						i++;
					}
				}
				foreach (Scheduler.Entry current2 in this.m_cache)
				{
					if (current2.Valid)
					{
						if (!current2.m_node.SchedulerPaused)
						{
							current2.m_interval_counter += dt;
						}
						if (current2.m_interval_counter > current2.m_interval)
						{
							current2.m_func(dt);
							current2.m_interval_counter -= current2.m_interval;
						}
					}
				}
			}
		}
	}
}
