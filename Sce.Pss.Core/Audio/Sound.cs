using System;
using System.Diagnostics;

using Sakura;

namespace Sce.Pss.Core.Audio
{
	public class Sound
	{
		private string __filename;
		private int __soundId = -1;
		
		public Sound(string filename)
		{
			this.__filename = filename.Replace("/Application/", "./");
			this.__soundId = SakuraSoundManager.LoadFile(this.__filename);
			if (this.__soundId < 0)
			{
				Debug.Assert(false);
			}
		}
		
		public SoundPlayer CreatePlayer()
		{
			SoundPlayer player = new SoundPlayer(__soundId);
			player.__filename = this.__filename;
			return player;
		}
		
		public void Dispose()
		{
		
		}
	}
}
