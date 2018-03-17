using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Profiler
	{
		public class Node
		{
			public string Name;

			public float Duration;

			public int Depth;

			public Timer Timer;
		}

		private int m_nodes_size = 0;

		private List<Profiler.Node> m_nodes = new List<Profiler.Node>();

		private List<Profiler.Node> m_stack = new List<Profiler.Node>();

		public void HeartBeat()
		{
			this.m_stack.Clear();
			this.m_nodes_size = 0;
		}

		public void Push(string name)
		{
			if (this.m_nodes_size >= this.m_nodes.Count)
			{
				this.m_nodes.Add(new Profiler.Node
				{
					Timer = new Timer()
				});
			}
			Profiler.Node node = this.m_nodes[this.m_nodes_size];
			node.Name = name;
			node.Depth = this.m_stack.Count;
			node.Timer.Reset();
			this.m_stack.Add(node);
			this.m_nodes_size++;
		}

		public void Pop()
		{
			this.m_stack[this.m_stack.Count - 1].Duration = (float)this.m_stack[this.m_stack.Count - 1].Timer.Milliseconds();
			this.m_stack.RemoveAt(this.m_stack.Count - 1);
		}

		public void Dump()
		{
			Console.WriteLine("");
			Console.WriteLine("--- frame " + Common.FrameCount + "'s timers:");
			Common.Assert(this.m_stack.Count == 0, "number of Profiler Push/Push doesn't match");
			for (int i = 0; i < this.m_nodes_size; i++)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					new string('\t', this.m_nodes[i].Depth),
					this.m_nodes[i].Name,
					" ",
					this.m_nodes[i].Duration,
					" ms"
				}));
			}
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			for (int i = 0; i < this.m_nodes_size; i++)
			{
				Profiler.Node node = this.m_nodes[i];
				if (!dictionary.ContainsKey(node.Name))
				{
					dictionary.Add(node.Name, 0f);
					dictionary2.Add(node.Name, 0);
				}
				Dictionary<string, float> dictionary3;
				string name;
				(dictionary3 = dictionary)[name = node.Name] = dictionary3[name] + node.Duration;
				Dictionary<string, int> dictionary4;
				(dictionary4 = dictionary2)[name = node.Name] = dictionary4[name] + 1;
			}
			Console.WriteLine("");
			Console.WriteLine("--- frame " + Common.FrameCount + "'s timers totals:");
			foreach (KeyValuePair<string, float> current in dictionary)
			{
				Console.WriteLine("total for {0} = {1} ms ({2} calls)", current.Key, current.Value, dictionary2[current.Key]);
			}
		}
	}
}
