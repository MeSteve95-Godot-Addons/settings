using Godot;

namespace Smars.Addons.Settings;

[Tool]
public partial class KeybindRebindButton : Button
{
	[Signal] public delegate void InputEventAssignedEventHandler(InputEvent inputEvent);

	private string _defaultTooltipText;
	[Export]
	public string DefaultTooltipText
	{
		get => _defaultTooltipText;
		set
		{
			_defaultTooltipText = value;
			Text = _defaultTooltipText;
		}
	}

	private bool _isRebinding;

	private const string REBINDING_TOOLTIP_TEXT = "Press new button to rebind...";

	public override void _Ready()
	{
		Text = _defaultTooltipText;
	}

	public override void _Input(InputEvent @event)
	{
		if (!_isRebinding)
			return;

		// Ignore certain types of input.
		if (@event is InputEventMouseMotion or InputEventJoypadMotion)
			return;

		_isRebinding = false;
		Disabled = false;
		Text = _defaultTooltipText;
		EmitSignal(SignalName.InputEventAssigned, @event);
	}

	public override void _Pressed()
	{
		if (_isRebinding)
			return;

		_isRebinding = true;
		Disabled = true;
		Text = REBINDING_TOOLTIP_TEXT;
	}
}
