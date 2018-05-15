using System;

namespace Sce.Pss.HighLevel.UI
{
	public enum GestureDetectorResponse
	{
		None,
		UndetectedAndContinue,
		DetectedAndContinue,
		DetectedAndStop,
		FailedAndStop
	}
}
