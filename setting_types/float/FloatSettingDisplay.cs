using Godot;

namespace Smars.Addons.Settings;

[Tool]
public partial class FloatSettingDisplay : Control, ISettingDisplay
{
	[ExportGroup("Internal Scene References")]
	[Export] private SpinBox _spinBox;
	[Export] private HSlider _slider;

	private FloatSetting _floatSetting;
	private FloatSetting FloatSetting
	{
		get => _floatSetting;
		set
		{
			if (_floatSetting is not null)
				_floatSetting.ValueChanged -= OnFloatSettingValueChanged;

			_floatSetting = value;

			if (_floatSetting is null)
				return;

			_floatSetting.ValueChanged += OnFloatSettingValueChanged;

			if (_spinBox is not null)
			{
				if (_spinBox.IsConnected(Range.SignalName.ValueChanged, _spinBoxValueChanged))
					_spinBox.Disconnect(Range.SignalName.ValueChanged, _spinBoxValueChanged);

				_spinBox.MinValue = _floatSetting.MinValue;
				_spinBox.MaxValue = _floatSetting.MaxValue;
				_spinBox.Value = _floatSetting.Value;
				_spinBox.Connect(Range.SignalName.ValueChanged, _spinBoxValueChanged);
			}

			if (_slider is not null)
			{
				if (_slider.IsConnected(Range.SignalName.ValueChanged, _sliderValueChanged))
					_slider.Disconnect(Range.SignalName.ValueChanged, _sliderValueChanged);

				_slider.MinValue = _floatSetting.MinValue;
				_slider.MaxValue = _floatSetting.MaxValue;
				_slider.Value = _floatSetting.Value;
				_slider.Connect(Range.SignalName.ValueChanged, _sliderValueChanged);
			}
		}
	}

	private Callable _spinBoxValueChanged;
	private Callable _sliderValueChanged;

	public void SetSetting(Setting setting) => FloatSetting = (FloatSetting)setting;

	private const float STEP = 0.01f;

	public override void _Ready()
	{
		_spinBoxValueChanged = new Callable(this, MethodName.OnSpinBoxValueChanged);
		_sliderValueChanged = new Callable(this, MethodName.OnSliderValueChanged);
		_spinBox.Step = STEP;
		_slider.Step = STEP;
	}

	public override void _ExitTree()
	{
		if (FloatSetting is not null)
			FloatSetting.ValueChanged -= OnFloatSettingValueChanged;
	}

	private void OnSpinBoxValueChanged(double newValue)
	{
		if (FloatSetting is not null && Mathf.Abs(FloatSetting.Value - newValue) > 1e-6)
			FloatSetting.SetValue((float)newValue);

		if (Mathf.Abs(_slider.Value - newValue) > 1e-6)
			_slider.Value = newValue;
	}

	private void OnSliderValueChanged(double newValue)
	{
		if (FloatSetting is not null && Mathf.Abs(FloatSetting.Value - newValue) > 1e-6)
			FloatSetting.SetValue((float)newValue);

		if (Mathf.Abs(_spinBox.Value - newValue) > 1e-6)
			_spinBox.Value = newValue;
	}

	private void OnFloatSettingValueChanged(float newValue)
	{
		if (Mathf.Abs(_slider.Value - newValue) > 1e-6)
			_slider.Value = newValue;

		if (Mathf.Abs(_spinBox.Value - newValue) > 1e-6)
			_spinBox.Value = newValue;
	}
}
