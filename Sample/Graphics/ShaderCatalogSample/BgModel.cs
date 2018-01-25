/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;

namespace Sample
{

class BgModel
{
    Matrix4 posture;

    // mesh
    VertexBuffer vbGrid;
    MeshData meshGrid;
    ShaderProgram shaderVtxColor;

    Texture2D texture;

    public BgModel( float width, float depth, int divW, int divH )
    {
        // grid
#if false
        meshGrid = BasicMeshFactory.CreateGrid( width, depth,
                                                divW, divH,
                                                new Rgba( 128, 128, 128, 255),
                                                new Rgba( 255, 0, 0, 255),
                                                new Rgba( 0, 255, 0, 255) );
#else
        meshGrid = BasicMeshFactory.CreatePlane( width, depth,
                                                 divW, divH, 0.25f, 0.25f );

#endif

        vbGrid = new VertexBuffer( meshGrid.VertexCount,
                                   meshGrid.IndexCount,
                                   VertexFormat.Float3,
                                   VertexFormat.Float2 );
		vbGrid.SetVertices( 0, meshGrid.Positions );
		vbGrid.SetVertices( 1, meshGrid.TexCoords );

		vbGrid.SetIndices( meshGrid.Indices ) ;


        // vertex color shader
        shaderVtxColor = new ShaderProgram( "/Application/Sample/Graphics/ShaderCatalogSample/shaders/Texture.cgx" );
		shaderVtxColor.SetAttributeBinding( 0, "a_Position" );
        shaderVtxColor.SetAttributeBinding( 1, "a_TexCoord" );
        texture = new Texture2D( "/Application/Sample/Graphics/ShaderCatalogSample/data/renga.png", false );
        texture.SetWrap( TextureWrapMode.Repeat );

        posture = Matrix4.Translation( new Vector3( 0.0f, 0.0f, 0.0f ) );
    }

    public void Dispose()
    {
        vbGrid.Dispose();
        meshGrid.Dispose();
        shaderVtxColor.Dispose();
        texture.Dispose();
    }

    public void Render( GraphicsContext graphics, Camera camera )
    {
        Matrix4 worldViewProj = camera.Projection * camera.View * posture;

        // uniform value
        shaderVtxColor.SetUniformValue( shaderVtxColor.FindUniform( "WorldViewProj" ), ref worldViewProj );
        graphics.SetShaderProgram( shaderVtxColor );
		graphics.SetTexture( 0, texture );
        graphics.SetVertexBuffer( 0, vbGrid );
        graphics.DrawArrays( meshGrid.Prim, 0, meshGrid.IndexCount );
    }
}

} // end ns Sample
