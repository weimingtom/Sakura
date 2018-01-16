/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Input;

namespace Sample
{

/**
 * SpriteSample
 */
class SpriteSample
{
    static GraphicsContext graphics;
    static Texture2D ballTexture;
    static Queue<Ball> balls;
    static float ballScale;
    static bool enableBallRotate;

    static bool loop = true;

    static void Main(string[] args)
    {
        Init();
        while (loop) {
            SystemEvents.CheckEvents();
            Update();
            Render();
        }
        Term();
    }

    static bool Init()
    {
        graphics = new GraphicsContext();

        SampleDraw.Init(graphics);

        ballTexture = new Texture2D("/Application/Sample/Graphics/SpriteSample/test.png", false);
        ballTexture.SetWrap(TextureWrapMode.ClampToEdge);
        ballScale = 1.0f;
        enableBallRotate = true;

        balls = new Queue<Ball>();
        balls.Enqueue(new Ball());
        return true;
    }

    static void Term()
    {
        SampleDraw.Term();
        foreach (var ball in balls) {
            ball.Dispose();
        }
        balls.Clear();
        ballTexture.Dispose();
        graphics.Dispose();
    }

    static bool Update()
    {
        var gamePadData = GamePad.GetData(0);

        if ((gamePadData.ButtonsDown & GamePadButtons.Circle) != 0) {
            enableBallRotate = (enableBallRotate) ? false : true;
        }

        if ((gamePadData.Buttons & GamePadButtons.Up) != 0) {
            ballScale += 0.1f;
        } else if ((gamePadData.Buttons & GamePadButtons.Down) != 0) {
            ballScale -= 0.1f;
        }

        float maxScale = (float)SampleDraw.Height / ballTexture.Height;

        if (ballScale < 0.1f) {
            ballScale = 0.1f;
        } else if (ballScale > maxScale) {
            ballScale = maxScale;
        }

        if ((gamePadData.ButtonsDown & GamePadButtons.Left) != 0) {
            if (balls.Count > 0) {
                balls.Dequeue();
            }
        } else if ((gamePadData.ButtonsDown & GamePadButtons.Right) != 0) {
            balls.Enqueue(new Ball());
        }

        foreach (var ball in balls) {
            ball.SetScale(ballScale);
            ball.Update();
        }

        return true;
    }

    static bool Render()
    {
        graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        graphics.Clear();

        foreach (var ball in balls) {
            ball.Render();
        }


        SampleDraw.DrawText("Sprite Sample", 0xffffffff, 0, 0);

        graphics.SwapBuffers();
        return true;
    }

    // ball class

    class Ball : IDisposable
    {
        SampleSprite sprite;
        int dirX;
        int dirY;
        float addPosition;
        float addRotation;

        public Ball()
        {
            Random rand = new System.Random();

            var positionX = rand.Next(0, SampleDraw.Width - ballTexture.Width);
            var positionY = rand.Next(0, SampleDraw.Height - ballTexture.Height);
            sprite = new SampleSprite(ballTexture, positionX, positionY, 0.0f, 1.0f);

            dirX = rand.Next(2) > 0 ? 1 : -1;
            dirY = rand.Next(2) > 0 ? 1 : -1;

            addPosition = 0.5f + (float)rand.NextDouble() * 5.0f;
            addRotation = 0.1f + (float)rand.NextDouble() * 5.0f;
        }

        public void SetScale(float scale)
        {
            sprite.ScaleX = scale;
            sprite.ScaleY = scale;
        }

        public void Dispose()
        {
            sprite.Dispose();
        }

        public void Update()
        {
            sprite.PositionX += addPosition * dirX;
            sprite.PositionY += addPosition * dirY;

            float normalRad = ballTexture.Width / 2;
            float scaleRad = ballTexture.Width * ballScale / 2;

            float centerX = sprite.PositionX + normalRad;
            float centerY = sprite.PositionY + normalRad;

            if (centerX < scaleRad) {
                centerX = scaleRad;
                dirX = 1;
            } else if (centerX > SampleDraw.Width - scaleRad) {
                centerX = SampleDraw.Width - scaleRad;
                dirX = -1;
            }

            if (centerY < scaleRad) {
                centerY = scaleRad;
                dirY = 1;
            } else if (centerY > SampleDraw.Height - scaleRad) {
                centerY = SampleDraw.Height - scaleRad;
                dirY = -1;
            }

            sprite.PositionX = centerX - normalRad;
            sprite.PositionY = centerY - normalRad;

            if (enableBallRotate) {
                sprite.Degree += addRotation;
                if (sprite.Degree >= 360) {
                    sprite.Degree -= 360;
                }
            }
        }

        public void Render()
        {
            SampleDraw.DrawSprite(sprite);
        }
    }
}

} // Sample
