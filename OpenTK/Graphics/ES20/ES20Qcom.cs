﻿//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2013 Stefanos Apostolopoulos for the Open Toolkit Library
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//

namespace OpenTK.Graphics.ES20
{
    using System;
    using System.Text;
    using System.Runtime.InteropServices;
    #pragma warning disable 3019
    #pragma warning disable 1591
    #pragma warning disable 1572
    #pragma warning disable 1573
    #pragma warning disable 626

    partial class GL
    { 
        public static partial class Qcom
        {
            /// <summary>[requires: QCOM_alpha_test]
            /// Specify the alpha test function
            /// </summary>
            /// <param name="func"> 
            /// Specifies the alpha comparison function. Symbolic constants Never, Less, Equal, Lequal, Greater, Notequal, Gequal, and Always are accepted. The initial value is Always.
            /// </param>
            /// <param name="@ref"> 
            /// Specifies the reference value that incoming alpha values are compared to. This value is clamped to the range [0,1], where 0 represents the lowest possible alpha value and 1 the highest possible value. The initial reference value is 0.
            /// </param>
            [AutoGenerated(Category = "QCOM_alpha_test", Version = "", EntryPoint = "glAlphaFuncQCOM")]
            public static void AlphaFunc(OpenTK.Graphics.ES20.All func, Single @ref) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glDisableDriverControlQCOM")]
            [CLSCompliant(false)]
            public static void DisableDriverControl(Int32 driverControl) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glDisableDriverControlQCOM")]
            [CLSCompliant(false)]
            public static void DisableDriverControl(UInt32 driverControl) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glEnableDriverControlQCOM")]
            [CLSCompliant(false)]
            public static void EnableDriverControl(Int32 driverControl) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glEnableDriverControlQCOM")]
            [CLSCompliant(false)]
            public static void EnableDriverControl(UInt32 driverControl) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_tiled_rendering]</summary>
            /// <param name="preserveMask"></param>
            [AutoGenerated(Category = "QCOM_tiled_rendering", Version = "", EntryPoint = "glEndTilingQCOM")]
            [CLSCompliant(false)]
            public static void EndTiling(Int32 preserveMask) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_tiled_rendering]</summary>
            /// <param name="preserveMask"></param>
            [AutoGenerated(Category = "QCOM_tiled_rendering", Version = "", EntryPoint = "glEndTilingQCOM")]
            [CLSCompliant(false)]
            public static void EndTiling(UInt32 preserveMask) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBufferPointervQCOM")]
            public static void ExtGetBufferPointer(OpenTK.Graphics.ES20.All target, [OutAttribute] IntPtr @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBufferPointervQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetBufferPointer<T1>(OpenTK.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[] @params)
                where T1 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBufferPointervQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetBufferPointer<T1>(OpenTK.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[,] @params)
                where T1 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBufferPointervQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetBufferPointer<T1>(OpenTK.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[,,] @params)
                where T1 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBufferPointervQCOM")]
            public static void ExtGetBufferPointer<T1>(OpenTK.Graphics.ES20.All target, [InAttribute, OutAttribute] ref T1 @params)
                where T1 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="buffers">[length: maxBuffers]</param>
            /// <param name="maxBuffers"></param>
            /// <param name="numBuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetBuffers([OutAttribute] Int32[] buffers, Int32 maxBuffers, [OutAttribute] out Int32 numBuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="buffers">[length: maxBuffers]</param>
            /// <param name="maxBuffers"></param>
            /// <param name="numBuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetBuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetBuffers([OutAttribute] UInt32[] buffers, Int32 maxBuffers, [OutAttribute] out Int32 numBuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="framebuffers">[length: maxFramebuffers]</param>
            /// <param name="maxFramebuffers"></param>
            /// <param name="numFramebuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetFramebuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetFramebuffers([OutAttribute] Int32[] framebuffers, Int32 maxFramebuffers, [OutAttribute] out Int32 numFramebuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="framebuffers">[length: maxFramebuffers]</param>
            /// <param name="maxFramebuffers"></param>
            /// <param name="numFramebuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetFramebuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetFramebuffers([OutAttribute] UInt32[] framebuffers, Int32 maxFramebuffers, [OutAttribute] out Int32 numFramebuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgramBinarySource(Int32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] Int32[] length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgramBinarySource(Int32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] out Int32 length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetProgramBinarySource(Int32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] Int32* length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgramBinarySource(UInt32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] Int32[] length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgramBinarySource(UInt32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] out Int32 length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            /// <param name="shadertype"></param>
            /// <param name="source"></param>
            /// <param name="length"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetProgramBinarySource(UInt32 program, OpenTK.Graphics.ES20.All shadertype, [OutAttribute] StringBuilder source, [OutAttribute] Int32* length) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="programs">[length: maxPrograms]</param>
            /// <param name="maxPrograms"></param>
            /// <param name="numPrograms">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramsQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgram([OutAttribute] Int32[] programs, Int32 maxPrograms, [OutAttribute] out Int32 numPrograms) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="programs">[length: maxPrograms]</param>
            /// <param name="maxPrograms"></param>
            /// <param name="numPrograms">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetProgramsQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetProgram([OutAttribute] UInt32[] programs, Int32 maxPrograms, [OutAttribute] out Int32 numPrograms) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="renderbuffers">[length: maxRenderbuffers]</param>
            /// <param name="maxRenderbuffers"></param>
            /// <param name="numRenderbuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetRenderbuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetRenderbuffers([OutAttribute] Int32[] renderbuffers, Int32 maxRenderbuffers, [OutAttribute] out Int32 numRenderbuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="renderbuffers">[length: maxRenderbuffers]</param>
            /// <param name="maxRenderbuffers"></param>
            /// <param name="numRenderbuffers">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetRenderbuffersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetRenderbuffers([OutAttribute] UInt32[] renderbuffers, Int32 maxRenderbuffers, [OutAttribute] out Int32 numRenderbuffers) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="shaders">[length: maxShaders]</param>
            /// <param name="maxShaders"></param>
            /// <param name="numShaders">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetShadersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetShaders([OutAttribute] Int32[] shaders, Int32 maxShaders, [OutAttribute] out Int32 numShaders) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="shaders">[length: maxShaders]</param>
            /// <param name="maxShaders"></param>
            /// <param name="numShaders">[length: 1]</param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtGetShadersQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetShaders([OutAttribute] UInt32[] shaders, Int32 maxShaders, [OutAttribute] out Int32 numShaders) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexLevelParameter(Int32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] Int32[] @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexLevelParameter(Int32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] out Int32 @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetTexLevelParameter(Int32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] Int32* @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexLevelParameter(UInt32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] Int32[] @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexLevelParameter(UInt32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] out Int32 @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="texture"></param>
            /// <param name="face"></param>
            /// <param name="level"></param>
            /// <param name="pname"></param>
            /// <param name="@params"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetTexLevelParameter(UInt32 texture, OpenTK.Graphics.ES20.All face, Int32 level, OpenTK.Graphics.ES20.All pname, [OutAttribute] Int32* @params) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="level"></param>
            /// <param name="xoffset"></param>
            /// <param name="yoffset"></param>
            /// <param name="zoffset"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="depth"></param>
            /// <param name="format"></param>
            /// <param name="type"></param>
            /// <param name="texels"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexSubImageQCOM")]
            public static void ExtGetTexSubImage(OpenTK.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, OpenTK.Graphics.ES20.All format, OpenTK.Graphics.ES20.All type, [OutAttribute] IntPtr texels) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="level"></param>
            /// <param name="xoffset"></param>
            /// <param name="yoffset"></param>
            /// <param name="zoffset"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="depth"></param>
            /// <param name="format"></param>
            /// <param name="type"></param>
            /// <param name="texels"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexSubImageQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexSubImage<T10>(OpenTK.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, OpenTK.Graphics.ES20.All format, OpenTK.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[] texels)
                where T10 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="level"></param>
            /// <param name="xoffset"></param>
            /// <param name="yoffset"></param>
            /// <param name="zoffset"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="depth"></param>
            /// <param name="format"></param>
            /// <param name="type"></param>
            /// <param name="texels"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexSubImageQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexSubImage<T10>(OpenTK.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, OpenTK.Graphics.ES20.All format, OpenTK.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,] texels)
                where T10 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="level"></param>
            /// <param name="xoffset"></param>
            /// <param name="yoffset"></param>
            /// <param name="zoffset"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="depth"></param>
            /// <param name="format"></param>
            /// <param name="type"></param>
            /// <param name="texels"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexSubImageQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTexSubImage<T10>(OpenTK.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, OpenTK.Graphics.ES20.All format, OpenTK.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,,] texels)
                where T10 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="level"></param>
            /// <param name="xoffset"></param>
            /// <param name="yoffset"></param>
            /// <param name="zoffset"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="depth"></param>
            /// <param name="format"></param>
            /// <param name="type"></param>
            /// <param name="texels"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexSubImageQCOM")]
            public static void ExtGetTexSubImage<T10>(OpenTK.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, OpenTK.Graphics.ES20.All format, OpenTK.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T10 texels)
                where T10 : struct
             { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTextures([OutAttribute] Int32[] textures, Int32 maxTextures, [OutAttribute] Int32[] numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTextures([OutAttribute] out Int32 textures, Int32 maxTextures, [OutAttribute] out Int32 numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetTextures([OutAttribute] Int32* textures, Int32 maxTextures, [OutAttribute] Int32* numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTextures([OutAttribute] UInt32[] textures, Int32 maxTextures, [OutAttribute] Int32[] numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static void ExtGetTextures([OutAttribute] out UInt32 textures, Int32 maxTextures, [OutAttribute] out Int32 numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="textures"></param>
            /// <param name="maxTextures"></param>
            /// <param name="numTextures"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtGetTexturesQCOM")]
            [CLSCompliant(false)]
            public static unsafe void ExtGetTextures([OutAttribute] UInt32* textures, Int32 maxTextures, [OutAttribute] Int32* numTextures) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtIsProgramBinaryQCOM")]
            [CLSCompliant(false)]
            public static bool ExtIsProgramBinary(Int32 program) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get2]</summary>
            /// <param name="program"></param>
            [AutoGenerated(Category = "QCOM_extended_get2", Version = "", EntryPoint = "glExtIsProgramBinaryQCOM")]
            [CLSCompliant(false)]
            public static bool ExtIsProgramBinary(UInt32 program) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_extended_get]</summary>
            /// <param name="target"></param>
            /// <param name="pname"></param>
            /// <param name="param"></param>
            [AutoGenerated(Category = "QCOM_extended_get", Version = "", EntryPoint = "glExtTexObjectStateOverrideiQCOM")]
            public static void ExtTexObjectStateOverride(OpenTK.Graphics.ES20.All target, OpenTK.Graphics.ES20.All pname, Int32 param) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControl([OutAttribute] Int32[] num, Int32 size, [OutAttribute] Int32[] driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControl([OutAttribute] Int32[] num, Int32 size, [OutAttribute] UInt32[] driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControl([OutAttribute] out Int32 num, Int32 size, [OutAttribute] out Int32 driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControl([OutAttribute] out Int32 num, Int32 size, [OutAttribute] out UInt32 driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static unsafe void GetDriverControl([OutAttribute] Int32* num, Int32 size, [OutAttribute] Int32* driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="num"></param>
            /// <param name="size"></param>
            /// <param name="driverControls">[length: size]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlsQCOM")]
            [CLSCompliant(false)]
            public static unsafe void GetDriverControl([OutAttribute] Int32* num, Int32 size, [OutAttribute] UInt32* driverControls) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static unsafe void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_driver_control]</summary>
            /// <param name="driverControl"></param>
            /// <param name="bufSize"></param>
            /// <param name="length"></param>
            /// <param name="driverControlString">[length: bufSize]</param>
            [AutoGenerated(Category = "QCOM_driver_control", Version = "", EntryPoint = "glGetDriverControlStringQCOM")]
            [CLSCompliant(false)]
            public static unsafe void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder driverControlString) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_tiled_rendering]</summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="preserveMask"></param>
            [AutoGenerated(Category = "QCOM_tiled_rendering", Version = "", EntryPoint = "glStartTilingQCOM")]
            [CLSCompliant(false)]
            public static void StartTiling(Int32 x, Int32 y, Int32 width, Int32 height, Int32 preserveMask) { throw new NotImplementedException(); }

            /// <summary>[requires: QCOM_tiled_rendering]</summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="preserveMask"></param>
            [AutoGenerated(Category = "QCOM_tiled_rendering", Version = "", EntryPoint = "glStartTilingQCOM")]
            [CLSCompliant(false)]
            public static void StartTiling(UInt32 x, UInt32 y, UInt32 width, UInt32 height, UInt32 preserveMask) { throw new NotImplementedException(); }

        }    	
    }
}
