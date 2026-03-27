using Godot;
using System;

[Tool]
[GlobalClass]
public partial class IntSetting : Setting, ISerializationListener
{
	[Signal] public delegate void ValueChangedEventHandler(int newValue);
	
	private bool _loadComplete;
	
	private int _value;
	[Export] public int Value
	{
		get => _value;
		private set
		{
			if (value == _value)
				return;

			_value = value;
			EmitSignal(SignalName.ValueChanged, _value);
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
			
			if (_loadComplete)
				SetValue(Value);
		}
	}
	private int _maxValue;
	[Export] public int MaxValue
	{
		get => _maxValue;
		private set
		{
			_maxValue = value;
			
			if (_loadComplete)
				SetValue(Value);
		}
	}

	public void OnBeforeSerialize() {}
	public void OnAfterDeserialize()
	{
		_loadComplete = true;
		SetValue(DefaultValue);
	}

	public void SetValue(int newValue)
	{
		if (MinValue > MaxValue)
			throw new Exception($"Invalid constraints. MinValue ({MinValue}) must be less than MaxValue ({MaxValue}).");
		
		Value = Mathf.Clamp(newValue, MinValue, MaxValue);
	}
}
