using Godot;
using Godot.Collections;
using System;

namespace Smars.Addons.Settings;

[Tool]
[GlobalClass]
public partial class SettingsWindow : GridContainer
{
	[Export] public Array<SettingsSection> SettingsSections { get; private set; } = [];
	[ExportToolButton("Update Window")] private Callable UpdateWindowButton => Callable.From(UpdateWindow);

	private SettingsMenu _settingsMenu;

	public override void _Ready()
	{
		Columns = 3;

		// TODO: Replace with inspector warning
		Node parentNode = GetParent();
		while (parentNode is not SettingsMenu)
		{
			parentNode = parentNode.GetParent();
			if (parentNode is null)
				throw new NullReferenceException("SettingsWindow not a child of a SettingsMenu.");
		}
		_settingsMenu = parentNode as SettingsMenu;

		CallDeferred(MethodName.UpdateWindow);
	}

	private void UpdateWindow()
	{
		foreach (Node child in GetChildren())
			child.QueueFree();

		foreach (SettingsSection section in SettingsSections)
		{
			CreateSectionHeadingLabel(section.SectionName, this);
			CreateSpacer(this);
			CreateSpacer(this);
			foreach (Setting setting in section.Settings)
			{
				if (setting is null)
					continue;

				CreateSettingNameLabel(setting, this);
				CreateSettingSeparator(this);
				CreateSettingDisplay(setting, this);
			}
			CreateSpacer(this, _settingsMenu.SectionVerticalSeparation);
			CreateSpacer(this, _settingsMenu.SectionVerticalSeparation);
			CreateSpacer(this, _settingsMenu.SectionVerticalSeparation);
		}
	}

	private static RichTextLabel CreateSectionHeadingLabel(string sectionName, Node parent)
	{
		RichTextLabel headingLabel = new()
		{
			BbcodeEnabled = true,
			Text = "[b]" + sectionName + "[/b]",
			FitContent = true,
		};

		parent.AddChild(headingLabel);
		headingLabel.Owner = parent;
		return headingLabel;
	}

	private static Control CreateSpacer(Node parent, float? customMinimumHeight = null)
	{
		Control spacer = new();
		if (customMinimumHeight is not null)
			spacer.CustomMinimumSize = new Vector2(0, customMinimumHeight.Value);
		parent.AddChild(spacer);
		spacer.Owner = parent;
		return spacer;
	}

	private static Label CreateSettingNameLabel(Setting setting, Node parent)
	{
		Label nameLabel = new()
		{
			Text = setting.Name,
			VerticalAlignment = VerticalAlignment.Center,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
		};
		parent.AddChild(nameLabel);
		nameLabel.Owner = parent;
		return nameLabel;
	}

	private static VSeparator CreateSettingSeparator(Node parent)
	{
		VSeparator separator = new()
		{
			SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
		};
		parent.AddChild(separator);
		separator.Owner = parent;
		return separator;
	}

	private static Control CreateSettingDisplay(Setting setting, Node parent)
	{
		Control settingDisplay = SettingsMenu.Instance.GetSettingDisplayScene(setting).Instantiate<Control>();
		settingDisplay.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		parent.AddChild(settingDisplay);
		settingDisplay.Owner = parent;

		((ISettingDisplay)settingDisplay).SetSetting(setting);
		return settingDisplay;
	}
}
