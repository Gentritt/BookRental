﻿using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookRental.Controllers
{
    public class GenreController : Controller
    {
        private static ApplicationDbContext _context;

		public GenreController()
		{
            _context = new ApplicationDbContext();
		}
        // GET: Genre
        public ActionResult Index()
        {
            var genres = _context.Genres.ToList();
            return View(genres);
        }

        public ActionResult Create()
		{
            return View();
		}

		protected override void Dispose(bool disposing)
		{
            _context.Dispose();
		}
	}
}