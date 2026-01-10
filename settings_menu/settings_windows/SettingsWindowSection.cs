using Godot;

[Tool]
[GlobalClass]
public partial class SettingsWindowSection : VBoxContainer
{
	private RichTextLabel _headingLabel;
	
	public override void _Ready()
	{
		_headingLabel ??= CreateHeadingLabel();
	}

	private RichTextLabel CreateHeadingLabel()
	{
		RichTextLabel headingLabel = new()
		{
			BbcodeEnabled = true,
			Text = "[b]" + Name + "[/b]",
			FitContent = true,
		};
		AddChild(headingLabel, @internal: InternalMode.Front);
		headingLabel.Owner = this;
		return headingLabel;
	}
}
