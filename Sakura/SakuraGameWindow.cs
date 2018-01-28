using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK;
using OpenTK.Graphics.ES20;

//OnLoad
//OnResize
//OnUpdateFrame
//OnRenderFrame
namespace Sakura
{
	public class SakuraGameWindowContext : IDisposable
	{
        public IGraphicsContext glContext;
        public bool isExiting = false;
        
       	//-----------------------------------------------------
		
        public GameWindowFlags options;
        public DisplayDevice device;
        public INativeWindow implementation;
        public bool disposed, events;

        //-----------------------------------------------------	

		public void Dispose()
        {
			try
            {
                if (this.glContext != null)
                {
                    this.glContext.Dispose();
                    this.glContext = null;
                }
            }
            finally
            {
	        	if (!disposed)
	            {
	                if ((this.options & GameWindowFlags.Fullscreen) != 0)
	                {
	                    if (this.device != null)
	                    {
	                        this.device.RestoreResolution();
	                    }
	                }
	                this.implementation.Dispose();
	                GC.SuppressFinalize(this);
	
	                disposed = true;
	            }
            }
    	}        
		//
		
		public bool isLoopFirst = true;
		public WindowState lastState = WindowState.Normal;
	}

	public class SakuraGameWindow
    {
       public static void Init()
        {
        	ToolkitOptions optionsToolkit = new ToolkitOptions()
			{
                Backend = PlatformBackend.PreferNative //for eglSwapBuffers to SwapBuffers
            };
            Toolkit.Init(optionsToolkit);
            
        	GraphicsContextFlags flags = GraphicsContextFlags.Embedded;
        	int width = 854;//800;
			int height = 480;//600;
			GraphicsMode mode = GraphicsMode.Default;
			string title = "PlayStation Suite Simulator"; //"Simple ES 2.0";
			GameWindowFlags options = GameWindowFlags.FixedWindow; //GameWindowFlags.Default;
			DisplayDevice device = DisplayDevice.Default;
			int major = 2;
			int minor = 0;
			
			if (mode == null) mode = GraphicsMode.Default;
			if (device == null) device = DisplayDevice.Default;
        	int x = (device != null ? device.Bounds.Left + (device.Bounds.Width - width) / 2 : 0);
        	int y = (device != null ? device.Bounds.Top + (device.Bounds.Height - height) / 2 : 0);
            
        	if (width < 1)
                throw new ArgumentOutOfRangeException("width", "Must be greater than zero.");
            if (height < 1)
                throw new ArgumentOutOfRangeException("height", "Must be greater than zero.");
            if (mode == null)
                throw new ArgumentNullException("mode");

            g_ctx.options = options;
            g_ctx.device = device;

            IPlatformFactory factory = Factory.Default;
            g_ctx.implementation = factory.CreateNativeWindow(x, y, width, height, title, mode, options, g_ctx.device);
            factory.RegisterResource(g_ctx);

            if ((options & GameWindowFlags.Fullscreen) != 0)
            {
                if (g_ctx.device != null)
                {
                    g_ctx.device.ChangeResolution(width, height, mode.ColorFormat.BitsPerPixel, 0);
                }
                setWindowState(WindowState.Fullscreen);
            }

            if ((options & GameWindowFlags.FixedWindow) != 0)
            {
            	setWindowBorder(WindowBorder.Fixed);
            }         
			
            try
            {
            	g_ctx.glContext = new GraphicsContext(mode == null ? GraphicsMode.Default : mode, getWindowInfo(), major, minor, flags);
            	g_ctx.glContext.MakeCurrent(getWindowInfo());
                (g_ctx.glContext as IGraphicsContextInternal).LoadAll();

                setVSync(VSyncMode.On);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                baseDispose();
                throw;
            }
            
            Run_1();
        }
		
		
		//-------------------------------------------------------------------------
        
       	private static SakuraGameWindowContext g_ctx = new SakuraGameWindowContext();
		
        public static void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                try
                {
                    if (g_ctx.glContext != null)
                    {
                        g_ctx.glContext.Dispose();
                        g_ctx.glContext = null;
                    }
                }
                finally
                {
                    baseDispose();
                }
            }
            //GC.SuppressFinalize(this);
        }

        public static void Exit()
        {
            Close();
        }
        
        private static void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel)
            {
            	g_ctx.isExiting = true;
                OnUnloadInternal(EventArgs.Empty);
            }
        }
        
        private static void OnUnload(EventArgs e) { }
        
        public static void Run_1()
        {
            EnsureUndisposed();
            setVisible(true);
        }

        public static void SwapBuffers()
        {
            EnsureUndisposed();
            getContext().SwapBuffers();
        }

        public static IGraphicsContext getContext()
        {
            EnsureUndisposed();
            return g_ctx.glContext;
        }
        
        public static bool getIsExiting()
        {
            EnsureUndisposed();
            return g_ctx.isExiting;
        }

        public static VSyncMode getVSync()
        {
            EnsureUndisposed();
            GraphicsContext.Assert();
            if (getContext().SwapInterval < 0)
            {
                return VSyncMode.Adaptive;
            }
            else if (getContext().SwapInterval == 0)
            {
                return VSyncMode.Off;
            }
            else
            {
                return VSyncMode.On;
            }
        }
        public static void setVSync(VSyncMode v)
        {
            EnsureUndisposed();
            GraphicsContext.Assert();
            switch (v)
            {
                case VSyncMode.On:
                    getContext().SwapInterval = 1;
                    break;

                case VSyncMode.Off:
                    getContext().SwapInterval = 0;
                    break;

                case VSyncMode.Adaptive:
                    getContext().SwapInterval = -1;
                    break;
            }
        }

        public static WindowState getWindowState() { return getBaseWindowState(); }
        public static void setWindowState(WindowState v)
        {
        	setBaseWindowState(v);
            Debug.Print("Updating Context after setting WindowState to {0}", v);

            if (getContext() != null)
            	getContext().Update(getWindowInfo());
        }
        private static void Dispose(bool manual) { }
        public static void OnBaseResize() { g_ctx.glContext.Update(getWindowInfo()); } 
        private static void OnUnloadInternal(EventArgs e) { OnUnload(e); }
        
		//-----------------------------------------------------

        public static void Close() { EnsureUndisposed(); g_ctx.implementation.Close(); }
        public static void ProcessEvents() 
        {
        	if (g_ctx.isLoopFirst)
        	{
	            OnResize();
	
	            Debug.Print("Entering main loop.");
	            g_ctx.isLoopFirst = false;
        	}
        	ProcessEvents(false); 
        	
            if (!(getExists() && !getIsExiting()))
            {
            	Dispose();
            	//return;
            	Environment.Exit(0);
            }
        }
        public static bool getExists() { return getIsDisposed() ? false : g_ctx.implementation.Exists; }
        public static bool getFocused() { EnsureUndisposed(); return g_ctx.implementation.Focused; }
        public static int getHeight() { EnsureUndisposed(); return g_ctx.implementation.Height; }
        public static void setHeight(int v) { EnsureUndisposed(); g_ctx.implementation.Height = v; }
        public static string getTitle() { EnsureUndisposed(); return g_ctx.implementation.Title; }
        public static void setTitle(string v) { EnsureUndisposed(); g_ctx.implementation.Title = v; }
        public static bool getVisible() {EnsureUndisposed(); return g_ctx.implementation.Visible; }
        public static void setVisible(bool v) { EnsureUndisposed(); g_ctx.implementation.Visible = v;}
        public static int getWidth() {EnsureUndisposed(); return g_ctx.implementation.Width; }
        public static void setWidth(int v) {EnsureUndisposed(); g_ctx.implementation.Width = v; }
        public static WindowBorder getWindowBorder() { return g_ctx.implementation.WindowBorder; }
        public static void setWindowBorder(WindowBorder v) { g_ctx.implementation.WindowBorder = v; }
        public static IWindowInfo getWindowInfo() {EnsureUndisposed(); return g_ctx.implementation.WindowInfo;}
        public static WindowState getBaseWindowState() {return g_ctx.implementation.WindowState;}
        public static void setBaseWindowState(WindowState v) { g_ctx.implementation.WindowState = v; }
        public static int getX() { EnsureUndisposed(); return g_ctx.implementation.X; }
        public static void setX(int v) { EnsureUndisposed(); g_ctx.implementation.X = v; }
        public static int getY() { EnsureUndisposed(); return g_ctx.implementation.Y; }
        public static void setY(int v) { EnsureUndisposed(); g_ctx.implementation.Y = v; }
        public static Point PointToClient(Point pt) { EnsureUndisposed(); return g_ctx.implementation.PointToClient(pt); }
        
        public static void baseDispose()
        {
        	if (!getIsDisposed())
            {
                if ((g_ctx.options & GameWindowFlags.Fullscreen) != 0)
                {
                    if (g_ctx.device != null)
                    {
                        g_ctx.device.RestoreResolution();
                    }
                }
                g_ctx.implementation.Dispose();
                //GC.SuppressFinalize(this);

                setIsDisposed(true);
            }
        }

        private static void EnsureUndisposed() { if (getIsDisposed()) throw new ObjectDisposedException(g_ctx.GetType().Name); }
        private static bool getIsDisposed() { return g_ctx.disposed; }
        private static void setIsDisposed(bool v) { g_ctx.disposed = v; }
        private static void ProcessEvents(bool retainEvents)
        {
            EnsureUndisposed();
            if (!retainEvents && !g_ctx.events) setEvents(true);
            g_ctx.implementation.ProcessEvents();
        }
        private static void OnClosedInternal(object sender, EventArgs e) { setEvents(false); }
        private static void OnClosingInternal(object sender, CancelEventArgs e) { OnClosing(e); }
        private static void OnResizeInternal(object sender, EventArgs e) { OnResize(); }
        private static void OnWindowStateInternal(object sender, EventArgs e) 
        { 
        	setWidth(getWidth() + 1);
        	setWidth(getWidth() - 1);
        	//Debug.WriteLine("width:" + getWidth());
        	WindowState windowState = getWindowState();
        	//Debug.WriteLine("===============>windowState: " + windowState);
        	if (g_ctx.lastState == WindowState.Minimized && windowState == WindowState.Normal)
        	{
        		//object sender, TEventArgs e
        		Restore(null, EventArgs.Empty);
        	}
        	g_ctx.lastState = windowState;
        }
		public static event EventHandler<EventArgs> Restore;
        
        private static void setEvents(bool v)
        {
            if (v)
            {
                if (g_ctx.events)
                {
                    throw new InvalidOperationException("Event propagation is already enabled.");
                }
                g_ctx.implementation.Closed += OnClosedInternal;
                g_ctx.implementation.Closing += OnClosingInternal;
                g_ctx.implementation.Resize += OnResizeInternal;
                g_ctx.implementation.WindowStateChanged += OnWindowStateInternal;
                g_ctx.events = true;
            }
            else if (g_ctx.events)
            {
                g_ctx.implementation.Closed -= OnClosedInternal;
                g_ctx.implementation.Closing -= OnClosingInternal;
                g_ctx.implementation.Resize -= OnResizeInternal;
                g_ctx.implementation.WindowStateChanged -= OnWindowStateInternal;
                
                g_ctx.events = false;
            }
            else
            {
                throw new InvalidOperationException("Event propagation is already disabled.");
            }
        }
        
        //-------------------------------------------------------------------------
       
        
        /// <summary>
        /// Called when the user resizes the window.
        /// </summary>
        /// <param name="e">Contains the new width/height of the window.</param>
        /// <remarks>
        /// You want the OpenGL viewport to match the window. This is the place to do it!
        /// </remarks>
        public static void OnResize()
        {
        	OnBaseResize();
        	int w = getWidth();
        	int h = getHeight();
        	GL.Viewport(0, 0, getWidth(), getHeight());
        }
    }
}
