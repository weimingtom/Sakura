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
		private static Stopwatch __timer = new Stopwatch();
		private static bool __isMouseLeftDown = false;
		private static bool __isKeyLeftDown = false;
		private static bool __isKeyRightDown = false;
		private static bool __isKeyUpDown = false;
		private static bool __isKeyDownDown = false;
		private static bool __isKeyADown = false;
		private static bool __isKeyWDown = false;
		private static bool __isKeyDDown = false;
		private static bool __isKeySDown = false;
		
		public static void CheckEvents ()
		{
			MyGameWindow.ProcessEvents();
			
			
			bool __isWinFocused = MyGameWindow.getFocused();
            OpenTK.Input.KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();
			GamePadData gamePadData = GamePad.__gamePadData;
			gamePadData.ButtonsPrev = gamePadData.Buttons;
			gamePadData.ButtonsDown = 0;
			gamePadData.Buttons = 0;
			gamePadData.ButtonsUp = 0;
			
			if (__isWinFocused)
			{
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Escape))
	            {
	            	System.Environment.Exit(0);
	            }
				//
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Left))
	            {
					if (!__isKeyLeftDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Left;
					}
					__isKeyLeftDown = true;
					gamePadData.Buttons |= GamePadButtons.Left;
	            }
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Right))
	            {
	            	if (!__isKeyRightDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Right;
					}
					__isKeyRightDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Right;
	            }
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Up))
	            {
	            	if (!__isKeyUpDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Up;
					}
					__isKeyUpDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Up;
	            }
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Down))
	            {
	            	if (!__isKeyDownDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Down;
					}
					__isKeyDownDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Down;
	            }
	            if (keyboard.IsKeyDown(OpenTK.Input.Key.A))
	            {
	            	if (!__isKeyADown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Square;
					}
					__isKeyADown = true;
	            	gamePadData.Buttons |= GamePadButtons.Square;
	            }
	            if (keyboard.IsKeyDown(OpenTK.Input.Key.W))
	            {
	            	if (!__isKeyWDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Triangle;
					}
					__isKeyWDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Triangle;
	            }
	            if (keyboard.IsKeyDown(OpenTK.Input.Key.D))
	            {
	            	if (!__isKeyDDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Circle;
					}
					__isKeyDDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Circle;
	            }
	            if (keyboard.IsKeyDown(OpenTK.Input.Key.S))
	            {
	            	if (!__isKeySDown)
					{
						gamePadData.ButtonsDown |= GamePadButtons.Cross;
					}
					__isKeySDown = true;
	            	gamePadData.Buttons |= GamePadButtons.Cross;
	            } 
	            
	            
	            
	            
	            
	            
	            
	            
	            
	            
	            
	            
	           	if (keyboard.IsKeyUp(OpenTK.Input.Key.Left))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Left;
	            	__isKeyLeftDown = false;
	            }
				if (keyboard.IsKeyUp(OpenTK.Input.Key.Right))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Right;
	            	__isKeyRightDown = false;
	            }
				if (keyboard.IsKeyUp(OpenTK.Input.Key.Up))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Up;
	            	__isKeyUpDown = false;
	            }
				if (keyboard.IsKeyUp(OpenTK.Input.Key.Down))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Down;
	            	__isKeyDownDown = false;
	            }
	            if (keyboard.IsKeyUp(OpenTK.Input.Key.A))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Square;
	            	__isKeyADown = false;
	            }
	            if (keyboard.IsKeyUp(OpenTK.Input.Key.W))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Triangle;
	            	__isKeyWDown = false;
	            }
	            if (keyboard.IsKeyUp(OpenTK.Input.Key.D))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Circle;
	            	__isKeyDDown = false;
	            }
	            if (keyboard.IsKeyUp(OpenTK.Input.Key.S))
	            {
	            	gamePadData.ButtonsUp |= GamePadButtons.Cross;
	            	__isKeySDown = false;
	            }  
			}
			else
			{
				__isKeyLeftDown = false;
				__isKeyRightDown = false;
				__isKeyUpDown = false;
				__isKeyDownDown = false;
				__isKeyADown = false;
				__isKeyWDown = false;
				__isKeyDDown = false;
				__isKeySDown = false;
			}
            
            
            Touch.__data.Clear();
            if (__isWinFocused)
			{
	            OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();
	            if (mouse.IsButtonUp(OpenTK.Input.MouseButton.Left))
	            {
	            	__isMouseLeftDown = false;          	
	            }
	            //OpenTK.WindowState wState = MyGameWindow.getWindowState();
	            //wState != OpenTK.WindowState.Minimized
	            if (__isMouseLeftDown == false && mouse.IsButtonDown(OpenTK.Input.MouseButton.Left))
	            {
	            	__isMouseLeftDown = true;
	            	TouchData touchData = new TouchData();
	            	touchData.Status = TouchStatus.Down;
	            	touchData.X = mouse.X;
	            	touchData.Y = mouse.Y;
	            	Touch.__data.Add(touchData);
	            }
            }
            else
            {
            	__isMouseLeftDown = false;
            }
            
#if false
			double delta = __timer.Elapsed.TotalMilliseconds;
			double frame = 1000.0 / 24.0;
			if (delta < frame)
			{
				int free = (int)(frame - delta);
				Thread.Sleep(free);
				//Debug.WriteLine("Sleep: " + free);
			}
			__timer.Restart();
#endif
		}
	}
}
