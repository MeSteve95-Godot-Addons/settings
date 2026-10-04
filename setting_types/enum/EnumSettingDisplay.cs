using Godot;
using System;

namespace Smars.Addons.Settings;

[Tool]
public partial class EnumSettingDisplay : OptionButton, ISettingDisplay
{
	private EnumSetting _enumSetting;
	private EnumSetting EnumSetting
	{
		get => _enumSetting;
		set
		{
			_enumSetting = value;
			UpdateOptions();
		}
	}

	public override void _Ready()
	{
		ItemSelected += OnItemSelected;
	}

	public void SetSetting(Setting setting) => EnumSetting = (EnumSetting)setting;

	private void UpdateOptions()
	{
		Clear();
		if (EnumSetting is null)
			return;

		foreach ((int id, string label) in EnumSetting.IdToLabel)
		{
			if (EnumSetting.IgnoredIds.Contains(id))
				continue;

			AddItem(label, id);
		}

		Selected = EnumSetting.SelectedId == -1 ? EnumSetting.DefaultSelectedId : EnumSetting.SelectedId;
	}

	private void OnItemSelected(long index)
	{
		if (EnumSetting is null)
			return;

		EnumSetting.SelectedId = (int)index;
	}
}
