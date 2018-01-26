# Sakura  
Sakura=OpenTK+PSSSDK  

## History  
* 2018-01-27: SocketSample done.  
* 2018-01-27: HttpSample done.  
* 2018-01-26: ImageSample done.  
* 2018-01-26: FontSample done.  
* 2018-01-26: TouchSample done.  
* 2018-01-26: MotionSample done.  
* 2018-01-26: GamePadSample done.  
* 2018-01-26: SoundPlayerSample done.  
* 2018-01-26: BgmPlayerSample done.  
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

* NAudio  
NAudio-NAudio\_1.7.2\_Release  
https://github.com/naudio/NAudio  
https://github.com/naudio/NAudio/releases/tag/NAudio_1.7.2_Release  

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
* Data files not found error.  
* BgmPlayerSample: if (Status == BgmStatus.Stopped || Status == BgmStatus.Paused) //FIXME: Can restart???   
* GamePadSample, ...: //SampleDraw.ClearSprite(); //FIXME:modified, for preventing memory overflow  
* GamePadSample: AnalogLeftX, AnalogLeftY, AnalogRightX, AnalogRightY not implemented  
* in Texture2D, public void SetPixels(...) { __supportNPOT = true; //NOTE:bug for repeat wrap mode //FIXME: for SampleDraw.DrawText()  
* in Font, * 0.70f), _style); //FIXME:???0.8  
* Render() memory leak  
* MotionSample: MotionData not set  
* Check OpenAL dll in Windows XP  
* SocketSample: in SampleButton, return; //FIXME: for textures overflow  

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
