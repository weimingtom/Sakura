using System;

namespace Sce.Pss.Core.Input
{
	[Flags]
	public enum GamePadButtons : uint
	{
		Left = 1u,
		Up = 2u,
		Right = 4u,
		Down = 8u,
		Square = 16u,
		Triangle = 32u,
		Circle = 64u,
		Cross = 128u,
		Start = 256u,
		Select = 512u,
		L = 1024u,
		R = 2048u,
		Enter = 65536u,
		Back = 131072u
	}
}
