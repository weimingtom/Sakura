using Sce.Pss.Core;
using System;
using System.Collections.Generic;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	internal class InternalSpinBox : Widget
	{
		internal class SpinItem : Widget
		{
			public delegate void ProtectedSelectActionHandler(object sender, int itemId);

			private bool enabled = true;

			private UISprite sprt;

			private bool isDown;

			public event InternalSpinBox.SpinItem.ProtectedSelectActionHandler ProtectedSelectAction;

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
				}
			}

			public bool IsFocused
			{
				get;
				set;
			}

			public bool IsSelected
			{
				get;
				set;
			}

			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
				set
				{
					this.enabled = value;
					if (this.enabled)
					{
						this.sprt.Alpha = 1f;
						return;
					}
					this.sprt.Alpha = 0.3f;
				}
			}

			public ImageAsset ImageAsset
			{
				get
				{
					if (this.sprt != null)
					{
						return this.sprt.Image;
					}
					return null;
				}
				set
				{
					if (this.sprt != null)
					{
						this.sprt.Image = value;
					}
				}
			}

			public int ItemId
			{
				get;
				set;
			}

			public ShaderType ShaderType
			{
				get
				{
					if (this.sprt != null)
					{
						return this.sprt.ShaderType;
					}
					return ShaderType.SolidFill;
				}
				set
				{
					if (this.sprt != null)
					{
						this.sprt.ShaderType = value;
					}
				}
			}

			public SpinItem()
			{
				this.sprt = new UISprite(1);
				base.RootUIElement.AddChildLast(this.sprt);
				this.sprt.ShaderType = ShaderType.SolidFill;
				this.IsFocused = false;
				this.IsSelected = false;
				this.ItemId = 0;
				this.isDown = false;
				this.PriorityHit = true;
			}

			public static InternalSpinBox.SpinItem FactoryMethod()
			{
				return new InternalSpinBox.SpinItem();
			}

			protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
			{
				base.OnTouchEvent(touchEvents);
				switch (touchEvents.PrimaryTouchEvent.Type)
				{
				case TouchEventType.Up:
					if (this.isDown)
					{
						this.FireSelect();
						this.isDown = false;
					}
					return;
				case TouchEventType.Down:
					this.isDown = true;
					return;
				default:
					return;
				}
			}

			private void FireSelect()
			{
				if (this.ProtectedSelectAction != null)
				{
					this.ProtectedSelectAction(this, this.ItemId);
				}
			}

			protected internal override void OnResetState()
			{
				base.OnResetState();
				this.isDown = false;
			}
		}

		internal class SpinList : Widget
		{
			protected enum AnimationState
			{
				None,
				Drag,
				Flick,
				Fit
			}

			public class ListContainer
			{
				public InternalSpinBox.SpinList.ListContainer next;

				public InternalSpinBox.SpinList.ListContainer prev;

				public InternalSpinBox.SpinItem listItem;

				public int totalId;

				public bool updateFlag;

				public ListContainer()
				{
					this.next = null;
					this.prev = null;
					this.listItem = null;
					this.totalId = 0;
					this.updateFlag = false;
				}
			}

			public static int itemIdNoFocus = -1;

			public static int itemIdNoSelect = -1;

			private static int noCache = -1;

			private bool isLoop;

			private Panel basePanel;

			private ushort scrollAreaLineNum;

			private int listItemNum;

			private ushort cacheLineCount;

			private int focusIndex;

			private int lastSelectIndex;

			private int cacheStartIndex;

			private int cacheEndIndex;

			private bool isSetup;

			public List<InternalSpinBox.SpinList.ListContainer> listItem;

			private List<InternalSpinBox.SpinList.ListContainer> poolItem;

			private InternalSpinBox.SpinList.AnimationState animationState;

			private float flickDistance;

			public float flickInterpRate;

			public float flickSpeedCoeff;

			private int fitTargetIndex;

			private float fitTargetOffsetPixel;

			public float fitInterpRate;

			private bool isPress;

			private Vector2 startTouchPos;

			private float minDelayDragDist;

			public event EventHandler<InternalSpinBox.SpinItemRequestEventArgs> ItemRequestAction;

			public event EventHandler<EventArgs> FocusChanged;

			public float ItemGapLine
			{
				get;
				set;
			}

			public float ItemGapStep
			{
				get
				{
					return this.Width;
				}
			}

			public int FocusIndex
			{
				get
				{
					return this.focusIndex;
				}
				set
				{
					if (this.focusIndex != value)
					{
						if (this.isSetup)
						{
							if (this.IsLoop)
							{
								this.focusIndex = this.CalcItemIndex(value);
								this.UpdateFocus();
								return;
							}
							if (value < this.listItemNum && value >= 0)
							{
								this.focusIndex = value;
								this.UpdateFocus();
								return;
							}
						}
						else
						{
							this.focusIndex = value;
						}
					}
				}
			}

			public bool IsLoop
			{
				get
				{
					return this.isLoop;
				}
				set
				{
					if (this.isLoop != value)
					{
						this.isLoop = value;
						if (this.isSetup)
						{
							this.UpdateListItem(false);
						}
					}
				}
			}

			public bool IsItemFit
			{
				get;
				set;
			}

			public int ScrollAreaFirstLine
			{
				get;
				private set;
			}

			public float ScrollAreaPixelOffset
			{
				get
				{
					return -this.basePanel.Y;
				}
			}

			public float ScrollAreaRatio
			{
				get
				{
					int num = this.CalcMaxLineIndex();
					if (num == 0)
					{
						return 0f;
					}
					int scrollAreaFirstLine = this.ScrollAreaFirstLine;
					float scrollAreaPixelOffset = this.ScrollAreaPixelOffset;
					float num2 = (float)num;
					float num3 = (float)scrollAreaFirstLine;
					return (num3 + scrollAreaPixelOffset / this.ItemGapLine) / num2;
				}
			}

			public int ScrollAreaLineNum
			{
				get
				{
					return (int)this.scrollAreaLineNum;
				}
				set
				{
					this.scrollAreaLineNum = ((value > 0) ? ((ushort)value) : (ushort)0);
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}

			public int ListItemNum
			{
				get
				{
					return this.listItemNum;
				}
				set
				{
					this.listItemNum = value;
					if (this.focusIndex >= this.listItemNum)
					{
						this.FocusIndex = this.listItemNum - 1;
					}
					if (this.ScrollAreaFirstLine > this.CalcMaxLineIndex())
					{
						this.SetScrollAreaLineIndex(this.CalcMaxLineIndex(), 0f);
					}
					if (this.isSetup)
					{
						this.UpdateListItem(false);
					}
				}
			}

			public int TotalLineNum
			{
				get
				{
					return this.listItemNum;
				}
			}

			public SpinList()
			{
				this.basePanel = new Panel();
				base.AddChildLast(this.basePanel);
				base.HookChildTouchEvent = true;
				this.IsLoop = true;
				this.scrollAreaLineNum = 1;
				this.listItemNum = 0;
				this.ItemGapLine = 0f;
				this.cacheLineCount = 1;
				this.IsItemFit = true;
				this.ScrollAreaFirstLine = 0;
				this.focusIndex = InternalSpinBox.SpinList.itemIdNoFocus;
				this.lastSelectIndex = InternalSpinBox.SpinList.itemIdNoSelect;
				this.cacheStartIndex = InternalSpinBox.SpinList.noCache;
				this.cacheEndIndex = InternalSpinBox.SpinList.noCache;
				this.isSetup = false;
				this.animationState = InternalSpinBox.SpinList.AnimationState.None;
				this.flickDistance = 0f;
				this.flickInterpRate = 0.09f;
				this.flickSpeedCoeff = 0.03f;
				this.fitTargetIndex = 0;
				this.fitTargetOffsetPixel = 0f;
				this.fitInterpRate = 0.2f;
				this.isPress = false;
				this.startTouchPos = Vector2.Zero;
				this.listItem = new List<InternalSpinBox.SpinList.ListContainer>();
				this.poolItem = new List<InternalSpinBox.SpinList.ListContainer>();
				this.minDelayDragDist = 5f;
				base.Clip = true;
				this.FocusChanged = null;
				DragGestureDetector dragGestureDetector = new DragGestureDetector();
				dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.DragEventHandler);
				base.AddGestureDetector(dragGestureDetector);
				FlickGestureDetector flickGestureDetector = new FlickGestureDetector();
				flickGestureDetector.FlickDetected += new EventHandler<FlickEventArgs>(this.FlickEventHandler);
				base.AddGestureDetector(flickGestureDetector);
			}

			public bool IsSelect(int itemId)
			{
				if (itemId != InternalSpinBox.SpinList.itemIdNoSelect)
				{
					InternalSpinBox.SpinItem spinItem = this.GetListItem(itemId);
					if (spinItem != null)
					{
						return spinItem.IsSelected;
					}
				}
				return false;
			}

			public void SetSelect(int itemId, bool isSelect)
			{
				int num = this.lastSelectIndex;
				if (isSelect)
				{
					if (num != itemId)
					{
						if (num != InternalSpinBox.SpinList.itemIdNoSelect)
						{
							this.SetSelectStatus(num, false);
						}
						if (itemId != InternalSpinBox.SpinList.itemIdNoSelect)
						{
							this.SetSelectStatus(itemId, true);
							this.lastSelectIndex = itemId;
							return;
						}
					}
				}
				else if (itemId != InternalSpinBox.SpinList.itemIdNoSelect)
				{
					this.SetSelectStatus(itemId, false);
				}
			}

			public void StartItemRequest()
			{
				if (this.ScrollAreaFirstLine > this.CalcMaxLineIndex())
				{
					this.SetScrollAreaLineIndex(this.CalcMaxLineIndex(), 0f);
				}
				else
				{
					this.SetScrollAreaLineIndex(this.ScrollAreaFirstLine, 0f);
				}
				this.listItem.Clear();
				this.poolItem.Clear();
				this.cacheStartIndex = 0;
				this.cacheEndIndex = 0;
				this.isSetup = true;
				this.UpdateListItem(false);
				this.UpdateFocus();
			}

			public InternalSpinBox.SpinItem GetListItem(int index)
			{
				if (index < 0)
				{
					return null;
				}
				for (int i = 0; i < this.listItem.Count; i++)
				{
					int itemId = this.listItem[i].listItem.ItemId;
					if (itemId == index)
					{
						return this.listItem[i].listItem;
					}
				}
				return null;
			}

			public void SetScrollAreaPos(int firstLine, float pixelOffset, bool withAnimate)
			{
				this.ScrollTo(firstLine, pixelOffset, withAnimate);
			}

			public void SetScrollAreaPos(float ratio, bool withAnimate)
			{
				this.ScrollTo(ratio, withAnimate);
			}

			private void NormalizeLineIndex(ref int lineIndex, ref float offsetPixel)
			{
				while (offsetPixel < 0f)
				{
					offsetPixel += this.ItemGapLine;
					lineIndex--;
				}
				while (offsetPixel > this.ItemGapLine)
				{
					offsetPixel -= this.ItemGapLine;
					lineIndex++;
				}
			}

			private void SetScrollAreaLineIndex(int lineIndex, float offsetPixel)
			{
				this.NormalizeLineIndex(ref lineIndex, ref offsetPixel);
				if (this.IsLoop)
				{
					while (lineIndex < 0)
					{
						lineIndex += this.TotalLineNum;
						this.cacheStartIndex += this.listItemNum;
						this.cacheEndIndex += this.listItemNum;
						this.fitTargetIndex += this.TotalLineNum;
						for (int i = 0; i < this.listItem.Count; i++)
						{
							this.listItem[i].totalId += this.listItemNum;
						}
					}
					while (lineIndex > this.TotalLineNum - 1)
					{
						lineIndex -= this.TotalLineNum;
						this.cacheStartIndex -= this.listItemNum;
						this.cacheEndIndex -= this.listItemNum;
						this.fitTargetIndex -= this.TotalLineNum;
						for (int j = 0; j < this.listItem.Count; j++)
						{
							this.listItem[j].totalId -= this.listItemNum;
						}
					}
					this.ScrollAreaFirstLine = lineIndex;
					this.SetNodeLinePos(offsetPixel);
					if (this.isSetup)
					{
						this.UpdateListItem(false);
						return;
					}
				}
				else if (lineIndex <= this.CalcMaxLineIndex() && lineIndex >= this.CalcMinLineIndex())
				{
					bool isMod = false;
					if (this.ScrollAreaFirstLine != lineIndex)
					{
						this.ScrollAreaFirstLine = lineIndex;
						isMod = true;
					}
					this.SetNodeLinePos(offsetPixel);
					if (this.isSetup)
					{
						this.UpdateListItem(isMod);
					}
				}
			}

			internal void ScrollTo(int scrollAreaLineIndex, float offsetPixel, bool withAnimate)
			{
				if (scrollAreaLineIndex < 0)
				{
					scrollAreaLineIndex += this.TotalLineNum;
				}
				this.NormalizeLineIndex(ref scrollAreaLineIndex, ref offsetPixel);
				if (this.IsItemFit)
				{
					if (offsetPixel > this.ItemGapLine * 0.5f)
					{
						scrollAreaLineIndex++;
					}
					offsetPixel = 0f;
				}
				if (!this.IsLoop)
				{
					if (scrollAreaLineIndex >= this.CalcMaxLineIndex())
					{
						scrollAreaLineIndex = this.CalcMaxLineIndex();
						offsetPixel = 0f;
					}
					if (scrollAreaLineIndex < this.CalcMinLineIndex())
					{
						scrollAreaLineIndex = this.CalcMinLineIndex();
						offsetPixel = 0f;
					}
				}
				else if (Math.Abs(scrollAreaLineIndex - scrollAreaLineIndex) > (this.TotalLineNum - 1) / 2)
				{
					if (scrollAreaLineIndex - scrollAreaLineIndex > 0)
					{
						scrollAreaLineIndex -= this.TotalLineNum;
					}
					else
					{
						scrollAreaLineIndex += this.TotalLineNum;
					}
				}
				if (this.isSetup && withAnimate)
				{
					this.fitTargetIndex = scrollAreaLineIndex;
					this.fitTargetOffsetPixel = offsetPixel;
					this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
					this.fitInterpRate = 0.2f;
					return;
				}
				this.SetScrollAreaLineIndex(scrollAreaLineIndex, offsetPixel);
			}

			private void ScrollTo(float ratio, bool withAnimate)
			{
				int num = this.CalcMaxLineIndex();
				if (ratio == 1f)
				{
					this.ScrollTo(num, 0f, withAnimate);
					return;
				}
				float num2 = (float)num;
				float num3 = num2 * ratio;
				int num4 = (int)num3;
				float offsetPixel = (num3 - (float)num4) * this.ItemGapLine;
				this.ScrollTo(num4, offsetPixel, withAnimate);
			}

			private void OnSelectHandler(object sender, int index)
			{
				this.ToggleSelectItemID(index);
				this.FocusIndex = index;
			}

			private void ToggleSelectItemID(int itemId)
			{
				if (itemId != InternalSpinBox.SpinList.itemIdNoSelect)
				{
					InternalSpinBox.SpinItem spinItem = this.GetListItem(itemId);
					if (spinItem != null)
					{
						this.SetSelect(itemId, !spinItem.IsSelected);
						this.ScrollTo(itemId - InternalSpinBox.visibleCount / 2, 0f, true);
					}
				}
			}

			private void SetSelectStatus(int index, bool isSelect)
			{
				InternalSpinBox.SpinItem spinItem = this.GetListItem(index);
				if (spinItem != null)
				{
					spinItem.IsSelected = isSelect;
				}
			}

			private void UpdateFocus()
			{
				if (!this.isSetup)
				{
					return;
				}
				if (this.focusIndex < 0)
				{
					return;
				}
				for (int i = 0; i < this.listItem.Count; i++)
				{
					int itemId = this.listItem[i].listItem.ItemId;
					if (this.focusIndex == itemId)
					{
						if (!this.listItem[i].listItem.IsFocused)
						{
							this.listItem[i].listItem.IsFocused = true;
							this.listItem[i].updateFlag = true;
						}
					}
					else if (this.listItem[i].listItem.IsFocused)
					{
						this.listItem[i].listItem.IsFocused = false;
						this.listItem[i].updateFlag = true;
					}
				}
			}

			private void UpdateListItem(bool isMod)
			{
				int num = this.ScrollAreaFirstLine - (int)this.cacheLineCount;
				int num2 = this.ScrollAreaFirstLine + (int)this.scrollAreaLineNum + (int)this.cacheLineCount + 1;
				if (!this.IsLoop)
				{
					if (num < 0)
					{
						num = 0;
					}
					if (num2 > this.listItemNum)
					{
						num2 = this.listItemNum;
					}
				}
				bool flag = isMod;
				InternalSpinBox.SpinList.ListContainer listContainer = new InternalSpinBox.SpinList.ListContainer();
				if (this.cacheEndIndex - 1 >= num)
				{
					if (num2 - 1 >= this.cacheStartIndex)
					{
						if (this.cacheStartIndex == num && this.cacheEndIndex == num2)
						{
							goto IL_1F5;
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
							goto IL_1F5;
						}
						goto IL_1F5;
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
				IL_1F5:
				this.cacheStartIndex = num;
				this.cacheEndIndex = num2;
				if (flag)
				{
					for (int n = 0; n < this.listItem.Count; n++)
					{
						int totalId = this.listItem[n].totalId;
						this.listItem[n].listItem.X = this.CalcItemPosOnScrollNodeX(totalId);
						this.listItem[n].listItem.Y = this.CalcItemPosOnScrollNodeY(totalId);
						if (this.listItem[n].updateFlag && this.ItemRequestAction != null)
						{
							InternalSpinBox.SpinItemRequestEventArgs spinItemRequestEventArgs = new InternalSpinBox.SpinItemRequestEventArgs();
							spinItemRequestEventArgs.Index = this.listItem[n].totalId;
							this.ItemRequestAction.Invoke(this, spinItemRequestEventArgs);
							this.listItem[n].updateFlag = false;
						}
					}
				}
				for (int num3 = 0; num3 < this.listItem.Count; num3++)
				{
					float num4 = this.listItem[num3].listItem.Y + this.listItem[num3].listItem.Height + this.basePanel.Y;
					int num5 = this.ScrollAreaLineNum / 2 + 1;
					if (num4 <= this.listItem[num3].listItem.Height * (float)num5)
					{
						this.listItem[num3].listItem.Alpha = num4 / (this.listItem[num3].listItem.Height * (float)num5);
					}
					else
					{
						this.listItem[num3].listItem.Alpha = 2f - num4 / (this.listItem[num3].listItem.Height * (float)num5);
					}
				}
			}

			private InternalSpinBox.SpinList.ListContainer GetContainer()
			{
				InternalSpinBox.SpinList.ListContainer listContainer = new InternalSpinBox.SpinList.ListContainer();
				if (this.poolItem.Count != 0)
				{
					listContainer = this.poolItem[0];
					this.poolItem.RemoveAt(0);
					listContainer.listItem.Visible = true;
					listContainer.listItem.TouchResponse = true;
				}
				else
				{
					listContainer = new InternalSpinBox.SpinList.ListContainer();
					listContainer.listItem = null;
				}
				return listContainer;
			}

			private void PoolContainer(ref InternalSpinBox.SpinList.ListContainer container)
			{
				if (container != null)
				{
					this.poolItem.Add(container);
					container.listItem.Visible = false;
				}
			}

			private void CreateListItem(ref InternalSpinBox.SpinList.ListContainer listContainer, int index)
			{
				InternalSpinBox.SpinItem spinItem;
				if (listContainer.listItem == null)
				{
					spinItem = new InternalSpinBox.SpinItem();
				}
				else
				{
					spinItem = listContainer.listItem;
				}
				listContainer.listItem = spinItem;
				listContainer.totalId = index;
				listContainer.updateFlag = true;
				spinItem.ItemId = this.CalcItemIndex(index);
				this.basePanel.AddChildLast(spinItem);
				spinItem.ProtectedSelectAction += new InternalSpinBox.SpinItem.ProtectedSelectActionHandler(this.OnSelectHandler);
				spinItem.IsFocused = (this.focusIndex == this.CalcItemIndex(index));
			}

			private int CalcMinLineIndex()
			{
				return 0;
			}

			private int CalcMaxLineIndex()
			{
				return Math.Max(this.TotalLineNum - (int)this.scrollAreaLineNum, 0);
			}

			private int CalcStepIndex(int index)
			{
				return 0;
			}

			private int CalcLineIndex(int index)
			{
				return index;
			}

			private int CalcItemIndex(int totalId)
			{
				if (totalId < 0)
				{
					int num = (Math.Abs(totalId) - 1) / this.listItemNum;
					num = -num - 1;
					totalId += -num * this.listItemNum;
					totalId %= this.listItemNum;
				}
				else
				{
					totalId %= this.listItemNum;
				}
				return totalId;
			}

			private float CalcItemPosOnScrollNodeX(int totalId)
			{
				float virtualPosX = (float)this.CalcStepIndex(totalId) * this.ItemGapStep;
				float virtualPosY = (float)(this.CalcLineIndex(totalId) - this.ScrollAreaFirstLine) * this.ItemGapLine;
				return this.GetRealPosX(virtualPosX, virtualPosY);
			}

			private float CalcItemPosOnScrollNodeY(int totalId)
			{
				float virtualPosX = (float)this.CalcStepIndex(totalId) * this.ItemGapStep;
				float virtualPosY = (float)(this.CalcLineIndex(totalId) - this.ScrollAreaFirstLine) * this.ItemGapLine;
				return this.GetRealPosY(virtualPosX, virtualPosY);
			}

			private void SetNodeLinePos(float pos)
			{
				this.basePanel.Y = -pos;
			}

			private float GetRealPosX(float virtualPosX, float virtualPosY)
			{
				return virtualPosX;
			}

			private float GetRealPosY(float virtualPosX, float virtualPosY)
			{
				return virtualPosY;
			}

			private float GetRealVecX(float virtualPosX, float virtualPosY)
			{
				return virtualPosX;
			}

			private float GetRealVecY(float virtualPosX, float virtualPosY)
			{
				return virtualPosY;
			}

			private float GetVirtualVec(Vector2 realVec)
			{
				return -realVec.Y;
			}

			protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
			{
				base.OnTouchEvent(touchEvents);
				TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
				switch (primaryTouchEvent.Type)
				{
				case TouchEventType.Up:
					this.isPress = false;
					if (this.animationState != InternalSpinBox.SpinList.AnimationState.Flick && this.IsItemFit)
					{
						if (this.ScrollAreaPixelOffset > this.ItemGapLine * 0.5f)
						{
							this.fitTargetIndex = this.ScrollAreaFirstLine + 1;
						}
						else
						{
							this.fitTargetIndex = this.ScrollAreaFirstLine;
						}
						this.fitTargetOffsetPixel = 0f;
						this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
						this.fitInterpRate = 0.2f;
					}
					break;
				case TouchEventType.Down:
					this.animationState = InternalSpinBox.SpinList.AnimationState.None;
					this.startTouchPos = primaryTouchEvent.LocalPosition;
					this.isPress = true;
					break;
				case TouchEventType.Move:
					if (this.isPress)
					{
						Vector2 realVec = this.startTouchPos - primaryTouchEvent.LocalPosition;
						if (Math.Abs(this.GetVirtualVec(realVec)) > this.minDelayDragDist)
						{
							this.animationState = InternalSpinBox.SpinList.AnimationState.Drag;
						}
					}
					break;
				}
				if (this.animationState != InternalSpinBox.SpinList.AnimationState.Drag && this.animationState != InternalSpinBox.SpinList.AnimationState.Flick)
				{
					touchEvents.Forward = true;
				}
			}

			private void DragEventHandler(object sender, DragEventArgs e)
			{
				if (this.animationState == InternalSpinBox.SpinList.AnimationState.Drag)
				{
					float num = this.GetVirtualVec(e.Distance) + this.ScrollAreaPixelOffset;
					int scrollAreaFirstLine = this.ScrollAreaFirstLine;
					this.NormalizeLineIndex(ref scrollAreaFirstLine, ref num);
					if (scrollAreaFirstLine < this.CalcMinLineIndex() && !this.IsLoop)
					{
						this.SetScrollAreaLineIndex(this.CalcMinLineIndex(), 0f);
						return;
					}
					if (num > 0f && scrollAreaFirstLine >= this.CalcMaxLineIndex() && !this.IsLoop)
					{
						this.SetScrollAreaLineIndex(this.CalcMaxLineIndex(), 0f);
						return;
					}
					this.SetScrollAreaLineIndex(scrollAreaFirstLine, num);
				}
			}

			private void FlickEventHandler(object sender, FlickEventArgs e)
			{
				this.animationState = InternalSpinBox.SpinList.AnimationState.Flick;
				float virtualVec = this.GetVirtualVec(e.Speed);
				this.flickDistance = virtualVec * this.flickSpeedCoeff;
				if (this.IsItemFit)
				{
					if (this.flickDistance > 0f)
					{
						if ((double)this.ItemGapLine * 2.0 - (double)this.ScrollAreaPixelOffset - (double)(this.flickDistance / this.flickInterpRate) > 0.0)
						{
							this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
							this.fitInterpRate = this.flickDistance / (this.ItemGapLine - this.ScrollAreaPixelOffset);
							if (this.ScrollAreaFirstLine >= this.CalcMaxLineIndex() && !this.IsLoop)
							{
								this.fitTargetIndex = this.ScrollAreaFirstLine;
							}
							else
							{
								this.fitTargetIndex = this.ScrollAreaFirstLine + 1;
							}
							this.fitTargetOffsetPixel = 0f;
							return;
						}
					}
					else if ((double)(this.ItemGapLine + this.ScrollAreaPixelOffset + this.flickDistance / this.flickInterpRate) > 0.0)
					{
						this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
						this.fitInterpRate = -this.flickDistance / this.ScrollAreaPixelOffset;
						this.fitTargetIndex = this.ScrollAreaFirstLine;
						this.fitTargetOffsetPixel = 0f;
					}
				}
			}

			protected internal override void OnResetState()
			{
				base.OnResetState();
				this.animationState = InternalSpinBox.SpinList.AnimationState.None;
				this.isPress = false;
			}

			protected override void OnUpdate(float elapsedTime)
			{
				base.OnUpdate(elapsedTime);
				if (this.animationState == InternalSpinBox.SpinList.AnimationState.Flick)
				{
					if (0.5f > this.flickDistance && -0.5f < this.flickDistance && !this.IsItemFit)
					{
						this.flickDistance = 0f;
						this.animationState = InternalSpinBox.SpinList.AnimationState.None;
					}
					float num = this.flickDistance + this.ScrollAreaPixelOffset;
					int scrollAreaFirstLine = this.ScrollAreaFirstLine;
					this.NormalizeLineIndex(ref scrollAreaFirstLine, ref num);
					if (scrollAreaFirstLine < this.CalcMinLineIndex() && !this.IsLoop)
					{
						this.SetScrollAreaLineIndex(this.CalcMinLineIndex(), 0f);
						this.animationState = InternalSpinBox.SpinList.AnimationState.None;
					}
					else if (num > 0f && scrollAreaFirstLine >= this.CalcMaxLineIndex() && !this.IsLoop)
					{
						this.SetScrollAreaLineIndex(this.CalcMaxLineIndex(), 0f);
						this.animationState = InternalSpinBox.SpinList.AnimationState.None;
					}
					else
					{
						this.SetScrollAreaLineIndex(scrollAreaFirstLine, num);
					}
					this.flickDistance = MathUtility.Lerp(this.flickDistance, 0f, this.flickInterpRate);
					if (this.IsItemFit)
					{
						if (this.flickDistance > 0f)
						{
							if (this.ItemGapLine - this.ScrollAreaPixelOffset - this.flickDistance < 0f)
							{
								if (!this.IsLoop && this.ScrollAreaFirstLine + 2 > this.CalcMaxLineIndex())
								{
									float num2 = this.ItemGapLine * (float)(this.CalcMaxLineIndex() - this.ScrollAreaFirstLine) - this.ScrollAreaPixelOffset;
									this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
									if (num2 < 0.5f)
									{
										this.fitInterpRate = 0.2f;
									}
									else
									{
										this.fitInterpRate = this.flickDistance / num2;
									}
									this.fitTargetIndex = this.CalcMaxLineIndex();
									this.fitTargetOffsetPixel = 0f;
									return;
								}
								if (this.ItemGapLine * 3f - this.ScrollAreaPixelOffset - this.flickDistance / this.flickInterpRate > 0f)
								{
									this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
									this.fitInterpRate = this.flickDistance / (this.ItemGapLine * 2f - this.ScrollAreaPixelOffset);
									this.fitTargetIndex = this.ScrollAreaFirstLine + 2;
									this.fitTargetOffsetPixel = 0f;
									return;
								}
							}
						}
						else if (this.ScrollAreaPixelOffset + this.flickDistance < 0f)
						{
							if (!this.IsLoop && this.ScrollAreaFirstLine - 1 < this.CalcMinLineIndex())
							{
								float num3 = -(this.ItemGapLine * (float)(this.ScrollAreaFirstLine - this.CalcMinLineIndex()) + this.ScrollAreaPixelOffset);
								this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
								if (num3 < 0.5f)
								{
									this.fitInterpRate = 0.8f;
								}
								else
								{
									this.fitInterpRate = this.flickDistance / num3;
								}
								this.fitTargetIndex = this.CalcMinLineIndex();
								this.fitTargetOffsetPixel = 0f;
								return;
							}
							if (this.ItemGapLine * 2f + this.ScrollAreaPixelOffset + this.flickDistance / this.flickInterpRate > 0f)
							{
								this.animationState = InternalSpinBox.SpinList.AnimationState.Fit;
								this.fitInterpRate = -this.flickDistance / (this.ItemGapLine * 1f + this.ScrollAreaPixelOffset);
								this.fitTargetIndex = this.ScrollAreaFirstLine - 1;
								this.fitTargetOffsetPixel = 0f;
								return;
							}
						}
					}
				}
				else if (this.animationState == InternalSpinBox.SpinList.AnimationState.Fit)
				{
					int num4 = this.ScrollAreaFirstLine - this.fitTargetIndex;
					if (this.isLoop)
					{
						if (num4 > this.TotalLineNum / 2)
						{
							num4 -= this.TotalLineNum;
						}
						else if (num4 < -this.TotalLineNum / 2)
						{
							num4 += this.TotalLineNum;
						}
					}
					float num5 = MathUtility.Lerp((float)num4, 0f, this.fitInterpRate);
					num4 = (int)num5;
					float num6 = (num5 - (float)num4) * this.ItemGapLine;
					num6 += MathUtility.Lerp(this.ScrollAreaPixelOffset - this.fitTargetOffsetPixel, 0f, this.fitInterpRate);
					this.SetScrollAreaLineIndex(num4 + this.fitTargetIndex, num6 + this.fitTargetOffsetPixel);
					if (num4 == 0 && Math.Abs(num6) <= 0.5f)
					{
						this.animationState = InternalSpinBox.SpinList.AnimationState.None;
						this.SetScrollAreaLineIndex(this.fitTargetIndex, this.fitTargetOffsetPixel);
						if (this.FocusChanged != null)
						{
							this.FocusChanged.Invoke(this, EventArgs.Empty);
						}
					}
				}
			}
		}

		internal class SpinItemRequestEventArgs : EventArgs
		{
			public int Index
			{
				get;
				set;
			}
		}

		internal const float defaultWidth = 80f;

		internal const float defaultHeight = 204f;

		internal const float centerHeight = 54f;

		internal const float bgWidthGap = 30f;

		internal const float listVerticalMargin = 2f;

		internal const float unitWidth = 80f;

		internal const float unitHeight = 40f;

		internal const float unitWidthPower = 1.6f;

		internal InternalSpinBox.SpinList spinList;

		internal static readonly int visibleCount = 5;

		private ImageAsset backgraundImage;

		private ImageAsset centerImage;

		private NinePatchMargin backgroundImageNinePatch;

		private NinePatchMargin centerImageNinePatch;

		private UIPrimitive backPrim;

		private UIPrimitive centerPrim;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.backPrim != null && this.centerPrim != null && this.spinList != null)
				{
					UIPrimitiveUtility.SetupNinePatch(this.backPrim, base.Width, base.Height, 0f, 0f, this.backgroundImageNinePatch);
					UIPrimitiveUtility.SetupNinePatch(this.centerPrim, base.Width, 54f, 0f, 0f, this.centerImageNinePatch);
					this.spinList.Width = base.Width;
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
				if (this.backPrim != null && this.centerPrim != null && this.spinList != null)
				{
					base.Height = value;
					UIPrimitiveUtility.SetupNinePatch(this.backPrim, base.Width, base.Height, 0f, 0f, this.backgroundImageNinePatch);
					UIPrimitiveUtility.SetupNinePatch(this.centerPrim, base.Width, 54f, 0f, 0f, this.centerImageNinePatch);
					this.centerPrim.SetPosition(0f, (base.Height - 54f) / 2f);
				}
			}
		}

		public InternalSpinBox()
		{
			base.Width = 80f;
			base.Height = 204f;
			this.backgraundImage = new ImageAsset(SystemImageAsset.SpinBoxBase);
			this.centerImage = new ImageAsset(SystemImageAsset.SpinBoxCenter);
			this.backgroundImageNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.SpinBoxBase);
			this.centerImageNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.SpinBoxCenter);
			this.backPrim = new UIPrimitive((DrawMode)4, 16, 28);
			this.backPrim.Image = this.backgraundImage;
			this.backPrim.ShaderType = ShaderType.Texture;
			UIPrimitiveUtility.SetupNinePatch(this.backPrim, 80f, 204f, 0f, 0f, this.backgroundImageNinePatch);
			this.backPrim.SetPosition(0f, 0f);
			base.RootUIElement.AddChildLast(this.backPrim);
			this.centerPrim = new UIPrimitive((DrawMode)4, 16, 28);
			this.centerPrim.Image = this.centerImage;
			this.centerPrim.ShaderType = ShaderType.Texture;
			UIPrimitiveUtility.SetupNinePatch(this.centerPrim, 80f, 54f, 0f, 0f, this.centerImageNinePatch);
			this.centerPrim.SetPosition(0f, 75f);
			base.RootUIElement.AddChildLast(this.centerPrim);
			this.spinList = new InternalSpinBox.SpinList();
			this.spinList.Width = 80f;
			this.spinList.Height = 40f * (float)InternalSpinBox.visibleCount;
			this.spinList.X = 0f;
			this.spinList.Y = 2f;
			this.spinList.ItemGapLine = 40f;
			this.spinList.ScrollAreaLineNum = InternalSpinBox.visibleCount;
			base.AddChildLast(this.spinList);
			this.PriorityHit = true;
		}

		protected override void DisposeSelf()
		{
			if (this.backgraundImage != null)
			{
				this.backgraundImage.Dispose();
			}
			if (this.centerImage != null)
			{
				this.centerImage.Dispose();
			}
			base.DisposeSelf();
		}
	}
}
