using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class ListPanel : ContainerWidget
	{
		private enum AnimationState
		{
			None,
			Drag,
			Flick
		}

		private enum TerminalState
		{
			None,
			Top,
			Bottom,
			TopReset,
			BottomReset
		}

		private class ListPanelSectionItem : ListPanelItem
		{
			private Label label;

			public override float Width
			{
				get
				{
					return base.Width;
				}
				set
				{
					base.Width = value;
					if (this.label != null)
					{
						this.label.Width = value;
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
					if (this.label != null)
					{
						this.label.Height = value;
					}
				}
			}

			public string Title
			{
				get
				{
					return this.label.Text;
				}
				set
				{
					this.label.Text = value;
				}
			}

			public ListPanelSectionItem()
			{
				this.label = new Label();
				this.AddChildLast(this.label);
				this.BackgroundColor = new UIColor(0.5f, 0.5f, 0.5f, 0.8f);
				base.HideEdge = true;
			}
		}

		private const float defaultSectionHeight = 47f;

		private const int defaultSectionFontSize = 30;

		private ListSectionCollection sections;

		private bool showSection;

		private bool showEmptySection;

		private bool showItemBorder;

		private ScrollBarVisibility scrollBarVisibility = ScrollBarVisibility.ScrollableVisible;

		private UISprite backgroundSprt;

		private ScrollBar scrollBar;

		private ListItemCreator itemCreator;

		private ListItemUpdater itemUpdater;

		private List<ListPanelItem> displayAllItems;

		private List<ListPanelItem> displayListItems;

		private List<ListPanelItem> cacheListItems;

		private List<ListPanel.ListPanelSectionItem> displaySectionItems;

		private List<ListPanel.ListPanelSectionItem> cacheSectionItems;

		private ListPanel.AnimationState animationState;

		private ListPanel.TerminalState terminalState;

		private bool animation;

		private float terminalDistance;

		private float scrollVelocity;

		private float standardItemHeight = -1f;

		private float flickStartRatio = 0.022f;

		private float flickDecelerationRatio = 0.012f;

		private float terminalDistanceRatio = 0.073f;

		private float terminalDecelerationRatio = 0.48f;

		private float maxTerminalDistance = 240f;

		private float terminalReturnRatio = 0.09f;

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
					this.backgroundSprt.GetUnit(0).Width = value;
				}
				if (this.scrollBar != null)
				{
					this.scrollBar.X = value - this.scrollBar.Width;
				}
				if (this.displayAllItems != null)
				{
					using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ListPanelItem current = enumerator.Current;
							if (current != null)
							{
								current.Width = value;
							}
						}
					}
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
					this.backgroundSprt.GetUnit(0).Height = value;
				}
				if (this.scrollBar != null)
				{
					this.scrollBar.Height = value;
				}
			}
		}

		public UIColor BackgroundColor
		{
			get
			{
				return this.backgroundSprt.GetUnit(0).Color;
			}
			set
			{
				this.backgroundSprt.GetUnit(0).Color = value;
			}
		}

		public ListSectionCollection Sections
		{
			get
			{
				return this.sections;
			}
			set
			{
				this.sections = value;
				if (this.sections != null)
				{
					this.sections.ItemsChanged += new EventHandler<EventArgs>(this.SectionsItemChanged);
					this.Refresh();
				}
			}
		}

		public int AllItemCount
		{
			get
			{
				if (this.sections != null)
				{
					return this.Sections.AllItemCount;
				}
				return 0;
			}
		}

		public bool ShowSection
		{
			get
			{
				return this.showSection;
			}
			set
			{
				if (this.showSection != value)
				{
					this.showSection = value;
					this.Refresh();
				}
			}
		}

		public bool ShowEmptySection
		{
			get
			{
				return this.showEmptySection;
			}
			set
			{
				if (this.showEmptySection != value)
				{
					this.showEmptySection = value;
					this.Refresh();
				}
			}
		}

		public bool ShowItemBorder
		{
			get
			{
				return this.showItemBorder;
			}
			set
			{
				if (this.showItemBorder != value)
				{
					this.showItemBorder = value;
					this.Refresh();
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
				this.UpdateScrollBarVisible();
			}
		}

		private int MaxDisplayListItemAllIndex
		{
			get
			{
				if (this.displayListItems.Count == 0)
				{
					return -1;
				}
				return this.displayListItems[this.displayListItems.Count - 1].Index;
			}
		}

		private int MinDisplayListItemAllIndex
		{
			get
			{
				if (this.displayListItems.Count == 0)
				{
					return -1;
				}
				return this.displayListItems[0].Index;
			}
		}

		public ListPanel()
		{
			this.backgroundSprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.backgroundSprt);
			UISpriteUnit unit = this.backgroundSprt.GetUnit(0);
			unit.Color = new UIColor(0f, 0f, 0f, 0f);
			this.scrollBar = new ScrollBar(ScrollBarOrientation.Vertical);
			this.AddChildLast(this.scrollBar);
			this.displayAllItems = new List<ListPanelItem>();
			this.displayListItems = new List<ListPanelItem>();
			this.cacheListItems = new List<ListPanelItem>();
			this.displaySectionItems = new List<ListPanel.ListPanelSectionItem>();
			this.cacheSectionItems = new List<ListPanel.ListPanelSectionItem>();
			this.animationState = ListPanel.AnimationState.None;
			this.terminalState = ListPanel.TerminalState.None;
			this.terminalDistance = 0f;
			this.Sections = null;
			this.ShowSection = true;
			this.ShowEmptySection = true;
			this.ShowItemBorder = true;
			base.Clip = true;
			base.HookChildTouchEvent = true;
			DragGestureDetector dragGestureDetector = new DragGestureDetector();
			dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(dragGestureDetector);
			FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
			flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(flickGestureDetector);
		}

		private void SectionsItemChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private void Refresh()
		{
			if (this.Sections == null || this.Sections.Count == 0)
			{
				return;
			}
			using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ListPanelItem current = enumerator.Current;
					if (current.IndexInSection == -1)
					{
						this.DestroySectionItem(current as ListPanel.ListPanelSectionItem);
					}
					else
					{
						this.DestroyListItem(current);
					}
				}
			}
			this.displayAllItems.Clear();
			this.animationState = ListPanel.AnimationState.None;
			this.terminalState = ListPanel.TerminalState.None;
			this.animation = false;
			this.UpdateItems(0f);
		}

		private void UpdateScrollBar()
		{
			if (this.displayListItems.Count != 0)
			{
				if (this.standardItemHeight < 0f)
				{
					this.standardItemHeight = this.displayListItems[0].Height;
				}
				int num = this.displayListItems[0].Index + (this.ShowSection ? (this.displayListItems[0].SectionIndex + 1) : 0);
				int num2 = 0;
				while (num2 < this.displayAllItems.Count && this.displayAllItems[num2] != null && this.displayAllItems[num2] is ListPanel.ListPanelSectionItem)
				{
					num--;
					num2++;
				}
				this.scrollBar.BarPosition = (float)num - this.displayAllItems[0].Y / this.displayAllItems[0].Height;
				this.scrollBar.BarLength = this.Height / (this.standardItemHeight + Math.Abs(this.terminalDistance) * this.terminalDistanceRatio);
				this.scrollBar.Length = (float)(this.AllItemCount + (this.ShowSection ? this.Sections.Count : 0));
				this.AddChildLast(this.scrollBar);
			}
			this.UpdateScrollBarVisible();
		}

		private void UpdateScrollBarVisible()
		{
			switch (this.scrollBarVisibility)
			{
			case ScrollBarVisibility.Visible:
				this.scrollBar.Visible = true;
				return;
			case ScrollBarVisibility.ScrollableVisible:
				this.scrollBar.Visible = (this.displayListItems.Count < this.AllItemCount);
				return;
			case ScrollBarVisibility.ScrollingVisible:
				this.scrollBar.Visible = (this.animationState != ListPanel.AnimationState.None || this.terminalState != ListPanel.TerminalState.None);
				return;
			case ScrollBarVisibility.Invisible:
				this.scrollBar.Visible = false;
				return;
			default:
				return;
			}
		}

		private ListPanelItem CreateListItem()
		{
			ListPanelItem listPanelItem;
			if (this.cacheListItems.Count == 0)
			{
				if (this.itemCreator != null)
				{
					listPanelItem = this.itemCreator();
				}
				else
				{
					listPanelItem = new ListPanelItem();
				}
				if (listPanelItem != null)
				{
					this.AddChildLast(listPanelItem);
				}
			}
			else
			{
				listPanelItem = this.cacheListItems[0];
				this.cacheListItems.Remove(listPanelItem);
			}
			listPanelItem.Width = this.Width;
			listPanelItem.HideEdge = !this.ShowItemBorder;
			listPanelItem.Visible = true;
			return listPanelItem;
		}

		private void DestroyListItem(ListPanelItem item)
		{
			item.Visible = false;
			this.displayListItems.Remove(item);
			this.cacheListItems.Add(item);
		}

		private void UpdateListItem(ListPanelItem item, int allIndex, int sectionIndex, int indexInSection)
		{
			item.Index = allIndex;
			item.SectionIndex = sectionIndex;
			item.IndexInSection = indexInSection;
			if (this.itemUpdater != null)
			{
				this.itemUpdater(item);
			}
			if (this.displayListItems.Count > 0 && this.displayListItems[0].Index > item.Index)
			{
				this.displayListItems.Insert(0, item);
				return;
			}
			this.displayListItems.Add(item);
		}

		private ListPanel.ListPanelSectionItem CreateSectionItem()
		{
			ListPanel.ListPanelSectionItem listPanelSectionItem;
			if (this.cacheSectionItems.Count == 0)
			{
				listPanelSectionItem = new ListPanel.ListPanelSectionItem();
				this.AddChildLast(listPanelSectionItem);
			}
			else
			{
				listPanelSectionItem = this.cacheSectionItems[0];
				this.cacheSectionItems.Remove(listPanelSectionItem);
			}
			listPanelSectionItem.Width = this.Width;
			listPanelSectionItem.Height = 47f;
			listPanelSectionItem.Visible = true;
			return listPanelSectionItem;
		}

		private void DestroySectionItem(ListPanel.ListPanelSectionItem item)
		{
			item.Visible = false;
			this.displaySectionItems.Remove(item);
			this.cacheSectionItems.Add(item);
		}

		private void UpdateSectionItem(ListPanel.ListPanelSectionItem item, int sectionIndex)
		{
			item.Title = this.Sections[sectionIndex].Title;
			item.SectionIndex = sectionIndex;
			if (this.displaySectionItems.Count > 0 && this.displaySectionItems[0].SectionIndex > item.SectionIndex)
			{
				this.displaySectionItems.Insert(0, item);
				return;
			}
			this.displaySectionItems.Add(item);
		}

		private int GetNextSectionIndex(int currentIndex)
		{
			if (!this.ShowEmptySection || !this.ShowSection)
			{
				for (int i = currentIndex + 1; i < this.Sections.Count; i++)
				{
					if (this.Sections[i].ItemCount != 0)
					{
						return i;
					}
				}
				return this.Sections.Count;
			}
			return currentIndex + 1;
		}

		private int GetPrevSectionIndex(int currentIndex)
		{
			if (!this.ShowEmptySection || !this.ShowSection)
			{
				for (int i = currentIndex - 1; i >= 0; i--)
				{
					if (this.Sections[i].ItemCount != 0)
					{
						return i;
					}
				}
				return -1;
			}
			return currentIndex - 1;
		}

		private void SetNextItem(float nextPos)
		{
			if (this.displayAllItems.Count != 0)
			{
				ListPanelItem listPanelItem = this.displayAllItems[this.displayAllItems.Count - 1];
				if (!this.IsBottomTerminalItem(listPanelItem))
				{
					if (this.ShowSection)
					{
						if (this.Sections[listPanelItem.SectionIndex].ItemCount == listPanelItem.IndexInSection + 1)
						{
							this.SetNextSectionItem(nextPos, this.GetNextSectionIndex(listPanelItem.SectionIndex));
							return;
						}
						this.SetNextListItem(nextPos, this.MaxDisplayListItemAllIndex + 1, listPanelItem.SectionIndex, listPanelItem.IndexInSection + 1);
						return;
					}
					else
					{
						if (this.Sections[listPanelItem.SectionIndex].ItemCount == listPanelItem.IndexInSection + 1)
						{
							this.SetNextListItem(nextPos, this.MaxDisplayListItemAllIndex + 1, this.GetNextSectionIndex(listPanelItem.SectionIndex), 0);
							return;
						}
						this.SetNextListItem(nextPos, this.MaxDisplayListItemAllIndex + 1, listPanelItem.SectionIndex, listPanelItem.IndexInSection + 1);
					}
				}
				return;
			}
			if (this.ShowSection)
			{
				this.SetNextSectionItem(nextPos, this.GetNextSectionIndex(-1));
				return;
			}
			this.SetNextListItem(nextPos, 0, this.GetNextSectionIndex(-1), 0);
		}

		private void SetNextSectionItem(float nextPos, int nextIndex)
		{
			if (nextIndex != this.Sections.Count)
			{
				ListPanel.ListPanelSectionItem listPanelSectionItem = this.CreateSectionItem();
				this.UpdateSectionItem(listPanelSectionItem, nextIndex);
				listPanelSectionItem.Y = nextPos;
				this.displayAllItems.Add(listPanelSectionItem);
			}
		}

		private void SetNextListItem(float nextPos, int nextAllIndex, int nextSctionIndex, int nextIndexInSection)
		{
			if (nextSctionIndex != this.Sections.Count)
			{
				ListPanelItem listPanelItem = this.CreateListItem();
				this.UpdateListItem(listPanelItem, nextAllIndex, nextSctionIndex, nextIndexInSection);
				listPanelItem.Y = nextPos;
				this.displayAllItems.Add(listPanelItem);
			}
		}

		private void SetPrevItem(float currentPos)
		{
			if (this.displayAllItems.Count == 0)
			{
				return;
			}
			ListPanelItem listPanelItem = this.displayAllItems[0];
			if (!this.IsTopTerminalItem(listPanelItem))
			{
				if (this.ShowSection)
				{
					if (listPanelItem.IndexInSection == -1)
					{
						int prevSectionIndex = this.GetPrevSectionIndex(listPanelItem.SectionIndex);
						if (this.Sections[prevSectionIndex].ItemCount == 0)
						{
							this.SetPrevSectionItem(currentPos, prevSectionIndex);
							return;
						}
						this.SetPrevListItem(currentPos, this.MinDisplayListItemAllIndex - 1, prevSectionIndex, this.Sections[prevSectionIndex].ItemCount - 1);
						return;
					}
					else
					{
						if (listPanelItem.IndexInSection == 0)
						{
							this.SetPrevSectionItem(currentPos, listPanelItem.SectionIndex);
							return;
						}
						this.SetPrevListItem(currentPos, this.MinDisplayListItemAllIndex - 1, listPanelItem.SectionIndex, listPanelItem.IndexInSection - 1);
						return;
					}
				}
				else
				{
					if (listPanelItem.IndexInSection == 0)
					{
						int prevSectionIndex2 = this.GetPrevSectionIndex(listPanelItem.SectionIndex);
						this.SetPrevListItem(currentPos, this.MinDisplayListItemAllIndex - 1, prevSectionIndex2, this.Sections[prevSectionIndex2].ItemCount - 1);
						return;
					}
					this.SetPrevListItem(currentPos, this.MinDisplayListItemAllIndex - 1, listPanelItem.SectionIndex, listPanelItem.IndexInSection - 1);
				}
			}
		}

		private void SetPrevSectionItem(float currentPos, int prevIndex)
		{
			if (prevIndex != -1)
			{
				ListPanel.ListPanelSectionItem listPanelSectionItem = this.CreateSectionItem();
				this.UpdateSectionItem(listPanelSectionItem, prevIndex);
				listPanelSectionItem.Y = currentPos - listPanelSectionItem.ContainEdgeHeight;
				this.displayAllItems.Insert(0, listPanelSectionItem);
			}
		}

		private void SetPrevListItem(float currentPos, int prevAllIndex, int prevSctionIndex, int prevIndexInSection)
		{
			if (prevSctionIndex != -1)
			{
				ListPanelItem listPanelItem = this.CreateListItem();
				this.UpdateListItem(listPanelItem, prevAllIndex, prevSctionIndex, prevIndexInSection);
				listPanelItem.Y = currentPos - listPanelItem.ContainEdgeHeight;
				this.displayAllItems.Insert(0, listPanelItem);
			}
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			if (!this.IsScrollable())
			{
				touchEvents.Forward = true;
				return;
			}
			if (this.animationState == ListPanel.AnimationState.None)
			{
				this.OnTouchEventImpl(touchEvents);
				touchEvents.Forward = true;
				return;
			}
			this.OnTouchEventImpl(touchEvents);
		}

		private void OnTouchEventImpl(TouchEventCollection touchEvents)
		{
			switch (touchEvents.PrimaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.animationState == ListPanel.AnimationState.Drag)
				{
					this.animationState = ListPanel.AnimationState.None;
					this.animation = false;
					switch (this.terminalState)
					{
					case ListPanel.TerminalState.Top:
						this.terminalState = ListPanel.TerminalState.TopReset;
						this.animation = true;
						break;
					case ListPanel.TerminalState.Bottom:
						this.terminalState = ListPanel.TerminalState.BottomReset;
						this.animation = true;
						break;
					}
				}
				if (this.ScrollBarVisibility == ScrollBarVisibility.ScrollingVisible && !this.animation)
				{
					this.scrollBar.Visible = false;
				}
				return;
			case TouchEventType.Down:
				switch (this.terminalState)
				{
				case ListPanel.TerminalState.None:
					this.animationState = ListPanel.AnimationState.None;
					this.animation = false;
					return;
				case ListPanel.TerminalState.Top:
				case ListPanel.TerminalState.TopReset:
					this.ResetTopTerminalItems();
					return;
				case ListPanel.TerminalState.Bottom:
				case ListPanel.TerminalState.BottomReset:
					this.ResetBottomTerminalItems();
					return;
				default:
					return;
				}
//				break;
			default:
				return;
			}
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			if (this.IsScrollable())
			{
				base.ResetState(false);
				this.animationState = ListPanel.AnimationState.Drag;
				this.UpdateItems(e.Distance.Y);
				this.animation = false;
			}
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			if (this.IsScrollable())
			{
				base.ResetState(false);
				this.animationState = ListPanel.AnimationState.Flick;
				this.animation = true;
				switch (this.terminalState)
				{
				case ListPanel.TerminalState.Top:
				case ListPanel.TerminalState.Bottom:
					this.scrollVelocity = 0f;
					return;
				default:
					this.scrollVelocity = e.Speed.Y * this.flickStartRatio;
					break;
				}
			}
		}

		private void UpdateTopTerminalItems()
		{
			if (this.displayAllItems.Count > 0)
			{
				if (this.terminalState == ListPanel.TerminalState.Top)
				{
					this.terminalDistance += this.displayAllItems[0].Y * 2f;
				}
				else
				{
					this.terminalDistance += this.displayAllItems[0].Y;
				}
			}
			if (this.terminalDistance > this.maxTerminalDistance)
			{
				this.terminalDistance = this.maxTerminalDistance;
				this.scrollVelocity = 0f;
				if (this.animationState == ListPanel.AnimationState.Flick)
				{
					this.terminalState = ListPanel.TerminalState.TopReset;
				}
			}
			if (this.terminalDistance > 0f)
			{
				float y = 0f;
				float num = this.terminalDistance * this.terminalDistanceRatio;
				using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ListPanelItem current = enumerator.Current;
						current.Y = y;
						y = current.Y + current.ContainEdgeHeight + num;
					}
				}
				if (this.terminalState == ListPanel.TerminalState.None)
				{
					this.terminalState = ListPanel.TerminalState.Top;
					return;
				}
			}
			else
			{
				this.ResetTopTerminalItems();
			}
		}

		private void UpdateBottomTerminalItems()
		{
			int num = this.displayAllItems.Count - 1;
			ListPanelItem listPanelItem = this.displayAllItems[num];
			if (this.displayAllItems.Count > 0)
			{
				if (this.terminalState == ListPanel.TerminalState.Bottom)
				{
					this.terminalDistance -= (this.Height - (listPanelItem.Y + listPanelItem.ContainEdgeHeight)) * 2f;
				}
				else
				{
					this.terminalDistance -= this.Height - (listPanelItem.Y + listPanelItem.ContainEdgeHeight);
				}
			}
			if (this.terminalDistance < -this.maxTerminalDistance)
			{
				this.terminalDistance = -this.maxTerminalDistance;
				this.scrollVelocity = 0f;
				if (this.animationState == ListPanel.AnimationState.Flick)
				{
					this.terminalState = ListPanel.TerminalState.BottomReset;
				}
			}
			if (this.terminalDistance < 0f)
			{
				float num2 = this.Height;
				float num3 = this.terminalDistance * this.terminalDistanceRatio;
				for (int i = num; i >= 0; i--)
				{
					ListPanelItem listPanelItem2 = this.displayAllItems[i];
					listPanelItem2.Y = num2 - listPanelItem2.ContainEdgeHeight;
					num2 = listPanelItem2.Y + num3;
				}
				if (this.terminalState == ListPanel.TerminalState.None)
				{
					this.terminalState = ListPanel.TerminalState.Bottom;
					return;
				}
			}
			else
			{
				this.ResetBottomTerminalItems();
			}
		}

		private void ResetTopTerminalItems()
		{
			float y = 0f;
			using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ListPanelItem current = enumerator.Current;
					current.Y = y;
					y = current.Y + current.ContainEdgeHeight;
				}
			}
			this.terminalState = ListPanel.TerminalState.None;
			this.terminalDistance = 0f;
			this.animationState = ListPanel.AnimationState.None;
			this.animation = false;
		}

		private void ResetBottomTerminalItems()
		{
			float num = this.Height;
			for (int i = this.displayAllItems.Count - 1; i >= 0; i--)
			{
				ListPanelItem listPanelItem = this.displayAllItems[i];
				listPanelItem.Y = num - listPanelItem.ContainEdgeHeight;
				num = listPanelItem.Y;
			}
			this.terminalState = ListPanel.TerminalState.None;
			this.terminalDistance = 0f;
			this.animationState = ListPanel.AnimationState.None;
			this.animation = false;
		}

		private void UpdateItems(float scrollDistance)
		{
			if (this.sections == null || this.sections.Count == 0)
			{
				return;
			}
			if (this.displayAllItems.Count == 0)
			{
				float num = 0f;
				while (num < this.Height)
				{
					this.SetNextItem(num);
					if (this.displayAllItems.Count == 0)
					{
						break;
					}
					ListPanelItem listPanelItem = this.displayAllItems[this.displayAllItems.Count - 1];
					num = listPanelItem.Y + listPanelItem.ContainEdgeHeight;
					if (this.IsBottomTerminalItem(listPanelItem))
					{
						break;
					}
				}
			}
			else
			{
				using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ListPanelItem current = enumerator.Current;
						current.Y += scrollDistance;
					}
				}
				switch (this.terminalState)
				{
				case ListPanel.TerminalState.None:
				{
					ListPanelItem listPanelItem2 = this.displayAllItems[0];
					ListPanelItem listPanelItem3 = this.displayAllItems[this.displayAllItems.Count - 1];
					float num2 = listPanelItem3.Y + listPanelItem3.ContainEdgeHeight;
					while (listPanelItem2.Y > 0f)
					{
						if (this.IsTopTerminalItem(listPanelItem2))
						{
							this.UpdateTopTerminalItems();
//							IL_1C6:
							while (num2 < this.Height)
							{
								if (this.IsTopTerminalItem(listPanelItem2) && num2 - listPanelItem2.Y < this.Height)
								{
									this.UpdateTopTerminalItems();
									break;
								}
								if (this.IsBottomTerminalItem(listPanelItem3))
								{
									this.UpdateBottomTerminalItems();
									break;
								}
								this.SetNextItem(num2);
								listPanelItem3 = this.displayAllItems[this.displayAllItems.Count - 1];
								num2 = listPanelItem3.Y + listPanelItem3.ContainEdgeHeight;
							}
							goto IL_1E0;
						}
						this.SetPrevItem(listPanelItem2.Y);
						listPanelItem2 = this.displayAllItems[0];
					}
//					goto IL_1C6;
					while (num2 < this.Height)
					{
						if (this.IsTopTerminalItem(listPanelItem2) && num2 - listPanelItem2.Y < this.Height)
						{
							this.UpdateTopTerminalItems();
							break;
						}
						if (this.IsBottomTerminalItem(listPanelItem3))
						{
							this.UpdateBottomTerminalItems();
							break;
						}
						this.SetNextItem(num2);
						listPanelItem3 = this.displayAllItems[this.displayAllItems.Count - 1];
						num2 = listPanelItem3.Y + listPanelItem3.ContainEdgeHeight;
					}
					goto IL_1E0;
				}
				case ListPanel.TerminalState.Top:
				case ListPanel.TerminalState.TopReset:
					this.UpdateTopTerminalItems();
					break;
				case ListPanel.TerminalState.Bottom:
				case ListPanel.TerminalState.BottomReset:
					this.UpdateBottomTerminalItems();
					break;
				}
				IL_1E0:
				if (this.terminalState == ListPanel.TerminalState.None)
				{
					List<ListPanelItem> list = new List<ListPanelItem>();
					using (List<ListPanelItem>.Enumerator enumerator2 = this.displayAllItems.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ListPanelItem current2 = enumerator2.Current;
							if (current2.Y + current2.ContainEdgeHeight < 0f || current2.Y > this.Height)
							{
								list.Add(current2);
							}
						}
					}
					using (List<ListPanelItem>.Enumerator enumerator3 = list.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ListPanelItem current3 = enumerator3.Current;
							if (current3 is ListPanel.ListPanelSectionItem)
							{
								this.DestroySectionItem(current3 as ListPanel.ListPanelSectionItem);
							}
							else
							{
								this.DestroyListItem(current3);
							}
							this.displayAllItems.Remove(current3);
						}
					}
				}
			}
			this.UpdateScrollBar();
		}

		private bool IsScrollable()
		{
			int num = 0;
			if (this.ShowSection)
			{
				num = this.Sections.Count;
				if (!this.ShowEmptySection)
				{
					foreach (ListSection current in this.Sections)
					{
						if (current.ItemCount <= 0)
						{
							num--;
						}
					}
				}
			}
			if (this.displayAllItems.Count >= this.AllItemCount + num)
			{
				float num2 = 0f;
				using (List<ListPanelItem>.Enumerator enumerator2 = this.displayAllItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ListPanelItem current2 = enumerator2.Current;
						num2 += current2.ContainEdgeHeight;
					}
				}
				if (num2 <= this.Height)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTopTerminalItem(ListPanelItem item)
		{
			if (this.ShowSection)
			{
				if (item.IndexInSection != -1)
				{
					return false;
				}
				if (!this.ShowEmptySection)
				{
					for (int i = 0; i < this.Sections.Count; i++)
					{
						if (this.Sections[i].ItemCount != 0)
						{
							return i == item.SectionIndex;
						}
					}
				}
				else if (item.SectionIndex == 0)
				{
					return true;
				}
			}
			else if (item.Index == 0)
			{
				return true;
			}
			return false;
		}

		private bool IsBottomTerminalItem(ListPanelItem item)
		{
			if (this.ShowSection)
			{
				if (!this.ShowEmptySection)
				{
					for (int i = this.Sections.Count - 1; i >= 0; i--)
					{
						int num = i;
						int itemCount = this.Sections[num].ItemCount;
						if (this.Sections[num].ItemCount != 0)
						{
							return item.SectionIndex == num && item.IndexInSection + 1 == itemCount;
						}
					}
				}
				else
				{
					int num2 = this.Sections.Count - 1;
					int itemCount2 = this.Sections[num2].ItemCount;
					if (item.SectionIndex == num2 && item.IndexInSection + 1 == itemCount2)
					{
						return true;
					}
				}
			}
			else if (this.AllItemCount == item.Index + 1)
			{
				return true;
			}
			return false;
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (!this.animation)
			{
				return;
			}
			ListPanel.AnimationState animationState = this.animationState;
			if (animationState == ListPanel.AnimationState.Flick)
			{
				if (this.terminalState == ListPanel.TerminalState.None)
				{
					this.scrollVelocity *= (float)Math.Pow((double)(1f - this.flickDecelerationRatio), (double)(elapsedTime / 16.6f));
				}
				else
				{
					this.scrollVelocity *= (float)Math.Pow((double)(1f - this.terminalDecelerationRatio), (double)(elapsedTime / 16.6f));
				}
				if (Math.Abs(this.scrollVelocity * elapsedTime / 16.6f) < 0.03012048f || Math.Abs(this.terminalDistance) >= this.maxTerminalDistance)
				{
					this.scrollVelocity = 0f;
					this.animationState = ListPanel.AnimationState.None;
					switch (this.terminalState)
					{
					case ListPanel.TerminalState.Top:
						this.terminalState = ListPanel.TerminalState.TopReset;
						this.animation = true;
						break;
					case ListPanel.TerminalState.Bottom:
						this.terminalState = ListPanel.TerminalState.BottomReset;
						this.animation = true;
						break;
					default:
						this.animation = false;
						break;
					}
				}
				else
				{
					this.UpdateItems(this.scrollVelocity);
				}
			}
			switch (this.terminalState)
			{
			case ListPanel.TerminalState.TopReset:
				if (this.terminalDistance < 1f)
				{
					this.ResetTopTerminalItems();
				}
				else
				{
					this.UpdateItems(this.terminalDistance * (float)Math.Pow((double)(1f - this.terminalReturnRatio), (double)(elapsedTime / 16.6f)) - this.terminalDistance);
				}
				break;
			case ListPanel.TerminalState.BottomReset:
				if (this.terminalDistance > -1f)
				{
					this.ResetBottomTerminalItems();
				}
				else
				{
					this.UpdateItems(this.terminalDistance * (float)Math.Pow((double)(1f - this.terminalReturnRatio), (double)(elapsedTime / 16.6f)) - this.terminalDistance);
				}
				break;
			}
			this.UpdateScrollBarVisible();
		}

		public void SetListItemCreator(ListItemCreator creator)
		{
			this.itemCreator = creator;
			this.standardItemHeight = -1f;
		}

		public void SetListItemUpdater(ListItemUpdater updater)
		{
			this.itemUpdater = updater;
			this.standardItemHeight = -1f;
		}

		public void Move(float moveDistance)
		{
			this.UpdateItems(moveDistance);
			switch (this.terminalState)
			{
			case ListPanel.TerminalState.Top:
			case ListPanel.TerminalState.TopReset:
				this.ResetTopTerminalItems();
				return;
			case ListPanel.TerminalState.Bottom:
			case ListPanel.TerminalState.BottomReset:
				this.ResetBottomTerminalItems();
				return;
			default:
				return;
			}
		}

		public void UpdateItems()
		{
			if (this.itemUpdater != null)
			{
				using (List<ListPanelItem>.Enumerator enumerator = this.displayAllItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ListPanelItem current = enumerator.Current;
						this.itemUpdater(current);
					}
				}
			}
		}
	}
}
