using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Environment
{
	//FIXME:need STAThread in Main()
	public static class Clipboard
	{
		public static string GetText()
		{
			System.Windows.Forms.IDataObject iData = System.Windows.Forms.Clipboard.GetDataObject();
			if(iData.GetDataPresent(System.Windows.Forms.DataFormats.Text)) 
			{
		    	return (string)iData.GetData(System.Windows.Forms.DataFormats.Text); 
		    }
			return "";
		}	
		
		public static void SetText(string text)
		{
			System.Windows.Forms.Clipboard.SetDataObject(text, true);
		}
	}
	
}
