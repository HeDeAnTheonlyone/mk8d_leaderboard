using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class DriverData
{
    public string Name { get; private set; }
    public int Points { get; private set; } = 0;
    public int Place { get; private set; } = 1;
    public Stack<int> PointsHistory { get; private set; } = new Stack<int>();



    public DriverData(string name)
    {
        Name = name;
    }



    public DriverData(Dictionary data)
    {
        Name = (string)data["name"];
        Points = (int)data["points"];
        Place = (int)data["place"];
        foreach (int points in (int[])data["pointsHistory"])
        {
            PointsHistory.Push(points);
        }
    }



    public void SetNewPlace(int newPlace) => Place = newPlace;



    /// <summary>
    /// Adds points for the current round or modifies them if an entry already exists
    /// </summary>
    /// <param name="newPoints"></param>
    public void AddPoints(int newPoints)
    {
        var round = Manager.Singleton.CurrentRound;
        
        if (PointsHistory.Count < round)
        {
            PointsHistory.Push(newPoints);
        }
        else
        {
            PointsHistory.Pop();
            PointsHistory.Push(newPoints);
        }

        Points = PointsHistory.Count >= 1 ? PointsHistory.Sum() : 0;
    }



    public Dictionary ToDictionary()
    {
        return new Dictionary
        {
            { "name", Name },
            { "points", Points },
            { "place", Place },
            { "pointsHistory", PointsHistory.ToArray() }
        };
    }
}