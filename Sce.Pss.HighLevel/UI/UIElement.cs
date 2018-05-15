using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class UIElement : IDisposable
	{
		internal const float MinimumRenderableAlpah = 0.003921569f;

		internal const float MaximumOpaqueAlpah = 0.996078432f;

		private bool _needUpdateLocalToWorld = true;

		private float alpha = 1f;

		private TextureFilterMode textureFilterMode;

		private TextureWrapMode textureWrapMode;

		private ImageAsset image;

		private Texture2D texture;

		internal Matrix4 transform3D;

		private InternalShaderType internalShaderType;

		internal LinkedTree<UIElement> linkedTree;

		internal float finalAlpha = 1f;

		internal Matrix4 localToWorld;

		internal float zSortValue;

		internal UIElement nextZSortElement;

		internal bool NeedUpdateLocalToWorld
		{
			get
			{
				return this._needUpdateLocalToWorld;
			}
			set
			{
				if (!this._needUpdateLocalToWorld && value)
				{
					LinkedTree<UIElement> nextAsList = this.linkedTree.LastDescendant.NextAsList;
					for (LinkedTree<UIElement> nextAsList2 = this.linkedTree.NextAsList; nextAsList2 != nextAsList; nextAsList2 = nextAsList2.NextAsList)
					{
						nextAsList2.Value._needUpdateLocalToWorld = true;
					}
				}
				this._needUpdateLocalToWorld = value;
			}
		}

		internal bool Disposed
		{
			get;
			private set;
		}

		public float X
		{
			get
			{
				return this.transform3D.M41;
			}
			set
			{
				this.transform3D.M41 = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public float Y
		{
			get
			{
				return this.transform3D.M42;
			}
			set
			{
				this.transform3D.M42 = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public float Alpha
		{
			get
			{
				return this.alpha;
			}
			set
			{
				this.alpha = value;
			}
		}

		public bool Visible
		{
			get;
			set;
		}

		public TextureFilterMode TextureFilterMode
		{
			get
			{
				return this.textureFilterMode;
			}
			set
			{
				this.textureFilterMode = value;
			}
		}

		public TextureWrapMode TextureWrapMode
		{
			get
			{
				return this.textureWrapMode;
			}
			set
			{
				this.textureWrapMode = value;
			}
		}

		public ImageAsset Image
		{
			get
			{
				return this.image;
			}
			set
			{
				if (this.texture != null)
				{
					this.texture.Dispose();
					this.texture = null;
				}
				this.image = value;
			}
		}

		public bool ZSort
		{
			get;
			set;
		}

		public UIElement Parent
		{
			get
			{
				if (this.linkedTree.Parent != null)
				{
					return this.linkedTree.Parent.Value;
				}
				return null;
			}
		}

		public IEnumerable<UIElement> Children
		{
			get
			{
				LinkedTree<UIElement> linkedTree = this.linkedTree.FirstChild;
				while (linkedTree != null && linkedTree.Value != null)
				{
					yield return linkedTree.Value;
					linkedTree = linkedTree.NextSibling;
				}
				yield break;
			}
		}

		public BlendMode BlendMode
		{
			get;
			set;
		}

		public Matrix4 Transform3D
		{
			get
			{
				return this.transform3D;
			}
			set
			{
				this.transform3D = value;
				this.NeedUpdateLocalToWorld = true;
			}
		}

		public float ZSortOffset
		{
			get;
			set;
		}

		public bool Culling
		{
			get;
			set;
		}

		public ShaderType ShaderType
		{
			get
			{
				switch (this.internalShaderType)
				{
				case InternalShaderType.SolidFill:
					return ShaderType.SolidFill;
				case InternalShaderType.TextureAlpha:
					return ShaderType.TextTexture;
				case InternalShaderType.OffscreenTexture:
					return ShaderType.OffscreenTexture;
				}
				return ShaderType.Texture;
			}
			set
			{
				this.InternalShaderType = (InternalShaderType)value;
			}
		}

		internal InternalShaderType InternalShaderType
		{
			get
			{
				return this.internalShaderType;
			}
			set
			{
				if (this.internalShaderType != value)
				{
					this.ShaderUniforms.Clear();
					this.internalShaderType = value;
				}
			}
		}

		internal Dictionary<string, float[]> ShaderUniforms
		{
			get;
			set;
		}

		public Matrix4 LocalToWorld
		{
			get
			{
				this.updateLocalToWorld();
				return this.localToWorld;
			}
		}

		public UIElement()
		{
			this.linkedTree = new LinkedTree<UIElement>(this);
			this.Visible = true;
			this.BlendMode = BlendMode.Half;
			this.TextureFilterMode = (TextureFilterMode)1;
			this.TextureWrapMode = (TextureWrapMode)1;
			this.ZSort = false;
			this.transform3D = Matrix4.Identity;
			this.ZSortOffset = 0f;
			this.Culling = false;
			this.ShaderUniforms = new Dictionary<string, float[]>();
		}

		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.linkedTree != null)
			{
				this.linkedTree.RemoveChild();
				for (LinkedTree<UIElement> nextAsList = this.linkedTree; nextAsList != null; nextAsList = nextAsList.NextAsList)
				{
					if (nextAsList.Value != null)
					{
						nextAsList.Value.DisposeSelf();
					}
				}
			}
		}

		protected virtual void DisposeSelf()
		{
			if (this.texture != null)
			{
				this.texture.Dispose();
				this.texture = null;
			}
		}

		public void SetPosition(float x, float y)
		{
			this.transform3D.M41 = x;
			this.transform3D.M42 = y;
			this.NeedUpdateLocalToWorld = true;
		}

		protected Texture2D GetTexture()
		{
			if (this.texture == null && this.image != null)
			{
				this.texture = this.image.CloneTexture();
			}
			if (this.texture != null)
			{
				this.texture.SetFilter(this.TextureFilterMode);
				this.texture.SetWrap(this.TextureWrapMode);
			}
			return this.texture;
		}

		public void AddChildFirst(UIElement child)
		{
			this.linkedTree.AddChildFirst(child.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		public void AddChildLast(UIElement child)
		{
			this.linkedTree.AddChildLast(child.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		public void InsertChildBefore(UIElement child, UIElement nextChild)
		{
			child.linkedTree.InsertChildBefore(nextChild.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		public void InsertChildAfter(UIElement child, UIElement prevChild)
		{
			child.linkedTree.InsertChildAfter(prevChild.linkedTree);
			child.NeedUpdateLocalToWorld = true;
		}

		public void RemoveChild(UIElement child)
		{
			child.linkedTree.RemoveChild();
			child.NeedUpdateLocalToWorld = true;
		}

		protected internal virtual void SetupDrawState()
		{
			GraphicsContext graphicsContext = UISystem.GraphicsContext;
			if (UISystem.IsOffScreenRendering)
			{
				graphicsContext.Enable((EnableMode)4u, true);
				graphicsContext.SetBlendFuncRgb((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)5);
				graphicsContext.SetBlendFuncAlpha((BlendFuncMode)0, (BlendFuncFactor)0, (BlendFuncFactor)5);
			}
			else
			{
				switch (this.BlendMode)
				{
				case BlendMode.Half:
					graphicsContext.Enable((EnableMode)4u, true);
					graphicsContext.SetBlendFunc((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)5);
					break;
				case BlendMode.Add:
					graphicsContext.Enable((EnableMode)4u, true);
					graphicsContext.SetBlendFunc((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)1);
					break;
				case BlendMode.Premultiplied:
					graphicsContext.Enable((EnableMode)4u, true);
					graphicsContext.SetBlendFunc((BlendFuncMode)0, (BlendFuncFactor)1, (BlendFuncFactor)5);
					break;
				case BlendMode.Off:
					graphicsContext.Enable((EnableMode)4u, false);
					break;
				}
			}
			graphicsContext.Enable((EnableMode)2, this.Culling);
			graphicsContext.SetCullFace((CullFaceMode)2, (CullFaceDirection)1);
		}

		protected internal virtual void Render()
		{
		}

		protected internal virtual void SetupFinalAlpha()
		{
			this.finalAlpha = this.Alpha * this.Parent.finalAlpha;
		}

		[Obsolete("Use UISystem.ViewProjectionMatrix * LocalToWorld")]
		protected Matrix4 CalcLocalToClipMatrix()
		{
			return UISystem.ViewProjectionMatrix * this.LocalToWorld;
		}

		protected internal void SetupSortValue()
		{
			this.updateLocalToWorld();
			this.zSortValue = this.localToWorld.M43 + this.ZSortOffset;
		}

		internal virtual void updateLocalToWorld()
		{
			if (this._needUpdateLocalToWorld)
			{
				if (this.Parent != null)
				{
					this.Parent.updateLocalToWorld();
					this.Parent.localToWorld.Multiply(ref this.transform3D, out this.localToWorld);
				}
				else
				{
					this.localToWorld = this.transform3D;
				}
				this.NeedUpdateLocalToWorld = false;
			}
		}
	}
}
