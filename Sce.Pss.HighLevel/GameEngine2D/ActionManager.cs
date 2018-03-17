using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class ActionManager
	{
		private delegate bool DRemoveCondition(ActionBase a);

		private List<ActionBase> m_cache = new List<ActionBase>();

		private HashSet<Node> m_nodes = new HashSet<Node>();

		internal static ActionManager m_instance;

		public static ActionManager Instance
		{
			get
			{
				return ActionManager.m_instance;
			}
		}

		public void AddAction(ActionBase action, Node target)
		{
			this.RemoveAction(action);
			if (!this.m_nodes.Contains(target))
			{
				this.m_nodes.Add(target);
				if (target.m_action_entries == null)
				{
					target.m_action_entries = new List<ActionBase>();
				}
			}
			action.set_target(target);
			target.m_action_entries.Add(action);
		}

		public void RemoveAllActions()
		{
			foreach (Node current in this.m_nodes)
			{
				this.remove_all_actions_from_target(current, false);
			}
			this.m_nodes.Clear();
		}

		public void RemoveAllActionsFromTarget(Node target)
		{
			this.remove_all_actions_from_target(target, true);
		}

		public void remove_all_actions_from_target(Node target, bool update_node_set)
		{
			List<ActionBase> action_entries = target.m_action_entries;
			if (action_entries != null)
			{
				foreach (ActionBase current in action_entries)
				{
					current.Stop();
					current.set_target(null);
				}
				action_entries.Clear();
			}
			if (update_node_set && this.m_nodes.Contains(target))
			{
				this.m_nodes.Remove(target);
			}
		}

		private void remove_action_from_target_if(Node target, ActionManager.DRemoveCondition cond)
		{
			List<ActionBase> action_entries = target.m_action_entries;
			if (action_entries != null)
			{
				for (int i = 0; i < action_entries.Count; i++)
				{
					if (cond(action_entries[i]))
					{
						action_entries[i].Stop();
						action_entries[i].set_target(null);
						action_entries.RemoveAt(i);
						break;
					}
				}
				if (action_entries.Count == 0)
				{
					this.m_nodes.Remove(target);
				}
			}
		}

		public void RemoveAction(ActionBase action)
		{
			action.Stop();
			if (action.Target != null)
			{
				this.remove_action_from_target_if(action.Target, (ActionBase a) => a == action);
			}
		}

		public void RemoveActionByTag(int tag, Node target)
		{
			this.remove_action_from_target_if(target, (ActionBase a) => a.Tag == tag);
		}

		public ActionBase GetActionByTag(int tag, Node target, int ith = 0)
		{
			List<ActionBase> action_entries = target.m_action_entries;
			ActionBase result;
			if (action_entries == null)
			{
				result = null;
			}
			else
			{
				foreach (ActionBase current in action_entries)
				{
					if (current.Tag == tag && ith-- == 0)
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		public int NumRunningActions(Node target)
		{
			List<ActionBase> action_entries = target.m_action_entries;
			int result;
			if (action_entries == null)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (ActionBase current in action_entries)
				{
					if (current.IsRunning)
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}

		public int NumRunningActions()
		{
			int num = 0;
			foreach (Node current in this.m_nodes)
			{
				List<ActionBase> action_entries = current.m_action_entries;
				if (action_entries != null)
				{
					foreach (ActionBase current2 in action_entries)
					{
						num++;
					}
				}
			}
			return num;
		}

		public void Update(float dt)
		{
			this.m_cache.Clear();
			foreach (Node current in this.m_nodes)
			{
				List<ActionBase> action_entries = current.m_action_entries;
				if (action_entries != null)
				{
					for (int i = 0; i < action_entries.Count; i++)
					{
						this.m_cache.Add(action_entries[i]);
					}
				}
			}
			foreach (ActionBase current2 in this.m_cache)
			{
				if (current2.IsRunning)
				{
					if (!current2.Target.ActionsPaused)
					{
						current2.Update(dt);
					}
					if (!current2.IsRunning)
					{
						this.RemoveAction(current2);
					}
				}
			}
		}

		public void Dump()
		{
			string text = Common.FrameCount + " ActionManager: ";
			int num = 0;
			foreach (Node current in this.m_nodes)
			{
				List<ActionBase> action_entries = current.m_action_entries;
				if (action_entries != null)
				{
					Console.WriteLine(text + current.DebugInfo());
					foreach (ActionBase current2 in action_entries)
					{
						if (current2 == null)
						{
							Console.WriteLine(text + "\tnull");
						}
						else
						{
							Console.WriteLine(text + "\t" + current2.GetType().ToString());
						}
						num++;
					}
				}
			}
			Console.WriteLine(string.Concat(new object[]
			{
				text,
				"(",
				num,
				" actions running)"
			}));
		}
	}
}
