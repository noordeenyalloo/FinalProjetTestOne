using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProjetTestOne.Models.User;
using FinalProjetTestOne.Models.User.UserDto;
using FinalProjetTestOne.Repository.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjetTestOne.Controllers
{
    public class AdminController : Controller
    {
        private IUserDeveloperRepository userDeveloperRepository;
        private IUserLeaderRepository userLeaderRepository;
        private IUserManagerRepository userManagerRepository;

        public AdminController(
            IUserDeveloperRepository userDeveloperRepository,
            IUserLeaderRepository userLeaderRepository,
            IUserManagerRepository userManagerRepository)
        {
            this.userDeveloperRepository = userDeveloperRepository;
            this.userLeaderRepository = userLeaderRepository;
            this.userManagerRepository = userManagerRepository;
        }
        public IActionResult Index()
        {
            ViewBag.devs = userDeveloperRepository.GetDevelopers();
            ViewBag.managers = userManagerRepository.GetProjectManagers();
            ViewBag.leaders = userLeaderRepository.GetTeamLeaders();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult CreateManager()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateManager(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                await userManagerRepository.AddProjectManager(createUserDto);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult CreateTeamleader()
        {
            return View();
        }
        public async Task<IActionResult> CreateTeamleader(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                await userLeaderRepository.AddTeamLeader(createUserDto);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [Authorize(Roles ="ADMIN")]
        [HttpGet]
        public IActionResult CreateDeveloper()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateDeveloper(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                await userDeveloperRepository.AddDeveloper(createUserDto);
                return RedirectToAction("Index");
            }
            else
            {
                return View("CreateDeveloper");
            }
        }
        //MANAGER
        //EDIT VIEW
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult EditManager(string Id)
        {
            ViewBag.manager = userManagerRepository.GetProjectManagerById(Id);
            return View();
        }
        //Edit POST
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult EditManager(UpdateUserDto updateUserDto)
        {
            if (ModelState.IsValid)
            {
               userManagerRepository.UpdateProjectManager(updateUserDto);
            return RedirectToAction("Index");
            }
            else
            {
                return View("EditManager");
            }
 
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult DeleteManager(string Id)
        {
            var manager = userManagerRepository.GetProjectManagerById(Id);
            return View(manager);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult DeleteManager(ProjectManager manager)
        {
            userManagerRepository.DeleteProjectManager(manager);
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult EditTeamLeader(string Id)
        {

            ViewBag.teamLeader = userLeaderRepository.GetTeamLeaderById(Id);
            return View();
        }
        //EDIT POST Update the chosen row
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult EditTeamLeader(UpdateUserDto updateUserDto)
        {
            if (ModelState.IsValid)
            {
                userLeaderRepository.UpdateTeamLeader(updateUserDto);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("EditTeamLeader");
            }

        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult DeleteLeader(string Id)
        {

            var teamLeader = userLeaderRepository.GetTeamLeaderById(Id);
            return View(teamLeader);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult DeleteLeader(TeamLeader leader)
        {
            userLeaderRepository.DeleteTeamLeader(leader);
            return View();
        }
        //EDIT VIEW
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult EditDeveloper(string Id)
        {
            ViewBag.developer = userDeveloperRepository.GetDeveloperById(Id);
            return View();
        }
        public IActionResult EditDeveloper(UpdateUserDto editDeveloperDto)
        {
            if (ModelState.IsValid)
            {
                userDeveloperRepository.UpdateDeveloper(editDeveloperDto);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("EditDeveloper");
            }
            
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult DeleteDeveloper(string Id)
        {
            var developer = userDeveloperRepository.GetDeveloperById(Id);
            return View(developer);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult DeleteDeveloper(Developer developer)
        {
            userDeveloperRepository.DeleteDeveloper(developer);
            return View();
        }
    }
}