using System;

namespace Sce.Pss.Core.Environment
{
	public static class Shell
	{
		public struct Action
		{
			public enum ActionType : uint
			{
				None,
				Browser
			}
			
			public ActionType type;
			public string parameter0;
			public string parameter1;
			public string parameter2;
			public string parameter3;
			
			public static Shell.Action BrowserAction(string url)
			{
				Shell.Action act = new Shell.Action();
				act.type = Shell.Action.ActionType.Browser;
				act.parameter0 = url;
				return act;
			}
		}
		
		public static void Execute(ref Action action)
		{
			if (action.type == Action.ActionType.Browser)
			{
				System.Diagnostics.Process.Start(action.parameter0);
			}
		}
	}
}

