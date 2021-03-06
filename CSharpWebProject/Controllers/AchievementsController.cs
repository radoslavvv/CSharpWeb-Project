﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebProject.Controllers
{
    [Authorize]
    public class AchievementsController : Controller
    {
        private IAchievementsService achievementsService;
        private IUsersService usersService;
        public AchievementsController(IAchievementsService achievementsService, IUsersService usersService)
        {
            this.achievementsService = achievementsService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            string username = this.User.Identity.Name;
            MyAchievementsViewModel achievements = this.achievementsService.GetAllAchievements(username);

            return View(achievements);
        }
    }
}