using Godot;

public partial class DriverListEntry : PanelContainer
{
	private DriverData driver;
	private Label label;



	public override void _Ready()
	{
		label = GetNode<Label>("HBoxContainer/Label");
		label.Text = driver.Name;
	}



	public void SetDriver(DriverData data) => driver = data;



	#region Signal Callback
	private void RemoveSelfFromList()
	{
		GetTree().Root.GetNode<PlayerListCreator>("PlayerListCreator").DriverList.Remove(driver);
		QueueFree();
	}
	#endregion
}
