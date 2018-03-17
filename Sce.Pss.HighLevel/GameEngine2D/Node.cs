using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class Node
	{
		public delegate void DDraw();

		public delegate void DOnExitEvent();

		public delegate bool DVisitor(Node node, int depth);

		private Vector2 m_position;

		private Vector2 m_rotation;

		private Vector2 m_scale;

		private Vector2 m_skew;

		private Vector2 m_skew_tan;

		private Vector2 m_pivot;

		private int m_order;

		private bool m_cached_local_transform_info_is_identity;

		private bool m_cached_local_transform_info_is_orthonormal;

		private bool m_cached_local_transform_info_is_dirty;

		private Matrix3 m_cached_local_transform;

		private bool m_is_running;

		private byte m_scheduler_and_action_manager_pause_flag;

		internal List<Scheduler.Entry> m_scheduler_entries;

		internal List<ActionBase> m_action_entries;

		public float VertexZ;

		public bool Visible;

		protected Node m_parent;

		protected List<Node> m_children;

		public ICamera Camera = null;

		public string Name;

		public static float DebugDrawTransformScale = 1f;

		public event Node.DDraw AdHocDraw;

		public event Node.DOnExitEvent OnExitEvents;

		public Vector2 Position
		{
			get
			{
				return this.m_position;
			}
			set
			{
				this.m_position = value;
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public Vector2 Rotation
		{
			get
			{
				return this.m_rotation;
			}
			set
			{
				this.m_rotation = value;
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public Vector2 RotationNormalize
		{
			get
			{
				return this.m_rotation;
			}
			set
			{
				this.m_rotation = value.Normalize();
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public float Angle
		{
			get
			{
				return Math.Angle(this.Rotation);
			}
			set
			{
				this.Rotation = Vector2.Rotation(value);
			}
		}

		public Vector2 Scale
		{
			get
			{
				return this.m_scale;
			}
			set
			{
				this.m_scale = value;
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public Vector2 Skew
		{
			get
			{
				return this.m_skew;
			}
			set
			{
				this.m_skew = value;
				this.m_skew_tan = new Vector2(FMath.Tan(this.m_skew.X), FMath.Tan(this.m_skew.Y));
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public Vector2 Pivot
		{
			get
			{
				return this.m_pivot;
			}
			set
			{
				this.m_pivot = value;
				this.m_cached_local_transform_info_is_dirty = true;
			}
		}

		public Node Parent
		{
			get
			{
				return this.m_parent;
			}
		}

		public List<Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		public Camera2D Camera2D
		{
			get
			{
				return (Camera2D)this.Camera;
			}
		}

		public Camera3D Camera3D
		{
			get
			{
				return (Camera3D)this.Camera;
			}
		}

		public bool IsRunning
		{
			get
			{
				return this.m_is_running;
			}
		}

		public int Order
		{
			get
			{
				return this.m_order;
			}
		}

		public bool ActionsPaused
		{
			get
			{
				return (this.m_scheduler_and_action_manager_pause_flag & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.m_scheduler_and_action_manager_pause_flag |= 1;
				}
				else
				{
					this.m_scheduler_and_action_manager_pause_flag &= 254;
				}
			}
		}

		public bool SchedulerPaused
		{
			get
			{
				return (this.m_scheduler_and_action_manager_pause_flag & 2) != 0;
			}
			set
			{
				if (value)
				{
					this.m_scheduler_and_action_manager_pause_flag |= 2;
				}
				else
				{
					this.m_scheduler_and_action_manager_pause_flag &= 253;
				}
			}
		}

		public void Rotate(float angle)
		{
			this.Rotation = this.Rotation.Rotate(angle);
		}

		public void Rotate(Vector2 rotation)
		{
			this.Rotation = this.Rotation.Rotate(rotation);
		}

		public Node()
		{
			this.Position = Math._00;
			this.Rotation = Math._10;
			this.Scale = Math._11;
			this.Skew = Math._00;
			this.Pivot = Math._00;
			this.VertexZ = 0f;
			this.m_order = 0;
			this.m_children = new List<Node>();
			this.Visible = true;
			this.m_parent = null;
			this.m_cached_local_transform_info_is_identity = true;
			this.m_cached_local_transform_info_is_orthonormal = true;
			this.m_cached_local_transform_info_is_dirty = false;
			this.m_cached_local_transform = Matrix3.Identity;
			this.m_is_running = false;
			this.m_scheduler_and_action_manager_pause_flag = 0;
		}

		~Node()
		{
			this.RemoveAllChildren(true);
			this.Cleanup();
		}

		public virtual void PushTransform()
		{
			if (this.Camera != null)
			{
				this.Camera.Push();
			}
			Director.Instance.GL.ModelMatrix.Push();
			if (this.m_cached_local_transform_info_is_orthonormal)
			{
				Director.Instance.GL.ModelMatrix.Mul1(this.GetTransform().Matrix4());
			}
			else
			{
				Director.Instance.GL.ModelMatrix.Mul(this.GetTransform().Matrix4());
			}
			if (this.VertexZ != 0f)
			{
				Director.Instance.GL.ModelMatrix.Translate(new Vector3(0f, 0f, this.VertexZ));
			}
		}

		public virtual void PopTransform()
		{
			Director.Instance.GL.ModelMatrix.Pop();
			if (this.Camera != null)
			{
				this.Camera.Pop();
			}
		}

		public virtual void OnEnter()
		{
			Director.Instance.DebugLog(" OnEnter " + this.DebugInfo());
			foreach (Node current in this.Children)
			{
				current.OnEnter();
			}
			this.ResumeSchedulerAndActions();
			this.m_is_running = true;
		}

		public virtual void OnExit()
		{
			Director.Instance.DebugLog(" OnExit " + this.DebugInfo());
			this.PauseSchedulerAndActions();
			this.m_is_running = false;
			foreach (Node current in this.Children)
			{
				current.OnExit();
			}
			if (this.OnExitEvents != null)
			{
				this.OnExitEvents();
				this.OnExitEvents = null;
			}
		}

		private void ListIDisposable(ref List<Node> list)
		{
			Type[] interfaces = base.GetType().GetInterfaces();
			Type[] array = interfaces;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				if (type == typeof(IDisposable))
				{
					list.Add(this);
					break;
				}
			}
			foreach (Node current in this.Children)
			{
				current.ListIDisposable(ref list);
			}
		}

		public void RegisterDisposeOnExitRecursive()
		{
			this.OnExitEvents += delegate
			{
				this.Cleanup();
			};
			List<Node> list = new List<Node>();
			this.ListIDisposable(ref list);
			foreach (Node current in list)
			{
				this.RegisterDisposeOnExit((IDisposable)current);
			}
		}

		public void RegisterDisposeOnExit(IDisposable disposable)
		{
			this.OnExitEvents += delegate
			{
				disposable.Dispose();
			};
		}

		private void insert_child(Node child, int order)
		{
			Node node = null;
			if (this.Children.Count != 0)
			{
				node = this.Children[this.Children.Count - 1];
			}
			if (node == null || node.m_order <= order)
			{
				this.Children.Add(child);
			}
			else
			{
				int num = 0;
				foreach (Node current in this.Children)
				{
					Common.Assert(current != null);
					if (current.m_order > order)
					{
						this.Children.Insert(num, child);
						break;
					}
					num++;
				}
			}
			child.m_order = order;
		}

		public void AddChild(Node child, int order)
		{
			Common.Assert(child != this, "Trying to add " + child + " as child of itself.");
			Common.Assert(child != null, "Trying to add a null child.");
			Common.Assert(child.Parent == null, "Child " + child + " alreay has a parent, it can't be added somewhere else.");
			this.insert_child(child, order);
			child.m_parent = this;
			if (this.m_is_running)
			{
				child.OnEnter();
			}
		}

		public void AddChild(Node child)
		{
			this.AddChild(child, child.m_order);
		}

		public void RemoveChild(Node child, bool do_cleanup)
		{
			if (child != null)
			{
				if (this.Children.Contains(child))
				{
					child.on_remove(do_cleanup);
					this.Children.Remove(child);
				}
			}
		}

		public void RemoveAllChildren(bool do_cleanup)
		{
			foreach (Node current in this.Children)
			{
				current.on_remove(do_cleanup);
				current.m_parent = null;
			}
			this.Children.Clear();
		}

		private void on_remove(bool do_cleanup)
		{
			if (this.m_is_running)
			{
				this.OnExit();
			}
			if (do_cleanup)
			{
				this.Cleanup();
			}
			this.m_parent = null;
		}

		private void ReorderChild(Node child, int order)
		{
			this.Children.Remove(child);
			this.insert_child(child, order);
		}

		private void Reorder(int order)
		{
			if (this.Parent != null)
			{
				this.Parent.ReorderChild(this, order);
			}
		}

		public virtual void Traverse(Node.DVisitor visitor, int depth)
		{
			if (visitor(this, depth))
			{
				foreach (Node current in this.Children)
				{
					current.Traverse(visitor, depth + 1);
				}
			}
		}

		public virtual void DrawHierarchy()
		{
			if (this.Visible)
			{
				this.PushTransform();
				int i;
				for (i = 0; i < this.Children.Count; i++)
				{
					if (this.Children[i].Order >= 0)
					{
						break;
					}
					this.Children[i].DrawHierarchy();
				}
				this.Draw();
				while (i < this.Children.Count)
				{
					this.Children[i].DrawHierarchy();
					i++;
				}
				if ((Director.Instance.DebugFlags & DebugFlags.DrawPivot) != 0u)
				{
					this.DebugDrawPivot();
				}
				if ((Director.Instance.DebugFlags & DebugFlags.DrawContentLocalBounds) != 0u)
				{
					this.DebugDrawContentLocalBounds();
				}
				this.PopTransform();
				if ((Director.Instance.DebugFlags & DebugFlags.DrawTransform) != 0u)
				{
					this.DebugDrawTransform();
				}
			}
		}

		public virtual void Draw()
		{
			if (this.AdHocDraw != null)
			{
				this.AdHocDraw();
			}
		}

		public virtual void Update(float dt)
		{
		}

		public virtual void DebugDrawContentLocalBounds()
		{
			Bounds2 bounds = default(Bounds2);
			this.GetlContentLocalBounds(ref bounds);
			Director.Instance.DrawHelpers.SetColor(Colors.Yellow);
			Director.Instance.DrawHelpers.DrawBounds2(bounds);
		}

		public void DebugDrawPivot()
		{
			Director.Instance.DrawHelpers.SetColor(Colors.White);
			Director.Instance.DrawHelpers.DrawDisk(this.Pivot, 0.1f, 12u);
		}

		public void DebugDrawTransform()
		{
			Matrix3 transform = this.GetTransform();
			transform.X.Xy = transform.X.Xy * Node.DebugDrawTransformScale;
			transform.Y.Xy = transform.Y.Xy * Node.DebugDrawTransformScale;
			Director.Instance.DrawHelpers.DrawCoordinateSystem2D(transform, new DrawHelpers.ArrowParams(Node.DebugDrawTransformScale));
		}

		public void RunAction(ActionBase action)
		{
			ActionManager.Instance.AddAction(action, this);
			action.Run();
		}

		public void StopAllActions()
		{
			ActionManager.Instance.RemoveAllActionsFromTarget(this);
		}

		public void StopAction(ActionBase action)
		{
			Common.Assert(action.Target == this || action.Target == null);
			ActionManager.Instance.RemoveAction(action);
		}

		public void StopActionByTag(int tag)
		{
			ActionManager.Instance.RemoveActionByTag(tag, this);
		}

		public ActionBase GetActionByTag(int tag, int ith = 0)
		{
			return ActionManager.Instance.GetActionByTag(tag, this, ith);
		}

		public int NumRunningActions()
		{
			return ActionManager.Instance.NumRunningActions(this);
		}

		public virtual void Cleanup()
		{
			this.StopAllActions();
			this.UnscheduleAll();
			foreach (Node current in this.Children)
			{
				current.Cleanup();
			}
		}

		public void ScheduleUpdate(int priority = 0)
		{
			Scheduler.Instance.ScheduleUpdateForTarget(this, priority, !this.m_is_running);
		}

		public void UnscheduleUpdate()
		{
			Scheduler.Instance.UnscheduleUpdateForTarget(this);
		}

		public void Schedule(DSchedulerFunc func, int priority = 0)
		{
			Scheduler.Instance.Schedule(this, func, 0f, !this.m_is_running, priority);
		}

		public void ScheduleInterval(DSchedulerFunc func, float interval, int priority = 0)
		{
			Scheduler.Instance.Schedule(this, func, interval, !this.m_is_running, priority);
		}

		public void Unschedule(DSchedulerFunc func)
		{
			Scheduler.Instance.Unschedule(this, func);
		}

		public void UnscheduleAll()
		{
			Scheduler.Instance.UnscheduleAll(this);
		}

		public void ResumeSchedulerAndActions()
		{
			this.SchedulerPaused = false;
			this.ActionsPaused = false;
		}

		public void PauseSchedulerAndActions()
		{
			this.SchedulerPaused = true;
			this.ActionsPaused = true;
		}

		public Matrix3 GetTransform()
		{
			if (this.m_cached_local_transform_info_is_dirty)
			{
				Math.TranslationRotationScale(ref this.m_cached_local_transform, this.Position + this.Pivot, this.Rotation, this.Scale);
				this.m_cached_local_transform *= new Matrix3(new Vector3(1f, this.m_skew_tan.X, 0f), new Vector3(this.m_skew_tan.Y, 1f, 0f), new Vector3(-this.Pivot * (Math._11 + this.m_skew_tan.Yx), 1f));
				this.m_cached_local_transform_info_is_identity = false;
				this.m_cached_local_transform_info_is_orthonormal = (this.Scale == Math._11 && this.Skew == Math._00);
				this.m_cached_local_transform_info_is_dirty = false;
			}
			return this.m_cached_local_transform;
		}

		public Matrix3 GetTransformInverse()
		{
			Matrix3 result;
			if (this.m_cached_local_transform_info_is_orthonormal)
			{
				result = this.GetTransform().InverseOrthonormal();
			}
			else
			{
				result = this.GetTransform().Inverse();
			}
			return result;
		}

		public Matrix3 GetWorldTransform()
		{
			Matrix3 matrix = this.GetTransform();
			for (Node parent = this.Parent; parent != null; parent = parent.Parent)
			{
				matrix = parent.GetTransform() * matrix;
			}
			return matrix;
		}

		public Matrix3 CalcWorldTransformInverse()
		{
			Matrix3 matrix = this.GetTransformInverse();
			for (Node parent = this.Parent; parent != null; parent = parent.Parent)
			{
				matrix *= parent.GetTransformInverse();
			}
			return matrix;
		}

		public Vector2 LocalToWorld(Vector2 local_point)
		{
			Vector3 v = this.GetTransform() * local_point.Xy1;
			for (Node parent = this.Parent; parent != null; parent = parent.Parent)
			{
				if (!parent.m_cached_local_transform_info_is_identity)
				{
					v = parent.GetTransform() * v;
				}
			}
			return v.Xy;
		}

		public Vector2 WorldToLocal(Vector2 world_point)
		{
			Matrix3 matrix = this.GetTransformInverse();
			for (Node parent = this.Parent; parent != null; parent = parent.Parent)
			{
				if (!parent.m_cached_local_transform_info_is_identity)
				{
					matrix *= parent.GetTransformInverse();
				}
			}
			return (matrix * world_point.Xy1).Xy;
		}

		public virtual bool GetlContentLocalBounds(ref Bounds2 bounds)
		{
			return false;
		}

		public virtual bool GetContentWorldBounds(ref Bounds2 bounds)
		{
			Bounds2 bounds2 = default(Bounds2);
			bool result;
			if (!this.GetlContentLocalBounds(ref bounds2))
			{
				result = false;
			}
			else
			{
				Matrix3 worldTransform = this.GetWorldTransform();
				bounds = new Bounds2((worldTransform * bounds2.Point00.Xy1).Xy);
				bounds.Add((worldTransform * bounds2.Point10.Xy1).Xy);
				bounds.Add((worldTransform * bounds2.Point01.Xy1).Xy);
				bounds.Add((worldTransform * bounds2.Point11.Xy1).Xy);
				result = true;
			}
			return result;
		}

		public bool IsWorldPointInsideContentLocalBounds(Vector2 world_position)
		{
			Bounds2 bounds = default(Bounds2);
			return this.GetlContentLocalBounds(ref bounds) && bounds.IsInside(this.WorldToLocal(world_position));
		}

		public virtual void FindParentPlane(ref Matrix4 mat)
		{
			if (this.Parent != null)
			{
				this.Parent.FindParentPlane(ref mat);
			}
		}

		private Vector2 NormalizedToWorld(Vector2 bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos)
		{
			Vector2 result;
			if (this.Camera != null)
			{
				result = this.Camera.NormalizedToWorld(bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos);
			}
			else
			{
				Matrix4 identity = Matrix4.Identity;
				this.FindParentPlane(ref identity);
				Director.Instance.CurrentScene.Camera.SetTouchPlaneMatrix(identity);
				result = Director.Instance.CurrentScene.Camera.NormalizedToWorld(bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos);
			}
			return result;
		}

		public Vector2 GetTouchPos(int nth = 0, bool prev = false)
		{
			Vector2 touchPos;
			if (this.Camera != null)
			{
				touchPos = this.Camera.GetTouchPos(nth, prev);
			}
			else
			{
				Matrix4 identity = Matrix4.Identity;
				this.FindParentPlane(ref identity);
				Director.Instance.CurrentScene.Camera.SetTouchPlaneMatrix(identity);
				touchPos = Director.Instance.CurrentScene.Camera.GetTouchPos(nth, prev);
			}
			return touchPos;
		}

		public virtual string DebugInfo()
		{
			return string.Concat(new string[]
			{
				"{",
				base.GetType().Name,
				":",
				this.Name,
				"}"
			});
		}
	}
}
