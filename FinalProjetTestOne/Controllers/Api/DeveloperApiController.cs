using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProjetTestOne.Models;
using FinalProjetTestOne.Repository.MainUsers;
using System.Security.Claims;
using FinalProjetTestOne.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace FinalProjetTestOne.Controllers.Api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DeveloperAPIController : ControllerBase
    {
        private IDeveloperRepository developerRepository;
        private UserManager<IdentityUser> UserManager;
        public DeveloperAPIController(IDeveloperRepository developerRepository, UserManager<IdentityUser> UserManager)
        {
            this.developerRepository = developerRepository;
            this.UserManager = UserManager;
        }
        [HttpGet]
        public IActionResult GetWorks(int id)
        {
            var user = UserManager.GetUserId(User);
            var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return new JsonResult(developerRepository.GetWorks(user, id).ToList());
        }
        [HttpGet]
        public IActionResult WrokPage(int Id)
        {
            var work = developerRepository.GetWorkById(Id);
            return new JsonResult(work);
        }
        [HttpPost]
        public void Create([FromBody] CreateWorkApiDto create)
        {
            developerRepository.AddWorkAPI(create);
        }
        [HttpPut]
        public void Edit([FromBody] UpdateWorkDto work)
        {
            developerRepository.UpdateWork(work);
        }
        [HttpDelete]
        public void RemoveWork([FromBody] Work RemoveWork)
        {
            developerRepository.DeleteWork(RemoveWork);
        }
    }
}
