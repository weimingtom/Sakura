using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D
{
	using Math = Sce.Pss.HighLevel.GameEngine2D.Base.Math;
	
	public class ParticleSystem : IDisposable
	{
		public interface IParticleShader
		{
			void SetMVP(ref Matrix4 value);

			void SetColor(ref Vector4 value);

			ShaderProgram GetShaderProgram();
		}

		public class ParticleShaderDefault : ParticleSystem.IParticleShader, IDisposable
		{
			public ShaderProgram m_shader_program;

			public ParticleShaderDefault()
			{
				this.m_shader_program = Common.CreateShaderProgram("cg/particles.cgx");
				this.m_shader_program.SetUniformBinding(0, "MVP");
				this.m_shader_program.SetUniformBinding(1, "Color");
				this.m_shader_program.SetAttributeBinding(0, "vin_data");
				this.m_shader_program.SetAttributeBinding(1, "vin_color");
				Matrix4 identity = Matrix4.Identity;
				this.SetMVP(ref identity);
				this.SetColor(ref Colors.White);
			}

			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					Common.DisposeAndNullify<ShaderProgram>(ref this.m_shader_program);
				}
			}

			public ShaderProgram GetShaderProgram()
			{
				return this.m_shader_program;
			}

			public void SetMVP(ref Matrix4 value)
			{
				this.m_shader_program.SetUniformValue(0, ref value);
			}

			public void SetColor(ref Vector4 value)
			{
				this.m_shader_program.SetUniformValue(1, ref value);
			}
		}

		public class Particle
		{
			public Vector2 Position;

			public Vector2 Velocity;

			public float Age;

			public float LifeSpan;

			public float LifeSpanRcp;

			public float Angle;

			public float AngularVelocity;

			public float ScaleStart;

			public float ScaleDelta;

			public Vector4 ColorStart;

			public Vector4 ColorDelta;

			public float Scale
			{
				get
				{
					return this.ScaleStart + this.ScaleDelta * this.Age;
				}
			}

			public Vector4 Color
			{
				get
				{
					return this.ColorStart + this.ColorDelta * this.Age;
				}
			}

			public bool Dead
			{
				get
				{
					return this.Age >= this.LifeSpan;
				}
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"Position=",
					this.Position,
					" Velocity=",
					this.Velocity,
					" Age=",
					this.Age,
					" LifeSpan=",
					this.LifeSpan,
					" Scale=",
					this.Scale,
					" Color=",
					this.Color
				});
			}
		}

		private struct Vertex
		{
			public Vector4 XYUV;

			public Vector4 Color;

			public Vertex(Vector4 a_data1, Vector4 a_data2)
			{
				this.XYUV = a_data1;
				this.Color = a_data2;
			}
		}

		public class EmitterParams
		{
			public Matrix3 Transform = Matrix3.Identity;

			public Matrix3 TransformForVelocityEstimate = Matrix3.Identity;

			public float ForwardMomentum = 0f;

			public float AngularMomentun = 0f;

			public float WaitTime = 1f;

			public float WaitTimeRelVar = 0.15f;

			public float LifeSpan = 5f;

			public float LifeSpanRelVar;

			public Vector2 Position = Math._00;

			public Vector2 PositionVar = Math._11 * 1.5f;

			public Vector2 Velocity = Math._01;

			public Vector2 VelocityVar = Math._11 * 0.2f;

			public float AngularVelocity;

			public float AngularVelocityVar;

			public float Angle;

			public float AngleVar;

			public Vector4 ColorStart = Colors.White;

			public Vector4 ColorStartVar = Math._0000;

			public Vector4 ColorEnd = Colors.White;

			public Vector4 ColorEndVar = Math._0000;

			public float ScaleStart = 1f;

			public float ScaleStartRelVar;

			public float ScaleEnd = 1f;

			public float ScaleEndRelVar;

			public string ToString(string prefix)
			{
				return string.Concat(new object[]
				{
					prefix,
					"WaitTime           ",
					this.WaitTime,
					"\n",
					prefix,
					"WaitTimeVar        ",
					this.WaitTimeRelVar,
					"\n",
					prefix,
					"Position           ",
					this.Position,
					"\n",
					prefix,
					"PositionVar        ",
					this.PositionVar,
					"\n",
					prefix,
					"Velocity           ",
					this.Velocity,
					"\n",
					prefix,
					"VelocityVar        ",
					this.VelocityVar,
					"\n",
					prefix,
					"AngularVelocity    ",
					this.AngularVelocity,
					"\n",
					prefix,
					"AngularVelocityVar ",
					this.AngularVelocityVar,
					"\n",
					prefix,
					"Angle              ",
					this.Angle,
					"\n",
					prefix,
					"AngleVar           ",
					this.AngleVar,
					"\n",
					prefix,
					"ColorStart         ",
					this.ColorStart,
					"\n",
					prefix,
					"ColorStartVar      ",
					this.ColorStartVar,
					"\n",
					prefix,
					"ColorEnd           ",
					this.ColorEnd,
					"\n",
					prefix,
					"ColorEndVar        ",
					this.ColorEndVar,
					"\n",
					prefix,
					"ScaleStart         ",
					this.ScaleStart,
					"\n",
					prefix,
					"ScaleStartRelVar   ",
					this.ScaleStartRelVar,
					"\n",
					prefix,
					"ScaleEnd           ",
					this.ScaleEnd,
					"\n",
					prefix,
					"ScaleEndRelVar     ",
					this.ScaleEndRelVar,
					"\n",
					prefix,
					"LifeSpan           ",
					this.LifeSpan,
					"\n",
					prefix,
					"LifeSpanVar        ",
					this.LifeSpanRelVar,
					"\n"
				});
			}
		}

		public class SimulationParams
		{
			public float Friction = 0.99f;

			public Vector2 GravityDirection = -Math._01;

			public float Gravity = 0.8f;

			public Vector2 WindDirection;

			public float Wind;

			public float BrownianScale = 5f;

			public float Fade = 0.1f;
		}

		public TextureInfo TextureInfo;

		public Vector4 Color = Colors.White;

		public BlendMode BlendMode = BlendMode.Normal;

		public ParticleSystem.EmitterParams Emit;

		public ParticleSystem.SimulationParams Simulation;

		public Matrix3 RenderTransform = Matrix3.Identity;

		private static ParticleSystem.ParticleShaderDefault m_default_shader;

		public ParticleSystem.IParticleShader Shader;

		private bool m_disposed = false;

		private ImmediateModeQuads<ParticleSystem.Vertex> m_imm_quads;

		private double m_elapsed;

		private float m_emit_timer;

		private Vector2 m_observed_velocity;

		private Matrix3 m_tracking_transform_prev = Matrix3.Identity;

		private float m_observed_angular_velocity;

		private Sce.Pss.HighLevel.GameEngine2D.Base.Math.RandGenerator m_random;

		private ParticleSystem.Vertex m_v0;

		private ParticleSystem.Vertex m_v1;

		private ParticleSystem.Vertex m_v2;

		private ParticleSystem.Vertex m_v3;

		private int m_particles_count;

		private ParticleSystem.Particle[] m_particles;

		private GraphicsContextAlpha GL;

		public int MaxParticles
		{
			get
			{
				return this.m_particles.Length;
			}
		}

		public int ParticlesCount
		{
			get
			{
				return this.m_particles_count;
			}
		}

		public bool IsFull
		{
			get
			{
				return this.MaxParticles == this.m_particles_count;
			}
		}

		public static ParticleSystem.ParticleShaderDefault DefaultShader
		{
			get
			{
				if (ParticleSystem.m_default_shader == null)
				{
					ParticleSystem.m_default_shader = new ParticleSystem.ParticleShaderDefault();
				}
				return ParticleSystem.m_default_shader;
			}
		}

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public ParticleSystem(int max_particles)
		{
			this.GL = Director.Instance.GL;
			this.m_imm_quads = new ImmediateModeQuads<ParticleSystem.Vertex>(this.GL, (uint)max_particles, new VertexFormat[]
			{
			                                                                 	(VertexFormat)259,
			                                                                 	(VertexFormat)259
			});
			this.m_particles = new ParticleSystem.Particle[max_particles];
			for (int i = 0; i < this.m_particles.Length; i++)
			{
				this.m_particles[i] = new ParticleSystem.Particle();
			}
			this.m_random = new Math.RandGenerator(0);
			this.Shader = ParticleSystem.DefaultShader;
			this.Emit = new ParticleSystem.EmitterParams();
			this.Simulation = new ParticleSystem.SimulationParams();
			this.m_v0.XYUV.Zw = Math._00;
			this.m_v1.XYUV.Zw = Math._10;
			this.m_v2.XYUV.Zw = Math._01;
			this.m_v3.XYUV.Zw = Math._11;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_imm_quads.Dispose();
				this.m_disposed = true;
			}
		}

		public static void Terminate()
		{
			Common.DisposeAndNullify<ParticleSystem.ParticleShaderDefault>(ref ParticleSystem.m_default_shader);
		}

		private void init_auto_particle(ParticleSystem.Particle p)
		{
			p.LifeSpan = FMath.Max(0f, this.Emit.LifeSpan * (1f + this.Emit.LifeSpanRelVar * this.m_random.NextFloatMinus1_1()));
			p.Age = 0f;
			p.LifeSpanRcp = 1f / p.LifeSpan;
			p.Position = this.Emit.Position + this.Emit.PositionVar * this.m_random.NextVector2(-1f, 1f);
			p.Position = (this.Emit.Transform * p.Position.Xy1).Xy;
			p.Velocity = this.Emit.Velocity + this.Emit.VelocityVar * this.m_random.NextVector2(-1f, 1f);
			p.Velocity = (this.Emit.Transform * p.Velocity.Xy0).Xy;
			p.Angle = FMath.Max(0f, this.Emit.Angle + this.Emit.AngleVar * this.m_random.NextFloatMinus1_1());
			p.Angle += Math.Angle(this.Emit.Transform.X.Xy.Normalize());
			p.AngularVelocity = FMath.Max(0f, this.Emit.AngularVelocity + this.Emit.AngularVelocityVar * this.m_random.NextFloatMinus1_1());
			p.Velocity += this.Emit.ForwardMomentum * this.m_observed_velocity;
			p.AngularVelocity += this.Emit.AngularMomentun * this.m_observed_angular_velocity;
			p.ScaleStart = FMath.Max(0f, this.Emit.ScaleStart * (1f + this.Emit.ScaleStartRelVar * this.m_random.NextFloatMinus1_1()));
			float num = FMath.Max(0f, this.Emit.ScaleEnd * (1f + this.Emit.ScaleEndRelVar * this.m_random.NextFloatMinus1_1()));
			p.ScaleDelta = (num - p.ScaleStart) / p.LifeSpan;
			p.ColorStart = (this.Emit.ColorStart + this.Emit.ColorStartVar * this.m_random.NextFloatMinus1_1()).Clamp(0f, 1f);
			Vector4 vector = (this.Emit.ColorEnd + this.Emit.ColorEndVar * this.m_random.NextFloatMinus1_1()).Clamp(0f, 1f);
			p.ColorDelta = (vector - p.ColorStart) / p.LifeSpan;
		}

		private void update(ParticleSystem.Particle p, float dt, Vector2 forces)
		{
			p.Velocity += (forces - p.Velocity * this.Simulation.Friction) * dt;
			p.Position += p.Velocity * dt;
			p.Angle += p.AngularVelocity * dt;
			p.Age += dt;
		}

		public ParticleSystem.Particle CreateParticle(bool skip_auto_init = false)
		{
			ParticleSystem.Particle result;
			if (this.IsFull)
			{
				result = null;
			}
			else
			{
				if (!skip_auto_init)
				{
					this.init_auto_particle(this.m_particles[this.m_particles_count]);
				}
				this.m_particles_count++;
				result = this.m_particles[this.m_particles_count - 1];
			}
			return result;
		}

		public void Update(float dt)
		{
			this.m_observed_velocity = (this.Emit.TransformForVelocityEstimate.Z - this.m_tracking_transform_prev.Z).Xy / dt;
			this.m_observed_angular_velocity = this.m_tracking_transform_prev.X.Xy.Normalize().Angle(this.Emit.TransformForVelocityEstimate.X.Xy.Normalize()) / dt;
			this.m_tracking_transform_prev = this.Emit.TransformForVelocityEstimate;
			this.m_elapsed += (double)dt;
			float num = FMath.Max(0f, this.Emit.WaitTime * (1f + this.Emit.WaitTimeRelVar * this.m_random.NextFloatMinus1_1()));
			this.m_emit_timer += dt;
			while (this.m_emit_timer > num && !this.IsFull)
			{
				this.init_auto_particle(this.m_particles[this.m_particles_count]);
				this.m_particles_count++;
				this.m_emit_timer -= num;
				num = FMath.Max(0f, this.Emit.WaitTime * (1f + this.Emit.WaitTimeRelVar * this.m_random.NextFloatMinus1_1()));
			}
			Vector2 vector = this.Simulation.Gravity * this.Simulation.GravityDirection + this.Simulation.Wind * this.Simulation.WindDirection;
			int i = 0;
			while (i < this.m_particles_count)
			{
				if (this.m_particles[i].Dead)
				{
					Common.Swap<ParticleSystem.Particle>(ref this.m_particles[i], ref this.m_particles[this.m_particles_count - 1]);
					this.m_particles_count--;
				}
				else
				{
					Vector2 vector2 = vector;
					if (this.Simulation.BrownianScale != 0f)
					{
						vector2 += this.m_random.NextVector2Minus1_1() * this.Simulation.BrownianScale;
					}
					this.update(this.m_particles[i], dt, vector2);
					i++;
				}
			}
		}

		private float age_to_alpha(float x, float dx)
		{
			x = FMath.Abs(x - 0.5f);
			float num = 0.5f - dx;
			return 1f - FMath.Max(0f, (x - num) * (1f / dx));
		}

		public void Draw()
		{
			this.GL.SetDepthMask(false);
			this.GL.ModelMatrix.Push();
			this.GL.ModelMatrix.Set(this.RenderTransform.Matrix4());
			Matrix4 mVP = this.GL.GetMVP();
			this.Shader.SetMVP(ref mVP);
			this.Shader.SetColor(ref this.Color);
			this.GL.SetBlendMode(this.BlendMode);
			Common.Assert(this.TextureInfo != null, "TextureInfo has not been set.");
			this.GL.Context.SetShaderProgram(this.Shader.GetShaderProgram());
			this.GL.Context.SetTexture(0, this.TextureInfo.Texture);
			this.m_imm_quads.ImmBeginQuads((uint)this.m_particles_count);
			for (int i = 0; i < this.m_particles_count; i++)
			{
				Vector2 vector = Vector2.Rotation(this.m_particles[i].Angle) * this.m_particles[i].Scale;
				Vector2 vector2 = Math.Perp(vector);
				Vector2 vector3 = this.m_particles[i].Position - (vector + vector2) * 0.5f;
				Vector4 color = this.m_particles[i].Color;
				color.W *= this.age_to_alpha(this.m_particles[i].Age * this.m_particles[i].LifeSpanRcp, this.Simulation.Fade);
				this.m_v0.XYUV.Xy = vector3;
				this.m_v0.Color = color;
				this.m_v1.XYUV.Xy = vector3 + vector;
				this.m_v1.Color = color;
				this.m_v2.XYUV.Xy = vector3 + vector2;
				this.m_v2.Color = color;
				this.m_v3.XYUV.Xy = vector3 + vector + vector2;
				this.m_v3.Color = color;
				this.m_imm_quads.ImmAddQuad(this.m_v0, this.m_v1, this.m_v2, this.m_v3);
			}
			this.m_imm_quads.ImmEndQuads();
			this.GL.SetDepthMask(true);
			this.GL.ModelMatrix.Pop();
		}

		private void Dump()
		{
			string text = Common.FrameCount + " ";
			Console.WriteLine(string.Concat(new object[]
			{
				text,
				"ParticlesCount ",
				this.ParticlesCount,
				"/",
				this.MaxParticles
			}));
			Console.WriteLine(text + "Emit" + this.Emit.ToString(text));
			for (int i = 0; i < this.m_particles_count; i++)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					text,
					"[",
					i,
					"] ",
					this.m_particles[i]
				}));
			}
		}
	}
}
