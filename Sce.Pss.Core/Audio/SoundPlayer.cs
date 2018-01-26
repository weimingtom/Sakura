using System;
using System.Diagnostics;

using OpenTK.Audio.OpenAL;

using Sakura;

namespace Sce.Pss.Core.Audio
{
	public class SoundPlayer
	{
		public string __filename;
		private int __soundId = -1;
		
		public SoundPlayer(int handle)
		{
			this.__soundId = handle;
		}
				
		public SoundStatus Status
		{
			get
			{
				SoundStatus result = SoundStatus.Stopped;
				ALSourceState state = SakuraSoundManager.GetStatus(this.__soundId);
				switch (state)
				{
					case ALSourceState.Initial:
						result = SoundStatus.Stopped;
						break;
						
					case ALSourceState.Playing:
						result = SoundStatus.Playing;
						break;
						
					case ALSourceState.Paused:
						result = SoundStatus.Stopped;
						break;
						
					case ALSourceState.Stopped:
						result = SoundStatus.Stopped;
						break;
				}
				return result;
			}
		}
		
		public void Play()
		{
			SakuraSoundManager.StopSound(this.__soundId);
			SakuraSoundManager.RewindSound(this.__soundId);
			SakuraSoundManager.PlaySound(this.__soundId);
			Debug.WriteLine("=============play");
		}
		
		public void Stop()
		{
			if (Status == SoundStatus.Playing)
			{
				SakuraSoundManager.StopSound(this.__soundId);
				Debug.WriteLine("=============stop");
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
