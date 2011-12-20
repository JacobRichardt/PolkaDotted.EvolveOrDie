using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie
{
	public class Sprite
	{
		private readonly Texture2D _Texture;
		private readonly SpriteBatch _SpriteBatch;

		public Sprite(Texture2D texture, SpriteBatch spriteBatch)
		{
			_Texture = texture;
			_SpriteBatch = spriteBatch;
		}

		public void Draw(Vector2 position)
		{
			_SpriteBatch.Draw(_Texture, position, Color.White);
		}
	}
}