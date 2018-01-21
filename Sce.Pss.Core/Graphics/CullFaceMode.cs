using System;
namespace Sce.Pss.Core.Graphics
{
	[Flags ]
	public enum CullFaceMode : byte
	{
		None = 0,
		Front = 1,
		Back = 2,
		FrontAndBack = 3
	}
}
