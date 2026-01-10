using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class SettingsWindow : GridContainer
{
	[Export] public Array<Setting> Settings { get; private set; } = [];
	[ExportToolButton("Update Window")] private Callable UpdateWindowButton => Callable.From(UpdateWindow);

	public override void _Ready()
	{
		UpdateWindow();
	}

	private void UpdateWindow()
	{
		foreach (Node child in GetChildren())
			child.QueueFree();
		
		foreach (Setting setting in Settings)
		{
			if (setting is null)
				continue;

			Label nameLabel = CreateSettingNameLabel(setting);
			AddChild(nameLabel);
			nameLabel.Owner = this;

			VSeparator separator = CreateSettingSeparator();
			AddChild(separator);
			separator.Owner = this;

			Control settingDisplay = setting switch
			{
				IntSetting intSetting => CreateIntSettingDisplay(intSetting),
				FloatSetting floatSetting => CreateFloatSettingDisplay(floatSetting),
				_ => throw new ArgumentException($"Unsupported setting type: {setting.GetType()}."),
			};
			AddChild(settingDisplay);
			settingDisplay.Owner = this;
		}
	}

	private static Label CreateSettingNameLabel(Setting setting)
	{
		Label nameLabel = new()
		{
			Text = setting.Name,
			VerticalAlignment = VerticalAlignment.Center,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
		};
		return nameLabel;
	}

	private static VSeparator CreateSettingSeparator()
	{
		VSeparator separator = new()
		{
			SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
		};
		return separator;
	}

	private static SpinBox CreateIntSettingDisplay(IntSetting intSetting)
	{
		GD.Print(intSetting.Value);
		SpinBox intSpinBox = new()
		{
			MinValue = intSetting.MinValue,
			MaxValue = intSetting.MaxValue,
			Value = intSetting.Value,
			Rounded = true,
		};

		intSpinBox.ValueChanged += value =>
		{
			int spinBoxValue = Mathf.RoundToInt(value);
			if (spinBoxValue != intSetting.Value)
				intSetting.Value = spinBoxValue;
		};

		intSetting.ValueChanged += () =>
		{
			int spinBoxValue = Mathf.RoundToInt(intSpinBox.Value);
			if (intSetting.Value != spinBoxValue)
				intSpinBox.Value = intSetting.Value;
		};

		return intSpinBox;
	}

	private static HBoxContainer CreateFloatSettingDisplay(FloatSetting floatSetting)
	{
		const float step = 0.01f;
		
		HBoxContainer hBoxContainer = new()
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
		};

		SpinBox spinBox = new()
		{
			MinValue = floatSetting.MinValue,
			MaxValue = floatSetting.MaxValue,
			Step = step,
			Value = floatSetting.Value,
		};
		hBoxContainer.AddChild(spinBox);
		spinBox.Owner = hBoxContainer;

		HSlider hSlider = new()
		{
			TickCount = 10,
			MinValue = floatSetting.MinValue,
			MaxValue = floatSetting.MaxValue,
			Step = step,
			Value = floatSetting.Value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
		};
		hBoxContainer.AddChild(hSlider);
		hSlider.Owner = hBoxContainer;
		
		spinBox.ValueChanged += value =>
		{
			if (Math.Abs(value - floatSetting.Value) > 1e-6)
				floatSetting.Value = (float)value;
			
			if (Math.Abs(value - hSlider.Value) > 1e-6)
				hSlider.Value = floatSetting.Value;
		};
		hSlider.ValueChanged += value =>
		{
			if (Math.Abs(value - floatSetting.Value) > 1e-6)
				floatSetting.Value = (float)value;
			
			if (Math.Abs(value - spinBox.Value) > 1e-6)
				spinBox.Value = floatSetting.Value;
		};

		floatSetting.ValueChanged += () =>
		{
			if (Math.Abs(floatSetting.Value - hSlider.Value) > 1e-6)
				hSlider.Value = floatSetting.Value;
			
			if (Math.Abs(floatSetting.Value - spinBox.Value) > 1e-6)
				spinBox.Value = floatSetting.Value;
		};
		
		return hBoxContainer;
	}
}
