using Godot;
using System.Collections.Generic;

public partial class PlayerListCreator : CanvasLayer
{
	public List<DriverData> DriverList { get; private set; } = new List<DriverData>();
	private LineEdit playerNameInput;
	private VBoxContainer driverListDisplay;
	private PackedScene driverListEntry = GD.Load<PackedScene>("res://Objects/DirverListEntry.tscn");



    public override void _Ready()
    {
        playerNameInput = GetNode<LineEdit>("Margin/Splitter/VBoxContainer/PlayerAdder/HBoxContainer/Panel/Inpt");
		driverListDisplay = GetNode<VBoxContainer>("Margin/Splitter/Scroll/PlayerList");
    }



    #region Signal Callbacks
    private void AddDriver(string name) => AddDriver();
    private void AddDriver()
	{
		if (playerNameInput.Text.Length < 1) return;
		
		var data = new DriverData(playerNameInput.Text);
		playerNameInput.Text = "";

		DriverList.Add(data);

		var driverEntry = driverListEntry.Instantiate<DriverListEntry>();
		driverEntry.SetDriver(data);
		driverListDisplay.AddChild(driverEntry);
	}



	private void StartWithCurrentDriverList()
	{
		Manager.Singleton.SetDriverList(DriverList);
		Manager.SwitchScene("Leaderboard");
	}
	#endregion
}
