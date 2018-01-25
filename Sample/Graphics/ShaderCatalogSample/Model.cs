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

class Model
{
    MeshData mesh;

    Matrix4 posture;
    Vector4 colorAmbient;
    Vector4 colorDiffuse;
    Vector4 colorSpecular;

    public Model( MeshData meshData )
    {
        this.mesh = meshData;
        posture = Matrix4.Translation( new Vector3( 0.0f, 0.0f, 0.0f ) );
        colorAmbient = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
        colorDiffuse = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
        colorSpecular = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
    }

    public void Dispose()
    {
    }

    public void RotateY( float rot )
    {
        posture = posture * Matrix4.RotationY( rot );
    }

    public void RotateX( float rot )
    {
        posture = posture * Matrix4.RotationX( rot );
    }

    public MeshData Mesh
    {
        get{ return mesh; }
    }
    public Vector3 Position
    {
        set{
            this.posture = Matrix4.Translation( value );
        }
        get{ return new Vector3( posture.M41, posture.M42, posture.M43 ); }
    }
    public Matrix4 Posture
    {
        get{ return posture; }
    }
    public Vector4 DiffuseColor
    {
        set{ this.colorDiffuse = value; }
        get{ return colorDiffuse; }
    }
    public Vector4 AmbientColor
    {
        set{ this.colorAmbient = value; }
        get{ return colorAmbient; }
    }
    public Vector4 SpecularColor
    {
        set{ this.colorSpecular = value; }
        get{ return colorSpecular; }
    }

}

} // end ns Sample
