/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Threading;
using System.Diagnostics;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Environment;

namespace Sample
{

/**
 * TriangleSample
 */
class TriangleSample
{
    static GraphicsContext graphics;
    static Stopwatch stopwatch;
    static ShaderProgram program;
    static VertexBuffer vertices;

    static bool loop = true;

    [STAThread]
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
        stopwatch = new Stopwatch();
        stopwatch.Start();

        SampleDraw.Init(graphics);

        program = new ShaderProgram("/Application/Sample/Graphics/TriangleSample/shaders/VertexColor.cgx");
        vertices = new VertexBuffer(3, VertexFormat.Float3, VertexFormat.Float4);

        program.SetUniformBinding(0, "WorldViewProj");
        program.SetAttributeBinding(0, "a_Position");
        program.SetAttributeBinding(1, "a_Color0");

        float[] positions = {
            0.0f, 1.0f, 0.0f,
            -0.5f, 0.0f, 0.0f,
            0.5f, 0.0f, 0.0f,
        };

        float[] colors = {
            0.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f,
        };
        vertices.SetVertices(0, positions);
        vertices.SetVertices(1, colors);
        return true;
    }

    static void Term()
    {
        SampleDraw.Term();
        program.Dispose();
        vertices.Dispose();
        graphics.Dispose();
    }

    static bool Update()
    {
        return true;
    }

    static bool Render()
    {
        float seconds = (float)stopwatch.ElapsedMilliseconds / 1000.0f;
        float aspect = graphics.Screen.AspectRatio;
        float fov = FMath.Radians(45.0f);

        Matrix4 proj = Matrix4.Perspective(fov, aspect, 1.0f, 1000000.0f);
        Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.5f, 3.0f),
                                    new Vector3(0.0f, 0.5f, 0.0f),
                                    Vector3.UnitY);
        Matrix4 world = Matrix4.RotationY(1.0f * seconds);

        Matrix4 worldViewProj = proj * view * world;
        program.SetUniformValue(0, ref worldViewProj);


        graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
        graphics.SetClearColor(0.0f, 0.5f, 1.0f, 0.0f);
        graphics.Clear();

        graphics.SetShaderProgram(program);
        graphics.SetVertexBuffer(0, vertices);
        graphics.DrawArrays(DrawMode.TriangleStrip, 0, 3);


        SampleDraw.DrawText("Triangle Sample", 0xffffffff, 0, 0);

        graphics.SwapBuffers();
        return true;
    }
}

} // end ns Sample
