using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PolkaDotted.EvolveOrDie.World
{
	public class View
	{
		private const float VIEW_MOVE_SPEED = 1;

		private readonly Viewport _Viewport;
		private readonly Vector2 _Size;

		private Vector2 _Position;
		public Vector2 Position { get { return _Position; } }

		public View(Viewport viewport, Vector2 size)
		{
			_Viewport = viewport;
			_Size = size;
			_Position = Vector2.Zero;
		}

		public void Update(GameTime gameTime)
		{
			var keyboard = Keyboard.GetState();

			if (keyboard.IsKeyDown(Keys.Up))
				_Position.Y -= (float)(VIEW_MOVE_SPEED * gameTime.ElapsedGameTime.TotalMilliseconds);
			else if (keyboard.IsKeyDown(Keys.Down))
				_Position.Y += (float)(VIEW_MOVE_SPEED * gameTime.ElapsedGameTime.TotalMilliseconds);

			if (keyboard.IsKeyDown(Keys.Left))
				_Position.X -= (float)(VIEW_MOVE_SPEED * gameTime.ElapsedGameTime.TotalMilliseconds);
			else if (keyboard.IsKeyDown(Keys.Right))
				_Position.X += (float)(VIEW_MOVE_SPEED * gameTime.ElapsedGameTime.TotalMilliseconds);

			if (Position.X < 0)
				_Position.X = 0;
			else if (Position.X + _Viewport.Width > _Size.X)
				_Position.X = _Size.X - _Viewport.Width;

			if (_Position.Y < 0)
				_Position.Y = 0;
			else if (_Position.Y + _Viewport.Height > _Size.Y)
				_Position.Y = _Size.Y - _Viewport.Height;
		}
	}
}