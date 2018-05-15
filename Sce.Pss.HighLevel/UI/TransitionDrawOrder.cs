using System;

namespace Sce.Pss.HighLevel.UI
{
	public enum TransitionDrawOrder
	{
		CurrentScene,
		NextScene,
		TransitionUIElement,
		CS_NS,
		CS_TE,
		NS_CS,
		NS_TE,
		TE_CS,
		TE_NS,
		CS_NS_TE,
		CS_TE_NS,
		NS_CS_TE,
		NS_TE_CS,
		TE_CS_NS,
		TE_NS_CS
	}
}
