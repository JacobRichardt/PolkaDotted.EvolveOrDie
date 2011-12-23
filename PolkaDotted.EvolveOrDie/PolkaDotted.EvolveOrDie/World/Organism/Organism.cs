using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions;

namespace PolkaDotted.EvolveOrDie.World.Organism
{
	public class Organism
	{
		private Texture2D _Texture;
		private SpriteBatch _SpriteBatch;
		
		public int Health { get; private set; }
		public int Attack { get; private set; }

		public int ResourcesRequirement { get { return Health + Attack; } }

		public int CurrentHealth { get; set; }
		public int CurrentResources { get; set; }

		public Color Color { get; private set; }

		public IList<Decision> DNA { get; private set; }

		public Tile Tile { get; set; }
		private readonly Tile[,] _Tiles;

		public Organism(IList<Decision> dna, Color color, int health, int attack, Tile[,] tiles)
		{
			DNA = dna;
			Color = color;
			Health = health;
			Attack = attack;
			_Tiles = tiles;
		}

		public void Load(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			_Texture = contentManager.Load<Texture2D>("Organism");
			_SpriteBatch = spriteBatch;
		}

		public void Draw(GameTime gameTime, Vector2 coordinate)
		{
			_SpriteBatch.Draw(_Texture, coordinate, Color);
		}

		public IAction Update(GameTime gameTime)
		{
			foreach (var decision in DNA)
			{
				if (decision.TakeDescision(_Tiles, this))
					return decision.Action;
			}

			return null;
		}
	}
}