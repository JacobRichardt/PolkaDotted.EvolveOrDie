using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions
{
	public class IsTilePassable : ICondition
	{
		private readonly int _X;
		private readonly int _Y;
		
		public IsTilePassable(int x, int y)
		{
			_X = x;
			_Y = y;
		}

		public bool IsFulfilled(Tile[,] tiles, Organism organism)
		{
			var absoluteX = organism.Tile.Coordinate.X + _X;
			var absoluteY = organism.Tile.Coordinate.Y + _Y;

			if (absoluteX < 0 || absoluteY < 0 || absoluteX >= tiles.GetLength(0) || absoluteY >= tiles.GetLength(1))
				return false;

			return tiles[absoluteX, absoluteY].IsPassable;
		}

		public ICondition Mutate(Random random)
		{
			return new IsTilePassable(_X + random.Next(-1, 2), _Y + random.Next(-1, 2));
		}
	}
}