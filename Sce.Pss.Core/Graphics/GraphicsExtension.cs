using System;

namespace Sce.Pss.Core.Graphics
{
	[Flags]
	public enum GraphicsExtension : uint
	{
		None = 0u,
		DepthTexture = 1u,
		Texture3D = 2u,
		TextureNpot = 4u,
		TextureFilterAnisotropic = 8u,
		Rgb8Rgba8 = 16u,
		Depth24 = 32u,
		Depth32 = 64u,
		PackedDepthStencil = 128u,
		VertexHalfFloat = 256u,
		Vertex1010102 = 512u,
		TextureFloat = 1024u,
		TextureHalfFloat = 2048u,
		TextureFloatLinear = 4096u,
		TextureHalfFloatLinear = 8192u,
		Texture2101010Rev = 16384u
	}
}
