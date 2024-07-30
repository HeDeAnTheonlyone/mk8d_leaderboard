using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class Manager : Node
{
    public static Manager Singleton;
    public bool IsNewGame { get; set; } = true;
    public CourseManager CM { get; private set; }
    public DriverData[] DriverList { get; private set; }
    [Export] public int CurrentRound { get; private set; } = 1;



    public override void _Ready()
    {
        if (Singleton == null)
        {
            Singleton = this;
            ProcessMode = ProcessModeEnum.Always;
            SetProcess(false);
            SetPhysicsProcess(false);
        }
        else QueueFree();

        CM = new CourseManager();
    }



    public void SetDriverList(List<DriverData> driverList) => DriverList = driverList.ToArray();



    public bool NextRound()
    {
        if (CurrentRound > 4) return false;

        foreach (DriverData d in DriverList)
        {
            if (d.PointsHistory.Count < CurrentRound) return false;
        }

        CurrentRound++;
        UpdateDriverRanks();
        return true;
    }



    public void UpdateDriverRanks()
    {
        DriverList = DriverList.OrderByDescending(driver => driver.Points).ToArray();
        for (int i = 0; i < DriverList.Length; i++)
        {
            DriverList[i].SetNewPlace(i + 1);
        }
    }



    public static void SwitchScene(string nextScene) => Singleton.GetTree().ChangeSceneToFile($"res://Scenes/{nextScene}.tscn");



    public static void Save()
    {
        var driverData = new Dictionary[Singleton.DriverList.Length];
        for (int i = 0; i < driverData.Length; i++)
        {
            driverData[i] = Singleton.DriverList[i].ToDictionary();
        }

        var saveData = new Dictionary
        {
            { "currentRound", Singleton.CurrentRound },
            { "coursePicker", Singleton.CM.CoursePicker.ToDictionary() },
            { "driverCount", driverData.Length }
        };

        for (int i = 0; i < driverData.Length; i++)
        {
            saveData.Add($"driver_{i}", driverData[i]);
        }

        var dataString = Json.Stringify(saveData, "\t");
        using (var file = FileAccess.Open("user://current_state.json", FileAccess.ModeFlags.Write))
        {
            file.StoreString(dataString);
        }
    }



    public static void Load()
    {
        string loadString;

        using (var file = FileAccess.Open("user://current_state.json", FileAccess.ModeFlags.Read))
        {
            loadString = file.GetAsText();
        }

        var loadData = Json.ParseString(loadString).As<Dictionary>();

        Singleton.CurrentRound = (int)loadData["currentRound"];
        Singleton.CM.CreateCoursePickList(loadData["coursePicker"].As<Dictionary>());
        var driverCount = (int)loadData["driverCount"];
        var driverList = new DriverData[driverCount];
        for (int i = 0; i < driverCount; i++)
        {
            driverList[i] = new DriverData(loadData[$"driver_{i}"].As<Dictionary>());
        }

        Singleton.DriverList = driverList;
    }
}
