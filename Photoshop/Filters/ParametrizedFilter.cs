namespace MyPhotoshop {
	public abstract class ParametrizedFilter<TParameters> : IFilter where TParameters : IParameters, new() {
		private readonly IParemeterHandler<TParameters> handler = new ExpressionParameterHandler<TParameters>();
		private string name;

		protected ParametrizedFilter(string name) {
			this.name = name;
		}

		public ParameterInfo[] GetParameters() {
			return handler.GetDesсription();
		}

		public Photo Process(Photo original, double[] values) {
			var parameters = handler.CreateParameters(values);
			return Process(original, parameters);
		}

		public override string ToString() {
			return name;
		}

		protected abstract Photo Process(Photo photo, TParameters parameters);
	}
}
