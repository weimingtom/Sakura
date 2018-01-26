/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Threading;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Input;

namespace Sample
{

/**
 * MotionSample
 */
public static class MotionSample
{
    private static GraphicsContext graphics;
		
	static bool loop = true;
		
    public static void Main(string[] args)
    {
        Init();

        while (loop) {
            SystemEvents.CheckEvents();
            Update();
            Render();
        }

        Term();
    }

    public static bool Init()
    {
        graphics = new GraphicsContext();
        SampleDraw.Init(graphics);

        return true;
    }

    /// Terminate
    public static void Term()
    {
        SampleDraw.Term();
        graphics.Dispose();
    }

    public static bool Update()
    {
        return true;
    }

    public static bool Render()
    {
        graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        graphics.Clear();

        //SampleDraw.ClearSprite(); //FIXME:modified, for preventing memory overflow

        var motionData = Motion.GetData(0);
        Vector3 acc = motionData.Acceleration;
        Vector3 vel = motionData.AngularVelocity;

        int fontHeight = SampleDraw.CurrentFont.Metrics.Height;
        int positionX = 256;
        int positionY = fontHeight * 3;

        SampleDraw.DrawText("Acceleration X : " + acc.X, 0xffffffff, positionX, positionY);
        positionY += fontHeight;
        SampleDraw.DrawText("Acceleration Y : " + acc.Y, 0xffffffff, positionX, positionY);
        positionY += fontHeight;
        SampleDraw.DrawText("Acceleration Z : " + acc.Z, 0xffffffff, positionX, positionY);

        positionY += fontHeight * 2;

        SampleDraw.DrawText("AngularVelocity X : " + vel.X, 0xffffffff, positionX, positionY);
        positionY += fontHeight;
        SampleDraw.DrawText("AngularVelocity Y : " + vel.Y, 0xffffffff, positionX, positionY);
        positionY += fontHeight;
        SampleDraw.DrawText("AngularVelocity Z : " + vel.Z, 0xffffffff, positionX, positionY);

        SampleDraw.DrawText("Motion Sample", 0xffffffff, 0, 0);
        graphics.SwapBuffers();

        return true;
    }
}

} // Sample
