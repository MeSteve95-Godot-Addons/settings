using Godot;
using System;

[Tool]
[GlobalClass]
public partial class IntSetting : Setting
{
	private int _value;
	[Export] public int Value
	{
		get => _value;
		set
		{
			if (MinValue > MaxValue)
				throw new Exception($"Invalid constraints. MinValue ({MinValue}) must be less than MaxValue ({MaxValue}).");
		
			_value = Mathf.Clamp(value, MinValue, MaxValue);
			EmitSignal(Setting.SignalName.ValueChanged);
		}
	}

	[Export] public int DefaultValue { get; private set; }

	private int _minValue;
	[Export] public int MinValue
	{
		get => _minValue;
		private set
		{
			_minValue = value;
			RecalculateValue();
		}
	}
	private int _maxValue;
	[Export] public int MaxValue
	{
		get => _maxValue;
		private set
		{
			_maxValue = value;
			RecalculateValue();
		}
	}

	private void RecalculateValue()
	{
		Value = _value;
	}
}
