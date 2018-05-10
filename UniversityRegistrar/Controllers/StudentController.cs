using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
  public class StudentController : Controller
  {
    [HttpGet("/students")]
    public ActionResult Index()
    {
      List<Student> allStudents = Student.GetAll();
      return View(allStudents);
    }

    [HttpGet("/students/new")]
    public ActionResult CreateForm()
    {
        return View();
    }
    [HttpPost("/students")]
    public ActionResult Create()
    {
        Student newStudent = new Student(Request.Form["student-name"]);
        newStudent.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/students/{id}")]
    public ActionResult Details(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(id);
        List<Course> studentCourses = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();
        model.Add("selectedStudent", selectedStudent);
        model.Add("studentCourses", studentCourses);
        model.Add("allCourses", allCourses);
        return View(model);

    }

    [HttpPost("/students/{studentId}/courses/new")]
    public ActionResult AddCourse(int studentId)
    {
        Student student = Student.Find(studentId);
        Course course = Course.Find(int.Parse(Request.Form["course-id"]));
        student.AddCourse(course);
        return RedirectToAction("Details",  new { id = studentId });
    }
  }
}
