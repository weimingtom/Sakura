using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct BlendMode
	{
		public bool Enabled;

		public BlendFunc BlendFunc;

		public static BlendMode None = new BlendMode(false, new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)1, (BlendFuncFactor)1));

		public static BlendMode Normal = new BlendMode(true, new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)5));

		public static BlendMode Additive = new BlendMode(true, new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)1, (BlendFuncFactor)1));

		public static BlendMode Multiplicative = new BlendMode(true, new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)6, (BlendFuncFactor)0));

		public static BlendMode PremultipliedAlpha = new BlendMode(true, new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)1, (BlendFuncFactor)5));

		public BlendMode(bool enabled, BlendFunc blend_func)
		{
			this.Enabled = enabled;
			this.BlendFunc = blend_func;
		}
	}
}
