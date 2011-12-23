using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolkaDotted.EvolveOrDie
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class EvolveOrDieGame : Game
	{
		private readonly GraphicsDeviceManager _Graphics;
		private SpriteBatch _SpriteBatch;

		private SpriteFont _DefaultFont;

		private World.World _World;

		public EvolveOrDieGame()
		{
			_Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			IsFixedTimeStep = false;
			_Graphics.SynchronizeWithVerticalRetrace = false;
			Window.Title = "Evolve or Die";
			_Graphics.ApplyChanges();

			_World = new World.World();

			_World.Initialize(GraphicsDevice.Viewport);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			_SpriteBatch = new SpriteBatch(GraphicsDevice);

			_DefaultFont = Content.Load<SpriteFont>("Fonts/Default");

			_World.Load(Content, _SpriteBatch);

			base.LoadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			_World.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_SpriteBatch.Begin();

			_World.Draw(gameTime);

			_SpriteBatch.DrawString(_DefaultFont, string.Format("FPS: {0}", Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)), Vector2.Zero, Color.Green);

			_SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
