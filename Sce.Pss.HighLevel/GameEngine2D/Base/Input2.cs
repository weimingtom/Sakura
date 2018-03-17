using Sce.Pss.Core;
using Sce.Pss.Core.Input;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public static class Input2
	{
		public struct ButtonState
		{
			internal byte m_data;

			internal static Input2.ButtonState Default = new Input2.ButtonState
			{
				m_data = 0
			};

			public bool Down
			{
				get
				{
					return (this.m_data & 1) != 0;
				}
			}

			public bool Press
			{
				get
				{
					return (this.m_data & 2) != 0;
				}
			}

			public bool On
			{
				get
				{
					return (this.m_data & 4) != 0;
				}
			}

			public bool Release
			{
				get
				{
					return (this.m_data & 8) != 0;
				}
			}

			internal void frame_update(bool down)
			{
				byte b = down ? (byte)1 : (byte)0;
				if (!this.Down && down)
				{
					b |= 2;
				}
				if (this.Down && down)
				{
					b |= 4;
				}
				if (this.Down && !down)
				{
					b |= 8;
				}
				this.m_data = b;
			}
		}

		public class TouchData
		{
			private Input2.ButtonState m_state;

			private Vector2 m_pos;

			private Vector2 m_pos_prev;

			internal bool m_visited;

			internal int m_id;

			public Vector2 Pos
			{
				get
				{
					return this.m_pos;
				}
			}

			public Vector2 PreviousPos
			{
				get
				{
					return this.m_pos_prev;
				}
			}

			public bool Down
			{
				get
				{
					return this.m_state.Down;
				}
			}

			public bool Press
			{
				get
				{
					return this.m_state.Press;
				}
			}

			public bool On
			{
				get
				{
					return this.m_state.On;
				}
			}

			public bool Release
			{
				get
				{
					return this.m_state.Release;
				}
			}

			public TouchData()
			{
				this.m_state = Input2.ButtonState.Default;
				this.m_pos = Math._00;
				this.m_pos_prev = Math._00;
				this.m_visited = false;
				this.m_id = -1;
			}

			internal void frame_update(Vector2 pos, bool down)
			{
				this.m_state.frame_update(down);
				this.m_pos_prev = this.m_pos;
				this.m_pos = pos;
				if (this.Press)
				{
					this.m_pos_prev = this.m_pos;
				}
			}
		}

		internal class TouchDataArray
		{
			internal Input2.TouchData[] m_touch_data;

			private int m_device_index;

			private bool m_external_control = false;

			private List<Sce.Pss.Core.Input.TouchData> m_external_data;

			private List<int> m_id_set;

			private int Capacity
			{
				get
				{
					return 10;
				}
			}

			internal TouchDataArray(int device_index)
			{
				this.m_device_index = device_index;
				this.m_touch_data = new Input2.TouchData[this.Capacity];
				for (int i = 0; i < this.m_touch_data.Length; i++)
				{
					this.m_touch_data[i] = new Input2.TouchData();
				}
				this.m_id_set = new List<int>();
			}

			private Input2.TouchData get_touch_data(int id)
			{
				Input2.TouchData[] touch_data = this.m_touch_data;
				Input2.TouchData result;
				for (int i = 0; i < touch_data.Length; i++)
				{
					Input2.TouchData touchData = touch_data[i];
					if (touchData.m_id == id)
					{
						result = touchData;
						return result;
					}
				}
				touch_data = this.m_touch_data;
				for (int i = 0; i < touch_data.Length; i++)
				{
					Input2.TouchData touchData = touch_data[i];
					if (touchData.m_id == -1)
					{
						touchData.m_id = id;
						result = touchData;
						return result;
					}
				}
				result = null;
				return result;
			}

			public void SetData(List<Sce.Pss.Core.Input.TouchData> data)
			{
				this.m_external_control = true;
				this.m_external_data = data;
			}

			internal void frame_update()
			{
				List<Sce.Pss.Core.Input.TouchData> list = this.m_external_data;
				if (!this.m_external_control)
				{
					list = Sce.Pss.Core.Input.Touch.GetData(this.m_device_index);
				}
				Common.Assert(list != null);
				Input2.TouchData[] touch_data = this.m_touch_data;
				for (int i = 0; i < touch_data.Length; i++)
				{
					Input2.TouchData touchData = touch_data[i];
					touchData.m_visited = false;
				}
				this.m_id_set.Clear();
				foreach (Sce.Pss.Core.Input.TouchData current in list)
				{
					this.m_id_set.Add(current.ID);
				}
				foreach (Sce.Pss.Core.Input.TouchData current in list)
				{
					Input2.TouchData touchData = this.get_touch_data(current.ID);
					touchData.m_visited = true;
					Vector2 pos = touchData.Pos;
					bool down = false;
					if (!current.Skip)
					{
						pos = new Vector2(current.X, -current.Y) * 2f;
						down = true;
					}
					touchData.frame_update(pos, down);
				}
				touch_data = this.m_touch_data;
				for (int i = 0; i < touch_data.Length; i++)
				{
					Input2.TouchData touchData = touch_data[i];
					if (!touchData.m_visited)
					{
						touchData.m_id = -1;
						touchData.frame_update(touchData.Pos, false);
					}
				}
			}
		}

		public static class Touch
		{
			private static uint m_last_frame_count = 4294967295u;

			private static Input2.TouchDataArray s_touch_data0 = new Input2.TouchDataArray(0);

			public static int MaxTouch
			{
				get
				{
					return Input2.Touch.s_touch_data0.m_touch_data.Length;
				}
			}

			public static Input2.TouchData[] GetData(uint deviceIndex = 0u)
			{
				Common.Assert(deviceIndex == 0u);
				if (Input2.Touch.m_last_frame_count != Common.FrameCount)
				{
					Input2.Touch.s_touch_data0.frame_update();
					Input2.Touch.m_last_frame_count = Common.FrameCount;
				}
				return Input2.Touch.s_touch_data0.m_touch_data;
			}

			public static void SetData(uint deviceIndex, List<Sce.Pss.Core.Input.TouchData> data)
			{
				Common.Assert(deviceIndex == 0u);
				Input2.Touch.s_touch_data0.SetData(data);
			}
		}

		public class GamePadData
		{
			private int m_device_index;

			private bool m_external_control = false;

			private Sce.Pss.Core.Input.GamePadData m_external_data;

			public Input2.ButtonState Left = Input2.ButtonState.Default;

			public Input2.ButtonState Up = Input2.ButtonState.Default;

			public Input2.ButtonState Right = Input2.ButtonState.Default;

			public Input2.ButtonState Down = Input2.ButtonState.Default;

			public Input2.ButtonState Square = Input2.ButtonState.Default;

			public Input2.ButtonState Triangle = Input2.ButtonState.Default;

			public Input2.ButtonState Circle = Input2.ButtonState.Default;

			public Input2.ButtonState Cross = Input2.ButtonState.Default;

			public Input2.ButtonState Start = Input2.ButtonState.Default;

			public Input2.ButtonState Select = Input2.ButtonState.Default;

			public Input2.ButtonState L = Input2.ButtonState.Default;

			public Input2.ButtonState R = Input2.ButtonState.Default;

			public Vector2 Dpad = Math._00;

			public Vector2 AnalogLeft = default(Vector2);

			public Vector2 AnalogRight = default(Vector2);

			internal GamePadData(int device_index)
			{
				this.m_device_index = device_index;
			}

			public void SetData(Sce.Pss.Core.Input.GamePadData data)
			{
				this.m_external_control = true;
				this.m_external_data = data;
			}

			internal void frame_update()
			{
				Sce.Pss.Core.Input.GamePadData gamePadData = this.m_external_data;
				if (!this.m_external_control)
				{
					gamePadData = Sce.Pss.Core.Input.GamePad.GetData(this.m_device_index);
				}
				bool flag = true;
				if (gamePadData.Skip)
				{
					flag = false;
				}
				this.Left.frame_update((gamePadData.Buttons & (GamePadButtons)1) != 0 && flag);
				this.Up.frame_update((gamePadData.Buttons & (GamePadButtons)2) != 0 && flag);
				this.Right.frame_update((gamePadData.Buttons & (GamePadButtons)4) != 0 && flag);
				this.Down.frame_update((gamePadData.Buttons & (GamePadButtons)8) != 0 && flag);
				this.Square.frame_update((gamePadData.Buttons & (GamePadButtons)16) != 0 && flag);
				this.Triangle.frame_update((gamePadData.Buttons & (GamePadButtons)32) != 0 && flag);
				this.Circle.frame_update((gamePadData.Buttons & (GamePadButtons)64) != 0 && flag);
				this.Cross.frame_update((gamePadData.Buttons & (GamePadButtons)128) != 0 && flag);
				this.Start.frame_update((gamePadData.Buttons & (GamePadButtons)256) != 0 && flag);
				this.Select.frame_update((gamePadData.Buttons & (GamePadButtons)512) != 0 && flag);
				this.L.frame_update((gamePadData.Buttons & (GamePadButtons)1024) != 0 && flag);
				this.R.frame_update((gamePadData.Buttons & (GamePadButtons)2048) != 0 && flag);
				if (flag)
				{
					this.Dpad = Math._00;
					if (this.Left.Down)
					{
						this.Dpad -= Math._10;
					}
					if (this.Up.Down)
					{
						this.Dpad += Math._01;
					}
					if (this.Right.Down)
					{
						this.Dpad += Math._10;
					}
					if (this.Down.Down)
					{
						this.Dpad -= Math._01;
					}
					this.AnalogLeft.X = gamePadData.AnalogLeftX;
					this.AnalogLeft.Y = gamePadData.AnalogLeftY;
					this.AnalogRight.X = gamePadData.AnalogRightX;
					this.AnalogRight.Y = gamePadData.AnalogRightY;
				}
			}
		}

		public static class GamePad
		{
			private static uint m_last_frame_count = 4294967295u;

			private static Input2.GamePadData s_game_pad_data0 = new Input2.GamePadData(0);

			public static Input2.GamePadData GetData(uint deviceIndex = 0u)
			{
				Common.Assert(deviceIndex == 0u);
				if (Input2.GamePad.m_last_frame_count != Common.FrameCount)
				{
					Input2.GamePad.s_game_pad_data0.frame_update();
					Input2.GamePad.m_last_frame_count = Common.FrameCount;
				}
				return Input2.GamePad.s_game_pad_data0;
			}

			public static void SetData(uint deviceIndex, Sce.Pss.Core.Input.GamePadData data)
			{
				Common.Assert(deviceIndex == 0u);
				Input2.GamePad.s_game_pad_data0.SetData(data);
			}
		}

		public static Input2.TouchData Touch00
		{
			get
			{
				return Input2.Touch.GetData(0u)[0];
			}
		}

		public static Input2.GamePadData GamePad0
		{
			get
			{
				return Input2.GamePad.GetData(0u);
			}
		}
	}
}
