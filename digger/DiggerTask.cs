using System;
using System.Drawing;
using System.Windows.Forms;

namespace Digger
{
	class Terrain : ICreature
	{
		public string GetImageFileName() => "Terrain.png";
		public int GetDrawingPriority() => 0;
		public CreatureCommand Act(int x, int y) => new CreatureCommand();
		public bool DeadInConflict(ICreature enemy) => enemy is Player;
	}

	class Player : ICreature
	{
		public string GetImageFileName() => "Digger.png";
		public int GetDrawingPriority() => 1;

		public CreatureCommand Act(int x, int y)
		{
			var cmd = HandleKeys();

			if (x + cmd.DeltaX < 0 || x + cmd.DeltaX > Game.MapWidth - 1)
			{
				cmd.DeltaX = 0;
			}

			if (y + cmd.DeltaY < 0 || y + cmd.DeltaY > Game.MapHeight - 1)
			{
				cmd.DeltaY = 0;
			}

			var nextEntity = Game.Map[x + cmd.DeltaX, y + cmd.DeltaY];
			if (nextEntity is Gold)
			{
				Game.Scores += 10;
			}
			else if (nextEntity is Sack)
			{
				cmd.DeltaX = 0;
				cmd.DeltaY = 0;
			}

			return cmd;
		}

		private CreatureCommand HandleKeys()
		{
			var cmd = new CreatureCommand();
			if (Game.KeyPressed == Keys.Left)
			{
				cmd.DeltaX = -1;
			}
			else if (Game.KeyPressed == Keys.Right)
			{
				cmd.DeltaX = 1;
			}
			else if (Game.KeyPressed == Keys.Up)
			{
				cmd.DeltaY = -1;
			}
			else if (Game.KeyPressed == Keys.Down)
			{
				cmd.DeltaY = 1;
			}

			return cmd;
		}

		public bool DeadInConflict(ICreature enemy)
		{
			return enemy is Sack || enemy is Monster;
		}
	}

	class Sack : ICreature
	{
		private int fallDistance;
		private bool isFalling;

		public string GetImageFileName() => "Sack.png";
		public int GetDrawingPriority() => 1;

		public CreatureCommand Act(int x, int y)
		{
			var cmd = new CreatureCommand();
			AccountFallDistance();

			if (HasFreeWay(x, y))
			{
				HandleFreeWay(cmd);
			}
			else
			{
				HandleBlockedWay(cmd);
			}

			return cmd;
		}

		private bool HasFreeWay(int x, int y)
		{
			var lowerCell = y + 1;
			if (lowerCell > Game.MapHeight - 1)
			{
				return false;
			}

			var lowerEntity = Game.Map[x, lowerCell];
			var playerInDanger = lowerEntity is Player && fallDistance > 0;
			var monsterInDanger = lowerEntity is Monster && fallDistance > 0;
			return lowerEntity == null || playerInDanger || monsterInDanger;
		}

		private void AccountFallDistance()
		{
			if (isFalling)
			{
				fallDistance++;
			}
			else
			{
				fallDistance = 0;
			}
		}

		private void HandleFreeWay(CreatureCommand cmd)
		{
			cmd.DeltaY = 1;
			isFalling = true;
		}

		private void HandleBlockedWay(CreatureCommand cmd)
		{
			if (fallDistance > 1)
			{
				cmd.TransformTo = new Gold();
			}

			isFalling = false;
		}

		public bool DeadInConflict(ICreature enemy) => false;
	}

	class Gold : ICreature
	{
		public string GetImageFileName() => "Gold.png";
		public int GetDrawingPriority() => 2;
		public CreatureCommand Act(int x, int y) => new CreatureCommand();

		public bool DeadInConflict(ICreature enemy)
		{
			return enemy is Player || enemy is Monster;
		}
	}

	class Monster : ICreature
	{
		public string GetImageFileName() => "Monster.png";
		public int GetDrawingPriority() => 2;

		public CreatureCommand Act(int x, int y)
		{
			var cmd = new CreatureCommand();
			var diggerPos = GetDiggerPosition();
			if (diggerPos == null)
			{
				return cmd;
			}

			var deltaX = Math.Sign(diggerPos.Value.X - x);
			var deltaY = Math.Sign(diggerPos.Value.Y - y);
			(cmd.DeltaX, cmd.DeltaY) = Move(x, y, deltaX, 0) ?? Move(x, y, 0, deltaY) ?? (0, 0);

			return cmd;
		}

		private (int DeltaX, int DeltaY)? Move(int x, int y, int deltaX, int deltaY)
		{
			var nextCell = Game.Map[x + deltaX, y + deltaY];
			if (nextCell == null || nextCell is Player || nextCell is Gold)
			{
				return (deltaX, deltaY);
			}

			return null;
		}

		public bool DeadInConflict(ICreature enemy)
		{
			return enemy is Monster || enemy is Sack;
		}

		private Point? GetDiggerPosition()
		{
			for (var x = 0; x < Game.MapWidth; x++)
			{
				for (var y = 0; y < Game.MapHeight; y++)
				{
					if (Game.Map[x, y] is Player)
					{
						return new Point(x, y);
					}
				}
			}

			return null;
		}
	}
}