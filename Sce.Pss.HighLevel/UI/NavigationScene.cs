using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.UI
{
	internal class NavigationScene : Scene
	{
		private static readonly int navigationBarFontSize = 28;

		private static readonly float navigationBarHeight = 66f;

		private static readonly float backButtonWidth = 200f;

		private static readonly float backButtonHeight = 56f;

		private static readonly float stackAnimationTime = 500f;

		private static readonly float showHideAnimationDuration = 500f;

		private Panel navigationPanel;

		private ImageBox backgroundImage;

		private Button backButtonCurrent;

		private Button backButtonNext;

		private Label labelCurrent;

		private Label labelNext;

		private float leftLabelPosX;

		private float centerLabelPosX;

		private float rightLabelPosX;

		private MoveEffect panelShowMoveEffect;

		private MoveEffect panelHideMoveEffect;

		private MoveEffect labelCurrentMoveEffect;

		private FadeOutEffect labelCurrentFadeOutEffect;

		private MoveEffect labelNextMoveEffect;

		private FadeInEffect labelNextFadeInEffect;

		private FadeOutEffect backButtonCurrentFadeOutEffect;

		private FadeInEffect backButtonNextFadeInEffect;

		public NavigationScene()
		{
			base.Visible = false;
			this.navigationPanel = new Panel
			{
				Width = (float)UISystem.FramebufferWidth,
				Height = NavigationScene.navigationBarHeight,
				X = 0f,
				Y = -NavigationScene.navigationBarHeight,
				BackgroundColor = new UIColor(0f, 0f, 0f, 0f)
			};
			base.RootWidget.AddChildLast(this.navigationPanel);
			this.backgroundImage = new ImageBox
			{
				Width = this.navigationPanel.Width,
				Height = this.navigationPanel.Height,
				X = 0f,
				Y = 0f,
				ImageScaleType = ImageScaleType.NinePatch,
				Image = new ImageAsset(SystemImageAsset.NavigationBarBackground),
				NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.NavigationBarBackground)
			};
			this.navigationPanel.AddChildLast(this.backgroundImage);
			CustomButtonImageSettings customImage = new CustomButtonImageSettings
			{
				BackgroundNormalImage = new ImageAsset(SystemImageAsset.BackButtonBackgroundNormal),
				BackgroundPressedImage = new ImageAsset(SystemImageAsset.BackButtonBackgroundPressed),
				BackgroundDisabledImage = new ImageAsset(SystemImageAsset.BackButtonBackgroundDisabled),
				BackgroundNinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.BackButtonBackgroundNormal)
			};
			float num = (NavigationScene.navigationBarHeight - NavigationScene.backButtonHeight) / 2f;
			Font font = new Font(0, NavigationScene.navigationBarFontSize, 0);
			this.backButtonCurrent = new Button
			{
				Width = NavigationScene.backButtonWidth,
				Height = NavigationScene.backButtonHeight,
				X = num,
				Y = num,
				TextFont = font,
				Style = ButtonStyle.Custom,
				CustomImage = customImage,
				Visible = false
			};
			this.backButtonCurrent.ButtonAction += new EventHandler<TouchEventArgs>(NavigationScene.PopAction);
			this.navigationPanel.AddChildLast(this.backButtonCurrent);
			this.backButtonNext = new Button
			{
				Width = NavigationScene.backButtonWidth,
				Height = NavigationScene.backButtonHeight,
				X = num,
				Y = num,
				TextFont = font,
				Style = ButtonStyle.Custom,
				CustomImage = customImage,
				Visible = false
			};
			this.backButtonNext.ButtonAction += new EventHandler<TouchEventArgs>(NavigationScene.PopAction);
			this.navigationPanel.AddChildLast(this.backButtonNext);
			float num2 = this.navigationPanel.Width - NavigationScene.backButtonWidth * 2f - num * 2f;
			this.leftLabelPosX = -num2;
			this.centerLabelPosX = NavigationScene.backButtonWidth + num;
			this.rightLabelPosX = (float)UISystem.FramebufferWidth;
			this.labelCurrent = new Label
			{
				Width = num2,
				Height = this.navigationPanel.Height,
				X = this.centerLabelPosX,
				Y = 0f,
				Font = font,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle
			};
			this.navigationPanel.AddChildLast(this.labelCurrent);
			this.labelNext = new Label
			{
				Width = num2,
				Height = this.navigationPanel.Height,
				X = this.rightLabelPosX,
				Y = 0f,
				Font = font,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle,
				Visible = false
			};
			this.navigationPanel.AddChildLast(this.labelNext);
			this.panelShowMoveEffect = new MoveEffect
			{
				Widget = this.navigationPanel,
				Interpolator = MoveEffectInterpolator.Custom,
				CustomInterpolator = new AnimationInterpolator(NavigationScene.EaseOutQuartInterpolator),
				Time = NavigationScene.showHideAnimationDuration,
				X = 0f,
				Y = 0f
			};
			this.panelHideMoveEffect = new MoveEffect
			{
				Widget = this.navigationPanel,
				Interpolator = MoveEffectInterpolator.Custom,
				CustomInterpolator = new AnimationInterpolator(NavigationScene.EaseOutQuartInterpolator),
				Time = NavigationScene.showHideAnimationDuration,
				X = 0f,
				Y = -NavigationScene.navigationBarHeight
			};
			this.panelHideMoveEffect.EffectStopped += new EventHandler<EventArgs>(this.OnPanelHideMoveEffectStopped);
			this.labelCurrentMoveEffect = new MoveEffect
			{
				Interpolator = MoveEffectInterpolator.Custom,
				CustomInterpolator = new AnimationInterpolator(NavigationScene.EaseOutQuartInterpolator),
				Time = NavigationScene.stackAnimationTime
			};
			this.labelCurrentMoveEffect.EffectStopped += new EventHandler<EventArgs>(this.OnLabelMoveEffectStopped);
			this.labelCurrentFadeOutEffect = new FadeOutEffect
			{
				Interpolator = FadeOutEffectInterpolator.Linear,
				Time = NavigationScene.stackAnimationTime
			};
			this.labelNextMoveEffect = new MoveEffect
			{
				Interpolator = MoveEffectInterpolator.Custom,
				CustomInterpolator = new AnimationInterpolator(NavigationScene.EaseOutQuartInterpolator),
				Time = NavigationScene.stackAnimationTime
			};
			this.labelNextFadeInEffect = new FadeInEffect
			{
				Interpolator = FadeInEffectInterpolator.Linear,
				Time = NavigationScene.stackAnimationTime
			};
			this.backButtonCurrentFadeOutEffect = new FadeOutEffect
			{
				Interpolator = FadeOutEffectInterpolator.Linear,
				Time = NavigationScene.stackAnimationTime
			};
			this.backButtonNextFadeInEffect = new FadeInEffect
			{
				Interpolator = FadeInEffectInterpolator.Linear,
				Time = NavigationScene.stackAnimationTime
			};
		}

		internal void Show(bool animation, string title)
		{
			if (this.panelShowMoveEffect.Playing || (!this.panelHideMoveEffect.Playing && base.Visible))
			{
				return;
			}
			if (this.panelHideMoveEffect.Playing)
			{
				this.panelHideMoveEffect.Stop();
			}
			base.Visible = true;
			this.labelCurrent.Text = title;
			if (animation)
			{
				this.panelShowMoveEffect.Start();
			}
		}

		internal void Hide(bool animation)
		{
			if (this.panelHideMoveEffect.Playing || !base.Visible)
			{
				return;
			}
			if (this.panelShowMoveEffect.Playing)
			{
				this.panelShowMoveEffect.Stop();
			}
			if (animation)
			{
				base.Visible = true;
				this.panelHideMoveEffect.Start();
				return;
			}
			base.Visible = false;
		}

		internal void StartPushAnimation(string backSceneTitle, string newSceneTitle)
		{
			if (this.labelCurrentMoveEffect.Playing)
			{
				this.labelCurrentMoveEffect.Stop();
			}
			this.labelCurrentMoveEffect.Widget = this.labelCurrent;
			this.labelCurrentMoveEffect.X = this.leftLabelPosX;
			this.labelCurrentMoveEffect.Y = this.labelCurrent.Y;
			this.labelCurrentMoveEffect.Start();
			this.labelNext.X = this.rightLabelPosX;
			this.labelNext.Text = newSceneTitle;
			this.labelNextMoveEffect.Widget = this.labelNext;
			this.labelNextMoveEffect.X = this.centerLabelPosX;
			this.labelNextMoveEffect.Y = this.labelNext.Y;
			this.labelNextMoveEffect.Start();
			this.PrepareLabelFadeAnimation();
			this.PrepareButtonFadeAnimtion(backSceneTitle);
		}

		internal void StartPopAnimation(string backSceneTitle, string newSceneTitle)
		{
			if (this.labelCurrentMoveEffect.Playing)
			{
				this.labelCurrentMoveEffect.Stop();
			}
			this.labelCurrentMoveEffect.Widget = this.labelCurrent;
			this.labelCurrentMoveEffect.X = this.rightLabelPosX;
			this.labelCurrentMoveEffect.Y = this.labelCurrent.Y;
			this.labelCurrentMoveEffect.Start();
			this.labelNext.X = this.leftLabelPosX;
			this.labelNext.Text = newSceneTitle;
			this.labelNextMoveEffect.Widget = this.labelNext;
			this.labelNextMoveEffect.X = this.centerLabelPosX;
			this.labelNextMoveEffect.Y = this.labelNext.Y;
			this.labelNextMoveEffect.Start();
			this.PrepareLabelFadeAnimation();
			this.PrepareButtonFadeAnimtion(backSceneTitle);
		}

		private void PrepareLabelFadeAnimation()
		{
			this.labelCurrentFadeOutEffect.Widget = this.labelCurrent;
			this.labelCurrentFadeOutEffect.Start();
			this.labelNextFadeInEffect.Widget = this.labelNext;
			this.labelNextFadeInEffect.Start();
		}

		private void PrepareButtonFadeAnimtion(string backSceneTitle)
		{
			if (this.backButtonCurrent.Visible)
			{
				this.backButtonCurrentFadeOutEffect.Widget = this.backButtonCurrent;
				this.backButtonCurrentFadeOutEffect.Start();
			}
			if (backSceneTitle != null)
			{
				this.backButtonNextFadeInEffect.Widget = this.backButtonNext;
				this.backButtonNextFadeInEffect.Start();
				this.backButtonNext.Text = backSceneTitle;
			}
		}

		private static void PopAction(object sender, TouchEventArgs e)
		{
			UISystem.PopScene();
		}

		private static float EaseOutQuartInterpolator(float from, float to, float ratio)
		{
			return AnimationUtility.EaseOutQuartInterpolator(from, to, ratio);
		}

		private void OnPanelHideMoveEffectStopped(object sender, EventArgs e)
		{
			base.Visible = false;
		}

		private void OnLabelMoveEffectStopped(object sender, EventArgs e)
		{
			Label label = this.labelCurrent;
			this.labelCurrent = this.labelNext;
			this.labelNext = label;
			Button button = this.backButtonCurrent;
			this.backButtonCurrent = this.backButtonNext;
			this.backButtonNext = button;
			this.labelNext.Visible = false;
			this.backButtonNext.Visible = false;
		}
	}
}
