using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie.GameObjects
{
	public class Organism : IGameObject
	{
		private readonly Tile[,] _Tiles;
		private SpriteBatch _SpriteBatch;
		private Texture2D _Texture;

		
		private readonly Color _Family;
		public Color Family { get { return _Family; } }

		private readonly Guid _OriginalAncestor;
		public Guid OriginalAncestor { get { return _OriginalAncestor; }}

		private readonly IList<Descesion> _DNA;
		public IList<Descesion> DNA { get { return _DNA; } }
		

		public uint X { get; private set; }
		public uint Y { get; private set; }

		private uint _LastX;
		private uint _LastY;
		private bool _InLoop;

		public uint NumberOfMoves { get; private set; }
		public bool IsDead { get { return _InLoop || !_Tiles[X, Y].IsSafe; } }
		public bool HaveWon { get { return X == _Tiles.GetLength(0) - 2 && Y == _Tiles.GetLength(1) - 2; } }
		public int Score { get { return (int) ((X + Y) * (HaveWon ? 200 : 100) - NumberOfMoves); } }

		

		public Organism(Tile[,] tiles, IList<Descesion> dna, Color family, Guid originalAncestor)
		{
			_Tiles = tiles;
			_DNA = dna;
			_Family = family;
			_OriginalAncestor = originalAncestor;

			Reset();
		}

		public void Load(SpriteBatch spriteBatch, ContentManager contactManager)
		{
			_SpriteBatch = spriteBatch;
			_Texture = contactManager.Load<Texture2D>("Organism");
		}

		public void Reset()
		{
			NumberOfMoves = 0;
			X = 1;
			Y = 1;

			_LastX = 0;
			_LastY = 0;
			_InLoop = false;
		}

		public void Update()
		{
			if(HaveWon || IsDead)
				return;
			
			var direction = new float[4];

			foreach (var descesion in _DNA)
				descesion.Decide(_Tiles, X, Y, direction);

			var highest = 0;

			for (var i = 1; i < direction.Length; i++)
			{
				if(direction[i] > direction[highest])
					highest = i;
			}

			switch (highest)
			{
				case 0:
					if(X != 0)
						X--;
					break;
				case 1:
					if(X != _Tiles.GetLength(0) - 1)
						X++;
					break;
				case 2:
					if(Y != 0)
						Y--;
					break;
				case 3:
					if(Y != _Tiles.GetLength(1) - 1)
						Y++;
					break;
			}

			NumberOfMoves++;

			if(NumberOfMoves % 2 == 0)
			{
				if (X == _LastX && Y == _LastY)
					_InLoop = true;
				else
				{
					_LastX = X;
					_LastY = Y;
				}
			}
		}

		public void Draw(Vector2 offset)
		{
			_SpriteBatch.Draw(_Texture, new Vector2(X * Tile.TILE_SIZE, Y * Tile.TILE_SIZE) + offset, Family);
		}
	}
}