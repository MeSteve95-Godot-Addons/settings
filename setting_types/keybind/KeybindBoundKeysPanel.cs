using Godot;
using System;

namespace smars.addons.settings;

[Tool]
public partial class KeybindBoundKeysPanel : Control
{
	[Signal] public delegate void RebindInputEventEventHandler(InputEvent oldInputEvent, InputEvent newInputEvent);
	[Signal] public delegate void DeleteInputEventEventHandler(InputEvent inputEvent);

	[ExportGroup("Internal Scene References")]
	[Export] private KeybindRebindButton _rebindButton;
	[Export] private Button _deleteButton;

	private InputEvent _inputEvent;
	public InputEvent InputEvent
	{
		get => _inputEvent;
		set
		{
			_inputEvent = value;

			if (_inputEvent is not null)
				_rebindButton.DefaultTooltipText = _inputEvent.AsText();
		}
	}

	public override void _Ready()
	{
		_rebindButton.InputEventAssigned += OnRebindButtonInputEventAssigned;
		_deleteButton.Pressed += OnDeleteButtonPressed;
	}

	private void OnRebindButtonInputEventAssigned(InputEvent inputEvent)
	{
		if (InputEvent is null)
			throw new NullReferenceException("InputEvent is null");

		EmitSignal(SignalName.RebindInputEvent, InputEvent, inputEvent);
	}

	private void OnDeleteButtonPressed()
	{
		if (InputEvent is null)
			throw new NullReferenceException("InputEvent is null");

		EmitSignal(SignalName.DeleteInputEvent, InputEvent);
	}
}
