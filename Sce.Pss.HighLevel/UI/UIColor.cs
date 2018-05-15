using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public struct UIColor
	{
		public float R;

		public float G;

		public float B;

		public float A;

		public UIColor(float r, float g, float b, float a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		public static explicit operator Vector4(UIColor color)
		{
			return new Vector4(color.R, color.G, color.B, color.A);
		}

		public static explicit operator UIColor(Vector4 vec)
		{
			return new UIColor(vec.X, vec.Y, vec.Z, vec.W);
		}

		public static bool operator ==(UIColor color1, UIColor color2)
		{
			return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B && color1.A == color2.A;
		}

		public static bool operator !=(UIColor color1, UIColor color2)
		{
			return !(color1 == color2);
		}

		public override bool Equals(object o)
		{
			return o is UIColor && (UIColor)o == this;
		}

		public override int GetHashCode()
		{
			return this.A.GetHashCode() ^ this.R.GetHashCode() ^ this.G.GetHashCode() ^ this.B.GetHashCode();
		}
	}
}
