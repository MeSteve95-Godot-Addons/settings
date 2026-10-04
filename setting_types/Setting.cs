using Godot;

namespace Smars.Addons.Settings;

[Tool]
[GlobalClass]
public abstract partial class Setting : Resource
{
	[Export] public string Name { get; private set; }
}
