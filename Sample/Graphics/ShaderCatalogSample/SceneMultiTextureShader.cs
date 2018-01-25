/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;

namespace Sample{
	
class SceneMultiTextureShader
    : IScene
{
    VertexBuffer vbTeapot;
	Texture2D[] textures = new Texture2D[ 2 ];
    ShaderProgram shaderMultiTexture;

    public SceneMultiTextureShader()
    {
    }

    /// Name used for display
    public string Name
    {
        get{ return @"< MultiTexture >"; }
    }

    /// Initialization
    public void Setup( GraphicsContext graphics, Model model )
    {
        MeshData meshTeapot = model.Mesh;

        // teapot
		vbTeapot = new VertexBuffer( meshTeapot.VertexCount,
                                     meshTeapot.IndexCount,
                                     VertexFormat.Float3,
                                     VertexFormat.Float2,
                                     VertexFormat.Float2 );
		vbTeapot.SetVertices( 0, meshTeapot.Positions );
        vbTeapot.SetVertices( 1, meshTeapot.TexCoords );
        vbTeapot.SetVertices( 2, meshTeapot.TexCoords );
		vbTeapot.SetIndices( meshTeapot.Indices );
			
        shaderMultiTexture = new ShaderProgram( "/Application/Sample/Graphics/ShaderCatalogSample/shaders/MultiTexture.cgx" );
        shaderMultiTexture.SetAttributeBinding( 0, "a_Position" );
        shaderMultiTexture.SetAttributeBinding( 1, "a_TexCoord0" );
        shaderMultiTexture.SetAttributeBinding( 2, "a_TexCoord1" );
        textures[ 0 ] = new Texture2D( "/Application/Sample/Graphics/ShaderCatalogSample/data/renga.png", false );
        textures[ 1 ] = new Texture2D( "/Application/Sample/Graphics/ShaderCatalogSample/data/multiple_texture.png", false );
        textures[ 0 ].SetWrap( TextureWrapMode.Repeat );
        textures[ 1 ].SetWrap( TextureWrapMode.Repeat );
    }

    /// Release unmanaged resources
    public void Dispose()
    {
        vbTeapot.Dispose();
        shaderMultiTexture.Dispose();
        textures[ 1 ].Dispose();
        textures[ 0 ].Dispose();
    }

    public void Update( float delta )
    {
    }

     public void Render( GraphicsContext graphics,
                         Camera camera,
                         LightModel light,
                         Model model,
                         BgModel bg
                         )
    {
		graphics.Clear() ;

        Matrix4 world = model.Posture;
        Matrix4 worldViewProj = camera.Projection * camera.View * world;

        shaderMultiTexture.SetUniformValue( shaderMultiTexture.FindUniform( "WorldViewProj" ), ref worldViewProj );
		graphics.SetTexture( 0, textures[ 0 ] );
		graphics.SetTexture( 1, textures[ 1 ] );

		graphics.SetShaderProgram( shaderMultiTexture );
		graphics.SetVertexBuffer( 0, vbTeapot );

        graphics.DrawArrays( model.Mesh.Prim, 0, model.Mesh.IndexCount );

        light.Render( graphics, camera );
        bg.Render( graphics, camera );
    }

}

} // end ns Sample
