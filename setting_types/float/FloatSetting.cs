using Godot;
using System;

namespace smars.addons.settings;

[Tool]
[GlobalClass]
public partial class FloatSetting : Setting, ISerializationListener
{
	[Signal] public delegate void ValueChangedEventHandler(float newValue);

	private bool _loadComplete;

	private float _value;
	[Export] public float Value
	{
		get => _value;
		private set
		{
			_value = value;
			EmitSignal(SignalName.ValueChanged, _value);
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

			if (_loadComplete)
				SetValue(Value);
		}
	}
	private float _maxValue;
	[Export] public float MaxValue
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

	public void SetValue(float newValue)
	{
		if (MinValue > MaxValue)
			throw new Exception($"Invalid constraints. MinValue ({MinValue}) must be less than MaxValue ({MaxValue}).");

		Value = Mathf.Clamp(newValue, MinValue, MaxValue);
	}
}
