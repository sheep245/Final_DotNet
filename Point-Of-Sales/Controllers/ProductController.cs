﻿using Microsoft.AspNetCore.Mvc;

namespace Point_Of_Sales.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}