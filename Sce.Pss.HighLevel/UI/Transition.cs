using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public abstract class Transition
	{
		internal TimeSpan startedTime;

		private bool isFristUpdate;

		public event EventHandler<EventArgs> TransitionStopped;

		public float TotalElapsedTime
		{
			get;
			private set;
		}

		protected TransitionDrawOrder DrawOrder
		{
			get;
			set;
		}

		protected Scene NextScene
		{
			get
			{
				return UISystem.NextScene;
			}
		}

		protected RootUIElement TransitionUIElement
		{
			get
			{
				return UISystem.TransitionUIElement;
			}
		}

		public Transition()
		{
		}

		internal void Start()
		{
			if (UISystem.NextScene == null)
			{
				return;
			}
			this.isFristUpdate = true;
			this.TotalElapsedTime = 0f;
			this.OnStart();
			UISystem.TransitionDrawOrder = this.DrawOrder;
		}

		internal void Stop()
		{
			this.OnStop();
			if (UISystem.NextScene != null)
			{
				UISystem.SetSceneInternal(UISystem.NextScene);
				UISystem.NextScene = null;
			}
			if (this.TransitionStopped != null)
			{
				this.TransitionStopped.Invoke(this, EventArgs.Empty);
			}
		}

		internal void Update(float elapsedTime)
		{
			if (this.isFristUpdate && UISystem.CurrentTime < this.startedTime)
			{
				return;
			}
			if (this.isFristUpdate)
			{
				this.isFristUpdate = false;
				elapsedTime = (float)(UISystem.CurrentTime - this.startedTime).TotalMilliseconds;
			}
			this.TotalElapsedTime += elapsedTime;
			TransitionUpdateResponse transitionUpdateResponse = this.OnUpdate(elapsedTime);
			if (transitionUpdateResponse == TransitionUpdateResponse.Finish)
			{
				this.Stop();
			}
		}

		protected abstract void OnStart();

		protected abstract TransitionUpdateResponse OnUpdate(float elapsedTime);

		protected abstract void OnStop();

		protected ImageAsset GetCurrentSceneRenderedImage()
		{
			if (UISystem.transitionCurrentSceneTextureCache == null)
			{
				UISystem.transitionCurrentSceneTextureCache = new Texture2D(UISystem.GraphicsContext.Screen.Width, UISystem.GraphicsContext.Screen.Height, false, (PixelFormat)1, (PixelBufferOption)1);
			}
			return this.getOffscreenImage(UISystem.CurrentScene, UISystem.transitionCurrentSceneTextureCache);
		}

		protected ImageAsset GetNextSceneRenderedImage()
		{
			if (UISystem.transitionNextSceneTextureCache == null)
			{
				UISystem.transitionNextSceneTextureCache = new Texture2D(UISystem.GraphicsContext.Screen.Width, UISystem.GraphicsContext.Screen.Height, false, (PixelFormat)1, (PixelBufferOption)1);
			}
			return this.getOffscreenImage(UISystem.NextScene, UISystem.transitionNextSceneTextureCache);
		}

		private ImageAsset getOffscreenImage(Scene scene, Texture2D texture)
		{
			FrameBuffer offScreenFramebufferCache = UISystem.offScreenFramebufferCache;
			offScreenFramebufferCache.SetColorTarget(texture, 0);
			scene.Update(0f);
			Matrix4 identity = Matrix4.Identity;
			scene.RootWidget.RenderToFrameBuffer(offScreenFramebufferCache, ref identity, false);
			return new ImageAsset(texture)
			{
				AdjustScaledSize = true
			};
		}
	}
}
