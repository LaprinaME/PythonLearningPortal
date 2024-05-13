﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class TestsListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public TestsListController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: TestsList
        public async Task<IActionResult> Index()
        {
            var tests = await _context.Тесты.ToListAsync();
            return View(tests);
        }
    }
}

