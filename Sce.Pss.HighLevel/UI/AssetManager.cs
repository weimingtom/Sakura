using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Sce.Pss.HighLevel.UI
{
	internal static class AssetManager
	{
		private class ImageData
		{
			public ImageSize Size;

			public byte[] Buffer;
		}

		private static Dictionary<string, Texture2D> cache;

		private static Dictionary<SystemImageAsset, string> imageAssetFilename;

		private static Dictionary<SystemImageAsset, NinePatchMargin> imageAssetNinePatchMargin;

		private static Dictionary<SystemImageAsset, ImageSize> imageAssetSize;

		private static Dictionary<string, object> loadingImageFilename;

		private static object lockObj;

		private static Dictionary<string, AssetManager.ImageData> imageCache;

		internal static void LoadSystemAsset()
		{
		}

		internal static void LoadTexture(string filename, bool isAsync)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("File not found.", filename);
			}
			if (!AssetManager.cache.ContainsKey(filename))
			{
				if (isAsync)
				{
					if (!AssetManager.loadingImageFilename.ContainsKey(filename))
					{
						AssetManager.loadingImageFilename.Add(filename, null);
						ThreadPool.QueueUserWorkItem(new WaitCallback(AssetManager.asyncLoad), filename);
						return;
					}
				}
				else
				{
					AssetManager.loadingImageFilename.Remove(filename);
					Texture2D texture2D = new Texture2D(filename, false);
					AssetManager.cache[filename] = texture2D;
				}
			}
		}

		internal static void LoadTextureFromAssembly(string filename, bool isAsync)
		{
			if (!AssetManager.cache.ContainsKey(filename))
			{
				if (isAsync)
				{
					if (!AssetManager.loadingImageFilename.ContainsKey(filename))
					{
						AssetManager.loadingImageFilename.Add(filename, null);
						ThreadPool.QueueUserWorkItem(new WaitCallback(AssetManager.asyncLoadFromAssembly), filename);
						return;
					}
				}
				else
				{
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					string text = typeof(UISystem).Namespace + "." + filename.Replace("/", ".");
					AssetManager.loadingImageFilename.Remove(filename);
					using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text))
					{
						if (manifestResourceStream == null)
						{
							throw new FileNotFoundException("File not found.", filename);
						}
						byte[] array = new byte[manifestResourceStream.Length];
						manifestResourceStream.Read(array, 0, array.Length);
						Texture2D texture2D = new Texture2D(array, false);
						AssetManager.cache[filename] = texture2D;
					}
				}
			}
		}

		internal static Texture2D GetTexture(string filename)
		{
			if (AssetManager.cache.ContainsKey(filename))
			{
				return AssetManager.cache[filename];
			}
			if (!AssetManager.loadingImageFilename.ContainsKey(filename))
			{
				AssetManager.LoadTexture(filename, false);
				return AssetManager.cache[filename];
			}
			AssetManager.ImageData imageData = null;
			lock (AssetManager.lockObj)
			{
				if (AssetManager.imageCache.ContainsKey(filename))
				{
					imageData = AssetManager.imageCache[filename];
					AssetManager.imageCache.Remove(filename);
				}
			}
			if (imageData != null)
			{
				Texture2D texture2D = new Texture2D(imageData.Size.Width, imageData.Size.Height, false, (PixelFormat)1);
				texture2D.SetPixels(0, imageData.Buffer);
				AssetManager.cache.Add(filename, texture2D);
				AssetManager.loadingImageFilename.Remove(filename);
				return texture2D;
			}
			return null;
		}

		internal static Texture2D GetTexture(SystemImageAsset name)
		{
			return AssetManager.GetTexture(AssetManager.imageAssetFilename[name]);
		}

		internal static string GetSystemFileName(SystemImageAsset name)
		{
			return AssetManager.imageAssetFilename[name];
		}

		internal static void WaitForLoad(string filename)
		{
			while (true)
			{
				Texture2D texture = AssetManager.GetTexture(filename);
				if (texture != null)
				{
					break;
				}
				Thread.Sleep(0);
			}
		}

		internal static void WaitForLoadAll()
		{
			while (AssetManager.loadingImageFilename.Count > 0)
			{
				using (Dictionary<string, object>.KeyCollection.Enumerator enumerator = AssetManager.loadingImageFilename.Keys.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						AssetManager.WaitForLoad(current);
					}
				}
			}
		}

		internal static bool UnloadTexture(string filename)
		{
			if (AssetManager.cache.ContainsKey(filename))
			{
				Texture2D texture2D = AssetManager.cache[filename];
				AssetManager.cache.Remove(filename);
				texture2D.Dispose();
				return true;
			}
			return false;
		}

		internal static void UnloadAllTexture()
		{
			using (Dictionary<string, Texture2D>.ValueCollection.Enumerator enumerator = AssetManager.cache.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Texture2D current = enumerator.Current;
					current.Dispose();
				}
			}
			AssetManager.cache.Clear();
		}

		internal static NinePatchMargin GetNinePatchMargin(SystemImageAsset name)
		{
			return AssetManager.imageAssetNinePatchMargin[name];
		}

		internal static ImageSize GetImageSize(SystemImageAsset name)
		{
			return AssetManager.imageAssetSize[name];
		}

		internal static ImageSize GetImageSize(string filename)
		{
			Texture2D texture = AssetManager.GetTexture(filename);
			if (texture != null)
			{
				return new ImageSize(texture.Width, texture.Height);
			}
			return default(ImageSize);
		}

		internal static bool IsLoadedTexture(string filename)
		{
			bool flag = AssetManager.cache.ContainsKey(filename);
			if (!flag)
			{
				lock (AssetManager.lockObj)
				{
					flag = AssetManager.imageCache.ContainsKey(filename);
				}
			}
			return flag;
		}

		private static void asyncLoad(object state)
		{
			string text = state as string;
			lock (AssetManager.lockObj)
			{
				if (AssetManager.imageCache.ContainsKey(text))
				{
					text = null;
				}
			}
			if (text != null)
			{
				Image image = new Image(text);
				AssetManager.ImageData imageData = new AssetManager.ImageData();
				image.Decode();
				imageData.Size = image.Size;
				imageData.Buffer = image.ToBuffer();
				image.Dispose();
				lock (AssetManager.lockObj)
				{
					AssetManager.imageCache.Add(text, imageData);
				}
			}
		}

		private static void asyncLoadFromAssembly(object state)
		{
			string text = state as string;
			lock (AssetManager.lockObj)
			{
				if (AssetManager.imageCache.ContainsKey(text))
				{
					text = null;
				}
			}
			if (text != null)
			{
#if false
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				string text2 = typeof(UISystem).Namespace + "." + text.Replace("/", ".");
				byte[] array;
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text2))
				{
					if (manifestResourceStream == null)
					{
						throw new FileNotFoundException("File not found.", text);
					}
					array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
				}
				Image image = new Image(array);
#else
				string filename = "/Application/Sce.Pss.HighLevel/UI/" + text;
				Image image = new Image(filename);
#endif
				AssetManager.ImageData imageData = new AssetManager.ImageData();
				image.Decode();
				imageData.Size = image.Size;
				imageData.Buffer = image.ToBuffer();
				image.Dispose();
				lock (AssetManager.lockObj)
				{
					AssetManager.imageCache.Add(text, imageData);
				}
			}
		}

		static AssetManager()
		{
			AssetManager.cache = new Dictionary<string, Texture2D>();
			AssetManager.loadingImageFilename = new Dictionary<string, object>();
			AssetManager.lockObj = new object();
			AssetManager.imageCache = new Dictionary<string, AssetManager.ImageData>();
			Dictionary<SystemImageAsset, string> dictionary = new Dictionary<SystemImageAsset, string>();
			dictionary.Add(SystemImageAsset.ButtonBackgroundNormal, "system_assets/button_9patch_normal.png");
			dictionary.Add(SystemImageAsset.ButtonBackgroundPressed, "system_assets/button_9patch_press.png");
			dictionary.Add(SystemImageAsset.ButtonBackgroundDisabled, "system_assets/button_9patch_disable.png");
			dictionary.Add(SystemImageAsset.CheckBoxCheckedNormal, "system_assets/check_box_active.png");
			dictionary.Add(SystemImageAsset.CheckBoxUncheckedNormal, "system_assets/check_box_normal.png");
			dictionary.Add(SystemImageAsset.CheckBoxCheckedPressed, "system_assets/check_box_active_press.png");
			dictionary.Add(SystemImageAsset.CheckBoxUncheckedPressed, "system_assets/check_box_normal_press.png");
			dictionary.Add(SystemImageAsset.CheckBoxCheckedDisabled, "system_assets/check_box_active_disable.png");
			dictionary.Add(SystemImageAsset.CheckBoxUncheckedDisabled, "system_assets/check_box_normal_disable.png");
			dictionary.Add(SystemImageAsset.RadioButtonCheckedNormal, "system_assets/radio_bt_selected.png");
			dictionary.Add(SystemImageAsset.RadioButtonUncheckedNormal, "system_assets/radio_bt_normal.png");
			dictionary.Add(SystemImageAsset.RadioButtonCheckedPressed, "system_assets/radio_bt_selected_press.png");
			dictionary.Add(SystemImageAsset.RadioButtonUncheckedPressed, "system_assets/radio_bt_normal_press.png");
			dictionary.Add(SystemImageAsset.RadioButtonCheckedDisabled, "system_assets/radio_bt_selected_disable.png");
			dictionary.Add(SystemImageAsset.RadioButtonUncheckedDisabled, "system_assets/radio_bt_normal_disable.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalBaseNormal, "system_assets/slider_bar_normal_9patch.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalBaseDisabled, "system_assets/slider_bar_normal_9patch_disable.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalBarNormal, "system_assets/slider_bar_active_9patch.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalBarDisabled, "system_assets/slider_bar_active_9patch_disable.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalHandleNormal, "system_assets/slider.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalHandlePressed, "system_assets/slider_press.png");
			dictionary.Add(SystemImageAsset.SliderHorizontalHandleDisabled, "system_assets/slider_disable.png");
			dictionary.Add(SystemImageAsset.SliderVerticalBaseNormal, "system_assets/slider_bar_verti_normal_9patch.png");
			dictionary.Add(SystemImageAsset.SliderVerticalBaseDisabled, "system_assets/slider_bar_verti_normal_9patch_disable.png");
			dictionary.Add(SystemImageAsset.SliderVerticalBarNormal, "system_assets/slider_bar_verti_active_9patch.png");
			dictionary.Add(SystemImageAsset.SliderVerticalBarDisabled, "system_assets/slider_bar_verti_active_9patch_disable.png");
			dictionary.Add(SystemImageAsset.SliderVerticalHandleNormal, "system_assets/slider_verti.png");
			dictionary.Add(SystemImageAsset.SliderVerticalHandlePressed, "system_assets/slider_verti_press.png");
			dictionary.Add(SystemImageAsset.SliderVerticalHandleDisabled, "system_assets/slider_verti_disable.png");
			dictionary.Add(SystemImageAsset.ProgressBarBase, "system_assets/slider_bar_normal_9patch.png");
			dictionary.Add(SystemImageAsset.ProgressBarNormal, "system_assets/slider_bar_active_9patch.png");
			dictionary.Add(SystemImageAsset.ProgressBarAccelerator, "system_assets/progress_accelerator.png");
			dictionary.Add(SystemImageAsset.ScrollBarHorizontalBackground, "system_assets/scroll_bar_horiz_normal_9patch.png");
			dictionary.Add(SystemImageAsset.ScrollBarHorizontalBar, "system_assets/scroll_bar_horiz_active_9patch.png");
			dictionary.Add(SystemImageAsset.ScrollBarVerticalBackground, "system_assets/scroll_bar_verti_normal_9patch.png");
			dictionary.Add(SystemImageAsset.ScrollBarVerticalBar, "system_assets/scroll_bar_verti_active_9patch.png");
			dictionary.Add(SystemImageAsset.EditableTextBackgroundNormal, "system_assets/text_field_9patch_normal.png");
			dictionary.Add(SystemImageAsset.EditableTextBackgroundDisabled, "system_assets/text_field_9patch_disable.png");
			dictionary.Add(SystemImageAsset.BusyIndicator, "system_assets/busy48.png");
			dictionary.Add(SystemImageAsset.DialogBackground, "system_assets/panel_9patch.png");
			dictionary.Add(SystemImageAsset.PagePanelNormal, "system_assets/pagepanel_point_normal.png");
			dictionary.Add(SystemImageAsset.PagePanelActive, "system_assets/pagepanel_point_active.png");
			dictionary.Add(SystemImageAsset.ListPanelSeparatorTop, "system_assets/list_separator_top_9patch.png");
			dictionary.Add(SystemImageAsset.ListPanelSeparatorBottom, "system_assets/list_separator_bottom_9patch.png");
			dictionary.Add(SystemImageAsset.ListPanelSectionSeparator, "system_assets/separator.png");
			dictionary.Add(SystemImageAsset.MessageDialogBackground, "system_assets/panel_9patch.png");
			dictionary.Add(SystemImageAsset.MessageDialogSeparator, "system_assets/title_separator_9patch.png");
			dictionary.Add(SystemImageAsset.SpinBoxBase, "system_assets/spin_base.png");
			dictionary.Add(SystemImageAsset.SpinBoxCenter, "system_assets/spin_center.png");
			dictionary.Add(SystemImageAsset.PopupListBackgroundNormal, "system_assets/popup_9patch_normal.png");
			dictionary.Add(SystemImageAsset.PopupListBackgroundPressed, "system_assets/popup_9patch_press.png");
			dictionary.Add(SystemImageAsset.PopupListBackgroundDisabled, "system_assets/popup_9patch_disable.png");
			dictionary.Add(SystemImageAsset.PopupListItemFocus, "system_assets/popup_focus_9patch.png");
			dictionary.Add(SystemImageAsset.BackButtonBackgroundNormal, "system_assets/back_button_9patch_normal.png");
			dictionary.Add(SystemImageAsset.BackButtonBackgroundPressed, "system_assets/back_button_9patch_press.png");
			dictionary.Add(SystemImageAsset.BackButtonBackgroundDisabled, "system_assets/back_button_9patch_disable.png");
			dictionary.Add(SystemImageAsset.NavigationBarBackground, "system_assets/titlebar_9patch.png");
			AssetManager.imageAssetFilename = dictionary;
			Dictionary<SystemImageAsset, ImageSize> dictionary2 = new Dictionary<SystemImageAsset, ImageSize>();
			dictionary2.Add(SystemImageAsset.ButtonBackgroundNormal, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.ButtonBackgroundPressed, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.ButtonBackgroundDisabled, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxCheckedNormal, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxUncheckedNormal, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxCheckedPressed, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxUncheckedPressed, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxCheckedDisabled, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.CheckBoxUncheckedDisabled, new ImageSize(56, 56));
			dictionary2.Add(SystemImageAsset.RadioButtonCheckedNormal, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.RadioButtonUncheckedNormal, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.RadioButtonCheckedPressed, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.RadioButtonUncheckedPressed, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.RadioButtonCheckedDisabled, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.RadioButtonUncheckedDisabled, new ImageSize(39, 39));
			dictionary2.Add(SystemImageAsset.SliderHorizontalBaseNormal, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.SliderHorizontalBaseDisabled, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.SliderHorizontalBarNormal, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.SliderHorizontalBarDisabled, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.SliderHorizontalHandleNormal, new ImageSize(40, 58));
			dictionary2.Add(SystemImageAsset.SliderHorizontalHandlePressed, new ImageSize(40, 58));
			dictionary2.Add(SystemImageAsset.SliderHorizontalHandleDisabled, new ImageSize(40, 58));
			dictionary2.Add(SystemImageAsset.SliderVerticalBaseNormal, new ImageSize(16, 17));
			dictionary2.Add(SystemImageAsset.SliderVerticalBaseDisabled, new ImageSize(16, 17));
			dictionary2.Add(SystemImageAsset.SliderVerticalBarNormal, new ImageSize(16, 17));
			dictionary2.Add(SystemImageAsset.SliderVerticalBarDisabled, new ImageSize(16, 17));
			dictionary2.Add(SystemImageAsset.SliderVerticalHandleNormal, new ImageSize(58, 40));
			dictionary2.Add(SystemImageAsset.SliderVerticalHandlePressed, new ImageSize(58, 40));
			dictionary2.Add(SystemImageAsset.SliderVerticalHandleDisabled, new ImageSize(58, 40));
			dictionary2.Add(SystemImageAsset.ProgressBarBase, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.ProgressBarNormal, new ImageSize(17, 16));
			dictionary2.Add(SystemImageAsset.ProgressBarAccelerator, new ImageSize(64, 8));
			dictionary2.Add(SystemImageAsset.ScrollBarHorizontalBackground, new ImageSize(11, 10));
			dictionary2.Add(SystemImageAsset.ScrollBarHorizontalBar, new ImageSize(11, 10));
			dictionary2.Add(SystemImageAsset.ScrollBarVerticalBackground, new ImageSize(10, 11));
			dictionary2.Add(SystemImageAsset.ScrollBarVerticalBar, new ImageSize(10, 11));
			dictionary2.Add(SystemImageAsset.EditableTextBackgroundNormal, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.EditableTextBackgroundDisabled, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.BusyIndicator, new ImageSize(192, 96));
			dictionary2.Add(SystemImageAsset.DialogBackground, new ImageSize(69, 69));
			dictionary2.Add(SystemImageAsset.PagePanelNormal, new ImageSize(24, 24));
			dictionary2.Add(SystemImageAsset.PagePanelActive, new ImageSize(24, 24));
			dictionary2.Add(SystemImageAsset.ListPanelSeparatorTop, new ImageSize(139, 1));
			dictionary2.Add(SystemImageAsset.ListPanelSeparatorBottom, new ImageSize(139, 1));
			dictionary2.Add(SystemImageAsset.ListPanelSectionSeparator, new ImageSize(15, 25));
			dictionary2.Add(SystemImageAsset.MessageDialogBackground, new ImageSize(69, 69));
			dictionary2.Add(SystemImageAsset.MessageDialogSeparator, new ImageSize(139, 2));
			dictionary2.Add(SystemImageAsset.SpinBoxBase, new ImageSize(40, 200));
			dictionary2.Add(SystemImageAsset.SpinBoxCenter, new ImageSize(9, 30));
			dictionary2.Add(SystemImageAsset.PopupListBackgroundNormal, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.PopupListBackgroundPressed, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.PopupListBackgroundDisabled, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.PopupListItemFocus, new ImageSize(139, 1));
			dictionary2.Add(SystemImageAsset.BackButtonBackgroundNormal, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.BackButtonBackgroundPressed, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.BackButtonBackgroundDisabled, new ImageSize(86, 56));
			dictionary2.Add(SystemImageAsset.NavigationBarBackground, new ImageSize(64, 64));
			AssetManager.imageAssetSize = dictionary2;
			Dictionary<SystemImageAsset, NinePatchMargin> dictionary3 = new Dictionary<SystemImageAsset, NinePatchMargin>();
			dictionary3.Add(SystemImageAsset.ButtonBackgroundNormal, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.ButtonBackgroundPressed, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.ButtonBackgroundDisabled, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.SliderHorizontalBaseNormal, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.SliderHorizontalBaseDisabled, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.SliderHorizontalBarNormal, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.SliderHorizontalBarDisabled, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.SliderVerticalBaseNormal, new NinePatchMargin
			{
				Left = 0,
				Top = 8,
				Right = 0,
				Bottom = 8
			});
			dictionary3.Add(SystemImageAsset.SliderVerticalBaseDisabled, new NinePatchMargin
			{
				Left = 0,
				Top = 8,
				Right = 0,
				Bottom = 8
			});
			dictionary3.Add(SystemImageAsset.SliderVerticalBarNormal, new NinePatchMargin
			{
				Left = 0,
				Top = 8,
				Right = 0,
				Bottom = 8
			});
			dictionary3.Add(SystemImageAsset.SliderVerticalBarDisabled, new NinePatchMargin
			{
				Left = 0,
				Top = 8,
				Right = 0,
				Bottom = 8
			});
			dictionary3.Add(SystemImageAsset.ProgressBarBase, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ProgressBarNormal, new NinePatchMargin
			{
				Left = 8,
				Top = 0,
				Right = 8,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ScrollBarHorizontalBackground, new NinePatchMargin
			{
				Left = 5,
				Top = 0,
				Right = 5,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ScrollBarHorizontalBar, new NinePatchMargin
			{
				Left = 5,
				Top = 0,
				Right = 5,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ScrollBarVerticalBackground, new NinePatchMargin
			{
				Left = 0,
				Top = 5,
				Right = 0,
				Bottom = 5
			});
			dictionary3.Add(SystemImageAsset.ScrollBarVerticalBar, new NinePatchMargin
			{
				Left = 0,
				Top = 5,
				Right = 0,
				Bottom = 5
			});
			dictionary3.Add(SystemImageAsset.EditableTextBackgroundNormal, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.EditableTextBackgroundDisabled, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.DialogBackground, new NinePatchMargin
			{
				Left = 34,
				Top = 34,
				Right = 34,
				Bottom = 34
			});
			dictionary3.Add(SystemImageAsset.ListPanelSeparatorTop, new NinePatchMargin
			{
				Left = 68,
				Top = 0,
				Right = 68,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ListPanelSeparatorBottom, new NinePatchMargin
			{
				Left = 68,
				Top = 0,
				Right = 68,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.ListPanelSectionSeparator, new NinePatchMargin
			{
				Left = 7,
				Top = 0,
				Right = 7,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.MessageDialogBackground, new NinePatchMargin
			{
				Left = 34,
				Top = 34,
				Right = 34,
				Bottom = 34
			});
			dictionary3.Add(SystemImageAsset.MessageDialogSeparator, new NinePatchMargin
			{
				Left = 68,
				Top = 0,
				Right = 68,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.SpinBoxBase, new NinePatchMargin
			{
				Left = 19,
				Top = 99,
				Right = 19,
				Bottom = 99
			});
			dictionary3.Add(SystemImageAsset.SpinBoxCenter, new NinePatchMargin
			{
				Left = 4,
				Top = 14,
				Right = 4,
				Bottom = 14
			});
			dictionary3.Add(SystemImageAsset.PopupListBackgroundNormal, new NinePatchMargin
			{
				Left = 42,
				Top = 0,
				Right = 42,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.PopupListItemFocus, new NinePatchMargin
			{
				Left = 68,
				Top = 0,
				Right = 68,
				Bottom = 0
			});
			dictionary3.Add(SystemImageAsset.BackButtonBackgroundNormal, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.BackButtonBackgroundPressed, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.BackButtonBackgroundDisabled, new NinePatchMargin
			{
				Left = 42,
				Top = 27,
				Right = 42,
				Bottom = 27
			});
			dictionary3.Add(SystemImageAsset.NavigationBarBackground, new NinePatchMargin
			{
				Left = 31,
				Top = 31,
				Right = 31,
				Bottom = 31
			});
			AssetManager.imageAssetNinePatchMargin = dictionary3;
		}
	}
}
