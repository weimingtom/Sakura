
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Reflection;

using Sce.Pss.Core;
using Sce.Pss.Core.Audio;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Input;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace SirAwesome
{
    public class Layer
    : Node
    {
    }

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

        public static void PrecacheTiledSprite(string filename, int x, int y)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
		}
			

        public static Sce.Pss.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
	            TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
            var tex = TextureCache[filename];
            var info = TextureInfoCache[filename];

   //       Vector2i tilesize=new Vector2i(256,256);
   //       if ( info.TextureSize )
   //       {
   //       }

            var result = new Sce.Pss.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };
            
			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);
//          result.Quad.S = info.TextureSizef;
			
			// DEBUG: testing for current assets
			result.Scale = new Vector2(1.0f);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
            return result;
        }
      
        public static Sce.Pss.HighLevel.GameEngine2D.SpriteTile UnicolorSprite(string name, byte r, byte g, byte b, byte a)
        {
        	uint color = (uint)(a << 24 | b << 16 | g << 8 | r);
				
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
			
//			sprite.TileIndex2D = new Vector2i(current % x, current / x);
            sprite.TileIndex1D = current;
			
			return current;
		}
		
		public static void SetTile(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite, int n)
		{
//			int x = sprite.TextureInfo.NumTiles.X;
//			int y = sprite.TextureInfo.NumTiles.Y;
//			sprite.TileIndex2D = new Vector2i(n % x, n / x);
            sprite.TileIndex1D = n;
		}
		
		public static int GetTileIndex(Sce.Pss.HighLevel.GameEngine2D.SpriteTile sprite)
		{
//			if (sprite.TextureInfo.NumTiles.X <= 1 &&
//				sprite.TextureInfo.NumTiles.Y <= 1)
//				return 0;
//				
//			return sprite.TileIndex2D.Y * sprite.TextureInfo.NumTiles.X + sprite.TileIndex2D.X;
            return sprite.TileIndex1D;
		}
		
		public class AnimationAction
			: Sce.Pss.HighLevel.GameEngine2D.ActionBase
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
			public static SoundSystem Instance = new SoundSystem("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sounds/");

			public string AssetsPrefix;
			public Dictionary<string, SoundPlayer> SoundDatabase;

			public SoundSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				SoundDatabase = new Dictionary<string, SoundPlayer>();
			}

			public void CheckCache(string name)
			{
				if (SoundDatabase.ContainsKey(name))
					return;

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
				if (SoundDatabase[name].Status == SoundStatus.Playing)
					return;
				SoundDatabase[name].Play();
			}
		}
		
		public class MusicSystem
		{
			public static MusicSystem Instance = new MusicSystem("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sounds/");

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
						return;
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
			public VertexBuffer VertexBuffer;
			public VertexData[] VertexDataArray;
			public ushort[] IndexDataArray;
			public ShaderProgram ShaderProgram;
			public Texture2D ParticleDotTexture;
			
            ImmediateModeQuads< VertexData > imm_quads;
            int max_particles { get { return 768; } }

			public ParticleEffectsManager()
			{
				Particles = new List<Particle>();
				for (int i = 0; i < max_particles; ++i)
					Particles.Add(new Particle());
					
				ShaderProgram = new ShaderProgram("/Application/Sample/GameEngine2D/ActionGameDemo/assets/pfx.cgx");
				ParticleDotTexture = new Texture2D("/Application/Sample/GameEngine2D/ActionGameDemo/assets/particle_dot.png", false);
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
				
				AdHocDraw += this.DrawParticles;

                imm_quads = new ImmediateModeQuads< VertexData >( Director.Instance.GL, (uint)max_particles, VertexFormat.Float2, VertexFormat.Float2, VertexFormat.Float4 );
			}
			
			public void Tick(float dt)
			{
				float fullness = (float)ActiveParticles / (float)Particles.Count;
				float life_speed = fullness;
				
				dt = 1.0f / 60.0f;
				
				for (int i = 0; i < ActiveParticles; ++i)
				{
					Particle p = Particles[i];
					p.position += p.velocity;
					p.velocity += new Vector2(0.0f, p.gravity * -0.5f);
					p.velocity *= p.friction;
					p.time += dt;
					p.time += dt * fullness;
					p.size += p.size_delta;
					
					if (p.position.Y < Game.Instance.FloorHeight)
					{
						p.velocity.Y *= -0.5f;
						p.velocity.X *= 0.75f;
						p.position += p.velocity;
					}
				}
				
				for (int i = 0; i < ActiveParticles; ++i)
				{
					Particle p = Particles[i];
					if (p.time < p.lifetime)
						continue;
						
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
						new VertexData() { position = p.position + new Vector2(0, 0), uv = new Vector2(0, 0), color = p.color},
						new VertexData() { position = p.position + new Vector2(p.size.X, 0), uv = new Vector2(1, 0), color = p.color},
						new VertexData() { position = p.position + new Vector2(0, p.size.Y), uv = new Vector2(0, 1), color = p.color},
						new VertexData() { position = p.position + new Vector2(p.size.X, p.size.Y), uv = new Vector2(1, 1), color = p.color} );

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
			
			public void AddParticlesBurstRandomy(int count, Vector2 position, Vector2 velocity, Vector4 color, float jitter = 0.0f, float scale_multiplier = 1.0f)
			{
				for (int i = 0; i < count; ++i)
				{
					Vector2 p = position + Game.Instance.Random.NextVector2() * jitter * (1.0f + Game.Instance.Random.NextFloat() * 0.5f);
					Vector2 v = velocity + Game.Instance.Random.NextVector2() * jitter * (1.0f + Game.Instance.Random.NextFloat() * 0.5f);
					AddParticle(p, v, color, scale_multiplier);
				}
			}
			
			public void AddParticlesCone(int count, Vector2 position, Vector2 velocity, Vector4 color, float spread, float scale_multiplier = 1.0f)
			{
				for (int i = 0; i < count; ++i)
				{
					Vector2 p = position + (velocity * (float)Game.Instance.Random.NextDouble());
					Vector2 v = velocity + velocity.Rotate((float)Game.Instance.Random.NextDouble() * spread);
					AddParticle(p, v, color, scale_multiplier);
				}
			}
			
			public void AddParticlesTile(string name, int tile_index, bool flip_u, Vector2 position, Vector2 velocity, float jitter = 0.0f, float scale_multiplier = 1.0f)
			{
				TextureTileMapManager.Entry entry = Game.Instance.TextureTileMaps.TileData[name];
				List<byte> tile_data = entry.Data[tile_index];
				Vector2 world_spacing = new Vector2(
					(float)Support.TextureTileMapManager.ScaleDivisor * 1.0f,
					(float)Support.TextureTileMapManager.ScaleDivisor * 1.0f
				);
				
				// debug
				// jitter = 0.0f;
				
				Vector2 topleft = new Vector2(entry.TileWidth, entry.TileHeight);
				topleft *= world_spacing;
				topleft *= -0.5f;
				
				for (int y = 0; y < entry.TileHeight; y++)
				{
					for (int x = 0; x < entry.TileWidth; x++)
					{
						int index = y * entry.TileWidth + x;
						byte r = tile_data[index * 4 + 0];
						byte g = tile_data[index * 4 + 1];
						byte b = tile_data[index * 4 + 2];
						byte a = tile_data[index * 4 + 3];
						
						if (a > 128)
						{
							int row = index / entry.TileWidth;
							int col = index % entry.TileWidth;
							
							if (flip_u)
								col = entry.TileWidth - col;
							
							Vector2 ofs = new Vector2(col, row) * world_spacing;
							Vector2 p = position + topleft + ofs + Game.Instance.Random.NextVector2() * jitter;
							Vector2 v = velocity + Game.Instance.Random.NextVector2() * jitter * 0.5f;
							Vector4 color = new Vector4(
								r / 255.0f,
								g / 255.0f,
								b / 255.0f,
								1.0f
							);
							
							AddParticle(p, v, color, scale_multiplier);
						}
					}
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
				p.size_delta = new Vector2(-0.15f);
				p.gravity = 1.0f;
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
				ShaderProgram = new ShaderProgram("/Application/Sample/GameEngine2D/ActionGameDemo/assets/offscreen.cgx");
				VertexDataArray = new VertexData[4];
				IndexDataArray = new ushort[4] { 0, 1, 2, 3 };
				
				TileData = new Dictionary<string, Entry>();
			}
			
			public void TestOffscreen(string name, Texture2D texture)
			{
				const int Width = 32;
				const int Height = 32;

				ImageRect old_scissor = Director.Instance.GL.Context.GetScissor();
				ImageRect old_viewport = Director.Instance.GL.Context.GetViewport();
				FrameBuffer old_frame_buffer = Director.Instance.GL.Context.GetFrameBuffer();

				ColorBuffer color_buffer = new ColorBuffer(Width, Height, PixelFormat.Rgba);
				FrameBuffer frame_buffer = new FrameBuffer();
				frame_buffer.SetColorTarget(color_buffer);
				
				Console.WriteLine("SetFrameBuffer(): enter");
				Director.Instance.GL.Context.SetFrameBuffer(frame_buffer);
				Console.WriteLine("SetFrameBuffer(): exit");

				ShaderProgram.SetAttributeBinding(0, "iPosition");
				ShaderProgram.SetAttributeBinding(1, "iUV");
				
				texture.SetWrap(TextureWrapMode.ClampToEdge);
				texture.SetFilter(TextureFilterMode.Linear);
				
				Director.Instance.GL.Context.SetTexture(0, texture);
				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
				Director.Instance.GL.Context.SetShaderProgram(ShaderProgram);
				Director.Instance.GL.Context.SetScissor(0, 0, Width, Height);
				Director.Instance.GL.Context.SetViewport(0, 0, Width, Height);

				float uv_x0 = 0.0f;
				float uv_x1 = 1.0f;
				float uv_y0 = 0.0f;
				float uv_y1 = 1.0f;
				
				VertexDataArray[0] = new VertexData() { position = new Vector2(-1.0f, -1.0f), uv = new Vector2(uv_x0, uv_y1) };
				VertexDataArray[1] = new VertexData() { position = new Vector2(-1.0f, +1.0f), uv = new Vector2(uv_x0, uv_y0) };
				VertexDataArray[2] = new VertexData() { position = new Vector2(+1.0f, +1.0f), uv = new Vector2(uv_x1, uv_y0) };
				VertexDataArray[3] = new VertexData() { position = new Vector2(+1.0f, -1.0f), uv = new Vector2(uv_x1, uv_y1) };

				VertexBuffer.SetIndices(IndexDataArray, 0, 0, 4);
				VertexBuffer.SetVertices(VertexDataArray, 0, 0, 4);

				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);

				Director.Instance.GL.Context.DrawArrays(DrawMode.TriangleFan, 0, 4);

				int count = Width * Height * 4;
				byte[] data = new byte[count];

				Console.WriteLine("ReadPixels(): enter");
				Director.Instance.GL.Context.ReadPixels(data, PixelFormat.Rgba, 0, 0, Width, Height);
				Console.WriteLine("ReadPixels(): exit");

				int nonzero = 0;
				int nonclear = 0;
				for (int i = 0; i < count; ++i)
				{
					if (data[i] != 0)
						nonzero++;
					if (data[i] != 0xfe)
						nonclear++;

					Console.Write("{0} ", data[i]);
					if (i % Width == 0)
						Console.WriteLine("");
				}

				Console.WriteLine("");
				Console.WriteLine("nonzero: {0}, nonclear: {1}", nonzero, nonclear);
						
				Director.Instance.GL.Context.SetVertexBuffer(0, null);
				Director.Instance.GL.Context.SetShaderProgram(null);
				Director.Instance.GL.Context.SetFrameBuffer(old_frame_buffer);
				Director.Instance.GL.Context.SetScissor(old_scissor);
				Director.Instance.GL.Context.SetViewport(old_viewport);

				Console.WriteLine("SwapBuffers(): enter");
				Director.Instance.GL.Context.SwapBuffers();
				Console.WriteLine("SwapBuffers(): exit");
				Thread.Sleep(250);
			}

			public void Add(string name, Texture2D texture, int tiles_x, int tiles_y)
			{
				int tile_width = (int)FMath.Round((float)texture.Width / (float)tiles_x);
				int tile_height = (int)FMath.Round((float)texture.Height / (float)tiles_y);
				tile_width /= ScaleDivisor;
				tile_height /= ScaleDivisor;
				tile_width = System.Math.Max(1, tile_width);
				tile_height = System.Math.Max(1, tile_height);

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
					entry.Data.Add(new List<byte>());
				
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
						
						Director.Instance.GL.Context.DrawArrays(DrawMode.TriangleFan, 0, 4);
						
						byte[] data = new byte[tile_width * tile_height * 4];

						// DEBUG: fill with visible memory pattern
						//for (int i = 0; i< tile_width * tile_height * 4; ++i)
							//data[i] = (byte)(i % (tile_width / 4));

						Director.Instance.GL.Context.ReadPixels(data, PixelFormat.Rgba, 0, 0, tile_width, tile_height);

						List<byte> output = entry.Data[tiles_x * y + x];
						for (int i = 0; i < tile_width * tile_height * 4; ++i)
							output.Add(data[i]);
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
	}

    static public class EntryPoint
    {
        public static void Run(string[] args)
        {
            Sce.Pss.HighLevel.GameEngine2D.Director.Initialize( 1024*4 );

			Game.Instance = new Game();
            var game = Game.Instance;
            
            Sce.Pss.HighLevel.GameEngine2D.Director.Instance.RunWithScene(game.Scene,true);
            
			Coin.InitializeCache();

			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            while (true)
            {
            	timer.Start();
                SystemEvents.CheckEvents();

                //Sce.Pss.HighLevel.GameEngine2D.Camera.DrawDefaultGrid(32.0f);

                Sce.Pss.HighLevel.GameEngine2D.Director.Instance.GL.SetBlendMode(BlendMode.Normal);
                Sce.Pss.HighLevel.GameEngine2D.Director.Instance.Update();
                Sce.Pss.HighLevel.GameEngine2D.Director.Instance.Render();
				
                game.FrameUpdate();
                
            	timer.Stop();
                long ms = timer.ElapsedMilliseconds;
                //Console.WriteLine("ms: {0}", (int)ms);
            	timer.Reset();

                Sce.Pss.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
                Sce.Pss.HighLevel.GameEngine2D.Director.Instance.PostSwap();
            }
        }
    }
    
	public class GameEntity
		: Sce.Pss.HighLevel.GameEngine2D.Node
	{
		public float Health { get; set; }
		public float InvincibleTime { get; set; }
		public int FrameCount { get; set; }
		
		public static Vector2 GetCollisionCenter(Node node)
		{
			Bounds2 bounds = new Bounds2();
			node.GetlContentLocalBounds(ref bounds);
			Vector2 center = node.LocalToWorld(bounds.Center);
			return center;
		}
		
		public List<EntityCollider.CollisionEntry> CollisionDatas;
		
		public GameEntity()
		{
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
			//AdHocDraw += this.DebugDraw;
			CollisionDatas = new List<EntityCollider.CollisionEntry>();
			Health = 1.0f;
		}
		
		public void DebugDraw()
		{
			foreach (EntityCollider.CollisionEntry c in CollisionDatas)
			{
				if (c.owner != null)
				{
					Director.Instance.GL.ModelMatrix.Push();
					Director.Instance.GL.ModelMatrix.SetIdentity();
					Director.Instance.DrawHelpers.DrawCircle(c.center(), c.radius(), 32);
					Director.Instance.GL.ModelMatrix.Pop();
				}
			}
		}
		
		public virtual void Tick(float dt)
		{
			FrameCount += 1;
			
			InvincibleTime -= dt;
			InvincibleTime = System.Math.Max(0.0f, InvincibleTime);
			
			if (InvincibleTime <= 0.0f)
			{
				foreach (EntityCollider.CollisionEntry c in CollisionDatas)
				{
					if (c.owner != null)
						Game.Instance.Collider.Add(c);
				}
			}
		} 
		
		public virtual void CollideTo(GameEntity owner, Node collider) { }
		public virtual void CollideFrom(GameEntity owner, Node collider) { }
		
		public void SpawnDamageParticles(Vector2 position, Vector2 source, float damage, Vector4 color)
		{
			//if (Health <= 0.0f)
				//return;
				
			Vector2 dir = position - source;
			if (dir.LengthSquared() > 0.0f)
				dir = dir.Normalize();
			dir *= 0.25f;
			int particles = (int)(damage * 4.0f);
			float jitter = 1.5f * damage;
			Game.Instance.ParticleEffects.AddParticlesBurst(particles, position, dir * damage * 4.0f + Vector2.UnitY * 2.0f, color, jitter, 1.0f);
		}
		
		public void DropCoinsWithAChanceOfHeart(Vector2 position, int count)
		{
			for (int i = 0; i < count; ++i)
				Coin.Spawn(position);
				
			// chance of heart
			if (Game.Instance.Random.NextFloat() < 0.06f)
			{
				var heart = new Heart() { Position = position };
				Game.Instance.World.AddChild(heart);
			}
		}
		
		public virtual void TakeDamage(float damage, Vector2? source)
		{
			Health -= damage;
			if (Health <= 0.0f)
				Die(source, damage);
		}
		
		public virtual void Die(Vector2? source, float damage)
		{
			Game.Instance.World.RemoveChild(this, true);
		}
	};
	
	public class EnemyWave
		: GameEntity
	{
		public List<EnemySpawner> Spawners { get; set; }
			
		public EnemyWave(float difficulty)
		{
			float inverse_difficulty = 1.0f - difficulty;
			Spawners = new List<EnemySpawner>();
			
			int count = 5 + Game.Instance.Random.Next() % 4;
			for (int i = 0; i < 8; ++i)
			{
				int type = Game.Instance.Random.Next() % 4;
				float spawn_base = inverse_difficulty * Game.Instance.Random.NextFloat() * 5.0f;
				
				switch (type)
				{
				case 0: spawn_base *= 0.3f; break;
				case 1: spawn_base *= 0.5f; break;
				case 2: spawn_base *= 0.7f; break;
				case 3: spawn_base *= 0.8f; break;
				}
				
				float spawn_rate = spawn_base + inverse_difficulty * 5.0f;
				
				int total = (int)(inverse_difficulty * 3.0f + Game.Instance.Random.NextFloat() * 4.0f);
				
				var spawner = new EnemySpawner() {
					SpawnCounter = Game.Instance.Random.NextFloat() * -i + 2.5f * -i,
					SpawnRate = spawn_rate,
					Type = type,
					Total = total,
					Position = new Vector2(-300.0f + Game.Instance.Random.NextFloat() * 1500.0f, 600.0f),
				};
				
				this.AddChild(spawner);
			}
		}
		
		public bool IsDone()
		{
			foreach (EnemySpawner spawner in Spawners)
			{
				if (spawner.Total != 0)
					return false;
			}
			
			return true;
		}
	};
	
	// TODO: side waves, falling waves, etc? more recognizable grouping of enemies, less uniform randomness
	public class EnemySideWave
		: GameEntity
	{
		float Difficulty;
		int Type;
		EnemySpawner Spawner;
		
		public EnemySideWave(int type, float difficulty)
		{
			float id = 1.0f - difficulty;
			Type = type;
			Difficulty = difficulty;
			Spawner = new EnemySpawner() {
				Position = new Vector2(Game.Instance.TitleCameraCenter.X + (Game.Instance.Random.NextBool() ? -700.0f : 700.0f), Game.Instance.FloorHeight),
				Type = type,
				SpawnRate = id * Game.Instance.Random.NextFloat() * 10.0f + id * (float)(Type + 1),
				SpawnCounter = Game.Instance.Random.NextFloat() * 5.0f,
				Total = (int)(Game.Instance.Random.NextFloat() * difficulty * 4.0f + 2.0f),
			};
			
			this.AddChild(Spawner);
		}
		
		public override void Tick(float dt)
		{
			base.Tick(dt);
			
			if (Spawner.Total == 0)
				Die(null, 0.0f);
		}
	}
	
    public class EnemySpawner
        : GameEntity
    {
    	public float SpawnCounter;
        public float SpawnRate;
        public int Type;
        public int Total;

        public override void Tick(float dt)
        {
        	base.Tick(dt);

            SpawnCounter += dt;

            if (SpawnCounter > SpawnRate)
            {
                SpawnCounter -= SpawnRate;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
			// dont spawn any more if player is dead
			if (Game.Instance.PlayerDead)
				return;

        	// -1 is infinite spawning
        	if (Total == 0)
				return;
			
			// DEBUG
			//Type = 2;
			//return;

        	switch (Type)
			{
			case 0: Game.Instance.AddQueue.Add(new EnemySlime() { Position = this.Position, }); break;
			case 1: Game.Instance.AddQueue.Add(new EnemyRedSlime() { Position = this.Position, }); break;
			case 2: Game.Instance.AddQueue.Add(new EnemyZombie() { Position = this.Position, }); break;
			case 3: Game.Instance.AddQueue.Add(new EnemyBat() { Position = this.Position, }); break;
			}
			
			Total -= 1;
        }
    }
    
	public class PhysicsGameEntity
		 : GameEntity
	{
		public Vector2 Velocity = Vector2.Zero;
		public Vector2 AirFriction = Vector2.One;
		public Vector2 GroundFriction = Vector2.One;
        public float AirborneTime = 0.15f;
		
		public override void Tick(float dt)
		{
			const float GravityForce = -0.45f;
			
            Velocity += Vector2.UnitY * GravityForce;
            Position += Velocity;
            
			if (AirborneTime <= 0.0f)
	            Velocity *= GroundFriction;
	        else
	            Velocity *= AirFriction;
            
            Position = new Vector2(
				FMath.Clamp(Position.X, -260.0f, 1030.0f),
				FMath.Max(Position.Y, Game.Instance.FloorHeight)
			);
            Position = new Vector2(Position.X, FMath.Max(Position.Y, Game.Instance.FloorHeight));
            
			// cleanup infinitesmal float values
			if (System.Math.Abs(Velocity.X) < 0.0001f)
				Velocity = new Vector2(0.0f, Velocity.Y);
			if (System.Math.Abs(Velocity.Y) < 0.0001f)
				Velocity = new Vector2(Velocity.X, 0.0f);
				
			// if on floor, stop fall
			if (Position.Y <= Game.Instance.FloorHeight)
            {
                AirborneTime = 0.0f;
				Velocity = new Vector2(Velocity.X, FMath.Max(Velocity.Y, 0.0f));
            }
            else
            {
                AirborneTime += dt;
            }

			base.Tick(dt);
			
			GetTransform();
		}
	}
	
	public class Coin
		: PhysicsGameEntity
	{
		public int index;
		public static LinkedList<Coin> Cache { get; set; }
		
		public static void InitializeCache()
		{
			Cache = new LinkedList<Coin>();
			for (int i = 0; i < 32; ++i)
			{
				Coin coin = new Coin();
				coin.index = i;
				coin.Scale = new Vector2(1.5f);
				coin.StopAllActions();
				coin.UnscheduleAll();
				Cache.AddFirst(coin);
			}
		}
		
		public static void Spawn(Vector2 position)
		{
			foreach (var c in Cache)
			{
				if (c.Parent != null)
					return;
			}
			
			if (Cache.Count == 0)
				return;
				
			Coin coin = Cache.First.Value;
			Cache.RemoveFirst();
			
			coin.Position = position;
			coin.Velocity = new Vector2(
				Game.Instance.Random.NextSignedFloat() * 0.5f,
				Game.Instance.Random.NextSignedFloat() * 2.5f + 10.0f
			);
			coin.FrameCount = 0;
			
			coin.Sprite.StopAllActions();
			coin.Sprite.RunAction(coin.Animation);
			
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.UnscheduleAll(coin);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(coin, coin.Tick, 0.0f, false);
			Game.Instance.World.AddChild(coin);

			Support.SoundSystem.Instance.Play("coin_spawn.wav");
		}
		
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
		public Support.AnimationAction Animation { get; set; }
		
		// TODO: bronze, silver, gold
		//public int Value { get; set; }
		
		public Coin()
		{
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/coins.png", 8, 3);
			Animation = new Support.AnimationAction(Sprite, 0, 8, 0.7f, looping: true);
			this.AddChild(Sprite);
			
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite) + new Vector2(0.0f, -8.0f),
				radius = () => 22.0f,
			});
		}
		
		public override void Tick(float dt)
		{
            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.Coin, this.Position, Sprite.TileIndex2D, Sprite.FlipU);

			base.Tick(dt);
			
			if (FrameCount > 60 * 4)
				Die(null, 0.0f);
		}
		
		public override void Die(Vector2? source, float damage)
		{
			Sprite.StopAllActions();	
			Sprite.RunAction(new Support.AnimationAction(Sprite, 0, 8, 0.3f, looping: true));
			Cache.AddFirst(this);
			base.Die(source, damage);
		}
	};

	
	public class Heart
		: PhysicsGameEntity
	{
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
		
		// TODO: bronze, silver, gold
		//public int Value { get; set; }
		
		public Heart()
		{
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/item_health.png", 1, 1);
			this.AddChild(Sprite);
			
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite) + new Vector2(0.0f, 0.0f),
				radius = () => 32.0f,
			});
		}
		
		public override void Tick(float dt)
		{
            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.Health, this.Position, Sprite.TileIndex2D, Sprite.FlipU);

			base.Tick(dt);
			
			if (FrameCount > 60 * 4)
				Die(null, 0.0f);
		}
	};

    public class EnemySlime
        : PhysicsGameEntity
    {
        public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
        public Support.AnimationAction IdleAnimation { get; set; }
        public Support.AnimationAction JumpInAnimation { get; set; }
        public Support.AnimationAction JumpMidAnimation { get; set; }
        public Support.AnimationAction JumpOutAnimation { get; set; }
        public Sequence JumpAnimationSequence { get; set; }
        
		public float MoveDelay { get; set; }

        public EnemySlime()
        {
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_green_frames.png", 4, 4);
			
			IdleAnimation = new Support.AnimationAction(Sprite, 0, 8, 0.5f, looping: true);
			JumpInAnimation = new Support.AnimationAction(Sprite, 8, 12, 0.3f, looping: false);
			JumpMidAnimation = new Support.AnimationAction(Sprite, 12, 13, 1.0f, looping: false);
			JumpOutAnimation = new Support.AnimationAction(Sprite, 13, 16, 0.2f, looping: false);
			
			JumpAnimationSequence = new Sequence();
			JumpAnimationSequence.Add(JumpInAnimation);
			JumpAnimationSequence.Add(new CallFunc(this.Jump));
			JumpAnimationSequence.Add(JumpMidAnimation);
			JumpAnimationSequence.Add(new DelayTime() { Duration = 0.40f });
			JumpAnimationSequence.Add(JumpOutAnimation);
			JumpAnimationSequence.Add(new DelayTime() { Duration = 0.05f });
			JumpAnimationSequence.Add(IdleAnimation);
            
            this.AddChild(Sprite);
            Sprite.RunAction(IdleAnimation);
            
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite) + new Vector2(0.0f, -8.0f),
				radius = () => 24.0f,
			});
			
			GroundFriction = new Vector2(0.85f);
			Health = 2.0f;
			MoveDelay = 3.0f;
        }

        public override void Tick(float dt)
        {
        	base.Tick(dt);

            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.Slime, this.Position, Sprite.TileIndex2D, Sprite.FlipU);

			if (InvincibleTime > 0.0f)
				return;
				
			// 0.0f will just leave the flip state as-is
			if (Velocity.X < 0.0f)
				Sprite.FlipU = true;
			if (Velocity.X > 0.0f)
				Sprite.FlipU = false;
				
			if (AirborneTime > 0.0f)
				return;
				
			MoveDelay -= dt;
			if (MoveDelay <= 0.0f)
			{
				Sprite.StopAllActions();
				Sprite.RunAction(JumpAnimationSequence);
				Support.SoundSystem.Instance.Play("green_slime_jump.wav");
				MoveDelay = 2.75f;
			}
        }
        
		public void Jump()
		{
            Vector2 offset = Game.Instance.Player.LocalToWorld(Vector2.Zero) - LocalToWorld(Vector2.Zero);
			Velocity += new Vector2(FMath.Sign(offset.X) * 3.0f, 10.0f + Game.Instance.Random.NextFloat() * 5.0f);
		}
        
		public override void TakeDamage(float damage, Vector2? source)
		{
			base.TakeDamage(damage, source);
			SpawnDamageParticles(GetCollisionCenter(Sprite), (Vector2)source, damage, Support.Color(32, 162, 99));
			MoveDelay = 3.0f;
			Sprite.StopAllActions();
			Sprite.RunAction(IdleAnimation);
			Support.SoundSystem.Instance.Play("green_slime_take_damage.wav");
		}
        
		public override void Die(Vector2? source, float damage)
		{
			base.Die(source, damage);
			Vector2 offset = (GetCollisionCenter(Sprite) - (Vector2)source);
			if (offset.LengthSquared() > 0.0f)
				offset = offset.Normalize() * 4.0f;	
			Game.Instance.ParticleEffects.AddParticlesTile("EnemySlime", Support.GetTileIndex(Sprite), Sprite.FlipU, GetCollisionCenter(Sprite), offset + Vector2.UnitY * 2.0f, damage * 2.0f);
			Support.SoundSystem.Instance.Play("green_slime_die.wav");
			DropCoinsWithAChanceOfHeart(GetCollisionCenter(Sprite), 2);
		}
    }
    
    public class EnemyRedSlime
        : PhysicsGameEntity
    {
        public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
        public Support.AnimationAction IdleAnimation { get; set; }
        public Support.AnimationAction RollAnimation { get; set; }
        public Support.AnimationAction BombAnimation { get; set; }
        public Support.AnimationAction BombAnimationOn { get; set; }
        public Support.AnimationAction BombAnimationOff { get; set; }
        public Sequence RollToIdleAction { get; set; }
        public Sequence RollToBombAction { get; set; }
        public Sequence BombAnimationSequence { get; set; }
        public float MoveDelay { get; set; }
        public float MoveTime { get; set; }
        public int MoveDirection { get; set; }
        public bool IsExploding { get; set; }
        
        public EnemyRedSlime()
        {
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_red_frames.png", 4, 6);
            this.AddChild(Sprite);
            
			IdleAnimation = new Support.AnimationAction(Sprite, 0, 4, 0.6f, looping: true);
			RollAnimation = new Support.AnimationAction(Sprite, 4, 12, 0.7f, looping: false);
			BombAnimation = new Support.AnimationAction(Sprite, 4, 12, 0.7f, looping: false);
			
			BombAnimationOn = new Support.AnimationAction(Sprite, 0, 1, 0.1f, looping: false);
			BombAnimationOff = new Support.AnimationAction(Sprite, 12, 13, 0.1f, looping: false);
			
			RollToIdleAction = new Sequence();
			RollToIdleAction.Add(RollAnimation);
			RollToIdleAction.Add(new DelayTime() { Duration = 0.05f });
			RollToIdleAction.Add(RollAnimation);
			RollToIdleAction.Add(new DelayTime() { Duration = 0.05f });
			RollToIdleAction.Add(IdleAnimation);
			
			RollToBombAction = new Sequence();
			RollToBombAction.Add(RollAnimation);
			RollToBombAction.Add(new DelayTime() { Duration = 0.05f });
			RollToBombAction.Add(RollAnimation);
			RollToBombAction.Add(new DelayTime() { Duration = 0.05f });
			RollToBombAction.Add(BombAnimation);
			
			BombAnimationSequence = new Sequence();
			float delay = 1.0f / 60.0f * 5.0f;
			for (int i = 0; i < 12; ++i)
			{
				BombAnimationSequence.Add(BombAnimationOn);
				BombAnimationSequence.Add(new DelayTime() { Duration = delay });
				BombAnimationSequence.Add(BombAnimationOff);
				BombAnimationSequence.Add(new DelayTime() { Duration = delay });
				delay -= 1.0f / 60.0f * 4.5f;
			}
			
			BombAnimationSequence.Add(new CallFunc(this.Explode));
			BombAnimationSequence.Add(new CallFunc(() => this.Die(GetCollisionCenter(Sprite) + -Vector2.UnitY, 9.0f)));
            
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite) + new Vector2(0.0f, -2.0f),
				radius = () => 24.0f,
			});
			
			GroundFriction = new Vector2(0.65f);
			Health = 4.0f;
			
			Sprite.RunAction(IdleAnimation);
        }
        
        public override void Tick(float dt)
        {
        	base.Tick(dt);

            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.RedSlime, this.Position, Sprite.TileIndex2D, Sprite.FlipU);
			
			if (InvincibleTime > 0.0f)
				return;
				
			// 0.0f will just leave the flip state as-is
			if (Velocity.X < 0.0f)
				Sprite.FlipU = true;
			if (Velocity.X > 0.0f)
				Sprite.FlipU = false;
				
			if (AirborneTime > 0.0f)
				return;
				
			MoveTime -= dt;
			MoveDelay -= dt;
			
			if (MoveTime > 0.0f)
			{
				Velocity += new Vector2(MoveDirection * 1.25f, 0.0f);
			}
			
			if (MoveDelay <= 0.0f)
			{
	            Vector2 offset = Game.Instance.Player.LocalToWorld(Vector2.Zero) - LocalToWorld(Vector2.Zero);
				MoveDirection = FMath.Sign(offset.X);
				
				MoveTime = 1.45f;
				MoveDelay = 3.0f;
				
				Sprite.StopAllActions();
				Sprite.RunAction(RollToIdleAction);
				Support.SoundSystem.Instance.Play("red_slime_roll.wav");
			}
        }
        
		public override void TakeDamage(float damage, Vector2? source)
		{
			base.TakeDamage(damage, source);
			SpawnDamageParticles(GetCollisionCenter(Sprite), (Vector2)source, damage, Support.Color(180, 40, 43));
			MoveTime = 0.0f;
			MoveDelay = 30.0f;
			
			if (Health <= 0.0f)
				return;
			
			if (IsExploding)
				return;
			
			Sprite.StopAllActions();
			Sprite.RunAction(BombAnimationSequence);
			
			IsExploding = true;
			Support.SoundSystem.Instance.Play("red_slime_explode.wav");
		}
		
		public override void Die(Vector2? source, float damage)
		{
			base.Die(source, damage);
			Vector2 offset = (GetCollisionCenter(Sprite) - (Vector2)source);
			if (offset.LengthSquared() > 0.0f)
				offset = offset.Normalize() * 4.0f;	
			Game.Instance.ParticleEffects.AddParticlesTile("EnemyRedSlime", Support.GetTileIndex(Sprite), Sprite.FlipU, GetCollisionCenter(Sprite), offset + Vector2.UnitY * 3.0f, damage * 2.0f);
			DropCoinsWithAChanceOfHeart(GetCollisionCenter(Sprite), 3);
		}
		
		public void Explode()
		{
			if (Health <= 0.0f)
				return;
			
			var explosion = new EnemyRedSlimeExplosion();
			explosion.Position = GetCollisionCenter(Sprite) + Vector2.UnitY * -10.0f;
			Game.Instance.World.AddChild(explosion);
		}
    }
    
	public class EnemyRedSlimeExplosion
		: GameEntity
	{           
		public EnemyRedSlimeExplosion()
		{
			CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = this,
				center = () => this.Position,
				radius = () => 140.0f,
			});
			
			CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = this,
				center = () => this.Position,
				radius = () => 160.0f,
			});
		}
		
		public override void Tick(float dt)
		{
			base.Tick(dt);
			Die(null, 0.0f);
		}
		
		public override void CollideTo(GameEntity owner, Sce.Pss.HighLevel.GameEngine2D.Node collider)
		{
			base.CollideTo(owner, collider);
			
			Type type = owner.GetType();
			if (type == typeof(EnemySlime) ||
				type == typeof(EnemyRedSlime) || 
				type == typeof(EnemyZombie) ||
				type == typeof(EnemyBat))
			{
				PhysicsGameEntity pge = owner as PhysicsGameEntity;
				Vector2 offset = pge.Position - Position;
				if (offset.LengthSquared() > 0.0f)
					offset = offset.Normalize();
				
				pge.Velocity += Vector2.UnitY * 15.0f;
				pge.Velocity += offset * 6.0f;
				
				owner.TakeDamage(1.0f, Position);
			}
		}
	}


    public class EnemyZombie
        : PhysicsGameEntity
    {
        public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
        public Support.AnimationAction IdleAnimation { get; set; }
        public Support.AnimationAction WalkAnimation { get; set; }
        public Sequence WalkToIdleSequence { get; set; }
        
        public float MoveDelay { get; set; }
        public float MoveTime { get; set; }
		public int MoveDirection { get; set; }
        
        public EnemyZombie()
        {
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/zombie_frames.png", 4, 2);
            this.AddChild(Sprite);
            
			IdleAnimation = new Support.AnimationAction(Sprite, 6, 8, 3.0f, looping: true);
			WalkAnimation = new Support.AnimationAction(Sprite, 0, 7, 1.0f, looping: false);
			
			WalkToIdleSequence = new Sequence();
			WalkToIdleSequence.Add(WalkAnimation);
			WalkToIdleSequence.Add(new DelayTime() { Duration = 0.05f });
			WalkToIdleSequence.Add(IdleAnimation);
            
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite),
				radius = () => 24.0f,
			});
			
			GroundFriction = new Vector2(0.5f);
			Health = 5.0f;
			
			Sprite.RunAction(IdleAnimation);
        }

        public override void Tick(float dt)
        {
        	base.Tick(dt);

            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.Zombie, this.Position, Sprite.TileIndex2D, Sprite.FlipU);
			
			if (InvincibleTime > 0.0f)
				return;
				
			// 0.0f will just leave the flip state as-is
			if (Velocity.X < 0.0f)
				Sprite.FlipU = true;
			if (Velocity.X > 0.0f)
				Sprite.FlipU = false;
				
			if (AirborneTime > 0.0f)
				return;
				
			MoveTime -= dt;
			MoveDelay -= dt;
			
			if (MoveTime > 0.0f && MoveTime < 0.5f)
			{
				//Velocity += new Vector2(MoveDirection * 0.25f, 0.0f);
				Velocity = new Vector2(MoveDirection * 1.5f, 0.0f);
			}
			
			if (MoveDelay <= 0.0f)
			{
				Vector2 player_position = Position;
				if (Game.Instance.Player != null)
		            player_position = Game.Instance.Player.LocalToWorld(Vector2.Zero);
	            Vector2 offset = player_position - LocalToWorld(Vector2.Zero);
				MoveDirection = FMath.Sign(offset.X);
				
				MoveTime = 0.65f;
				MoveDelay = 2.0f + Game.Instance.Random.NextFloat() * 1.0f;;
				
				Sprite.StopAllActions();
				Sprite.RunAction(WalkToIdleSequence);

				Support.SoundSystem.Instance.Play("zombie_shuffle.wav");
			}
        }
		
		public override void TakeDamage(float damage, Vector2? source)
		{
			base.TakeDamage(damage, source);
			SpawnDamageParticles(GetCollisionCenter(Sprite), (Vector2)source, damage, Support.Color(117, 168, 130));
			MoveTime = 0.0f;
			MoveDelay = 2.0f;
			Sprite.StopAllActions();
			Sprite.RunAction(IdleAnimation);
			Support.SoundSystem.Instance.Play("zombie_take_damage.wav");
		}
		
		public override void Die(Vector2? source, float damage)
		{
			base.Die(source, damage);
			Vector2 offset = (GetCollisionCenter(Sprite) - (Vector2)source);
			if (offset.LengthSquared() > 0.0f)
				offset = offset.Normalize() * 4.0f;	
			Game.Instance.ParticleEffects.AddParticlesTile("EnemyZombie", Support.GetTileIndex(Sprite), Sprite.FlipU, GetCollisionCenter(Sprite), offset + Vector2.UnitY * 4.0f, damage * 2.0f);
			Support.SoundSystem.Instance.Play("zombie_die.wav");
			DropCoinsWithAChanceOfHeart(GetCollisionCenter(Sprite), 5);
		}
    }
    
    public class EnemyBat
        : PhysicsGameEntity
    {
        public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
        public Support.AnimationAction FlyAnimation { get; set; }
        public Support.AnimationAction DiveAnimation { get; set; }
        public Support.AnimationAction PreDiveAnimation { get; set; }
        public Sce.Pss.HighLevel.GameEngine2D.Sequence PreDiveSequence { get; set; }
        
        public float MoveDelay { get; set; }
        public float MoveTime { get; set; }
		public int MoveDirection { get; set; }
		
		public float DiveTime { get; set; }
		
		public float HoverHeight = 300.0f;
		public float RiseHeight = 240.0f;

        public EnemyBat()
        {
            Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/bat_frames.png", 2, 2);
			AddChild(Sprite);
            
			FlyAnimation = new Support.AnimationAction(Sprite, 0, 4, 0.3f, looping: true);
			DiveAnimation = new Support.AnimationAction(Sprite, 0, 1, 1.0f, looping: false);
			PreDiveAnimation = new Support.AnimationAction(Sprite, 0, 4, 0.22f, looping: true);
			PreDiveSequence = new Sequence();
			PreDiveSequence.Add(new DelayTime() { Duration = 1.0f });
			PreDiveSequence.Add(new CallFunc(() => { Sprite.StopAllActions(); Sprite.RunAction(DiveAnimation); }));
            
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Enemy,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite),
				radius = () => 16.0f,
			});
			
			GroundFriction = new Vector2(0.5f);
			AirFriction = new Vector2(0.98f);
			Health = 0.6f;
			
			Sprite.RunAction(FlyAnimation);
        }

        public override void Tick(float dt)
        {
            Sprite.Visible = false;
			Game.Instance.SpriteBatch.Register(SpriteBatchType.Bat, this.Position, Sprite.TileIndex2D, Sprite.FlipU);

	        // reduce gravity
			Velocity += Vector2.UnitY * 0.45f;
			
        	base.Tick(dt);
			
			if (InvincibleTime > 0.0f)
				return;
				
			// 0.0f will just leave the flip state as-is
			if (Velocity.X < 0.0f)
				Sprite.FlipU = true;
			if (Velocity.X > 0.0f)
				Sprite.FlipU = false;
				
			MoveTime -= dt;
			MoveDelay -= dt;
			
			if (FrameCount % 60 == 0)
			{
				if (DiveTime <= 0.0f)
				{
					Support.SoundSystem.Instance.PlayNoClobber("bat_fly.wav");
				}

				if (Game.Instance.Random.Next() % 4 == 0)
				{
					HoverHeight = 300.0f + Game.Instance.Random.NextSignedFloat() * 10.0f * 10.0f;
				}
			}
				
			float velocity_x = FMath.Lerp(Velocity.X, MoveDirection * 4.0f, 0.015f);
			float velocity_y = Velocity.Y;
			
			if (DiveTime > 0.0f)
			{
				if (DiveTime > 2.0f)
				{
					velocity_x *= 0.9f;
					velocity_y *= 0.9f;
					velocity_y += 0.1f;		
				}
				else if (DiveTime > 1.0f)
				{
					velocity_y += -0.30f;		
				}
				else
				{
					velocity_y += 0.06f;		
				}
				
				DiveTime -= dt;
				if (DiveTime <= 0.0f)
				{
					Sprite.StopAllActions();
					Sprite.RunAction(FlyAnimation);
				}
			}
			else
			{
				if (Position.Y > HoverHeight)
					velocity_y += -0.075f;
					
				if (Position.Y < RiseHeight)
					velocity_y += 0.2f;
				
				velocity_y += 0.075f * FMath.Sin(FrameCount * 0.015f) * FMath.Sin(FrameCount * 0.04f);
			}
			
			Velocity = new Vector2(velocity_x, velocity_y);
			
			if (MoveDelay <= 0.0f)
			{
	            Vector2 offset = Game.Instance.Player.LocalToWorld(Vector2.Zero) - LocalToWorld(Vector2.Zero);
				MoveDirection = FMath.Sign(offset.X);
				
				if (Game.Instance.Random.Next() % 3 == 0)
				{
					DiveTime = 3.0f;
					Sprite.StopAllActions();
					Sprite.RunAction(PreDiveAnimation);
					Sprite.RunAction(PreDiveSequence);
				}
				
				MoveTime = 1.75f + Game.Instance.Random.NextFloat() * 1.25f;
				MoveDelay = MoveTime;
			}
        }
		
		public override void TakeDamage(float damage, Vector2? source)
		{
			base.TakeDamage(damage, source);
			SpawnDamageParticles(GetCollisionCenter(Sprite), (Vector2)source, damage, Support.Color(108, 71, 22));
			MoveTime = 0.0f;
			MoveDelay = 2.0f;
			Sprite.StopAllActions();
			Sprite.RunAction(FlyAnimation);
			Support.SoundSystem.Instance.Play("bat_take_damage.wav");
		}
		
		public override void Die(Vector2? source, float damage)
		{
			base.Die(source, damage);
			Vector2 offset = (GetCollisionCenter(Sprite) - (Vector2)source);
			if (offset.LengthSquared() > 0.0f)
				offset = offset.Normalize() * 4.0f;	
			Game.Instance.ParticleEffects.AddParticlesTile("EnemyBat", Support.GetTileIndex(Sprite), Sprite.FlipU, GetCollisionCenter(Sprite), offset + Vector2.UnitY * 4.0f, damage * 2.0f);
			Support.SoundSystem.Instance.PlayNoClobber("bat_die.wav");
			DropCoinsWithAChanceOfHeart(GetCollisionCenter(Sprite), 5);
		}
    }
    
	public class PlayerInput
	{
    	static float FilterAnalogValue( float value, float deadzone )
    	{
            float sign = ( value > 0.0f ? 1.0f : -1.0f );
    		value *= sign;

    		if ( value < deadzone ) return 0.0f;
    		else return sign * ( value - deadzone ) / ( 1.0f - deadzone );
    	}

		public static float LeftRightAxis()
		{
			GamePadData data = GamePad.GetData(0);
			
			if (Input2.GamePad0.Left.Down)
				return -1.0f;
				
			if (Input2.GamePad0.Right.Down)
				return 1.0f;
				
			return FilterAnalogValue( data.AnalogLeftX, 0.08f );
		}
		
		public static bool JumpButton()
		{
			if (Input2.GamePad0.Cross.Press)
				return true;
				
			return false;
		}
		
		public static bool AttackButton()
		{
			if (Input2.GamePad0.Square.Press)
				return true;
				
			return false;
		}
		
		public static bool SpecialButton()
        {
			if (Input2.GamePad0.Circle.Press)
				return true;
				
			return false;
		}
		
		public static bool AnyButton()
		{
			return
				AttackButton() || 
				JumpButton() ||
				SpecialButton();
		}
	}

    public class Player
        : PhysicsGameEntity
    {
        public const float GroundResponsiveness = 1.35f;
        public const float AirResponsiveness = 1.25f;
        public const float JumpPower = 11.0f;
        public const float IdleAnimationSpeedThreshold = 1.25f;

        public Sce.Pss.HighLevel.GameEngine2D.SpriteTile BodySprite { get; set; }
        public string CurrentAnimation { get; set; }
        public Dictionary<string, Support.AnimationAction> AnimationTable { get; set; }

        public float AttackTime { get; set; }
        
		public PlayerSwordAttack SwordAttack { get; set; }
		
        public float Beer { get; set; }
        public int Coins { get; set; }
        
		public bool DoneFirstLanding { get; set; }
		
		public int FootstepDelay { get; set; }
		
        public Player()
        {
            BodySprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sir_awesome_frames.png", 4, 4);
            this.AddChild(BodySprite);
            
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = BodySprite,
				center = () => GetCollisionCenter(BodySprite),
				radius = () => 14.0f,
			});
			
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = BodySprite,
				center = () => GetCollisionCenter(BodySprite) + new Vector2(0.0f, -40.0f),
				radius = () => 8.0f,
			});
			
			const float SingleFrame = 1.0f / 60.0f;
			AnimationTable = new Dictionary<string, Support.AnimationAction>() {
				{ "Idle", new Support.AnimationAction(BodySprite, 4, 5, SingleFrame * 30, looping: true) },
				{ "Walk", new Support.AnimationAction(BodySprite, 0, 4, SingleFrame * 60, looping: true) },
				{ "Attack", new Support.AnimationAction(BodySprite, 5, 7, SingleFrame * 6) },
				{ "JumpAttack", new Support.AnimationAction(BodySprite, 11, 13, SingleFrame * 6) },
				{ "Jump", new Support.AnimationAction(BodySprite, 8, 10, 0.2f) },
				{ "Fall", new Support.AnimationAction(BodySprite, 9, 10, SingleFrame * 30) },
			};
				
			SetAnimation("Fall");
			
			AttackTime = -1.0f;
			
			Position = new Vector2(400.0f, 1500.0f);
	        AirFriction = new Vector2(0.70f, 0.98f);
	        GroundFriction = new Vector2(0.70f, 0.99f);
	        
			Beer = 4.0f;
			Coins = 0;
			Health = 5.0f;
        }

        public override void Tick(float dt)
        {
        	base.Tick(dt);
            
            // DEBUG
			//Game.Instance.ParticleEffects.AddParticlesBurst(4, GetCollisionCenter(BodySprite), Vector2.UnitY, Colors.Blue, 4.0f);
			
            if (InvincibleTime <= 0.0f)
			{
	            // ground control (to major tom)
	            if (AirborneTime == 0.0f)
	            {
	            	float axis = PlayerInput.LeftRightAxis();
	            	Velocity += Vector2.UnitX * GroundResponsiveness * axis;
	                    
	                if (PlayerInput.JumpButton())
					{
			            // cannot jump while attacking
		            	if (AttackTime <= 0.0f)
						{
							SetAnimation("Jump");
		                    Velocity += Vector2.UnitY * JumpPower;
		                    EmitJumpLandParticles(5, 3.0f);
							Support.SoundSystem.Instance.Play("player_jump.wav");
						}
					}
                    
	                // no movement if attacking from ground
					if (AttackTime > 0.0f)
						Velocity = new Vector2(0.0f, Velocity.Y);
	                
					if (System.Math.Abs(Velocity.X) > IdleAnimationSpeedThreshold)
					{
						if (CurrentAnimation == "Idle")
							SetAnimation("Walk");

						if (FootstepDelay <= 0)
						{
							Support.SoundSystem.Instance.PlayNoClobber("player_walk.wav");
							FootstepDelay = 10;
						}
						FootstepDelay -= 1;
					}
					else
					{
						if (CurrentAnimation == "Walk")
							SetAnimation("Idle");
					}
				}
	            // air control
	            else
	            {
	            	float axis = PlayerInput.LeftRightAxis();
	            	Velocity += Vector2.UnitX * AirResponsiveness * axis;
	
	                // double jump?
	                //if (AirborneFrames > N) ?
	            	//if (PlayerInput.JumpButton())
	                    //Velocity += Vector2.UnitY * JumpPower;
	            }
            }

            // Attacks
            if (AttackTime < 0.0f)
			{
				if (PlayerInput.AttackButton())
				{
					StartAttack();
				}
			}
			
			if (PlayerInput.SpecialButton())
			{
				if (Beer > 1.0f)
				{
					ThrowGlass();
				}
			}
                
			
			if (AttackTime > 0.0f)
			{
				AttackTime -= dt;
				
				if (AttackTime <= 0.0f)
				{
					StopAttack();
				}
			}

			// if on floor, stop fall
			if (AirborneTime <= 0.0f)
            {
				if (CurrentAnimation == "Fall")
				{
					DetermineAnimation();
					EmitJumpLandParticles(8, 3.0f);
					Support.SoundSystem.Instance.Play("player_land.wav");
					
					if (!DoneFirstLanding)
					{
						EmitJumpLandParticles(64, 5.0f);
						Game.Instance.UI.Shake = 1.75f;
						DoneFirstLanding = true;
					}
				}
					
				if (CurrentAnimation == "JumpAttack")
					StopAttack();
            }
            else
            {
				if (CurrentAnimation == "Jump")
				{
					if (Velocity.Y < 0.0f)
						SetAnimation("Fall");
				}
            }
            
			// touch attack
			CheckTouch();	

			// walk animation speed based on velocity
			AnimationTable["Walk"].SetSpeed(System.Math.Abs(Velocity.X));
			
			// 0.0f will just leave the flip state as-is
			if (InvincibleTime <= 0.0f)
			{
				if (Velocity.X < -0.1f)
					BodySprite.FlipU = true;
				if (Velocity.X > 0.1f)
					BodySprite.FlipU = false;
			}
			
			//if (CurrentAnimation == "Attack")
				//System.Console.WriteLine("frame: {0}, {1}", BodySprite.TileIndex2D.X, BodySprite.TileIndex2D.Y);
				
			Beer = FMath.Min(Beer + 0.15f * dt, 4.0f);
        }
        
		public void StartAttack()
		{
			//Game.Instance.ParticleEffects.AddParticle(GetCollisionCenter(BodySprite), Colors.White);
			//Game.Instance.ParticleEffects.AddParticlesBurst(1, GetCollisionCenter(BodySprite), Vector2.UnitY, Colors.Blue, 4.0f);
			//Game.Instance.ParticleEffects.AddParticlesTile("EnemyZombie", 0, BodySprite.FlipU, GetCollisionCenter(BodySprite), Vector2.UnitY, 1.0f);
			//Game.Instance.ParticleEffects.AddParticlesTile("EnemySlime", 0, BodySprite.FlipU, GetCollisionCenter(BodySprite), Vector2.UnitY, 1.0f);
			//Game.Instance.ParticleEffects.AddParticlesTile("EnemySlime", 0, BodySprite.FlipU, GetCollisionCenter(BodySprite) + Vector2.UnitX * 150.0f, Vector2.UnitY, 1.0f);
			//Game.Instance.ParticleEffects.AddParticlesCone(16, GetCollisionCenter(BodySprite), Vector2.UnitY, Colors.White, 1.0f);
			//DropCoinsWithAChanceOfHeart(GetCollisionCenter(BodySprite) + Vector2.UnitY * 160.0f, 4);
			
			if (AirborneTime > 0.0f)
			{
				SetAnimation("JumpAttack");
				AttackTime = 0.225f;
			}
			else
			{
				SetAnimation("Attack");
				AttackTime = 0.125f;
			}
			
			SwordAttack = new PlayerSwordAttack();
			this.AddChild(SwordAttack);

			Support.SoundSystem.Instance.Play("player_sword_attack.wav");
		}
		
		public void ThrowGlass()
		{
			var glass = new PlayerGlassAttack() { Position = GetCollisionCenter(BodySprite) };
			glass.Velocity = new Vector2((BodySprite.FlipU ? -1.0f : 1.0f) * 8.0f, 8.0f);
			Game.Instance.World.AddChild(glass);
			Support.SoundSystem.Instance.Play("beer_throw.wav");
			Beer -= 1.0f;
		}
		
		public void StopAttack()
		{
			AttackTime = -1.0f;
			this.RemoveChild(SwordAttack,true);
			SwordAttack = null;
			DetermineAnimation();
		}
		
		public void CheckTouch()
		{
			// TODO: multitouch
			if (Input2.Touch00.Press)
			{
				Vector2 position = Game.Instance.Scene.Camera.GetTouchPos();
				var touch_attack = new PlayerTouchAttack() { Position = position, };
				Game.Instance.World.AddChild(touch_attack);
			}
		}
        
		public void DetermineAnimation()
		{
			if (AttackTime > 0.0f)
			{
				if (AirborneTime > 0.0f)
					SetAnimation("Jump");
				else
					SetAnimation("Attack");
			}
			else	
			{
				if (AirborneTime > 0.0f)
				{
					if (Velocity.Y < 0.0f)
						SetAnimation("Jump");
					else
						SetAnimation("Fall");
				}
				else
				{
					if (Velocity.X != 0.0f)
						SetAnimation("Walk");
					else
						SetAnimation("Idle");
				}
			}
		}
        
		public void SetAnimation(string animation)
		{
			if (CurrentAnimation != null)
				BodySprite.StopAction(AnimationTable[CurrentAnimation]);
				
			CurrentAnimation = animation;
			BodySprite.RunAction(AnimationTable[animation]);
			AnimationTable[animation].Reset();
			
			//Console.WriteLine("SetAnimation(): {0}", animation);
		}
		
		public override void CollideFrom(GameEntity owner, Node collider)
		{
			base.CollideTo(owner, collider);
			
			Type type = owner.GetType();
			if (type == typeof(EnemySlime) ||
				type == typeof(EnemyRedSlime) || 
				type == typeof(EnemyZombie) ||
				type == typeof(EnemyBat))
			{
				Vector2 source = GetCollisionCenter(collider);
				Vector2 ofs = GetCollisionCenter(BodySprite) - source;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 push = ofs.Normalize() * new Vector2(20.0f, 1.0f) + Vector2.UnitY * 4.0f;
				Velocity += push;
				(owner as PhysicsGameEntity).Velocity -= push * 0.1f;
				(owner as PhysicsGameEntity).InvincibleTime = 0.50f;
				InvincibleTime = 0.25f;
				
				if (type == typeof(EnemySlime))
				{
					TakeDamage(0.2f, source);
				}
				else if (type == typeof(EnemyRedSlime))
				{
					TakeDamage(0.4f, source);
					owner.TakeDamage(0.1f, GetCollisionCenter(BodySprite));
				}
				else if (type == typeof(EnemyZombie))
				{
					TakeDamage(0.75f, source);
				}
				else if (type == typeof(EnemyBat))
				{
					TakeDamage(0.35f, source);
				}
			}
			
			if (type == typeof(EnemyRedSlimeExplosion))
			{
				Vector2 source = GetCollisionCenter(collider);
				Vector2 ofs = GetCollisionCenter(BodySprite) - source;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				float damage = 40.0f / ofs.Length();
				Vector2 push = ofs.Normalize() * new Vector2(30.0f * damage, 1.0f) + Vector2.UnitY * 8.0f * damage;
				Velocity += push;
				InvincibleTime = 0.15f;
				TakeDamage(damage, source);
			}
			
			if (type == typeof(Coin))
			{
				Coins += 1;
				(owner as Coin).Die(null, 0.0f);
				Support.SoundSystem.Instance.Play("coin_collect.wav");
			}
			
			if (type == typeof(Heart))
			{
				if ((owner as Heart).FrameCount > 20)
				{
					Health = FMath.Min(Health + 1.0f, 5.0f);
					(owner as Heart).Die(null, 0.0f);
					Support.SoundSystem.Instance.Play("heart_collect.wav");
				}
			}
		}
		
		public override void TakeDamage(float damage, Vector2? source)
		{
			SpawnDamageParticles(GetCollisionCenter(BodySprite), (Vector2)source, damage * 3.0f, Support.Color(178, 35, 50));
			
			Game.Instance.UI.Shake += damage;

			Support.SoundSystem.Instance.Play("player_take_damage.wav");
			
			base.TakeDamage(damage, source);
		}
		
		public override void Die(Vector2? source, float damage)
		{
			Vector2 center = GetCollisionCenter(BodySprite);
			SpawnDamageParticles(center, (Vector2)source, damage * 5.0f, Support.Color(174, 35, 50));
			SpawnDamageParticles(center, (Vector2)source, damage * 8.0f, Support.Color(96, 35, 50));
			SpawnDamageParticles(center, (Vector2)source, damage * 12.0f, Support.Color(255, 0, 8));
			
			PlayerDeadSword sword = new PlayerDeadSword();
			sword.Position = center + Vector2.UnitY * 12.0f;
			sword.Velocity = Vector2.UnitY * 12.0f;
			Game.Instance.World.AddChild(sword);
			
			var sequence = new Sequence();
			sequence.Add(new DelayTime() { Duration = 3.0f });
			sequence.Add(new CallFunc(Game.Instance.PlayerDied));
			Game.Instance.World.RunAction(sequence);

			Support.MusicSystem.Instance.Stop("game_game_music.mp3");
			Support.SoundSystem.Instance.Play("game_game_over.wav");
			
			base.Die(source, damage);
		}
		
		public void EmitJumpLandParticles(int count, float power)
		{
			Vector2 feet = GetCollisionCenter(BodySprite) + Vector2.UnitY * -64.0f;
			Vector4 color = feet.X < 168.0f ? Support.Color(129, 133, 110) : Support.Color(127, 89, 42);
			Game.Instance.ParticleEffects.AddParticlesBurst(count, feet, Vector2.UnitY, color, power, 0.75f);
		}
    }
    
	public class PlayerDeadSword
		: PhysicsGameEntity
	{
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
		public float RotationSpeed { get; set; }
		public float CustomRotation { get; set; }
		
		public PlayerDeadSword()
		{
			Sprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/dead_sword.png", 1, 1);
			//Sprite.Pivot = Sprite.TextureInfo.Sizef * 0.5f;
			Sprite.Pivot = Sprite.TextureInfo.TextureSizef * 0.5f;
			
			this.AddChild(Sprite);
		}
		
		public override void Tick(float dt)
		{
			base.Tick(dt);	
			
			if (AirborneTime > 0.0f)
			{
				Sprite.Rotation = Sprite.Rotation.Rotate(0.22f);
			}
		}
	};
    
	public class PlayerSwordAttack
		: GameEntity
	{
		public float Damage { get; set; }
		
		public PlayerSwordAttack()
		{
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = this,
				center = () => this.GetAttackCenter(),
				radius = () => 58.0f,
			});
			
			Damage = 1.0f;
		}

		public Vector2 GetAttackCenter()
		{
			if (Parent == null) // collision delegate gets called
			{
//				System.Console.WriteLine( "Common.FrameCount + " PlayerSwordAttack.GetAttackCenter called but Parent is null" );
				return Sce.Pss.HighLevel.GameEngine2D.Base.Math._00;
			}
			Vector2 center = GetCollisionCenter((Parent as Player).BodySprite);
			Vector2 offset = new Vector2(34.0f, -4.0f);
			Player owner = Parent as Player;
			if (owner.BodySprite.FlipU)
				offset *= new Vector2(-1.0f, 1.0f);
			return center + offset;
		}
		
		public override void CollideTo(GameEntity owner, Node collider)
		{
			base.CollideTo(owner, collider);

			Player player = Parent as Player;	
			SpriteTile sprite = player.BodySprite;
			Vector2 center = GetCollisionCenter(sprite);
			
			Type type = owner.GetType();
			if (type == typeof(EnemySlime))
			{
				Vector2 ofs = GetCollisionCenter(owner) - GetCollisionCenter(Parent as Player);
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 dir = ofs.Normalize();
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += dir * new Vector2(4.0f, 0.25f) + Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.25f;
				owner.TakeDamage(Damage, GetCollisionCenter((Parent as Player).BodySprite));
				Support.SoundSystem.Instance.Play("player_sword_hit_enemy.wav");
			}
			
			if (type == typeof(EnemyRedSlime))
			{
				Vector2 ofs = GetCollisionCenter(owner) - GetCollisionCenter(Parent as Player);
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 dir = ofs.Normalize();
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += dir * 8.0f + Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.25f;
				owner.TakeDamage(Damage, GetCollisionCenter((Parent as Player).BodySprite));
				Support.SoundSystem.Instance.Play("player_sword_hit_enemy.wav");
			}
			
			if (type == typeof(EnemyZombie))
			{
				Vector2 ofs = GetCollisionCenter(owner) - GetCollisionCenter(Parent as Player);
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 dir = ofs.Normalize();
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += dir * 6.0f + Vector2.UnitY * 5.0f;
				owner.InvincibleTime = 0.25f;
				owner.TakeDamage(Damage, GetCollisionCenter((Parent as Player).BodySprite));
				Support.SoundSystem.Instance.Play("player_sword_hit_enemy.wav");
			}
			
			if (type == typeof(EnemyBat))
			{
				Vector2 ofs = GetCollisionCenter(owner) - GetCollisionCenter(Parent as Player);
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 dir = ofs.Normalize();
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += dir * 6.0f + Vector2.UnitY * 5.0f;
				owner.InvincibleTime = 0.25f;
				owner.TakeDamage(Damage, GetCollisionCenter((Parent as Player).BodySprite));
				Support.SoundSystem.Instance.Play("player_sword_hit_enemy.wav");
			}
		}
	}
	
	public class PlayerTouchAttack
		: GameEntity
	{
		public float Damage { get; set; }
		
		public PlayerTouchAttack()
		{
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = this,
				center = () => Position,
				radius = () => 32.0f,
			});
			
			Damage = 0.5f;
		}
		
		public override void CollideTo(GameEntity owner, Node collider)
		{
			base.CollideTo(owner, collider);
			
			Type type = owner.GetType();
			if (type == typeof(EnemySlime))
			{
				Vector2 ofs = GetCollisionCenter(owner) - Position;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.10f;
				owner.TakeDamage(Damage, Position);
			}
			
			if (type == typeof(EnemyRedSlime))
			{
				Vector2 ofs = GetCollisionCenter(owner) - Position;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.10f;
				owner.TakeDamage(Damage, Position);
			}
			
			if (type == typeof(EnemyZombie))
			{
				Vector2 ofs = GetCollisionCenter(owner) - Position;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.10f;
				owner.TakeDamage(Damage, Position);
			}
			
			if (type == typeof(EnemyBat))
			{
				Vector2 ofs = GetCollisionCenter(owner) - Position;
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += Vector2.UnitY * 8.0f;
				owner.InvincibleTime = 0.10f;
				owner.TakeDamage(Damage, Position);
			}
		}
		
		public override void Tick(float dt)
		{
			base.Tick(dt);
			
			if (FrameCount > 4)
				Die(null, 0.0f);
		}
	}

	public class PlayerGlassAttack
		: PhysicsGameEntity
	{
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
		public float Damage { get; set; }
		public bool Exploded { get; set; }
		public float CustomRotation { get; set; }
		public float Radius { get; set; }
		
		public PlayerGlassAttack()
		{
			Sprite = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/glass_frames.png");
			this.AddChild(Sprite);
			
			// TODO: i just want to lerp from a to b, not write delegates
			//Sprite.RunAction(
				//new ActionTweenGenericVector2Rotation(
					//() => Sprite.GetAttribute(Sce.Pss.HighLevel.GameEngine2D.Attribute.Rotation),
					//() => Sprite.SetAttribute(Sce.Pss.HighLevel.GameEngine2D.Attribute.Rotation)
				//)
			//);
				
            CollisionDatas.Add(new EntityCollider.CollisionEntry() {
	            type = EntityCollider.CollisionEntityType.Player,
				owner = this,
				collider = Sprite,
				center = () => GetCollisionCenter(Sprite),
				radius = () => Radius,
			});
			
			Sprite.Pivot = Sprite.TextureInfo.TextureSizef / 2.0f;
			
			Damage = 4.0f;
			//InvincibleTime = 60.0f;
			Radius = 16.0f;
		}
		
		public override void CollideTo(GameEntity owner, Node collider)
		{
			base.CollideTo(owner, collider);
			
			Type type = owner.GetType();
			if (type == typeof(EnemySlime) ||
			    type == typeof(EnemyRedSlime) ||
				type == typeof(EnemyZombie) ||
				type == typeof(EnemyBat))
			{
				Vector2 ofs = GetCollisionCenter(owner) - GetCollisionCenter(Sprite);
				if (ofs.LengthSquared() <= 0.0f)
					ofs = Vector2.UnitY;
					
				Vector2 dir = ofs.Normalize();
				PhysicsGameEntity p = owner as PhysicsGameEntity;
				p.Velocity += dir * 6.0f + Vector2.UnitY * 16.0f;
				owner.InvincibleTime = 0.25f;
				owner.TakeDamage(Damage, GetCollisionCenter(Sprite));
				
				if (!Exploded)
					Explode();
			}
		}
		
		public override void Tick(float dt)
		{
			base.Tick(dt);
			Velocity += Vector2.UnitY * -0.025f;
			Sprite.Rotation = Vector2.Rotation(CustomRotation);
			CustomRotation += 0.275f;
			
			if (Exploded)
				Die(null, 1.0f);
				
			if (AirborneTime <= 0.0f)
			{
				Explode();
			}
		}
		
		public void Explode()
		{
			Exploded = true;
			InvincibleTime = 0.0f;
			Radius = 72.0f;
		}
		
		public override void Die(Vector2? source, float damage)
		{
			base.Die(source, damage);
			
			Vector2 p = GetCollisionCenter(Sprite) + Vector2.UnitY * -12.0f;
			Vector4 color_a = new Vector4(250.0f / 255.0f, 250.0f / 255.0f, 160.0f / 255.0f, 1.0f);
			Vector4 color_b = new Vector4(233.0f / 255.0f, 169.0f / 255.0f, 5.0f / 255.0f, 1.0f);
			Vector4 color_c = new Vector4(9.0f / 255.0f, 7.0f / 255.0f, 2.0f / 255.0f, 1.0f);
			Game.Instance.ParticleEffects.AddParticlesBurstRandomy(48, p, Vector2.UnitY * 6.0f, color_a, 3.5f, 2.0f);
			Game.Instance.ParticleEffects.AddParticlesBurstRandomy(16, p, Vector2.UnitY * 6.0f, color_b, 3.5f, 1.7f);
			Game.Instance.ParticleEffects.AddParticlesBurstRandomy(8, p, Vector2.UnitY * 6.0f, color_c, 2.5f, 1.7f);
			Support.SoundSystem.Instance.Play("beer_splash.wav");
		}	
	}

    public class GameScene
        : Sce.Pss.HighLevel.GameEngine2D.Scene
    {
        //public override void OnCreate()
        //{
            // create player
            // create enemies
        //}
    }
    
	public class EntityCollider
	{
		public enum CollisionEntityType
		{
			Player,
			Enemy,
			//Weapon,
			//Spell,
			//Item,
		}
		
		public delegate Vector2 GetCenterDelegate();
		public delegate float GetRadiusDelegate();
		// GetForceVector()?
		
		public struct CollisionEntry
		{
			public CollisionEntityType type;
			public GameEntity owner;
			public Node collider;
			public GetCenterDelegate center;
			public GetRadiusDelegate radius;
		}
		
		List<List<CollisionEntry>> typed_entries;
		
		public EntityCollider()
		{
			typed_entries = new List<List<CollisionEntry>>();
			typed_entries.Add(new List<CollisionEntry>()); // Player
			typed_entries.Add(new List<CollisionEntry>()); // Enemy
		}
		
		public void Add(CollisionEntityType type, GameEntity owner, Node collider, GetCenterDelegate center, GetRadiusDelegate radius)
		{	
			CollisionEntry entry = new CollisionEntry() { type = type, owner = owner, collider = collider, center = center, radius = radius };
			List<CollisionEntry> entries = typed_entries[(int)type];
			entries.Add(entry);
		}
		
		public void Add(CollisionEntry entry)
		{
			List<CollisionEntry> entries = typed_entries[(int)entry.type];
			entries.Add(entry);
		}
		
		public void Collide()
		{
			// for each list
			//   check for each other list
			foreach (List<CollisionEntry> entries in typed_entries)
			{
				foreach (List<CollisionEntry> other_entries in typed_entries)
				{
					if (other_entries == entries)
						continue;
					
					for (int i = 0; i < entries.Count; ++i)
					{
						GameEntity collider_owner = entries[i].owner;
						Node collider_collider = entries[i].collider;
						Vector2 collider_center = entries[i].center();
						float collider_radius = entries[i].radius();
						
						for (int j = 0; j < other_entries.Count; ++j)
						{
							GameEntity collidee_owner = other_entries[j].owner;
							Node collidee_collider = other_entries[j].collider;
							if (collider_owner == collidee_owner)
								continue;
							
							Vector2 collidee_center = other_entries[j].center();
							float collidee_radius = other_entries[j].radius();
							
							float r = collider_radius + collidee_radius;
							
							Vector2 offset = collidee_center - collider_center;
							float lensqr = offset.LengthSquared();	
							
							if (lensqr < r * r)
							{
								collider_owner.CollideTo(collidee_owner, collidee_collider);
								collidee_owner.CollideFrom(collider_owner, collider_collider);
							}
						}
					}
				}
			}
			
			Clear();
		}
		
		public void Clear()
		{
			foreach (List<CollisionEntry> entries in typed_entries)
				entries.Clear();
		}
	}
	
	public class UI
		: Sce.Pss.HighLevel.GameEngine2D.Node
	{
		public Node HealthNode { get; set; }
		public Node BeerNode { get; set; }
		public Node CoinsNode { get; set; }
		public SpriteTile HealthPanel { get; set; }
		public SpriteTile BeerPanel { get; set; }
		public SpriteTile CoinsPanel { get; set; }
		public SpriteUV HealthBar { get; set; }
		public SpriteUV BeerBar { get; set; }
		public SpriteTile HealthBarBG { get; set; }
		public SpriteTile BeerBarBG { get; set; }
		public Label CoinsLabel { get; set; }
		public Label CoinsLabelShadow { get; set; }
		public Title Title { get; set; }
		public Font Font { get; set; }
		public FontMap FontMap { get; set; }
		
		public float FallSpeed { get; set; }
		public float HangDown { get; set; }
		public float HangDownTarget { get; set; }
		public float HangDownSpeed { get; set; }
		public float Shake { get; set; }
		public int FrameCount { get; set; }
		private float HealthSway { get; set; }
		private float BeerSway { get; set; }
		private float CoinsSway { get; set; }
		
		public UI()
		{
			this.AdHocDraw += this.Draw;
			
			HealthNode = new Sce.Pss.HighLevel.GameEngine2D.Node();
			BeerNode = new Sce.Pss.HighLevel.GameEngine2D.Node();
			CoinsNode = new Sce.Pss.HighLevel.GameEngine2D.Node();
			
			CoinsLabel = new Label();
			CoinsLabelShadow = new Label();
			
			HealthBarBG = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png", 1, 3);
			BeerBarBG = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png", 1, 3);
			HealthBar = Support.SpriteUVFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png");
			BeerBar = Support.SpriteUVFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png");
			
			float third = 1.0f / 3.0f;
			HealthBar.UV.T = new Vector2(0.0f, third * 1.0f);
			BeerBar.UV.T = new Vector2(0.0f, third * 2.0f);
			HealthBar.UV.S = new Vector2(1.0f, third);
			BeerBar.UV.S = new Vector2(1.0f, third);
			
			HealthPanel = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_panels.png", 3, 1);
			BeerPanel = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_panels.png", 3, 1);
			CoinsPanel = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_panels.png", 3, 1);
			
			HealthPanel.TileIndex2D = new Vector2i(0, 0);
			BeerPanel.TileIndex2D = new Vector2i(1, 0);
			CoinsPanel.TileIndex2D = new Vector2i(2, 0);
			
			float pivot_height = 750.0f;
			HealthNode.Pivot = HealthPanel.TextureInfo.TextureSizef / 6.0f + Vector2.UnitY * pivot_height;
			BeerNode.Pivot = BeerPanel.TextureInfo.TextureSizef / 6.0f + Vector2.UnitY * pivot_height;
			CoinsNode.Pivot = CoinsPanel.TextureInfo.TextureSizef / 6.0f + Vector2.UnitY * pivot_height;
			
			HealthPanel.VertexZ = 0.05f;
			
			Title = new Title();
			
			Font = new Font("/Application/Sample/GameEngine2D/ActionGameDemo/assets/fonts/IndieFlower.ttf", 72, FontStyle.Bold);
			FontMap = new FontMap(Font);
			
			CoinsLabel.FontMap = FontMap;
			CoinsLabelShadow.FontMap = FontMap;
			
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
		}
		
		public void TitleMode()
		{
			this.RemoveChild(CoinsNode, true);
			this.RemoveChild(BeerNode, true);
			this.RemoveChild(HealthNode, true);
			CoinsNode.RemoveChild(CoinsLabel, true);
			CoinsNode.RemoveChild(CoinsLabelShadow, true);
			CoinsNode.RemoveChild(CoinsPanel, true);
			BeerNode.RemoveChild(BeerPanel, true);
			BeerNode.RemoveChild(BeerBar, true);
			BeerNode.RemoveChild(BeerBarBG, true);
			HealthNode.RemoveChild(HealthPanel, true);
			HealthNode.RemoveChild(HealthBar, true);
			HealthNode.RemoveChild(HealthBarBG, true);
			this.AddChild(Title);

			Support.MusicSystem.Instance.Play("game_title_screen.mp3");
		}
		
		public void GameMode()
		{
			this.RemoveChild(Title, true);
			HealthNode.AddChild(HealthBarBG);
			HealthNode.AddChild(HealthBar);
			HealthNode.AddChild(HealthPanel);
			BeerNode.AddChild(BeerBarBG);
			BeerNode.AddChild(BeerBar);
			BeerNode.AddChild(BeerPanel);
			CoinsNode.AddChild(CoinsPanel);
			CoinsNode.AddChild(CoinsLabelShadow);
			CoinsNode.AddChild(CoinsLabel);
			this.AddChild(HealthNode);
			this.AddChild(BeerNode);
			this.AddChild(CoinsNode);
			
			Shake = 0.0f;
			FallSpeed = 0.0f;
			HangDown = 0.0f;			
			HangDownTarget = 1.0f;
			HangDownSpeed = 0.175f;

			Support.MusicSystem.Instance.Stop("game_title_screen.mp3");
		}
		
		public void Tick(float dt)
		{
			Shake *= 0.98f;
			if (HangDown < 1.0f)
			{
				FallSpeed += 0.0025f;
				Shake += 0.05f;
			}
			
			FallSpeed *= 0.9f;	
			HangDown += FallSpeed;
			HangDown = FMath.Lerp(HangDown, HangDownTarget, HangDownSpeed);
			
			if (Shake > 0.01f)
			{
				HealthSway = FMath.Lerp(HealthSway, FMath.Sin(FrameCount * 0.10f) * 0.025f * Shake, 0.25f);
				BeerSway = FMath.Lerp(BeerSway, FMath.Sin(FrameCount * 0.15f) * 0.025f * Shake, 0.25f);
				CoinsSway = FMath.Lerp(CoinsSway, FMath.Cos(FrameCount * 0.11f) * 0.025f * Shake, 0.25f);
			}
			else
			{
				HealthSway = FMath.Lerp(HealthSway, 0.0f, 0.01f);
				BeerSway = FMath.Lerp(BeerSway, 0.0f, 0.01f);
				CoinsSway = FMath.Lerp(CoinsSway, 0.0f, 0.01f);
			}
		
			FrameCount += 1;
		}
		
		public new void Draw()
		{
			Vector2 topleft = Director.Instance.CurrentScene.Camera.CalcBounds().Point01;
			
			float hanging_base = -0.0f;
			float fall_distance = -130.0f;
			float hanging = hanging_base + fall_distance * HangDown;
			
			Vector2 health_pos = topleft + new Vector2(960.0f / 3.0f * 0.0f + HealthSway, hanging);
			Vector2 beer_pos = topleft + new Vector2(960.0f / 3.0f * 1.0f + BeerSway, hanging);
			Vector2 coins_pos = topleft + new Vector2(960.0f / 3.0f * 2.0f + CoinsSway, hanging);
			
			Vector2 bar_offset = new Vector2(70.0f, 32.0f);
			
			HealthBarBG.Position = bar_offset;
			HealthBar.Position = bar_offset;
			
			HealthNode.Position = health_pos;
			HealthNode.Rotation = Vector2.UnitX.Rotate(HealthSway);
			
			BeerBarBG.Position = bar_offset;
			BeerBar.Position = bar_offset;
			
			BeerNode.Position = beer_pos;
			BeerNode.Rotation = Vector2.UnitX.Rotate(BeerSway);
			
			CoinsLabel.Position = new Vector2(120.0f, 20.0f);
			CoinsLabelShadow.Position = CoinsLabel.Position + new Vector2(2.0f, -2.0f);
			
			CoinsNode.Position = coins_pos;
			CoinsNode.Rotation = Vector2.UnitX.Rotate(CoinsSway);
			
			float health = 0.0f;
			float beer = 0.0f;
			int coins = 0;
			
			if (Game.Instance.Player != null)
			{
				health = Game.Instance.Player.Health / 5.0f;
				beer = Game.Instance.Player.Beer / 4.0f;
				coins = Game.Instance.Player.Coins;
			}
			
			float third = 1.0f / 3.0f;
			HealthBar.Scale = new Vector2(health, third);
			BeerBar.Scale = new Vector2(beer, third);
			HealthBar.UV.S = new Vector2(health, third);
			BeerBar.UV.S = new Vector2(beer, third);
			
			CoinsLabel.Color = Support.Color(255, 255, 0, 255);
			CoinsLabelShadow.Color = Support.Color(0, 0, 0, 255);
			
			CoinsLabel.Text = String.Format("{0}", coins);
			CoinsLabelShadow.Text = CoinsLabel.Text;
		}
	}

	public class Title
		: Sce.Pss.HighLevel.GameEngine2D.Node
	{
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile TitleSprite { get; set; }
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile SparkleSprite { get; set; }
		public Sequence SparkleSequence { get; set; }
		public Sce.Pss.HighLevel.GameEngine2D.Label PressAnyKeyLabel { get; set; }
		
		public Title()
		{
			TitleSprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/title.png", 1, 1);
			SparkleSprite = Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sparkle_frames.png", 8, 1);
			
			TitleSprite.Scale = new Vector2(1.0f);
			TitleSprite.Position = new Vector2(0.0f, 270.0f);
			SparkleSprite.Scale = new Vector2(1.0f);
			
			// doesn't look nice because of mismatch between title font
			//PressAnyKeyLabel = new Label();
			//var font = new Font("/Application/Sample/GameEngine2D/ActionGameDemo/assets/fonts/MedievalSharp.ttf", 48, FontStyle.Bold);
			//var font_map = new FontMap(font, 256);
			//PressAnyKeyLabel.FontMap = font_map;
			//PressAnyKeyLabel.Text = "Press Any Key";
			//PressAnyKeyLabel.Position = new Vector2(960.0f / 2.0f - 128.0f, 200.0f);
			//this.AddChild(PressAnyKeyLabel);
			
			SparkleSequence = new Sequence();
			
			SparkleSequence.Add(new CallFunc(() => SparkleSprite.Position = new Vector2(208.0f, 40.0f)));
			SparkleSequence.Add(new Support.AnimationAction(SparkleSprite, 0, 8, 0.4f, looping: false));
			SparkleSequence.Add(new DelayTime() { Duration = 0.6f });
			SparkleSequence.Add(new CallFunc(() => SparkleSprite.Position = new Vector2(336.0f, 151.0f)));
			SparkleSequence.Add(new Support.AnimationAction(SparkleSprite, 0, 8, 0.4f, looping: false));
			SparkleSequence.Add(new DelayTime() { Duration = 0.6f });
			SparkleSequence.Add(new CallFunc(() => SparkleSprite.Position = new Vector2(746.0f, 68.0f)));
			SparkleSequence.Add(new Support.AnimationAction(SparkleSprite, 0, 8, 0.4f, looping: false));
			SparkleSequence.Add(new DelayTime() { Duration = 0.6f });
			SparkleSequence.Add(new CallFunc(() => SparkleSprite.Position = new Vector2(405.0f, 123.0f)));
			SparkleSequence.Add(new Support.AnimationAction(SparkleSprite, 0, 8, 0.4f, looping: false));
			SparkleSequence.Add(new DelayTime() { Duration = 3.0f });
			
			this.AddChild(TitleSprite);
			TitleSprite.AddChild(SparkleSprite);
			
			var repeat = new RepeatForever() { InnerAction = SparkleSequence };
			SparkleSprite.RunAction(repeat);
		}
	}

	public enum SpriteBatchType
	{
		Slime,
		RedSlime,
		Zombie,
		Bat,
		Coin,
		Health,
		Gauge,
		Panel,
	};

	public class SpriteBatch
		: Node
	{
		public struct SpriteItem
		{
			public Vector2 position;
			public Vector2i tile;
			public bool flip_u;
		};

		public List<SpriteTile> Sprites = new List<SpriteTile>();
		public List<SpriteItem> Slimes = new List<SpriteItem>();
		public List<SpriteItem> RedSlimes = new List<SpriteItem>();
		public List<SpriteItem> Zombies = new List<SpriteItem>();
		public List<SpriteItem> Bats = new List<SpriteItem>();
		public List<SpriteItem> Coins = new List<SpriteItem>();
		public List<SpriteItem> Healths = new List<SpriteItem>();
		public List<SpriteItem> Gauges = new List<SpriteItem>();
		public List<SpriteItem> Panels = new List<SpriteItem>();

		public SpriteBatch()
		{
			PrecacheSprites();

			Sprites.Clear();
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_green_frames.png", 4, 4));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_red_frames.png", 4, 6));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/zombie_frames.png", 4, 2));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/bat_frames.png", 2, 2));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/coins.png", 8, 3));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/item_health.png", 1, 1));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png", 1, 3));
			Sprites.Add(Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_panels.png", 3, 1));

			AdHocDraw += this.DrawBatch;
		}

		public void PrecacheSprites()
		{
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_green_frames.png", 4, 4);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_red_frames.png", 4, 6);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/zombie_frames.png", 4, 2);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/bat_frames.png", 2, 2);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/coins.png", 8, 3);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/item_health.png", 1, 1);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/dead_sword.png", 1, 1);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_gauge_fill.png", 1, 3);
			Support.PrecacheTiledSprite("/Application/Sample/GameEngine2D/ActionGameDemo/assets/ui_panels.png", 3, 1);
		}

		public void Register(SpriteBatchType type, Vector2 position, Vector2i tile, bool flip_u)
		{
			List<SpriteItem> list = null;

			switch (type)
			{
				case SpriteBatchType.Slime: list = Slimes; break;
				case SpriteBatchType.RedSlime: list = RedSlimes; break;
				case SpriteBatchType.Zombie: list = Zombies; break;
				case SpriteBatchType.Bat: list = Bats; break;
				case SpriteBatchType.Coin: list = Coins; break;
				case SpriteBatchType.Health: list = Healths; break;
				case SpriteBatchType.Gauge: list = Gauges; break;
				case SpriteBatchType.Panel: list = Panels; break;
			}

			list.Add(new SpriteItem() { position = position, tile = tile, flip_u = flip_u });
		}

		public void DrawBatch()
		{
			DrawList(Slimes, Sprites[(int)SpriteBatchType.Slime]);
			DrawList(RedSlimes, Sprites[(int)SpriteBatchType.RedSlime]);
			DrawList(Zombies, Sprites[(int)SpriteBatchType.Zombie]);
			DrawList(Bats, Sprites[(int)SpriteBatchType.Bat]);
			DrawList(Coins, Sprites[(int)SpriteBatchType.Coin]);
			DrawList(Healths, Sprites[(int)SpriteBatchType.Health]);
			//DrawList(Gauges, Sprites[(int)SpriteBatchType.Gauge]);
			//DrawList(Panels, Sprites[(int)SpriteBatchType.Panel]);
		}

		public void DrawList(List<SpriteItem> items, SpriteTile sprite)
		{
			Director.Instance.GL.SetBlendMode( sprite.BlendMode );
			sprite.Shader.SetColor( ref sprite.Color );
            sprite.Shader.SetUVTransform( ref Sce.Pss.HighLevel.GameEngine2D.Base.Math.UV_TransformFlipV );

			Director.Instance.SpriteRenderer.BeginSprites(sprite.TextureInfo, sprite.Shader, items.Count);

			for (int i = 0; i < items.Count; ++i)
			{
				SpriteItem item = items[i];
				sprite.Quad.T = item.position;
				sprite.TileIndex2D = item.tile;
				Director.Instance.SpriteRenderer.FlipU = item.flip_u;
				Director.Instance.SpriteRenderer.FlipV = sprite.FlipV;
				TRS copy = sprite.Quad;
				Director.Instance.SpriteRenderer.AddSprite( ref copy, sprite.TileIndex2D );
			}

			Director.Instance.SpriteRenderer.EndSprites(); 

			items.Clear();
		}
	};

    public class Game
    {
    	public static Game Instance;// = new Game();
    	
        public Sce.Pss.HighLevel.GameEngine2D.Scene Scene { get; set; }
        public Layer Background { get; set; }
        public Layer World { get; set; }
        public Layer EffectsLayer { get; set; }
        public Layer Foreground { get; set; }
        public Layer Curtains { get; set; }
        public Layer Interface { get; set; }
        
		public Random Random { get; set; }
        public Player Player { get; set; }
        public EntityCollider Collider { get; set; }
		public Support.ParticleEffectsManager ParticleEffects { get; set; }
		public Support.TextureTileMapManager TextureTileMaps { get; set; }
		public UI UI { get; set; }
        
		public List<GameEntity> AddQueue { get; set; }
		public List<GameEntity> RemoveQueue { get; set; }
		
        public float FloorHeight = 80.0f;
        public float WorldScale = 1.0f;
        
		public Vector2 TitleCameraCenter { get; set; }
		public Vector2 CameraTarget { get; set; }
		
		public int WaveCount { get; set; }
		public bool PlayerDead { get; set; }
		
		public Sce.Pss.HighLevel.GameEngine2D.SpriteTile LightShafts { get; set; }
		public Sce.Pss.HighLevel.GameEngine2D.ActionBase EnemySpawnerLoop { get; set; }
		
		public SpriteBatch SpriteBatch;

        public Game()
        {
			//Director.Instance.DebugFlags |= DebugFlags.Navigate; // press left alt + mouse to navigate in 2d space
			//Director.Instance.DebugFlags |= DebugFlags.DrawGrid;
			//Director.Instance.DebugFlags |= DebugFlags.DrawContentWorldBounds;
			//Director.Instance.DebugFlags |= DebugFlags.DrawContentLocalBounds;
			//Director.Instance.DebugFlags |= DebugFlags.DrawTransform;
			//Director.Instance.DebugFlags |= DebugFlags.Log;

            Scene = new Sce.Pss.HighLevel.GameEngine2D.Scene();
            Background = new Layer();
            World = new Layer();
            EffectsLayer = new Layer();
            Foreground = new Layer();
            Curtains = new Layer();
            Interface = new Layer();
            Random = new Random();
            Collider = new EntityCollider();
            ParticleEffects = new Support.ParticleEffectsManager();
            TextureTileMaps = new Support.TextureTileMapManager();
            UI = new UI();

			SpriteBatch = new SpriteBatch();

			BuildTextureTileMaps();
            
			AddQueue = new List<GameEntity>();
			RemoveQueue = new List<GameEntity>();

            Scene.AddChild(Background);
            Scene.AddChild(World);
            Scene.AddChild(EffectsLayer);
            Scene.AddChild(Foreground);
            Scene.AddChild(Interface);
            Scene.AddChild(Curtains);
            
			Scene.Camera.SetViewFromViewport();
			
			// temporary: munge viewport to match vita + assets
			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);
			Camera2D camera = Scene.Camera as Camera2D;
			camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, ideal_screen_size / 2.0f);
			TitleCameraCenter = camera.Center;
			CameraTarget = TitleCameraCenter;

			EffectsLayer.AddChild(ParticleEffects);
			Interface.AddChild(UI);
		
            // world
            var bg_forest = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/background_back.png");
            var fg_log = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/background_front.png");

            bg_forest.Position = new Vector2(-160.0f, 0.0f);
			fg_log.Position = new Vector2(-160.0f, 0.0f);
			
			bg_forest.Pivot = new Vector2(bg_forest.TextureInfo.TextureSizef.X * 0.5f, 0.0f);
            fg_log.Pivot = new Vector2(fg_log.TextureInfo.TextureSizef.X * 0.5f, 0.0f);
			
			//LightShafts = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/background_light_shafts.png");
			//LightShafts.Position = new Vector2(-160.0f, 0.0f);
			//LightShafts.Color.A = 0.2f;

            Background.AddChild(bg_forest);
            //Foreground.AddChild(LightShafts);
            Foreground.AddChild(fg_log);
			
            var curtain_left = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/curtain.png");
            var curtain_right = Support.SpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/curtain.png");
            
			curtain_left.Position = new Vector2(-200.0f, 0.0f);
			curtain_right.Position = new Vector2(1280.0f - 380.0f, 0.0f);
			curtain_right.FlipU = true;
			Curtains.AddChild(curtain_left);
			Curtains.AddChild(curtain_right);
			
			UI.TitleMode();
			
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, TickTitle, 0.0f, false);
			
			World.AdHocDraw += this.DrawWorld;
        }
        
		public void TickTitle(float dt)
		{
			((Camera2D)Scene.Camera).Center = TitleCameraCenter;
			
			//LightShafts.Color.A = FMath.Sin(Game.Instance.UI.FrameCount * 0.01f) * 0.15f;
			
			if (PlayerInput.AnyButton())
			{
	            Player = new Player();
	            World.AddChild(Player);
				World.AddChild(SpriteBatch);
	            
	            // proper enemies
	            /*
	            if (false)
				{
		            World.AddChild(new EnemySpawner() { Position = new Vector2(-10.0f, FloorHeight), SpawnRate = 3.0f, SpawnCounter = 3.0f, Type = 0, Total = -1, });
		            World.AddChild(new EnemySpawner() { Position = new Vector2(970.0f, FloorHeight), SpawnRate = 4.0f, SpawnCounter = 4.0f, Type = 0, Total = -1, });
		            World.AddChild(new EnemySpawner() { Position = new Vector2(120.0f, 460.0f), SpawnRate = 8.0f, SpawnCounter = 5.0f, Type = 1, Total = -1, });
		            World.AddChild(new EnemySpawner() { Position = new Vector2(310.0f, 460.0f), SpawnRate = 20.0f, SpawnCounter = 20.0f, Type = 2, Total = -1, });
		            World.AddChild(new EnemySpawner() { Position = new Vector2(960.0f, 460.0f), SpawnRate = 40.0f, SpawnCounter = 22.0f, Type = 2, Total = -1, });
		            World.AddChild(new EnemySpawner() { Position = new Vector2(-10.0f, 460.0f), SpawnRate = 40.0f, SpawnCounter = 14.0f, Type = 2, Total = -1, });
				}
				*/
	            
				// test enemies
				/*
				if (false)
				{
		            World.AddChild(new EnemySpawner() { Position = new Vector2(200.0f, FloorHeight), SpawnRate = 6.0f, SpawnCounter = 6.0f, Type = 3, Total = -1 });
		            //World.AddChild(new EnemySpawner() { Position = new Vector2(200.0f, FloorHeight), SpawnRate = 3.0f, SpawnCounter = 6.0f, Type = 3, Total = -1 });
				}
				*/
				
	            // test high enemy count
				//for (int i = 0; i < 300; ++i)
					//World.AddChild(new EnemySlime() { Position = new Vector2(200.0f + i, 150.0f + (i % 30)) });
					
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, this.TickTitle);
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, this.TickGame, 0.0f, false);

				Support.SoundSystem.Instance.Play("game_press_start.wav");

				UI.GameMode();

				StartEnemySpawning();
			}
		}
		
		public void TickGame(float dt)
		{
			if (Player == null)
			{
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, this.TickGame);
				Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, this.TickTitle, 0.0f, false);
				UI.TitleMode();

				StopEnemySpawning();
			}
			else
			{
				const float Border = 200.0f;
				const float Limit = 160.0f;
				
				// Pull camera to left/right if near screen edge
				Camera2D camera = (Camera2D)Director.Instance.CurrentScene.Camera;
				Vector2 world_position = Player.GetCollisionCenter(Player.BodySprite);
				CameraTarget = new Vector2(world_position.X, CameraTarget.Y);
				
				Vector2 offset = CameraTarget - camera.Center;
				if (offset.LengthSquared() > 0.0f)
				{
					float distance = offset.Length();
					if (distance > Border)
					{
						camera.Center += offset * 0.01f;
						camera.Center = new Vector2(
							FMath.Clamp(camera.Center.X, TitleCameraCenter.X - Limit, TitleCameraCenter.X + Limit),
							camera.Center.Y
						);
					}
				}

				// dont play if player is dead
				if (Player.Health > 0.0f)
					Support.MusicSystem.Instance.PlayNoClobber("game_game_music.mp3");
			}
		}

		public void StartEnemySpawning()
		{
			if (EnemySpawnerLoop != null)
				return;

			Game.Instance.WaveCount = 0;

			Sequence waves = new Sequence();
			waves.Add(new CallFunc(() => World.AddChild(new EnemyWave(0.0f))));
			waves.Add(new DelayTime() { Duration = 45.0f });
			waves.Add(new CallFunc(() => { Game.Instance.WaveCount += 1; }));
			
			EnemySpawnerLoop = new RepeatForever() { InnerAction = waves };
			World.RunAction(EnemySpawnerLoop);
		}

		public void StopEnemySpawning()
		{
			if (EnemySpawnerLoop == null)
				return;

			World.StopAction(EnemySpawnerLoop);
			EnemySpawnerLoop = null;
		}

		public void BuildTextureTileMaps()
		{
			TextureTileMaps.Add("Player", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sir_awesome_frames.png", 4, 4).TextureInfo.Texture, 4, 4);
			TextureTileMaps.Add("EnemySlime", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_green_frames.png", 4, 4).TextureInfo.Texture, 4, 4);
			TextureTileMaps.Add("EnemyRedSlime", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_red_frames.png", 4, 6).TextureInfo.Texture, 4, 6);
			TextureTileMaps.Add("EnemyZombie", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/zombie_frames.png", 4, 2).TextureInfo.Texture, 4, 2);
			TextureTileMaps.Add("EnemyBat", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/bat_frames.png", 2, 2).TextureInfo.Texture, 2, 2);

			for (int i = 0; i < 32; ++i)
			{
				//Console.WriteLine("TestOffscreen: {0}", i);
				//TextureTileMaps.TestOffscreen("Player", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sir_awesome_frames.png", 4, 4).TextureInfo.Texture);
			}

			//TextureTileMaps.Add("Player", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/sir_awesome_frames.png", 4, 4).TextureInfo.Texture, 1, 1);
			//TextureTileMaps.Add("EnemySlime", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_green_frames.png", 4, 4).TextureInfo.Texture, 1, 1);
			//TextureTileMaps.Add("EnemyRedSlime", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/slime_red_frames.png", 4, 6).TextureInfo.Texture, 1, 1);
			//TextureTileMaps.Add("EnemyZombie", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/zombie_frames.png", 4, 2).TextureInfo.Texture, 1, 1);
			//TextureTileMaps.Add("EnemyBat", Support.TiledSpriteFromFile("/Application/Sample/GameEngine2D/ActionGameDemo/assets/bat_frames.png", 2, 2).TextureInfo.Texture, 1, 1);
		}
        
		// NOTE: no delta time, frame specific
		public void FrameUpdate()
		{
			Collider.Collide();
			
			foreach (GameEntity e in RemoveQueue)
				World.RemoveChild(e,true);
			foreach (GameEntity e in AddQueue)
				World.AddChild(e);
				
			RemoveQueue.Clear();
			AddQueue.Clear();
			
			// is player dead?
			if (PlayerDead)
			{
				if (PlayerInput.AnyButton())
				{
					// ui will transition to title mode
					World.RemoveAllChildren(true);
					Collider.Clear();
					PlayerDead = false;
					// TODO: why is this not removing things from scheduler
					// will trigger title mode with null player
					//UI.TitleMode();
					//Player = null;
					
					// hide UI and then null player to swap back to title
					UI.HangDownTarget = -1.0f;
					UI.HangDownSpeed = 0.175f;
					var sequence = new Sequence();
					sequence.Add(new DelayTime() { Duration = 0.4f });
					sequence.Add(new CallFunc(() => this.Player = null));
					World.RunAction(sequence);
				}
			}
		}
		
		public void DrawWorld()
		{
			// debug
			//Director.Instance.GL.ModelMatrix.Push();
			//Director.Instance.GL.ModelMatrix.SetIdentity();
			//Director.Instance.DrawHelpers.DrawCircle(TitleCameraCenter, 30.0f, 32);
			//Director.Instance.GL.ModelMatrix.Pop();
		}
		
		public void PlayerDied()
		{
			PlayerDead = true;
		}
    }
}

static class Program
{
	static void Main( string[] args )
	{
		Log.SetToConsole();
		SirAwesome.EntryPoint.Run(args);
	}
}
