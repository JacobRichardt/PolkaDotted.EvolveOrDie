using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PolkaDotted.EvolveOrDie.GameObjects;
using System.Linq;

namespace PolkaDotted.EvolveOrDie
{
	public class Level
	{
		private const float ORGANISM_MOVE_TIME = 0.5f;
		private const uint MAX_ORGANISM_MOVES = 200;
		private const uint NUMBER_OF_ORGANISM = 50;
		private const float ORGANISMS_TO_KEEP = 0.2f;
		private const float DIVERSETY = 0.333f;

		private readonly Vector2 _Size;
		private readonly View _View;
		
		private readonly IList<IGameObject> _GameObjects;
		private readonly IList<Organism> _Organisms;
		
		private Tile[,] _Tiles;

		private TimeSpan _LastOrganismUpdate;
		private bool _IsInTurboMode;
		private bool _TurboButtonReleased = true;
		private uint _OrganismsNumberOfMoves;
		private ContentManager _ContentManager;
		private SpriteBatch _SpriteBatch;

		public Level(GraphicsDevice graphicsDevice)
		{
			_GameObjects = new List<IGameObject>();
			_Organisms = new List<Organism>();
			_Size = new Vector2(2000);
			_View = new View(graphicsDevice.Viewport, _Size);

			InitiateTiles();

			InitiateOrganisms();
		}

		private void InitiateTiles()
		{
			_Tiles = new Tile[(int)Math.Floor(_Size.X / Tile.TILE_SIZE),(int)Math.Floor(_Size.Y / Tile.TILE_SIZE)];

			var rand = new Random();

			for (uint x = 0; x < _Tiles.GetLength(0); x++)
			{
				for (uint y = 0; y < _Tiles.GetLength(1); y++)
				{
					var tile = new Tile(x != 0 && x != _Tiles.GetLength(0) - 1 && y != 0 && y != _Tiles.GetLength(1) - 1 && rand.Next(5) != 0, x, y);

					_Tiles[x, y] = tile;

					_GameObjects.Add(tile);
				}
			}
		}

		private void InitiateOrganisms()
		{
			var rand = new Random();

			for (int i = 0; i < NUMBER_OF_ORGANISM; i++)
			{
				var org = GetRandomOrganism(rand);
				_GameObjects.Add(org);
				_Organisms.Add(org);
			}
		}

		public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			_ContentManager = contentManager;
			_SpriteBatch = spriteBatch;
			
			foreach (var gameObject in _GameObjects)
				gameObject.Load(spriteBatch, contentManager);
		}

		public void Update(GameTime gameTime)
		{
			_View.Update(gameTime);

			if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _TurboButtonReleased)
			{
				_TurboButtonReleased = false;
				_IsInTurboMode = !_IsInTurboMode;
			}
			else if (Keyboard.GetState().IsKeyUp(Keys.Enter) && !_TurboButtonReleased)
				_TurboButtonReleased = true;

			var moveTime = gameTime.TotalGameTime.Subtract(_LastOrganismUpdate).TotalSeconds;

			if (!_IsInTurboMode && moveTime < ORGANISM_MOVE_TIME)
				return;

			_LastOrganismUpdate = gameTime.TotalGameTime;
			var foundAlive = false;

			foreach (var organism in _Organisms)
			{
				if(organism.IsDead || organism.HaveWon)
					continue;

				foundAlive = true;

				organism.Update();
			}

			if(!foundAlive || _OrganismsNumberOfMoves++ >= MAX_ORGANISM_MOVES)
				ResetOrganisms();
		}

		private void ResetOrganisms()
		{
			_OrganismsNumberOfMoves = 0;
			
			var result = _Organisms.OrderByDescending(org => org.Score);
			var numberToKeep = (uint)(NUMBER_OF_ORGANISM * ORGANISMS_TO_KEEP);
			var maximumPerFamily = (uint)Math.Round(numberToKeep * DIVERSETY);

			var keptFamilyCount = new Dictionary<Guid, uint>();
			var rand = new Random();

			foreach (var organism in result)
			{
				if(numberToKeep != 0)
				{
					if (!keptFamilyCount.ContainsKey(organism.OriginalAncestor))
						keptFamilyCount[organism.OriginalAncestor] = 1;
					
					if(keptFamilyCount[organism.OriginalAncestor] != maximumPerFamily )
					{
						keptFamilyCount[organism.OriginalAncestor]++;
						
						organism.Reset();

						AddOrganism(GetMutatedOrganism(organism, rand));
						AddOrganism(GetMutatedOrganism(organism, rand));
						AddOrganism(GetMutatedOrganism(organism, rand));

						numberToKeep--;

						continue;
					}
				}

				RemoveOrganism(organism);
			}

			while (_Organisms.Count < NUMBER_OF_ORGANISM)
				AddOrganism(GetRandomOrganism(rand));
		}

		private Organism GetMutatedOrganism(Organism organism, Random random)
		{
			var newDNA = new List<Descesion>();

			foreach (var descesion in organism.DNA)
			{
				var mutation = random.NextDouble();

				if (mutation < 0.9)
					newDNA.Add(descesion);
				else if (mutation < 0.95)
					newDNA.Add(GetMutatedDescesion(descesion, random));

				if (mutation > 0.95 && mutation < 0.97)
					newDNA.Add(GetRandomDescesion(random));
			}

			return new Organism(_Tiles, newDNA, Color.Lerp(organism.Family, new Color(random.Next(256), random.Next(256), random.Next(256)), 0.01f), organism.OriginalAncestor);
		}

		private Organism GetRandomOrganism(Random random)
		{
			var dna = new List<Descesion>();

			for (int i = 0; i < 10; i++)
			{
				dna.Add(GetRandomDescesion(random));
			}

			return new Organism(_Tiles, dna, new Color(random.Next(256), random.Next(256), random.Next(256)), Guid.NewGuid());
		}

		private Descesion GetMutatedDescesion(Descesion descesion, Random random)
		{
			return new Descesion((int) (descesion.X + Math.Round(random.NextDouble() * 2 - 1)), (int) (descesion.Y + Math.Round(random.NextDouble() * 2 - 1)), descesion.Direction, (float) (descesion.Weight * (random.NextDouble() * 0.4 - 1.2)));
		}

		private Descesion GetRandomDescesion(Random random)
		{
			return new Descesion(random.Next(-5, 5), random.Next(-5, 5), random.Next(4), (float)(random.NextDouble() * 2 - 1));
		}

		private void RemoveOrganism(Organism organism)
		{
			_Organisms.Remove(organism);
			_GameObjects.Remove(organism);
		}

		private void AddOrganism(Organism organism)
		{
			organism.Load(_SpriteBatch, _ContentManager);

			_Organisms.Add(organism);
			_GameObjects.Add(organism);
		}

		public void Draw(GameTime gameTime)
		{
			foreach (var gameObject in _GameObjects)
				gameObject.Draw(-_View.Position);
		}
	}
}