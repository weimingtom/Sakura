using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class GridListPanel : ContainerWidget
	{
		private enum AnimationState
		{
			None,
			Drag,
			Flick,
			Fit
		}

		private enum TerminalState
		{
			None,
			Top,
			TopReset,
			Bottom,
			BottomReset
		}

		private class ListContainer
		{
			public ListPanelItem Item
			{
				get;
				set;
			}

			public int TotalIndex
			{
				get;
				set;
			}

			public bool Updated
			{
				get;
				set;
			}

			public ListContainer()
			{
				this.Item = null;
				this.TotalIndex = 0;
				this.Updated = false;
			}
		}

		private const float minDelayDragDist = 0.5f;

		private const float defaultWidth = 100f;

		private const float defaultHeight = 100f;

		private float itemWidth;

		private float itemHeight;

		private float itemVerticalGap;

		private float itemHorizontalGap;

		private ScrollBarVisibility scrollBarVisibility = ScrollBarVisibility.ScrollableVisible;

		private bool loopScroll;

		private int itemCount;

		private UISprite backgroundSprt;

		private Panel basePanel;

		private int scrollAreaFirstIndex;

		private ushort scrollAreaStepCount;

		private ushort scrollAreaLineCount;

		private bool isSetup;

		private bool isScrollSyncEnabled;

		private ushort cacheLineCount;

		private int cacheStartIndex;

		private int cacheEndIndex;

		private int fitTargetIndex;

		private float fitTargetOffsetPixel;

		private float fitInterpRatio;

		private GridListPanel.AnimationState animationState;

		private GridListPanel.TerminalState terminalState;

		private bool animation;

		private float scrollVelocity;

		private float terminalDistance;

		private List<GridListPanel.ListContainer> listItem;

		private List<GridListPanel.ListContainer> poolItem;

		private ListItemCreator itemCreator;

		private ListItemUpdater itemUpdater;

		private ScrollBar scrollBar;

		private float flickStartRatio = 0.022f;

		private float flickDecelerationRatio = 0.012f;

		private float terminalDistanceRatio = 0.073f;

		private float terminalDecelerationRatio = 0.48f;

		private float maxTerminalDistance = 240f;

		private float terminalReturnRatio = 0.09f;

		private float verticalEdgeMargin;

		private float horizontalEdgeMargin;

		public event EventHandler<TouchEventArgs> Scrolling;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.backgroundSprt != null)
				{
					UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
					unit.Width = value;
				}
				if (this.scrollBar != null)
				{
					if (this.scrollBar.Orientation == ScrollBarOrientation.Horizontal)
					{
						this.scrollBar.Y = this.Height - this.scrollBar.Height;
						this.scrollBar.Width = value;
					}
					else
					{
						this.scrollBar.X = this.Width - this.scrollBar.Width;
					}
				}
				this.CalcScrollAreaStepNum();
				this.CalcEdgeMarginHorizontal();
				if (this.isSetup)
				{
					this.UpdateListItem(false);
				}
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
				if (this.backgroundSprt != null)
				{
					UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
					unit.Height = value;
				}
				if (this.scrollBar != null)
				{
					if (this.scrollBar.Orientation == ScrollBarOrientation.Vertical)
					{
						this.scrollBar.X = this.Width - this.scrollBar.Width;
						this.scrollBar.Height = value;
					}
					else
					{
						this.scrollBar.Y = this.Height - this.scrollBar.Height;
					}
				}
				this.CalcScrollAreaLineNum();
				this.CalcEdgeMarginVertical();
				if (this.isSetup)
				{
					this.UpdateListItem(false);
				}
			}
		}

		public GridListScrollOrientation ScrollOrientation
		{
			get;
			private set;
		}

		public UIColor BackgroundColor
		{
			get
			{
				UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
				return unit.Color;
			}
			set
			{
				UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
				unit.Color = value;
			}
		}

		public float ItemWidth
		{
			get
			{
				return this.itemWidth;
			}
			set
			{
				if (value + this.ItemHorizontalGap > this.Width)
				{
					throw new ArgumentOutOfRangeException("ItemWidth");
				}
				if (this.itemWidth != value)
				{
					this.itemWidth = ((value > 0f) ? value : 0f);
					this.CalcScrollAreaStepNum();
					this.CalcEdgeMarginHorizontal();
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}
		}

		public float ItemHeight
		{
			get
			{
				return this.itemHeight;
			}
			set
			{
				if (value + this.ItemVerticalGap > this.Height)
				{
					throw new ArgumentOutOfRangeException("ItemHeight");
				}
				if (this.itemHeight != value)
				{
					this.itemHeight = ((value > 0f) ? value : 0f);
					this.CalcScrollAreaLineNum();
					this.CalcEdgeMarginVertical();
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}
		}

		public float ItemVerticalGap
		{
			get
			{
				return this.itemVerticalGap;
			}
			set
			{
				if (value + this.ItemHeight > this.Height)
				{
					throw new ArgumentOutOfRangeException("ItemVerticalGap");
				}
				if (this.ItemVerticalGap != value)
				{
					this.itemVerticalGap = value;
					this.CalcScrollAreaLineNum();
					this.CalcEdgeMarginVertical();
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}
		}

		public float ItemHorizontalGap
		{
			get
			{
				return this.itemHorizontalGap;
			}
			set
			{
				if (value + this.ItemWidth > this.Width)
				{
					throw new ArgumentOutOfRangeException("ItemHorizontalGap");
				}
				if (this.itemHorizontalGap != value)
				{
					this.itemHorizontalGap = value;
					this.CalcScrollAreaStepNum();
					this.CalcEdgeMarginHorizontal();
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}
		}

		public ScrollBarVisibility ScrollBarVisibility
		{
			get
			{
				return this.scrollBarVisibility;
			}
			set
			{
				this.scrollBarVisibility = value;
				this.updateScrollBarVisible();
			}
		}

		internal bool LoopScroll
		{
			get
			{
				return this.loopScroll;
			}
			set
			{
				if (this.loopScroll != value)
				{
					this.loopScroll = value;
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}
		}

		public bool SnapScroll
		{
			get;
			set;
		}

		public int ItemIndex
		{
			get
			{
				int result = 0;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
					result = this.scrollAreaFirstIndex * (int)this.scrollAreaLineCount;
					break;
				case GridListScrollOrientation.Vertical:
					result = this.scrollAreaFirstIndex * (int)this.scrollAreaStepCount;
					break;
				}
				return result;
			}
		}

		public float ItemPixelOffset
		{
			get
			{
				float result = 0f;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
					result = -this.basePanel.X;
					break;
				case GridListScrollOrientation.Vertical:
					result = -this.basePanel.Y;
					break;
				}
				return result;
			}
		}

		public int ItemCount
		{
			get
			{
				return this.itemCount;
			}
			set
			{
				this.itemCount = value;
				if (this.scrollAreaFirstIndex > this.MaxScrollAreaFirstIndex)
				{
					this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
				}
				if (this.isSetup)
				{
					this.UpdateListItem(false);
				}
			}
		}

		private int TotalLineCount
		{
			get
			{
				if (this.scrollAreaStepCount == 0 || this.itemCount == 0)
				{
					return 0;
				}
				return (this.itemCount - 1) / (int)this.scrollAreaStepCount + 1;
			}
		}

		private int TotalStepCount
		{
			get
			{
				if (this.scrollAreaLineCount == 0 || this.itemCount == 0)
				{
					return 0;
				}
				return (this.itemCount - 1) / (int)this.scrollAreaLineCount + 1;
			}
		}

		private int MinScrollAreaFirstIndex
		{
			get
			{
				return 0;
			}
		}

		private int MaxScrollAreaFirstIndex
		{
			get
			{
				int result = 0;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
				{
					float num = (this.Width - this.itemHorizontalGap) % (this.itemWidth + this.itemHorizontalGap);
					if (num == 0f)
					{
						result = Math.Max(this.TotalStepCount - (int)this.scrollAreaStepCount - 1, 0);
					}
					else
					{
						result = Math.Max(this.TotalStepCount - (int)this.scrollAreaStepCount, 0);
					}
					break;
				}
				case GridListScrollOrientation.Vertical:
				{
					float num2 = (this.Height - this.ItemVerticalGap) % (this.ItemHeight + this.ItemVerticalGap);
					if (num2 == 0f)
					{
						result = Math.Max(this.TotalLineCount - (int)this.scrollAreaLineCount - 1, 0);
					}
					else
					{
						result = Math.Max(this.TotalLineCount - (int)this.scrollAreaLineCount, 0);
					}
					break;
				}
				}
				return result;
			}
		}

		private float MaxScrollAreaFirstOffset
		{
			get
			{
				float result = 0f;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
				{
					float num = (this.Width - this.ItemHorizontalGap) % (this.ItemWidth + this.ItemHorizontalGap);
					result = -(num - (this.ItemWidth + this.ItemHorizontalGap));
					break;
				}
				case GridListScrollOrientation.Vertical:
				{
					float num2 = (this.Height - this.ItemVerticalGap) % (this.ItemHeight + this.ItemVerticalGap);
					result = -(num2 - (this.ItemHeight + this.ItemVerticalGap));
					break;
				}
				}
				return result;
			}
		}

		public GridListPanel(GridListScrollOrientation scrollOrientation)
		{
			this.backgroundSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.backgroundSprt);
			this.basePanel = new Panel();
			base.AddChildLast(this.basePanel);
			if (scrollOrientation == GridListScrollOrientation.Horizontal)
			{
				this.scrollBar = new ScrollBar(ScrollBarOrientation.Horizontal);
			}
			else
			{
				this.scrollBar = new ScrollBar(ScrollBarOrientation.Vertical);
			}
			this.Width = (float)UISystem.FramebufferWidth;
			this.Height = (float)UISystem.FramebufferHeight;
			this.X = 0f;
			this.Y = 0f;
			this.ScrollOrientation = scrollOrientation;
			this.scrollAreaStepCount = 1;
			this.scrollAreaLineCount = 1;
			this.scrollAreaFirstIndex = 0;
			this.ItemVerticalGap = 5f;
			this.ItemHorizontalGap = 5f;
			this.loopScroll = false;
			this.SnapScroll = false;
			this.isSetup = false;
			this.isScrollSyncEnabled = true;
			this.itemCount = 0;
			this.itemWidth = 64f;
			this.itemHeight = 64f;
			this.cacheLineCount = 1;
			this.cacheStartIndex = -1;
			this.cacheEndIndex = -1;
			this.fitTargetIndex = 0;
			this.fitTargetOffsetPixel = 0f;
			this.fitInterpRatio = 0.2f;
			this.scrollVelocity = 0f;
			this.listItem = new List<GridListPanel.ListContainer>();
			this.poolItem = new List<GridListPanel.ListContainer>();
			this.animationState = GridListPanel.AnimationState.None;
			this.terminalState = GridListPanel.TerminalState.None;
			this.terminalDistance = 0f;
			base.Clip = true;
			base.HookChildTouchEvent = true;
			this.animation = false;
		}

		private void CalcScrollAreaLineNum()
		{
			if (this.Height <= 0f || this.ItemHeight <= 0f)
			{
				this.scrollAreaLineCount = 0;
			}
			float num = (this.Height - this.ItemVerticalGap) / (this.ItemHeight + this.ItemVerticalGap);
			float num2 = (this.Height - this.ItemVerticalGap) % (this.ItemHeight + this.ItemVerticalGap);
			if (num2 == 0f)
			{
				this.scrollAreaLineCount = (ushort)num;
				return;
			}
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				this.scrollAreaLineCount = (ushort)num;
				return;
			case GridListScrollOrientation.Vertical:
				this.scrollAreaLineCount = (ushort)(num + 1f);
				return;
			default:
				return;
			}
		}

		private void CalcScrollAreaStepNum()
		{
			if (this.Width <= 0f || this.ItemWidth <= 0f)
			{
				this.scrollAreaStepCount = 0;
			}
			float num = (this.Width - this.ItemHorizontalGap) / (this.ItemWidth + this.ItemHorizontalGap);
			float num2 = (this.Width - this.ItemHorizontalGap) % (this.ItemWidth + this.ItemHorizontalGap);
			if (num2 == 0f)
			{
				this.scrollAreaStepCount = (ushort)num;
				return;
			}
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				this.scrollAreaStepCount = (ushort)(num + 1f);
				return;
			case GridListScrollOrientation.Vertical:
				this.scrollAreaStepCount = (ushort)num;
				return;
			default:
				return;
			}
		}

		private void CalcEdgeMarginVertical()
		{
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				this.verticalEdgeMargin = (this.Height - (float)this.scrollAreaLineCount * (this.ItemHeight + this.ItemVerticalGap) + this.ItemVerticalGap) / 2f;
				return;
			case GridListScrollOrientation.Vertical:
				this.verticalEdgeMargin = this.ItemVerticalGap;
				return;
			default:
				return;
			}
		}

		private void CalcEdgeMarginHorizontal()
		{
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				this.horizontalEdgeMargin = this.ItemHorizontalGap;
				return;
			case GridListScrollOrientation.Vertical:
				this.horizontalEdgeMargin = (this.Width - (float)this.scrollAreaStepCount * (this.ItemWidth + this.ItemHorizontalGap) + this.ItemHorizontalGap) / 2f;
				return;
			default:
				return;
			}
		}

		public void StartItemRequest()
		{
			if (this.itemCreator == null)
			{
				return;
			}
			if (!this.isSetup)
			{
				DragGestureDetector dragGestureDetector = new DragGestureDetector();
				dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
				base.AddGestureDetector(dragGestureDetector);
				FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
				flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
				base.AddGestureDetector(flickGestureDetector);
			}
			if (this.scrollAreaFirstIndex > this.MaxScrollAreaFirstIndex)
			{
				this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
			}
			else
			{
				this.SetScrollAreaFirstIndex(this.scrollAreaFirstIndex, 0f);
			}
			this.listItem.Clear();
			this.poolItem.Clear();
			this.cacheStartIndex = 0;
			this.cacheEndIndex = 0;
			this.isSetup = true;
			this.UpdateListItem(false);
		}

		public ListPanelItem GetListItem(int index)
		{
			if (index < 0)
			{
				return null;
			}
			for (int i = 0; i < this.listItem.Count; i++)
			{
				ListPanelItem item = this.listItem[i].Item;
				if (item != null && item.Index == index)
				{
					return item;
				}
			}
			return null;
		}

		private void NormalizeScrollAreaFirstIndex(ref int lineIndex, ref float offsetPixel)
		{
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				while (offsetPixel < 0f)
				{
					offsetPixel += this.ItemWidth + this.ItemHorizontalGap;
					lineIndex--;
				}
				while (offsetPixel > this.ItemWidth + this.ItemHorizontalGap)
				{
					offsetPixel -= this.ItemWidth + this.ItemHorizontalGap;
					lineIndex++;
				}
				return;
			case GridListScrollOrientation.Vertical:
				while (offsetPixel < 0f)
				{
					offsetPixel += this.ItemHeight + this.ItemVerticalGap;
					lineIndex--;
				}
				while (offsetPixel > this.ItemHeight + this.ItemVerticalGap)
				{
					offsetPixel -= this.ItemHeight + this.ItemVerticalGap;
					lineIndex++;
				}
				return;
			default:
				return;
			}
		}

		private void SetScrollAreaFirstIndex(int lineIndex, float offsetPixel)
		{
			this.NormalizeScrollAreaFirstIndex(ref lineIndex, ref offsetPixel);
			if (this.loopScroll)
			{
				while (lineIndex < 0)
				{
					lineIndex += this.TotalLineCount;
					this.cacheStartIndex += this.itemCount;
					this.cacheEndIndex += this.itemCount;
					this.fitTargetIndex += this.TotalLineCount;
					for (int i = 0; i < this.listItem.Count; i++)
					{
						this.listItem[i].TotalIndex += this.itemCount;
					}
				}
				while (lineIndex > this.TotalLineCount - 1)
				{
					lineIndex -= this.TotalLineCount;
					this.cacheStartIndex -= this.itemCount;
					this.cacheEndIndex -= this.itemCount;
					this.fitTargetIndex -= this.TotalLineCount;
					for (int j = 0; j < this.listItem.Count; j++)
					{
						this.listItem[j].TotalIndex -= this.itemCount;
					}
				}
				this.scrollAreaFirstIndex = lineIndex;
				this.SetNodePos(offsetPixel);
				if (this.isSetup)
				{
					this.UpdateListItem(false);
					return;
				}
			}
			else if (this.MinScrollAreaFirstIndex <= lineIndex && lineIndex <= this.MaxScrollAreaFirstIndex)
			{
				bool isMod = false;
				if (this.scrollAreaFirstIndex != lineIndex)
				{
					this.scrollAreaFirstIndex = lineIndex;
					isMod = true;
				}
				if (this.terminalState != GridListPanel.TerminalState.None)
				{
					isMod = true;
				}
				this.SetNodePos(offsetPixel);
				if (this.isSetup)
				{
					this.UpdateListItem(isMod);
				}
			}
		}

		public void ScrollTo(int itemIndex, float pixelOffset, bool withAnimation)
		{
			if (itemIndex >= this.ItemCount)
			{
				return;
			}
			if (this.isSetup)
			{
				this.isScrollSyncEnabled = true;
			}
			int num = this.CalcLineIndex(itemIndex);
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				num = this.CalcStepIndex(itemIndex);
				break;
			case GridListScrollOrientation.Vertical:
				num = this.CalcLineIndex(itemIndex);
				break;
			}
			this.NormalizeScrollAreaFirstIndex(ref num, ref pixelOffset);
			if (this.SnapScroll)
			{
				if (pixelOffset > (this.itemHeight + this.ItemVerticalGap) * 0.5f)
				{
					num++;
				}
				pixelOffset = 0f;
			}
			if (!this.loopScroll)
			{
				if (num >= this.MaxScrollAreaFirstIndex)
				{
					num = this.MaxScrollAreaFirstIndex;
					pixelOffset = 0f;
				}
				if (num < this.MinScrollAreaFirstIndex)
				{
					num = this.MinScrollAreaFirstIndex;
					pixelOffset = 0f;
				}
			}
			else if (Math.Abs(num - num) > (this.TotalLineCount - 1) / 2)
			{
				if (num - num > 0)
				{
					num -= this.TotalLineCount;
				}
				else
				{
					num += this.TotalLineCount;
				}
			}
			if (num >= this.MaxScrollAreaFirstIndex)
			{
				pixelOffset = this.MaxScrollAreaFirstOffset;
			}
			if (this.isSetup && withAnimation)
			{
				this.fitTargetIndex = num;
				this.fitTargetOffsetPixel = pixelOffset;
				this.animationState = GridListPanel.AnimationState.Fit;
				this.fitInterpRatio = 0.2f;
				this.animation = true;
				return;
			}
			this.SetScrollAreaFirstIndex(num, pixelOffset);
		}

		public void ScrollTo(int itemIndex, bool withAnimation)
		{
			this.ScrollTo(itemIndex, 0f, withAnimation);
		}

		private void UpdateListItem(bool isMod)
		{
			int num = 0;
			int num2 = 0;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				num = (this.scrollAreaFirstIndex - (int)this.cacheLineCount) * (int)this.scrollAreaLineCount;
				num2 = (this.scrollAreaFirstIndex + (int)this.scrollAreaStepCount + (int)this.scrollAreaStepCount + 1) * (int)this.scrollAreaLineCount;
				break;
			case GridListScrollOrientation.Vertical:
				num = (this.scrollAreaFirstIndex - (int)this.cacheLineCount) * (int)this.scrollAreaStepCount;
				num2 = (this.scrollAreaFirstIndex + (int)this.scrollAreaLineCount + (int)this.cacheLineCount + 1) * (int)this.scrollAreaStepCount;
				break;
			}
			if (!this.loopScroll)
			{
				if (num < 0)
				{
					num = 0;
				}
				if (num2 > this.itemCount)
				{
					num2 = this.itemCount;
				}
			}
			bool flag = isMod;
			GridListPanel.ListContainer listContainer = new GridListPanel.ListContainer();
			if (this.cacheEndIndex - 1 >= num)
			{
				if (num2 - 1 >= this.cacheStartIndex)
				{
					if (this.cacheStartIndex == num && this.cacheEndIndex == num2)
					{
						goto IL_255;
					}
					if (this.cacheStartIndex < num)
					{
						for (int i = num - 1; i >= this.cacheStartIndex; i--)
						{
							listContainer = this.listItem[0];
							this.listItem.RemoveAt(0);
							this.PoolContainer(ref listContainer);
							flag = true;
						}
					}
					if (this.cacheEndIndex > num2)
					{
						for (int j = num2; j < this.cacheEndIndex; j++)
						{
							listContainer = this.listItem[this.listItem.Count - 1];
							this.listItem.RemoveAt(this.listItem.Count - 1);
							this.PoolContainer(ref listContainer);
							flag = true;
						}
					}
					if (this.cacheStartIndex > num)
					{
						for (int k = this.cacheStartIndex - 1; k >= num; k--)
						{
							listContainer = this.GetContainer();
							this.CreateListItem(ref listContainer, k);
							this.listItem.Insert(0, listContainer);
							flag = true;
						}
					}
					if (this.cacheEndIndex < num2)
					{
						for (int l = this.cacheEndIndex; l < num2; l++)
						{
							listContainer = this.GetContainer();
							this.CreateListItem(ref listContainer, l);
							this.listItem.Add(listContainer);
							flag = true;
						}
						goto IL_255;
					}
					goto IL_255;
				}
			}
			while (this.listItem.Count != 0)
			{
				listContainer = this.listItem[0];
				this.listItem.RemoveAt(0);
				this.PoolContainer(ref listContainer);
				flag = true;
			}
			for (int m = num; m < num2; m++)
			{
				listContainer = this.GetContainer();
				this.CreateListItem(ref listContainer, m);
				this.listItem.Add(listContainer);
				flag = true;
			}
			IL_255:
			this.cacheStartIndex = num;
			this.cacheEndIndex = num2;
			if (flag)
			{
				for (int n = 0; n < this.listItem.Count; n++)
				{
					int totalIndex = this.listItem[n].TotalIndex;
					this.listItem[n].Item.X = this.CalcItemPosOnScrollNodeX(totalIndex);
					this.listItem[n].Item.Y = this.CalcItemPosOnScrollNodeY(totalIndex);
					if (this.listItem[n].Updated && this.itemUpdater != null)
					{
						this.itemUpdater(this.listItem[n].Item);
						this.listItem[n].Updated = false;
					}
				}
			}
			if (this.isScrollSyncEnabled && this.Scrolling != null)
			{
				this.Scrolling.Invoke(this, null);
			}
			this.UpdateScrollBar();
		}

		private GridListPanel.ListContainer GetContainer()
		{
			GridListPanel.ListContainer listContainer = new GridListPanel.ListContainer();
			if (this.poolItem.Count != 0)
			{
				listContainer = this.poolItem[0];
				this.poolItem.RemoveAt(0);
				listContainer.Item.Visible = true;
				listContainer.Item.TouchResponse = true;
			}
			else
			{
				listContainer = new GridListPanel.ListContainer();
				listContainer.Item = null;
			}
			return listContainer;
		}

		private void PoolContainer(ref GridListPanel.ListContainer container)
		{
			if (container != null)
			{
				this.poolItem.Add(container);
				container.Item.Visible = false;
			}
		}

		private void CreateListItem(ref GridListPanel.ListContainer listContainer, int index)
		{
			if (this.itemCreator == null)
			{
				return;
			}
			ListPanelItem listPanelItem;
			if (listContainer.Item == null)
			{
				listPanelItem = this.itemCreator();
			}
			else
			{
				listPanelItem = listContainer.Item;
			}
			listContainer.Item = listPanelItem;
			listContainer.TotalIndex = index;
			listContainer.Updated = true;
			listPanelItem.Width = this.ItemWidth;
			listPanelItem.Height = this.ItemHeight;
			listPanelItem.Index = this.CalcItemIndex(index);
			this.basePanel.AddChildLast(listPanelItem);
		}

		private int CalcStepIndex(int index)
		{
			if (index <= 0)
			{
				return 0;
			}
			int result = 0;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				result = index / (int)this.scrollAreaLineCount;
				break;
			case GridListScrollOrientation.Vertical:
				result = index % (int)this.scrollAreaStepCount;
				break;
			}
			return result;
		}

		private int CalcLineIndex(int index)
		{
			if (index <= 0)
			{
				return 0;
			}
			int result = 0;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				result = index % (int)this.scrollAreaLineCount;
				break;
			case GridListScrollOrientation.Vertical:
				result = index / (int)this.scrollAreaStepCount;
				break;
			}
			return result;
		}

		private int CalcItemIndex(int totalIndex)
		{
			if (totalIndex < 0)
			{
				int num = (Math.Abs(totalIndex) - 1) / this.itemCount;
				num = -num - 1;
				totalIndex += -num * this.itemCount;
				totalIndex %= this.itemCount;
			}
			else
			{
				totalIndex %= this.itemCount;
			}
			return totalIndex;
		}

		private float CalcItemPosOnScrollNodeX(int totalIndex)
		{
			float num = 0f;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				if (this.terminalState == GridListPanel.TerminalState.Top || this.terminalState == GridListPanel.TerminalState.TopReset)
				{
					num = (float)(this.CalcStepIndex(totalIndex) - this.MinScrollAreaFirstIndex) * (this.itemWidth + this.ItemHorizontalGap + this.terminalDistance * this.terminalDistanceRatio) + this.horizontalEdgeMargin;
				}
				else if (this.terminalState == GridListPanel.TerminalState.Bottom || this.terminalState == GridListPanel.TerminalState.BottomReset)
				{
					num = (float)(this.CalcStepIndex(totalIndex) - this.MaxScrollAreaFirstIndex) * (this.itemWidth + this.ItemHorizontalGap) + this.horizontalEdgeMargin;
					num -= (float)(this.CalcStepIndex(this.ItemCount - 1) - this.CalcStepIndex(totalIndex)) * this.terminalDistance * this.terminalDistanceRatio;
				}
				else
				{
					num = (float)(this.CalcStepIndex(totalIndex) - this.scrollAreaFirstIndex) * (this.itemWidth + this.ItemHorizontalGap) + this.horizontalEdgeMargin;
				}
				break;
			case GridListScrollOrientation.Vertical:
				num = (float)this.CalcStepIndex(totalIndex) * (this.itemWidth + this.ItemHorizontalGap) + this.horizontalEdgeMargin;
				break;
			}
			return num;
		}

		private float CalcItemPosOnScrollNodeY(int totalIndex)
		{
			float num = 0f;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				num = (float)this.CalcLineIndex(totalIndex) * (this.itemHeight + this.ItemVerticalGap) + this.verticalEdgeMargin;
				break;
			case GridListScrollOrientation.Vertical:
				if (this.terminalState == GridListPanel.TerminalState.Top || this.terminalState == GridListPanel.TerminalState.TopReset)
				{
					num = (float)(this.CalcLineIndex(totalIndex) - this.MinScrollAreaFirstIndex) * (this.itemHeight + this.ItemVerticalGap + this.terminalDistance * this.terminalDistanceRatio) + this.verticalEdgeMargin;
				}
				else if (this.terminalState == GridListPanel.TerminalState.Bottom || this.terminalState == GridListPanel.TerminalState.BottomReset)
				{
					num = (float)(this.CalcLineIndex(totalIndex) - this.MaxScrollAreaFirstIndex) * (this.itemHeight + this.ItemVerticalGap) + this.verticalEdgeMargin;
					num -= (float)(this.CalcLineIndex(this.ItemCount - 1) - this.CalcLineIndex(totalIndex)) * this.terminalDistance * this.terminalDistanceRatio;
				}
				else
				{
					num = (float)(this.CalcLineIndex(totalIndex) - this.scrollAreaFirstIndex) * (this.itemHeight + this.ItemVerticalGap) + this.verticalEdgeMargin;
				}
				break;
			}
			return num;
		}

		private void SetNodePos(float pos)
		{
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				this.basePanel.X = -pos;
				return;
			case GridListScrollOrientation.Vertical:
				this.basePanel.Y = -pos;
				return;
			default:
				return;
			}
		}

		private float GetVirtualVec(Vector2 realVec)
		{
			float result = 0f;
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
				result = -realVec.X;
				break;
			case GridListScrollOrientation.Vertical:
				result = -realVec.Y;
				break;
			}
			return result;
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (!this.isSetup)
			{
				return;
			}
			if (!this.IsScrollable())
			{
				touchEvents.Forward = true;
				return;
			}
			if (this.animationState != GridListPanel.AnimationState.Drag && this.animationState != GridListPanel.AnimationState.Flick)
			{
				touchEvents.Forward = true;
			}
			switch (touchEvents.PrimaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.animationState == GridListPanel.AnimationState.Drag)
				{
					if (this.terminalState == GridListPanel.TerminalState.Top)
					{
						this.terminalState = GridListPanel.TerminalState.TopReset;
						this.animationState = GridListPanel.AnimationState.None;
						this.animation = true;
					}
					else if (this.terminalState == GridListPanel.TerminalState.Bottom)
					{
						this.terminalState = GridListPanel.TerminalState.BottomReset;
						this.animationState = GridListPanel.AnimationState.None;
						this.animation = true;
					}
					else if (this.SnapScroll)
					{
						float num = 0f;
						switch (this.ScrollOrientation)
						{
						case GridListScrollOrientation.Horizontal:
							num = this.ItemWidth + this.ItemHorizontalGap;
							break;
						case GridListScrollOrientation.Vertical:
							num = this.ItemHeight + this.ItemVerticalGap;
							break;
						}
						if (this.scrollAreaFirstIndex == this.MaxScrollAreaFirstIndex && this.MaxScrollAreaFirstOffset != 0f)
						{
							num = this.MaxScrollAreaFirstOffset;
						}
						if (this.ItemPixelOffset > num * 0.5f)
						{
							this.fitTargetIndex = this.scrollAreaFirstIndex + 1;
						}
						else
						{
							this.fitTargetIndex = this.scrollAreaFirstIndex;
						}
						this.fitTargetOffsetPixel = 0f;
						if (this.fitTargetIndex > this.MaxScrollAreaFirstIndex)
						{
							this.fitTargetIndex = this.MaxScrollAreaFirstIndex;
							this.fitTargetOffsetPixel = this.MaxScrollAreaFirstOffset;
						}
						this.animationState = GridListPanel.AnimationState.Fit;
						this.fitInterpRatio = 0.2f;
						this.animation = true;
					}
				}
				this.updateScrollBarVisible();
				return;
			case TouchEventType.Down:
				if (this.terminalState != GridListPanel.TerminalState.None)
				{
					this.ResetTerminalAnimation();
				}
				this.animationState = GridListPanel.AnimationState.None;
				this.animation = false;
				return;
			default:
				return;
			}
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			if (!this.IsScrollable())
			{
				return;
			}
			this.isScrollSyncEnabled = true;
			this.animationState = GridListPanel.AnimationState.Drag;
			float num = this.GetVirtualVec(e.Distance) + this.ItemPixelOffset;
			float virtualVec = this.GetVirtualVec(e.Distance);
			int num2 = this.scrollAreaFirstIndex;
			this.NormalizeScrollAreaFirstIndex(ref num2, ref num);
			if (num2 < this.MinScrollAreaFirstIndex && !this.loopScroll)
			{
				this.terminalDistance -= virtualVec;
				this.UpdateTerminalAnimation(GridListPanel.TerminalState.Top);
				return;
			}
			if (this.IsOverMaxScrollArea(num2, num) && !this.loopScroll)
			{
				this.terminalDistance += virtualVec;
				this.UpdateTerminalAnimation(GridListPanel.TerminalState.Bottom);
				return;
			}
			if (this.terminalState == GridListPanel.TerminalState.Top)
			{
				if (this.terminalDistance <= 0f)
				{
					this.terminalDistance = 0f;
					this.UpdateTerminalAnimation(GridListPanel.TerminalState.None);
					return;
				}
				this.terminalDistance -= virtualVec;
				this.UpdateTerminalAnimation(this.terminalState);
				return;
			}
			else
			{
				if (this.terminalState != GridListPanel.TerminalState.Bottom)
				{
					this.SetScrollAreaFirstIndex(num2, num);
					return;
				}
				if (this.terminalDistance <= 0f)
				{
					this.terminalDistance = 0f;
					this.UpdateTerminalAnimation(GridListPanel.TerminalState.None);
					return;
				}
				this.terminalDistance += virtualVec;
				this.UpdateTerminalAnimation(this.terminalState);
				return;
			}
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			if (!this.IsScrollable())
			{
				return;
			}
			this.isScrollSyncEnabled = true;
			this.animationState = GridListPanel.AnimationState.Flick;
			float virtualVec = this.GetVirtualVec(e.Speed);
			if (this.terminalState == GridListPanel.TerminalState.None)
			{
				this.scrollVelocity = virtualVec * this.flickStartRatio;
			}
			else
			{
				this.scrollVelocity = 0f;
			}
			if (this.terminalState == GridListPanel.TerminalState.Top)
			{
				if (this.scrollVelocity > 0f)
				{
					this.ResetTerminalAnimation();
				}
				else
				{
					this.terminalDistance += this.scrollVelocity;
					this.UpdateTerminalAnimation(this.terminalState);
				}
			}
			else if (this.terminalState == GridListPanel.TerminalState.Bottom)
			{
				if (this.scrollVelocity < 0f)
				{
					this.ResetTerminalAnimation();
				}
				else
				{
					this.terminalDistance += this.scrollVelocity;
					this.UpdateTerminalAnimation(this.terminalState);
				}
			}
			else if (this.SnapScroll)
			{
				float num = 0f;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
					num = this.ItemWidth + this.ItemHorizontalGap;
					break;
				case GridListScrollOrientation.Vertical:
					num = this.ItemHeight + this.ItemVerticalGap;
					break;
				}
				if (this.scrollVelocity > 0f)
				{
					if ((double)num * 2.0 - (double)this.ItemPixelOffset - (double)(this.scrollVelocity / this.flickDecelerationRatio) > 0.0)
					{
						this.animationState = GridListPanel.AnimationState.Fit;
						if (num - this.ItemPixelOffset != 0f)
						{
							this.fitInterpRatio = this.scrollVelocity / (num - this.ItemPixelOffset);
						}
						else
						{
							this.fitInterpRatio = 1f;
						}
						if (this.scrollAreaFirstIndex >= this.MaxScrollAreaFirstIndex && !this.loopScroll)
						{
							this.fitTargetIndex = this.scrollAreaFirstIndex;
						}
						else
						{
							this.fitTargetIndex = this.scrollAreaFirstIndex + 1;
						}
						this.fitTargetOffsetPixel = 0f;
					}
				}
				else if ((double)(num + this.ItemPixelOffset + this.scrollVelocity / this.flickDecelerationRatio) > 0.0)
				{
					this.animationState = GridListPanel.AnimationState.Fit;
					if (this.ItemPixelOffset != 0f)
					{
						this.fitInterpRatio = -this.scrollVelocity / this.ItemPixelOffset;
					}
					else
					{
						this.fitInterpRatio = 1f;
					}
					this.fitTargetIndex = this.scrollAreaFirstIndex;
					this.fitTargetOffsetPixel = 0f;
				}
			}
			this.animation = true;
		}

		private bool IsOverMaxScrollArea(int index, float offset)
		{
			return index > this.MaxScrollAreaFirstIndex || (index == this.MaxScrollAreaFirstIndex && offset >= this.MaxScrollAreaFirstOffset);
		}

		protected internal override void OnResetState()
		{
			base.OnResetState();
			this.animationState = GridListPanel.AnimationState.None;
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (!this.animation)
			{
				return;
			}
			if (!this.isSetup)
			{
				this.StartItemRequest();
			}
			switch (this.animationState)
			{
			case GridListPanel.AnimationState.None:
				this.terminalDistance *= (float)Math.Pow((double)(1f - this.terminalReturnRatio), (double)(elapsedTime / 16.6f));
				this.UpdateTerminalAnimation(this.terminalState);
				break;
			case GridListPanel.AnimationState.Drag:
				break;
			case GridListPanel.AnimationState.Flick:
			{
				if (Math.Abs(this.scrollVelocity * elapsedTime / 16.6f) < 0.3012048f || Math.Abs(this.terminalDistance) > this.maxTerminalDistance)
				{
					if (this.terminalState == GridListPanel.TerminalState.Top)
					{
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.TopReset);
						if (this.terminalState == GridListPanel.TerminalState.None)
						{
							return;
						}
					}
					else if (this.terminalState == GridListPanel.TerminalState.Bottom)
					{
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.BottomReset);
						if (this.terminalState == GridListPanel.TerminalState.None)
						{
							return;
						}
					}
					else if (!this.SnapScroll)
					{
						this.scrollVelocity = 0f;
						this.animationState = GridListPanel.AnimationState.None;
						this.animation = false;
					}
				}
				float num = this.scrollVelocity * elapsedTime / 16.6f;
				float num2 = num + this.ItemPixelOffset;
				int num3 = this.scrollAreaFirstIndex;
				this.NormalizeScrollAreaFirstIndex(ref num3, ref num2);
				if (((this.MinScrollAreaFirstIndex == num3 && num2 == 0f) || num3 < this.MinScrollAreaFirstIndex) && !this.loopScroll)
				{
					if (this.terminalState == GridListPanel.TerminalState.BottomReset || this.terminalState == GridListPanel.TerminalState.TopReset)
					{
						this.terminalDistance *= (float)Math.Pow((double)(1f - this.terminalReturnRatio), (double)(elapsedTime / 16.6f));
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.Top);
					}
					else
					{
						this.terminalDistance -= num * 2f;
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.Top);
					}
				}
				else if (this.IsOverMaxScrollArea(num3, num2) && !this.loopScroll)
				{
					if (this.terminalState == GridListPanel.TerminalState.BottomReset || this.terminalState == GridListPanel.TerminalState.TopReset)
					{
						this.terminalDistance *= (float)Math.Pow((double)(1f - this.terminalReturnRatio), (double)(elapsedTime / 16.6f));
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.Bottom);
					}
					else
					{
						this.terminalDistance += num * 2f;
						this.UpdateTerminalAnimation(GridListPanel.TerminalState.Bottom);
					}
				}
				else
				{
					this.SetScrollAreaFirstIndex(num3, num2);
				}
				if (this.terminalState == GridListPanel.TerminalState.None)
				{
					this.scrollVelocity *= (float)Math.Pow((double)(1f - this.flickDecelerationRatio), (double)(elapsedTime / 16.6f));
				}
				else
				{
					this.scrollVelocity *= (float)Math.Pow((double)(1f - this.terminalDecelerationRatio), (double)(elapsedTime / 16.6f));
				}
				if (this.terminalState == GridListPanel.TerminalState.None && this.SnapScroll)
				{
					float num4 = 0f;
					switch (this.ScrollOrientation)
					{
					case GridListScrollOrientation.Horizontal:
						num4 = this.ItemWidth + this.ItemHorizontalGap;
						break;
					case GridListScrollOrientation.Vertical:
						num4 = this.ItemHeight + this.ItemVerticalGap;
						break;
					}
					if (this.scrollVelocity > 0f)
					{
						if (this.itemHeight + this.ItemVerticalGap - this.ItemPixelOffset - this.scrollVelocity * elapsedTime / 16.6f < 0f && num4 * 3f - this.ItemPixelOffset - this.scrollVelocity * elapsedTime / 16.6f / this.flickDecelerationRatio > 0f)
						{
							this.animationState = GridListPanel.AnimationState.Fit;
							this.fitInterpRatio = this.scrollVelocity * elapsedTime / 16.6f / ((this.itemHeight + this.ItemVerticalGap) * 2f - this.ItemPixelOffset);
							this.fitTargetIndex = this.scrollAreaFirstIndex + 2;
							this.fitTargetOffsetPixel = 0f;
							return;
						}
					}
					else if (this.ItemPixelOffset + this.scrollVelocity * elapsedTime / 16.6f < 0f && num4 * 2f + this.ItemPixelOffset + this.scrollVelocity * elapsedTime / 16.6f / this.flickDecelerationRatio > 0f)
					{
						this.animationState = GridListPanel.AnimationState.Fit;
						this.fitInterpRatio = -this.scrollVelocity * elapsedTime / 16.6f / ((this.itemHeight + this.ItemVerticalGap) * 1f + this.ItemPixelOffset);
						this.fitTargetIndex = this.scrollAreaFirstIndex - 1;
						this.fitTargetOffsetPixel = 0f;
						return;
					}
				}
				break;
			}
			case GridListPanel.AnimationState.Fit:
			{
				int num5 = this.scrollAreaFirstIndex - this.fitTargetIndex;
				float num6 = (float)num5 * (float)Math.Pow((double)(1f - this.fitInterpRatio), (double)(elapsedTime / 16.6f));
				num5 = (int)num6;
				float num7 = 0f;
				switch (this.ScrollOrientation)
				{
				case GridListScrollOrientation.Horizontal:
					num7 = (num6 - (float)num5) * (this.itemWidth + this.itemHorizontalGap);
					num7 += (this.ItemPixelOffset - this.fitTargetOffsetPixel) * (float)Math.Pow((double)(1f - this.fitInterpRatio), (double)(elapsedTime / 16.6f));
					break;
				case GridListScrollOrientation.Vertical:
					num7 = (num6 - (float)num5) * (this.itemHeight + this.ItemVerticalGap);
					num7 += (this.ItemPixelOffset - this.fitTargetOffsetPixel) * (float)Math.Pow((double)(1f - this.fitInterpRatio), (double)(elapsedTime / 16.6f));
					break;
				}
				this.SetScrollAreaFirstIndex(num5 + this.fitTargetIndex, num7 + this.fitTargetOffsetPixel);
				if (num5 == 0 && Math.Abs(num7) <= 0.5f)
				{
					this.animationState = GridListPanel.AnimationState.None;
					this.SetScrollAreaFirstIndex(this.fitTargetIndex, this.fitTargetOffsetPixel);
					this.animation = false;
				}
				if (this.IsOverMaxScrollArea(this.scrollAreaFirstIndex, this.ItemPixelOffset))
				{
					this.animationState = GridListPanel.AnimationState.None;
					this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
					this.animation = false;
					return;
				}
				break;
			}
			default:
				return;
			}
		}

		private void UpdateTerminalAnimation(GridListPanel.TerminalState state)
		{
			if (Math.Abs(this.terminalDistance) > this.maxTerminalDistance)
			{
				if (this.terminalDistance > 0f)
				{
					this.terminalDistance = this.maxTerminalDistance;
					this.terminalState = GridListPanel.TerminalState.TopReset;
					this.scrollVelocity = 0f;
				}
				else
				{
					this.terminalDistance = -this.maxTerminalDistance;
					this.terminalState = GridListPanel.TerminalState.BottomReset;
					this.scrollVelocity = 0f;
				}
			}
			this.terminalState = state;
			switch (this.terminalState)
			{
			case GridListPanel.TerminalState.Top:
				this.SetScrollAreaFirstIndex(this.MinScrollAreaFirstIndex, 0f);
				break;
			case GridListPanel.TerminalState.TopReset:
				if (this.terminalDistance < 0.5f)
				{
					this.terminalState = GridListPanel.TerminalState.None;
					this.terminalDistance = 0f;
					this.animationState = GridListPanel.AnimationState.None;
					this.animation = false;
				}
				this.SetScrollAreaFirstIndex(this.MinScrollAreaFirstIndex, 0f);
				break;
			case GridListPanel.TerminalState.Bottom:
				this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
				break;
			case GridListPanel.TerminalState.BottomReset:
				if (this.terminalDistance < 0.5f)
				{
					this.terminalState = GridListPanel.TerminalState.None;
					this.terminalDistance = 0f;
					this.animationState = GridListPanel.AnimationState.None;
					this.animation = false;
				}
				this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
				break;
			}
			this.UpdateScrollBar();
		}

		private void ResetTerminalAnimation()
		{
			switch (this.terminalState)
			{
			case GridListPanel.TerminalState.Top:
			case GridListPanel.TerminalState.TopReset:
				this.terminalDistance = 0f;
				this.SetScrollAreaFirstIndex(this.MinScrollAreaFirstIndex, 0f);
				this.terminalState = GridListPanel.TerminalState.None;
				return;
			case GridListPanel.TerminalState.Bottom:
			case GridListPanel.TerminalState.BottomReset:
				this.terminalDistance = 0f;
				this.SetScrollAreaFirstIndex(this.MaxScrollAreaFirstIndex, this.MaxScrollAreaFirstOffset);
				this.terminalState = GridListPanel.TerminalState.None;
				return;
			default:
				return;
			}
		}

		private void UpdateScrollBar()
		{
			if (this.scrollBar.Orientation == ScrollBarOrientation.Vertical)
			{
				float num = this.itemHeight + this.itemVerticalGap;
				this.scrollBar.BarPosition = (float)this.scrollAreaFirstIndex + this.ItemPixelOffset / num;
				if (this.terminalState == GridListPanel.TerminalState.Bottom || this.terminalState == GridListPanel.TerminalState.BottomReset)
				{
					for (int i = 0; i < (int)this.scrollAreaLineCount; i++)
					{
						this.scrollBar.BarPosition += this.terminalDistance * this.terminalDistanceRatio / num;
					}
				}
				this.scrollBar.BarLength = this.Height / (num + Math.Abs(this.terminalDistance) * this.terminalDistanceRatio);
				this.scrollBar.Length = (float)this.TotalLineCount;
			}
			else
			{
				float num2 = this.itemWidth + this.itemHorizontalGap;
				this.scrollBar.BarPosition = (float)this.scrollAreaFirstIndex + this.ItemPixelOffset / num2;
				if (this.terminalState == GridListPanel.TerminalState.Bottom || this.terminalState == GridListPanel.TerminalState.BottomReset)
				{
					for (int j = 0; j < (int)this.scrollAreaStepCount; j++)
					{
						this.scrollBar.BarPosition += this.terminalDistance * this.terminalDistanceRatio / num2;
					}
				}
				this.scrollBar.BarLength = this.Width / (num2 + Math.Abs(this.terminalDistance) * this.terminalDistanceRatio);
				this.scrollBar.Length = (float)this.TotalStepCount;
			}
			this.AddChildLast(this.scrollBar);
			this.updateScrollBarVisible();
		}

		private void updateScrollBarVisible()
		{
			switch (this.scrollBarVisibility)
			{
			case ScrollBarVisibility.Visible:
				this.scrollBar.Visible = true;
				return;
			case ScrollBarVisibility.ScrollableVisible:
				this.scrollBar.Visible = this.IsScrollable();
				return;
			case ScrollBarVisibility.ScrollingVisible:
				this.scrollBar.Visible = (this.animationState != GridListPanel.AnimationState.None || this.terminalState != GridListPanel.TerminalState.None);
				return;
			case ScrollBarVisibility.Invisible:
				this.scrollBar.Visible = false;
				return;
			default:
				return;
			}
		}

		private bool IsScrollable()
		{
			switch (this.ScrollOrientation)
			{
			case GridListScrollOrientation.Horizontal:
			{
				float num = (this.itemWidth + this.ItemHorizontalGap) * (float)this.TotalStepCount;
				if (num <= this.Width)
				{
					return false;
				}
				break;
			}
			case GridListScrollOrientation.Vertical:
			{
				float num2 = (this.itemHeight + this.ItemVerticalGap) * (float)this.TotalLineCount;
				if (num2 <= this.Height)
				{
					return false;
				}
				break;
			}
			}
			return true;
		}

		public void SetListItemCreator(ListItemCreator creator)
		{
			this.itemCreator = creator;
		}

		public void SetListItemUpdater(ListItemUpdater updater)
		{
			this.itemUpdater = updater;
		}
	}
}
