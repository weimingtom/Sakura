using System;
using System.Diagnostics;

using OpenTK.Graphics.ES20;

namespace Sce.Pss.Core.Graphics
{
	public class DepthFunc
	{
		public uint bits;
		
		public DepthFunction Mode
		{
			get
			{
				return (DepthFunction)this.bits;
			}
			set
			{
				this.bits = ((this.bits & 0xFFFFFF00) | (uint)value);
			}
		}
		
		public bool WriteMask
		{
			get
			{
				return (this.bits & 256u) != 0u;
			}
			set
			{
				this.bits = ((this.bits & 4294902015u) | ((!value) ? 0u : 256u));
			}
		}		
		
		public DepthFunc()
		{
			//Debug.Assert(false);
		}
	}
}
