using System;
using System.Collections;

namespace Sce.Pss.HighLevel.UI
{
	public class TouchEventCollection : CollectionBase
	{
		public TouchEvent this[int index]
		{
			get
			{
				return (TouchEvent)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		public TouchEvent PrimaryTouchEvent
		{
			get;
			internal set;
		}

		public bool Forward
		{
			get;
			set;
		}

		public TouchEventCollection()
		{
			this.Forward = false;
			this.PrimaryTouchEvent = null;
		}

		public TouchEvent GetTouchEventByID(int id)
		{
			foreach (TouchEvent touchEvent in base.List)
			{
				if (touchEvent.FingerID == id)
				{
					return touchEvent;
				}
			}
			return null;
		}

		public int Add(TouchEvent touchEvent)
		{
			return base.List.Add(touchEvent);
		}

		public void Insert(int index, TouchEvent touchEvent)
		{
			base.List.Insert(index, touchEvent);
		}

		public int IndexOf(TouchEvent touchEvent)
		{
			return base.List.IndexOf(touchEvent);
		}

		public void Remove(TouchEvent touchEvent)
		{
			base.List.Remove(touchEvent);
		}

		public bool Contains(TouchEvent touchEvent)
		{
			return base.List.Contains(touchEvent);
		}
	}
}
