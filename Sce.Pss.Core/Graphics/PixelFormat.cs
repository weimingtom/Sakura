using System;

namespace Sce.Pss.Core.Graphics
{
	public enum PixelFormat : uint
	{
		None,
		Rgba,
		RgbaH,
		Rgba4444,
		Rgba5551,
		Rgb565,
		LuminanceAlpha,
		LuminanceAlphaH,
		Luminance,
		LuminanceH,
		Alpha,
		AlphaH,
		Depth16,
		Depth24,
		Depth16Stencil8,
		Depth24Stencil8
	}
}
