using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	public class Plane3D : Node
	{
		public Matrix4 ModelMatrix = Matrix4.Identity;

		public Plane3D()
		{
		}

		public Plane3D(Matrix4 modelmatrix)
		{
			this.ModelMatrix = modelmatrix;
		}

		public override void PushTransform()
		{
			Director.Instance.GL.ModelMatrix.Push();
			Director.Instance.GL.ModelMatrix.Mul1(this.ModelMatrix);
		}

		public override void FindParentPlane(ref Matrix4 mat)
		{
			mat = this.ModelMatrix;
		}
	}
}
