using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions
{
	public class HaveNumberOfResources : ICondition
	{
		private readonly int _NumberOfResources;

		public HaveNumberOfResources(int numberOfResources)
		{
			_NumberOfResources = numberOfResources;
		}


		public bool IsFulfilled(Tile[,] tiles, Organism organism)
		{
			return organism.CurrentResources >= _NumberOfResources;
		}

		public ICondition Mutate(Random random)
		{
			return new HaveNumberOfResources(Math.Max(0, _NumberOfResources + random.Next(3) - 1));
		}
	}
}