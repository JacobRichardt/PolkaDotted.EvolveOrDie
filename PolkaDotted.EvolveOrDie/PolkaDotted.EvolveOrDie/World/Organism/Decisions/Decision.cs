using System.Collections.Generic;
using System.Linq;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions.Actions;
using PolkaDotted.EvolveOrDie.World.Organism.Decisions.Conditions;

namespace PolkaDotted.EvolveOrDie.World.Organism.Decisions
{
	public class Decision
	{
		public IList<ICondition> Conditions { get; private set; }
		public IAction Action { get; private set; }

		public Decision(IList<ICondition> conditions, IAction action)
		{
			Conditions = conditions;
			Action = action;
		}

		public bool TakeDescision(Tile[,] tiles, Organism organism)
		{
			return Conditions.All(condition => condition.IsFulfilled(tiles, organism));
		}
	}
}