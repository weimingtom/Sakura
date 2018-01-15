using System;

namespace Sce.Pss.Core.Graphics
{
	[Flags]
	public enum EnableMode : uint
	{
		None = 0u,
		ScissorTest = 1u,
		CullFace = 2u,
		Blend = 4u,
		DepthTest = 8u,
		PolygonOffsetFill = 16u,
		StencilTest = 32u,
		Dither = 64u,
		All = 127u
	}
}
