namespace TaSked.Application.Common;

public class Optional<T>
{
	private readonly T? _value;
	public bool IsSet { get; }
	public bool IsEmpty => !IsSet;
	public T Value => _value!;

	public Optional()
	{
		IsSet = false;
	}

	public Optional(T? value)
	{
		_value = value;
		IsSet = true;
	}

	public bool TryGetValue(out T? value)
	{
		value = _value;
		return IsSet;
	}

	public void IfSet(Action<T> action)
	{
		if (IsSet)
		{
			action(_value!);
		}
	}
}
