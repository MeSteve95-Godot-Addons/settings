using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class SettingsWindow : GridContainer
{
	[Export] public Array<Setting> Settings { get; private set; } = [];
	[ExportToolButton("Update Window")] private Callable UpdateWindowButton => Callable.From(UpdateWindow);

	public override void _Ready()
	{
		CallDeferred(MethodName.UpdateWindow);
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

			Control settingDisplay = SettingsMenu.Instance.GetSettingDisplayScene(setting).Instantiate<Control>();
			AddChild(settingDisplay);
			settingDisplay.Owner = this;

			((ISettingDisplay)settingDisplay).SetSetting(setting);
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
}
