using Godot;

namespace Towerdefense.source.debug;

public partial class DataRoot : Control
{
	[Export] private Label _label;
	
	public void SetDataLabelText(string text)
	{
		_label.Text = text;
	}
}