namespace Incapsulation.Failures {
	public struct Device {
		public Device(int id, string name) {
			Id = id;
			Name = name;
		}
		
		public int Id {
			get;
			private set;
		}
		
		public string Name {
			get;
			private set;
		}
	}
}
