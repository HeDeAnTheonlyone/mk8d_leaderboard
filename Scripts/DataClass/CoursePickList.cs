using Godot.Collections;

public partial class CoursePickList
{
    public int[] CourseIndexList { get; private set; }
    public int Pointer { get; private set;} = 0;



    public CoursePickList(int[] originalCourseIndexList) => CourseIndexList = originalCourseIndexList;
    
    
    
    public CoursePickList(Dictionary data)
    {
        CourseIndexList = (int[])data["indexes"];
        Pointer = (int)data["pointer"];
    }



    public Dictionary ToDictionary()
    {
        return new Dictionary
        {
            { "indexes", CourseIndexList },
            { "pointer", Pointer}
        };
    }



    /// <summary>
    /// Returns 4 courses without moving the pointer.
    /// </summary>
    /// <returns></returns>
    public Course[] Get4Courses()
    {
        var courseList = GetNext4Courses();
        Pointer -= courseList.Length;
        return courseList;
    }



    /// <summary>
    /// Moves the pointer and then returns 4 courses.
    /// </summary>
    /// <returns></returns>
    public Course[] GetNext4Courses()
    {
        var courseList = new Course[4];
        
        for (int i = 0; i < courseList.Length; i++)
        {
            courseList[i] = Manager.Singleton.CM.CourseList[CourseIndexList[Pointer]];
            Pointer++;
        }

        return courseList;
    }
}