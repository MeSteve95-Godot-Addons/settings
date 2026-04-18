using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class SettingsSection : Resource
{
	[Export] public string SectionName;
	[Export] public Array<Setting> Settings;
}
