using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
  public class CourseController : Controller
  {
    [HttpGet("/courses")]
    public ActionResult Index()
    {
      List<Course> allCourses = Course.GetAll();
      return View(allCourses);
    }

    [HttpGet("/courses/new")]
    public ActionResult CreateForm()
    {
        return View();
    }
    [HttpPost("/courses")]
    public ActionResult Create()
    {
        Course newCourse = new Course(Request.Form["course-name"]);
        newCourse.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/courses/{id}")]
    public ActionResult Details(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(id);
        List<Student> courseStudents = selectedCourse.GetStudents();
        List<Student> allStudents = Student.GetAll();
        model.Add("selectedCourse", selectedCourse);
        model.Add("courseStudents", courseStudents);
        model.Add("allStudents", allStudents);
        return View(model);
    }

    [HttpGet("/courses/delete")]
    public ActionResult DeleteCourse()
    {
        List<Course> allCourses = Course.GetAll();
        return View(allCourses);
    }

    [HttpPost("/courses/delete")]
    public ActionResult DeletePost()
    {
        int id= int.Parse(Request.Form["course-delete-dropdown"]);
        Course selectedCourse=Course.Find(id);
        selectedCourse.Delete();
        return RedirectToAction("Index");
    }

    [HttpGet("/courses/update")]
    public ActionResult UpdateCourse()
    {
        List<Course> allCourses = Course.GetAll();
        return View(allCourses);
    }

    [HttpPost("/courses/update")]
    public ActionResult UpdatePost()
    {
        int id= int.Parse(Request.Form["course-update-dropdown"]);
        Course selectedCourse=Course.Find(id);

        string newName=Request.Form["new-course-name"];

        selectedCourse.Update(newName);
        return RedirectToAction("Index");
    }



    [HttpPost("/courses/{courseId}/students/new")]
    public ActionResult AddStudent(int courseId)
    {
        Course course = Course.Find(courseId);
        Student student = Student.Find(int.Parse(Request.Form["student-id"]));
        course.AddStudent(student);
        return RedirectToAction("Details",  new { id = courseId });
    }
  }
}
