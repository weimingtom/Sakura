/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;

namespace Sample{

class SceneTextureShader
    : IScene
{
    VertexBuffer vbTeapot;
    ShaderProgram shaderTexture;
    Texture2D texture;

    public SceneTextureShader()
    {
    }

    /// Name used for display
    public string Name
    {
        get{ return @"< Texture >"; }
    }

    /// Initialization
    public void Setup( GraphicsContext graphics, Model model )
    {
        MeshData meshTeapot = model.Mesh;

		vbTeapot = new VertexBuffer( meshTeapot.VertexCount,
                                     meshTeapot.IndexCount,
                                     VertexFormat.Float3,
                                     VertexFormat.Float2 );
		vbTeapot.SetVertices( 0, meshTeapot.Positions );
        vbTeapot.SetVertices( 1, meshTeapot.TexCoords );
		vbTeapot.SetIndices( meshTeapot.Indices );

        // texture shader
        shaderTexture = new ShaderProgram( "/Application/Sample/Graphics/ShaderCatalogSample/shaders/Texture.cgx" );
		shaderTexture.SetAttributeBinding( 0, "a_Position" );
		shaderTexture.SetAttributeBinding( 1, "a_TexCoord" );

        texture = new Texture2D( "/Application/Sample/Graphics/ShaderCatalogSample/data/renga.png", false );
    }

    /// Release unmanaged resources
    public void Dispose()
    {
        vbTeapot.Dispose();
        shaderTexture.Dispose();
        texture.Dispose();
    }

    public void Update( float delta )
    {
    }

    public void Render( 
                       GraphicsContext graphics,
                       Camera camera,
                       LightModel light,
                       Model model,
                       BgModel bg
                        )
    {
		graphics.Clear() ;

        drawModel( graphics, camera, model );
        light.Render( graphics, camera );
        bg.Render( graphics, camera );

    }

    void drawModel( 
                   GraphicsContext graphics,
                   Camera camera,
                   Model model
                    )
    {
 		Matrix4 world = model.Posture;
        Matrix4 worldViewProj = camera.Projection * camera.View * world;

        shaderTexture.SetUniformValue( shaderTexture.FindUniform( "WorldViewProj" ), ref worldViewProj );
		graphics.SetShaderProgram( shaderTexture );
		graphics.SetVertexBuffer( 0, vbTeapot );

		graphics.SetTexture( 0, texture );
        graphics.DrawArrays( model.Mesh.Prim, 0, model.Mesh.IndexCount );
    }
}

} // end ns Sample
