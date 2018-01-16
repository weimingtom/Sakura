using System;

namespace Sce.Pss.Core.Input
{
	public class GamePadData //FIXME:not struct
	{
		public bool Skip;

		public GamePadButtons Buttons;
		public GamePadButtons ButtonsPrev;
		public GamePadButtons ButtonsDown;
		public GamePadButtons ButtonsUp;

		public float AnalogLeftX;
		public float AnalogLeftY;
		public float AnalogRightX;
		public float AnalogRightY;
	}
}
