using Godot;

namespace smars.addons.settings;

[Tool]
public partial class KeyBindSettingDisplay : Control, ISettingDisplay
{
	[Export] private PackedScene _boundKeysPanelScene;
	[ExportGroup("Internal Scene References")]
	[Export] private Control _boundKeysPanelParent;
	[Export] private KeybindRebindButton _bindNewInputButton;

	private KeyBindSetting _keyBindSetting;
	private KeyBindSetting KeyBindSetting
	{
		get => _keyBindSetting;
		set
		{
			_keyBindSetting = value;
			FreeBoundKeyPanels();

			if (_keyBindSetting is not null)
				CreateBoundKeysPanel();
		}
	}
	public void SetSetting(Setting setting) => KeyBindSetting = (KeyBindSetting)setting;

	public override void _Ready()
	{
		_bindNewInputButton.InputEventAssigned += OnAddInputEvent;
	}

	private void RegenerateBoundKeysPanels()
	{
		FreeBoundKeyPanels();
		CreateBoundKeysPanel();
	}

	private void FreeBoundKeyPanels()
	{
		if (_boundKeysPanelParent is null)
			return;

		foreach (Node child in _boundKeysPanelParent.GetChildren())
		{
			if (child is not KeybindBoundKeysPanel boundKeysPanel)
				continue;

			boundKeysPanel.DeleteInputEvent -= OnDeleteInputEvent;
			boundKeysPanel.QueueFree();
		}
	}

	private void CreateBoundKeysPanel()
	{
		foreach (InputEvent inputEvent in KeyBindSetting.InputEvents)
		{
			KeybindBoundKeysPanel boundKeysPanel = _boundKeysPanelScene.Instantiate<KeybindBoundKeysPanel>();
			boundKeysPanel.InputEvent = inputEvent;

			_boundKeysPanelParent.AddChild(boundKeysPanel);
			boundKeysPanel.Owner = _boundKeysPanelParent;
			boundKeysPanel.DeleteInputEvent += OnDeleteInputEvent;
			boundKeysPanel.RebindInputEvent += OnRebindInputEvent;
		}
	}

	private void OnAddInputEvent(InputEvent inputEvent)
	{
		KeyBindSetting.AddInputEvent(inputEvent);
		RegenerateBoundKeysPanels();
	}

	private void OnRebindInputEvent(InputEvent oldInputEvent, InputEvent newInputEvent)
	{
		KeyBindSetting.RebindInputEvent(oldInputEvent, newInputEvent);
		RegenerateBoundKeysPanels();
	}

	private void OnDeleteInputEvent(InputEvent inputEvent)
	{
		KeyBindSetting.RemoveInputEvent(inputEvent);
		RegenerateBoundKeysPanels();
	}
}
