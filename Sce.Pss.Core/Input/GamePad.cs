using System;
/*
About the PC Simulator

Keys on the gamepad	Key assignments on the PC simulator
Left directional key	Cursor key: ←
Up directional key	Cursor key: ↑
Right directional key	Cursor key: →
Down directional key	Cursor key: ↓
Square button	Alphabet: A
Triangle button	Alphabet: W
Circle button	Alphabet: D
Cross button	Alphabet: S
SELECT button	Alphabet: Z
START button	Alphabet: X
L button	Alphabet: Q
R button	Alphabet: E
*/
namespace Sce.Pss.Core.Input
{
	public static class GamePad
	{
		public static GamePadData __gamePadData = new GamePadData();
		
		public static GamePadData GetData(int deviceIndex)
		{
			return __gamePadData;
		}
	}
}
