using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProjetTestOne.Helpers;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Models.Dto;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using FinalProjetTestOne.Repository;
using FinalProjetTestOne.Repository.MainUsers;
using FinalProjetTestOne.Repository.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjetTestOne.Controllers
{
    public class ProjectManagerController : Controller
    {
        private IProjectManagerRepository projectManagerRepository;
        private IUserLeaderRepository userLeaderRepository;
        private IUserManagerRepository userManagerRepository;
        private IUserDeveloperRepository userDeveloperRepository;
        MailHandler mail = new MailHandler();
        public ProjectManagerController(
            IProjectManagerRepository projectManagerRepository,
            IUserLeaderRepository userLeaderRepository,
            IUserManagerRepository userManagerRepository,
            IUserDeveloperRepository userDeveloperRepository)
        {
            this.projectManagerRepository = projectManagerRepository;
            this.userLeaderRepository = userLeaderRepository;
            this.userManagerRepository = userManagerRepository;
            this.userDeveloperRepository = userDeveloperRepository;
        }

        [Authorize(Roles ="MANAGER")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        public IActionResult ShowProjects()
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.P = projectManagerRepository.GetProjects(UID);
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        public IActionResult ShowSprints(int Id)
        {
            ViewBag.sprints = projectManagerRepository.GetSprints(Id);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public IActionResult ShowWorks(int Id, string devId)
        {
            ViewBag.works = projectManagerRepository.GetWorks(Id, devId);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        public IActionResult WorkPage(int Id)
        {
            ViewBag.Work = projectManagerRepository.getWorkById(Id);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        public IActionResult ShowTasks(int Id)
        {
            ViewBag.Tasks = projectManagerRepository.GetSprintTasks(Id);
            return View();
        }

        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.TeamLeaders = userLeaderRepository.GetTeamLeaders();
            ViewBag.Developers = userDeveloperRepository.GetDevelopers();
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public IActionResult Create(CreateProjectDto CreateProjectDto)
        {
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            projectManagerRepository.AddProject(CreateProjectDto);
            return RedirectToAction("ShowProjects", "ProjectManager", UID);

        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {

            ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.TeamLeaders = userLeaderRepository.GetTeamLeaders();
            ViewBag.Developers = userDeveloperRepository.GetDevelopers();
            ViewBag.Project = projectManagerRepository.GetProjectById(Id);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public IActionResult Edit(UpdateProjectDto updateProjectDto)
        {
            var id = updateProjectDto.Id;
            if (ModelState.IsValid)
            {
                projectManagerRepository.UpdateProject(updateProjectDto);
                return Redirect("ShowProjects");
            }
            else
            {
                ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.TeamLeaders = userLeaderRepository.GetTeamLeaders();
                ViewBag.Developers = userDeveloperRepository.GetDevelopers();
                ViewBag.Project = projectManagerRepository.GetProjectById(id);
                return View();
            }
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            ViewBag.Project = projectManagerRepository.GetProjectById(Id);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public IActionResult Delete(Project project)
        {
            projectManagerRepository.DeleteProject(project);
            return View();
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public void ToggleStatus(int Id)
        {
            projectManagerRepository.ToggleStatus(Id);
        }
        public FileResult GetFiles(int Id)
        {
            var files = projectManagerRepository.GetFilesByWorkId(Id);
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
