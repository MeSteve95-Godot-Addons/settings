using Godot;
using Godot.Collections;

namespace smars.addons.settings;

[Tool]
[GlobalClass]
public partial class SettingsSection : Resource
{
	[Export] public string SectionName;
	[Export] public Array<Setting> Settings;
}
