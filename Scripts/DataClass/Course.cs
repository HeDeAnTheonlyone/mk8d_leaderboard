using Godot;

public partial class Course
{
    public string Name { get; private  set; }
    public string Tag { get; private set; }
    public Texture2D Preview { get; private set; }



    public Course(string name, string tag, Texture2D preview)
    {
        Name = name;
        Tag = tag;
        Preview = preview;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Tag: {Tag}, Preview: {Preview}";
    }
}