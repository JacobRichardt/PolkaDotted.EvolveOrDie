using System;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions
{
	public interface IAction
	{
		void Execute(Tile[,] tiles, Organism organism);
		
		IAction Mutate(Random random);
	}
}