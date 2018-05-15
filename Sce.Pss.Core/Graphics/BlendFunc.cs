using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Graphics
{
	public struct BlendFunc
	{
		public BlendFuncMode mode;
		public BlendFuncFactor srcFactor;
		public BlendFuncFactor dstFactor;
		
		public BlendFunc(BlendFuncMode mode, BlendFuncFactor srcFactor, BlendFuncFactor dstFactor)
		{
			this.mode = mode;
			this.srcFactor = srcFactor;
			this.dstFactor = dstFactor;
		}
	}
}
