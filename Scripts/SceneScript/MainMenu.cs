using Godot;

public partial class MainMenu : CanvasLayer
{
	#region Signal Callback
	private void StartNewGame()
	{
		Manager.Singleton.CM.CreateCoursePickList();
		Manager.SwitchScene("PlayerListCreator");
	}



	private void LoadGame()
	{
		Manager.Load();
		Manager.SwitchScene("Leaderboard");
	}
	#endregion
}
