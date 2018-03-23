/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using Sce.Pss.Core;
using Sce.Pss.Core.Audio;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Input;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PuzzleGameDemo
{
	public static class RandomExtensions
	{
		public static bool NextBool(this Random self)
		{
			return (self.Next() & 1) == 0;
		}

		public static float NextFloat(this Random self)
		{
			return (float)self.NextDouble();
		}

		public static float NextSignedFloat(this Random self)
		{
			return (float)self.NextDouble() * (float)self.NextSign();
		}

		public static float NextAngle(this Random self)
		{
			return self.NextFloat() * FMath.PI * 2.0f;
		}

		public static float NextSign(this Random self)
		{
			return self.NextDouble() < 0.5 ? -1.0f : 1.0f;
		}

		public static Vector2 NextVector2(this Random self)
		{
			return Vector2.UnitX.Rotate(self.NextFloat() * FMath.PI * 2.0f);
		}

		public static Vector2 NextVector2(this Random self, float magnitude)
		{
			return Vector2.UnitX.Rotate(self.NextFloat() * FMath.PI * 2.0f) * magnitude;
		}

		public static Vector2 NextVector2Variable(this Random self)
		{
			return new Vector2(self.NextFloat(), self.NextFloat());
		}
	}

	public static class Support
	{
		public static TextureFilterMode DefaultTextureFilterMode = TextureFilterMode.Linear;
		public static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
		public static Dictionary<string, TextureInfo> TextureInfoCache = new Dictionary<string, TextureInfo>();

		public static Sce.Pss.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };

			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);

			// DEBUG: testing for current assets
			result.Scale = new Vector2(1.0f);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}

		public static Sce.Pss.HighLevel.GameEngine2D.SpriteTile UnicolorSprite(string name, byte r, byte g, byte b, byte a)
		{
			uint color = (uint)((uint)a << 24 | (uint)b << 16 | (uint)g << 8 | (uint)r);

			if (TextureCache.ContainsKey(name) == false)
			{
				TextureCache[name] = GraphicsContextAlpha.CreateTextureUnicolor(color);
				TextureInfoCache[name] = new TextureInfo(TextureCache[name], new Vector2i(1, 1));
			}

			var tex = TextureCache[name];
			var info = TextureInfoCache[name];
			var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info };

			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}

		public static Sce.Pss.HighLevel.GameEngine2D.SpriteUV SpriteUVFromFile(string filename)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename]);
			}

			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteUV() { TextureInfo = info };

			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}

		public static Sce.Pss.HighLevel.GameEngine2D.SpriteTile TiledSpriteFromFile(string filename, int x, int y)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info };

			// TODO: this probably shouldn't be necessary, the TextureInfo should know it is tiled
			result.TileIndex2D = new Vector2i(0, 0);

			result.Quad.S = new Vector2(info.Texture.Width / x, info.Texture.Height / y);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}

		public static Sce.Pss.HighLevel.GameEngine2D.SpriteList TiledSpriteListFromFile(string filename, int x, int y)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}

			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteList(info);

			// TODO: this probably shouldn't be necessary, the TextureInfo should know it is tiled
			//result.TileIndex2D = new Vector2i(0, 0);
			//result.Quad.S = new Vector2(info.Texture.Width / x, info.Texture.Height / y);

			tex.SetFilter(DefaultTextureFilterMode);
			
			return result;
		}

		public static int IncrementTile(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite, int steps, int min, int max, bool looping)
		{
			int x = sprite.TextureInfo.NumTiles.X;
			int y = sprite.TextureInfo.NumTiles.Y;

			int current = sprite.TileIndex2D.X + sprite.TileIndex2D.Y * x;

			if (looping)
			{
				current -= min;
				current += steps;
				current %= max - min;
				current += min;
			}
			else
			{
				current += steps;
				current = System.Math.Min(current, max - 1);
			}

			sprite.TileIndex2D = new Vector2i(current % x, current / x);

			return current;
		}

		public static void SetTile(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite, int n)
		{
			int x = sprite.TextureInfo.NumTiles.X;
			int y = sprite.TextureInfo.NumTiles.Y;
			sprite.TileIndex2D = new Vector2i(n % x, n / x);
		}

		public static int GetTileIndex2D(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite)
		{
			if (sprite.TextureInfo.NumTiles.X <= 1 && sprite.TextureInfo.NumTiles.Y <= 1)
			{
				return 0;
			}

			return sprite.TileIndex2D.Y * sprite.TextureInfo.NumTiles.X + sprite.TileIndex2D.X;
		}

		public class AnimationAction : Sce.Pss.HighLevel.GameEngine2D.ActionBase
		{
			int animation_start;
			int animation_end;
			Sce.Pss.HighLevel.GameEngine2D.SpriteTile attached_sprite;
			float counter;
			float frame_time;
			float speed;
			bool looping;

			public AnimationAction(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite, float seconds)
				: this(sprite, 0, sprite.TextureInfo.NumTiles.X * sprite.TextureInfo.NumTiles.Y, seconds)
			{
			}

			public AnimationAction(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite, int a, int b, float seconds, bool looping = false)
			{
				this.looping = looping;
				speed = 1.0f;

				attached_sprite = sprite;

				int min = System.Math.Min(a, b);
				int max = System.Math.Max(a, b);
				int frames = System.Math.Max(1, max - min);	

				frame_time = seconds / (float)frames;
				animation_start = min;
				animation_end = max;

				Reset();
			}

			public override void Run()
			{
				base.Run();
				Reset();
			}

			public override void Update(float dt)
			{
				dt *= speed;
				
				base.Update(dt);

				counter += dt;

				int frames = 0;
				while (frame_time > 0.0f && counter > frame_time)
				{
					counter -= frame_time;
					frames += 1;
				}

				int tile_index = IncrementTile(attached_sprite, frames, animation_start, animation_end, looping);

				if (!looping && tile_index == animation_end - 1)
				{
					Stop();
				}
			}

			public void SetSpeed(float speed)
			{
				this.speed = speed;
			}

			public void Reset()
			{
				counter = 0.0f;
				SetTile(attached_sprite, animation_start);
			}
		}

		public class SoundSystem
		{
			public static SoundSystem Instance = new SoundSystem("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/");

			public string AssetsPrefix;
			public Dictionary<string, SoundPlayer> SoundDatabase;

			public SoundSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				SoundDatabase = new Dictionary<string,SoundPlayer>();
			}

			public void CheckCache(string name)
			{
				if (SoundDatabase.ContainsKey(name)){
					return;
				}

				var sound = new Sound(AssetsPrefix + name);
				var player = sound.CreatePlayer();
				SoundDatabase[name] = player;
			}

			public void Play(string name)
			{
				CheckCache(name);

				// replace any playing instance
				SoundDatabase[name].Stop();
				SoundDatabase[name].Play();
				SoundDatabase[name].Volume = 0.5f;
			}

			public void Stop(string name)
			{
				CheckCache(name);
				SoundDatabase[name].Stop();
			}

			public void PlayNoClobber(string name)
			{
				CheckCache(name);

				if (SoundDatabase[name].Status == SoundStatus.Playing){
					return;
				}

				SoundDatabase[name].Play();
				SoundDatabase[name].Volume = 0.5f;
			}
		}

		public class MusicSystem
		{
			public static MusicSystem Instance = new MusicSystem("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/");

			public string AssetsPrefix;
			public Dictionary<string, BgmPlayer> MusicDatabase;

			public MusicSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				MusicDatabase = new Dictionary<string, BgmPlayer>();
			}

			public void StopAll()
			{
				foreach (KeyValuePair<string, BgmPlayer> kv in MusicDatabase)
				{
					kv.Value.Stop();
					kv.Value.Dispose();
				}

				MusicDatabase.Clear();
			}

			public void Play(string name)
			{
				StopAll();

				var music = new Bgm(AssetsPrefix + name);
				var player = music.CreatePlayer();
				MusicDatabase[name] = player;

				MusicDatabase[name].Play();
				MusicDatabase[name].Volume = 0.5f;
			}

			public void Stop(string name)
			{
				StopAll();
			}

			public void PlayNoClobber(string name)
			{
				if (MusicDatabase.ContainsKey(name))
				{
					if (MusicDatabase[name].Status == BgmStatus.Playing)
					{
						return;
					}
				}

				Play(name);
			}
		}

		public class AdjustableDelayAction
			: Sce.Pss.HighLevel.GameEngine2D.DelayTime
		{
			public float Speed { get; set; }

			public AdjustableDelayAction()
			{
				Speed = 1.0f;
			}

			public override void Update(float dt)
			{
				base.Update(dt * Speed);
			}
		}

		public class ParticleEffectsManager
			: Sce.Pss.HighLevel.GameEngine2D.Node
		{
			public class Particle
			{
				public Vector2 position;
				public Vector2 velocity;
				public Vector2 friction;
				public Vector4 color;
				public float time;
				public float lifetime;
				public Vector2 size;
				public Vector2 size_delta;
				public float gravity;
			};

			public struct VertexData
			{
				public Vector2 position;
				public Vector2 uv;
				public Vector4 color;
			};

			public List<Particle> Particles;
			public int ActiveParticles;
			public VertexData[] VertexDataArray;
			public ShaderProgram ShaderProgram;
			public Texture2D ParticleDotTexture;

			ImmediateModeQuads< VertexData > imm_quads;
			int max_particles { get { return 768; } }

			public ParticleEffectsManager()
			{
				Particles = new List<Particle>();

				for (int i = 0; i < max_particles; ++i){
					Particles.Add(new Particle());
				}

				ShaderProgram = new ShaderProgram("/Application/Sample/GameEngine2D/PuzzleGameDemo/shaders/pfx.cgx");
				ParticleDotTexture = new Texture2D("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/particle_dot.png", false);
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);

				AdHocDraw += this.DrawParticles;

				imm_quads = new ImmediateModeQuads< VertexData >( Director.Instance.GL, (uint)max_particles, VertexFormat.Float2, VertexFormat.Float2, VertexFormat.Float4 );
			}

			public void Tick(float dt)
			{
				float fullness = (float)ActiveParticles / (float)Particles.Count;
				float life_speed = fullness;

				for (int i = 0; i < ActiveParticles; ++i)
				{
					Particle p = Particles[i];
					p.position += p.velocity;
					p.velocity += new Vector2(0.0f, p.gravity * -0.5f);
					p.velocity *= p.friction;
					p.time += dt;
					p.time += dt * fullness;
					p.size += p.size_delta;

					//if (p.position.Y < Game.Instance.FloorHeight)
					//{
						//p.velocity.Y *= -0.5f;
						//p.velocity.X *= 0.75f;
						//p.position += p.velocity;
					//}
				}

				for (int i = 0; i < ActiveParticles; ++i)
				{
					Particle p = Particles[i];

					if (p.size.X <= 0.0f || p.size.Y <= 0.0f)
					{
						p.time = p.lifetime;
					}

					if (p.time < p.lifetime)
					{
						continue;
					}

					Particles[i] = Particles[ActiveParticles - 1];
					Particles[ActiveParticles - 1] = p;
					ActiveParticles--;
					i--;
				}
			}

			public void DrawParticles()
			{
				Director.Instance.GL.ModelMatrix.Push();
				Director.Instance.GL.ModelMatrix.SetIdentity();

				Matrix4 transform = Director.Instance.GL.GetMVP();
				ShaderProgram.SetUniformValue(ShaderProgram.FindUniform("MVP"), ref transform);

				ShaderProgram.SetAttributeBinding(0, "iPosition");
				ShaderProgram.SetAttributeBinding(1, "iUV");
				ShaderProgram.SetAttributeBinding(2, "iColor");

				Director.Instance.GL.Context.SetShaderProgram(ShaderProgram);
				//Director.Instance.GL.Context.Disable(EnableMode.Blend);
				//Director.Instance.GL.Context.Disable(EnableMode.CullFace);

				Director.Instance.GL.Context.SetTexture(0, ParticleDotTexture);

				Common.Assert( ActiveParticles <= imm_quads.MaxQuads );

				imm_quads.ImmBeginQuads( (uint)ActiveParticles );

				for (int i = 0; i < ActiveParticles; ++i)
				{
					Particle p = Particles[i];
					//p.color = new Vector4(1,1,0,1);
					imm_quads.ImmAddQuad( 
					new VertexData() { position = p.position + new Vector2(0, 0), uv = new Vector2(0, 0), color = p.color },
					new VertexData() { position = p.position + new Vector2(p.size.X, 0), uv = new Vector2(1, 0), color = p.color },
					new VertexData() { position = p.position + new Vector2(0, p.size.Y), uv = new Vector2(0, 1), color = p.color },
					new VertexData() { position = p.position + new Vector2(p.size.X, p.size.Y), uv = new Vector2(1, 1), color = p.color } );

					//Director.Instance.DrawHelpers.DrawDisk(p.position, p.size.X, 8);
					//Director.Instance.DrawHelpers.SetColor(p.color);
					//Director.Instance.DrawHelpers.DrawBounds2Fill(new Bounds2(p.position, p.position + p.size));
				}

				imm_quads.ImmEndQuads();

				Director.Instance.GL.Context.SetShaderProgram(null);
				Director.Instance.GL.Context.SetVertexBuffer(0, null);
				Director.Instance.GL.ModelMatrix.Pop();
			}

			public void AddParticlesBurst(int count, Vector2 position, Vector2 velocity, Vector4 color, float jitter = 0.0f, float scale_multiplier = 1.0f)
			{
				for (int i = 0; i < count; ++i)
				{
					Vector2 p = position + Game.Instance.Random.NextVector2() * jitter;
					Vector2 v = velocity + Game.Instance.Random.NextVector2() * jitter;
					AddParticle(p, v, color, scale_multiplier);
				}
			}

			public void AddParticlesCone(int count, Vector2 position, Vector2 velocity, Vector4 color, float spread, float scale_multiplier = 1.0f)
			{
				for (int i = 0; i < count; ++i)
				{
					Vector2 p = position + velocity * Game.Instance.Random.NextFloat();
					Vector2 v = velocity + velocity.Rotate(Game.Instance.Random.NextSignedFloat() * spread + spread * 0.5f);
					AddParticle(p, v, color, scale_multiplier);
				}
			}

			public void AddParticle(Vector2 position, Vector2 velocity, Vector4 color, float scale_multiplier)
			{
				if (ActiveParticles >= Particles.Count)
				{
					return;
				}

				Particle p = Particles[ActiveParticles];
				p.position = position;
				p.velocity = velocity;
				p.friction = Vector2.One;
				p.color = color;
				p.time = 0.0f;
				p.lifetime = 1.5f;
				p.size = Vector2.One * 12.0f * scale_multiplier;
				p.size_delta = new Vector2(-0.75f);
				p.gravity = 0.75f;
				ActiveParticles++;
			}

			public void AddParticleWater(Vector2 position, Vector2 velocity, Vector4 color, float scale_multiplier)
			{
				if (ActiveParticles >= Particles.Count)
				{
					return;
				}
				
				Particle p = Particles[ActiveParticles];
				p.position = position;
				p.velocity = velocity;
				p.friction = Vector2.One;
				p.color = color;
				p.time = 0.0f;
				p.lifetime = 1.0f;
				p.size = Vector2.One * 8.0f * scale_multiplier;
				p.size_delta = new Vector2(-0.1f);
				p.gravity = -0.025f;
				ActiveParticles++;
			}
		}

		public class TextureTileMapManager
		{
			public struct VertexData
			{
				public Vector2 position;
				public Vector2 uv;
			};

			public VertexBuffer VertexBuffer;
			public ShaderProgram ShaderProgram;
			public VertexData[] VertexDataArray;
			public ushort[] IndexDataArray;

			public class Entry
			{
				public int TilesX;
				public int TilesY;
				public int TileWidth;
				public int TileHeight;
				public List<List<byte>> Data;
			};

			public Dictionary<string, Entry> TileData;

			public static int ScaleDivisor = 4;

			public TextureTileMapManager()
			{
				VertexBuffer = new VertexBuffer(4, 4, VertexFormat.Float2, VertexFormat.Float2);
				ShaderProgram = new ShaderProgram("SirAwesomealot/offscreen.cgx");
				VertexDataArray = new VertexData[4];
				IndexDataArray = new ushort[4] { 0, 1, 2, 3 };
				
				TileData = new Dictionary<string, Entry>();
			}

			public void Add(string name, Texture2D texture, int tiles_x, int tiles_y)
			{
				int tile_width = (int)FMath.Round((float)texture.Width / (float)tiles_x);
				int tile_height = (int)FMath.Round((float)texture.Height / (float)tiles_y);
				tile_height /= ScaleDivisor;
				tile_width /= ScaleDivisor;
				ColorBuffer color_buffer = new ColorBuffer(tile_width, tile_height, PixelFormat.Rgba);
				FrameBuffer frame_buffer = new FrameBuffer();
				frame_buffer.SetColorTarget(color_buffer);

				FrameBuffer old_frame_buffer = Director.Instance.GL.Context.GetFrameBuffer();
				Director.Instance.GL.Context.SetFrameBuffer(frame_buffer);

				//Director.Instance.GL.ModelMatrix.Push();
				//Director.Instance.GL.ModelMatrix.SetIdentity();

				ShaderProgram.SetAttributeBinding(0, "iPosition");
				ShaderProgram.SetAttributeBinding(1, "iUV");

				texture.SetWrap(TextureWrapMode.ClampToEdge);
				texture.SetFilter(TextureFilterMode.Linear);

				Director.Instance.GL.Context.SetTexture(0, texture);
				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
				Director.Instance.GL.Context.SetShaderProgram(ShaderProgram);
				
				ImageRect old_scissor = Director.Instance.GL.Context.GetScissor();
				ImageRect old_viewport = Director.Instance.GL.Context.GetViewport();
				
				Director.Instance.GL.Context.SetScissor(0, 0, tile_width, tile_height);
				Director.Instance.GL.Context.SetViewport(0, 0, tile_width, tile_height);
				
				Entry entry = new Entry();
				entry.TilesX = tiles_x;
				entry.TilesY = tiles_y;
				entry.TileWidth = tile_width;
				entry.TileHeight = tile_height;
				entry.Data = new List<List<byte>>();
				for (int i = 0; i < tiles_y * tiles_x; ++i)
				{
					entry.Data.Add(new List<byte>());
				}

				Vector2 uv_step = new Vector2(1.0f, 1.0f) / new Vector2(tiles_x, tiles_y);
				for (int y = 0; y < tiles_y; y++)
				{
					for (int x = 0; x < tiles_x; x++)
					{
						float uv_x0 = uv_step.X * (x + 0);
						float uv_x1 = uv_step.X * (x + 1);
						float uv_y0 = uv_step.Y * (tiles_y - 1 - y + 0);
						float uv_y1 = uv_step.Y * (tiles_y - 1 - y + 1);

						VertexDataArray[0] = new VertexData() { position = new Vector2(-1.0f, -1.0f), uv = new Vector2(uv_x0, uv_y1) };
						VertexDataArray[1] = new VertexData() { position = new Vector2(-1.0f, +1.0f), uv = new Vector2(uv_x0, uv_y0) };
						VertexDataArray[2] = new VertexData() { position = new Vector2(+1.0f, +1.0f), uv = new Vector2(uv_x1, uv_y0) };
						VertexDataArray[3] = new VertexData() { position = new Vector2(+1.0f, -1.0f), uv = new Vector2(uv_x1, uv_y1) };
						VertexBuffer.SetIndices(IndexDataArray, 0, 0, 4);
						VertexBuffer.SetVertices(VertexDataArray, 0, 0, 4);

						Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
						Director.Instance.GL.Context.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
						Director.Instance.GL.Context.Clear();
						Director.Instance.GL.Context.DrawArrays(DrawMode.TriangleFan, 0, 4);
						
						byte[] data = new byte[tile_width * tile_height * 4];
						Director.Instance.GL.Context.ReadPixels(data, PixelFormat.Rgba, 0, 0, tile_width, tile_height);
						
						List<byte> output = entry.Data[tiles_x * y + x];
						for (int i = 0; i < tile_width * tile_height * 4; ++i)
						{
							output.Add(data[i]);
						}
					}
				}

				Director.Instance.GL.Context.SetVertexBuffer(0, null);
				Director.Instance.GL.Context.SetShaderProgram(null);
				Director.Instance.GL.Context.SetFrameBuffer(old_frame_buffer);
				Director.Instance.GL.Context.SetScissor(old_scissor);
				Director.Instance.GL.Context.SetViewport(old_viewport);

				TileData[name] = entry;
			}

			public Texture2D MakeTexture(string name, int tile)
			{
				Entry entry = TileData[name];
				List<byte> data = entry.Data[tile];

				Texture2D texture = new Texture2D(entry.TileWidth, entry.TileHeight, false, PixelFormat.Rgba);
				texture.SetPixels(0, data.ToArray());
				return texture;
			}
		}

		public static Vector4 Color(byte r, byte g, byte b, byte a = 255)
		{
			return new Sce.Pss.Core.Vector4(
				(float)r / 255.0f,
				(float)g / 255.0f,
				(float)b / 255.0f,
				(float)a / 255.0f
			);
		}

		public static float SafeLength(this Vector2 self)
		{
			float length_squared = self.LengthSquared();

			if (length_squared < 0.00001f)
			{
				return 0.0f;
			}

			return (float)System.Math.Sqrt(length_squared);
		}
	}

	public class ScissorNode : Node
	{
		ImageRect desired;
		ImageRect restore;

		public ScissorNode(int x, int y, int w, int h)
		{
			desired = new ImageRect(x, y, w, h);
			restore = desired;
		}

		public override void PushTransform()
		{
			base.PushTransform();
			restore = Director.Instance.GL.Context.GetScissor();
			Director.Instance.GL.Context.SetScissor(desired);
		}

		public override void PopTransform()
		{
			Director.Instance.GL.Context.SetScissor(restore);
			base.PopTransform();
		}
	}

	public class King : Node
	{
		public int State;
		public SpriteTile CurrentSprite;
		public SpriteTile SpriteIdle;
		public SpriteTile SpriteBlink;
		public SpriteTile SpriteWatering1;
		public SpriteTile SpriteWatering2;
		public SpriteTile SpriteEmpty1;
		public SpriteTile SpriteEmpty2;
		public SpriteTile SpriteGameOver;

		public King()
		{
			SpriteIdle = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_chillin.png", 1, 1);
			SpriteBlink = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_chillin_blink.png", 1, 1);
			SpriteWatering1 = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_watering1.png", 1, 1);
			SpriteWatering2 = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_watering2.png", 1, 1);
			SpriteEmpty1 = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_empty1.png", 1, 1);
			SpriteEmpty2 = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_empty2.png", 1, 1);
			SpriteGameOver = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/king_game_over.png", 1, 1);

			Vector2 position = new Vector2(24.0f, 32.0f);
			SpriteIdle.Position = position;
			SpriteBlink.Position = position;
			SpriteWatering1.Position = position;
			SpriteWatering2.Position = position;
			SpriteEmpty1.Position = position;
			SpriteEmpty2.Position = position;
			SpriteGameOver.Position = position;

			AddChild(SpriteIdle);
			AddChild(SpriteBlink);
			AddChild(SpriteWatering1);
			AddChild(SpriteWatering2);
			AddChild(SpriteEmpty1);
			AddChild(SpriteEmpty2);
			AddChild(SpriteGameOver);

			ShowSprite(SpriteBlink);
		}

		public void DoBlinkSequence()
		{
			this.StopAllActions();

			var blink_speed = Game.Instance.Random.NextFloat() * 0.1f + 0.1f;
			var sequence = new Sequence();
			sequence.Add(new CallFunc(() => ShowSprite(SpriteIdle)));
			sequence.Add(new DelayTime() { Duration = 1.0f });

			if (Game.Instance.Random.Next() % 100 < 50)
			{
				sequence.Add(new CallFunc(() => ShowSprite(SpriteBlink)));
				sequence.Add(new DelayTime() { Duration = blink_speed });
				sequence.Add(new CallFunc(() => ShowSprite(SpriteIdle)));
				sequence.Add(new DelayTime() { Duration = blink_speed });
			}

			sequence.Add(new CallFunc(() => ShowSprite(SpriteBlink)));
			sequence.Add(new DelayTime() { Duration = blink_speed });
			sequence.Add(new CallFunc(() => ShowSprite(SpriteIdle)));
			sequence.Add(new DelayTime() { Duration = Game.Instance.Random.NextFloat() * 2.0f + 0.5f });
			sequence.Add(new CallFunc(() => DoBlinkSequence()));

			this.RunAction(sequence);
		}

		public void DoGameOverSequence()
		{
			this.StopAllActions();

			ShowSprite(SpriteGameOver);
		}

		public void DoWaterEmptySequence()
		{
			this.StopAllActions();

			var anim_speed = 0.2f;
			var sequence = new Sequence();

			for (int i = 0; i < 6; ++i)
			{
				sequence.Add(new CallFunc(() => ShowSprite(SpriteEmpty1)));
				sequence.Add(new DelayTime() { Duration = anim_speed });
				sequence.Add(new CallFunc(() => ShowSprite(SpriteEmpty2)));
				sequence.Add(new DelayTime() { Duration = anim_speed });
			}

			sequence.Add(new CallFunc(() => DoBlinkSequence()));
			this.RunAction(sequence);
		}

		public void DoWaterSequence()
		{
			this.StopAllActions();

			var anim_speed = 0.3f;
			var sequence = new Sequence();

			for (int i = 0; i < 1; ++i)
			{
				sequence.Add(new CallFunc(() => ShowSprite(SpriteWatering1)));
				sequence.Add(new DelayTime() { Duration = anim_speed });
				sequence.Add(new CallFunc(() => ShowSprite(SpriteWatering2)));
				sequence.Add(new DelayTime() { Duration = anim_speed });
			}

			sequence.Add(new CallFunc(() => { if (Game.Instance.Board.WaterMode) DoWaterSequence(); else DoWaterEmptySequence(); }));

			this.RunAction(sequence);
		}

		public void ShowSprite(SpriteTile sprite)
		{
			CurrentSprite = sprite;

			SpriteIdle.Visible = false;
			SpriteBlink.Visible = false;
			SpriteWatering1.Visible = false;
			SpriteWatering2.Visible = false;
			SpriteEmpty1.Visible = false;
			SpriteEmpty2.Visible = false;
			SpriteGameOver.Visible = false;

			CurrentSprite.Visible = true;
		}
	};

	public class CrystalBlinker : Node
	{
		int BlinkState;
		int BlinkStateTransitionCountdown;

		Crystal BlinkCrystal;

		public void Tick(float dt)
		{
			if (Game.Instance.Board.IsBoardLocked())
				return;

			// 0: blink-close
			// 1: blink-open
			// 2: delay
			switch (BlinkState)
			{
				case 0:
					{
						BlinkStateTransitionCountdown -= 1;
						if (BlinkStateTransitionCountdown > 0)
							return;

						if (BlinkCrystal == null)
						{
							BlinkCrystal = Game.Instance.Board.GetCrystalAtTile(
								Game.Instance.Random.Next() % Game.Instance.Board.BoardSize.X,
								Game.Instance.Random.Next() % Game.Instance.Board.BoardSize.Y
							);
						}

						if (BlinkCrystal.SubType != 2)
						{
							BlinkCrystal = null;
							return;
						}

						BlinkCrystal.Sprite.TileIndex2D = new Vector2i(3, BlinkCrystal.Sprite.TileIndex2D.Y);
						BlinkStateTransitionCountdown = Game.Instance.Random.Next() % 20 + 20;
						BlinkState += 1;
						break;
					}

				case 1:
					{
						BlinkStateTransitionCountdown -= 1;
						if (BlinkStateTransitionCountdown > 0)
						{
							return;
						}

						BlinkCrystal.Sprite.TileIndex2D = new Vector2i(BlinkCrystal.SubType, BlinkCrystal.Sprite.TileIndex2D.Y);
						BlinkStateTransitionCountdown = Game.Instance.Random.Next() % 20 + 20;
						BlinkState -= 1;
						BlinkCrystal = null;
						break;
					}
			}
		}
	}

	public class Crystal : Node
	{
		public int Type;
		public int SubType;
		public SpriteTile Sprite;
		public int FallQueue;
		public int ShuffleSteps;

		// fall anim
		// disappear anim
		// swap swap anim
		
		public Crystal()
		{
			Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/crystals.png", 5, 7);
			Sprite.Pivot = new Vector2(Board.CrystalSize);
			SetType(0, 0);
		}

		public void RandomizeType()
		{
			var type = Game.Instance.Random.Next() % 7;
			var subtype = Game.Instance.Random.Next() % 3;
			SetType(type, subtype);
		}

		public void SetType(int type, int subtype)
		{
			Type = type;
			SubType = subtype;
			Sprite.TileIndex2D = new Vector2i(subtype, type);
		}

		public void MoveTo(Vector2 position, float duration)
		{
			var move_action = new MoveTo(position, duration) {
				Set = value => { Sprite.Quad.T = value; },
				Get = () => Sprite.Quad.T
			};

			Game.Instance.GameScene.RunAction(move_action);
		}

		public void FailMoveTo(Vector2 position, float duration)
		{
			var move_action = new MoveTo(position, duration) {
				Set = value => { Sprite.Quad.T = value; },
				Get = () => Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.Impulse(t, 1.0f)
			};

			Game.Instance.GameScene.RunAction(move_action);
		}

		public void ScaleTo(float scale, float duration)
		{
			var scale_action = new ScaleTo(new Vector2(scale, scale), duration) {
				Set = value => { Sprite.Quad.S = value; },
				Get = () => Sprite.Quad.S
			};
			
			Game.Instance.GameScene.RunAction(scale_action);
		}

		public void QueueFallAction()
		{
			FallQueue += 1;
		}

		private void AddFallActionInternal()
		{
			const float SingleFallDuration = 0.15f;

			var duration = SingleFallDuration * FallQueue;
			var distance = -Board.CrystalSize * FallQueue;

			var sequence = new Sequence();
			var move_action = new MoveBy(new Vector2(0.0f, distance), duration) {
				Set = value => { Sprite.Quad.T = value; },
				Get = () => Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear(t)
			};

			Game.Instance.Board.LockBoard();
			//sequence.Add(new CallFunc(Game.Instance.Board.LockBoard));
			sequence.Add(move_action);
			sequence.Add(new CallFunc(CheckFallActionQueue));
			sequence.Add(new CallFunc(Game.Instance.Board.UnlockBoard));
			sequence.Add(new CallFunc(() => Support.SoundSystem.Instance.Play("veggie_land.wav")));

			RunAction(sequence);

			FallQueue = 0;
		}

		public void CheckFallActionQueue()
		{
			if (ShuffleSteps > 0)
			{
				var sequence = new Sequence();
				var scale_action = new ScaleTo(new Vector2(Sprite.Quad.S.X, 0.0f), 0.0f) {
					Set = value => { Sprite.Quad.S = value; },
					Get = () => Sprite.Quad.S
				};
				var move_action = new MoveTo(Sprite.Quad.T + new Vector2(0.0f, Board.CrystalSize * ShuffleSteps), 0.0f) {
					Set = value => { Sprite.Quad.T = value; },
					Get = () => Sprite.Quad.T
				};

				Game.Instance.Board.LockBoard();
				//sequence.Add(new CallFunc(Game.Instance.Board.LockBoard));
				//sequence.Add(scale_action);
				int local_shuffle_steps = ShuffleSteps;
				//sequence.Add(move_action);
				sequence.Add(new CallFunc(() => {
					Sprite.Quad.T += new Vector2(0.0f, Board.CrystalSize * local_shuffle_steps);
					RandomizeType();
					Sprite.Quad.S = new Vector2(Board.CrystalSize);
					CheckFallActionQueue();
				}));

				sequence.Add(new CallFunc(Game.Instance.Board.UnlockBoard));

				Vector2 position = Sprite.Quad.Center + Vector2.UnitX * -8.0f;
				int count = 4 + SubType * 3 + Game.Instance.Random.Next() % 4;
				float scale = 1.0f + SubType * 0.5f;
				Game.Instance.Board.ParticleEffectsManager.AddParticlesBurst(count, position, Vector2.UnitY * 1.5f, GetTypeColor(), 2.0f, scale);

				//Sprite.Quad.T += new Vector2(0.0f, Board.CrystalSize * ShuffleSteps);

				this.RunAction(sequence);

				ShuffleSteps = 0;

				return;
			}

			if (FallQueue > 0)
			{
				AddFallActionInternal();
				return;
			}
		}

		public Vector4 GetTypeColor()
		{
			switch (Type)
			{
				case 0: return Support.Color(116, 218, 36, 255);
				case 1: return Support.Color(60, 109, 221, 255);
				case 2: return Support.Color(113, 23, 216, 255);
				case 3: return Support.Color(255, 41, 180, 255);
				case 4: return Support.Color(11, 39, 171, 255);
				case 5: return Support.Color(219, 189, 48, 255);
				case 6: return Support.Color(216, 23, 25, 255);
			}

			return Support.Color(255, 0, 0, 255);
		}
	}

	public class Board : Node
	{
		public static float CrystalSize = 64.0f;
		public static int WaterLimit = 10000;

		public Vector2 CrystalsOffset;
		public Vector2i BoardSize;
		public SpriteList SpriteList;
		public Crystal SelectedCrystal;
		public List<Crystal> Crystals;
		public SpriteTile Background;
		public SpriteTile CrystalHighlight;
		public bool WasTouch;
		public bool IsTouch;
		public Vector2 TouchStart;
		public int BoardLockCount;
		public int BoardLockTimerCountdown;
		public int ComboCount;
		public ScissorNode ScissorNode;
		public int ScoreValue;
		public Label ScoreLabel;
		public int WaterValue;
		public SpriteTile WaterTileBackground;
		public SpriteTile WaterTile;
		public Vector2 WaterTileBaseScale;
		public Support.ParticleEffectsManager ParticleEffectsManager;
		public bool WaterMode;
		public Label WaterTouchLabel;
		public SpriteTile WaterCan;
		public SpriteTile WaterCanPanel;
		public int WaterNoTouchCounter;
		public Crystal HintCrystal;
		public Sequence HintSequence;
		public int HintDelayCounter;
		public Vector2 HintRestorePosition;
		public CrystalBlinker CrystalBlinker;
		public King King;

		public Board(int size_x, int size_y)
		{
			CrystalsOffset = new Vector2(428.0f, 16.0f);
			BoardSize = new Vector2i(size_x, size_y);
			Background = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/board.png", 1, 1);
			CrystalHighlight = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/crystal_highlight.png", 1, 1);
			SpriteList = Support.TiledSpriteListFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/crystals.png", 5, 7);
			Crystals = new List<Crystal>();

			for (int y = 0; y < size_y; ++y)
			{
				for (int x = 0; x < size_x; ++x)
				{
					var crystal = new Crystal();
					Crystals.Add(crystal);
					SpriteList.AddChild(crystal.Sprite);
				}
			}

			ScissorNode = new ScissorNode((int)CrystalsOffset.X, (int)CrystalsOffset.Y, 512, 512);
			ScissorNode.AddChild(SpriteList);

			ScoreLabel = new Label() { Text = "0" };
			var font = new Font("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/IndieFlower.ttf", 96, FontStyle.Regular);
			var map = new FontMap(font);
			ScoreLabel.FontMap = map;
			ScoreLabel.Position = new Vector2(150.0f, 434.0f);
			ScoreLabel.Color = Support.Color(0, 0, 0, 255);
			ScoreLabel.HeightScale = 1.0f;

			WaterTouchLabel = new Label() { Text = "Touch!" };
			font = new Font("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/IndieFlower.ttf", 72, FontStyle.Bold);
			map = new FontMap(font);
			WaterTouchLabel.FontMap = map;
			WaterTouchLabel.Position = new Vector2(540.0f, 260.0f);
			WaterTouchLabel.Color = Support.Color(216, 216, 216, 255);
			WaterTouchLabel.HeightScale = 1.0f;
			WaterTouchLabel.Visible = false;

			WaterTile = Support.UnicolorSprite("Water", 8, 32, 192, 96);
			WaterTile.Position = new Vector2(293.0f, 49.0f);
			WaterTileBaseScale = new Vector2(220.0f / 960.0f, 360.0f / 544.0f) * 32.0f;
			WaterTile.Scale = WaterTileBaseScale;
			WaterTile.Quad.Centering(new Vector2(0.0f, 0.0f));

			WaterTileBackground = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/water_meter_bg.png", 1, 1);
			WaterTileBackground.Position = WaterTile.Position;
			WaterTile.Scale = WaterTileBaseScale;

			WaterCan = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/watering_can.png", 1, 1);
			WaterCan.Position = new Vector2(770.0f, 300.0f);
			WaterCan.Quad.Centering(new Vector2(0.5f, 0.5f));
			WaterCan.Visible = false;

			WaterCanPanel = Support.UnicolorSprite("WaterCanPanel", 32, 32, 32, 160);
			WaterCanPanel.Position = new Vector2(520.0f, 240.0f);
			WaterCanPanel.Scale = new Vector2(590.0f / 960.0f, 140.0f / 544.0f) * 32.0f;
			WaterCanPanel.Visible = false;

			King = new King();

			AddChild(WaterTileBackground);
			AddChild(WaterTile);
			AddChild(Background);
			AddChild(ScissorNode);
			AddChild(CrystalHighlight);

			AddChild(ScoreLabel);
			AddChild(WaterCanPanel);
			AddChild(WaterTouchLabel);
			AddChild(WaterCan);
			AddChild(King);

			ParticleEffectsManager = new Support.ParticleEffectsManager();
			AddChild(ParticleEffectsManager);

			CrystalBlinker = new CrystalBlinker();
		}

		public void Refresh(int level)
		{
			ScoreValue = 0;
			ScoreLabel.Text= "0";

			BoardLockCount = 0;
			WaterMode = false;
			WaterValue = 0;
			AddWater(WaterLimit / 2);

			King.DoBlinkSequence();

			SelectedCrystal = null;
			CrystalHighlight.Visible = false;

			// generate one row at a time
			// if crystal in row /below/
			for (int y = 0; y < BoardSize.Y; ++y)
			{
				for (int x = 0; x < BoardSize.X; ++x)
				{
					var crystal = Crystals[y * BoardSize.X + x];
					crystal.RandomizeType();

					// avoid some starting 3x blocks
					var left = GetCrystalAtTile(x - 1, y);
					var leftleft = GetCrystalAtTile(x - 2, y);
					var down = GetCrystalAtTile(x, y - 1);
					for (int i = 0; i < 8; ++i)
					{
						if (left != null && leftleft != null)
						{
							if (crystal.Type == left.Type && crystal.Type == leftleft.Type)
								crystal.RandomizeType();
						}

						if (down != null && crystal.Type == down.Type)
							crystal.RandomizeType();
					}

					crystal.Sprite.Quad.T = GetPositionAtTile(x, y);
					crystal.Sprite.Quad.S = new Vector2(crystal.Sprite.TextureInfo.TextureSizef.X / 5.0f, crystal.Sprite.TextureInfo.TextureSizef.Y / 7.0f);
					crystal.Visible = true;
				}
			}

			// perform a lock/unlock so that game over conditions/etc get correctly checked before first move
			LockBoard();
			UnlockBoard();

			King.DoBlinkSequence();
		}

		public Vector2 GetPositionAtTile(int x, int y)
		{
			return
				new Vector2(512.0f / (float)BoardSize.X * x, 512.0f / (float)BoardSize.Y * y) + 
				CrystalsOffset;
		}

		public Vector2i GetTileAtPosition(Vector2 position)
		{
			position -= CrystalsOffset;
			position /= CrystalSize;

			int x = (int)position.X;
			int y = (int)position.Y;

			x = System.Math.Max(0, System.Math.Min(BoardSize.X - 1, x));
			y = System.Math.Max(0, System.Math.Min(BoardSize.Y - 1, y));
			
			return new Vector2i(x, y);
		}

		public Crystal GetCrystalAtPosition(Vector2 position)
		{
			position -= CrystalsOffset;
			position /= CrystalSize;
			
			int x = (int)position.X;
			int y = (int)position.Y;

			return GetCrystalAtTile(x, y);
		}

		public Crystal GetCrystalAtTile(int x, int y)
		{
			if (x < 0 || x >= BoardSize.X)
			{
				return null;
			}

			if (y < 0 || y >= BoardSize.Y)
			{
				return null;
			}

			var crystal = Crystals[y * BoardSize.X + x];
			return crystal;
		}

		public void UpdateHint()
		{
			HintDelayCounter++;

			if (HintCrystal == null)
			{
				return;
			}

			if (HintDelayCounter <= 0)
			{
				return;
			}

			const int show_hint_time = 60 * 7;

			if (HintDelayCounter % show_hint_time != 0)
			{
				return;
			}

			if (HintSequence != null && HintSequence.IsRunning)
			{
				(HintSequence.Target as Crystal).Sprite.Quad.T = HintRestorePosition;
				HintSequence.Stop();
				HintSequence = null;
			}

			var movement = Vector2.UnitY * 4.0f;
			var src = HintCrystal.Sprite.Quad.T;
			var dst = HintCrystal.Sprite.Quad.T + movement;

			HintRestorePosition = src;

			var move_action_0 = new MoveTo(dst, 0.2f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.PowEaseIn(t, 2.0f),
			};

			var move_action_1 = new MoveTo(src, 0.3f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear(t),
			};

			var move_action_2 = new MoveTo(dst, 0.2f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.PowEaseIn(t, 2.0f),
			};

			var move_action_3 = new MoveTo(src, 0.3f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear(t),
			};

			var move_action_4 = new MoveTo(dst, 0.2f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.PowEaseIn(t, 2.0f),
			};

			var move_action_5 = new MoveTo(src, 0.3f) {
				Set = value => { HintCrystal.Sprite.Quad.T = value; },
				Get = () => HintCrystal.Sprite.Quad.T,
				Tween = (t) => Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear(t),
			};

			var restore_action_6 = new CallFunc(() => HintCrystal.Sprite.Quad.T = src);

			HintSequence = new Sequence();
			HintSequence.Add(move_action_0);
			HintSequence.Add(move_action_1);
			HintSequence.Add(move_action_2);
			HintSequence.Add(move_action_3);
			HintSequence.Add(move_action_4);
			HintSequence.Add(move_action_5);
			HintSequence.Add(restore_action_6);
			HintCrystal.RunAction(HintSequence);
		}

		public void LockBoard()
		{
			BoardLockCount += 1;
			HintDelayCounter = -100000000;
		}

		public Sce.Pss.HighLevel.GameEngine2D.ActionBase MakeGameOverDestroyCrystalAction(int x, int y)
		{
			var action = new CallFunc(() => {
				var crystal = GetCrystalAtTile(x, y);
				for (int i = 0; i < 7; ++i)
				{
					crystal.QueueFallAction();
					crystal.ShuffleSteps += 1;
				}

				crystal.Visible = false;
			});
			return action;
		}

		public Sce.Pss.HighLevel.GameEngine2D.ActionBase MakeExplodeCrystalAction(int x, int y)
		{
			var action = new CallFunc(() => {
				var crystal = GetCrystalAtTile(x, y);
				Vector2 position = crystal.Sprite.Quad.Center + Vector2.UnitX * -8.0f;
				int count = 4 + crystal.SubType * 3 + Game.Instance.Random.Next() % 4;
				float scale = 1.0f + crystal.SubType * 0.5f;
				Game.Instance.Board.ParticleEffectsManager.AddParticlesBurst(count, position, Vector2.UnitY * 1.5f, crystal.GetTypeColor(), 2.0f, scale);
				// NOTE: these dont work on spritelist items
				//crystal.Visible = false;
				//crystal.Sprite.Visible = false;
				crystal.Sprite.Quad.S = Vector2.Zero;
			});
			return action;
		}

		public void GameOver()
		{
			var destroy_crystals = new Sequence();

			var indices = new List<Vector2i>();
			for (int y = 0; y < BoardSize.Y; ++y)
			{
				for (int x = 0; x < BoardSize.X; ++x)
				{
					indices.Add(new Vector2i(x, y));
				}
			}

			for (int i = 0; i < indices.Count; ++i)
			{
				int ia = Game.Instance.Random.Next() % indices.Count;
				int ib = Game.Instance.Random.Next() % indices.Count;
				Vector2i tmp = indices[ia];
				indices[ia] = indices[ib];
				indices[ib] = tmp;
			}

			for (int i = 0; i < indices.Count; ++i)
			{
				var tile = indices[i];
				var destroy = MakeExplodeCrystalAction(tile.X, tile.Y);
				destroy_crystals.Add(destroy);

				if (i % 4 == 0){
					destroy_crystals.Add(new DelayTime() { Duration = 0.0001f });
				}
			}
			//this.RunAction(destroy_crystals);

			var sequence_end = new Sequence();
			Game.Instance.Board.LockBoard();
			//sequence_end.Add(new CallFunc(LockBoard));
			sequence_end.Add(new CallFunc(King.DoGameOverSequence));
			sequence_end.Add(new CallFunc(() => Support.SoundSystem.Instance.Play("gameover.wav")));
			sequence_end.Add(destroy_crystals);
			sequence_end.Add(new DelayTime() { Duration = 1.0f });
			sequence_end.Add(new CallFunc(UnlockBoard));
			sequence_end.Add(new CallFunc(() => Game.Instance.StartTitle()));
			this.RunAction(sequence_end);
		}

		public void UnlockBoard()
		{
			BoardLockCount -= 1;
			if (BoardLockCount == 0)
			{
				BoardLockTimerCountdown = 4;
			}
		}

		public void StopHintCrystal()
		{
			if (HintSequence != null)
			{
				HintSequence.Stop();
				HintSequence = null;
				HintCrystal.Sprite.Quad.T = HintRestorePosition;
			}

			HintRestorePosition = Vector2.Zero;
			HintDelayCounter = -100000000;
		}

		public void SwapCrystals(Crystal a, Crystal b)
		{
			//var ap = a.Sprite.Quad.T;
			//var bp = b.Sprite.Quad.T;
			//a.Sprite.Quad.T = bp;
			//b.Sprite.Quad.T = ap;

			const float time = 0.5f;

			a.MoveTo(b.Sprite.Quad.T, time);
			b.MoveTo(a.Sprite.Quad.T, time);

			var sequence_swap = new Sequence();
			Game.Instance.Board.LockBoard();
			//sequence_swap.Add(new CallFunc(LockBoard));
			sequence_swap.Add(new DelayTime() { Duration = time });
			sequence_swap.Add(new CallFunc(() => SwapCrystalIndices(a, b)));
			sequence_swap.Add(new CallFunc(UnlockBoard));
			this.RunAction(sequence_swap);

			//SelectedCrystal = null;
			//RemoveChild(CrystalHighlight);
		}

		public void FailSwapCrystals(Crystal a, Crystal b)
		{
			const float time = 0.3f;

			//a.FailMoveTo(b.Sprite.Quad.T, time);
			//b.FailMoveTo(a.Sprite.Quad.T, time);

			var sequence_swap = new Sequence();
			Game.Instance.Board.LockBoard();
			//sequence_swap.Add(new CallFunc(LockBoard));
			sequence_swap.Add(new DelayTime() { Duration = time });
			//sequence_swap.Add(new CallFunc(() => SwapCrystalIndices(a, b)));
			sequence_swap.Add(new CallFunc(UnlockBoard));
			this.RunAction(sequence_swap);
			
			//SelectedCrystal = null;
			//RemoveChild(CrystalHighlight);
		}

		public void SwapCrystalIndices(Crystal a, Crystal b)
		{
			int ai = Crystals.FindIndex(x => x == a);
			int bi = Crystals.FindIndex(x => x == b);

			Crystals[ai] = b;
			Crystals[bi] = a;
		}

		public int CountChainH(int x, int y)
		{
			Crystal current = GetCrystalAtTile(x, y);

			int chain = 0;
			for (int h = x; h < BoardSize.X; ++h)
			{
				Crystal next = GetCrystalAtTile(h, y);	
				if (next.Type != current.Type)
				{
					break;
				}
				chain++;
			}

			return chain;
		}

		public int CountChainV(int x, int y)
		{
			Crystal current = GetCrystalAtTile(x, y);

			int chain = 0;
			for (int v = y; v < BoardSize.Y; ++v)
			{
				Crystal next = GetCrystalAtTile(x, v);
				if (next.Type != current.Type)
				{
					break;
				}
				chain++;
			}

			return chain;
		}

		public struct ChainEvent
		{
			public Vector2i tile;
			public int count;
			public int power;
		};

		public Vector2i FindPotentialChain()
		{
			// search horizontal pairs
			for (int y = 0; y < BoardSize.Y; ++y)
			{
				for (int x = 0; x < BoardSize.X; ++x)
				{
					// check for pair
					Crystal current = GetCrystalAtTile(x, y);
					Crystal previous = GetCrystalAtTile(x - 1, y);

					if (previous == null)
					{
						continue;
					}

					if (current.Type != previous.Type)
					{
						continue;
					}

					// found pair, search surrounding crystals
					Crystal match_left_up = GetCrystalAtTile(x - 2, y + 1);
					Crystal match_left_left = GetCrystalAtTile(x - 3, y);
					Crystal match_left_down = GetCrystalAtTile(x - 2, y - 1);

					if (match_left_up != null && match_left_up.Type == previous.Type)
					{
						return new Vector2i(x - 2, y + 1);
					}

					if (match_left_left != null && match_left_left.Type == previous.Type)
					{
						return new Vector2i(x - 3, y);
					}

					if (match_left_down != null && match_left_down.Type == previous.Type)
					{
						return new Vector2i(x - 2, y - 1);
					}

					// found pair, search surrounding crystals
					Crystal match_right_up = GetCrystalAtTile(x + 1, y + 1);
					Crystal match_right_left = GetCrystalAtTile(x + 2, y);
					Crystal match_right_down = GetCrystalAtTile(x + 1, y - 1);

					if (match_right_up != null && match_right_up.Type == current.Type)
					{
						return new Vector2i(x + 1, y + 1);
					}

					if (match_right_left != null && match_right_left.Type == current.Type)
					{
						return new Vector2i(x + 2, y);
					}

					if (match_right_down != null && match_right_down.Type == current.Type)
					{
						return new Vector2i(x + 1, y - 1);
					}
				}
			}

			// search vertical pairs
			for (int x = 0; x < BoardSize.X; ++x)
			{
				for (int y = 0; y < BoardSize.Y; ++y)
				{
					// check for pair
					Crystal current = GetCrystalAtTile(x, y);
					Crystal previous = GetCrystalAtTile(x, y - 1);

					if (previous == null)
					{
						continue;
					}

					if (current.Type != previous.Type)
					{
						continue;
					}

					// found pair, search surrounding crystals
					Crystal match_down_left = GetCrystalAtTile(x - 1, y - 2);
					Crystal match_down_down = GetCrystalAtTile(x, y - 3);
					Crystal match_down_right = GetCrystalAtTile(x + 1, y - 2);

					if (match_down_left != null && match_down_left.Type == previous.Type)
					{
						return new Vector2i(x - 1, y - 2);
					}

					if (match_down_down != null && match_down_down.Type == previous.Type)
					{
						return new Vector2i(x, y - 3);
					}

					if (match_down_right != null && match_down_right.Type == previous.Type)
					{
						return new Vector2i(x + 1, y - 2);
					}

					// found pair, search surrounding crystals
					Crystal match_up_left = GetCrystalAtTile(x - 1, y + 1);
					Crystal match_up_up = GetCrystalAtTile(x, y + 2);
					Crystal match_up_right = GetCrystalAtTile(x + 1, y + 1);

					if (match_up_left != null && match_up_left.Type == current.Type)
					{
						return new Vector2i(x - 1, y + 1);
					}

					if (match_up_up != null && match_up_up.Type == current.Type)
					{
						return new Vector2i(x, y + 2);
					}

					if (match_up_right != null && match_up_right.Type == current.Type)
					{
						return new Vector2i(x + 1, y + 1);
					}
				}
			}

			return new Vector2i(-1, -1);
		}

		public bool CheckBoardForChains(bool trigger_clear)
		{
			var chain_tiles = new List<Vector2i>();
			var chain_events = new List<ChainEvent>();

			for (int x = 0; x < BoardSize.X; ++x)
			{
				for (int y = 0; y < BoardSize.Y; )
				{
					int chain = CountChainV(x, y);	

					if (chain >= 3)
					{
						int power = 0;
						for (int i = 0; i < chain; ++i)
						{
							var c = GetCrystalAtTile(x, y + i);
							power += c.SubType + 1;	
						}

						chain_events.Add(new ChainEvent() { tile = new Vector2i(x, y), count = chain, power = power });
						for (int i = 0; i < chain; ++i)
						{
							chain_tiles.Add(new Vector2i(x, y + i));
						}
					}

					y += chain;
				}
			}

			for (int y = 0; y < BoardSize.Y; ++y)
			{
				for (int x = 0; x < BoardSize.X; )
				{
					int chain = CountChainH(x, y);	
					
					if (chain >= 3)
					{
						int power = 0;
						for (int i = 0; i < chain; ++i)
						{
							var c = GetCrystalAtTile(x + i, y);
							power += c.SubType + 1;	
						}

						chain_events.Add(new ChainEvent() { tile = new Vector2i(x, y), count = chain, power = power });
						for (int i = 0; i < chain; ++i)
						{
							chain_tiles.Add(new Vector2i(x + i, y));
						}
					}
					x += chain;
				}
			}

			if (!trigger_clear)
			{
				return chain_events.Count > 0;
			}

			if (chain_events.Count == 0)
			{
				ComboCount = 0;
			}
			else
			{
				ComboCount += 1;

				// match1.wav to match5.wav
				int combo_sound_index = System.Math.Max(1, System.Math.Min(ComboCount, 5));
				Support.SoundSystem.Instance.Play(String.Format("match{0}.wav", combo_sound_index));
			}

			for (int i = 0; i < chain_events.Count; ++i)
			{
				Vector2i tile = chain_events[i].tile;
				Vector2 position = GetCrystalAtTile(tile.X, tile.Y).Sprite.Quad.T;

				int count = chain_events[i].count;
				int power = chain_events[i].power;

				int score = count * count * power * power * ComboCount;
				int round = score - score % 10;

				AddScore(position, round);
			}

			chain_tiles = chain_tiles.Distinct().ToList();

			var columns = new List<List<Vector2i>>();
			for (int x = 0; x < BoardSize.X; ++x)
			//for (int x = 0; x < 1; ++x)
			{
				columns.Add(new List<Vector2i>());
				for (int i = 0; i < chain_tiles.Count; ++i)
				{
					if (chain_tiles[i].X == x)
					{
						columns[x].Add(chain_tiles[i]);
					}
				}
			}

			for (int i = 0; i < columns.Count; ++i)
			{
				columns[i] = columns[i].OrderBy(item => item.Y).ToList();
			}

			// bubble-swap each chained crystal in a given column upwards to the top,
			// adding a fall-down action to each bubble that gets shuffled
			for (int i = 0; i < columns.Count; ++i)
			{
				List<Vector2i> column = columns[i];
				for (int j = 0; j < column.Count(); ++j)
				{
					var crystal = GetCrystalAtTile(column[j].X, column[j].Y);
					int x = column[j].X;
					for (int y = column[j].Y; y < BoardSize.Y - 1; ++y)
					{
						var cleared_block = GetCrystalAtTile(x, y);
						var existing_block = GetCrystalAtTile(x, y + 1);

						SwapCrystalIndices(cleared_block, existing_block);

						// only the old block needs to fall in the shuffles here
						existing_block.QueueFallAction();

						// push only the new block up one to shuffle it above the top of the board
						cleared_block.ShuffleSteps += 1;
					}

					// column indices need to swap to reflect change (except last tile)
					if (j < column.Count - 1)
					{
						Vector2i tmp = column[j];
						column[j] = column[j + 1];
						column[j + 1] = tmp;
					}

					// reposition up one more block to go off the edge and fall back into the board
					crystal.QueueFallAction();

					// needs to be moved up one more
					crystal.ShuffleSteps += 1;
				}
			}
			
			for (int y = 0; y < BoardSize.Y; ++y)
			{
				for (int x = 0; x < BoardSize.X; ++x)
				{
					GetCrystalAtTile(x, y).CheckFallActionQueue();
				}
			}
			return chain_events.Count > 0;
		}

		public void AddScore(Vector2 position, int amount)
		{
			ScoreValue += amount;
			ScoreLabel.Text = ScoreValue.ToString();

			AddWater(amount);
		}

		public void EnterWaterMode()
		{
			LockBoard();
			WaterMode = true;
			WaterNoTouchCounter = 60 * 2;
			King.DoWaterSequence();
			Support.SoundSystem.Instance.Play("watermode.wav");
		}

		public void ExitWaterMode()
		{
			WaterNoTouchCounter = 0;
			WaterValue = 0;
			WaterMode = false;
			WaterTouchLabel.Visible = false;
			WaterCanPanel.Visible = false;
			Support.SoundSystem.Instance.Stop("watermode.wav");
			UnlockBoard();
		}

		public void AddWater(int amount)
		{
			WaterValue += amount;
			WaterValue = System.Math.Min(WaterValue, WaterLimit);
			float ratio = (float)WaterValue / (float)WaterLimit;
			WaterTile.Scale = WaterTileBaseScale * new Vector2(1.0f, ratio);
		}

		public void ClearChain(List<Vector2i> chained, int x, int y, int dx, int dy)
		{
			int step_x = dx > 0 ? 1 : 0;
			int step_y = dy > 0 ? 1 : 0;

			dx = System.Math.Min(dx, 1);
			dy = System.Math.Min(dy, 1);

			for (; y < y + dy; ++y)
			{
				for (; x < x + dx; ++x)
				{
					chained.Add(new Vector2i(x, y));
				}
			}
		}

		public bool IsValidAdjacent(Crystal a, Crystal b)
		{
			if (a == null || b == null)
			{
				return false;
			}

			Vector2i ta = GetTileAtPosition(a.Sprite.Quad.T);
			Vector2i tb = GetTileAtPosition(b.Sprite.Quad.T);
			Vector2i t = tb - ta;
			int difference = System.Math.Abs(t.X) + System.Math.Abs(t.Y);

			return difference == 1;
		}
		
		delegate bool CheckMakeChain(int x, int y, int type);

		public bool WillSwapMakeChain(Crystal a, Crystal b)
		{
			if (a == null || b == null)
			{
				return false;
			}

			SwapCrystalIndices(a, b);

			Vector2i tile_a = GetTileAtPosition(a.Sprite.Quad.T);	
			Vector2i tile_b = GetTileAtPosition(b.Sprite.Quad.T);	
			
			CheckMakeChain xf = (int x, int y, int type) => {
				var x0 = GetCrystalAtTile(x - 2, y);
				var x1 = GetCrystalAtTile(x - 1, y);
				var x3 = GetCrystalAtTile(x + 1, y);
				var x4 = GetCrystalAtTile(x + 2, y);
				
				if (x0 != null && x0.Type == type && x1 != null && x1.Type == type)
				{
					return true;
				}

				if (x1 != null && x1.Type == type && x3 != null && x3.Type == type)
				{
					return true;
				}

				if (x3 != null && x3.Type == type && x4 != null && x4.Type == type)
				{
					return true;
				}

				return false;
			};
			
			CheckMakeChain yf = (int x, int y, int type) => {
				var y0 = GetCrystalAtTile(x, y - 2);
				var y1 = GetCrystalAtTile(x, y - 1);
				var y3 = GetCrystalAtTile(x, y + 1);
				var y4 = GetCrystalAtTile(x, y + 2);
				
				if (y0 != null && y0.Type == type && y1 != null && y1.Type == type)
				{
					return true;
				}

				if (y1 != null && y1.Type == type && y3 != null && y3.Type == type)
				{
					return true;
				}

				if (y3 != null && y3.Type == type && y4 != null && y4.Type == type)
				{
					return true;
				}

				return false;
			};
				
			bool a_x_chain = xf(tile_b.X, tile_b.Y, a.Type);
			bool a_y_chain = yf(tile_b.X, tile_b.Y, a.Type);
			bool b_x_chain = xf(tile_a.X, tile_a.Y, b.Type);
			bool b_y_chain = yf(tile_a.X, tile_a.Y, b.Type);

			SwapCrystalIndices(b, a);

			return a_x_chain || a_y_chain || b_x_chain || b_y_chain;
		}

		public void EmitWaterParticles()
		{
			Random r = Game.Instance.Random;

			float water_ratio = (float)WaterValue / (float)WaterLimit;
			float chance = water_ratio * 0.10f;

			if (WaterMode)
			{
				chance += 0.25f;
			}

			if (r.NextFloat() > chance)
			{
				return;
			}

			Vector2 base_position = WaterTile.LocalToWorld(WaterTile.Quad.T);
			Vector2 emit_position = base_position +
				new Vector2(
					r.NextFloat() * 72.0f,
					r.NextFloat() * 240.0f * water_ratio
				);


			ParticleEffectsManager.AddParticleWater(
				emit_position, Vector2.UnitY * 0.0f, Support.Color(192, 192, 220, 96), 1.0f
			);
		}

		public void Tick(float dt)
		{
			//Director.Instance.DebugFlags |= DebugFlags.Navigate; // press left alt + mouse to navigate in 2d space
			//Director.Instance.DebugFlags |= DebugFlags.DrawGrid;
			//Director.Instance.DebugFlags |= DebugFlags.DrawContentWorldBounds;
			//Director.Instance.DebugFlags |= DebugFlags.DrawContentLocalBounds;
			//Director.Instance.DebugFlags |= DebugFlags.DrawTransform;
			//Director.Instance.DebugFlags |= DebugFlags.Log;

			BoardLockTimerCountdown = System.Math.Max(0, BoardLockTimerCountdown - 1);
			if (BoardLockTimerCountdown == 1)
			{
				BoardLockTimerCountdown = 0;

				WaterCan.Visible = false;

				Vector2i potential = FindPotentialChain();
				var hint_crystal = GetCrystalAtTile(potential.X, potential.Y);

				// debug
				//if (Game.Instance.Random.Next() % 4 == 0) { hint_crystal = null; }

				if (hint_crystal != null)
				{
					HintCrystal = hint_crystal;
					HintDelayCounter = 0;
				}
				else
				{
					// Game over!
					GameOver();
				}

				bool has_chains = CheckBoardForChains(true);

				// only enter water mode if there are no more chains to do first
				if (!has_chains)
				{
					if (WaterValue >= WaterLimit)
					{
						EnterWaterMode();
					}
				}
			}

			UpdateHint();

			// disabling for now, looks a bit weird and needs better animation
			//CrystalBlinker.Tick(dt);

			EmitWaterParticles();

			if (WaterMode)
			{
				Input2.TouchData touch = Input2.Touch00;

				WasTouch = false;
				IsTouch = false;

				AddWater(-15);

				if (touch.Down)
				{
					WaterNoTouchCounter = 0;
					WaterCan.Visible = false;
					WaterTouchLabel.Visible = false;
					WaterCanPanel.Visible = false;

					var normalized = touch.Pos;
					var world = Game.Instance.GameScene.Camera.NormalizedToWorld(normalized);
					var crystal = GetCrystalAtPosition(world);

					if (crystal != null)
					{
						AddWater(-20);

						Support.SoundSystem.Instance.PlayNoClobber("sprinklewater.wav");

						ParticleEffectsManager.AddParticlesBurst(
							7,
							world,
							Vector2.UnitY * 2.5f,
							Support.Color(48, 64, 192, 128),
							3.0f,
							2.0f
						);

						if (Game.Instance.Random.Next() % 100 < 25)
						{
							if (crystal.SubType < 2)
							{
								crystal.SetType(crystal.Type, crystal.SubType + 1);
								Support.SoundSystem.Instance.PlayNoClobber("veggie_upgrade.wav");
							}
						}

						if (Game.Instance.Random.Next() % 100 < 25)
						{
							Vector2i tile = new Vector2i(0, 0);
							switch (Game.Instance.Random.Next() % 8)
							{
								case 0: tile = new Vector2i(-1, -1); break;
								case 1: tile = new Vector2i(+0, -1); break;
								case 2: tile = new Vector2i(+1, -1); break;
								case 3: tile = new Vector2i(-1, +0); break;
								case 4: tile = new Vector2i(+1, +0); break;
								case 5: tile = new Vector2i(-1, +1); break;
								case 6: tile = new Vector2i(+0, +1); break;
								case 7: tile = new Vector2i(+1, +1); break;
							}

							var from = GetTileAtPosition(crystal.Sprite.Quad.T);
							var next = from + tile;
							var bonus = GetCrystalAtTile(next.X, next.Y);
							if (bonus != null)
							{
								if (bonus.SubType < 2)
								{
									bonus.SetType(bonus.Type, bonus.SubType + 1);
									Support.SoundSystem.Instance.PlayNoClobber("veggie_upgrade.wav");
								}
							}
						}
					}
				}
				else
				{
					WaterNoTouchCounter++;

					if (WaterNoTouchCounter > 60 * 2)
					{
						WaterCanPanel.Visible = true;
						WaterTouchLabel.Visible = true;
						WaterCan.Visible = true;
						WaterCan.Rotation = Vector2.UnitX.Rotate((WaterNoTouchCounter / 30) % 2 * -0.5f);
					}
				}

				if (WaterValue <= 0)
				{
					ExitWaterMode();
				}
			}

			UpdateInput(dt);
			//ParticleEffectsManager.Update(dt);
		}

		public bool IsBoardLocked()
		{
			return BoardLockCount > 0 || BoardLockTimerCountdown > 0;
		}

		public void UpdateInput(float dt)
		{
			Input2.TouchData touch = Input2.Touch00;

			WasTouch = IsTouch;
			IsTouch = touch.Down;

			if (IsBoardLocked() == false)
			{	
				var normalized = touch.Pos;
				var world = Game.Instance.GameScene.Camera.NormalizedToWorld(normalized);
				var crystal = GetCrystalAtPosition(world);
				var moved = TouchStart - world;
				var moved_distance = moved.SafeLength();
				
				Crystal a = null;
				Crystal b = null;

				if (crystal != null)
				{
					// used for hint
					//CrystalHint.Quad.T = crystal.Sprite.Quad.T;
				}

				// first touch frame
				if (IsTouch && !WasTouch)
				{
					if (SelectedCrystal == null)
					{
						if (crystal != null)
						{
							TouchStart = world;
							SelectedCrystal = crystal;
							CrystalHighlight.Quad.T = SelectedCrystal.Sprite.Quad.T;
							CrystalHighlight.Visible = true;
							Support.SoundSystem.Instance.Play("selectedcrystal.wav");
						}
					}
					else
					{
						if (SelectedCrystal == crystal || crystal == null)
						{
							SelectedCrystal = null;
							CrystalHighlight.Visible = false;
							Support.SoundSystem.Instance.Play("cannotmatch.wav");
						}
						else
						{
							if (IsValidAdjacent(SelectedCrystal, crystal))
							{
								a = SelectedCrystal;
								b = crystal;
							}
							else
							{
								Support.SoundSystem.Instance.Play("selectedcrystal.wav");
								TouchStart = world;
								SelectedCrystal = crystal;
								CrystalHighlight.Quad.T = SelectedCrystal.Sprite.Quad.T;
								CrystalHighlight.Visible = true;
							}
						}
					}
				}

				if (IsTouch && WasTouch)
				{
					if (SelectedCrystal != null)
					{
						if (moved_distance > CrystalSize * 0.25f)
						{
							int step_x = 0;
							int step_y = 0;
							
							if (moved.Abs().X > moved.Abs().Y)
								step_x = -System.Math.Sign(moved.X);
							else
								step_y = -System.Math.Sign(moved.Y);
								
							Vector2i tile = GetTileAtPosition(SelectedCrystal.Sprite.Quad.T);
							tile += new Vector2i(step_x, step_y);
							crystal = GetCrystalAtTile(tile.X, tile.Y);
							
							a = SelectedCrystal;
							b = crystal;
						}
					}
				}

				if (a != null && b != null && a != b)
				{
					// check adjacency rules
					if (IsValidAdjacent(a, b))
					{
						if (WillSwapMakeChain(a, b))
						{
							Support.SoundSystem.Instance.Play("swapcrystals.wav");
							StopHintCrystal();
							SwapCrystals(a, b);
							CrystalHighlight.Visible = false;
							SelectedCrystal = null;
						}
						else
						{
							Support.SoundSystem.Instance.Play("cannotmatch.wav");
							FailSwapCrystals(a, b);
							CrystalHighlight.Visible = false;
							SelectedCrystal = null;
						}
					}
				}
			}
		}
	}

	public class NoCleanupScene : Scene
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnExit()
		{
			StopAllActions();
		}
	}

	public class Game
	{
		public static Game Instance;
		
		public Random Random = new Random();
		public NoCleanupScene TitleScene;
		public NoCleanupScene GameScene;

		public Board Board;
		public SpriteTile Title;

		public Game()
		{
		}

		public void Initialize()
		{
			TitleScene = new NoCleanupScene();
			Title = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/PuzzleGameDemo/assets/veggie_royale_title.png", 1, 1);
			TitleScene.AddChild(Title);

			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);
			Camera2D title_camera = TitleScene.Camera as Camera2D;
			title_camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, ideal_screen_size / 2.0f);

			GameScene = new NoCleanupScene();
			Board = new Board(8, 8);
			GameScene.AddChild(Board);

			Director.Instance.RunWithScene(new Scene(),true);

			// force tick so the scene is set
			Director.Instance.Update();
			
			StartTitle();
		}

		public void StartTitle()
		{
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(GameScene, TickGame);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(TitleScene, TickTitle, 0.0f, false);

			var transition = new TransitionSolidFade(TitleScene) { PreviousScene = Director.Instance.CurrentScene, Duration = 1.5f, Tween = Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear	 };
			Director.Instance.ReplaceScene(transition);
		}

		public void TickTitle(float dt)
		{
			// wait for transition
			if (Director.Instance.CurrentScene != TitleScene)
			{
				return;
			}

			Support.MusicSystem.Instance.PlayNoClobber("title.mp3");

			Input2.TouchData touch = Input2.Touch00;
			if (touch.Down)
			{
				StartGame();
			}
		}

		public void StartGame()
		{
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(TitleScene, TickTitle);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(GameScene, TickGame, 0.0f, false);

			var transition = new TransitionSolidFade(GameScene) { PreviousScene = Director.Instance.CurrentScene, Duration = 1.5f, Tween = Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear };
			Director.Instance.ReplaceScene(transition);

			Board.Refresh(0);

			Support.SoundSystem.Instance.Play("startgame.wav");
		}

		public void TickGame(float dt)
		{
			Support.MusicSystem.Instance.PlayNoClobber("music_ingame.mp3");

			Board.Tick(dt);

		}
	}

	public class AppMain
	{
		public static void Main(string[] args)
		{
			Initialize();

			while (true) {
				SystemEvents.CheckEvents();
				Update();
				Render();
			}

//			Director.Terminate();
		}

		public static void Initialize()
		{
			Director.Initialize();
			Game.Instance = new Game();
			Game.Instance.Initialize();
			
			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);
			Camera2D camera = Game.Instance.GameScene.Camera as Camera2D;
			camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, ideal_screen_size / 2.0f);
		}

		public static void Update()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData(0);

			//Director.Instance.GL.SetBlendMode(BlendFuncMode.Normal);
			Director.Instance.Update();
			//Game.Instance.FrameUpdate();
		}

		public static void Render()
		{
			Director.Instance.Render();

			// Clear the screen
			//graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			//graphics.Clear();

			// Present the screen
			Sce.Pss.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
			Director.Instance.PostSwap();
		}
	}
}
