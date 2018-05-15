using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.UI
{
	public static class UISystem
	{
		private static bool __USE_COLORMASK = false; //FIXME:hide some bugs
		
		private class ReversibleList<T> : List<T>
		{
			public IEnumerable<T> GetReverseOrder()
			{
				for (int i = base.Count - 1; i >= 0; i--)
				{
					yield return base[i];
				}
				yield break;
			}
		}

		private class GraphicsState
		{
			public bool EnableModeBlend;

			public bool EnableModeCullFace;

			public bool EnableModeDepthTest;

			public bool EnableModeScissorTest;

			public bool EnableModeStencilTest;

			public ColorMask ColorMask;

			public CullFace CullFace;

			public BlendFunc BlendFuncRgb;

			public BlendFunc BlendFuncAlpha;

			public Vector4 ClearColor;

			public readonly CullFace DefaultCullFace = new CullFace((CullFaceMode)2, (CullFaceDirection)1);

			public readonly BlendFunc DefaultBlendFanc = new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)5);
		}

		private static float scale = 1f;

		private static float dpi;

		private static Scene blankScene;

		private static Scene currentScene;

		private static Scene nextScene;

		private static Scene transitionUIElementScene;

		private static Scene modalScene;

		private static NavigationScene navigationScene;

		private static Scene debugInfoScene;

		private static Transition currentTransition = null;

		private static TransitionDrawOrder transitionDrawOrder;

		internal static Matrix4 viewProjectionMatrix;

		internal static Vector4 viewpoint;

		internal static Matrix4 screenMatrix;

		private static UISystem.ReversibleList<Scene> sceneLayers;

		private static bool needUpdateSceneLayersOrder;

		private static Stack<Scene> navigationSceneStack;

		private static Stopwatch stopwatch;

		private static float elapsedTime;

		private static UIElement zSortList;

		private static List<Effect> effects;

		private static List<Effect> addPendingEffects;

		private static List<Effect> removePendingEffects;

		private static TouchEventCollection lowLevelTouchEvents;

		private static TouchEventCollection highLevelTouchEvents;

		private static Dictionary<Widget, TouchEventCollection> touchEventSets;

		private static Dictionary<Widget, int> primaryIDs;

		private static bool effectUpdating;

		private static Dictionary<int, Widget> capturingWidgets;

		private static Dictionary<int, Widget> hitWidgets;

		private static Dictionary<KeyType, TimeSpan> nextKeyLongPressTimes;

		private static Dictionary<KeyType, TimeSpan> nextKeyRepeatTimes;

		private static readonly TimeSpan keyLongPressTime = TimeSpan.FromMilliseconds(800.0);

		private static readonly TimeSpan keyRepeatTime = TimeSpan.FromMilliseconds(800.0);

		private static readonly TimeSpan keyRepeatIntervalTime = TimeSpan.FromMilliseconds(500.0);

		internal static FrameBuffer offScreenFramebufferCache;

		internal static bool IsOffScreenRendering;

		internal static Texture2D transitionNextSceneTextureCache;

		internal static Texture2D transitionCurrentSceneTextureCache;

		private static UISystem.GraphicsState StoredGraphicsState;

		private static Vector2[] hitTestPointOffsets;

		private static UISprite[] hitTestPointSprts;

		private static readonly bool showHitTestDebugInfo = false;

		public static Widget KeyReceiverWidget
		{
			get;
			set;
		}

		public static GamePadData GamePadData
		{
			get;
			private set;
		}

		public static MotionData MotionData
		{
			get;
			private set;
		}

		public static int FramebufferWidth
		{
			get;
			private set;
		}

		public static int FramebufferHeight
		{
			get;
			private set;
		}

		internal static float Scale
		{
			get
			{
				return UISystem.scale;
			}
		}

		internal static bool Scaled
		{
			get;
			private set;
		}

		internal static float Dpi
		{
			get
			{
				return UISystem.dpi;
			}
			set
			{
				UISystem.dpi = value;
			}
		}

		public static Scene CurrentScene
		{
			get
			{
				return UISystem.currentScene;
			}
			private set
			{
				UISystem.currentScene = value;
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		internal static Scene NextScene
		{
			get
			{
				return UISystem.nextScene;
			}
			set
			{
				UISystem.nextScene = value;
				if (UISystem.nextScene != null)
				{
					UISystem.TransitionUIElementScene = new Scene();
				}
				else
				{
					if (UISystem.TransitionUIElementScene != null && UISystem.TransitionUIElementScene.RootWidget != null)
					{
						UISystem.TransitionUIElementScene.RootWidget.Dispose();
					}
					UISystem.TransitionUIElementScene = null;
					UISystem.TransitionDrawOrder = TransitionDrawOrder.CurrentScene;
				}
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		private static Scene TransitionUIElementScene
		{
			get
			{
				return UISystem.transitionUIElementScene;
			}
			set
			{
				UISystem.transitionUIElementScene = value;
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		private static Scene ModalScene
		{
			get
			{
				return UISystem.modalScene;
			}
			set
			{
				UISystem.modalScene = value;
				if (UISystem.modalScene != null)
				{
					UISystem.modalScene.ConsumeAllTouchEvent = true;
				}
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		private static NavigationScene NavigationScene
		{
			get
			{
				return UISystem.navigationScene;
			}
			set
			{
				UISystem.navigationScene = value;
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		private static Scene DebugInfoScene
		{
			get
			{
				return UISystem.debugInfoScene;
			}
			set
			{
				UISystem.debugInfoScene = value;
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		public static Transition ScenePushTransition
		{
			get;
			set;
		}

		public static Transition ScenePopTransition
		{
			get;
			set;
		}

		internal static RootUIElement TransitionUIElement
		{
			get
			{
				return UISystem.TransitionUIElementScene.RootWidget.RootUIElement;
			}
		}

		internal static TransitionDrawOrder TransitionDrawOrder
		{
			get
			{
				return UISystem.transitionDrawOrder;
			}
			set
			{
				UISystem.transitionDrawOrder = value;
				UISystem.needUpdateSceneLayersOrder = true;
			}
		}

		public static GraphicsContext GraphicsContext
		{
			get;
			private set;
		}

		public static Matrix4 ViewProjectionMatrix
		{
			get
			{
				return UISystem.viewProjectionMatrix;
			}
			internal set
			{
				UISystem.viewProjectionMatrix = value;
			}
		}

		public static Widget CurrentModalWidget
		{
			get
			{
				if (UISystem.ModalScene != null)
				{
					return UISystem.ModalScene.RootWidget.LinkedTree.LastChild.Value;
				}
				return null;
			}
		}

		public static TimeSpan CurrentTime
		{
			get;
			private set;
		}

		internal static Font DefaultFont
		{
			get;
			private set;
		}

		private static bool IsTransition
		{
			get
			{
				return UISystem.NextScene != null && UISystem.currentTransition != null;
			}
		}

		private static bool IsModalScene
		{
			get
			{
				return UISystem.ModalScene != null;
			}
		}

		internal static bool IsDialogEffect
		{
			get;
			set;
		}

		public static void Initialize(GraphicsContext graphics)
		{
            if (UISprite.__USE_SampleDraw)
            {
            	Sample.SampleDraw.Init(graphics);
            }			
			
			UISystem.stopwatch = new Stopwatch();
			UISystem.stopwatch.Start();
			UISystem.CurrentTime = TimeSpan.Zero;
			UISystem.elapsedTime = 0f;
			if (UISystem.Scaled)
			{
				UISystem.FramebufferWidth = (int)((float)graphics.Screen.Width / UISystem.scale);
				UISystem.FramebufferHeight = (int)((float)graphics.Screen.Height / UISystem.scale);
			}
			else
			{
				UISystem.FramebufferWidth = graphics.Screen.Width;
				UISystem.FramebufferHeight = graphics.Screen.Height;
			}
			float num = SystemParameters.DisplayDpiX / UISystem.scale;
			float num2 = SystemParameters.DisplayDpiY / UISystem.scale;
			UISystem.dpi = (num + num2) / 2f;
			UISystem.hitTestPointOffsets = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(-0.08f * num, -0.14f * num2),
				new Vector2(0.08f * num, -0.14f * num2),
				new Vector2(-0.04f * num, -0.07f * num2),
				new Vector2(0.04f * num, -0.07f * num2)
			};
			UISystem.GraphicsContext = graphics;
			UISystem.StoredGraphicsState = new UISystem.GraphicsState();
			UISystem.SetClipRegionFull();
			UISystem.GraphicsContext.SetFrameBuffer(null);
			UISystem.GraphicsContext.Enable(/*(EnableMode)4*/EnableMode.Blend, true);
			UISystem.GraphicsContext.Enable(/*(EnableMode)1*/EnableMode.ScissorTest, true);
			UISystem.GraphicsContext.SetBlendFunc(UISystem.StoredGraphicsState.DefaultBlendFanc);
			UISystem.KeyReceiverWidget = null;
			UISystem.zSortList = null;
			UISystem.effects = new List<Effect>();
			UISystem.addPendingEffects = new List<Effect>();
			UISystem.removePendingEffects = new List<Effect>();
			UISystem.effectUpdating = false;
			UISystem.lowLevelTouchEvents = new TouchEventCollection();
			UISystem.highLevelTouchEvents = new TouchEventCollection();
			UISystem.touchEventSets = new Dictionary<Widget, TouchEventCollection>();
			UISystem.primaryIDs = new Dictionary<Widget, int>();
			UISystem.capturingWidgets = new Dictionary<int, Widget>();
			UISystem.hitWidgets = new Dictionary<int, Widget>();
			UISystem.nextKeyLongPressTimes = new Dictionary<KeyType, TimeSpan>();
			UISystem.nextKeyRepeatTimes = new Dictionary<KeyType, TimeSpan>();
			UISystem.DefaultFont = new Font(0, 25, 0);
			UISystem.offScreenFramebufferCache = new FrameBuffer();
			UISystem.IsOffScreenRendering = false;
			ShaderProgramManager.Initialize();
			float num3 = 100000f;
			float num4 = 10f;
			float num5 = 1000f;
			UISystem.viewpoint = new Vector4((float)(UISystem.FramebufferWidth / 2), (float)(UISystem.FramebufferHeight / 2), -num5, 1f);
			Matrix4 matrix = new Matrix4(2f * num5 / (float)UISystem.FramebufferWidth, 0f, 0f, 0f, 0f, 2f * num5 / (float)UISystem.FramebufferHeight, 0f, 0f, 0f, 0f, (num3 + num4) / (num3 - num4), -1f, 0f, 0f, 2f * num3 * num4 / (num3 - num4), 0f);
			Matrix4 matrix2 = new Matrix4(1f, 0f, 0f, 0f, 0f, -1f, 0f, 0f, 0f, 0f, -1f, 0f, -UISystem.viewpoint.X, UISystem.viewpoint.Y, UISystem.viewpoint.Z, UISystem.viewpoint.W);
			matrix.Multiply(ref matrix2, out UISystem.viewProjectionMatrix);
			UISystem.screenMatrix = new Matrix4((float)UISystem.FramebufferWidth / 2f, 0f, 0f, 0f, 0f, (float)(-(float)UISystem.FramebufferHeight) / 2f, 0f, 0f, 0f, 0f, -1f, 0f, (float)UISystem.FramebufferWidth / 2f, (float)UISystem.FramebufferHeight / 2f, 0f, 1f);
			UISystem.blankScene = new Scene();
			UISystem.CurrentScene = UISystem.blankScene;
			UISystem.NavigationScene = new NavigationScene();
			UISystem.sceneLayers = new UISystem.ReversibleList<Scene>();
			UISystem.needUpdateSceneLayersOrder = true;
			UISystem.navigationSceneStack = new Stack<Scene>();
			UISystem.ScenePushTransition = new DefaultNavigationTransition(DefaultNavigationTransition.TransitionType.Push);
			UISystem.ScenePopTransition = new DefaultNavigationTransition(DefaultNavigationTransition.TransitionType.Pop);
			if (UISystem.showHitTestDebugInfo)
			{
				UISystem.DebugInfoScene = new Scene();
				UISystem.hitTestPointSprts = new UISprite[UISystem.hitTestPointOffsets.Length];
				for (int i = 0; i < UISystem.hitTestPointOffsets.Length; i++)
				{
					UISprite uISprite = new UISprite(1);
					UISpriteUnit unit = uISprite.GetUnit(0);
					unit.Width = 4f;
					unit.Height = 4f;
					UISystem.DebugInfoScene.RootWidget.RootUIElement.AddChildLast(uISprite);
					UISystem.hitTestPointSprts[i] = uISprite;
				}
			}
			UISystem.IsDialogEffect = false;
		}

		public static void Terminate()
		{
			UISystem.UpdateSceneLayersOrder();
			using (List<Scene>.Enumerator enumerator = UISystem.sceneLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scene current = enumerator.Current;
					if (current != null)
					{
						for (LinkedTree<Widget> linkedTree = current.RootWidget.LinkedTree; linkedTree != null; linkedTree = linkedTree.NextAsList)
						{
							linkedTree.Value.Dispose();
						}
					}
				}
			}
			UISystem.sceneLayers.Clear();
			UISystem.sceneLayers = null;
			if (UISystem.transitionCurrentSceneTextureCache != null)
			{
				UISystem.transitionCurrentSceneTextureCache.Dispose();
				UISystem.transitionCurrentSceneTextureCache = null;
			}
			if (UISystem.transitionNextSceneTextureCache != null)
			{
				UISystem.transitionNextSceneTextureCache.Dispose();
				UISystem.transitionNextSceneTextureCache = null;
			}
			ShaderProgramManager.Terminate(UISystem.GraphicsContext);
			if (UISystem.offScreenFramebufferCache != null)
			{
				UISystem.offScreenFramebufferCache.Dispose();
				UISystem.offScreenFramebufferCache = null;
			}
			if (UISystem.DefaultFont != null)
			{
				UISystem.DefaultFont.Dispose();
				UISystem.DefaultFont = null;
			}
			AssetManager.UnloadAllTexture();
			UISystem.zSortList = null;
			UISystem.effects = null;
			UISystem.addPendingEffects = null;
			UISystem.removePendingEffects = null;
			UISystem.capturingWidgets = null;
			UISystem.hitWidgets = null;
			UISystem.nextKeyLongPressTimes = null;
			UISystem.nextKeyRepeatTimes = null;
			UISystem.blankScene = null;
			UISystem.currentScene = null;
			UISystem.nextScene = null;
			UISystem.transitionUIElementScene = null;
			UISystem.navigationScene = null;
			UISystem.modalScene = null;
			UISystem.debugInfoScene = null;
			UISystem.GraphicsContext = null;
			UISystem.stopwatch.Stop();
			UISystem.stopwatch.Reset();
			UISystem.stopwatch = null;
			UISystem.hitTestPointOffsets = null;
			UISystem.scale = 1f;
			UISystem.Scaled = false;
		}

		public static void Update(List<TouchData> touchDataList)
		{
			UISystem.UpdateTime();
			UISystem.SendTouchDataList(touchDataList);
			UISystem.UpdateSystem();
		}

		public static void Update(List<TouchData> touchDataList, ref GamePadData gamePadData)
		{
			UISystem.UpdateTime();
			UISystem.SendTouchDataList(touchDataList);
			UISystem.SendGamePadData(ref gamePadData);
			UISystem.UpdateSystem();
		}

		public static void Update(List<TouchData> touchDataList, ref MotionData motionData)
		{
			UISystem.UpdateTime();
			UISystem.SendTouchDataList(touchDataList);
			UISystem.SendMotionData(ref motionData);
			UISystem.UpdateSystem();
		}

		public static void Update(List<TouchData> touchDataList, ref GamePadData gamePadData, ref MotionData motionData)
		{
			UISystem.UpdateTime();
			UISystem.SendTouchDataList(touchDataList);
			UISystem.SendGamePadData(ref gamePadData);
			UISystem.SendMotionData(ref motionData);
			UISystem.UpdateSystem();
		}

		private static void UpdateTime()
		{
			TimeSpan currentTime = UISystem.CurrentTime;
			UISystem.CurrentTime = UISystem.stopwatch.Elapsed;
			UISystem.elapsedTime = (float)(UISystem.CurrentTime - currentTime).TotalMilliseconds;
			if (UISystem.elapsedTime <= 0f)
			{
				UISystem.elapsedTime = 0.1f;
			}
		}

		private static void UpdateSystem()
		{
			UISystem.UpdateFrame();
			UISystem.UpdateEffect();
			UISystem.UpdateTransition();
		}

		private static void SendTouchDataList(List<TouchData> touchDataList)
		{
			if (UISystem.IsTransition || UISystem.IsDialogEffect)
			{
				for (int i = 0; i < touchDataList.Count; i++)
				{
					TouchData touchData = touchDataList[i];
					touchData.Skip = true;
					touchDataList[i] = touchData;
				}
				return;
			}
			UISystem.lowLevelTouchEvents.Clear();
			UISystem.highLevelTouchEvents.Clear();
			for (int j = 0; j < touchDataList.Count; j++)
			{
				TouchData touchData2 = touchDataList[j];
				float num = (touchData2.X + 0.5f) * (float)UISystem.FramebufferWidth;
				float num2 = (touchData2.Y + 0.5f) * (float)UISystem.FramebufferHeight;
				Vector2 vector = new Vector2(num, num2);
				int iD = touchData2.ID;
				Widget widget = UISystem.hitWidgets.ContainsKey(iD) ? UISystem.hitWidgets[iD] : null;
				Widget widget2 = UISystem.capturingWidgets.ContainsKey(iD) ? UISystem.capturingWidgets[iD] : null;
				Widget widget3 = UISystem.FindHitWidgetByNPoint(vector);
				switch (touchData2.Status)
				{
				case (TouchStatus)1:
					if (widget3 != null)
					{
						UISystem.hitWidgets[iD] = widget3;
						UISystem.capturingWidgets.Add(iD, widget3);
						UISystem.lowLevelTouchEvents.Add(UISystem.CreateTouchEvent(widget3, TouchEventType.Down, vector, iD));
						touchData2.Skip = true;
					}
					break;
				case (TouchStatus)2:
					UISystem.hitWidgets.Remove(iD);
					if (widget2 != null)
					{
						UISystem.capturingWidgets.Remove(iD);
						UISystem.lowLevelTouchEvents.Add(UISystem.CreateTouchEvent(widget2, TouchEventType.Up, vector, iD));
						touchData2.Skip = true;
					}
					else if (UISystem.IsModalScene)
					{
						Dialog dialog = UISystem.ModalScene.RootWidget.LinkedTree.LastChild.Value as Dialog;
						if (dialog != null && dialog.HideOnTouchOutside)
						{
							dialog.Hide();
						}
					}
					break;
				case (TouchStatus)3:
					if (widget3 != null)
					{
						UISystem.hitWidgets[iD] = widget3;
					}
					else
					{
						UISystem.hitWidgets.Remove(iD);
					}
					if (widget2 != null)
					{
						UISystem.lowLevelTouchEvents.Add(UISystem.CreateTouchEvent(widget2, TouchEventType.Move, vector, iD));
						if (widget3 != widget)
						{
							if (widget3 == widget2)
							{
								UISystem.highLevelTouchEvents.Add(UISystem.CreateTouchEvent(widget2, TouchEventType.Enter, vector, iD));
							}
							else if (widget == widget2)
							{
								UISystem.highLevelTouchEvents.Add(UISystem.CreateTouchEvent(widget2, TouchEventType.Leave, vector, iD));
							}
						}
						touchData2.Skip = true;
					}
					break;
				case (TouchStatus)4:
					UISystem.ResetStateAll();
					return;
				}
				if (UISystem.IsModalScene)
				{
					touchData2.Skip = true;
				}
				if (touchData2.Skip)
				{
					touchDataList[j] = touchData2;
				}
			}
			if (UISystem.lowLevelTouchEvents.Count > 0)
			{
				UISystem.CallTouchEventHandlers(UISystem.lowLevelTouchEvents, true);
			}
			if (UISystem.highLevelTouchEvents.Count > 0)
			{
				UISystem.CallTouchEventHandlers(UISystem.highLevelTouchEvents, false);
			}
		}

		public static Widget FindHitWidget(Vector2 screenPoint)
		{
			UISystem.UpdateSceneLayersOrder();
			foreach (Scene current in UISystem.sceneLayers.GetReverseOrder())
			{
				if (current != null && current.Visible)
				{
					Widget widget = UISystem.FindHitWidgetRecursively(current.RootWidget, screenPoint);
					if (widget != null || current.ConsumeAllTouchEvent)
					{
						return widget;
					}
				}
			}
			return null;
		}

		private static Widget FindHitWidgetByNPoint(Vector2 screenPoint)
		{
			UISystem.UpdateSceneLayersOrder();
			foreach (Scene current in UISystem.sceneLayers.GetReverseOrder())
			{
				if (current != null && current.Visible)
				{
					int num = UISystem.hitTestPointOffsets.Length;
					Widget[] array = new Widget[num];
					int[] array2 = new int[num];
					for (int i = 0; i < num; i++)
					{
						Vector2 touchPosition = screenPoint + UISystem.hitTestPointOffsets[i];
						Widget widget = UISystem.FindHitWidgetRecursively(current.RootWidget, touchPosition);
						if (widget != null)
						{
							array[i] = widget;
							for (int j = 0; j <= i; j++)
							{
								if (array[j] == widget)
								{
									array2[j]++;
									break;
								}
							}
						}
					}
					Widget widget2 = null;
					int num2 = 0;
					for (int k = 0; k < num; k++)
					{
						if (array[k] != null && array[k].PriorityHit && num2 < array2[k])
						{
							widget2 = array[k];
							num2 = array2[k];
						}
					}
					if (widget2 == null)
					{
						for (int l = 0; l < num; l++)
						{
							if (array[l] != null && num2 < array2[l])
							{
								widget2 = array[l];
								num2 = array2[l];
							}
						}
					}
					if (UISystem.showHitTestDebugInfo)
					{
						for (int m = 0; m < num; m++)
						{
							Vector2 vector = screenPoint + UISystem.hitTestPointOffsets[m];
							UISystem.hitTestPointSprts[m].GetUnit(0).X = vector.X;
							UISystem.hitTestPointSprts[m].GetUnit(0).Y = vector.Y;
							if (array[m] == widget2)
							{
								UISystem.hitTestPointSprts[m].GetUnit(0).Color = new UIColor(1f, 0.3f, 0.3f, 1f);
							}
							else
							{
								UISystem.hitTestPointSprts[m].GetUnit(0).Color = new UIColor(1f, 1f, 0f, 1f);
							}
						}
					}
					if (widget2 != null || current.ConsumeAllTouchEvent)
					{
						return widget2;
					}
				}
			}
			return null;
		}

		private static Widget FindHitWidgetRecursively(Widget widget, Vector2 touchPosition)
		{
			if (!widget.TouchResponse || !widget.Visible)
			{
				return null;
			}
			for (LinkedTree<Widget> linkedTree = widget.LinkedTree.LastChild; linkedTree != null; linkedTree = linkedTree.PreviousSibling)
			{
				Widget widget2 = UISystem.FindHitWidgetRecursively(linkedTree.Value, touchPosition);
				if (widget2 != null)
				{
					return widget2;
				}
			}
			if (!widget.HitTest(touchPosition))
			{
				return null;
			}
			return widget;
		}

		private static TouchEvent CreateTouchEvent(Widget widget, TouchEventType eventType, Vector2 worldTouchPosition, int id)
		{
			return new TouchEvent
			{
				Time = UISystem.CurrentTime,
				Type = eventType,
				WorldPosition = worldTouchPosition,
				FingerID = id,
				Source = widget
			};
		}

		private static void CallTouchEventHandlers(TouchEventCollection allTouchEvents, bool isUpdatePrimaryTouchEvent)
		{
			Stack<KeyValuePair<Widget, TouchEventCollection>> stack = new Stack<KeyValuePair<Widget, TouchEventCollection>>();
			stack.Push(new KeyValuePair<Widget, TouchEventCollection>(null, allTouchEvents));
			while (stack.Count > 0)
			{
				KeyValuePair<Widget, TouchEventCollection> keyValuePair = stack.Pop();
				Widget key = keyValuePair.Key;
				TouchEventCollection value = keyValuePair.Value;
				UISystem.CreateTouchEventSets(value, key, isUpdatePrimaryTouchEvent);
				using (Dictionary<Widget, TouchEventCollection>.Enumerator enumerator = UISystem.touchEventSets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Widget, TouchEventCollection> current = enumerator.Current;
						if (UISystem.IsTransition || UISystem.IsDialogEffect)
						{
							return;
						}
						Widget key2 = current.Key;
						TouchEventCollection value2 = current.Value;
						key2.OnTouchEvent(value2);
						using (List<GestureDetector>.Enumerator enumerator2 = key2.GestureDetectors.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GestureDetector current2 = enumerator2.Current;
								if (current2.State == GestureDetectorResponse.DetectedAndStop || current2.State == GestureDetectorResponse.FailedAndStop)
								{
									current2.OnResetState();
									current2.State = GestureDetectorResponse.None;
								}
							}
						}
						if (value2.Forward && value2.Count > 0)
						{
							stack.Push(current);
						}
					}
				}
			}
		}

		private static void CreateTouchEventSets(TouchEventCollection touchEvents, Widget deliveredWidget, bool isUpdatePrimaryTouchEvent)
		{
			UISystem.touchEventSets.Clear();
			foreach (TouchEvent touchEvent in touchEvents)
			{
				if (touchEvent.Source != deliveredWidget && touchEvent.Source != null)
				{
					Widget widget = touchEvent.Source;
					Widget parent = widget.Parent;
					while (parent != deliveredWidget && parent != null)
					{
						if (parent.HookChildTouchEvent)
						{
							widget = parent;
						}
						parent = parent.Parent;
					}
					touchEvent.LocalPosition = widget.ConvertScreenToLocal(touchEvent.WorldPosition);
					if (UISystem.touchEventSets.ContainsKey(widget))
					{
						UISystem.touchEventSets[widget].Add(touchEvent);
					}
					else
					{
						TouchEventCollection touchEventCollection = new TouchEventCollection();
						touchEventCollection.Add(touchEvent);
						UISystem.touchEventSets.Add(widget, touchEventCollection);
					}
				}
			}
			UISystem.SetPrimaryTouchEvent(isUpdatePrimaryTouchEvent);
		}

		private static void SetPrimaryTouchEvent(bool isUpdatePrimaryTouchEvent)
		{
			foreach (KeyValuePair<Widget, TouchEventCollection> current in UISystem.touchEventSets)
			{
				Widget key = current.Key;
				TouchEventCollection value = current.Value;
				foreach (TouchEvent touchEvent in value)
				{
					if (value.PrimaryTouchEvent != null)
					{
						break;
					}
					if (UISystem.primaryIDs.ContainsKey(key))
					{
						if (UISystem.primaryIDs[key] == touchEvent.FingerID)
						{
							value.PrimaryTouchEvent = touchEvent;
							if (isUpdatePrimaryTouchEvent && touchEvent.Type == TouchEventType.Up)
							{
								UISystem.primaryIDs.Remove(key);
							}
						}
					}
					else if (touchEvent.Type == TouchEventType.Down)
					{
						value.PrimaryTouchEvent = touchEvent;
						if (isUpdatePrimaryTouchEvent)
						{
							UISystem.primaryIDs.Add(key, touchEvent.FingerID);
						}
					}
				}
				if (value.PrimaryTouchEvent == null)
				{
					if (isUpdatePrimaryTouchEvent && UISystem.primaryIDs.ContainsKey(key))
					{
						UISystem.primaryIDs.Remove(key);
					}
					value.PrimaryTouchEvent = new TouchEvent
					{
						FingerID = -1,
						Type = TouchEventType.None,
						LocalPosition = Vector2.Zero,
						WorldPosition = Vector2.Zero,
						Time = value[0].Time,
						Source = null
					};
				}
			}
		}

		private static void SendGamePadData(ref GamePadData gamePadData)
		{
			UISystem.GamePadData = gamePadData;
			if (UISystem.KeyReceiverWidget != null)
			{
				List<KeyEvent> list = UISystem.GamePadDataToKeyEventList(gamePadData);
				using (List<KeyEvent>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyEvent current = enumerator.Current;
						UISystem.CallKeyEventHandlerRecursively(UISystem.KeyReceiverWidget, current);
					}
				}
				gamePadData.Skip = true;
				return;
			}
			if (UISystem.IsTransition || UISystem.IsDialogEffect || UISystem.IsModalScene)
			{
				gamePadData.Skip = true;
			}
		}

		private static List<KeyEvent> GamePadDataToKeyEventList(GamePadData gamePadData)
		{
			List<KeyEvent> list = new List<KeyEvent>();
			if (gamePadData.ButtonsPrev != (GamePadButtons)0u)
			{
				List<KeyType> list2 = UISystem.GamePadButtonsToKeyTypeList(gamePadData.ButtonsPrev & gamePadData.ButtonsUp);
				using (List<KeyType>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyType current = enumerator.Current;
						list.Add(new KeyEvent
						{
							KeyType = current,
							KeyEventType = KeyEventType.Up,
							Time = UISystem.CurrentTime
						});
						UISystem.nextKeyLongPressTimes.Remove(current);
						UISystem.nextKeyRepeatTimes.Remove(current);
					}
				}
			}
			if (gamePadData.Buttons != (GamePadButtons)0u)
			{
				List<KeyType> list3 = UISystem.GamePadButtonsToKeyTypeList(gamePadData.Buttons & gamePadData.ButtonsDown);
				using (List<KeyType>.Enumerator enumerator2 = list3.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyType current2 = enumerator2.Current;
						KeyEvent keyEvent = new KeyEvent();
						keyEvent.KeyType = current2;
						keyEvent.KeyEventType = KeyEventType.Down;
						keyEvent.Time = UISystem.CurrentTime;
						list.Add(keyEvent);
						UISystem.nextKeyLongPressTimes.Add(current2, keyEvent.Time + UISystem.keyLongPressTime);
						UISystem.nextKeyRepeatTimes.Add(current2, keyEvent.Time + UISystem.keyRepeatTime);
					}
				}
				list3 = UISystem.GamePadButtonsToKeyTypeList(gamePadData.Buttons & ~gamePadData.ButtonsDown);
				using (List<KeyType>.Enumerator enumerator3 = list3.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyType current3 = enumerator3.Current;
						KeyEvent keyEvent2 = new KeyEvent();
						keyEvent2.KeyType = current3;
						keyEvent2.KeyEventType = KeyEventType.Down;
						keyEvent2.Time = UISystem.CurrentTime;
						list.Add(keyEvent2);
						if (keyEvent2.Time >= UISystem.nextKeyLongPressTimes[current3])
						{
							keyEvent2 = new KeyEvent();
							keyEvent2.KeyType = current3;
							keyEvent2.KeyEventType = KeyEventType.LongPress;
							keyEvent2.Time = UISystem.CurrentTime;
							list.Add(keyEvent2);
						}
						if (keyEvent2.Time >= UISystem.nextKeyRepeatTimes[current3])
						{
							list.Add(new KeyEvent
							{
								KeyType = current3,
								KeyEventType = KeyEventType.Repeat,
								Time = UISystem.CurrentTime
							});
							Dictionary<KeyType, TimeSpan> dictionary;
							KeyType keyType;
							(dictionary = UISystem.nextKeyRepeatTimes)[keyType = current3] = dictionary[keyType] + UISystem.keyRepeatIntervalTime;
						}
					}
				}
			}
			return list;
		}

		private static List<KeyType> GamePadButtonsToKeyTypeList(GamePadButtons buttons)
		{
			Dictionary<GamePadButtons, KeyType> dictionary = new Dictionary<GamePadButtons, KeyType>();
			dictionary.Add((GamePadButtons)1, KeyType.Left);
			dictionary.Add((GamePadButtons)2, KeyType.Up);
			dictionary.Add((GamePadButtons)4, KeyType.Right);
			dictionary.Add((GamePadButtons)8, KeyType.Down);
			dictionary.Add((GamePadButtons)16, KeyType.Square);
			dictionary.Add((GamePadButtons)32, KeyType.Triangle);
			dictionary.Add((GamePadButtons)64, KeyType.Circle);
			dictionary.Add((GamePadButtons)128, KeyType.Cross);
			dictionary.Add((GamePadButtons)256, KeyType.Start);
			dictionary.Add((GamePadButtons)512, KeyType.Select);
			dictionary.Add((GamePadButtons)1024, KeyType.L);
			dictionary.Add((GamePadButtons)2048, KeyType.R);
			Dictionary<GamePadButtons, KeyType> dictionary2 = dictionary;
			List<KeyType> list = new List<KeyType>();
			using (Dictionary<GamePadButtons, KeyType>.KeyCollection.Enumerator enumerator = dictionary2.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GamePadButtons current = enumerator.Current;
					if ((buttons & current) != 0)
					{
						list.Add(dictionary2[current]);
					}
				}
			}
			return list;
		}

		private static void CallKeyEventHandlerRecursively(Widget widget, KeyEvent keyEvent)
		{
			Widget localKeyReceiverWidget = widget.LocalKeyReceiverWidget;
			if (localKeyReceiverWidget != null)
			{
				UISystem.CallKeyEventHandlerRecursively(localKeyReceiverWidget, keyEvent);
				if (keyEvent.Forward)
				{
					keyEvent.Forward = false;
					widget.OnKeyEvent(keyEvent);
					return;
				}
			}
			else
			{
				widget.OnKeyEvent(keyEvent);
			}
		}

		private static void SendMotionData(ref MotionData motionData)
		{
			UISystem.MotionData = motionData;
			UISystem.CallMotionEventHandler(new MotionEvent
			{
				Acceleration = motionData.Acceleration,
				AngularVelocity = motionData.AngularVelocity,
				Time = UISystem.CurrentTime
			});
		}

		private static void CallMotionEventHandler(MotionEvent motionEvent)
		{
			Widget rootWidget = UISystem.CurrentScene.RootWidget;
			LinkedTree<Widget> nextAsList = rootWidget.LinkedTree.LastDescendant.NextAsList;
			for (LinkedTree<Widget> linkedTree = rootWidget.LinkedTree; linkedTree != nextAsList; linkedTree = linkedTree.NextAsList)
			{
				Widget value = linkedTree.Value;
				if (value != null)
				{
					value.OnMotionEvent(motionEvent);
				}
			}
		}

		private static void UpdateFrame()
		{
			UISystem.UpdateSceneLayersOrder();
			using (List<Scene>.Enumerator enumerator = UISystem.sceneLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scene current = enumerator.Current;
					if (current != null)
					{
						current.Update(UISystem.elapsedTime);
					}
				}
			}
		}

		public static void Render()
		{
			UISystem.SetupGraphics();
			UISystem.UpdateSceneLayersOrder();
			using (List<Scene>.Enumerator enumerator = UISystem.sceneLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scene current = enumerator.Current;
					if (current != null)
					{
						UISystem.Render(current.RootWidget);
					}
				}
			}
			UISystem.RestoreGraphics(); //FIXME:something bad
	    }

		internal static void Render(Widget widget)
		{
			UISystem.RenderWidgets(widget);
			UISystem.RenderZSortList();
		}

		private static void SetupGraphics()
		{
			UISystem.StoredGraphicsState.EnableModeBlend = UISystem.GraphicsContext.IsEnabled(EnableMode.Blend);//(EnableMode)4);
			UISystem.StoredGraphicsState.EnableModeCullFace = UISystem.GraphicsContext.IsEnabled(EnableMode.CullFace);//(EnableMode)2);
			UISystem.StoredGraphicsState.EnableModeDepthTest = UISystem.GraphicsContext.IsEnabled(EnableMode.DepthTest);//(EnableMode)8);
			UISystem.StoredGraphicsState.EnableModeScissorTest = UISystem.GraphicsContext.IsEnabled(EnableMode.ScissorTest);//(EnableMode)1);
			UISystem.StoredGraphicsState.EnableModeStencilTest = UISystem.GraphicsContext.IsEnabled(EnableMode.StencilTest);//(EnableMode)32);
			UISystem.StoredGraphicsState.BlendFuncRgb = UISystem.GraphicsContext.GetBlendFuncRgb();
			UISystem.StoredGraphicsState.BlendFuncAlpha = UISystem.GraphicsContext.GetBlendFuncAlpha();
			UISystem.StoredGraphicsState.ClearColor = UISystem.GraphicsContext.GetClearColor();
			UISystem.StoredGraphicsState.ColorMask = UISystem.GraphicsContext.GetColorMask();
			UISystem.StoredGraphicsState.CullFace = UISystem.GraphicsContext.GetCullFace();
			UISystem.GraphicsContext.Enable((EnableMode)8, false);
			UISystem.GraphicsContext.Enable((EnableMode)1, true);
			UISystem.GraphicsContext.Enable(EnableMode.StencilTest/*(EnableMode)32*/, false);
			if (UISystem.StoredGraphicsState.ColorMask != (ColorMask)7)
			{
				UISystem.GraphicsContext.SetColorMask((ColorMask)7);
			}
			FrameBuffer frameBuffer = UISystem.GraphicsContext.GetFrameBuffer();
			if (!frameBuffer.Status || frameBuffer.GetColorTarget().Buffer != null)
			{
				UISystem.GraphicsContext.SetFrameBuffer(null);
			}
			UISystem.SetClipRegionFull();
			ImageRect viewport = UISystem.GraphicsContext.GetViewport();
			ImageRect viewport2 = new ImageRect(0, 0, UISystem.GraphicsContext.Screen.Width, UISystem.GraphicsContext.Screen.Height);
			if (!viewport2.Equals(viewport))
			{
				UISystem.GraphicsContext.SetViewport(viewport2);
			}
		}

		//FIXME:not work very well!!!!
		private static void RestoreGraphics()
		{
			UISystem.GraphicsContext.Enable((EnableMode)4u, UISystem.StoredGraphicsState.EnableModeBlend);
			UISystem.GraphicsContext.Enable((EnableMode)2u, UISystem.StoredGraphicsState.EnableModeCullFace);
			UISystem.GraphicsContext.Enable((EnableMode)8u, UISystem.StoredGraphicsState.EnableModeDepthTest);
			UISystem.GraphicsContext.Enable((EnableMode)1u, UISystem.StoredGraphicsState.EnableModeScissorTest);
			UISystem.GraphicsContext.Enable((EnableMode)32u, UISystem.StoredGraphicsState.EnableModeStencilTest);
			if (!UISystem.StoredGraphicsState.BlendFuncRgb.Equals(UISystem.GraphicsContext.GetBlendFuncRgb()))
			{
				UISystem.GraphicsContext.SetBlendFuncRgb(UISystem.StoredGraphicsState.BlendFuncRgb);
			}
			if (!UISystem.StoredGraphicsState.BlendFuncAlpha.Equals(UISystem.GraphicsContext.GetBlendFuncAlpha()))
			{
				UISystem.GraphicsContext.SetBlendFuncAlpha(UISystem.StoredGraphicsState.BlendFuncAlpha);
			}
			if (!UISystem.StoredGraphicsState.ClearColor.Equals(UISystem.GraphicsContext.GetClearColor()))
			{
				UISystem.GraphicsContext.SetClearColor(UISystem.StoredGraphicsState.ClearColor);
			}
			//FIXME:not very well--->???
			if (__USE_COLORMASK)
			{
				if (!UISystem.StoredGraphicsState.ColorMask.Equals(UISystem.GraphicsContext.GetColorMask()))
				{
					UISystem.GraphicsContext.SetColorMask(UISystem.StoredGraphicsState.ColorMask);
				}
			}
			if (!UISystem.StoredGraphicsState.CullFace.Equals(UISystem.GraphicsContext.GetCullFace()))
			{
				UISystem.GraphicsContext.SetCullFace(UISystem.StoredGraphicsState.CullFace);
			}
			UISystem.SetClipRegionFull();
		}

		internal static void RenderWidgets(Widget rootWidget)
		{
			LinkedTree<Widget> nextAsList = rootWidget.LinkedTree.LastDescendant.NextAsList;
			for (LinkedTree<Widget> linkedTree = rootWidget.LinkedTree; linkedTree != nextAsList; linkedTree = linkedTree.NextAsList)
			{
				Widget value = linkedTree.Value;
				if (value.Visible)
				{
					for (LinkedTree<UIElement> linkedTree2 = value.RootUIElement.linkedTree; linkedTree2 != null; linkedTree2 = linkedTree2.NextAsList)
					{
						UIElement value2 = linkedTree2.Value;
						if (value2.Visible)
						{
							value2.SetupFinalAlpha();
						}
						else
						{
							linkedTree2 = linkedTree2.LastDescendant;
						}
					}
				}
				else
				{
					linkedTree = linkedTree.LastDescendant;
				}
			}
			UISystem.zSortList = null;
			nextAsList = rootWidget.LinkedTree.LastDescendant.NextAsList;
			for (LinkedTree<Widget> linkedTree3 = rootWidget.LinkedTree; linkedTree3 != nextAsList; linkedTree3 = linkedTree3.NextAsList)
			{
				Widget value3 = linkedTree3.Value;
				if (value3.Visible && value3.SetupClipArea(value3 == rootWidget))
				{
					value3.AddZSortUIElements(ref UISystem.zSortList);
					if (value3.ZSort)
					{
						linkedTree3 = linkedTree3.LastDescendant;
					}
					else
					{
						value3.SetupFinalAlpha();
						value3.Render();
					}
				}
				else
				{
					linkedTree3 = linkedTree3.LastDescendant;
				}
			}
		}

		internal static void RenderZSortList()
		{
			UISystem.SetClipRegionFull();
			if (UISystem.zSortList != null)
			{
				UISystem.zSortList = UISystem.SortDrawSequence(UISystem.zSortList, null, null);
			}
			for (UIElement nextZSortElement = UISystem.zSortList; nextZSortElement != null; nextZSortElement = nextZSortElement.nextZSortElement)
			{
				if (nextZSortElement.Parent == null)
				{
					Widget parentWidget = ((RootUIElement)nextZSortElement).parentWidget;
					parentWidget.ZSort = false;
					parentWidget.Render();
					parentWidget.ZSort = true;
					LinkedTree<Widget> nextAsList = parentWidget.LinkedTree.LastDescendant.NextAsList;
					for (LinkedTree<Widget> nextAsList2 = parentWidget.LinkedTree.NextAsList; nextAsList2 != nextAsList; nextAsList2 = nextAsList2.NextAsList)
					{
						Widget value = nextAsList2.Value;
						value.Render();
					}
				}
				else
				{
					nextZSortElement.SetupFinalAlpha();
					nextZSortElement.SetupDrawState();
					nextZSortElement.Render();
					LinkedTree<UIElement> nextAsList3 = nextZSortElement.linkedTree.LastDescendant.NextAsList;
					for (LinkedTree<UIElement> linkedTree = nextZSortElement.linkedTree.NextAsList; linkedTree != nextAsList3; linkedTree = linkedTree.NextAsList)
					{
						UIElement value2 = linkedTree.Value;
						if (value2.ZSort)
						{
							linkedTree = linkedTree.LastDescendant;
						}
						else
						{
							value2.SetupFinalAlpha();
							value2.SetupDrawState();
							value2.Render();
						}
					}
				}
			}
		}

		private static UIElement SortDrawSequence(UIElement targetList, UIElement prevTargetList, UIElement nextTargetList)
		{
			UIElement uIElement = targetList;
			targetList = targetList.nextZSortElement;
			UIElement uIElement2 = null;
			UIElement uIElement3 = null;
			UIElement nextZSortElement;
			for (UIElement uIElement4 = targetList; uIElement4 != nextTargetList; uIElement4 = nextZSortElement)
			{
				nextZSortElement = uIElement4.nextZSortElement;
				if (uIElement4.zSortValue >= uIElement.zSortValue)
				{
					uIElement4.nextZSortElement = ((uIElement2 != null) ? uIElement2 : uIElement);
					uIElement2 = uIElement4;
				}
				else
				{
					uIElement4.nextZSortElement = ((uIElement3 != null) ? uIElement3 : nextTargetList);
					uIElement3 = uIElement4;
				}
			}
			if (uIElement2 != null)
			{
				if (prevTargetList != null)
				{
					prevTargetList.nextZSortElement = uIElement2;
				}
				targetList = UISystem.SortDrawSequence(uIElement2, prevTargetList, uIElement);
			}
			else
			{
				if (prevTargetList != null)
				{
					prevTargetList.nextZSortElement = uIElement;
				}
				targetList = uIElement;
			}
			if (uIElement3 != null)
			{
				uIElement.nextZSortElement = uIElement3;
				UISystem.SortDrawSequence(uIElement3, uIElement, nextTargetList);
			}
			else
			{
				uIElement.nextZSortElement = nextTargetList;
			}
			return targetList;
		}

		public static void SetScene(Scene newScene)
		{
			Transition transition = (newScene == null) ? null : newScene.Transition;
			UISystem.SetScene(newScene, transition);
		}

		public static void SetScene(Scene newScene, Transition transition)
		{
			UISystem.navigationSceneStack.Clear();
			UISystem.SetSceneInternal(newScene, transition);
		}

		public static void PushScene(Scene newScene)
		{
			UISystem.navigationSceneStack.Push(UISystem.CurrentScene);
			UISystem.SetSceneInternal(newScene, UISystem.ScenePushTransition, true);
			UISystem.NavigationScene.StartPushAnimation((0 < UISystem.navigationSceneStack.Count) ? UISystem.navigationSceneStack.Peek().Title : null, newScene.Title);
		}

		public static void PopScene()
		{
			if (0 < UISystem.navigationSceneStack.Count)
			{
				Scene scene = UISystem.navigationSceneStack.Pop();
				UISystem.SetSceneInternal(scene, UISystem.ScenePopTransition, true);
				UISystem.NavigationScene.StartPopAnimation((0 < UISystem.navigationSceneStack.Count) ? UISystem.navigationSceneStack.Peek().Title : null, scene.Title);
			}
		}

		internal static void SetSceneInternal(Scene newScene, Transition transition)
		{
			UISystem.SetSceneInternal(newScene, transition, false);
		}

		internal static void SetSceneInternal(Scene newScene, Transition transition, bool withNavigationAnimation)
		{
			if (newScene == null)
			{
				newScene = UISystem.blankScene;
			}
			UISystem.CurrentScene.OnHiding();
			newScene.OnShowing();
			UISystem.currentTransition = transition;
			if (UISystem.currentTransition == null)
			{
				UISystem.SetSceneInternal(newScene);
			}
			else
			{
				UISystem.NextScene = newScene;
				UISystem.currentTransition.Start();
				UISystem.currentTransition.startedTime = UISystem.stopwatch.Elapsed;
			}
			if (!newScene.ShowNavigationBar)
			{
				UISystem.NavigationScene.Hide(true);
				return;
			}
			if (withNavigationAnimation)
			{
				UISystem.NavigationScene.Show(true, null);
				return;
			}
			UISystem.NavigationScene.Show(true, newScene.Title);
		}

		internal static void SetSceneInternal(Scene newScene)
		{
			UISystem.ResetStateAll();
			UISystem.CurrentScene.OnHidden();
			newScene.OnShown();
			UISystem.CurrentScene = newScene;
			UISystem.NextScene = null;
			UISystem.currentTransition = null;
		}

		private static void UpdateSceneLayersOrder()
		{
			if (!UISystem.needUpdateSceneLayersOrder)
			{
				return;
			}
			if (UISystem.sceneLayers == null)
			{
				UISystem.sceneLayers = new UISystem.ReversibleList<Scene>();
			}
			else
			{
				UISystem.sceneLayers.Clear();
			}
			switch (UISystem.transitionDrawOrder)
			{
			case TransitionDrawOrder.CurrentScene:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			case TransitionDrawOrder.NextScene:
				UISystem.sceneLayers.Add(UISystem.NextScene);
				break;
			case TransitionDrawOrder.TransitionUIElement:
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				break;
			case TransitionDrawOrder.CS_NS:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				break;
			case TransitionDrawOrder.CS_TE:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				break;
			case TransitionDrawOrder.NS_CS:
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			case TransitionDrawOrder.NS_TE:
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				break;
			case TransitionDrawOrder.TE_CS:
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			case TransitionDrawOrder.TE_NS:
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				break;
			case TransitionDrawOrder.CS_NS_TE:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				break;
			case TransitionDrawOrder.CS_TE_NS:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				break;
			case TransitionDrawOrder.NS_CS_TE:
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				break;
			case TransitionDrawOrder.NS_TE_CS:
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			case TransitionDrawOrder.TE_CS_NS:
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				break;
			case TransitionDrawOrder.TE_NS_CS:
				UISystem.sceneLayers.Add(UISystem.TransitionUIElementScene);
				UISystem.sceneLayers.Add(UISystem.NextScene);
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			default:
				UISystem.sceneLayers.Add(UISystem.CurrentScene);
				break;
			}
			UISystem.sceneLayers.Add(UISystem.NavigationScene);
			UISystem.sceneLayers.Add(UISystem.ModalScene);
			UISystem.sceneLayers.Add(UISystem.DebugInfoScene);
			UISystem.needUpdateSceneLayersOrder = false;
		}

		public static ShaderProgram GetShaderProgram(ShaderType type)
		{
			return ShaderProgramManager.GetShaderProgram((InternalShaderType)type);
		}

		internal static int GetUniformLocation(ShaderType type, string name)
		{
			return ShaderProgramManager.GetUniforms((InternalShaderType)type)[name];
		}

		internal static void PushModalWidget(Widget modalWidget)
		{
			if (UISystem.ModalScene == null)
			{
				UISystem.ModalScene = new Scene();
			}
			UISystem.ModalScene.RootWidget.AddChildLast(modalWidget);
			UISystem.ResetStateAll();
		}

		internal static Widget PopModalWidget()
		{
			if (UISystem.ModalScene == null)
			{
				return null;
			}
			UISystem.ResetStateAll();
			LinkedTree<Widget> linkedTree = UISystem.ModalScene.RootWidget.LinkedTree;
			if (linkedTree.LastChild == null)
			{
				UISystem.ModalScene = null;
				return null;
			}
			Widget value = linkedTree.LastChild.Value;
			UISystem.ModalScene.RootWidget.RemoveChild(value);
			if (linkedTree.FirstChild == null)
			{
				UISystem.ModalScene = null;
			}
			return value;
		}

		internal static void RegisterEffect(Effect effect)
		{
			if (UISystem.effectUpdating)
			{
				UISystem.addPendingEffects.Add(effect);
				return;
			}
			UISystem.effects.Add(effect);
		}

		internal static void UnregisterEffect(Effect effect)
		{
			if (UISystem.effectUpdating)
			{
				UISystem.removePendingEffects.Add(effect);
				return;
			}
			UISystem.effects.Remove(effect);
		}

		private static void UpdateEffect()
		{
			UISystem.effectUpdating = true;
			using (List<Effect>.Enumerator enumerator = UISystem.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Effect current = enumerator.Current;
					current.Update(UISystem.elapsedTime);
				}
			}
			UISystem.effectUpdating = false;
			using (List<Effect>.Enumerator enumerator2 = UISystem.addPendingEffects.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Effect current2 = enumerator2.Current;
					UISystem.effects.Add(current2);
				}
			}
			UISystem.addPendingEffects.Clear();
			using (List<Effect>.Enumerator enumerator3 = UISystem.removePendingEffects.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Effect current3 = enumerator3.Current;
					UISystem.effects.Remove(current3);
				}
			}
			UISystem.removePendingEffects.Clear();
		}

		private static void UpdateTransition()
		{
			if (UISystem.IsTransition)
			{
				UISystem.currentTransition.Update(UISystem.elapsedTime);
			}
		}

		public static void ResetStateAll()
		{
			if (UISystem.CurrentScene != null)
			{
				UISystem.CurrentScene.RootWidget.ResetState(true);
			}
			if (UISystem.ModalScene != null)
			{
				UISystem.ModalScene.RootWidget.ResetState(true);
			}
			UISystem.capturingWidgets.Clear();
			UISystem.hitWidgets.Clear();
			UISystem.primaryIDs.Clear();
		}

		internal static void ClearBuffer()
		{
			if (UISystem.transitionCurrentSceneTextureCache != null)
			{
				UISystem.transitionCurrentSceneTextureCache.Dispose();
				UISystem.transitionCurrentSceneTextureCache = null;
			}
			if (UISystem.transitionNextSceneTextureCache != null)
			{
				UISystem.transitionNextSceneTextureCache.Dispose();
				UISystem.transitionNextSceneTextureCache = null;
			}
		}

		internal static bool CheckTextureSizeCapacity(int width, int height)
		{
			return width <= UISystem.GraphicsContext.Caps.MaxTextureSize && height <= UISystem.GraphicsContext.Caps.MaxTextureSize;
		}

		internal static void SetClipRegion(int x, int y, int w, int h)
		{
			if (w <= 0 || h <= 0)
			{
				return;
			}
			FrameBuffer frameBuffer = UISystem.GraphicsContext.GetFrameBuffer();
			ImageRect scissor = UISystem.GraphicsContext.GetScissor();
			ImageRect scissor2;
			if (UISystem.Scaled)
			{
				scissor2 = new ImageRect((int)((float)x * UISystem.Scale), frameBuffer.Height - (int)((float)(y + h) * UISystem.Scale), (int)((float)w * UISystem.Scale), (int)((float)h * UISystem.Scale));
			}
			else
			{
				scissor2 = new ImageRect(x, frameBuffer.Height - y - h, w, h);
			}
			if (!scissor2.Equals(scissor))
			{
				UISystem.GraphicsContext.SetScissor(scissor2);
			}
		}

		internal static void SetClipRegionFull()
		{
			FrameBuffer frameBuffer = UISystem.GraphicsContext.GetFrameBuffer();
			ImageRect scissor = UISystem.GraphicsContext.GetScissor();
			ImageRect scissor2 = new ImageRect(0, 0, frameBuffer.Width, frameBuffer.Height);
			if (!scissor2.Equals(scissor))
			{
				UISystem.GraphicsContext.SetScissor(scissor2);
			}
		}
	}
}
