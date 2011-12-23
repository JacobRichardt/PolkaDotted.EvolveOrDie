using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie.World
{
	public class Tile
	{
		public const int TILE_SIZE = 50;
		
		private Texture2D _BackgroundTexture;
		private Texture2D _ResourceTexture;
		private SpriteBatch _SpriteBatch;

		public WorldCoordinate Coordinate { get; private set; }
		public int Resources { get; set; }

		private readonly bool _IsAccessable;	
		public bool IsPassable { get { return _IsAccessable && Organism == null; } }

		private Organism.Organism _Organism;
		public Organism.Organism Organism 
		{
			get { return _Organism; }
			set
			{
				if(value == null)
				{
					_Organism = value;
					return;
				}

				if(_Organism != null)
					throw new Exception("Tile already has Organism");

				_Organism = value;

				if(_Organism.Tile != null)
					_Organism.Tile.Organism = null;

				_Organism.Tile = this;
				_Organism.CurrentResources += Resources;
				Resources = 0;
			}
		}

		public Tile(WorldCoordinate coordinate, bool isAccessable, int resources)
		{
			_IsAccessable = isAccessable;
			Coordinate = coordinate;
			Resources = resources;
		}

		public void Load(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			_BackgroundTexture = contentManager.Load<Texture2D>(_IsAccessable ? "NormalTile" : "BlockedTile");
			_ResourceTexture = contentManager.Load<Texture2D>("Resource");
			_SpriteBatch = spriteBatch;
		}

		public void Draw(GameTime gameTime, Vector2 offSet)
		{
			var screenCoordinate = new Vector2(Coordinate.X * TILE_SIZE, Coordinate.Y * TILE_SIZE) + offSet;
			
			_SpriteBatch.Draw(_BackgroundTexture, screenCoordinate, Color.White);

			if(Organism != null)
				_Organism.Draw(gameTime, screenCoordinate);
			else if(Resources != 0)
				_SpriteBatch.Draw(_ResourceTexture, screenCoordinate, Color.White);
		}
	}
}