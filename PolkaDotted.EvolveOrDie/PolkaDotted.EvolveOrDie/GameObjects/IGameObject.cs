using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie.GameObjects
{
	public interface IGameObject
	{
		void Load(SpriteBatch spriteBatch, ContentManager contactManager);
		void Draw(Vector2 offset);
	}
}