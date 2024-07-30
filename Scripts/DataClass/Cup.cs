using Godot;


public partial class Cup
{
    public string Name { get; private  set; }
    public Texture2D Icon { get; private set; }
    public Course[] Courses { get; private set; }
    public bool IsDlcCup { get;  private set; }



    public Cup (string name, Texture2D icon, Course[] courses, bool isDlcCup)
    {
        Name = name;
        Icon = icon;
        Courses = courses;
        IsDlcCup = isDlcCup;
    }



    public override string ToString()
    {
        return $"Name: {Name},\nIcon: {Icon},\nCourses: [\n\t{Courses[0]},\n\t{Courses[1]},\n\t{Courses[2]},\n\t{Courses[3]}\n],\nIsDlcCup: {IsDlcCup}";
    }
}

