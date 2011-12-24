using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions
{
	public class CreateChild : IAction
	{
		private readonly Random _Random;
		private readonly Direction _Direction;

		private CreateChild(Random random, Direction direction)
		{
			_Random = random;
			_Direction = direction;
		}

		public void Execute(Tile[,] tiles, Organism organism)
		{
			throw new NotImplementedException();
		}

		public IAction Mutate(Random random)
		{
			return new CreateChild(random, (Direction)random.Next(4));
		}
	}
}