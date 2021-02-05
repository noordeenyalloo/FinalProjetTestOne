using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using FinalProjetTestOne.Repository.MainUsers;
using FinalProjetTestOne.Repository.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace FinalProjetTestOne.Controllers
{
    public class TeamLeaderController : Controller
    {
        private ITeamLeaderRepository teamLeaderRepository;
        public TeamLeaderController(
            ITeamLeaderRepository teamLeaderRepository)
        {
            this.teamLeaderRepository = teamLeaderRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult ShowTasks(int Id)
        {
            ViewBag.Tasks = teamLeaderRepository.GetSprintTasksBySprintId(Id);
            return View("Tasks");
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult ShowSprints(int Id)
        {
            ViewBag.ProjectId = Id;
            ViewBag.sprints = teamLeaderRepository.GetSprints(Id);
            
            return View(ViewBag.ProjectId = Id);
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult ShowWorks(string devId,int taskId)
        {
            ViewBag.Works = teamLeaderRepository.GetWorks(devId,taskId);
            return View("Works");
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult ShowProject()
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.projects = teamLeaderRepository.GetProjects(UID);
            return View();
        }
        [HttpGet]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult CreateSprint(int Id)
        {
            ViewBag.UId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.ProjectId = Id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult CreateSprint(CreateSprintDto createSprintDto)
        {
            teamLeaderRepository.AddSprint(createSprintDto);
            return RedirectToAction("ShowSprints", new RouteValueDictionary(
                        new { controller = "TeamLeader", action = "ShowSprints", Id = createSprintDto.ProjectId }));
        }
        [HttpGet]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult EditSprint(int Id)
        {
            ViewBag.UId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Sprint = teamLeaderRepository.GetSprintById(Id);
            return View();
        }

        [HttpPost]

        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult EditSprint(UpdateSprintDto updateSprintDto)
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var project = teamLeaderRepository.GetProjects(UID).Where(x => x.Id == updateSprintDto.ProjectId).SingleOrDefault();
            teamLeaderRepository.UpdateSprint(updateSprintDto);
            //return RedirectToAction("ShowSprints");
            return RedirectToAction("ShowSprints", new RouteValueDictionary(
                        new { controller = "TeamLeader", action = "ShowSprints", Id = updateSprintDto.ProjectId }));
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult DeleteSprint(int Id)
        {
            ViewBag.UId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Sprint = teamLeaderRepository.GetSprintById(Id);
            ViewBag.ProjectId = Id;
            return View();
        }
        [HttpGet]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult CreateTask(int Id)
        {
            var sprint = teamLeaderRepository.GetSprintById(Id);
            ViewBag.Developers = teamLeaderRepository.GetDevelopers(sprint.ProjectId);
            ViewBag.SprintId = Id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult CreateTask(CreateTaskDto createTaskDto)
        {
            teamLeaderRepository.CreateSprintTask(createTaskDto);
            return RedirectToAction("ShowSprints", new RouteValueDictionary(
                        new { controller = "TeamLeader", action = "ShowTasks", Id = createTaskDto.SprintId }));
        }
        [HttpGet]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult EditTask(int Id)
        {
            //var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var task = teamLeaderRepository.Getsprintbytaskid(Id);
            ViewBag.Developers = teamLeaderRepository.GetDevelopers(task.Sprint.Project.Id);
            //var xx = teamLeaderRepository.GetDevelopers(sprint.ProjectId);
            ViewBag.Task = teamLeaderRepository.GetSprintTaskById(Id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult EditTask(UpdateTaskDto updateTaskDto)
        {
            teamLeaderRepository.UpdateSprintTask(updateTaskDto);
            return RedirectToAction("ShowSprints", new RouteValueDictionary(
            new { controller = "TeamLeader", action = "ShowTasks", Id = updateTaskDto.SprintId }));
        }
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult DeleteTask(int Id)
        {
            ViewBag.UId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Sprint = teamLeaderRepository.GetSprintById(Id);
            ViewBag.ProjectId = Id;
            return View();
        }
        [Authorize(Roles ="TEAMLEADER")]
        public void ToggleSprintStatus(int Id)
        {
            teamLeaderRepository.ToggleSprintStatus(Id);
        }
        [Authorize(Roles ="TEAMLEADER")]
        public void ToggleTaskStatus(int Id)
        {
            teamLeaderRepository.ToggleTaskStatus(Id);
        }
        [Authorize(Roles ="TEAMLEADER")]
        public void ToggleWorkStatus(int Id)
        {
            teamLeaderRepository.ToggleWorkStatus(Id);
        }
        [HttpGet]
        [Authorize(Roles ="TEAMLEADER")]
        public IActionResult WrokPage(int Id)
        {
            ViewBag.Work = teamLeaderRepository.getWorkById(Id);
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="TEAMLEADER")]
        public void WrokPage(RejectDto rejectDto)
        {
            teamLeaderRepository.Reject(rejectDto);
        }
        [Authorize(Roles ="TEAMLEADER")]
        public void ToggleAllStatus(int Id)
        {
            teamLeaderRepository.checkStatus(Id);
        }
        public FileResult GetFiles(int Id)
        {
            var files = teamLeaderRepository.GetFilesByWorkId(Id);
            using(MemoryStream ms = new MemoryStream())
            {
                using(var archive = new ZipArchive(ms,ZipArchiveMode.Create, true))
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

//Stream stream = new MemoryStream(teacher.File);//from bytes to stream
//return new FileStreamResult(stream, teacher.FileExtension);