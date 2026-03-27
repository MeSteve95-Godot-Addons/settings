using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class SettingsMenu : Control
{
	public static SettingsMenu Instance { get; private set; }
	
	[Export] public Dictionary<Script, PackedScene> SettingToDisplayScene { get; private set; } = new();

	public override void _Ready()
	{
		if (Instance is not null && !Engine.IsEditorHint())
			throw new Exception("Multiple instances of singleton.");

		Instance = this;
	}

	public PackedScene GetSettingDisplayScene(Setting setting)
	{
		Variant? scriptVariant = setting.GetScript();
		if (scriptVariant is null)
			throw new NullReferenceException($"Setting has no script attached. Setting: {setting.Name}");

		Script script = (Script)scriptVariant.Value;
		return SettingToDisplayScene[script];
	}
}
