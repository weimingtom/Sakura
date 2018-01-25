/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Environment; // FIXME : for FMath

namespace Sample{

class SceneGaussianFilter
    : IScene
{
    int offWidth = ShaderCatalog.DspWidth;// / 2;
    int offHeight = ShaderCatalog.DspHeight;// / 2;

    VertexBuffer vbTeapot;
    ShaderProgram shaderTexture;
    Texture2D texture;

    // filter
    ShaderProgram shaderGaussianX;
    ShaderProgram shaderGaussianY;

    FrameBuffer sceneBuffer;
    Texture2D texScene;

    FrameBuffer gaussianXBuffer;
    Texture2D texGaussianX;

    FrameBuffer gaussianXYBuffer;
    Texture2D texGaussianXY;

    // test
    TextureRenderer texRenderer;

    public SceneGaussianFilter()
    {
    }

    /// Name used for display
    public string Name
    {
        get{ return @"Gaussian Filter"; }
    }

    public void Setup( GraphicsContext graphics, Model model )
    {
        createVertexBuffer( model.Mesh );
        createOffscreenBuffer();

        texRenderer = new TextureRenderer();
        texRenderer.BindGraphicsContext( graphics );
    }

    private void createVertexBuffer( MeshData meshTeapot )
    {
		vbTeapot = new VertexBuffer( meshTeapot.VertexCount,
                                     meshTeapot.IndexCount,
                                     VertexFormat.Float3,
                                     VertexFormat.Float2 );
		vbTeapot.SetVertices( 0, meshTeapot.Positions );
        vbTeapot.SetVertices( 1, meshTeapot.TexCoords );
		vbTeapot.SetIndices( meshTeapot.Indices );

        // texture shader
        shaderTexture = new ShaderProgram( "Texture.cgx" );
		shaderTexture.SetAttributeBinding( 0, "a_Position" );
		shaderTexture.SetAttributeBinding( 1, "a_TexCoord" );

        // gaussian x
        shaderGaussianX = new ShaderProgram( "/Application/Sample/Graphics/ShaderCatalogSample/shaders/GaussianX.cgx" );
		shaderGaussianX.SetAttributeBinding( 0, "a_Position" );
		shaderGaussianX.SetAttributeBinding( 1, "a_TexCoord" );

        // gaussian y
        shaderGaussianY = new ShaderProgram( "/Application/Sample/Graphics/ShaderCatalogSample/shaders/GaussianY.cgx" );
		shaderGaussianY.SetAttributeBinding( 0, "a_Position" );
		shaderGaussianY.SetAttributeBinding( 1, "a_TexCoord" );

        texture = new Texture2D( "earthmap1k.png", false );
        texture.SetWrap( TextureWrapMode.Repeat );
    }

    private void createOffscreenBuffer()
    {
        // scene
        sceneBuffer = new FrameBuffer();
        texScene = new Texture2D( offWidth, offHeight, false, PixelFormat.Rgba );
        sceneBuffer.SetColorTarget( texScene, 0 );
        sceneBuffer.SetDepthTarget( new DepthBuffer( offWidth, offHeight, PixelFormat.Depth16 ) );

        // gaussian x
        gaussianXBuffer = new FrameBuffer();
        texGaussianX = new Texture2D( offWidth, offHeight, false, PixelFormat.Rgba );
        gaussianXBuffer.SetColorTarget( texGaussianX, 0 );
//        gaussianXBuffer.SetDepthTarget( null ); // not use DepthBuffer

        // gaussian xy
        gaussianXYBuffer = new FrameBuffer();
        texGaussianXY = new Texture2D( offWidth, offHeight, false, PixelFormat.Rgba );
        gaussianXYBuffer.SetColorTarget( texGaussianXY, 0 );
//        gaussianXYBuffer.SetDepthTarget( null ); // not use DepthBuffer
    }


    public void Dispose()
    {
        vbTeapot.Dispose();
        shaderTexture.Dispose();
        shaderGaussianX.Dispose();
        shaderGaussianY.Dispose();

        gaussianXYBuffer.Dispose();
        gaussianXBuffer.Dispose();
        sceneBuffer.Dispose();
    }

    bool flg = true;
    float w;
    public void Update( float delta )
    {
        if( flg ){
            w += 1.0f * delta;
        }else{
            w -= 1.0f * delta;
        }

        updateGaussianWeight( w * w );

        if( flg ){
            if( w >= 10.0f ){
                flg = !flg;
            }
        }else{
            if( w <= 0.1f ){
                flg = !flg;
            }
        }
        
    }

    public void Render( 
                       GraphicsContext graphics,
                       Camera camera,
                       LightModel light,
                       Model model,
                       BgModel bg
                        )
    {

        // offscreen rendering
        FrameBuffer oldBuffer = graphics.GetFrameBuffer();

        // render the scene
        graphics.SetFrameBuffer( sceneBuffer );
		graphics.SetViewport(0, 0, sceneBuffer.Width, sceneBuffer.Height );
        graphics.SetClearColor( 0.0f, 0.5f, 1.0f, 1.0f ) ;

        renderSimpleScene( graphics, camera, light, model, bg);

        // apply gaussian along X axis 
        graphics.SetFrameBuffer( gaussianXBuffer );
		graphics.SetViewport(0, 0, gaussianXBuffer.Width, gaussianXBuffer.Height );

        renderGaussianX( graphics );

        // apply gaussian along Y axis 
        graphics.SetFrameBuffer( gaussianXYBuffer );
		graphics.SetViewport(0, 0, gaussianXYBuffer.Width, gaussianXYBuffer.Height );

        renderGaussianY( graphics );

        // final draw
        {       
            // restore frame buffer
            graphics.SetFrameBuffer( oldBuffer );
            graphics.SetViewport( 0, 0, oldBuffer.Width, oldBuffer.Height );
            graphics.SetClearColor( 0.0f, 0.5f, 1.0f, 1.0f ) ;

            graphics.Clear();

            int width = sceneBuffer.Width / 4;
            int height = sceneBuffer.Height / 4;

            texRenderer.Begin();
            texRenderer.Render( texGaussianXY,   0, 0, 0, 0, sceneBuffer.Width, sceneBuffer.Height );

            texRenderer.Render( texScene,   0, height * 0, 0, 0, width, height );
            texRenderer.Render( texGaussianX,  0, height * 1, 0, 0, width, height );
            texRenderer.Render( texGaussianXY, 0, height * 2, 0, 0, width, height );
            texRenderer.End();
        }
    }

    private void renderSimpleScene(
                                   GraphicsContext graphics,
                                   Camera camera,
                                   LightModel light,
                                   Model model,
                                   BgModel bg
                                   )
    {

        // rendering
		graphics.Clear() ;

 		Matrix4 world = model.Posture;
        Matrix4 worldViewProj = camera.Projection * camera.View * world;

        shaderTexture.SetUniformValue( shaderTexture.FindUniform( "WorldViewProj" ), ref worldViewProj );
		graphics.SetShaderProgram( shaderTexture );
		graphics.SetVertexBuffer( 0, vbTeapot );

		graphics.SetTexture( 0, texture );
        graphics.DrawArrays( model.Mesh.Prim, 0, model.Mesh.IndexCount );

        light.Render( graphics, camera );
        bg.Render( graphics, camera );

    }

    float[] gaussianWeightTable = new float[ 8 ]; ///< 8 * 2 = 16 samples table

    void updateGaussianWeight( 
                              float i_dispersion 
                               )
    {
        float a_total = 0.0f;

        for( int i = 0; i < gaussianWeightTable.Length; i++ ){
            float a_pos = 1.0f + 2.0f * (float)i;
            gaussianWeightTable[ i ] = (float)FMath.Exp( -0.5f * (float)(a_pos * a_pos) / i_dispersion );
            a_total += 2.0f * gaussianWeightTable[ i ];
        }

        // normalize
        for( int i = 0; i < gaussianWeightTable.Length; i++ ){
            gaussianWeightTable[ i ] /= a_total;
        }
    }

    private void renderGaussianX( GraphicsContext graphics )
    {
        shaderGaussianX.SetUniformValue( shaderGaussianX.FindUniform( "ReciprocalTexWidth" ), 1.0f / texScene.Width );
        shaderGaussianX.SetUniformValue( shaderGaussianX.FindUniform( "Weight" ), 0, gaussianWeightTable );
        Vector2 offset = new Vector2( 16.0f / texScene.Width, 0.0f );
        shaderGaussianX.SetUniformValue( shaderGaussianX.FindUniform( "Offset" ), ref offset );

        texRenderer.Begin( shaderGaussianX );
        texRenderer.Render( texScene );
        texRenderer.End();

//        graphics.SetClearColor( 1.0f, 0.0f, 0.0f, 1.0f ) ;
//        graphics.Clear();
    }

    private void renderGaussianY( GraphicsContext graphics )
    {
        shaderGaussianY.SetUniformValue( shaderGaussianY.FindUniform( "ReciprocalTexWidth" ), 1.0f / texScene.Width );
        shaderGaussianY.SetUniformValue( shaderGaussianY.FindUniform( "Weight" ), 0, gaussianWeightTable );
        Vector2 offset = new Vector2( 0.0f, 16.0f / texScene.Width );
        shaderGaussianY.SetUniformValue( shaderGaussianY.FindUniform( "Offset" ), ref offset );

        texRenderer.Begin( shaderGaussianY );
        texRenderer.Render( texGaussianX );
        texRenderer.End();
    }

}

} // end ns Sample
