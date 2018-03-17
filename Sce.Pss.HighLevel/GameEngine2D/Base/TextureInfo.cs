using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class TextureInfo : IDisposable
	{
		public class CachedTileData
		{
			public Vector2 UV_00;

			public Vector2 UV_10;

			public Vector2 UV_01;

			public Vector2 UV_11;
		}

		private Texture2D _Texture;
		public Texture2D Texture
		{
			get
			{
				return _Texture;
			}
			set
			{
				_Texture = value;
			}
		}

		public Vector2 TileSizeInUV;

		public Vector2i NumTiles;

		private bool m_disposed = false;

		private TextureInfo.CachedTileData[] m_tiles_uvs;

		public Vector2 TextureSizef
		{
			get
			{
				return new Vector2((float)this.Texture.Width, (float)this.Texture.Height);
			}
		}

		public Vector2i TextureSizei
		{
			get
			{
				return new Vector2i(this.Texture.Width, this.Texture.Height);
			}
		}

		public Vector2 TileSizeInPixelsf
		{
			get
			{
				Common.Assert(this.Texture != null);
				return this.TextureSizef * this.TileSizeInUV;
			}
		}

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public TextureInfo.CachedTileData GetCachedTiledData(ref Vector2i tile_index)
		{
			return this.m_tiles_uvs[tile_index.X + this.NumTiles.X * tile_index.Y];
		}

		public TextureInfo()
		{
		}

		public TextureInfo(string filename)
		{
			this.Initialize(new Texture2D(filename, false), Math._11i, TRS.Quad0_1);
		}

		public TextureInfo(Texture2D texture)
		{
			this.Initialize(texture, Math._11i, TRS.Quad0_1);
		}

		public TextureInfo(Texture2D texture, Vector2i num_tiles)
		{
			this.Initialize(texture, num_tiles, TRS.Quad0_1);
		}

		public TextureInfo(Texture2D texture, Vector2i num_tiles, TRS source_area)
		{
			this.Initialize(texture, num_tiles, source_area);
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
//				Common.DisposeAndNullify<Texture2D>(ref this.Texture);
				this.m_disposed = true;
			}
		}

		public void Initialize(Texture2D texture, Vector2i num_tiles, TRS source_area)
		{
			this.Texture = texture;
			this.TileSizeInUV = source_area.S / num_tiles.Vector2();
			this.NumTiles = num_tiles;
			this.m_tiles_uvs = new TextureInfo.CachedTileData[num_tiles.Product()];
			for (int i = 0; i < this.NumTiles.Y; i++)
			{
				for (int j = 0; j < this.NumTiles.X; j++)
				{
					Vector2i tile_index = new Vector2i(j, i);
					TRS tRS = TRS.Tile(this.NumTiles, tile_index, source_area);
					int num = tile_index.X + tile_index.Y * this.NumTiles.X;
					this.m_tiles_uvs[num] = new TextureInfo.CachedTileData
					{
						UV_00 = tRS.Point00,
						UV_10 = tRS.Point10,
						UV_01 = tRS.Point01,
						UV_11 = tRS.Point11
					};
				}
			}
		}
	}
}
