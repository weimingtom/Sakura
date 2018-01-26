/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Input;
using Sce.Pss.Core.Audio;

namespace Sample
{

/**
 * BgmPlayerSample
 */
public static class BgmPlayerSample
{
    private static GraphicsContext graphics;

    private static SampleButton playButton;
    private static SampleButton stopButton;
    private static SampleButton pauseButton;
    private static SampleButton resumeButton;
    private static SampleSlider volumeSlider;
    private static Bgm bgm;
    private static BgmPlayer bgmPlayer;
    private static int volTextPosX;
    private static int volTextPosY;

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

        int sx = (graphics.Screen.Width / 2) - ((96 * 2) + (16 * 2));
        int sy = (graphics.Screen.Height / 2) - (96 / 2);

        playButton = new SampleButton(sx + 0, sy - 24, 96, 48);
        stopButton = new SampleButton(sx + 112, sy - 24, 96, 48);
        pauseButton = new SampleButton(sx + 224, sy - 24, 96, 48);
        resumeButton = new SampleButton(sx + 336, sy - 24, 96, 48);
        volumeSlider = new SampleSlider(sx + 88, sy + 96, 256, 48);

        volTextPosX = (sx + 88) - 96;
        volTextPosY = (sy + 96) + 12;

        playButton.SetText("Play");
        stopButton.SetText("Stop");
        pauseButton.SetText("Pause");
        resumeButton.SetText("Resume");

        bgm = new Bgm("/Application/Sample/Audio/BgmPlayerSample/GAME_BGM_01.mp3");
        bgmPlayer = bgm.CreatePlayer();

        return true;
    }

    /// Terminate
    public static void Term()
    {
        bgmPlayer.Stop();
        bgm.Dispose();

        playButton.Dispose();
        stopButton.Dispose();
        pauseButton.Dispose();
        resumeButton.Dispose();
        volumeSlider.Dispose();

        SampleDraw.Term();
        graphics.Dispose();
    }

    public static bool Update()
    {
        uint enableColor = 0xffffffff;
        uint disableColor = 0xff7f7f7f;

        playButton.ButtonColor = disableColor;
        stopButton.ButtonColor = disableColor;
        pauseButton.ButtonColor = disableColor;
        resumeButton.ButtonColor = disableColor;

        if (bgmPlayer.Status == BgmStatus.Stopped) {
            playButton.ButtonColor = enableColor;
        } else if (bgmPlayer.Status == BgmStatus.Playing) {
            stopButton.ButtonColor = enableColor;
            pauseButton.ButtonColor = enableColor;
        } else if (bgmPlayer.Status == BgmStatus.Paused) {
            stopButton.ButtonColor = enableColor;
            resumeButton.ButtonColor = enableColor;
        }

        List<TouchData> touchDataList = Touch.GetData(0);

        if (playButton.ButtonColor == enableColor &&
            playButton.TouchDown(touchDataList)) {
            bgmPlayer.Play();
        }
        if (stopButton.ButtonColor == enableColor &&
            stopButton.TouchDown(touchDataList)) {
            bgmPlayer.Stop();
        }
        if (pauseButton.ButtonColor == enableColor &&
            pauseButton.TouchDown(touchDataList)) {
            bgmPlayer.Pause();
        }
        if (resumeButton.ButtonColor == enableColor &&
            resumeButton.TouchDown(touchDataList)) {
            bgmPlayer.Resume();
        }

        volumeSlider.Update(touchDataList);
        bgmPlayer.Volume = volumeSlider.Rate;

        return true;
    }

    public static bool Render()
    {
        graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        graphics.Clear();

        SampleDraw.DrawText("Status : " + bgmPlayer.Status, 0xffffffff, 0, 64);
        SampleDraw.DrawText("Volume", 0xffffffff, volTextPosX, volTextPosY);

        playButton.Draw();
        stopButton.Draw();
        pauseButton.Draw();
        resumeButton.Draw();
        volumeSlider.Draw();

        SampleDraw.DrawText("BgmPlayer Sample", 0xffffffff, 0, 0);
        graphics.SwapBuffers();

        return true;
    }
}

} // Sample
