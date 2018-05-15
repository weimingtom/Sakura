using System;

namespace Sce.Pss.HighLevel.UI
{
	public class ListPanelItem : Panel
	{
		private const float edgeHeight = 1f;

		private const float defaultWidth = 200f;

		private const float defaultHeight = 60f;

		private static NinePatchMargin margin = AssetManager.GetNinePatchMargin(SystemImageAsset.ListPanelSeparatorTop);

		private int index;

		private bool hideEdge = true;

		private UISprite upperEdgeSprt;

		private UISprite lowerEdgeSprt;

		private ContainerWidget listItemContainer;

		public override float Width
		{
			get
			{
				return this.listItemContainer.Width;
			}
			set
			{
				base.Width = value;
				if (this.listItemContainer != null)
				{
					this.listItemContainer.Width = value;
				}
				if (!this.hideEdge)
				{
					this.SetupSprite();
				}
			}
		}

		public override float Height
		{
			get
			{
				return this.listItemContainer.Height;
			}
			set
			{
				if (this.listItemContainer != null)
				{
					this.listItemContainer.Height = value;
				}
				base.Height = value;
				if (!this.hideEdge && this.upperEdgeSprt != null && this.lowerEdgeSprt != null)
				{
					this.lowerEdgeSprt.Y = base.Height - 1f;
					this.SetupSprite();
				}
			}
		}

		public virtual int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		public virtual int IndexInSection
		{
			get;
			set;
		}

		public int SectionIndex
		{
			get;
			set;
		}

		internal float ContainEdgeHeight
		{
			get
			{
				if (this.HideEdge)
				{
					return this.listItemContainer.Height;
				}
				return base.Height;
			}
		}

		internal bool HideEdge
		{
			get
			{
				return this.hideEdge;
			}
			set
			{
				if (this.hideEdge != value)
				{
					this.hideEdge = value;
					if (this.upperEdgeSprt != null && this.lowerEdgeSprt != null)
					{
						this.upperEdgeSprt.Visible = !this.hideEdge;
						this.lowerEdgeSprt.Visible = !this.hideEdge;
					}
					if (!this.hideEdge)
					{
						this.SetupSprite();
					}
				}
			}
		}

		public ListPanelItem()
		{
			this.listItemContainer = new ContainerWidget();
			this.listItemContainer.Y = 0f;
			this.listItemContainer.PriorityHit = true;
			base.AddChildLast(this.listItemContainer);
			this.index = -1;
			this.SectionIndex = -1;
			this.IndexInSection = -1;
			this.Width = 200f;
			this.Height = 60f;
			this.PriorityHit = true;
		}

		protected override void DisposeSelf()
		{
			if (this.upperEdgeSprt != null && this.upperEdgeSprt.Image != null)
			{
				this.upperEdgeSprt.Image.Dispose();
			}
			if (this.lowerEdgeSprt != null && this.lowerEdgeSprt.Image != null)
			{
				this.lowerEdgeSprt.Image.Dispose();
			}
			base.DisposeSelf();
		}

		private void SetupSprite()
		{
			if (this.upperEdgeSprt == null || this.lowerEdgeSprt == null)
			{
				this.upperEdgeSprt = new UISprite(3);
				this.listItemContainer.RootUIElement.AddChildLast(this.upperEdgeSprt);
				this.upperEdgeSprt.Image = new ImageAsset(SystemImageAsset.ListPanelSeparatorTop);
				this.upperEdgeSprt.ShaderType = ShaderType.Texture;
				this.lowerEdgeSprt = new UISprite(3);
				this.listItemContainer.RootUIElement.AddChildLast(this.lowerEdgeSprt);
				this.lowerEdgeSprt.Image = new ImageAsset(SystemImageAsset.ListPanelSeparatorBottom);
				this.lowerEdgeSprt.ShaderType = ShaderType.Texture;
			}
			UISpriteUtility.SetupHorizontalThreePatch(this.upperEdgeSprt, this.listItemContainer.Width, 1f, (float)ListPanelItem.margin.Left, (float)ListPanelItem.margin.Right);
			UISpriteUtility.SetupHorizontalThreePatch(this.lowerEdgeSprt, this.listItemContainer.Width, 1f, (float)ListPanelItem.margin.Left, (float)ListPanelItem.margin.Right);
		}
	}
}
