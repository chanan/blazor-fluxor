namespace CounterSample.Client.Store.Counter
{
	public class CounterState
	{
		public int Value { get; private set; }

		public CounterState(int value)
		{
			Value = value;
		}
	}
}
