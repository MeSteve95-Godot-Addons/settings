using Godot;

[Tool]
public partial class IntSettingDisplay : SpinBox, ISettingDisplay
{
	private IntSetting _intSetting;
	private IntSetting IntSetting
	{
		get => _intSetting;
		set
		{
			if (_intSetting is not null)
				_intSetting.ValueChanged -= OnIntSettingValueChanged;

			_intSetting = value;

			if (_intSetting is null)
				return;

			_intSetting.ValueChanged += OnIntSettingValueChanged;
			MinValue = _intSetting.MinValue;
			MaxValue = _intSetting.MaxValue;
			Value = _intSetting.Value;
		}
	}

	public override void _ExitTree()
	{
		if (IntSetting is not null)
			IntSetting.ValueChanged -= OnIntSettingValueChanged;
	}

	public void SetSetting(Setting setting) => IntSetting = (IntSetting)setting;

	public override void _ValueChanged(double newValue)
	{
		int roundedValue = Mathf.RoundToInt(newValue);
		if (IntSetting is not null && IntSetting.Value != roundedValue)
			IntSetting.SetValue(roundedValue);
	}

	private void OnIntSettingValueChanged(int newValue)
	{
		int roundedValue = Mathf.RoundToInt(Value);
		if (roundedValue != newValue)
			Value = newValue;
	}
}
