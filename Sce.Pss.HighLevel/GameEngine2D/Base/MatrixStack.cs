using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class MatrixStack
	{
		internal struct Entry
		{
			public Matrix4 m_value;

			public Matrix4 m_inverse;

			public bool m_inverse_dirty;

			public bool m_orthonormal;
		}

		private MatrixStack.Entry[] m_stack;

		private uint m_index;

		private uint m_capacity;

		private uint m_tag;

		public uint Tag
		{
			get
			{
				return this.m_tag;
			}
		}

		public uint Size
		{
			get
			{
				return this.m_index + 1u;
			}
		}

		public uint Capacity
		{
			get
			{
				return this.m_capacity;
			}
		}

		public MatrixStack(uint capacity)
		{
			this.m_index = 0u;
			this.m_capacity = capacity;
			this.m_stack = new MatrixStack.Entry[capacity];
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = Matrix4.Identity;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse = Matrix4.Identity;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = false;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = true;
			this.m_tag = 0u;
		}

		public void Push()
		{
			this.m_index += 1u;
			Common.Assert(this.m_index < this.m_capacity);
			this.m_stack[(int)((UIntPtr)this.m_index)] = this.m_stack[(int)((UIntPtr)(this.m_index - 1u))];
			this.m_tag += 1u;
		}

		public void Pop()
		{
			Common.Assert(this.m_index >= 0u);
			this.m_index -= 1u;
			this.m_tag += 1u;
		}

		public void Mul(Matrix4 mat)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * mat;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = false;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void Mul1(Matrix4 mat)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * mat;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void Set(Matrix4 mat)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = mat;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = false;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void Set1(Matrix4 mat)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = mat;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = true;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public Matrix4 Get()
		{
			return this.m_stack[(int)((UIntPtr)this.m_index)].m_value;
		}

		public Matrix4 GetInverse()
		{
			if (this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty)
			{
				if (this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal)
				{
					this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse = this.m_stack[(int)((UIntPtr)this.m_index)].m_value.InverseOrthonormal();
				}
				else
				{
					this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse = this.m_stack[(int)((UIntPtr)this.m_index)].m_value.Inverse();
				}
				this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = false;
			}
			return this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse;
		}

		public void SetIdentity()
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = Matrix4.Identity;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse = Matrix4.Identity;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = true;
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = false;
			this.m_tag += 1u;
		}

		public void RotateX(float angle)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.RotationX(angle);
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void RotateY(float angle)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.RotationY(angle);
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void RotateZ(float angle)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.RotationZ(angle);
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void Rotate(Vector3 axis, float angle)
		{
			this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.RotationAxis(axis, angle);
			this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
			this.m_tag += 1u;
		}

		public void Scale(Vector3 value)
		{
			if (!(value == Math._111))
			{
				this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.Scale(value);
				this.m_stack[(int)((UIntPtr)this.m_index)].m_orthonormal = false;
				this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
				this.m_tag += 1u;
			}
		}

		public void Translate(Vector3 value)
		{
			if (!(value == Math._000))
			{
				this.m_stack[(int)((UIntPtr)this.m_index)].m_value = this.m_stack[(int)((UIntPtr)this.m_index)].m_value * Matrix4.Translation(value);
				this.m_stack[(int)((UIntPtr)this.m_index)].m_inverse_dirty = true;
				this.m_tag += 1u;
			}
		}
	}
}
