using System;

using System.Diagnostics;

namespace Sce.Pss.Core.Graphics
{
	public struct CullFace
	{
		internal uint bits;

		public CullFaceMode Mode
		{
			get
			{
				return (CullFaceMode)this.bits;
			}
			set
			{
				this.bits = ((this.bits & 0xFFFFFF00) | (uint)value);
			}
		}

		public CullFaceDirection Direction
		{
			get
			{
				return (CullFaceDirection)(this.bits >> 8);
			}
			set
			{
				this.bits = ((this.bits & 0xFFFFFF00) | (uint)((uint)value << 8));
			}
		}

		public CullFace(CullFaceMode mode, CullFaceDirection direction)
		{
			this.bits = (uint)((uint)mode | ((uint)direction << 8));
		}

		public void Set(CullFaceMode mode, CullFaceDirection direction)
		{
			this.bits = (uint)((uint)mode | ((uint)direction << 8));
		}
	}
}
