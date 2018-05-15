/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */


using System;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Input;
using Sce.Pss.HighLevel.UI;

namespace HelloWorld
{
    public class AppMain
    {
        private static GraphicsContext graphics;
        
        public static void Main (string[] args)
        {
            Initialize ();

            while (true) {
                SystemEvents.CheckEvents ();
                Update ();
                Render ();
            }
        }

        public static void Initialize ()
        {
            // Set up the graphics system
            graphics = new GraphicsContext ();
            
            // Initialize UI Toolkit
            UISystem.Initialize (graphics);

            // Create scene
            Scene myScene = new Scene();
            Label label = new Label();
            label.X = 10.0f;
            label.Y = 50.0f;
            label.Text = "Hello World!";
            myScene.RootWidget.AddChildLast(label);
            // Set scene
            UISystem.SetScene(myScene, null);
        }

        public static void Update ()
        {
            // Query touch for current state
            List<TouchData> touchDataList = Touch.GetData (0);
            
            // Update UI Toolkit
            UISystem.Update(touchDataList);
        }

        public static void Render ()
        {
            // Clear the screen
            graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
            graphics.Clear ();
            
            // Render UI Toolkit
            UISystem.Render ();
            
            // Present the screen
            graphics.SwapBuffers ();
        }
    }
}
