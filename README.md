# Sakura  
Sakura=OpenTK+PSSSDK  

## History  
* 2018-01-25: ShaderCatalogSample done.  
* 2018-01-21: PixelBufferSample done.  
* 2018-01-17: MathSample done.  
* 2018-01-17: PrimitiveSample done.  
* 2018-01-16: SpriteSample done.  
* 2018-01-15: TriangleSample done.  

## References    
* PSSSDK  
PlayStation Suite SDK ver 0.98  
PSSuiteSDK_098.exe  

* OpenTK  
opentk-2014-07-23  
https://github.com/opentk/opentk  

* ANGLE  
https://github.com/CoherentLabs/angle  
https://gitee.com/weimingtom/angle    

* Work  
https://gitee.com/weimingtom/TriangleSample  

* cgc  
http://developer.download.nvidia.com/cg/cgc.html  

## Bug / TODO  
* SpriteSample: The sprite is not center, larger than original size  
* SpriteSample: NPOT, see Texture2D.SetPixels, see __supportNPOT  
* SampleLib : Font size  
* search FIXME:not used  
* ShaderCatalogSample : in Texture2D, if (fileName.Contains("renga.png")), 256 is changed to 250  
* ShaderCatalogSample : in GraphicsContext, global textureDic is clear in SwapBuffers(), but global vertexBuffer is clear in SetShaderProgram()  
* SpriteSample: GL.Clear(ClearBufferMask.DepthBufferBit);  //FIXME:??????in SpriteSample, for sprite black bg problem  
* SpriteSample: __supportNPOT can be set true if REPEAT wrap mode not used  

## cgc  
cgc.exe "BlinnPhong.fcg" -profile glslf -o "BlinnPhong.glslf"  
cgc.exe "BlinnPhong.vcg" -profile glslv -o "BlinnPhong.glslv"  
cgc input_f.txt -profile glslf -o a.txt  
python cg_glsl_to_es20_glsl.py a.txt > output_f.txt  
cgc input_v.txt -profile glslv -o a.txt  
python cg_glsl_to_es20_glsl.py a.txt > output_v.txt  

## cg to glsl mod 
//gl_TexCoord[0].xy = a_TexCoord.xy; -> removed  
vec2 _v_TexCoord1; -> varying vec2 _v_TexCoord1;  
uniform sampler2D xxx:TEXUNIT0 -> Texture0  
uniform sampler2D xxx:TEXUNIT0 -> Texture1  
