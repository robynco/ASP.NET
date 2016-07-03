﻿using Domain.Entities;
using Domain.Interfaces;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMenagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectController : Controller
    {
        private IProjectRepository _projectRepository = new ProjectRepository();
        private ICustomerRepository _customerRepository = new CustomerRepository();
        private ITaskRepository _taskRepository = new TaskRepository();
        // GET: Project

        public ActionResult Index()
        {
            ViewBag.TasksCount = _taskRepository.GetAll();
            return View(_projectRepository.GetAll());
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_customerRepository.GetAll().Where(x=>x.IsActive == true), "ID", "Email");

            return View();
        }

        [HttpPost]
        public JsonResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                if (_projectRepository.Create(project))
                    return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            Project project = _projectRepository.GetById(id);
             ViewBag.CustomerId = new SelectList(_customerRepository.GetAll().Where(x=>x.IsActive == true), "ID", "Email");
            return View(project);
        }

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                if (_projectRepository.Update(project))
                    return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if(_projectRepository.Delete(id))
                return RedirectToAction("Index");

            ViewBag.ErrorProject = "Error while deletin project";
            return RedirectToAction("Index");
        }
    }
}