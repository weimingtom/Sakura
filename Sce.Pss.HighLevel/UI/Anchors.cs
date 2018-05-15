using System;

namespace Sce.Pss.HighLevel.UI
{
	[Flags]
	public enum Anchors
	{
		None = 0,
		Top = 1,
		Bottom = 2,
		Height = 4,
		Left = 16,
		Right = 32,
		Width = 64
	}
}
