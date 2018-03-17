using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class SpriteRenderer : IDisposable
	{
		public interface ISpriteShader
		{
			void SetMVP(ref Matrix4 value);

			void SetColor(ref Vector4 value);

			void SetUVTransform(ref Vector4 value);

			ShaderProgram GetShaderProgram();
		}

		public class DefaultShader_ : SpriteRenderer.ISpriteShader, IDisposable
		{
			public ShaderProgram m_shader_program;

			public DefaultShader_()
			{
				this.m_shader_program = Common.CreateShaderProgram("cg/sprite.cgx");
				this.m_shader_program.SetUniformBinding(0, "MVP");
				this.m_shader_program.SetUniformBinding(1, "Color");
				this.m_shader_program.SetUniformBinding(2, "UVTransform");
				this.m_shader_program.SetAttributeBinding(0, "vin_data");
				Matrix4 identity = Matrix4.Identity;
				this.SetMVP(ref identity);
				this.SetColor(ref Colors.White);
				this.SetUVTransform(ref Math.UV_TransformFlipV);
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
//				Debug.WriteLine(">>>SetMVP:" + value.ToString());
				this.m_shader_program.SetUniformValue(0, ref value);
			}

			public void SetColor(ref Vector4 value)
			{
				this.m_shader_program.SetUniformValue(1, ref value);
			}

			public void SetUVTransform(ref Vector4 value)
			{
//				Debug.WriteLine(">>>SetUVTransform:" + value.ToString());
				this.m_shader_program.SetUniformValue(2, ref value);
			}
		}

		public class DefaultFontShader_ : SpriteRenderer.ISpriteShader, IDisposable
		{
			public ShaderProgram m_shader_program;

			public DefaultFontShader_()
			{
				this.m_shader_program = Common.CreateShaderProgram("cg/font.cgx");
				this.m_shader_program.SetUniformBinding(0, "MVP");
				this.m_shader_program.SetUniformBinding(1, "Color");
				this.m_shader_program.SetUniformBinding(2, "UVTransform");
				this.m_shader_program.SetAttributeBinding(0, "vin_data");
				Matrix4 identity = Matrix4.Identity;
				this.SetMVP(ref identity);
				this.SetColor(ref Colors.White);
				this.SetUVTransform(ref Math.UV_TransformFlipV);
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

			public void SetUVTransform(ref Vector4 value)
			{
				this.m_shader_program.SetUniformValue(2, ref value);
			}
		}

		private GraphicsContextAlpha GL;

		private ImmediateModeQuads<Vector4> m_imm_quads;

		private Vector4 m_v0;

		private Vector4 m_v1;

		private Vector4 m_v2;

		private Vector4 m_v3;

		private TextureInfo m_current_texture_info;

		private TextureInfo m_embedded_font_texture_info;

		public bool FlipU = false;

		public bool FlipV = false;

		private SpriteRenderer.DefaultShader_ m_default_shader = new SpriteRenderer.DefaultShader_();

		private SpriteRenderer.DefaultFontShader_ m_default_font_shader = new SpriteRenderer.DefaultFontShader_();

		private bool m_disposed = false;

		public SpriteRenderer.DefaultShader_ DefaultShader
		{
			get
			{
				return this.m_default_shader;
			}
		}

		public SpriteRenderer.DefaultFontShader_ DefaultFontShader
		{
			get
			{
				return this.m_default_font_shader;
			}
		}

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public SpriteRenderer(GraphicsContextAlpha gl, uint max_sprites)
		{
			this.GL = gl;
			this.m_imm_quads = new ImmediateModeQuads<Vector4>(this.GL, max_sprites, new VertexFormat[]
			{
			                                                   	(VertexFormat)259
			});
			Texture2D texture2D = EmbeddedDebugFontData.CreateTexture();
			this.m_embedded_font_texture_info = new TextureInfo();
			this.m_embedded_font_texture_info.Initialize(texture2D, new Vector2i(EmbeddedDebugFontData.NumChars, 1), new TRS(new Bounds2(new Vector2(0f, 0f), new Vector2((float)(EmbeddedDebugFontData.CharSizei.X * EmbeddedDebugFontData.NumChars) / (float)texture2D.Width, (float)EmbeddedDebugFontData.CharSizei.Y / (float)texture2D.Height))));
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
				this.m_embedded_font_texture_info.Dispose();
				Common.DisposeAndNullify<SpriteRenderer.DefaultFontShader_>(ref this.m_default_font_shader);
				Common.DisposeAndNullify<SpriteRenderer.DefaultShader_>(ref this.m_default_shader);
				this.m_disposed = true;
			}
		}

		public Bounds2 DrawTextDebug(string str, Vector2 bottom_left_start_pos, float char_height, bool draw = true, SpriteRenderer.ISpriteShader shader = null)
		{
			if (null == shader)
			{
				shader = this.m_default_font_shader;
			}
			float num = char_height / (float)EmbeddedDebugFontData.CharSizei.Y;
			Vector2 vector = new Vector2(-1f, 1f);
			Vector2 vector2 = vector * num;
			TRS tRS = default(TRS);
			tRS.R = Math._10;
			tRS.S = EmbeddedDebugFontData.CharSizef * num;
			tRS.T = bottom_left_start_pos;
			Vector2 max = bottom_left_start_pos;
			float x = tRS.T.X;
			if (draw)
			{
				this.DefaultFontShader.SetUVTransform(ref Math.UV_TransformFlipV);
				this.BeginSprites(this.m_embedded_font_texture_info, this.DefaultFontShader, str.Length);
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\n')
				{
					tRS.T -= new Vector2(0f, tRS.S.Y + vector2.Y);
					tRS.T.X = x;
				}
				else
				{
					if (draw)
					{
						int num2 = (int)(str[i] - ' ');
						if (num2 < 0 || num2 >= EmbeddedDebugFontData.NumChars)
						{
							num2 = 31;
						}
						this.AddSprite(ref tRS, new Vector2i(num2, 0));
					}
					tRS.T += new Vector2(tRS.S.X + vector2.X, 0f);
					max.X = FMath.Max(tRS.T.X, max.X);
					max.Y = FMath.Min(tRS.T.Y, max.Y);
				}
			}
			if (draw)
			{
				this.EndSprites();
			}
			return Bounds2.SafeBounds(bottom_left_start_pos + new Vector2(0f, tRS.S.Y), max);
		}

		public Bounds2 DrawTextWithFontMap(string str, Vector2 bottom_left_start_pos, float char_height, bool draw, FontMap fontmap, SpriteRenderer.ISpriteShader shader)
		{
			float num = char_height / fontmap.CharPixelHeight;
			Vector2 vector = new Vector2(1f, 1f);
			Vector2 vector2 = vector * num;
			Vector2 vector3 = bottom_left_start_pos;
			Vector2 max = bottom_left_start_pos;
			float x = bottom_left_start_pos.X;
			if (draw)
			{
				shader.SetUVTransform(ref Math.UV_TransformFlipV);
				this.BeginSprites(new TextureInfo(fontmap.Texture), shader, str.Length);
			}
			for (int i = 0; i < str.Length; i++)
			{
				FontMap.CharData charData;
				if (str[i] == '\n')
				{
					vector3 -= new Vector2(0f, char_height + vector2.Y);
					vector3.X = x;
				}
				else if (fontmap.TryGetCharData(str[i], out charData))
				{
					Vector2 vector4 = charData.PixelSize * num;
					if (draw)
					{
						this.AddSprite(vector3, new Vector2(vector4.X, 0f), charData.UV);
					}
					vector3 += new Vector2(vector4.X + vector2.X, 0f);
					max.X = FMath.Max(vector3.X, max.X);
					max.Y = FMath.Min(vector3.Y, max.Y);
				}
			}
			if (draw)
			{
				this.EndSprites();
			}
			return Bounds2.SafeBounds(bottom_left_start_pos + new Vector2(0f, char_height), max);
		}

		public Bounds2 DrawTextWithFontMap(string str, Vector2 bottom_left_start_pos, float char_height, bool draw, FontMap fontmap)
		{
			return this.DrawTextWithFontMap(str, bottom_left_start_pos, char_height, draw, fontmap, this.DefaultFontShader);
		}

		public void BeginSprites(TextureInfo texture_info, SpriteRenderer.ISpriteShader shader, int num_sprites)
		{
			Matrix4 mVP = this.GL.GetMVP();
//			mVP = Matrix4.Identity;
			shader.SetMVP(ref mVP);
			this.GL.Context.SetShaderProgram(shader.GetShaderProgram());
			Common.Assert(!texture_info.Disposed, "This TextureInfo oject has been disposed");
			this.GL.Context.SetTexture(0, texture_info.Texture);
			this.m_current_texture_info = texture_info;
			this.m_imm_quads.ImmBeginQuads((uint)num_sprites);
		}

		public void BeginSprites(TextureInfo texture_info, int num_sprites)
		{
			this.BeginSprites(texture_info, this.DefaultShader, num_sprites);
		}

		public void EndSprites()
		{
			this.m_imm_quads.ImmEndQuads();
			this.m_current_texture_info = null;
		}

		public void AddSprite(ref TRS quad, Vector2i tile_index)
		{
			Vector2 x = quad.X;
			Vector2 y = quad.Y;
			TextureInfo.CachedTileData cachedTiledData = this.m_current_texture_info.GetCachedTiledData(ref tile_index);
			this.m_v0 = new Vector4(quad.T, cachedTiledData.UV_00);
			this.m_v1 = new Vector4(quad.T + x, cachedTiledData.UV_10);
			this.m_v2 = new Vector4(quad.T + y, cachedTiledData.UV_01);
			this.m_v3 = new Vector4(quad.T + x + y, cachedTiledData.UV_11);
			this.add_quad();
		}

		public void AddSprite(ref TRS quad, Vector2i tile_index, ref Matrix3 mat)
		{
			Vector2 x = quad.X;
			Vector2 y = quad.Y;
			TextureInfo.CachedTileData cachedTiledData = this.m_current_texture_info.GetCachedTiledData(ref tile_index);
			this.m_v0 = new Vector4(this.transform_point(ref mat, quad.T), cachedTiledData.UV_00);
			this.m_v1 = new Vector4(this.transform_point(ref mat, quad.T + x), cachedTiledData.UV_10);
			this.m_v2 = new Vector4(this.transform_point(ref mat, quad.T + y), cachedTiledData.UV_01);
			this.m_v3 = new Vector4(this.transform_point(ref mat, quad.T + x + y), cachedTiledData.UV_11);
			this.add_quad();
		}

		public void AddSprite(ref TRS quad, ref TRS uv)
		{
			Vector2 x = quad.X;
			Vector2 y = quad.Y;
			Vector2 x2 = uv.X;
			Vector2 y2 = uv.Y;
			this.m_v0 = new Vector4(quad.T, uv.T);
			this.m_v1 = new Vector4(quad.T + x, uv.T + x2);
			this.m_v2 = new Vector4(quad.T + y, uv.T + y2);
			this.m_v3 = new Vector4(quad.T + x + y, uv.T + x2 + y2);
			this.add_quad();
		}

		public void AddSprite(ref TRS quad, ref TRS uv, ref Matrix3 mat)
		{
			Vector2 x = quad.X;
			Vector2 y = quad.Y;
			Vector2 x2 = uv.X;
			Vector2 y2 = uv.Y;
			this.m_v0 = new Vector4(this.transform_point(ref mat, quad.T), uv.T);
			this.m_v1 = new Vector4(this.transform_point(ref mat, quad.T + x), uv.T + x2);
			this.m_v2 = new Vector4(this.transform_point(ref mat, quad.T + y), uv.T + y2);
			this.m_v3 = new Vector4(this.transform_point(ref mat, quad.T + x + y), uv.T + x2 + y2);
			this.add_quad();
		}

		public void AddSprite(Vector2 bottom_left_start_pos, Vector2 x, Bounds2 uv_bounds)
		{
			Vector2 vector = uv_bounds.Size.Abs() * this.m_current_texture_info.TextureSizef;
			Vector2 vector2 = Math.Perp(x) * vector.Y / vector.X;
			this.m_v0 = new Vector4(bottom_left_start_pos, uv_bounds.Point00);
			this.m_v1 = new Vector4(bottom_left_start_pos + x, uv_bounds.Point10);
			this.m_v2 = new Vector4(bottom_left_start_pos + vector2, uv_bounds.Point01);
			this.m_v3 = new Vector4(bottom_left_start_pos + x + vector2, uv_bounds.Point11);
			this.add_quad();
		}

		public void AddSprite(Vector4 v0, Vector4 v1, Vector4 v2, Vector4 v3)
		{
			this.m_v0 = v0;
			this.m_v1 = v1;
			this.m_v2 = v2;
			this.m_v3 = v3;
			this.add_quad();
		}

		private void add_quad()
		{
			if (this.FlipU)
			{
				Common.Swap<float>(ref this.m_v0.X, ref this.m_v1.X);
				Common.Swap<float>(ref this.m_v2.X, ref this.m_v3.X);
			}
			if (this.FlipV)
			{
				Common.Swap<float>(ref this.m_v0.Y, ref this.m_v2.Y);
				Common.Swap<float>(ref this.m_v1.Y, ref this.m_v3.Y);
			}
			this.m_imm_quads.ImmAddQuad(this.m_v0, this.m_v1, this.m_v2, this.m_v3);
		}

		private Vector2 transform_point(ref Matrix3 mat, Vector2 pos)
		{
			return mat.X.Xy * pos.X + mat.Y.Xy * pos.Y + mat.Z.Xy;
		}
	}
}
