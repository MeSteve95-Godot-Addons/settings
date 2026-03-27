using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class KeyBindSetting : Setting
{
	[Export] public string InputActionName { get; private set; }
	
	public Array<InputEvent> InputEvents => InputMap.HasAction(InputActionName)
		? InputMap.ActionGetEvents(InputActionName)
		: [];

	public void AddInputEvent(InputEvent inputEvent)
	{
		if (!InputMap.HasAction(InputActionName))
			throw new Exception($"Invalid  action name: {InputActionName}.");
		
		if (!InputMap.ActionHasEvent(InputActionName, inputEvent))
			InputMap.ActionAddEvent(InputActionName, inputEvent);
	}

	public void RebindInputEvent(InputEvent oldInputEvent, InputEvent newInputEvent)
	{
		if (!InputMap.HasAction(InputActionName))
			throw new Exception($"Invalid  action name: {InputActionName}.");
		
		RemoveInputEvent(oldInputEvent);
		AddInputEvent(newInputEvent);
	}

	public void RemoveInputEvent(InputEvent inputEvent)
	{
		if (!InputMap.HasAction(InputActionName))
			throw new Exception($"Invalid  action name: {InputActionName}.");
		
		if (InputMap.ActionHasEvent(InputActionName, inputEvent))
			InputMap.ActionEraseEvent(InputActionName, inputEvent);
	}
}
