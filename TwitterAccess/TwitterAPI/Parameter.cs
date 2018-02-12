
namespace TwitterAccess.TwitterAPI
{
	public enum ParamStatus
	{
		Required,
		Optional
	}

	public enum ParamType
	{
		number,
		text,
		boolean
	}

	public class Parameter
	{
		public string Key { get; set; }

		public string Value { get; set; }

		public ParamStatus Status { get; set; }

		public ParamType ParamType { get; set; }

		public int MaxValue { get; set; }

		public Parameter() { }

		public Parameter(string key, ParamStatus status)
		{
			Key = key;
			Status = status;
			ParamType = ParamType.text;
		}

		public Parameter(string key, ParamStatus status, ParamType paramType)
		{
			Key = key;
			Status = status;
			ParamType = paramType;
		}

		public Parameter(string key, ParamStatus status, ParamType paramType, int maxValue)
		{
			Key = key;
			Status = status;
			ParamType = paramType;
			MaxValue = maxValue;
		}
	}
}
