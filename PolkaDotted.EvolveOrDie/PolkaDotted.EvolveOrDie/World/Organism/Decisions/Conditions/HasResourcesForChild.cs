using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions
{
	public class HasResourcesForChild : ICondition
	{
		public bool IsFulfilled(Tile[,] tiles, Organism organism)
		{
			return organism.CurrentResources == organism.ResourcesRequirement;
		}

		public ICondition Mutate(Random random)
		{
			return new HasResourcesForChild();
		}
	}
}