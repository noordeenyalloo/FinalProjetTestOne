using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using FinalProjetTestOne.Repository.MainUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FinalProjetTestOne.Controllers.User
{
    public class DeveloperController : Controller
    {
        private IDeveloperRepository developerRepository;
        public DeveloperController(IDeveloperRepository developerRepository)
        {
            this.developerRepository = developerRepository;
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult ShowProjects()
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.projects = developerRepository.GetProjects(UID);
            return View("Projects");
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult ShowSprints(int Id)
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.tasks = developerRepository.GetTasksBySprintId(Id);
            ViewBag.Sprints = developerRepository.GetSprints(Id);
            return View("Sprints");
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult ShowTasks(int id)
        {
            ViewBag.UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.taskes = developerRepository.GetTasksBySprintId(id);
            return View("Tasks");
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult ShowWorks(int Id)
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.works = developerRepository.GetWorks(UID,Id);
            return View("Works");
        }
        [Authorize(Roles ="DEVELOPER")]
        [HttpGet]
        public IActionResult CreateWork(int Id)
        {
            ViewBag.SprintTaskId = Id;
            ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }
        [Authorize(Roles ="DEVELOPER")]
        [HttpPost]
        public IActionResult CreateWork(CreateWorkDto createWorkDto)
        {
            if (ModelState.IsValid)
            {
            developerRepository.AddWork(createWorkDto);
            return RedirectToAction("ShowProjects");
            }
            else
            {
                ViewBag.SprintTaskId = createWorkDto.SprintTaskId;
                ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return View("CreateWork");
            }

        }
        [Authorize(Roles ="DEVELOPER")]
        [HttpGet]
        public IActionResult RemoveWork(int id)
        {
            ViewBag.Work = developerRepository.GetWorkById(id);
            return View();
        }
        [Authorize(Roles ="DEVELOPER")]
        [HttpPost]
        public IActionResult RemoveWork(Work RemoveWork)
        {
            developerRepository.DeleteWork(RemoveWork);
            return Redirect("ShowWorks");
        }
        [Authorize(Roles ="DEVELOPER")]
        public void ToggleAllStatus(string Id)
        {
            developerRepository.checkStatus(Id);
        }
        [Authorize(Roles ="DEVELOPER")]
        public IActionResult WrokPage(int Id)
        {
            ViewBag.Work = developerRepository.GetWorkById(Id);
            return View();
        }
        [HttpGet]
        public IActionResult EditWork(int id)
        {
            ViewBag.Work = developerRepository.GetWorkById(id);
            return View();
        }
        [HttpPost]
        public IActionResult EditWork(UpdateWorkDto UpdateWorkDto)
        {
            developerRepository.UpdateWork(UpdateWorkDto);
            return RedirectToAction("ShowProjects");
        }
        [Authorize(Roles ="DEVELOPER")]
        public FileResult GetFiles(int Id)
        {
            var files = developerRepository.GetFilesByWorkId(Id);
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var entry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipStream = entry.Open())
                        {
                            zipStream.Write(file.FileBytes, 0, file.FileBytes.Length);
                        }
                    }
                }
                return File(ms.ToArray(), "application/zip", "Archive.zip");
            }
        }
    }
}



