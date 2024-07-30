using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CourseManager
{
    private Random rng = new Random();
    public Cup[] CupList;
    public Course[] CourseList;
    public CoursePickList CoursePicker { get; private set; }



    public CourseManager()
    {
        CreateCupList();
        CreateCourseList();
    }



    private void CreateCupList()
    {
        var cupIndex = 0;

        CupList = new Cup[DirAccess.Open("res://Assets/Courses").GetDirectories().Length];
        var cupGroupDirs = DirAccess.Open("res://Assets/Cups").GetDirectories();

        for (int i = 0; i < cupGroupDirs.Length; i++)
        {
            var cupIconNames = DirAccess.Open($"res://Assets/Cups/{cupGroupDirs[i]}").GetFiles().Where(file => file.EndsWith(".import")).ToArray();
            for (int j = 0; j < cupIconNames.Length; j++)
            {
                CupList[cupIndex] = BuildCup(cupIconNames[j].TrimSuffix(".import"), cupGroupDirs[i]);
                cupIndex++;
            }
        }
    }



    private void CreateCourseList()
    {
        var courseList = new List<Course>();

        foreach (Cup cup in CupList)
        {
            courseList.AddRange(cup.Courses);
        }
        CourseList = courseList.ToArray();
    }



    private Cup BuildCup(string cupIconName, string cupGroup)
    {
        var cupName = cupIconName.Substring(0, cupIconName.Length - 4);
        var cupIcon = GD.Load<Texture2D>($"res://Assets/Cups/{cupGroup}/{cupIconName}");
        var cupCourses = new Course[4];
        var isDlcCup = cupGroup == "dlc" ? true : false;

        var coursePreviewNames = DirAccess.Open($"res://Assets/Courses/{cupName}").GetFiles().Where(file => file.EndsWith(".import")).ToArray();

        for (int i = 0; i < coursePreviewNames.Length; i++)
        {
            cupCourses[i] = BuildCourse(cupName, coursePreviewNames[i].TrimSuffix(".import"));
        }

        return new Cup(TranslationServer.Translate(cupName).ToString(), cupIcon, cupCourses, isDlcCup);
    }



    private Course BuildCourse(string cupName, string coursePreviewName)
    {
        string courseName, courseTag;
        var coursePreview = GD.Load<Texture2D>($"res://Assets/Courses/{cupName}/{coursePreviewName}");

        if (coursePreviewName.StartsWith('['))
        {
            var tagEnd = coursePreviewName.IndexOf(']');
            courseTag = coursePreviewName[1..tagEnd];
            courseName = coursePreviewName.TrimPrefix($"[{courseTag}]").TrimSuffix(".jpg");
        }
        else
        {
            courseTag = " ";
            courseName = coursePreviewName.TrimSuffix(".jpg");
        }

        return new Course(TranslationServer.Translate(courseName).ToString(), courseTag, coursePreview);
    }



    public void CreateCoursePickList(Dictionary data) => CoursePicker = new CoursePickList(data);



    public void CreateCoursePickList()
    {
        var shuffledList = new int[CourseList.Length];
        System.Array.Fill(shuffledList, -1);

        for (int i = 0; i < CourseList.Length; i++)
        {
            int index;

            do
            {
                index = rng.Next(0, shuffledList.Length);
            }
            while (shuffledList[index] != -1);

            shuffledList[index] = i;
        }

        CoursePicker = new CoursePickList(shuffledList);
    }
}
