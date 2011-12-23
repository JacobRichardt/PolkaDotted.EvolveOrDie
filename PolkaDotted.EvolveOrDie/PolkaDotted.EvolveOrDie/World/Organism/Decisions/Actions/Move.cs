using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions
{
	public class Move : IAction
	{
		private Direction _Direction;

		public Move(Direction direction)
		{
			_Direction = direction;
		}

		public void Execute(Tile[,] tiles, Organism organism)
		{
			var newX = organism.Tile.Coordinate.X;
			var newY = organism.Tile.Coordinate.Y;

			switch (_Direction)
			{
				case Direction.Left:
					newX--;
					break;
				case Direction.Right:
					newX++;
					break;
				case Direction.Up:
					newY--;
					break;
				case Direction.Down:
					newY++;
					break;
			}

			if (newX < 0 || newY < 0 || newX >= tiles.GetLength(0) || newY >= tiles.GetLength(1))
				return;

			tiles[newX, newY].Organism = organism;
		}

		public IAction Mutate(Random random)
		{
			return new Move((Direction) random.Next(4));
		}
	}
}