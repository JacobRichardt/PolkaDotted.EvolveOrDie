namespace PolkaDotted.EvolveOrDie.GameObjects
{
	public class Descesion
	{
		private readonly int _X;
		private readonly int _Y;
		private readonly int _Direction;
		private readonly float _Weight;

		public int X { get { return _X; } }
		public int Y { get { return _Y; } }
		public int Direction { get { return _Direction; } }
		public float Weight { get { return _Weight; } }

		public Descesion(int x, int y, int direction, float weight)
		{
			_X = x;
			_Y = y;
			_Direction = direction;
			_Weight = weight;
		}

		public void Decide(Tile[,] tiles, uint x, uint y, float[] directions)
		{
			var newX = x + X;
			var newY = y + Y;

			if (newX < 0 || newY < 0 || newX >= tiles.GetLength(0) || newY >= tiles.GetLength(1) || !tiles[newX, newY].IsSafe)
				return;

			directions[_Direction] += Weight;
		}
	}
}