using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Test.DotNet.Models;
using Test.Model;
using Test.Services;

namespace Test.DotNet.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ICandidateService candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult List()
        {
            var model = candidateService.GetAll();

            return Json(ToViewModel(model));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                candidateService.Add(candidate);

                return RedirectToAction(nameof(Edit), new { id = candidate.IdCandidate });
            }
            return View(candidate);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var candidate = candidateService.GetById(id);

            if(candidate == null)
            {
                Response.StatusCode = 404;
                return View("CandidateNotFound", id);
            }

            return View(candidate);
        }

        [HttpPost]
        public IActionResult Edit(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                candidateService.Update(candidate);

                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var candidate = candidateService.GetById(id);

            if (candidate == null)
            {
                return NotFound("Candidate cannot be found.");
            }

            candidateService.Delete(candidate);
            
            return NoContent();
        }

        private List<CandidateListViewModel> ToViewModel(List<Candidate> candidates)
        {
            List<CandidateListViewModel> convertedList = new List<CandidateListViewModel>();
            foreach (var candidate in candidates)
            {
                convertedList.Add(ToViewModel(candidate));
            }
            return convertedList;
        }

        private CandidateListViewModel ToViewModel(Candidate candidate)
        {
            return new CandidateListViewModel()
            {
                IdCandidate = candidate.IdCandidate,
                Name = candidate.Name,
                Surname = candidate.Surname,
                Email = candidate.Email
            };
        }
    }
}
