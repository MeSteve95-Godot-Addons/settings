using Godot;

namespace smars.addons.settings;

[Tool]
[GlobalClass]
public abstract partial class Setting : Resource
{
	[Export] public string Name { get; private set; }
}
