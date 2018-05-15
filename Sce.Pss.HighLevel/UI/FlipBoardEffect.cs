using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FlipBoardEffect : Effect
	{
		private float time = 720f;

		private AnimationInterpolator interpolatorCallback;

		private UISprite currentUpperSprt;

		private UISprite currentLowerSprt;

		private UISprite nextUpperSprt;

		private UISprite nextLowerSprt;

		private float degree;

		private Vector2 centerPosition;

		private float firstZOffset = 0.001f;

		private float secondZOffset;

		private float turnupTime;

		public Widget NextWidget
		{
			get;
			set;
		}

		public FlipBoardEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public FlipBoardEffect()
		{
			base.Widget = null;
			this.NextWidget = null;
		}

		public FlipBoardEffect(Widget currentWidget, Widget nextWidget)
		{
			base.Widget = currentWidget;
			this.NextWidget = nextWidget;
		}

		public static FlipBoardEffect CreateAndStart(Widget currentWidget, Widget nextWidget)
		{
			FlipBoardEffect flipBoardEffect = new FlipBoardEffect(currentWidget, nextWidget);
			flipBoardEffect.Start();
			return flipBoardEffect;
		}

		protected override void OnStart()
		{
			if (this.Interpolator == FlipBoardEffectInterpolator.Custom && this.CustomInterpolator != null)
			{
				this.interpolatorCallback = this.CustomInterpolator;
			}
			else
			{
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.FlipBounceInterpolator);
			}
			int num;
			int num2;
			int num3;
			int num4;
			if (UISystem.Scaled)
			{
				num = (int)(base.Widget.Width * UISystem.Scale);
				num2 = (int)(base.Widget.Height * UISystem.Scale);
				num3 = (int)(this.NextWidget.Width * UISystem.Scale);
				num4 = (int)(this.NextWidget.Height * UISystem.Scale);
			}
			else
			{
				num = (int)base.Widget.Width;
				num2 = (int)base.Widget.Height;
				num3 = (int)this.NextWidget.Width;
				num4 = (int)this.NextWidget.Height;
			}
			if (!UISystem.CheckTextureSizeCapacity(num, num2) || !UISystem.CheckTextureSizeCapacity(num3, num4))
			{
				throw new ArgumentOutOfRangeException();
			}
			Texture2D texture2D = new Texture2D(num, num2, false, (PixelFormat)1, (PixelBufferOption)1);
			base.Widget.RenderToTexture(texture2D);
			ImageAsset imageAsset = new ImageAsset(texture2D);
			imageAsset.AdjustScaledSize = true;
			texture2D.Dispose();
			Texture2D texture2D2 = new Texture2D(num3, num4, false, (PixelFormat)1, (PixelBufferOption)1);
			this.NextWidget.RenderToTexture(texture2D2);
			ImageAsset imageAsset2 = new ImageAsset(texture2D2);
			imageAsset2.AdjustScaledSize = true;
			texture2D2.Dispose();
			float width = base.Widget.Width;
			float num5 = base.Widget.Height / 2f;
			float width2 = this.NextWidget.Width;
			float num6 = this.NextWidget.Height / 2f;
			PivotType pivotType = base.Widget.PivotType;
			base.Widget.PivotType = PivotType.MiddleCenter;
			this.centerPosition = new Vector2(base.Widget.X, base.Widget.Y);
			base.Widget.PivotType = pivotType;
			pivotType = this.NextWidget.PivotType;
			this.NextWidget.PivotType = PivotType.MiddleCenter;
			this.NextWidget.Transform3D = Matrix4.Translation(this.centerPosition.X, this.centerPosition.Y, 0f);
			this.NextWidget.PivotType = pivotType;
			this.currentUpperSprt = new UISprite(1);
			this.currentUpperSprt.X = this.centerPosition.X;
			this.currentUpperSprt.Y = this.centerPosition.Y - num5 * 0.5f;
			this.currentUpperSprt.Image = imageAsset;
			this.currentUpperSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentUpperSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit = this.currentUpperSprt.GetUnit(0);
			unit.X = -width * 0.5f;
			unit.Y = -num5 * 0.5f;
			unit.Width = width;
			unit.Height = num5;
			unit.V2 = 0.5f;
			this.currentLowerSprt = new UISprite(1);
			this.currentLowerSprt.X = this.centerPosition.X;
			this.currentLowerSprt.Y = this.centerPosition.Y + num5 * 0.5f;
			this.currentLowerSprt.Image = imageAsset;
			this.currentLowerSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentLowerSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit2 = this.currentLowerSprt.GetUnit(0);
			unit2.X = -width * 0.5f;
			unit2.Y = -num5 * 0.5f;
			unit2.Width = width;
			unit2.Height = num5;
			unit2.V1 = 0.5f;
			this.nextUpperSprt = new UISprite(1);
			this.nextUpperSprt.X = this.centerPosition.X;
			this.nextUpperSprt.Y = this.centerPosition.Y - num6 * 0.5f;
			this.nextUpperSprt.Image = imageAsset2;
			this.nextUpperSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextUpperSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit3 = this.nextUpperSprt.GetUnit(0);
			unit3.X = -width2 * 0.5f;
			unit3.Y = -num6 * 0.5f;
			unit3.Width = width2;
			unit3.Height = num6;
			unit3.V2 = 0.5f;
			this.nextLowerSprt = new UISprite(1);
			this.nextLowerSprt.X = this.centerPosition.X;
			this.nextLowerSprt.Y = this.centerPosition.Y + num6 * 0.5f;
			this.nextLowerSprt.Image = imageAsset2;
			this.nextLowerSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextLowerSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit4 = this.nextLowerSprt.GetUnit(0);
			unit4.X = -width2 * 0.5f;
			unit4.Y = -num6 * 0.5f;
			unit4.Width = width2;
			unit4.Height = num6;
			unit4.V1 = 0.5f;
			base.Widget.Parent.RootUIElement.AddChildLast(this.currentLowerSprt);
			this.NextWidget.Parent.RootUIElement.AddChildLast(this.nextUpperSprt);
			base.Widget.Parent.RootUIElement.AddChildLast(this.currentUpperSprt);
			this.NextWidget.Parent.RootUIElement.AddChildLast(this.nextLowerSprt);
			this.degree = 0f;
			this.turnupTime = this.Time;
			this.secondZOffset = num5 / 4f;
			base.Widget.Visible = false;
			this.NextWidget.Visible = false;
			this.currentUpperSprt.Culling = true;
			this.nextLowerSprt.Culling = true;
			this.currentUpperSprt.ZSort = true;
			this.currentLowerSprt.ZSort = true;
			this.nextUpperSprt.ZSort = true;
			this.nextLowerSprt.ZSort = true;
			this.currentUpperSprt.ZSortOffset = -this.secondZOffset - this.firstZOffset;
			this.currentLowerSprt.ZSortOffset = 0f;
			this.nextUpperSprt.ZSortOffset = -this.firstZOffset;
			this.nextLowerSprt.ZSortOffset = -this.secondZOffset - this.firstZOffset;
			this.RotateNextLowerSprite();
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			float num = this.degree;
			this.degree = this.interpolatorCallback(0f, 180f, base.TotalElapsedTime / this.Time);
			this.degree = MathUtility.Clamp<float>(this.degree, 0f, 180f);
			if (this.degree < num && this.turnupTime > base.TotalElapsedTime)
			{
				this.turnupTime = base.TotalElapsedTime;
			}
			this.UpdateWidgetVisible();
			if (base.TotalElapsedTime >= this.time)
			{
				return EffectUpdateResponse.Finish;
			}
			return EffectUpdateResponse.Continue;
		}

		protected override void OnStop()
		{
			this.NextWidget.Visible = true;
			base.Widget.Visible = false;
			if (this.currentUpperSprt != null)
			{
				this.currentUpperSprt.Image.Dispose();
				this.currentUpperSprt.Dispose();
				this.currentUpperSprt = null;
			}
			if (this.currentLowerSprt != null)
			{
				this.currentLowerSprt.Image.Dispose();
				this.currentLowerSprt.Dispose();
				this.currentLowerSprt = null;
			}
			if (this.nextUpperSprt != null)
			{
				this.nextUpperSprt.Image.Dispose();
				this.nextUpperSprt.Dispose();
				this.nextUpperSprt = null;
			}
			if (this.nextLowerSprt != null)
			{
				this.nextLowerSprt.Image.Dispose();
				this.nextLowerSprt.Dispose();
				this.nextLowerSprt = null;
			}
		}

		private void UpdateWidgetVisible()
		{
			this.RotateCurrentUpperSprite();
			this.RotateNextLowerSprite();
			this.currentLowerSprt.ZSortOffset = -base.TotalElapsedTime / this.Time * this.firstZOffset;
			this.nextUpperSprt.ZSortOffset = -this.firstZOffset - this.currentLowerSprt.ZSortOffset;
			if (base.TotalElapsedTime > this.turnupTime)
			{
				this.nextLowerSprt.ZSortOffset = -this.nextLowerSprt.Transform3D.M43 - ((this.Time - base.TotalElapsedTime) / (this.Time - this.turnupTime) * this.secondZOffset + this.firstZOffset);
			}
		}

		private void RotateCurrentUpperSprite()
		{
			float num = this.degree / 360f * 2f * 3.14159274f;
			UISpriteUnit unit = this.currentUpperSprt.GetUnit(0);
			Matrix4 matrix = Matrix4.Translation(0f, -unit.Height * 0.5f, 0f);
			Matrix4 matrix2 = Matrix4.RotationX(num);
			Matrix4 matrix3 = Matrix4.Translation(this.centerPosition.X, this.centerPosition.Y, 0f);
			this.currentUpperSprt.Transform3D = matrix3 * matrix2 * matrix;
		}

		private void RotateNextLowerSprite()
		{
			float num = (this.degree - 180f) / 360f * 2f * 3.14159274f;
			UISpriteUnit unit = this.nextLowerSprt.GetUnit(0);
			Matrix4 matrix = Matrix4.Translation(0f, unit.Height * 0.5f, 0f);
			Matrix4 matrix2 = Matrix4.RotationX(num);
			Matrix4 matrix3 = Matrix4.Translation(this.centerPosition.X, this.centerPosition.Y, 0f);
			this.nextLowerSprt.Transform3D = matrix3 * matrix2 * matrix;
		}
	}
}
