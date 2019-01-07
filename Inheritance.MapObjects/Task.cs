namespace Inheritance.MapObjects {
	public interface IHasTreasure {
		Treasure Treasure { get; set; }
	}

	public interface IHasOwner {
		int Owner { get; set; }
	}

	public interface IHasArmy {
		Army Army { get; set; }
	}

	public class ResourcePile : IHasTreasure {
		public Treasure Treasure { get; set; }
	}

	public class Dwelling : IHasOwner {
		public int Owner { get; set; }
	}

	public class Wolfs : IHasArmy {
		public Army Army { get; set; }
	}

	public class Creeps : IHasArmy, IHasTreasure {
		public Army Army { get; set; }
		public Treasure Treasure { get; set; }
	}

	public class Mine : IHasOwner, IHasArmy, IHasTreasure {
		public int Owner { get; set; }
		public Army Army { get; set; }
		public Treasure Treasure { get; set; }
	}

	public static class Interaction {

		public static void Interact(this Player player, IHasTreasure valuable) {
			if (!player.Dead) {
				player.Consume(valuable.Treasure);
			}
		}

		public static void Interact(this Player player, IHasOwner owned) {
			if (!player.Dead) {
				owned.Owner = player.Id;
			}
		}

		public static void Interact(this Player player, IHasArmy guarded) {
			if (!player.CanBeat(guarded.Army)) {
				player.Die();
			}
		}

		public static void Make(Player player, object mapObject) {
			if (mapObject is IHasArmy) {
				player.Interact(mapObject as IHasArmy);
			}
			if (mapObject is IHasOwner) {
				player.Interact(mapObject as IHasOwner);
			}
			if (mapObject is IHasTreasure) {
				player.Interact(mapObject as IHasTreasure);
			}
		}
	}
}
