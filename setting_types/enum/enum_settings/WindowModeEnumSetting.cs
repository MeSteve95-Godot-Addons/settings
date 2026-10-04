using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace smars.addons.settings;

public partial class WindowModeEnumSetting : EnumSetting
{
	public override Dictionary<int, string> IdToLabel { get; protected set; } = new(
		Enum.GetValues(typeof(DisplayServer.WindowMode))
			.Cast<DisplayServer.WindowMode>()
			.ToDictionary(e => (int)e, e => e.ToString())
	);
	public override Array<int> IgnoredIds { get; protected set; } = [(int)DisplayServer.WindowMode.Minimized];

	public override int DefaultSelectedId { get; protected set; } = (int)DisplayServer.WindowMode.ExclusiveFullscreen;

	protected override void OnValueChanged(int newId)
	{
		DisplayServer.WindowSetMode((DisplayServer.WindowMode)newId);
	}
}
