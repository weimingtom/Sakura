using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveListPanel : Widget
	{
		private enum AnimationState
		{
			None,
			Drag,
			Flick
		}

		public class ListItemData
		{
			public ListPanelItem item;

			public float rotationZ;

			public Vector3 slideStartPos;

			public Vector3 slideEndPos;

			public float slideStartRotationZ;

			public float slideEndRotationZ;

			public float slideStartRotationY;

			public float scrollEndPosX;

			public float scrollStartRotationZ;

			public float scrollEndRotationZ;

			public float totalScrollIncrement;

			public float SlideElapsedTime;

			public bool IsSliding;

			public bool IsScrolling;
		}

		private const float defaultLiveListPanelWidth = 320f;

		private const float defaultLiveListPanelHeight = 480f;

		private const float defaultItemLineGap = 32f;

		private const float defaultItemHeight = 95f;

		private const int ELAPSED_TIME_COUNT = 60;

		private float animationTime = 1500f;

		private float itemSideMargin = 10f;

		private float itemTiltAngle = 0.1f;

		private float itemOffsetX = 10f;

		private float itemSlideInOffsetX = 200f;

		private float itemSlideInHeight = -500f;

		private float itemSlideInRotationY;

		private float itemSlideInTime = 500f;

		private float itemSlideInDelay = 100f;

		private float terminalAnimationFactor = 1f;

		private float terminalDecay = 2f;

		private ContainerWidget itemContainerPanel;

		private ScrollBar scrollBar;

		private UISprite sprt;

		private ListItemCreator itemCreator;

		private ListItemUpdater itemUpdater;

		private List<LiveListPanel.ListItemData> usingItemDataList;

		private List<LiveListPanel.ListItemData> cacheItemDataList;

		private FourWayDirection scrollDirection;

		private FourWayDirection prevScrollDirection;

		private LiveListPanel.AnimationState animationState;

		private float flickDistance;

		private float flickStartDistance;

		private float flickElapsedTime;

		private float terminalResetElapsedTime;

		private float scrollPosition;

		private int scrollBaseIndex = -1;

		private bool needRefresh = true;

		private bool isScrolling;

		private float scrollGravity = 0.3f;

		private int slideInRotationDelta = 1;

		public float slideInOffsetX;

		private bool isMoveX = true;

		private bool isMoveZ = true;

		private float rotationSpeed = 0.5f;

		private float[] elapsedTimes;

		private int elapsedTimeIndex;

		private bool initElapsedTimes;

		private static Random rand = new Random();

		private ScrollBarVisibility scrollBarVisibility = ScrollBarVisibility.ScrollingVisible;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.sprt != null)
				{
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.Width = value;
				}
				if (this.scrollBar != null)
				{
					this.scrollBar.X = value - this.scrollBar.Width;
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
				if (this.sprt != null)
				{
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.Height = value;
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
				UISpriteUnit unit = this.sprt.GetUnit(0);
				return unit.Color;
			}
			set
			{
				UISpriteUnit unit = this.sprt.GetUnit(0);
				unit.Color = value;
			}
		}

		public float ItemHeight
		{
			get;
			set;
		}

		public float ItemWidth
		{
			get
			{
				return this.Width - this.itemSideMargin * 2f;
			}
			set
			{
				this.itemSideMargin = (this.Width - value) / 2f;
			}
		}

		public int ItemCount
		{
			get;
			set;
		}

		public float ItemVerticalGap
		{
			get;
			set;
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

		public float ItemTiltAngle
		{
			get
			{
				return this.itemTiltAngle;
			}
			set
			{
				this.itemTiltAngle = value;
			}
		}

		public float ItemSlideInTime
		{
			get
			{
				return this.itemSlideInTime;
			}
			set
			{
				this.itemSlideInTime = value;
			}
		}

		public float ItemSlideInHeight
		{
			get
			{
				return -this.itemSlideInHeight;
			}
			set
			{
				this.itemSlideInHeight = -value;
			}
		}

		public float ItemSlideInTiltAngle
		{
			get
			{
				return this.itemSlideInRotationY;
			}
			set
			{
				this.itemSlideInRotationY = value;
			}
		}

		public float ItemSlideInOffset
		{
			get
			{
				return this.itemSlideInOffsetX;
			}
			set
			{
				this.itemSlideInOffsetX = value;
			}
		}

		private int ScrollAreaItemCount
		{
			get
			{
				float num = 0f;
				if (0f < this.scrollPosition && this.scrollPosition < this.MaxScrollPosition)
				{
					num = this.scrollPosition - this.LineHeight * (float)this.ScrollAreaFirstItemIndex;
				}
				return (int)((this.Height + num) / this.LineHeight + 1f);
			}
		}

		private int ScrollAreaFirstItemIndex
		{
			get
			{
				if (this.scrollPosition <= 0f)
				{
					return 0;
				}
				if (this.scrollPosition <= this.MaxScrollPosition)
				{
					return (int)(this.scrollPosition / this.LineHeight);
				}
				int num = this.ItemCount - this.ScrollAreaItemCount;
				if (num <= 0)
				{
					return 0;
				}
				return num;
			}
		}

		private int ScrollAreaLastItemIndex
		{
			get
			{
				if (this.ItemCount == 0)
				{
					return 0;
				}
				if (this.ItemCount < this.ScrollAreaItemCount)
				{
					return this.ItemCount - 1;
				}
				return this.ScrollAreaFirstItemIndex + this.ScrollAreaItemCount - 1;
			}
		}

		private float ScrollAreaFirstItemOffset
		{
			get
			{
				return -(this.scrollPosition - this.LineHeight * (float)this.ScrollAreaFirstItemIndex);
			}
		}

		private int MaxScrollAreaFirstItemIndex
		{
			get
			{
				return Math.Max(this.ItemCount - this.ScrollAreaItemCount, 0);
			}
		}

		private float MaxScrollPosition
		{
			get
			{
				float num = this.LineHeight * (float)this.ItemCount - this.Height;
				if (num <= 0f)
				{
					return 0f;
				}
				return num;
			}
		}

		private float LineHeight
		{
			get
			{
				return this.ItemHeight + this.ItemVerticalGap;
			}
		}

		public LiveListPanel()
		{
			this.sprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.sprt);
			this.itemContainerPanel = new ContainerWidget();
			base.AddChildLast(this.itemContainerPanel);
			this.scrollBar = new ScrollBar(ScrollBarOrientation.Vertical);
			base.AddChildLast(this.scrollBar);
			this.BackgroundColor = default(UIColor);
			this.ItemCount = 0;
			this.ItemVerticalGap = 32f;
			this.ItemHeight = 95f;
			this.usingItemDataList = new List<LiveListPanel.ListItemData>();
			this.cacheItemDataList = new List<LiveListPanel.ListItemData>();
			this.Width = 320f;
			this.Height = 480f;
			base.Clip = true;
			base.HookChildTouchEvent = true;
			DragGestureDetector dragGestureDetector = new DragGestureDetector();
			dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
			base.AddGestureDetector(dragGestureDetector);
			FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
			flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
			base.AddGestureDetector(flickGestureDetector);
			this.elapsedTimes = new float[60];
		}

		public void StartItemRequest()
		{
		}

		private void refresh()
		{
			this.usingItemDataList.Clear();
			this.cacheItemDataList.Clear();
			this.scrollPosition = 0f;
			this.UpdateListItemData();
		}

		private void UpdateScrollBar()
		{
			if (this.usingItemDataList.Count != 0)
			{
				this.scrollBar.BarPosition = this.scrollPosition;
				this.scrollBar.BarLength = this.Height;
				this.scrollBar.Length = (float)this.ItemCount * this.LineHeight;
				if (this.scrollPosition < 0f)
				{
					this.scrollBar.BarLength /= (float)Math.Pow(2.0, (double)(-(double)this.scrollPosition / this.Height));
				}
				else if (this.scrollPosition > this.MaxScrollPosition)
				{
					this.scrollBar.BarLength /= (float)Math.Pow(2.0, (double)((this.scrollPosition - this.MaxScrollPosition) / this.Height));
					this.scrollBar.BarPosition = this.scrollBar.Length;
				}
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
				this.scrollBar.Visible = (this.usingItemDataList.Count < this.ItemCount);
				return;
			case ScrollBarVisibility.ScrollingVisible:
				this.scrollBar.Visible = (this.animationState != LiveListPanel.AnimationState.None || this.isScrolling);
				return;
			case ScrollBarVisibility.Invisible:
				this.scrollBar.Visible = false;
				return;
			default:
				return;
			}
		}

		private LiveListPanel.ListItemData CreateListItemData(int index)
		{
			if (this.itemCreator == null)
			{
				return null;
			}
			LiveListPanel.ListItemData data = null;
			if (this.cacheItemDataList.Count == 0)
			{
				data = new LiveListPanel.ListItemData();
				data.item = this.itemCreator();
				data.item.PivotType = PivotType.MiddleCenter;
			}
			else
			{
				data = this.cacheItemDataList[0];
				this.cacheItemDataList.RemoveAt(0);
			}
			ListPanelItem item = data.item;
			item.Width = this.ItemWidth;
			item.Height = this.ItemHeight;
			item.Transform3D = Matrix4.Identity;
			item.Index = index;
			item.Visible = true;
			item.Y = (float)item.Index * this.LineHeight + this.LineHeight / 2f - this.scrollPosition;
			if (this.itemUpdater != null)
			{
				this.itemUpdater(item);
			}
			if (this.usingItemDataList.Count == 0 || index > this.ScrollAreaFirstItemIndex)
			{
				if (!this.usingItemDataList.Exists((LiveListPanel.ListItemData listData) => listData.item.Index == data.item.Index))
				{
					this.usingItemDataList.Add(data);
				}
				else
				{
					item.Visible = false;
					this.cacheItemDataList.Add(data);
				}
			}
			else if (!this.usingItemDataList.Exists((LiveListPanel.ListItemData listData) => listData.item.Index == data.item.Index))
			{
				this.usingItemDataList.Insert(0, data);
			}
			else
			{
				item.Visible = false;
				this.cacheItemDataList.Add(data);
			}
			this.SetupItemDataForSlide(data);
			return data;
		}

		private void DestroyListItemData(LiveListPanel.ListItemData data)
		{
			ListPanelItem item = data.item;
			item.Visible = false;
			this.usingItemDataList.Remove(data);
			if (!this.cacheItemDataList.Exists((LiveListPanel.ListItemData listData) => listData.item.Index == data.item.Index))
			{
				this.cacheItemDataList.Add(data);
			}
		}

		private void UpdateTransform3D(LiveListPanel.ListItemData data, float elapsedTime)
		{
			ListPanelItem item = data.item;
			Vector3 vector;
			float num;
			float num2;
			if (data.IsSliding)
			{
				if (data.SlideElapsedTime > 0f)
				{
					vector = new Vector3(AnimationUtility.EaseOutQuadInterpolator(data.slideStartPos.X, data.slideEndPos.X, data.SlideElapsedTime / this.itemSlideInTime), AnimationUtility.EaseOutQuadInterpolator(data.slideStartPos.Y, data.slideEndPos.Y, data.SlideElapsedTime / this.itemSlideInTime), AnimationUtility.EaseOutQuintInterpolator(data.slideStartPos.Z, data.slideEndPos.Z, data.SlideElapsedTime / this.itemSlideInTime));
					num = AnimationUtility.EaseOutQuadInterpolator(data.slideStartRotationZ, data.slideEndRotationZ, data.SlideElapsedTime / this.itemSlideInTime);
					num2 = AnimationUtility.EaseOutQuadInterpolator(data.slideStartRotationY, 0f, data.SlideElapsedTime / this.itemSlideInTime);
				}
				else
				{
					vector = data.slideStartPos;
					num = data.slideStartRotationZ;
					num2 = data.slideStartRotationY;
					if (this.scrollDirection == FourWayDirection.Up)
					{
						item.Y = data.slideStartPos.Y + this.Height;
					}
				}
			}
			else
			{
				vector = data.slideEndPos;
				num = data.slideEndRotationZ;
				num2 = 0f;
			}
			float num3 = (this.scrollPosition < 0f) ? 0f : ((this.scrollPosition > this.MaxScrollPosition) ? this.MaxScrollPosition : this.scrollPosition);
			if (this.animationState == LiveListPanel.AnimationState.Drag || this.animationState == LiveListPanel.AnimationState.Flick)
			{
				if ((this.scrollDirection == FourWayDirection.Down && item.Index >= this.scrollBaseIndex) || (this.scrollDirection == FourWayDirection.Up && item.Index <= this.scrollBaseIndex))
				{
					vector.Y += (float)item.Index * this.LineHeight + this.LineHeight / 2f - num3;
				}
				else
				{
					vector.Y = this.GetItemPosYWithSpring(data, num3, elapsedTime);
				}
			}
			else
			{
				vector.Y = this.GetItemPosYWithSpring(data, num3, elapsedTime);
			}
			vector.X += this.Width / 2f;
			if (this.isMoveX)
			{
				vector.X += (data.scrollEndPosX - data.slideEndPos.X) * (vector.Y - data.slideEndPos.Y) / this.Height;
			}
			if (this.isMoveZ)
			{
				data.slideEndRotationZ = MathUtility.Lerp(data.scrollStartRotationZ, data.scrollEndRotationZ, data.totalScrollIncrement / (this.Height * (1.01f - this.rotationSpeed)));
			}
			float num4 = 0f;
			if (this.scrollPosition < 0f || this.scrollPosition > this.MaxScrollPosition)
			{
				float num5 = AnimationUtility.EaseOutQuadInterpolator(this.terminalAnimationFactor, 0f, FMath.Clamp(this.terminalResetElapsedTime / 100f, 0f, 1f));
				if (this.scrollPosition < 0f)
				{
					num5 *= (float)Math.Max(4 - item.Index, 0);
				}
				else
				{
					num5 *= (float)Math.Max(4 - (this.ItemCount - 1 - item.Index), 0);
				}
				num4 += (float)(LiveListPanel.rand.NextDouble() - 0.5) / 10f * num5;
				num2 += (float)(LiveListPanel.rand.NextDouble() - 0.5) / 20f * num5;
				num += (float)(LiveListPanel.rand.NextDouble() - 0.5) / 40f * num5;
				vector.X += (float)(LiveListPanel.rand.NextDouble() - 0.5) * 4f * num5;
				vector.Y += (float)(LiveListPanel.rand.NextDouble() - 0.5) * 4f * num5;
				vector.Z += (float)(LiveListPanel.rand.NextDouble() - 0.5) * 4f * num5;
			}
			item.Transform3D = Matrix4.Translation(vector) * Matrix4.RotationZ(num) * Matrix4.RotationY(num2) * Matrix4.RotationX(num4);
		}

		private float GetItemPosYWithSpring(LiveListPanel.ListItemData data, float scrollPos, float elapsedTime)
		{
			ListPanelItem item = data.item;
			float num = 0f;
			if (this.scrollDirection == FourWayDirection.Up)
			{
				if (item.Index <= this.ScrollAreaFirstItemIndex)
				{
					num = item.Y - ((float)item.Index * this.LineHeight + this.LineHeight / 2f - scrollPos);
				}
				else
				{
					ListPanelItem item2 = this.GetItem(item.Index - 1);
					if (item2 != null)
					{
						num = item.Y - (item2.Y + this.LineHeight);
					}
				}
			}
			else if (this.scrollDirection == FourWayDirection.Down)
			{
				if (item.Index >= this.ScrollAreaLastItemIndex)
				{
					num = item.Y - ((float)item.Index * this.LineHeight + this.LineHeight / 2f - scrollPos);
				}
				else
				{
					ListPanelItem item3 = this.GetItem(item.Index + 1);
					if (item3 != null)
					{
						num = item.Y - (item3.Y - this.LineHeight);
					}
				}
			}
			float num2;
			if (this.animationState == LiveListPanel.AnimationState.Drag)
			{
				num2 = this.scrollGravity + 0.01f * (FMath.Abs((float)(this.scrollBaseIndex - item.Index)) - 1f);
			}
			else if (this.animationState == LiveListPanel.AnimationState.Flick)
			{
				num2 = this.scrollGravity + 0.05f * FMath.Abs((float)(this.scrollBaseIndex - item.Index));
			}
			else
			{
				num2 = this.scrollGravity + 0.05f * FMath.Abs((float)(this.scrollBaseIndex - item.Index));
			}
			float num3 = elapsedTime / 16.6f;
			num2 = FMath.Clamp(num2, -1f, 1f);
			float num4 = num * num2 * num3;
			if (num < 0f)
			{
				num4 = FMath.Clamp(num4, num, 0f);
			}
			else
			{
				num4 = FMath.Clamp(num4, 0f, num);
			}
			data.IsScrolling = (FMath.Abs(num) > 1f);
			return item.Y - num4;
		}

		private ListPanelItem GetItem(int itemIndex)
		{
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LiveListPanel.ListItemData current = enumerator.Current;
					if (current.item.Index == itemIndex)
					{
						return current.item;
					}
				}
			}
			return null;
		}

		private void SetupItemDataForSlide(LiveListPanel.ListItemData itemData)
		{
			itemData.slideEndRotationZ = (float)LiveListPanel.rand.NextDouble() * this.itemTiltAngle;
			if ((itemData.item.Index & 1) == 0)
			{
				itemData.slideStartPos = new Vector3(this.Width * this.slideInOffsetX, 0f, this.itemSlideInHeight);
				itemData.slideEndPos = new Vector3((float)LiveListPanel.rand.NextDouble() * this.itemOffsetX, 0f, 0f);
				itemData.scrollEndPosX = -(float)LiveListPanel.rand.NextDouble() * this.itemOffsetX;
				if (this.scrollDirection == FourWayDirection.Down)
				{
					itemData.slideEndRotationZ = -itemData.slideEndRotationZ;
				}
				itemData.slideStartRotationY = this.itemSlideInRotationY;
			}
			else
			{
				itemData.slideStartPos = new Vector3(-this.Width * this.slideInOffsetX, 0f, this.itemSlideInHeight);
				itemData.slideEndPos = new Vector3(-(float)LiveListPanel.rand.NextDouble() * this.itemOffsetX, 0f, 0f);
				itemData.scrollEndPosX = (float)LiveListPanel.rand.NextDouble() * this.itemOffsetX;
				if (this.scrollDirection == FourWayDirection.Up)
				{
					itemData.slideEndRotationZ = -itemData.slideEndRotationZ;
				}
				itemData.slideStartRotationY = -this.itemSlideInRotationY;
			}
			itemData.scrollStartRotationZ = itemData.slideEndRotationZ;
			itemData.scrollEndRotationZ = itemData.slideEndRotationZ;
			itemData.slideStartPos.Y = itemData.slideEndRotationZ * itemData.slideStartPos.X;
			itemData.slideStartRotationZ = itemData.slideEndRotationZ * (float)this.slideInRotationDelta;
			itemData.SlideElapsedTime = -this.itemSlideInDelay;
			itemData.IsSliding = true;
			this.itemContainerPanel.AddChildLast(itemData.item);
		}

		private void UpdateListItemData()
		{
			this.UpdateListItemData(0f);
		}

		private void UpdateListItemData(float elapsedTime)
		{
			List<LiveListPanel.ListItemData> list = new List<LiveListPanel.ListItemData>();
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LiveListPanel.ListItemData current = enumerator.Current;
					if (current.item.Index < this.ScrollAreaFirstItemIndex || current.item.Index > this.ScrollAreaLastItemIndex)
					{
						list.Add(current);
					}
				}
			}
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LiveListPanel.ListItemData current2 = enumerator2.Current;
					this.DestroyListItemData(current2);
				}
			}
			int num;
			if (this.usingItemDataList.Count == 0)
			{
				num = this.ScrollAreaFirstItemIndex - 1;
			}
			else
			{
				num = this.usingItemDataList[this.usingItemDataList.Count - 1].item.Index;
			}
			for (int i = num + 1; i <= this.ScrollAreaLastItemIndex; i++)
			{
				this.CreateListItemData(i);
			}
			int num2;
			if (this.usingItemDataList.Count == 0)
			{
				num2 = this.ScrollAreaLastItemIndex + 1;
			}
			else
			{
				num2 = this.usingItemDataList[0].item.Index;
			}
			for (int j = num2 - 1; j >= this.ScrollAreaFirstItemIndex; j--)
			{
				this.CreateListItemData(j);
			}
			if (this.scrollDirection == FourWayDirection.Up)
			{
				using (List<LiveListPanel.ListItemData>.Enumerator enumerator3 = this.usingItemDataList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						LiveListPanel.ListItemData current3 = enumerator3.Current;
						this.UpdateTransform3D(current3, elapsedTime);
					}
					goto IL_1B8;
				}
			}
			for (int k = this.usingItemDataList.Count - 1; k >= 0; k--)
			{
				LiveListPanel.ListItemData data = this.usingItemDataList[k];
				this.UpdateTransform3D(data, elapsedTime);
			}
			IL_1B8:
			this.UpdateScrollBar();
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			elapsedTime = this.GetElapsedTimeMovingAverage(elapsedTime);
			if (this.needRefresh)
			{
				this.refresh();
				this.needRefresh = false;
			}
			bool flag = false;
			if (this.scrollPosition < 0f || this.scrollPosition > this.MaxScrollPosition)
			{
				flag = true;
				this.terminalResetElapsedTime = 0f;
			}
			else
			{
				this.terminalResetElapsedTime += elapsedTime;
			}
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LiveListPanel.ListItemData current = enumerator.Current;
					if (current.IsSliding)
					{
						flag = true;
						current.SlideElapsedTime += elapsedTime;
						if (current.SlideElapsedTime > this.itemSlideInTime)
						{
							current.IsSliding = false;
						}
					}
				}
			}
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator2 = this.usingItemDataList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LiveListPanel.ListItemData current2 = enumerator2.Current;
					if (current2.IsScrolling)
					{
						flag = true;
						break;
					}
				}
			}
			if (this.animationState == LiveListPanel.AnimationState.Flick)
			{
				this.flickElapsedTime += elapsedTime;
				flag = true;
				this.flickDistance = this.flickStartDistance * (1f - this.flickElapsedTime / this.animationTime) * (1f - this.flickElapsedTime / this.animationTime);
				bool flag2 = false;
				using (List<LiveListPanel.ListItemData>.Enumerator enumerator3 = this.usingItemDataList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						LiveListPanel.ListItemData current3 = enumerator3.Current;
						if (current3.IsScrolling)
						{
							flag2 = true;
						}
					}
				}
				if (!flag2)
				{
					this.animationState = LiveListPanel.AnimationState.None;
				}
				else if (this.flickElapsedTime > this.animationTime)
				{
					this.ScrollOffset(0f);
				}
				else
				{
					this.ScrollOffset(this.flickDistance);
				}
			}
			if (this.animationState != LiveListPanel.AnimationState.Drag)
			{
				if (this.scrollPosition < 0f)
				{
					this.scrollPosition += elapsedTime * this.terminalDecay;
					if (this.scrollPosition > 0f)
					{
						this.scrollPosition = 0f;
					}
				}
				else if (this.scrollPosition > this.MaxScrollPosition)
				{
					this.scrollPosition -= elapsedTime * this.terminalDecay;
					if (this.scrollPosition < this.MaxScrollPosition)
					{
						this.scrollPosition = this.MaxScrollPosition;
					}
				}
			}
			if (flag)
			{
				this.UpdateListItemData(elapsedTime);
				return;
			}
			this.isScrolling = false;
			this.UpdateScrollBarVisible();
		}

		private float GetElapsedTimeMovingAverage(float elapsedTime)
		{
			this.elapsedTimes[this.elapsedTimeIndex] = elapsedTime;
			this.elapsedTimeIndex++;
			if (this.elapsedTimeIndex >= 60)
			{
				this.elapsedTimeIndex = 0;
				this.initElapsedTimes = true;
			}
			if (this.initElapsedTimes)
			{
				elapsedTime = 0f;
				for (int i = 0; i < 60; i++)
				{
					elapsedTime += this.elapsedTimes[i];
				}
				return elapsedTime / 60f;
			}
			return elapsedTime;
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			bool flag = false;
			switch (primaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.animationState == LiveListPanel.AnimationState.Drag)
				{
					this.animationState = LiveListPanel.AnimationState.None;
					this.isScrolling = false;
					flag = true;
				}
				this.UpdateScrollBarVisible();
				break;
			case TouchEventType.Down:
			{
				if (this.animationState == LiveListPanel.AnimationState.Flick)
				{
					this.animationState = LiveListPanel.AnimationState.None;
				}
				float y = primaryTouchEvent.LocalPosition.Y;
				using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LiveListPanel.ListItemData current = enumerator.Current;
						ListPanelItem item = this.GetItem(current.item.Index + 1);
						if (item != null && current.item.Y <= y && y < item.Y)
						{
							this.scrollBaseIndex = current.item.Index;
						}
						current.scrollStartRotationZ = current.slideEndRotationZ;
					}
				}
				this.UpdateScrollEndRotationZ();
				break;
			}
			}
			if (this.animationState == LiveListPanel.AnimationState.None && !this.isScrolling && !flag)
			{
				touchEvents.Forward = true;
			}
		}

		private void DragEventHandler(object sender, DragEventArgs e)
		{
			if (e.Distance.Y == 0f)
			{
				return;
			}
			this.animationState = LiveListPanel.AnimationState.Drag;
			this.isScrolling = true;
			if (e.Distance.Y > 0f)
			{
				this.scrollDirection = FourWayDirection.Down;
			}
			else
			{
				this.scrollDirection = FourWayDirection.Up;
			}
			if (this.prevScrollDirection != this.scrollDirection)
			{
				this.UpdateScrollEndRotationZ();
			}
			this.prevScrollDirection = this.scrollDirection;
			this.ScrollOffset(e.Distance.Y);
			this.UpdateListItemData();
		}

		private void FlickEventHandler(object sender, FlickEventArgs e)
		{
			if (e.Speed.Y == 0f)
			{
				return;
			}
			this.animationState = LiveListPanel.AnimationState.Flick;
			if (e.Speed.Y > 0f)
			{
				this.scrollDirection = FourWayDirection.Down;
			}
			else
			{
				this.scrollDirection = FourWayDirection.Up;
			}
			this.flickDistance = e.Speed.Y * 0.015f;
			this.flickStartDistance = this.flickDistance;
			this.flickElapsedTime = 0f;
			this.ScrollOffset(this.flickDistance);
			this.UpdateListItemData();
		}

		private void UpdateScrollEndRotationZ()
		{
			using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LiveListPanel.ListItemData current = enumerator.Current;
					float num = (float)LiveListPanel.rand.NextDouble() * this.itemTiltAngle;
					current.scrollEndRotationZ = ((current.scrollEndRotationZ > 0f) ? num : (-num));
					current.scrollStartRotationZ = current.slideEndRotationZ;
					current.totalScrollIncrement = 0f;
				}
			}
		}

		private void ScrollOffset(float offsetY)
		{
			this.scrollPosition -= offsetY;
			if (0f < this.scrollPosition && this.scrollPosition < this.MaxScrollPosition)
			{
				using (List<LiveListPanel.ListItemData>.Enumerator enumerator = this.usingItemDataList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LiveListPanel.ListItemData current = enumerator.Current;
						current.totalScrollIncrement += FMath.Abs(offsetY);
					}
				}
			}
		}

		public void SetListItemCreator(ListItemCreator creator)
		{
			this.itemCreator = creator;
			this.needRefresh = true;
		}

		public void SetListItemUpdater(ListItemUpdater updater)
		{
			this.itemUpdater = updater;
			this.needRefresh = true;
		}
	}
}
