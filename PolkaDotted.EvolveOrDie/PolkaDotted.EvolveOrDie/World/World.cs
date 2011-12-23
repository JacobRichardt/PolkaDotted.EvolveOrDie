using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions;

namespace PolkaDotted.EvolveOrDie.World
{
	public class World
	{
		private View _View;
		private WorldCoordinate _Size;

		private ContentManager _ContentManager;
		private SpriteBatch _SpriteBatch;

		private Tile[,] _Tiles;
		private IList<Organism.Organism> _Organisms;

		private Random _Random;
		private TurboHandler _Turbo;

		public void Initialize(Viewport viewport)
		{
			_Size = new WorldCoordinate(20, 20);
			_View = new View(viewport, new Vector2(_Size.X * Tile.TILE_SIZE, _Size.Y * Tile.TILE_SIZE));
			_Tiles = CreateTilese(_Size);
			_Organisms = CreateOrganisms(_Tiles);
			_Random = new Random();
			_Turbo = new TurboHandler();
		}

		private static Tile[,] CreateTilese(WorldCoordinate size)
		{
			var result = new Tile[size.X,size.Y];

			for (var x = 0; x < size.X; x++)
			{
				for (var y = 0; y < size.Y; y++)
				{
					var isAccessable = x != 0 && x != size.X - 1 && y != 0 && y != size.Y - 1;

					result[x, y] = new Tile(new WorldCoordinate(x, y), isAccessable, isAccessable ? 1 : 0);
				}
			}
			return result;
		}

		private IList<Organism.Organism> CreateOrganisms(Tile[,] tiles)
		{
			var result = new List<Organism.Organism>();

			var dna = new List<Decision>();

			dna.Add(new Decision(new List<ICondition>{new IsTilePassable(-1, 0)}, new Move(Direction.Left)));

			var organism = new Organism.Organism(dna, Color.Violet, 10, 10, tiles);

			tiles[10, 3].Organism = organism;

			result.Add(organism);

			return result;
		}

		public void Load(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			_SpriteBatch = spriteBatch;
			_ContentManager = contentManager;

			foreach (var tile in _Tiles)
				tile.Load(_ContentManager, _SpriteBatch);

			foreach (var organism in _Organisms)
				organism.Load(_ContentManager, _SpriteBatch);
		}

		public void Update(GameTime gameTime)
		{
			_View.Update(gameTime);

			if (_Turbo.UpdateOrganisms(gameTime))
				return;

			foreach (var organism in _Organisms)
			{
				var action = organism.Update(gameTime);

				if (action != null)
					action.Execute(_Tiles, organism);
			}
		}

		public void Draw(GameTime gameTime)
		{
			var offset = -_View.Position;
			
			foreach (var tile in _Tiles)
				tile.Draw(gameTime, offset);
		}
	}
}