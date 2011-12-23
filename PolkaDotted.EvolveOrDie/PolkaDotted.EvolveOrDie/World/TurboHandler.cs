using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PolkaDotted.EvolveOrDie.World
{
	public class TurboHandler
	{
		private const float ORGANISM_MOVE_TIME = 0.5f;

		private bool _TurboButtonReleased = true;
		private bool _IsInTurboMode;
		
		private TimeSpan _LastOrganismUpdate;

		public bool UpdateOrganisms(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _TurboButtonReleased)
			{
				_TurboButtonReleased = false;
				_IsInTurboMode = !_IsInTurboMode;
			}
			else if (Keyboard.GetState().IsKeyUp(Keys.Enter) && !_TurboButtonReleased)
				_TurboButtonReleased = true;

			var moveTime = gameTime.TotalGameTime.Subtract(_LastOrganismUpdate).TotalSeconds;

			if (!_IsInTurboMode && moveTime < ORGANISM_MOVE_TIME)
				return true;

			_LastOrganismUpdate = gameTime.TotalGameTime;

			return false;
		}
	}
}