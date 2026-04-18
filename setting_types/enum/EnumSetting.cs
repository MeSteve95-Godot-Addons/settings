using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class EnumSetting : Setting
{
	[Signal] public delegate void ValueChangedEventHandler(int newId);
	
	[Export] public virtual Dictionary<int, string> IdToLabel { get; protected set; } = new();
	[Export] public virtual Array<int> IgnoredIds { get; protected set; } = [];
	
	private int _selectedId = -1;
	[Export] public int SelectedId
	{
		get => _selectedId;
		set
		{
			if (!IdToLabel.ContainsKey(value))
				throw new Exception($"Trying to set invalid enum ID: {value}.");
			
			_selectedId = value;
			OnValueChanged(_selectedId);
			EmitSignal(SignalName.ValueChanged, _selectedId);
		}
	}
	
	public virtual int DefaultSelectedId { get; protected set; } = -1;

	protected virtual void OnValueChanged(int newId) {}
}
