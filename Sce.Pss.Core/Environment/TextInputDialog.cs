using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Sakura;

namespace Sce.Pss.Core.Environment
{
	public class TextInputDialog: IDisposable
	{
		private string __text;
		private DialogResult __result = DialogResult.None;
		
		public TextInputDialog()
		{
		
		}
		
		public void Dispose()
		{
			__inputBox = null;
		}
		
		public string Text
		{
			get
			{
				return this.__text;
			}
			set
			{
				this.__text = value;
			}
		}
		
		public void Open()
		{
			this.__result = __ShowInputDialog(SakuraGameWindow.getTitle(), ref __text);
		}
		
		public CommonDialogState State
		{
			get
			{
				if (__inputBox == null)
				{
					return CommonDialogState.None;
				}
				else if (__inputBox.Visible == true)
				{
					return CommonDialogState.Running;
				}
				else
				{
					return CommonDialogState.Finished;
				}
			}
		}
		
		public CommonDialogResult Result
		{
			get
			{
				switch (this.__result)
				{
					case DialogResult.OK:
						return CommonDialogResult.OK;	
					
					case DialogResult.Cancel:
						return CommonDialogResult.Canceled;
					
					case DialogResult.Abort:
						return CommonDialogResult.Aborted;
				}
				return CommonDialogResult.Canceled;
			}
		}
		
		private Form __inputBox;
		//see https://stackoverflow.com/questions/97097/what-is-the-c-sharp-version-of-vb-nets-inputdialog
		private DialogResult __ShowInputDialog(string caption, ref string input)
	    {
	        System.Drawing.Size size = new System.Drawing.Size(300, 70);
	        if (__inputBox == null)
	        {
	        	__inputBox = new Form();
	        }
	        __inputBox.StartPosition = FormStartPosition.CenterScreen;
	
	        __inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
	        __inputBox.MinimizeBox = false;
	        __inputBox.MaximizeBox = false;
	        __inputBox.ClientSize = size;
	        __inputBox.Text = caption;
	
	        System.Windows.Forms.TextBox textBox = new TextBox();
	        textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
	        textBox.Location = new System.Drawing.Point(5, 10);
	        textBox.Text = input;
	        textBox.SelectAll();
	        __inputBox.Controls.Add(textBox);
	
	        Button okButton = new Button();
	        okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
	        okButton.Name = "okButton";
	        okButton.Size = new System.Drawing.Size(75, 23);
	        okButton.Text = "&OK";
	        okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
	        __inputBox.Controls.Add(okButton);
	
	        Button cancelButton = new Button();
	        cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
	        cancelButton.Name = "cancelButton";
	        cancelButton.Size = new System.Drawing.Size(75, 23);
	        cancelButton.Text = "&Cancel";
	        cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
	        __inputBox.Controls.Add(cancelButton);
	
	        __inputBox.AcceptButton = okButton;
	        __inputBox.CancelButton = cancelButton; 
	
	        DialogResult result = __inputBox.ShowDialog();
	        input = textBox.Text;
	        return result;
	    }
		
		private static String __InputBox(String caption, String prompt, String defaultText)
	    {
	        String localInputText = defaultText;
	        if (__InputQuery(caption, prompt, ref localInputText))
	        {
	            return localInputText;
	        }
	        else
	        {
	            return "";
	        }
	    }
    
		//see https://stackoverflow.com/questions/97097/what-is-the-c-sharp-version-of-vb-nets-inputdialog
		private static Boolean __InputQuery(String caption, String prompt, ref String value)
	    {
	        Form form;
	        form = new Form();
	        form.AutoScaleMode = AutoScaleMode.Font;
	        form.Font = SystemFonts.IconTitleFont;
	
	        SizeF dialogUnits;
	        dialogUnits = form.AutoScaleDimensions;
	
	        form.FormBorderStyle = FormBorderStyle.FixedDialog;
	        form.MinimizeBox = false;
	        form.MaximizeBox = false;
	        form.Text = caption;
	
	        form.ClientSize = new Size(
	                    __MulDiv(180, dialogUnits.Width, 4),
	                    __MulDiv(63, dialogUnits.Height, 8));
	
	        form.StartPosition = FormStartPosition.CenterScreen;
	
	        System.Windows.Forms.Label lblPrompt;
	        lblPrompt = new System.Windows.Forms.Label();
	        lblPrompt.Parent = form;
	        lblPrompt.AutoSize = true;
	        lblPrompt.Left = __MulDiv(8, dialogUnits.Width, 4);
	        lblPrompt.Top = __MulDiv(8, dialogUnits.Height, 8);
	        lblPrompt.Text = prompt;
	
	        System.Windows.Forms.TextBox edInput;
	        edInput = new System.Windows.Forms.TextBox();
	        edInput.Parent = form;
	        edInput.Left = lblPrompt.Left;
	        edInput.Top = __MulDiv(19, dialogUnits.Height, 8);
	        edInput.Width = __MulDiv(164, dialogUnits.Width, 4);
	        edInput.Text = value;
	        edInput.SelectAll();
	
	
	        int buttonTop = __MulDiv(41, dialogUnits.Height, 8);
	        //Command buttons should be 50x14 dlus
	        Size buttonSize = _ScaleSize(new Size(50, 14), dialogUnits.Width / 4, dialogUnits.Height / 8);
	
	        System.Windows.Forms.Button bbOk = new System.Windows.Forms.Button();
	        bbOk.Parent = form;
	        bbOk.Text = "OK";
	        bbOk.DialogResult = DialogResult.OK;
	        form.AcceptButton = bbOk;
	        bbOk.Location = new Point(__MulDiv(38, dialogUnits.Width, 4), buttonTop);
	        bbOk.Size = buttonSize;
	
	        System.Windows.Forms.Button bbCancel = new System.Windows.Forms.Button();
	        bbCancel.Parent = form;
	        bbCancel.Text = "Cancel";
	        bbCancel.DialogResult = DialogResult.Cancel;
	        form.CancelButton = bbCancel;
	        bbCancel.Location = new Point(__MulDiv(92, dialogUnits.Width, 4), buttonTop);
	        bbCancel.Size = buttonSize;
	
	        if (form.ShowDialog() == DialogResult.OK)
	        {
	            value = edInput.Text;
	            return true;
	        }
	        else
	        {
	            return false;
	        }
	    }
		
			
		private static int __MulDiv(int number, float numerator, int denominator) 
		{ 
			return (int)(((long)number * (int)numerator + (denominator >> 1)) / denominator);
		}

		private static Size _ScaleSize(Size size, float x, float y) 
		{ 
			return new Size((int)(size.Width * x), (int)(size.Height * y));
		}
		
		public TextInputMode Mode 
		{
			get 
			{
				Debug.Assert(false);
				return TextInputMode.Normal;
			}
			set
			{
				Debug.Assert(false);
			}
		}
	}
}
