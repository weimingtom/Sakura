using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class UIPrimitiveVertex
	{
		internal bool NeedUpdatePosition = true;

		internal bool NeedUpdateTexcoord = true;

		internal bool NeedUpdateColor = true;

		private float u;

		private float v;

		private UIColor color = new UIColor(1f, 1f, 1f, 1f);

		private Vector3 position3D = default(Vector3);

		public float X
		{
			get
			{
				return this.position3D.X;
			}
			set
			{
				this.position3D.X = value;
				this.NeedUpdatePosition = true;
			}
		}

		public float Y
		{
			get
			{
				return this.position3D.Y;
			}
			set
			{
				this.position3D.Y = value;
				this.NeedUpdatePosition = true;
			}
		}

		public float Z
		{
			get
			{
				return this.position3D.Z;
			}
			set
			{
				this.position3D.Z = value;
				this.NeedUpdatePosition = true;
			}
		}

		public float U
		{
			get
			{
				return this.u;
			}
			set
			{
				this.u = value;
				this.NeedUpdateTexcoord = true;
			}
		}

		public float V
		{
			get
			{
				return this.v;
			}
			set
			{
				this.v = value;
				this.NeedUpdateTexcoord = true;
			}
		}

		public UIColor Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.NeedUpdateColor = true;
			}
		}

		public Vector3 Position3D
		{
			get
			{
				return this.position3D;
			}
			set
			{
				this.position3D = value;
				this.NeedUpdatePosition = true;
			}
		}

		public void SetPosition(float x, float y)
		{
			this.position3D.X = x;
			this.position3D.Y = y;
			this.NeedUpdatePosition = true;
		}

		public void SetPosition(float x, float y, float z)
		{
			this.position3D.X = x;
			this.position3D.Y = y;
			this.position3D.Z = z;
			this.NeedUpdatePosition = true;
		}
	}
}
