using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Graphics
{
	public enum ColorMask : byte
	{
		None = 0,
		R = 1,
		G = 2,
		B = 4,
		A = 8,
		Rgb = 7,
		Rgba = 15
	}
}
