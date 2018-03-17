/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */

//#define EASY_SETUP
//#define EXTERNAL_INPUT

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Input;
using Sce.Pss.Core.Environment;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

static class HelloSprite
{
	static void Main( string[] args )
	{
		Log.SetToConsole();

		#if EASY_SETUP

		// initialize GameEngine2D's singletons
		Director.Initialize();

		#else // #if EASY_SETUP

		// create our own context
		Sce.Pss.Core.Graphics.GraphicsContext context = new Sce.Pss.Core.Graphics.GraphicsContext();

		// maximum number of sprites you intend to use (not including particles)
		uint sprites_capacity = 500;

		// maximum number of vertices that can be used in debug draws
		uint draw_helpers_capacity = 400;

		// initialize GameEngine2D's singletons, passing context from outside
		Director.Initialize( sprites_capacity, draw_helpers_capacity, context );

		#endif // #if EASY_SETUP 

		Director.Instance.GL.Context.SetClearColor( Colors.Grey20 );

		// set debug flags that display rulers to debug coordinates
//		Director.Instance.DebugFlags |= DebugFlags.DrawGrid;
		// set the camera navigation debug flag (press left alt + mouse to navigate in 2d space)
		Director.Instance.DebugFlags |= DebugFlags.Navigate; 

		// create a new scene
		var scene = new Scene();

		// set the camera so that the part of the word we see on screen matches in screen coordinates
		scene.Camera.SetViewFromViewport();

		// create a new TextureInfo object, used by sprite primitives
		var texture_info = new TextureInfo( new Texture2D("/Application/Sample/GameEngine2D/HelloSprite/king_water_drop.png", false ) );

		// create a new sprite
		var sprite = new SpriteUV() { TextureInfo = texture_info};

		// make the texture 1:1 on screen
		sprite.Quad.S = texture_info.TextureSizef;
 		
		// center the sprite around its own .Position 
		// (by default .Position is the lower left bit of the sprite)
		sprite.CenterSprite();

		// put the sprite at the center of the screen
		sprite.Position = scene.Camera.CalcBounds().Center;
		//sprite.Scale = Vector2.One * 0.9f;
		// our scene only has 2 nodes: scene->sprite
		scene.AddChild( sprite );

		#if EASY_SETUP

		Director.Instance.RunWithScene( scene );

		#else // #if EASY_SETUP

		// handle the loop ourself

		Director.Instance.RunWithScene( scene, true );

		while ( !Input2.GamePad0.Cross.Press )
		{
			Sce.Pss.Core.Environment.SystemEvents.CheckEvents();

			#if EXTERNAL_INPUT

			// it is not needed but you can set external input data if you want

			List<TouchData> touch_data_list = Touch.GetData(0);
			Input2.Touch.SetData( 0, touch_data_list );

			GamePadData pad_data = GamePad.GetData(0);
			Input2.GamePad.SetData( 0, pad_data );

			#endif // #if EXTERNAL_INPUT
			
			Director.Instance.Update();
//			Debug.WriteLine("===============>" + Director.Instance.SpriteRenderer);
			Director.Instance.Render();

			Director.Instance.GL.Context.SwapBuffers();
			Director.Instance.PostSwap(); // you must call this after SwapBuffers

//			System.Console.WriteLine( "Director.Instance.DebugStats.DrawArraysCount " + Director.Instance.GL.DebugStats.DrawArraysCount );
		}

		#endif // #if EASY_SETUP

		Director.Terminate();

		System.Console.WriteLine( "Bye!" );
	}
}

