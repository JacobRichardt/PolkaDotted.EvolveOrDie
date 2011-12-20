using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie.GameObjects
{
	public class Tile : IGameObject
	{
		public const uint TILE_SIZE = 50;

		private Texture2D _Texture;
		private SpriteBatch _SpriteBatch;
		private readonly Vector2 _DrawPosition;

		public bool IsSafe { get; set; }

		public Tile(bool isSafe, uint x, uint y)
		{
			_DrawPosition = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
			IsSafe = isSafe;
		}

		public void Load(SpriteBatch spriteBatch, ContentManager contactManager)
		{
			_SpriteBatch = spriteBatch;

			_Texture = contactManager.Load<Texture2D>(IsSafe ? "SafeTile" : "DangerTile");
		}

		public void Draw(Vector2 offset)
		{
			_SpriteBatch.Draw(_Texture, _DrawPosition + offset, Color.White);
		}
	}
}