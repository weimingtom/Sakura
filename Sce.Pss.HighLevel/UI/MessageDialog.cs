using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class MessageDialog : Dialog
	{
		private const float minimumWidth = 500f;

		private const float minimumHeight = 260f;

		private const float margin = 15f;

		private const float titleLabelHeight = 70f;

		private bool needUpdateMessageSize = true;

		private bool showTitle;

		private MessageDialogStyle style;

		private ScrollPanel messageScrollPanel;

		private Label message;

		private Label title;

		private ImageBox separatorImage;

		private Button okButton;

		private Button cancelButton;

		private Button leftButton;

		private Button rightButton;

		private Panel buttonPanel;

		public event EventHandler<MessageDialogButtonEventArgs> ButtonPressed;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				if (value > 500f)
				{
					base.Width = value;
				}
				else
				{
					base.Width = 500f;
				}
				if (this.messageScrollPanel != null)
				{
					this.messageScrollPanel.PanelWidth = this.messageScrollPanel.Width;
					this.needUpdateMessageSize = true;
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
				if (value > 260f)
				{
					base.Height = value;
					return;
				}
				base.Height = 260f;
			}
		}

		public string Message
		{
			get
			{
				return this.message.Text;
			}
			set
			{
				this.message.Text = value;
				this.needUpdateMessageSize = true;
			}
		}

		public string Title
		{
			get
			{
				return this.title.Text;
			}
			set
			{
				this.title.Text = value;
			}
		}

		public bool ShowTitle
		{
			get
			{
				return this.showTitle;
			}
			set
			{
				this.showTitle = value;
				if (this.title != null)
				{
					this.title.Visible = this.showTitle;
				}
				if (this.separatorImage != null)
				{
					this.separatorImage.Visible = this.showTitle;
				}
			}
		}

		public MessageDialogStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
				this.UpdateLayout();
			}
		}

		public MessageDialog() : base(null, null)
		{
			base.Width = 500f;
			base.Height = 260f;
			base.X = (float)UISystem.FramebufferWidth * 0.1f;
			base.Y = (float)UISystem.FramebufferHeight * 0.05f;
			this.title = new Label();
			this.title.SetPosition(15f, 15f);
			this.title.SetSize(470f, 70f);
			this.title.HorizontalAlignment = HorizontalAlignment.Center;
			this.title.VerticalAlignment = VerticalAlignment.Middle;
			this.title.TextColor = new UIColor(1f, 1f, 1f, 1f);
			this.title.Font = new Font(0, 28, 0);
			this.title.Text = "";
			this.title.Anchors = (Anchors.Top | Anchors.Height | Anchors.Left | Anchors.Right);
			this.AddChildLast(this.title);
			this.separatorImage = new ImageBox();
			this.separatorImage.X = 35f;
			this.separatorImage.Y = 85f;
			this.separatorImage.Width = 430f;
			this.separatorImage.Height = 2f;
			this.separatorImage.Image = new ImageAsset(SystemImageAsset.MessageDialogSeparator);
			this.separatorImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.MessageDialogSeparator);
			this.separatorImage.ImageScaleType = ImageScaleType.NinePatch;
			this.separatorImage.Anchors = (Anchors.Top | Anchors.Height | Anchors.Left | Anchors.Right);
			this.AddChildLast(this.separatorImage);
			this.okButton = new Button();
			this.okButton.Text = "OK";
			this.okButton.ButtonAction += new EventHandler<TouchEventArgs>(this.ButtonExecute);
			this.cancelButton = new Button();
			this.cancelButton.Text = "Cancel";
			this.cancelButton.ButtonAction += new EventHandler<TouchEventArgs>(this.ButtonExecute);
			this.buttonPanel = new Panel();
			this.buttonPanel.SetPosition(15f, 230f - this.okButton.Height);
			this.buttonPanel.SetSize(470f, this.okButton.Height);
			this.buttonPanel.BackgroundColor = default(UIColor);
			this.buttonPanel.Anchors = (Anchors.Bottom | Anchors.Height | Anchors.Width);
			this.AddChildLast(this.buttonPanel);
			this.buttonPanel.AddChildLast(this.okButton);
			this.buttonPanel.AddChildLast(this.cancelButton);
			this.messageScrollPanel = new ScrollPanel();
			this.messageScrollPanel.SetPosition(35f, 105f);
			this.messageScrollPanel.SetSize(445f, 58f);
			this.messageScrollPanel.PanelWidth = this.messageScrollPanel.Width;
			this.messageScrollPanel.PanelHeight = this.messageScrollPanel.Height;
			this.messageScrollPanel.PanelColor = new UIColor(0f, 0f, 0f, 0f);
			this.messageScrollPanel.ScrollBarVisibility = ScrollBarVisibility.ScrollableVisible;
			this.messageScrollPanel.Anchors = (Anchors.Top | Anchors.Bottom | Anchors.Left | Anchors.Right);
			this.AddChildLast(this.messageScrollPanel);
			this.message = new Label();
			this.message.Width = this.messageScrollPanel.PanelWidth - 15f;
			this.message.Height = this.messageScrollPanel.PanelHeight;
			this.message.TextColor = new UIColor(1f, 1f, 1f, 1f);
			this.message.Font = new Font(0, 24, 0);
			this.message.HorizontalAlignment = HorizontalAlignment.Left;
			this.message.VerticalAlignment = VerticalAlignment.Top;
			this.message.Text = "";
			this.message.Anchors = (Anchors.Top | Anchors.Bottom | Anchors.Left | Anchors.Right);
			this.messageScrollPanel.AddChildLast(this.message);
			this.Style = MessageDialogStyle.OkCancel;
			this.ShowTitle = true;
			this.PriorityHit = true;
			this.Width = (float)((int)((float)UISystem.FramebufferWidth * 0.8f));
			this.Height = (float)((int)((float)UISystem.FramebufferHeight * 0.9f));
		}

		public MessageDialog(MessageDialogStyle style, string title, string message) : this()
		{
			this.Message = message;
			this.Title = title;
			this.Style = style;
		}

		public static MessageDialog CreateAndShow(MessageDialogStyle style, string title, string message)
		{
			MessageDialog messageDialog = new MessageDialog(style, title, message);
			messageDialog.Show();
			return messageDialog;
		}

		protected internal override void Render()
		{
			if (this.needUpdateMessageSize)
			{
				this.messageScrollPanel.PanelHeight = this.message.TextHeight;
				this.needUpdateMessageSize = false;
			}
			base.Render();
		}

		protected override void DisposeSelf()
		{
			if (this.message != null && this.message.Font != null)
			{
				this.message.Font.Dispose();
			}
			if (this.title != null && this.title.Font != null)
			{
				this.title.Font.Dispose();
			}
			if (this.separatorImage != null && this.separatorImage.Image != null)
			{
				this.separatorImage.Image.Dispose();
			}
			base.DisposeSelf();
		}

		private void UpdateLayout()
		{
			if (this.Style == MessageDialogStyle.Ok)
			{
				this.leftButton = this.okButton;
				this.rightButton = this.cancelButton;
				this.okButton.X = (this.buttonPanel.Width - this.okButton.Width) / 2f;
				this.cancelButton.Visible = false;
				this.cancelButton.TouchResponse = false;
				return;
			}
			if (this.Style == MessageDialogStyle.OkCancel)
			{
				if (SystemParameters.YesNoLayout == (YesNoLayout)0)
				{
					this.leftButton = this.okButton;
					this.rightButton = this.cancelButton;
				}
				else
				{
					this.leftButton = this.cancelButton;
					this.rightButton = this.okButton;
				}
				this.buttonPanel.X = (this.Width - this.buttonPanel.Width) / 2f;
				this.leftButton.X = 0f;
				this.rightButton.X = this.buttonPanel.Width - this.rightButton.Width;
				this.cancelButton.Visible = true;
				this.cancelButton.TouchResponse = true;
			}
		}

		private void ButtonExecute(object sender, TouchEventArgs e)
		{
			if (this.ButtonPressed != null)
			{
				Button button = sender as Button;
				if (button.Text == "OK")
				{
					MessageDialogButtonEventArgs messageDialogButtonEventArgs = new MessageDialogButtonEventArgs(MessageDialogResult.Ok);
					this.ButtonPressed.Invoke(this, messageDialogButtonEventArgs);
				}
				else if (button.Text == "Cancel")
				{
					MessageDialogButtonEventArgs messageDialogButtonEventArgs2 = new MessageDialogButtonEventArgs(MessageDialogResult.Cancel);
					this.ButtonPressed.Invoke(this, messageDialogButtonEventArgs2);
				}
			}
			base.Hide();
		}
	}
}
