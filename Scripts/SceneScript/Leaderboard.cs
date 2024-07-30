using Godot;

public partial class Leaderboard : CanvasLayer
{
	private PackedScene leaderboardDriverEntry = GD.Load<PackedScene>("res://Objects/LeaderboardDriverEntry.tscn");
	private PackedScene leaderboardSeparator = GD.Load<PackedScene>("res://Objects/LeaderboardSeparator.tscn");
	private VBoxContainer leaderboardList;
	private VBoxContainer courseImageList;
	private Label nextRoundButton; 



	public override void _Ready()
	{
		leaderboardList = GetNode<VBoxContainer>("CourseSplitter/ButtonSplitter/LeaderboardMargin/Scroll/LeaderboardList");
		courseImageList = GetNode<VBoxContainer>("CourseSplitter/CourseImageList");
		nextRoundButton = GetNode<Label>("CourseSplitter/ButtonSplitter/Buttonsmargin/HBoxContainer/MenuButton/TextMargine/Label");
		PopulateLeaderboard(Manager.Singleton.DriverList);
		UpdateCourseImages(Manager.Singleton.CM.CoursePicker.GetNext4Courses());
		Manager.Save();

		if (Manager.Singleton.CurrentRound >= 4) nextRoundButton.Text = TranslationServer.Translate("show_winner");
	}



	private void PopulateLeaderboard(DriverData[] driverList)
	{
		foreach (DriverData driver in driverList)
		{
			if (leaderboardList.GetChildCount() % 13 == 0)
			{
				var separatorInstance = leaderboardSeparator.Instantiate<LeaderboardSeparator>();
				leaderboardList.AddChild(separatorInstance);
			}

			var entryInstance = leaderboardDriverEntry.Instantiate<LeaderboardDriverEntry>();
			entryInstance.SetDriver(driver);
			leaderboardList.AddChild(entryInstance);
		}
	}



	private void UpdateLeaderboard()
	{
		Manager.Singleton.UpdateDriverRanks();

		foreach (GodotObject entry in leaderboardList.GetChildren())
		{
			if (entry.GetType() == typeof(LeaderboardDriverEntry))
			{
				(entry as LeaderboardDriverEntry).Free();
			}
			else if (entry.GetType() == typeof(LeaderboardSeparator))
			{
				(entry as LeaderboardSeparator).Free();
			}
		}

		PopulateLeaderboard(Manager.Singleton.DriverList);
	}



	private void UpdateCourseImages(Course[] courses)
	{
		for (int i = 0; i < courses.Length; i++)
		{
			courseImageList.GetNode<CourseImage>($"CourseImage{i}").SetCourse(courses[i]);
		}
	}



	#region Signal Callback
	private void NextRound()
	{
		if (!Manager.Singleton.NextRound()) return;
		if (Manager.Singleton.CurrentRound >= 4) nextRoundButton.Text = TranslationServer.Translate("show_winner");
		UpdateLeaderboard();
		UpdateCourseImages(Manager.Singleton.CM.CoursePicker.GetNext4Courses());
		Manager.Save();
	}
	#endregion
}
