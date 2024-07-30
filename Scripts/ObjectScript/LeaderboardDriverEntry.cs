using System.Linq;
using Godot;

public partial class LeaderboardDriverEntry : PanelContainer
{
	public DriverData Driver { get; private set; }
	private Label placeDisplay;
	private Label nameDisplay;
	private HBoxContainer pointsDisplayList;
	private Label pointsDisplay1;
	private Label pointsDisplay2;
	private Label pointsDisplay3;
	private Label pointsDisplay4;
	private Label pointsDisplaySum;
	private LineEdit pointsInput;
	private int pointsBuffer;



	public override void _Ready()
	{
		placeDisplay = GetNode<Label>("StatsMargin/HBoxContainer/Place");
		nameDisplay = GetNode<Label>("StatsMargin/HBoxContainer/Name");
		var pointsPanel = GetNode("StatsMargin/HBoxContainer/PointsPanel");
		pointsDisplayList = pointsPanel.GetNode<HBoxContainer>("PointsDisplayList");
		pointsDisplay1 = pointsPanel.GetNode<Label>("PointsDisplayList/Points1");
		pointsDisplay2 = pointsPanel.GetNode<Label>("PointsDisplayList/Points2");
		pointsDisplay3 = pointsPanel.GetNode<Label>("PointsDisplayList/Points3");
		pointsDisplay4 = pointsPanel.GetNode<Label>("PointsDisplayList/Points4");
		pointsDisplaySum = pointsPanel.GetNode<Label>("PointsDisplayList/PointsSum");
		pointsInput = pointsPanel.GetNode<LineEdit>("PointsInput");

		placeDisplay.Text = Driver.Place.ToString();
		nameDisplay.Text = Driver.Name;
		UpdatePointsDisplay();
	}



	private void UpdatePointsDisplay()
	{
		var pointHistory = Driver.PointsHistory.Reverse().ToArray();

		for (int i = 0; i < 4; i++)
		{
			string str = pointHistory.Length > i ? $"{pointHistory[i]}" : "-";

			switch (i)
			{
				case 0:
				{
					pointsDisplay1.Text = str;
					break;
				}

				case 1:
				{
					pointsDisplay2.Text = str;
					break;
				}

				case 2:
				{
					pointsDisplay3.Text = str;
					break;
				}

				case 3:
				{
					pointsDisplay4.Text = str;
					break;
				}
			}
		}

		pointsDisplaySum.Text = Driver.Points.ToString();
	}



    public void SetDriver(DriverData data) => Driver = data;



	#region Signal Callback
	private void SelectInput() => pointsDisplayList.Visible = false;



	private void DeselectInput()
	{
		pointsInput.Text = "";
		pointsDisplayList.Visible = true;
		UpdatePointsDisplay();
		Manager.Save();
	}



	private void CheckInput(string input)
	{
		if (int.TryParse(input, out int parsedInput))
		{
			Driver.AddPoints(parsedInput);
		}
		else
		{
			if (pointsInput.Text.Length == 0) return;
			var str = Driver.Points.ToString();
			pointsInput.Text = str;
			pointsInput.CaretColumn = str.Length;
		}
	}



	private void SubmitInput(string input) => pointsInput.ReleaseFocus();
	#endregion
}
