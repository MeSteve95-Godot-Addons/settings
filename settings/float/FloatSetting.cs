using Godot;
using System;

[Tool]
[GlobalClass]
public partial class FloatSetting : Setting
{
	private float _value;
	[Export] public float Value
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

	[Export] public float DefaultValue { get; private set; }
	
	private float _minValue;
	[Export] public float MinValue
	{
		get => _minValue;
		private set
		{
			_minValue = value;
			RecalculateValue();
		}
	}
	private float _maxValue;
	[Export] public float MaxValue
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
