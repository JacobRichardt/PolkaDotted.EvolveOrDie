using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions
{
	public interface ICondition
	{
		bool IsFulfilled(Tile[,] tiles, Organism organism);

		ICondition Mutate(Random random);
	}
}