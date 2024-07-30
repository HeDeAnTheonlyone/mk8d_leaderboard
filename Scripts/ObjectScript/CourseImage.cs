using System.Linq;
using Godot;

public partial class CourseImage : MarginContainer
{
	private TextureRect preview;
	private TextureRect icon;
	private Label label;



	public override void _Ready()
	{
		preview = GetNode<TextureRect>("CoursePreview");
		icon = GetNode<TextureRect>("ItemList/CupIcon");
		label =  GetNode<Label>("CoursePreview/Label");
	}



	public void SetCourse(Course course)
	{
		preview.Texture = course.Preview;
		label.Text = $"{course.Tag}\n{course.Name}";
		foreach (Cup cup in Manager.Singleton.CM.CupList)
		{
			if (cup.Courses.Contains(course))
			{
				icon.Texture = cup.Icon;
			}
		}
	}
}
