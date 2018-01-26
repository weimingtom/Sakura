using System;
using System.Diagnostics;

using Sakura;

namespace Sce.Pss.Core.Audio
{
	public class Bgm : IDisposable
	{
		private string __filename;
		private int __soundId = -1;
		
		public Bgm(string filename)
		{
			this.__filename = filename.Replace("/Application/", "./");
			this.__soundId = SakuraSoundManager.LoadFile(this.__filename);
			if (this.__soundId < 0)
			{
				Debug.Assert(false);
			}
		}
		
		public BgmPlayer CreatePlayer()
		{
			BgmPlayer player = new BgmPlayer(__soundId);
			player.__filename = this.__filename;
			return player;
		}
		
		public void Dispose()
		{
		
		}
	}
}
