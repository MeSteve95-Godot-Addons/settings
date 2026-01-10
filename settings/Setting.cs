using Godot;

[GlobalClass]
public abstract partial class Setting : Resource
{
	[Signal] public delegate void ValueChangedEventHandler();
	
	[Export] public string Name { get; private set; }
}
