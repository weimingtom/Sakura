using System;
using System.Diagnostics;

using OpenTK.Audio.OpenAL;

using Sakura;

namespace Sce.Pss.Core.Audio
{
	public class BgmPlayer
	{
		public string __filename;
		private int __soundId = -1;
		
		public BgmPlayer(int handle)
		{
			this.__soundId = handle;
		}
				
		public BgmStatus Status
		{
			get
			{
				BgmStatus result = BgmStatus.Stopped;
				ALSourceState state = SakuraSoundManager.GetStatus(this.__soundId);
				switch (state)
				{
					case ALSourceState.Initial:
						result = BgmStatus.Stopped;
						break;
						
					case ALSourceState.Playing:
						result = BgmStatus.Playing;
						break;
						
					case ALSourceState.Paused:
						result = BgmStatus.Paused;
						break;
						
					case ALSourceState.Stopped:
						result = BgmStatus.Stopped;
						break;
				}
				return result;
			}
		}
		
		public void Play()
		{
			if (Status == BgmStatus.Stopped || Status == BgmStatus.Paused) //FIXME: Can restart???
			{
				SakuraSoundManager.StopSound(this.__soundId);
				SakuraSoundManager.RewindSound(this.__soundId);
				SakuraSoundManager.PlaySound(this.__soundId);
				Debug.WriteLine("=============play");
			}
		}
		
		public void Stop()
		{
			if (Status == BgmStatus.Playing)
			{
				SakuraSoundManager.StopSound(this.__soundId);
				Debug.WriteLine("=============stop");
			}
		}
		
		public void Pause()
		{
			if (Status == BgmStatus.Playing)
			{
				SakuraSoundManager.PauseSound(this.__soundId);
				Debug.WriteLine("=============pause");
			}
		}
		
		public void Resume()
		{
			//see https://www.gamedev.net/forums/topic/410512-pause-and-then-resume-in-openal/
			if (Status == BgmStatus.Paused)
			{
				SakuraSoundManager.PlaySound(this.__soundId);
				Debug.WriteLine("=============resume");
			}
		}
		
		public float Volume
		{
			get
			{
				return SakuraSoundManager.GetVolume(this.__soundId);
			}
			
			set
			{
				SakuraSoundManager.SetVolume(this.__soundId, value);
			}
		}
	}
}
