namespace MyPhotoshop {
	public interface IParemeterHandler<out TParameters> {
		ParameterInfo[] GetDesсription();
		TParameters CreateParameters(double[] values);
	}
}