using System;

namespace Sce.Pss.Core.Graphics
{
	public enum VertexFormat : uint
	{
		None,
		Float = 256u,
		Float2,
		Float3,
		Float4,
		Half = 512u,
		Half2,
		Half3,
		Half4,
		Short = 1536u,
		Short2,
		Short3,
		Short4,
		UShort = 1792u,
		UShort2,
		UShort3,
		UShort4,
		Byte = 2048u,
		Byte2,
		Byte3,
		Byte4,
		UByte = 2304u,
		UByte2,
		UByte3,
		UByte4,
		ShortN = 5632u,
		Short2N,
		Short3N,
		Short4N,
		UShortN = 5888u,
		UShort2N,
		UShort3N,
		UShort4N,
		ByteN = 6144u,
		Byte2N,
		Byte3N,
		Byte4N,
		UByteN = 6400u,
		UByte2N,
		UByte3N,
		UByte4N
	}
}
