using Godot;
using Godot.Collections;

namespace Smars.Addons.Settings;

[Tool]
[GlobalClass]
public partial class SettingsSection : Resource
{
	[Export] public string SectionName;
	[Export] public Array<Setting> Settings;
}
