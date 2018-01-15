//
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
        [Slot(5)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBeginPerfMonitorAMD(UInt32 monitor);
        [Slot(68)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeletePerfMonitorsAMD(Int32 n, UInt32* monitors);
        [Slot(104)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEndPerfMonitorAMD(UInt32 monitor);
        [Slot(136)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenPerfMonitorsAMD(Int32 n, [OutAttribute] UInt32* monitors);
        [Slot(171)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfMonitorCounterDataAMD(UInt32 monitor, System.Int32 pname, Int32 dataSize, [OutAttribute] UInt32* data, [OutAttribute] Int32* bytesWritten);
        [Slot(172)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGetPerfMonitorCounterInfoAMD(UInt32 group, UInt32 counter, System.Int32 pname, [OutAttribute] IntPtr data);
        [Slot(173)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfMonitorCountersAMD(UInt32 group, [OutAttribute] Int32* numCounters, [OutAttribute] Int32* maxActiveCounters, Int32 counterSize, [OutAttribute] UInt32* counters);
        [Slot(174)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfMonitorCounterStringAMD(UInt32 group, UInt32 counter, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr counterString);
        [Slot(175)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfMonitorGroupsAMD([OutAttribute] Int32* numGroups, Int32 groupsSize, [OutAttribute] UInt32* groups);
        [Slot(176)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfMonitorGroupStringAMD(UInt32 group, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr groupString);
        [Slot(301)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glSelectPerfMonitorCountersAMD(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, [OutAttribute] UInt32* counterList);
        [Slot(28)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlitFramebufferANGLE(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, System.Int32 mask, System.Int32 filter);
        [Slot(88)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawArraysInstancedANGLE(System.Int32 mode, Int32 first, Int32 count, Int32 primcount);
        [Slot(95)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawElementsInstancedANGLE(System.Int32 mode, Int32 count, System.Int32 type, IntPtr indices, Int32 primcount);
        [Slot(205)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetTranslatedShaderSourceANGLE(UInt32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] IntPtr source);
        [Slot(291)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorageMultisampleANGLE(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(372)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttribDivisorANGLE(UInt32 index, UInt32 divisor);
        [Slot(37)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern System.Int32 glClientWaitSyncAPPLE(IntPtr sync, System.Int32 flags, UInt64 timeout);
        [Slot(50)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyTextureLevelsAPPLE(UInt32 destinationTexture, UInt32 sourceTexture, Int32 sourceBaseLevel, Int32 sourceLevelCount);
        [Slot(75)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDeleteSyncAPPLE(IntPtr sync);
        [Slot(120)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr glFenceSyncAPPLE(System.Int32 condition, System.Int32 flags);
        [Slot(159)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetInteger64vAPPLE(System.Int32 pname, [OutAttribute] Int64* @params);
        [Slot(200)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetSyncivAPPLE(IntPtr sync, System.Int32 pname, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* values);
        [Slot(224)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsSyncAPPLE(IntPtr sync);
        [Slot(292)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorageMultisampleAPPLE(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(296)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glResolveMultisampleFramebufferAPPLE();
        [Slot(377)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glWaitSyncAPPLE(IntPtr sync, System.Int32 flags, UInt64 timeout);
        [Slot(2)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glActiveTexture(System.Int32 texture);
        [Slot(4)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glAttachShader(UInt32 program, UInt32 shader);
        [Slot(8)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindAttribLocation(UInt32 program, UInt32 index, IntPtr name);
        [Slot(9)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindBuffer(System.Int32 target, UInt32 buffer);
        [Slot(10)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindFramebuffer(System.Int32 target, UInt32 framebuffer);
        [Slot(12)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindRenderbuffer(System.Int32 target, UInt32 renderbuffer);
        [Slot(13)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindTexture(System.Int32 target, UInt32 texture);
        [Slot(17)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendColor(Single red, Single green, Single blue, Single alpha);
        [Slot(18)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendEquation(System.Int32 mode);
        [Slot(21)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendEquationSeparate(System.Int32 modeRGB, System.Int32 modeAlpha);
        [Slot(23)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendFunc(System.Int32 sfactor, System.Int32 dfactor);
        [Slot(25)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendFuncSeparate(System.Int32 sfactorRGB, System.Int32 dfactorRGB, System.Int32 sfactorAlpha, System.Int32 dfactorAlpha);
        [Slot(30)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBufferData(System.Int32 target, IntPtr size, IntPtr data, System.Int32 usage);
        [Slot(31)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBufferSubData(System.Int32 target, IntPtr offset, IntPtr size, IntPtr data);
        [Slot(32)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern System.Int32 glCheckFramebufferStatus(System.Int32 target);
        [Slot(33)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glClear(System.Int32 mask);
        [Slot(34)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glClearColor(Single red, Single green, Single blue, Single alpha);
        [Slot(35)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glClearDepthf(Single d);
        [Slot(36)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glClearStencil(Int32 s);
        [Slot(38)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glColorMask(bool red, bool green, bool blue, bool alpha);
        [Slot(40)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCompileShader(UInt32 shader);
        [Slot(41)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCompressedTexImage2D(System.Int32 target, Int32 level, System.Int32 internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
        [Slot(43)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCompressedTexSubImage2D(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Int32 format, Int32 imageSize, IntPtr data);
        [Slot(47)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyTexImage2D(System.Int32 target, Int32 level, System.Int32 internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
        [Slot(48)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyTexSubImage2D(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
        [Slot(54)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glCreateProgram();
        [Slot(55)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glCreateShader(System.Int32 type);
        [Slot(58)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCullFace(System.Int32 mode);
        [Slot(59)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDebugMessageCallback(DebugProc callback, IntPtr userParam);
        [Slot(61)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDebugMessageControl(System.Int32 source, System.Int32 type, System.Int32 severity, Int32 count, UInt32* ids, bool enabled);
        [Slot(63)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDebugMessageInsert(System.Int32 source, System.Int32 type, UInt32 id, System.Int32 severity, Int32 length, IntPtr buf);
        [Slot(65)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteBuffers(Int32 n, UInt32* buffers);
        [Slot(67)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteFramebuffers(Int32 n, UInt32* framebuffers);
        [Slot(70)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDeleteProgram(UInt32 program);
        [Slot(73)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteRenderbuffers(Int32 n, UInt32* renderbuffers);
        [Slot(74)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDeleteShader(UInt32 shader);
        [Slot(76)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteTextures(Int32 n, UInt32* textures);
        [Slot(78)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDepthFunc(System.Int32 func);
        [Slot(79)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDepthMask(bool flag);
        [Slot(80)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDepthRangef(Single n, Single f);
        [Slot(81)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDetachShader(UInt32 program, UInt32 shader);
        [Slot(82)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDisable(System.Int32 cap);
        [Slot(85)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDisableVertexAttribArray(UInt32 index);
        [Slot(87)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawArrays(System.Int32 mode, Int32 first, Int32 count);
        [Slot(94)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawElements(System.Int32 mode, Int32 count, System.Int32 type, IntPtr indices);
        [Slot(100)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEnable(System.Int32 cap);
        [Slot(103)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEnableVertexAttribArray(UInt32 index);
        [Slot(121)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFinish();
        [Slot(123)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFlush();
        [Slot(125)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferRenderbuffer(System.Int32 target, System.Int32 attachment, System.Int32 renderbuffertarget, UInt32 renderbuffer);
        [Slot(126)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferTexture2D(System.Int32 target, System.Int32 attachment, System.Int32 textarget, UInt32 texture, Int32 level);
        [Slot(131)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFrontFace(System.Int32 mode);
        [Slot(132)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenBuffers(Int32 n, [OutAttribute] UInt32* buffers);
        [Slot(133)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGenerateMipmap(System.Int32 target);
        [Slot(135)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenFramebuffers(Int32 n, [OutAttribute] UInt32* framebuffers);
        [Slot(139)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenRenderbuffers(Int32 n, [OutAttribute] UInt32* renderbuffers);
        [Slot(140)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenTextures(Int32 n, [OutAttribute] UInt32* textures);
        [Slot(142)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetActiveAttrib(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Int32* type, [OutAttribute] IntPtr name);
        [Slot(143)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetActiveUniform(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Int32* type, [OutAttribute] IntPtr name);
        [Slot(144)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetAttachedShaders(UInt32 program, Int32 maxCount, [OutAttribute] Int32* count, [OutAttribute] UInt32* shaders);
        [Slot(145)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glGetAttribLocation(UInt32 program, IntPtr name);
        [Slot(146)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetBooleanv(System.Int32 pname, [OutAttribute] bool* data);
        [Slot(147)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetBufferParameteriv(System.Int32 target, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(149)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe Int32 glGetDebugMessageLog(UInt32 count, Int32 bufSize, [OutAttribute] System.Int32* sources, [OutAttribute] System.Int32* types, [OutAttribute] UInt32* ids, [OutAttribute] System.Int32* severities, [OutAttribute] Int32* lengths, [OutAttribute] IntPtr messageLog);
        [Slot(153)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern System.Int32 glGetError();
        [Slot(156)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetFloatv(System.Int32 pname, [OutAttribute] Single* data);
        [Slot(157)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetFramebufferAttachmentParameteriv(System.Int32 target, System.Int32 attachment, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(161)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetIntegerv(System.Int32 pname, [OutAttribute] Int32* data);
        [Slot(165)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetObjectLabel(System.Int32 identifier, UInt32 name, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr label);
        [Slot(168)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetObjectPtrLabel(IntPtr ptr, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr label);
        [Slot(180)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGetPointerv(System.Int32 pname, [OutAttribute] IntPtr @params);
        [Slot(183)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetProgramInfoLog(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr infoLog);
        [Slot(184)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetProgramiv(UInt32 program, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(192)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetRenderbufferParameteriv(System.Int32 target, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(195)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetShaderInfoLog(UInt32 shader, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr infoLog);
        [Slot(196)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetShaderiv(UInt32 shader, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(197)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetShaderPrecisionFormat(System.Int32 shadertype, System.Int32 precisiontype, [OutAttribute] Int32* range, [OutAttribute] Int32* precision);
        [Slot(198)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetShaderSource(UInt32 shader, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr source);
        [Slot(199)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr glGetString(System.Int32 name);
        [Slot(201)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetTexParameterfv(System.Int32 target, System.Int32 pname, [OutAttribute] Single* @params);
        [Slot(204)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetTexParameteriv(System.Int32 target, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(206)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetUniformfv(UInt32 program, Int32 location, [OutAttribute] Single* @params);
        [Slot(207)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetUniformiv(UInt32 program, Int32 location, [OutAttribute] Int32* @params);
        [Slot(208)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glGetUniformLocation(UInt32 program, IntPtr name);
        [Slot(209)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetVertexAttribfv(UInt32 index, System.Int32 pname, [OutAttribute] Single* @params);
        [Slot(210)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetVertexAttribiv(UInt32 index, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(211)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGetVertexAttribPointerv(UInt32 index, System.Int32 pname, [OutAttribute] IntPtr pointer);
        [Slot(212)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glHint(System.Int32 target, System.Int32 mode);
        [Slot(214)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsBuffer(UInt32 buffer);
        [Slot(215)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsEnabled(System.Int32 cap);
        [Slot(218)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsFramebuffer(UInt32 framebuffer);
        [Slot(219)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsProgram(UInt32 program);
        [Slot(222)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsRenderbuffer(UInt32 renderbuffer);
        [Slot(223)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsShader(UInt32 shader);
        [Slot(225)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsTexture(UInt32 texture);
        [Slot(228)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glLineWidth(Single width);
        [Slot(229)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glLinkProgram(UInt32 program);
        [Slot(235)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glObjectLabel(System.Int32 identifier, UInt32 name, Int32 length, IntPtr label);
        [Slot(237)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glObjectPtrLabel(IntPtr ptr, Int32 length, IntPtr label);
        [Slot(240)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPixelStorei(System.Int32 pname, Int32 param);
        [Slot(241)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPolygonOffset(Single factor, Single units);
        [Slot(242)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPopDebugGroup();
        [Slot(281)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPushDebugGroup(System.Int32 source, UInt32 id, Int32 length, IntPtr message);
        [Slot(288)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Int32 format, System.Int32 type, [OutAttribute] IntPtr pixels);
        [Slot(289)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glReleaseShaderCompiler();
        [Slot(290)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorage(System.Int32 target, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(297)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glSampleCoverage(Single value, bool invert);
        [Slot(300)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glScissor(Int32 x, Int32 y, Int32 width, Int32 height);
        [Slot(303)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glShaderBinary(Int32 count, UInt32* shaders, System.Int32 binaryformat, IntPtr binary, Int32 length);
        [Slot(304)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glShaderSource(UInt32 shader, Int32 count, IntPtr @string, Int32* length);
        [Slot(306)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilFunc(System.Int32 func, Int32 @ref, UInt32 mask);
        [Slot(307)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilFuncSeparate(System.Int32 face, System.Int32 func, Int32 @ref, UInt32 mask);
        [Slot(308)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilMask(UInt32 mask);
        [Slot(309)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilMaskSeparate(System.Int32 face, UInt32 mask);
        [Slot(310)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilOp(System.Int32 fail, System.Int32 zfail, System.Int32 zpass);
        [Slot(311)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStencilOpSeparate(System.Int32 face, System.Int32 sfail, System.Int32 dpfail, System.Int32 dppass);
        [Slot(315)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexImage2D(System.Int32 target, Int32 level, System.Int32 internalformat, Int32 width, Int32 height, Int32 border, System.Int32 format, System.Int32 type, IntPtr pixels);
        [Slot(317)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexParameterf(System.Int32 target, System.Int32 pname, Single param);
        [Slot(318)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glTexParameterfv(System.Int32 target, System.Int32 pname, Single* @params);
        [Slot(319)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexParameteri(System.Int32 target, System.Int32 pname, Int32 param);
        [Slot(322)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glTexParameteriv(System.Int32 target, System.Int32 pname, Int32* @params);
        [Slot(327)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexSubImage2D(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Int32 format, System.Int32 type, IntPtr pixels);
        [Slot(333)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform1f(Int32 location, Single v0);
        [Slot(334)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform1fv(Int32 location, Int32 count, Single* value);
        [Slot(335)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform1i(Int32 location, Int32 v0);
        [Slot(336)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform1iv(Int32 location, Int32 count, Int32* value);
        [Slot(337)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform2f(Int32 location, Single v0, Single v1);
        [Slot(338)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform2fv(Int32 location, Int32 count, Single* value);
        [Slot(339)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform2i(Int32 location, Int32 v0, Int32 v1);
        [Slot(340)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform2iv(Int32 location, Int32 count, Int32* value);
        [Slot(341)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform3f(Int32 location, Single v0, Single v1, Single v2);
        [Slot(342)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform3fv(Int32 location, Int32 count, Single* value);
        [Slot(343)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform3i(Int32 location, Int32 v0, Int32 v1, Int32 v2);
        [Slot(344)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform3iv(Int32 location, Int32 count, Int32* value);
        [Slot(345)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform4f(Int32 location, Single v0, Single v1, Single v2, Single v3);
        [Slot(346)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform4fv(Int32 location, Int32 count, Single* value);
        [Slot(347)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUniform4i(Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
        [Slot(348)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniform4iv(Int32 location, Int32 count, Int32* value);
        [Slot(349)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix2fv(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(352)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix3fv(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(355)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix4fv(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(359)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUseProgram(UInt32 program);
        [Slot(362)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glValidateProgram(UInt32 program);
        [Slot(364)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttrib1f(UInt32 index, Single x);
        [Slot(365)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glVertexAttrib1fv(UInt32 index, Single* v);
        [Slot(366)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttrib2f(UInt32 index, Single x, Single y);
        [Slot(367)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glVertexAttrib2fv(UInt32 index, Single* v);
        [Slot(368)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttrib3f(UInt32 index, Single x, Single y, Single z);
        [Slot(369)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glVertexAttrib3fv(UInt32 index, Single* v);
        [Slot(370)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttrib4f(UInt32 index, Single x, Single y, Single z, Single w);
        [Slot(371)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glVertexAttrib4fv(UInt32 index, Single* v);
        [Slot(375)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttribPointer(UInt32 index, Int32 size, System.Int32 type, bool normalized, Int32 stride, IntPtr pointer);
        [Slot(376)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glViewport(Int32 x, Int32 y, Int32 width, Int32 height);
        [Slot(0)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glActiveProgramEXT(UInt32 program);
        [Slot(1)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glActiveShaderProgramEXT(UInt32 pipeline, UInt32 program);
        [Slot(7)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBeginQueryEXT(System.Int32 target, UInt32 id);
        [Slot(11)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindProgramPipelineEXT(UInt32 pipeline);
        [Slot(19)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendEquationEXT(System.Int32 mode);
        [Slot(20)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendEquationiEXT(UInt32 buf, System.Int32 mode);
        [Slot(22)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendEquationSeparateiEXT(UInt32 buf, System.Int32 modeRGB, System.Int32 modeAlpha);
        [Slot(24)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendFunciEXT(UInt32 buf, System.Int32 src, System.Int32 dst);
        [Slot(26)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendFuncSeparateiEXT(UInt32 buf, System.Int32 srcRGB, System.Int32 dstRGB, System.Int32 srcAlpha, System.Int32 dstAlpha);
        [Slot(39)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glColorMaskiEXT(UInt32 index, bool r, bool g, bool b, bool a);
        [Slot(46)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyImageSubDataEXT(UInt32 srcName, System.Int32 srcTarget, Int32 srcLevel, Int32 srcX, Int32 srcY, Int32 srcZ, UInt32 dstName, System.Int32 dstTarget, Int32 dstLevel, Int32 dstX, Int32 dstY, Int32 dstZ, Int32 srcWidth, Int32 srcHeight, Int32 srcDepth);
        [Slot(56)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glCreateShaderProgramEXT(System.Int32 type, IntPtr @string);
        [Slot(57)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern Int32 glCreateShaderProgramvEXT(System.Int32 type, Int32 count, IntPtr strings);
        [Slot(71)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteProgramPipelinesEXT(Int32 n, UInt32* pipelines);
        [Slot(72)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteQueriesEXT(Int32 n, UInt32* ids);
        [Slot(84)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDisableiEXT(System.Int32 target, UInt32 index);
        [Slot(86)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDiscardFramebufferEXT(System.Int32 target, Int32 numAttachments, System.Int32* attachments);
        [Slot(89)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawArraysInstancedEXT(System.Int32 mode, Int32 start, Int32 count, Int32 primcount);
        [Slot(91)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDrawBuffersEXT(Int32 n, System.Int32* bufs);
        [Slot(92)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDrawBuffersIndexedEXT(Int32 n, System.Int32* location, Int32* indices);
        [Slot(96)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawElementsInstancedEXT(System.Int32 mode, Int32 count, System.Int32 type, IntPtr indices, Int32 primcount);
        [Slot(102)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEnableiEXT(System.Int32 target, UInt32 index);
        [Slot(106)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEndQueryEXT(System.Int32 target);
        [Slot(124)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFlushMappedBufferRangeEXT(System.Int32 target, IntPtr offset, IntPtr length);
        [Slot(127)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferTexture2DMultisampleEXT(System.Int32 target, System.Int32 attachment, System.Int32 textarget, UInt32 texture, Int32 level, Int32 samples);
        [Slot(130)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferTextureEXT(System.Int32 target, System.Int32 attachment, UInt32 texture, Int32 level);
        [Slot(137)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenProgramPipelinesEXT(Int32 n, [OutAttribute] UInt32* pipelines);
        [Slot(138)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenQueriesEXT(Int32 n, [OutAttribute] UInt32* ids);
        [Slot(158)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern System.Int32 glGetGraphicsResetStatusEXT();
        [Slot(160)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetIntegeri_vEXT(System.Int32 target, UInt32 index, [OutAttribute] Int32* data);
        [Slot(163)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetnUniformfvEXT(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] Single* @params);
        [Slot(164)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetnUniformivEXT(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] Int32* @params);
        [Slot(166)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetObjectLabelEXT(System.Int32 type, UInt32 @object, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr label);
        [Slot(185)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetProgramPipelineInfoLogEXT(UInt32 pipeline, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr infoLog);
        [Slot(186)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetProgramPipelineivEXT(UInt32 pipeline, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(187)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetQueryivEXT(System.Int32 target, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(188)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetQueryObjecti64vEXT(UInt32 id, System.Int32 pname, [OutAttribute] Int64* @params);
        [Slot(189)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetQueryObjectivEXT(UInt32 id, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(190)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetQueryObjectui64vEXT(UInt32 id, System.Int32 pname, [OutAttribute] UInt64* @params);
        [Slot(191)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetQueryObjectuivEXT(UInt32 id, System.Int32 pname, [OutAttribute] UInt32* @params);
        [Slot(193)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetSamplerParameterIivEXT(UInt32 sampler, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(194)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetSamplerParameterIuivEXT(UInt32 sampler, System.Int32 pname, [OutAttribute] UInt32* @params);
        [Slot(202)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetTexParameterIivEXT(System.Int32 target, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(203)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetTexParameterIuivEXT(System.Int32 target, System.Int32 pname, [OutAttribute] UInt32* @params);
        [Slot(213)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glInsertEventMarkerEXT(Int32 length, IntPtr marker);
        [Slot(216)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsEnablediEXT(System.Int32 target, UInt32 index);
        [Slot(220)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsProgramPipelineEXT(UInt32 pipeline);
        [Slot(221)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsQueryEXT(UInt32 id);
        [Slot(227)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glLabelObjectEXT(System.Int32 type, UInt32 @object, Int32 length, IntPtr label);
        [Slot(231)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr glMapBufferRangeEXT(System.Int32 target, IntPtr offset, IntPtr length, UInt32 access);
        [Slot(233)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glMultiDrawArraysEXT(System.Int32 mode, Int32* first, Int32* count, Int32 primcount);
        [Slot(234)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glMultiDrawElementsEXT(System.Int32 mode, Int32* count, System.Int32 type, IntPtr indices, Int32 primcount);
        [Slot(239)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPatchParameteriEXT(System.Int32 pname, Int32 value);
        [Slot(244)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPopGroupMarkerEXT();
        [Slot(245)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPrimitiveBoundingBoxEXT(Single minX, Single minY, Single minZ, Single minW, Single maxX, Single maxY, Single maxZ, Single maxW);
        [Slot(247)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramParameteriEXT(UInt32 program, System.Int32 pname, Int32 value);
        [Slot(248)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform1fEXT(UInt32 program, Int32 location, Single v0);
        [Slot(249)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform1fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
        [Slot(250)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform1iEXT(UInt32 program, Int32 location, Int32 v0);
        [Slot(251)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform1ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
        [Slot(252)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform1uiEXT(UInt32 program, Int32 location, UInt32 v0);
        [Slot(253)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform1uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
        [Slot(254)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform2fEXT(UInt32 program, Int32 location, Single v0, Single v1);
        [Slot(255)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform2fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
        [Slot(256)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform2iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1);
        [Slot(257)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform2ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
        [Slot(258)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform2uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1);
        [Slot(259)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform2uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
        [Slot(260)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform3fEXT(UInt32 program, Int32 location, Single v0, Single v1, Single v2);
        [Slot(261)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform3fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
        [Slot(262)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform3iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2);
        [Slot(263)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform3ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
        [Slot(264)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform3uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2);
        [Slot(265)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform3uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
        [Slot(266)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform4fEXT(UInt32 program, Int32 location, Single v0, Single v1, Single v2, Single v3);
        [Slot(267)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform4fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
        [Slot(268)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform4iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
        [Slot(269)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform4ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
        [Slot(270)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramUniform4uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2, UInt32 v3);
        [Slot(271)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniform4uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
        [Slot(272)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(273)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix2x3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(274)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix2x4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(275)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(276)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix3x2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(277)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix3x4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(278)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(279)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix4x2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(280)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glProgramUniformMatrix4x3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(283)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPushGroupMarkerEXT(Int32 length, IntPtr marker);
        [Slot(284)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glQueryCounterEXT(UInt32 id, System.Int32 target);
        [Slot(285)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glReadBufferIndexedEXT(System.Int32 src, Int32 index);
        [Slot(287)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glReadnPixelsEXT(Int32 x, Int32 y, Int32 width, Int32 height, System.Int32 format, System.Int32 type, Int32 bufSize, [OutAttribute] IntPtr data);
        [Slot(293)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorageMultisampleEXT(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(298)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glSamplerParameterIivEXT(UInt32 sampler, System.Int32 pname, Int32* param);
        [Slot(299)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glSamplerParameterIuivEXT(UInt32 sampler, System.Int32 pname, UInt32* param);
        [Slot(313)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexBufferEXT(System.Int32 target, System.Int32 internalformat, UInt32 buffer);
        [Slot(314)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexBufferRangeEXT(System.Int32 target, System.Int32 internalformat, UInt32 buffer, IntPtr offset, IntPtr size);
        [Slot(320)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glTexParameterIivEXT(System.Int32 target, System.Int32 pname, Int32* @params);
        [Slot(321)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glTexParameterIuivEXT(System.Int32 target, System.Int32 pname, UInt32* @params);
        [Slot(323)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexStorage1DEXT(System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width);
        [Slot(324)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexStorage2DEXT(System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(325)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexStorage3DEXT(System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width, Int32 height, Int32 depth);
        [Slot(329)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTextureStorage1DEXT(UInt32 texture, System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width);
        [Slot(330)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTextureStorage2DEXT(UInt32 texture, System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(331)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTextureStorage3DEXT(UInt32 texture, System.Int32 target, Int32 levels, System.Int32 internalformat, Int32 width, Int32 height, Int32 depth);
        [Slot(332)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTextureViewEXT(UInt32 texture, System.Int32 target, UInt32 origtexture, System.Int32 internalformat, UInt32 minlevel, UInt32 numlevels, UInt32 minlayer, UInt32 numlayers);
        [Slot(360)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUseProgramStagesEXT(UInt32 pipeline, UInt32 stages, UInt32 program);
        [Slot(361)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glUseShaderProgramEXT(System.Int32 type, UInt32 program);
        [Slot(363)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glValidateProgramPipelineEXT(UInt32 pipeline);
        [Slot(373)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttribDivisorEXT(UInt32 index, UInt32 divisor);
        [Slot(128)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferTexture2DMultisampleIMG(System.Int32 target, System.Int32 attachment, System.Int32 textarget, UInt32 texture, Int32 level, Int32 samples);
        [Slot(294)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorageMultisampleIMG(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(6)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBeginPerfQueryINTEL(UInt32 queryHandle);
        [Slot(53)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glCreatePerfQueryINTEL(UInt32 queryId, [OutAttribute] UInt32* queryHandle);
        [Slot(69)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDeletePerfQueryINTEL(UInt32 queryHandle);
        [Slot(105)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEndPerfQueryINTEL(UInt32 queryHandle);
        [Slot(155)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetFirstPerfQueryIdINTEL([OutAttribute] UInt32* queryId);
        [Slot(162)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetNextPerfQueryIdINTEL(UInt32 queryId, [OutAttribute] UInt32* nextQueryId);
        [Slot(170)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfCounterInfoINTEL(UInt32 queryId, UInt32 counterId, UInt32 counterNameLength, [OutAttribute] IntPtr counterName, UInt32 counterDescLength, [OutAttribute] IntPtr counterDesc, [OutAttribute] UInt32* counterOffset, [OutAttribute] UInt32* counterDataSize, [OutAttribute] UInt32* counterTypeEnum, [OutAttribute] UInt32* counterDataTypeEnum, [OutAttribute] UInt64* rawCounterMaxValue);
        [Slot(177)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfQueryDataINTEL(UInt32 queryHandle, UInt32 flags, Int32 dataSize, [OutAttribute] IntPtr data, [OutAttribute] UInt32* bytesWritten);
        [Slot(178)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfQueryIdByNameINTEL([OutAttribute] IntPtr queryName, [OutAttribute] UInt32* queryId);
        [Slot(179)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetPerfQueryInfoINTEL(UInt32 queryId, UInt32 queryNameLength, [OutAttribute] IntPtr queryName, [OutAttribute] UInt32* dataSize, [OutAttribute] UInt32* noCounters, [OutAttribute] UInt32* noInstances, [OutAttribute] UInt32* capsMask);
        [Slot(15)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendBarrierKHR();
        [Slot(60)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDebugMessageCallbackKHR(DebugProcKhr callback, IntPtr userParam);
        [Slot(62)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDebugMessageControlKHR(System.Int32 source, System.Int32 type, System.Int32 severity, Int32 count, UInt32* ids, bool enabled);
        [Slot(64)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDebugMessageInsertKHR(System.Int32 source, System.Int32 type, UInt32 id, System.Int32 severity, Int32 length, IntPtr buf);
        [Slot(150)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe Int32 glGetDebugMessageLogKHR(UInt32 count, Int32 bufSize, [OutAttribute] System.Int32* sources, [OutAttribute] System.Int32* types, [OutAttribute] UInt32* ids, [OutAttribute] System.Int32* severities, [OutAttribute] Int32* lengths, [OutAttribute] IntPtr messageLog);
        [Slot(167)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetObjectLabelKHR(System.Int32 identifier, UInt32 name, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr label);
        [Slot(169)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetObjectPtrLabelKHR(IntPtr ptr, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr label);
        [Slot(181)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGetPointervKHR(System.Int32 pname, [OutAttribute] IntPtr @params);
        [Slot(236)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glObjectLabelKHR(System.Int32 identifier, UInt32 name, Int32 length, IntPtr label);
        [Slot(238)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glObjectPtrLabelKHR(IntPtr ptr, Int32 length, IntPtr label);
        [Slot(243)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPopDebugGroupKHR();
        [Slot(282)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glPushDebugGroupKHR(System.Int32 source, UInt32 id, Int32 length, IntPtr message);
        [Slot(16)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendBarrierNV();
        [Slot(27)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlendParameteriNV(System.Int32 pname, Int32 value);
        [Slot(29)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBlitFramebufferNV(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, System.Int32 mask, System.Int32 filter);
        [Slot(45)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyBufferSubDataNV(System.Int32 readTarget, System.Int32 writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
        [Slot(51)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCoverageMaskNV(bool mask);
        [Slot(52)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCoverageOperationNV(System.Int32 operation);
        [Slot(66)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteFencesNV(Int32 n, UInt32* fences);
        [Slot(90)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawArraysInstancedNV(System.Int32 mode, Int32 first, Int32 count, Int32 primcount);
        [Slot(93)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDrawBuffersNV(Int32 n, System.Int32* bufs);
        [Slot(97)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDrawElementsInstancedNV(System.Int32 mode, Int32 count, System.Int32 type, IntPtr indices, Int32 primcount);
        [Slot(122)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFinishFenceNV(UInt32 fence);
        [Slot(134)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenFencesNV(Int32 n, [OutAttribute] UInt32* fences);
        [Slot(154)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetFenceivNV(UInt32 fence, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(217)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsFenceNV(UInt32 fence);
        [Slot(286)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glReadBufferNV(System.Int32 mode);
        [Slot(295)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glRenderbufferStorageMultisampleNV(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height);
        [Slot(302)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glSetFenceNV(UInt32 fence, System.Int32 condition);
        [Slot(312)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glTestFenceNV(UInt32 fence);
        [Slot(350)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix2x3fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(351)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix2x4fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(353)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix3x2fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(354)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix3x4fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(356)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix4x2fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(357)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glUniformMatrix4x3fvNV(Int32 location, Int32 count, bool transpose, Single* value);
        [Slot(374)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glVertexAttribDivisorNV(UInt32 index, UInt32 divisor);
        [Slot(14)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glBindVertexArrayOES(UInt32 array);
        [Slot(42)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCompressedTexImage3DOES(System.Int32 target, Int32 level, System.Int32 internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
        [Slot(44)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCompressedTexSubImage3DOES(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Int32 format, Int32 imageSize, IntPtr data);
        [Slot(49)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glCopyTexSubImage3DOES(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
        [Slot(77)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glDeleteVertexArraysOES(Int32 n, UInt32* arrays);
        [Slot(98)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEGLImageTargetRenderbufferStorageOES(System.Int32 target, IntPtr image);
        [Slot(99)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEGLImageTargetTexture2DOES(System.Int32 target, IntPtr image);
        [Slot(129)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glFramebufferTexture3DOES(System.Int32 target, System.Int32 attachment, System.Int32 textarget, UInt32 texture, Int32 level, Int32 zoffset);
        [Slot(141)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGenVertexArraysOES(Int32 n, [OutAttribute] UInt32* arrays);
        [Slot(148)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glGetBufferPointervOES(System.Int32 target, System.Int32 pname, [OutAttribute] IntPtr @params);
        [Slot(182)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetProgramBinaryOES(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Int32* binaryFormat, [OutAttribute] IntPtr binary);
        [Slot(226)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glIsVertexArrayOES(UInt32 array);
        [Slot(230)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr glMapBufferOES(System.Int32 target, System.Int32 access);
        [Slot(232)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glMinSampleShadingOES(Single value);
        [Slot(246)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glProgramBinaryOES(UInt32 program, System.Int32 binaryFormat, IntPtr binary, Int32 length);
        [Slot(316)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexImage3DOES(System.Int32 target, Int32 level, System.Int32 internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Int32 format, System.Int32 type, IntPtr pixels);
        [Slot(326)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexStorage3DMultisampleOES(System.Int32 target, Int32 samples, System.Int32 internalformat, Int32 width, Int32 height, Int32 depth, bool fixedsamplelocations);
        [Slot(328)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glTexSubImage3DOES(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Int32 format, System.Int32 type, IntPtr pixels);
        [Slot(358)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glUnmapBufferOES(System.Int32 target);
        [Slot(3)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glAlphaFuncQCOM(System.Int32 func, Single @ref);
        [Slot(83)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glDisableDriverControlQCOM(UInt32 driverControl);
        [Slot(101)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEnableDriverControlQCOM(UInt32 driverControl);
        [Slot(107)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glEndTilingQCOM(UInt32 preserveMask);
        [Slot(108)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glExtGetBufferPointervQCOM(System.Int32 target, [OutAttribute] IntPtr @params);
        [Slot(109)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetBuffersQCOM([OutAttribute] UInt32* buffers, Int32 maxBuffers, [OutAttribute] Int32* numBuffers);
        [Slot(110)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetFramebuffersQCOM([OutAttribute] UInt32* framebuffers, Int32 maxFramebuffers, [OutAttribute] Int32* numFramebuffers);
        [Slot(111)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetProgramBinarySourceQCOM(UInt32 program, System.Int32 shadertype, [OutAttribute] IntPtr source, [OutAttribute] Int32* length);
        [Slot(112)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetProgramsQCOM([OutAttribute] UInt32* programs, Int32 maxPrograms, [OutAttribute] Int32* numPrograms);
        [Slot(113)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetRenderbuffersQCOM([OutAttribute] UInt32* renderbuffers, Int32 maxRenderbuffers, [OutAttribute] Int32* numRenderbuffers);
        [Slot(114)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetShadersQCOM([OutAttribute] UInt32* shaders, Int32 maxShaders, [OutAttribute] Int32* numShaders);
        [Slot(115)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetTexLevelParameterivQCOM(UInt32 texture, System.Int32 face, Int32 level, System.Int32 pname, [OutAttribute] Int32* @params);
        [Slot(116)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glExtGetTexSubImageQCOM(System.Int32 target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Int32 format, System.Int32 type, [OutAttribute] IntPtr texels);
        [Slot(117)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glExtGetTexturesQCOM([OutAttribute] UInt32* textures, Int32 maxTextures, [OutAttribute] Int32* numTextures);
        [Slot(118)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern byte glExtIsProgramBinaryQCOM(UInt32 program);
        [Slot(119)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glExtTexObjectStateOverrideiQCOM(System.Int32 target, System.Int32 pname, Int32 param);
        [Slot(151)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetDriverControlsQCOM([OutAttribute] Int32* num, Int32 size, [OutAttribute] UInt32* driverControls);
        [Slot(152)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern unsafe void glGetDriverControlStringQCOM(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] IntPtr driverControlString);
        [Slot(305)]
        [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern void glStartTilingQCOM(UInt32 x, UInt32 y, UInt32 width, UInt32 height, UInt32 preserveMask);    	
    }
}