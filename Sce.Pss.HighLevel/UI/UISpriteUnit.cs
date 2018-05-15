using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class UISpriteUnit
	{
		internal bool NeedUpdatePosition = true;

		internal bool NeedUpdateTexcoord = true;

		internal bool NeedUpdateColor = true;

		private float width;

		private float height;

		private float u1;

		private float v1;

		private float u2;

		private float v2;

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

		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				this.NeedUpdatePosition = true;
			}
		}

		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				this.NeedUpdatePosition = true;
			}
		}

		public float U1
		{
			get
			{
				return this.u1;
			}
			set
			{
				this.u1 = value;
				this.NeedUpdateTexcoord = true;
			}
		}

		public float V1
		{
			get
			{
				return this.v1;
			}
			set
			{
				this.v1 = value;
				this.NeedUpdateTexcoord = true;
			}
		}

		public float U2
		{
			get
			{
				return this.u2;
			}
			set
			{
				this.u2 = value;
				this.NeedUpdateTexcoord = true;
			}
		}

		public float V2
		{
			get
			{
				return this.v2;
			}
			set
			{
				this.v2 = value;
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

		public UISpriteUnit()
		{
			this.U2 = 1f;
			this.V2 = 1f;
		}

		public void SetPosition(float x, float y)
		{
			this.position3D.X = x;
			this.position3D.Y = y;
			this.NeedUpdatePosition = true;
		}

		public void SetSize(float width, float height)
		{
			this.width = width;
			this.height = height;
			this.NeedUpdatePosition = true;
		}
	}
}
