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

interface IScene
{
    string Name
    {
        get;
    }
    void Setup( GraphicsContext graphics, Model model );
    void Dispose();
    void Update( float delta );
    void Render( GraphicsContext graphics, Camera camera, LightModel light, Model model, BgModel bg );
}


}
