using System;
using System.Diagnostics;
using System.Threading;
using Sakura.OpenTK;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Input;

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
namespace Sce.Pss.Core.Environment
{
	public static class SystemEvents
	{
		private static Stopwatch timer = new Stopwatch();
		public static void CheckEvents ()
		{
			MyGameWindow.ProcessEvents();
			
			var keyboard = OpenTK.Input.Keyboard.GetState();
			GamePadData gamePadData = GamePad.__gamePadData;
			gamePadData.ButtonsPrev = gamePadData.Buttons;
			gamePadData.ButtonsDown = 0;
			gamePadData.Buttons = 0;
			gamePadData.ButtonsUp = 0;
			
			if (keyboard.IsKeyDown(OpenTK.Input.Key.Escape))
            {
            	System.Environment.Exit(0);
            }
			//
			if (keyboard.IsKeyDown(OpenTK.Input.Key.Left))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Left;
            	gamePadData.Buttons |= GamePadButtons.Left;
            }
			if (keyboard.IsKeyDown(OpenTK.Input.Key.Right))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Right;
            	gamePadData.Buttons |= GamePadButtons.Right;
            }
			if (keyboard.IsKeyDown(OpenTK.Input.Key.Up))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Up;
            	gamePadData.Buttons |= GamePadButtons.Up;
            }
			if (keyboard.IsKeyDown(OpenTK.Input.Key.Down))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Down;
            	gamePadData.Buttons |= GamePadButtons.Down;
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.A))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Square;
            	gamePadData.Buttons |= GamePadButtons.Square;
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.W))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Triangle;
            	gamePadData.Buttons |= GamePadButtons.Triangle;
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.D))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Circle;
            	gamePadData.Buttons |= GamePadButtons.Circle;
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.S))
            {
            	gamePadData.ButtonsDown |= GamePadButtons.Cross;
            	gamePadData.Buttons |= GamePadButtons.Cross;
            } 
            
            
            
            
            
            
            
            
            
            
            
            
           	if (keyboard.IsKeyUp(OpenTK.Input.Key.Left))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Left;
            }
			if (keyboard.IsKeyUp(OpenTK.Input.Key.Right))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Right;
            }
			if (keyboard.IsKeyUp(OpenTK.Input.Key.Up))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Up;
            }
			if (keyboard.IsKeyUp(OpenTK.Input.Key.Down))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Down;
            }
            if (keyboard.IsKeyUp(OpenTK.Input.Key.A))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Square;
            }
            if (keyboard.IsKeyUp(OpenTK.Input.Key.W))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Triangle;
            }
            if (keyboard.IsKeyUp(OpenTK.Input.Key.D))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Circle;
            }
            if (keyboard.IsKeyUp(OpenTK.Input.Key.S))
            {
            	gamePadData.ButtonsUp |= GamePadButtons.Cross;
            }  
            
            
            
            
            
#if false
			double delta = timer.Elapsed.TotalMilliseconds;
			double frame = 1000.0 / 24.0;
			if (delta < frame)
			{
				int free = (int)(frame - delta);
				Thread.Sleep(free);
				//Debug.WriteLine("Sleep: " + free);
			}
			timer.Restart();
#endif
		}
	}
}
