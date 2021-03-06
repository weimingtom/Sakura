﻿/* SCE CONFIDENTIAL
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

using System.IO;
using System.Reflection;

namespace Sample
{

/**
 * SampleDraw class
 */
public static class SampleDraw
{
	private static ShaderProgram __testShaderProgram1;
	private static VertexBuffer __testVertices1;
	private static ShaderProgram __testShaderProgram2;
	private static VertexBuffer __testVertices2;
	
    private static GraphicsContext graphics;

    private static ShaderProgram textureShaderProgram;
    private static ShaderProgram colorShaderProgram;

    private static Matrix4 projectionMatrix;
    private static Matrix4 viewMatrix;

    private static VertexBuffer rectVertices;
    private static VertexBuffer circleVertices;

    private static Dictionary<string, SampleSprite> spriteDict;
    private static int spriteNamelessCount;

    private static Font defaultFont;
    private static Font currentFont;


    public static bool Init(GraphicsContext graphicsContext)
    {
        graphics = graphicsContext;

        textureShaderProgram = createSimpleTextureShader();
        colorShaderProgram = createSimpleColorShader();

        projectionMatrix = Matrix4.Ortho(0, Width,
                                         0, Height,
                                         0.0f, 32768.0f);

        viewMatrix = Matrix4.LookAt(new Vector3(0, Height, 0),
                                    new Vector3(0, Height, 1),
                                    new Vector3(0, -1, 0));

        rectVertices =  new VertexBuffer(4, VertexFormat.Float3);
        rectVertices.SetVertices(0, new float[]{0, 0, 0,
                                                1, 0, 0,
                                                1, 1, 0,
                                                0, 1, 0});

        circleVertices =  new VertexBuffer(36, VertexFormat.Float3);
        float[] circleVertex = new float[3 * 36];
        for (int i = 0; i < 36; i++) {
            float radian = ((i * 10) / 180.0f * FMath.PI);
            circleVertex[3 * i + 0] = FMath.Cos(radian);
            circleVertex[3 * i + 1] = FMath.Sin(radian);
            circleVertex[3 * i + 2] = 0.0f;
        }
        circleVertices.SetVertices(0, circleVertex);

        defaultFont = new Font(FontAlias.System, 24, FontStyle.Regular);
        SetDefaultFont();

        spriteDict = new Dictionary<string, SampleSprite>();
        spriteNamelessCount = 0;

#if true
        __testShaderProgram1 = new ShaderProgram("/Application/Sample/Lib/SampleLib/shaders/Test.cgx");
        __testVertices1 = new VertexBuffer(3, VertexFormat.Float3);
        __testShaderProgram1.SetAttributeBinding(0, "vPosition");
        __testVertices1.SetVertices(0, new float[]{
        	0.0f,  0.5f, 0.0f,
	        -0.5f, -0.5f, 0.0f,
	        0.5f, -0.5f, 0.0f 
        });

        __testShaderProgram2 = new ShaderProgram("/Application/Sample/Lib/SampleLib/shaders/Test.cgx");
        __testVertices2 = new VertexBuffer(3, VertexFormat.Float3);
        __testShaderProgram2.SetAttributeBinding(0, "vPosition");
        __testVertices2.SetVertices(0, new float[]{
        	0.0f,  0.5f - 0.5f, 0.0f,
	        -0.5f, -0.5f - 0.5f, 0.0f,
	        0.5f, -0.5f - 0.5f, 0.0f 
        });        
#endif
        
        return true;
    }


    /// Terminate
    public static void Term()
    {
        ClearSprite();

        defaultFont.Dispose();
        circleVertices.Dispose();
        rectVertices.Dispose();
        colorShaderProgram.Dispose();
        textureShaderProgram.Dispose();
        graphics = null;
    }

    public static int Width
    {
        get {return graphics.GetFrameBuffer().Width;}
    }

    public static int Height
    {
        get {return graphics.GetFrameBuffer().Height;}
    }

    /// Touch coordinates -> screen coordinates conversion : X
    public static int TouchPixelX(TouchData touchData)
    {
        return (int)((touchData.X + 0.5f) * Width);
    }

    /// Touch coordinates -> screen coordinates conversion : Y
    public static int TouchPixelY(TouchData touchData)
    {
        return (int)((touchData.Y + 0.5f) * Height);
    }

    public static void AddSprite(SampleSprite sprite)
    {
        AddSprite("[nameless]:" + spriteNamelessCount, sprite);
        spriteNamelessCount++;
    }

    public static void AddSprite(string key, SampleSprite sprite)
    {
        if (spriteDict.ContainsKey(key) == false) {
            spriteDict.Add(key, sprite);
        }
    }

    /// Register a sprite that draws text
    public static void AddSprite(string key, string text, uint argb, Font font, int positionX, int positionY)
    {
        if (spriteDict.ContainsKey(text) == false) {
    		//Debug.WriteLine("=======AddSprite======>" + text + ", spriteDict.Count == " + spriteDict.Count);
            AddSprite(key, new SampleSprite(text, argb, font, positionX, positionY));
        }
    }

    /// Register a sprite that draws text
    public static void AddSprite(string text, uint argb, int positionX, int positionY)
    {
        AddSprite(text, text, argb, currentFont, positionX, positionY);
    }

    public static void RemoveSprite(string key)
    {
        if (spriteDict.ContainsKey(key)) {
            spriteDict[key].Dispose();
            spriteDict.Remove(key);
        }
    }

    public static void ClearSprite()
    {
        foreach (string key in spriteDict.Keys) {
            spriteDict[key].Dispose();
        }

        spriteDict.Clear();
        spriteNamelessCount = 0;
    }

    /// Draw all registered sprites
    public static void DrawSprites()
    {
        foreach (string key in spriteDict.Keys) {
            DrawSprite(spriteDict[key]);
        }
    }

    public static void DrawSprite(string key)
    {
        if (spriteDict.ContainsKey(key)) {
            DrawSprite(spriteDict[key]);
        }
    }
    
    public static Texture2D __DrawSprite(string key, Matrix4 worldViewProj, bool notdraw)
    {
        if (spriteDict.ContainsKey(key)) {
            return __DrawSprite(spriteDict[key], worldViewProj, notdraw);
        }
    	return null;
    }

    public static void DrawSprite(SampleSprite sprite)
    {
#if true
    	var modelMatrix = sprite.CreateModelMatrix();
        var worldViewProj = projectionMatrix * viewMatrix * modelMatrix;

        textureShaderProgram.SetUniformValue(0, ref worldViewProj);

        graphics.SetShaderProgram(textureShaderProgram);
        graphics.SetVertexBuffer(0, sprite.Vertices);
        graphics.SetTexture(0, sprite.Texture);

        graphics.Enable(EnableMode.Blend);
        graphics.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);

        graphics.DrawArrays(DrawMode.TriangleFan, 0, 4);
#else

#endif
    }

    
    public static Texture2D __DrawSprite(SampleSprite sprite, Matrix4 worldViewProj, bool notdraw)
    {
    	if (!notdraw)
    	{
	        textureShaderProgram.SetUniformValue(0, ref worldViewProj);
	
	        graphics.SetShaderProgram(textureShaderProgram);
	        graphics.SetVertexBuffer(0, sprite.Vertices);
	        graphics.SetTexture(0, sprite.Texture);
	
	        graphics.Enable(EnableMode.Blend);
	        graphics.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
	
	        graphics.DrawArrays(DrawMode.TriangleFan, 0, 4);
    	}
    	
    	return sprite.Texture;
    }    
    
    public static void __DrawTest1()
    {
    	graphics.SetShaderProgram(__testShaderProgram1);
        graphics.SetVertexBuffer(0, __testVertices1);
        graphics.DrawArrays(DrawMode.TriangleStrip, 0, 3);
    }
    public static void __DrawTest2()
    {
    	graphics.SetShaderProgram(__testShaderProgram2);
        graphics.SetVertexBuffer(0, __testVertices2);
        graphics.DrawArrays(DrawMode.TriangleStrip, 0, 3);
    }
    public static void __setblend()
    {
    	graphics.__setblend();
    }
    
    public static Font CurrentFont
    {
        get {return currentFont;}
    }

    public static Font SetDefaultFont()
    {
        return SetFont(defaultFont);
    }

    public static Font SetFont(Font font)
    {
        Font previousFont = currentFont;
        currentFont = font;
        return previousFont;
    }

    public static void DrawText(string text, uint argb, Font font, int positionX, int positionY)
    {
        AddSprite(text, text, argb, font, positionX, positionY);
        DrawSprite(text);
    }

    public static void DrawText(string text, uint argb, int positionX, int positionY)
    {
        AddSprite(text, text, argb, currentFont, positionX, positionY);
        DrawSprite(text);
    }

    public static Texture2D __DrawText(string text, uint argb, int positionX, int positionY, Matrix4 worldViewProj, bool notdraw)
    {
        AddSprite(text, text, argb, currentFont, positionX, positionY);
        return __DrawSprite(text, worldViewProj, notdraw);
    }    
    
    public static void FillRect(uint argb, int positionX, int positionY, int rectW, int rectH)
    {
        FillVertices(rectVertices, argb, positionX, positionY, rectW, rectH);
    }

    public static void FillCircle(uint argb, int positionX, int positionY, int radius)
    {
        FillVertices(circleVertices, argb, positionX, positionY, radius, radius);
    }

    // 頂点バッファ塗りつぶし
    public static void FillVertices(VertexBuffer vertices, uint argb, float positionX, float positionY, float scaleX, float scaleY)
    {
        var transMatrix = Matrix4.Translation(new Vector3(positionX, positionY, 0.0f));
        var scaleMatrix = Matrix4.Scale(new Vector3(scaleX, scaleY, 1.0f));
        var modelMatrix = transMatrix * scaleMatrix;

        var worldViewProj = projectionMatrix * viewMatrix * modelMatrix;

        colorShaderProgram.SetUniformValue(0, ref worldViewProj);

        Vector4 color = new Vector4((float)((argb >> 16) & 0xff) / 0xff,
                                    (float)((argb >> 8) & 0xff) / 0xff,
                                    (float)((argb >> 0) & 0xff) / 0xff,
                                    (float)((argb >> 24) & 0xff) / 0xff);
        colorShaderProgram.SetUniformValue(colorShaderProgram.FindUniform("MaterialColor"), ref color);

        graphics.SetShaderProgram(colorShaderProgram);
        graphics.SetVertexBuffer(0, vertices);

        graphics.DrawArrays(DrawMode.TriangleFan, 0, vertices.VertexCount);
    }

    private static ShaderProgram createSimpleTextureShader()
    {
 #if false
		string ResourceName = "SampleLib.shaders.Texture.cgx";
			
		Assembly resourceAssembly = Assembly.GetExecutingAssembly();
		if (resourceAssembly.GetManifestResourceInfo(ResourceName) == null)
		{
			throw new FileNotFoundException("File not found.", ResourceName);
		}

        Stream fileStreamVertex = resourceAssembly.GetManifestResourceStream(ResourceName);
        Byte[] dataBufferVertex = new Byte[fileStreamVertex.Length];
        fileStreamVertex.Read(dataBufferVertex, 0, dataBufferVertex.Length);
			
		var shaderProgram = new ShaderProgram(dataBufferVertex);
#else
//		var shaderProgram = new ShaderProgram("/Application/shaders/Texture.cgx");
		var shaderProgram = new ShaderProgram("/Application/Sample/Lib/SampleLib/shaders/Texture.cgx");
#endif

		shaderProgram.SetAttributeBinding(0, "a_Position");
        shaderProgram.SetAttributeBinding(1, "a_TexCoord");

        shaderProgram.SetUniformBinding(0, "WorldViewProj");

        return shaderProgram;
    }

    private static ShaderProgram createSimpleColorShader()
    {
#if false
		string ResourceName = "SampleLib.shaders.Simple.cgx";
			
		Assembly resourceAssembly = Assembly.GetExecutingAssembly();
		if (resourceAssembly.GetManifestResourceInfo(ResourceName) == null)
		{
			throw new FileNotFoundException("File not found.", ResourceName);
		}

        Stream fileStreamVertex = resourceAssembly.GetManifestResourceStream(ResourceName);
        Byte[] dataBufferVertex = new Byte[fileStreamVertex.Length];
        fileStreamVertex.Read(dataBufferVertex, 0, dataBufferVertex.Length);
			
		var shaderProgram = new ShaderProgram(dataBufferVertex);
#else
		//var shaderProgram = new ShaderProgram("/Application/shaders/Simple.cgx");
		var shaderProgram = new ShaderProgram("/Application/Sample/Lib/SampleLib/shaders/Simple.cgx");
#endif

        shaderProgram.SetAttributeBinding(0, "a_Position");
        //shaderProgram.SetAttributeBinding(1, "a_Color0");

        shaderProgram.SetUniformBinding(0, "WorldViewProj");

        return shaderProgram;
    }
}

} // Sample
