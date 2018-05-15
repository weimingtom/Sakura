using Sce.Pss.Core.Imaging;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class PopupList : Widget
	{
		private enum ButtonState
		{
			Normal,
			Pressed,
			Disabled
		}

		private class PopupListPanelItem : ListPanelItem
		{
			private bool isSelected;

			private Label label;

			public UISprite selectedSprite;

			public NinePatchMargin selectedImageNinePatch;

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
					this.updateSprite();
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
					this.updateSprite();
				}
			}

			public string Text
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

			public bool IsSelected
			{
				get
				{
					return this.isSelected;
				}
				set
				{
					this.isSelected = value;
					this.selectedSprite.Visible = this.IsSelected;
				}
			}

			public PopupListPanelItem()
			{
				this.selectedSprite = new UISprite(3);
				this.selectedSprite.Visible = false;
				base.RootUIElement.AddChildLast(this.selectedSprite);
				this.label = new Label();
				this.label.X = 40f;
				this.label.HorizontalAlignment = HorizontalAlignment.Left;
				this.AddChildLast(this.label);
			}

			private void updateSprite()
			{
				if (this.selectedSprite != null)
				{
					UISpriteUtility.SetupHorizontalThreePatch(this.selectedSprite, this.Width, this.Height, (float)this.selectedImageNinePatch.Left, (float)this.selectedImageNinePatch.Right);
				}
			}
		}

		private class CancellableDialog : Dialog
		{
			private Panel panel;

			private PopupList popupList;

			public override float X
			{
				get
				{
					return base.X;
				}
				set
				{
					base.X = value;
					if (this.panel != null)
					{
						this.panel.X = -value;
					}
				}
			}

			public override float Y
			{
				get
				{
					return base.Y;
				}
				set
				{
					base.Y = value;
					if (this.panel != null)
					{
						this.panel.Y = -value;
					}
				}
			}

			public CancellableDialog(PopupList list)
			{
				this.popupList = list;
				this.panel = new Panel();
				this.panel.Width = (float)UISystem.FramebufferWidth;
				this.panel.Height = (float)UISystem.FramebufferHeight;
				this.panel.X = -this.X;
				this.panel.Y = -this.Y;
				this.panel.TouchEventReceived += new EventHandler<TouchEventArgs>(this.CancellableDialogTouchEventReceived);
				this.AddChildLast(this.panel);
			}

			protected override void OnUpdate(float elapsedTime)
			{
				base.OnUpdate(elapsedTime);
				this.popupList.TimerUpdate(elapsedTime);
			}

			private void CancellableDialogTouchEventReceived(object sender, TouchEventArgs e)
			{
				this.popupList.DialogTouchEventReceived(sender, e);
			}
		}

		private const float defaultWidth = 360f;

		private const float defaultHeight = 56f;

		private const float selectedLabelMargin = 10f;

		private const float dialogMargin = 30f;

		private const float listItemLabelMargin = 40f;

		private const float listItemHeight = 50f;

		private ImageAsset[] backgroundImages;

		private NinePatchMargin backgroundNinePatch;

		private ImageAsset itemSelectedImageAsset;

		private UIColor itemSelectedImageColor;

		private NinePatchMargin itemSelectedImageNinePatch;

		private bool timerUpdate;

		private float touchTotalTime;

		private readonly float selectItemDelay = 100f;

		private int touchItemIndex = -1;

		private bool canSelectItem;

		private bool isHidingDialog;

		private float dialogMinWidth;

		private float dialogMaxWidth;

		private float dialogMinHeight;

		private float dialogMaxHeight;

		private int selectedIndex;

		private bool enabled = true;

		private PopupListItemCollection listItems;

		private PopupList.ButtonState state;

		private int previousSelectedIndex;

		private int listSelectedIndex;

		private ImageBox backGroundImage;

		private Label selectedLabel;

		private PopupList.CancellableDialog dialog;

		public event EventHandler<PopupSelectionChangedEventArgs> SelectionChanged;

		public event EventHandler<PopupListItemsChangedEventArgs> ListItemsChanged;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.backGroundImage != null)
				{
					this.backGroundImage.Width = value;
				}
				if (this.selectedLabel != null)
				{
					float num = value - this.selectedLabel.X - (float)this.backgroundNinePatch.Right;
					this.selectedLabel.Width = ((num > 0f) ? num : 0f);
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
				if (this.backGroundImage != null)
				{
					this.backGroundImage.Height = value;
				}
				if (this.selectedLabel != null)
				{
					this.selectedLabel.Height = value;
				}
			}
		}

		public string ListTitle
		{
			get;
			set;
		}

		public Font Font
		{
			get
			{
				return this.selectedLabel.Font;
			}
			set
			{
				this.selectedLabel.Font = value;
			}
		}

		public UIColor TextColor
		{
			get
			{
				return this.selectedLabel.TextColor;
			}
			set
			{
				this.selectedLabel.TextColor = value;
			}
		}

		public TextShadowSettings TextShadow
		{
			get
			{
				return this.selectedLabel.TextShadow;
			}
			set
			{
				this.selectedLabel.TextShadow = value;
			}
		}

		public TextTrimming TextTrimming
		{
			get
			{
				return this.selectedLabel.TextTrimming;
			}
			set
			{
				this.selectedLabel.TextTrimming = value;
			}
		}

		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				if (this.selectedIndex >= 0 && value < this.listItems.Count)
				{
					this.selectedIndex = value;
					this.selectedLabel.Text = this.listItems[this.selectedIndex];
				}
			}
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
					this.updateState(PopupList.ButtonState.Normal);
					return;
				}
				this.updateState(PopupList.ButtonState.Disabled);
			}
		}

		public PopupListItemCollection ListItems
		{
			get
			{
				return this.listItems;
			}
			set
			{
				this.listItems.ItemChanged -= new EventHandler(this.HandleListItemsItemChanged);
				if (value == null)
				{
					this.listItems.Clear();
				}
				else
				{
					this.listItems = value;
				}
				this.listItems.ItemChanged += new EventHandler(this.HandleListItemsItemChanged);
				this.HandleListItemsItemChanged(this, EventArgs.Empty);
			}
		}

		public PopupList()
		{
			this.backgroundImages = new ImageAsset[3];
			this.backgroundImages[0] = new ImageAsset(SystemImageAsset.PopupListBackgroundNormal);
			this.backgroundImages[1] = new ImageAsset(SystemImageAsset.PopupListBackgroundPressed);
			this.backgroundImages[2] = new ImageAsset(SystemImageAsset.PopupListBackgroundDisabled);
			this.backgroundNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.PopupListBackgroundNormal);
			this.itemSelectedImageAsset = new ImageAsset(SystemImageAsset.PopupListItemFocus);
			this.itemSelectedImageColor = new UIColor(0.9f, 0.9f, 0.9f, 1f);
			this.itemSelectedImageNinePatch = AssetManager.GetNinePatchMargin(SystemImageAsset.PopupListItemFocus);
			this.backGroundImage = new ImageBox();
			this.backGroundImage.Image = this.backgroundImages[0];
			this.backGroundImage.NinePatchMargin = this.backgroundNinePatch;
			this.backGroundImage.ImageScaleType = ImageScaleType.NinePatch;
			this.backGroundImage.TouchResponse = false;
			base.AddChildLast(this.backGroundImage);
			this.selectedLabel = new Label();
			this.selectedLabel.X = 10f;
			this.selectedLabel.HorizontalAlignment = HorizontalAlignment.Left;
			this.selectedLabel.TextTrimming = TextTrimming.EllipsisCharacter;
			this.selectedLabel.LineBreak = LineBreak.AtCode;
			this.selectedLabel.Text = "";
			this.selectedLabel.TouchResponse = false;
			base.AddChildLast(this.selectedLabel);
			this.ListTitle = "";
			this.listItems = new PopupListItemCollection();
			this.listItems.ItemChanged += new EventHandler(this.HandleListItemsItemChanged);
			this.PriorityHit = true;
			this.dialogMinWidth = (float)UISystem.FramebufferWidth * 0.5f;
			this.dialogMaxWidth = (float)UISystem.FramebufferWidth * 0.9f;
			this.dialogMinHeight = 110f;
			this.dialogMaxHeight = (float)UISystem.FramebufferHeight * 0.9f;
			this.Width = 360f;
			this.Height = 56f;
		}

		protected override void DisposeSelf()
		{
			ImageAsset[] array = this.backgroundImages;
			for (int i = 0; i < array.Length; i++)
			{
				ImageAsset imageAsset = array[i];
				imageAsset.Dispose();
			}
			if (this.itemSelectedImageAsset != null)
			{
				this.itemSelectedImageAsset.Dispose();
			}
			base.DisposeSelf();
		}

		protected internal override void OnTouchEvent(TouchEventCollection touchEvents)
		{
			base.OnTouchEvent(touchEvents);
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (this.state != PopupList.ButtonState.Disabled)
			{
				switch (primaryTouchEvent.Type)
				{
				case TouchEventType.Up:
					if (this.state == PopupList.ButtonState.Pressed)
					{
						this.ShowDialog();
						return;
					}
					this.updateState(PopupList.ButtonState.Normal);
					return;
				case TouchEventType.Down:
					this.updateState(PopupList.ButtonState.Pressed);
					return;
				case TouchEventType.Move:
					break;
				case TouchEventType.Enter:
					this.updateState(PopupList.ButtonState.Pressed);
					return;
				case TouchEventType.Leave:
					this.updateState(PopupList.ButtonState.Normal);
					break;
				default:
					return;
				}
			}
		}

		protected internal override void OnResetState()
		{
			base.OnResetState();
			if (this.state != PopupList.ButtonState.Disabled)
			{
				this.updateState(PopupList.ButtonState.Normal);
			}
		}

		private void TimerUpdate(float elapsedTime)
		{
			if (!this.timerUpdate)
			{
				return;
			}
			this.touchTotalTime += elapsedTime;
			if (this.touchTotalTime < this.selectItemDelay)
			{
				return;
			}
			this.ApplyTouchItemIndex();
		}

		private void ApplyTouchItemIndex()
		{
			if (this.touchItemIndex >= 0)
			{
				if (this.listSelectedIndex >= 0)
				{
					this.previousSelectedIndex = this.listSelectedIndex;
				}
				this.listSelectedIndex = this.touchItemIndex;
				this.updateListItems();
			}
			this.touchTotalTime = 0f;
			this.timerUpdate = false;
			this.touchItemIndex = -1;
		}

		private void CancelTouchItemIndex()
		{
			this.timerUpdate = false;
			this.touchItemIndex = -1;
		}

		private void UpdateBackgroundSprite()
		{
		}

		private void updateState(PopupList.ButtonState value)
		{
			if (this.state != value)
			{
				this.state = value;
				this.backGroundImage.Image = this.backgroundImages[(int)this.state];
			}
		}

		private void ShowDialog()
		{
			if (this.dialog == null)
			{
				this.previousSelectedIndex = this.SelectedIndex;
				this.listSelectedIndex = this.SelectedIndex;
				this.dialog = new PopupList.CancellableDialog(this);
				this.dialog.TouchEventReceived += new EventHandler<TouchEventArgs>(this.DialogTouchEventReceived);
				Label label = new Label();
				label.X = 10f;
				label.Y = 0f;
				label.Text = this.ListTitle;
				label.HorizontalAlignment = HorizontalAlignment.Center;
				this.dialog.AddChildLast(label);
				int num = label.Font.GetTextWidth(this.ListTitle);
				foreach (string current in this.ListItems)
				{
					int textWidth = this.selectedLabel.Font.GetTextWidth(current);
					if (num < textWidth)
					{
						num = textWidth;
					}
				}
				this.dialog.Width = (float)num * 1.5f;
				if (this.dialog.Width < this.dialogMinWidth)
				{
					this.dialog.Width = this.dialogMinWidth;
				}
				if (this.dialogMaxWidth < this.dialog.Width)
				{
					this.dialog.Width = this.dialogMaxWidth;
				}
				this.dialog.Height = 50f * (float)(this.listItems.Count + 1) + 10f;
				if (this.dialog.Height < this.dialogMinHeight)
				{
					this.dialog.Height = this.dialogMinHeight;
				}
				if (this.dialogMaxHeight < this.dialog.Height)
				{
					this.dialog.Height = this.dialogMaxHeight;
				}
				label.Width = this.dialog.Width - 20f;
				label.Height = 50f;
				ListSectionCollection sections = new ListSectionCollection
				{
					new ListSection("", this.ListItems.Count)
				};
				ListPanel listPanel = new ListPanel();
				listPanel.X = label.X;
				listPanel.Y = label.Y + label.Height;
				listPanel.Width = label.Width;
				listPanel.Height = this.dialog.Height - (label.Y + label.Height) - 10f;
				listPanel.SetListItemCreator(new ListItemCreator(this.ListItemCreator));
				listPanel.SetListItemUpdater(new ListItemUpdater(this.ListItemUpdater));
				listPanel.ShowSection = false;
				listPanel.Sections = sections;
				DragGestureDetector dragGestureDetector = new DragGestureDetector();
				dragGestureDetector.DragDetected += new EventHandler<DragEventArgs>(this.dragEventHandler);
				listPanel.AddGestureDetector(dragGestureDetector);
				listPanel.PriorityHit = this.PriorityHit;
				this.dialog.AddChildLast(listPanel);
				int num2 = (int)(listPanel.Height / 50f);
				float num3 = (float)(this.listSelectedIndex - num2 / 2);
				float num4 = -(num3 * 50f);
				if (num2 % 2 == 0)
				{
					num4 -= 25f;
				}
				listPanel.Move(num4);
				FadeInEffect fadeInEffect = new FadeInEffect();
				fadeInEffect.Time = 300f;
				this.dialog.ShowEffect = fadeInEffect;
				this.dialog.Show();
			}
		}

		private void HideDialog()
		{
			if (this.dialog != null && !this.isHidingDialog)
			{
				this.isHidingDialog = true;
				FadeOutEffect fadeOutEffect = new FadeOutEffect();
				fadeOutEffect.Time = 300f;
				this.dialog.HideEffect = fadeOutEffect;
				this.dialog.HideEffect.EffectStopped += new EventHandler<EventArgs>(this.FadeOutEffectStopped);
				this.dialog.Hide();
				this.updateState(PopupList.ButtonState.Normal);
				this.CancelTouchItemIndex();
				if (this.listSelectedIndex >= 0)
				{
					this.SelectedIndex = this.listSelectedIndex;
				}
			}
		}

		private void FadeOutEffectStopped(object sender, EventArgs e)
		{
			this.dialog.Dispose();
			this.dialog = null;
			this.isHidingDialog = false;
		}

		private void DialogTouchEventReceived(object sender, TouchEventArgs eventArgs)
		{
			TouchEvent primaryTouchEvent = eventArgs.TouchEvents.PrimaryTouchEvent;
			if (primaryTouchEvent.Type == TouchEventType.Up)
			{
				if (sender is Panel)
				{
					this.listSelectedIndex = -1;
				}
				this.HideDialog();
			}
		}

		private ListPanelItem ListItemCreator()
		{
			PopupList.PopupListPanelItem popupListPanelItem = new PopupList.PopupListPanelItem();
			popupListPanelItem.selectedSprite.Image = this.itemSelectedImageAsset;
			popupListPanelItem.selectedSprite.ShaderType = ShaderType.Texture;
			for (int i = 0; i < popupListPanelItem.selectedSprite.UnitCount; i++)
			{
				popupListPanelItem.selectedSprite.GetUnit(i).Color = this.itemSelectedImageColor;
			}
			popupListPanelItem.selectedImageNinePatch = this.itemSelectedImageNinePatch;
			popupListPanelItem.HookChildTouchEvent = true;
			popupListPanelItem.TouchEventReceived += new EventHandler<TouchEventArgs>(this.ListItemHookedChildTouchEventReceived);
			return popupListPanelItem;
		}

		private void ListItemUpdater(ListPanelItem item)
		{
			if (item is PopupList.PopupListPanelItem)
			{
				PopupList.PopupListPanelItem popupListPanelItem = item as PopupList.PopupListPanelItem;
				popupListPanelItem.Height = 50f;
				popupListPanelItem.Text = this.ListItems[popupListPanelItem.Index];
				popupListPanelItem.IsSelected = (item.Index == this.listSelectedIndex);
			}
		}

		private void ListItemHookedChildTouchEventReceived(object sender, TouchEventArgs eventArgs)
		{
			TouchEvent primaryTouchEvent = eventArgs.TouchEvents.PrimaryTouchEvent;
			PopupList.PopupListPanelItem popupListPanelItem = sender as PopupList.PopupListPanelItem;
			if (popupListPanelItem != null)
			{
				switch (primaryTouchEvent.Type)
				{
				case TouchEventType.Up:
					if (this.canSelectItem)
					{
						this.ApplyTouchItemIndex();
						this.HideDialog();
						if (this.SelectionChanged != null && this.previousSelectedIndex != this.selectedIndex)
						{
							this.SelectionChanged.Invoke(this, new PopupSelectionChangedEventArgs(this.previousSelectedIndex, this.SelectedIndex));
						}
					}
					break;
				case TouchEventType.Down:
					this.touchTotalTime = 0f;
					this.touchItemIndex = popupListPanelItem.Index;
					this.timerUpdate = true;
					this.canSelectItem = true;
					return;
				default:
					return;
				}
			}
		}

		private void dragEventHandler(object sender, DragEventArgs e)
		{
			this.canSelectItem = false;
			if (this.touchItemIndex > 0)
			{
				this.CancelTouchItemIndex();
				return;
			}
			if (this.previousSelectedIndex != this.listSelectedIndex)
			{
				this.updateListItems();
			}
		}

		private void updateListItems()
		{
			if (this.dialog != null)
			{
				foreach (Widget current in this.dialog.Children)
				{
					ListPanel listPanel = current as ListPanel;
					if (listPanel != null)
					{
						listPanel.UpdateItems();
						break;
					}
				}
			}
		}

		private void HandleListItemsItemChanged(object sender, EventArgs e)
		{
			if (this.selectedIndex >= this.listItems.Count)
			{
				this.selectedIndex = this.listItems.Count - 1;
				if (this.selectedIndex <= 0)
				{
					this.selectedIndex = 0;
				}
			}
			this.selectedLabel.Text = ((this.listItems.Count > 0) ? this.listItems[this.selectedIndex] : "");
			if (this.ListItemsChanged != null)
			{
				this.ListItemsChanged.Invoke(this, new PopupListItemsChangedEventArgs());
			}
		}
	}
}
