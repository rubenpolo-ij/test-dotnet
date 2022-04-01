using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DotNet.Models;
using Test.Model;
using Test.Services;

namespace Test.DotNet.Controllers
{
    public class CandidateExperienceController : Controller
    {

        private readonly ICandidateExperienceService candidateExperienceService;

        public CandidateExperienceController(ICandidateExperienceService candidateExperienceService)
        {
            this.candidateExperienceService = candidateExperienceService ?? throw new ArgumentNullException(nameof(candidateExperienceService));
        }

        [HttpGet]
        [Route("Candidate/Edit/{idCandidate}/Experience/List")]
        public IActionResult List(int idCandidate)
        {
            var model = candidateExperienceService.GetByIdCandidate(idCandidate);

            return Json(ToViewModel(model));
        }

        [HttpGet]
        [Route("Candidate/{idCandidate}/Experience/Create")]
        public IActionResult Create(int idCandidate)
        {
            CandidateExperience candidateExperience = new CandidateExperience()
            {
                IdCandidate = idCandidate
            };
            return View(candidateExperience);
        }

        [HttpPost]
        [Route("Candidate/{idCandidate}/Experience/Create")]
        public IActionResult Create(CandidateExperience candidateExperience)
        {
            if (ModelState.IsValid)
            {
                candidateExperienceService.Add(candidateExperience);

                return RedirectToAction( nameof(Edit),"Candidate", new { id = candidateExperience.IdCandidate });
            }
            return View(candidateExperience);
        }

        [HttpGet]
        [Route("Candidate/{idCandidate}/Experience/Edit/{id}")]
        public IActionResult Edit(int idCandidate, int id)
        {
            var candidateExperience = candidateExperienceService.GetById(idCandidate,id);

            if (candidateExperience == null)
            {
                Response.StatusCode = 404;
                return View("ExperienceNotFound", new List<int> { idCandidate, id });
            }

            return View(candidateExperience);
        }

        [HttpPost]
        [Route("Candidate/{idCandidate}/Experience/Edit/{id}")]
        public IActionResult Edit(CandidateExperience candidateExperience)
        {
            if (ModelState.IsValid)
            {
                candidateExperienceService.Update(candidateExperience);

                return RedirectToAction(nameof(Edit), "Candidate", new { id = candidateExperience.IdCandidate });
            }
            return View(candidateExperience);
        }

        [HttpPost]
        [Route("Candidate/{idCandidate}/Experience/Delete/{id}")]
        public IActionResult Delete(int idCandidate, int id)
        {
            var candidateExperience = candidateExperienceService.GetById(idCandidate, id);

            if (candidateExperience == null)
            {
                return NotFound("Candidate experience cannot be found.");
            }

            candidateExperienceService.Delete(candidateExperience);

            return NoContent();
        }

        private List<CandidateExperienceListViewModel> ToViewModel(List<CandidateExperience> candidateExperiences)
        {
            List<CandidateExperienceListViewModel> convertedList = new List<CandidateExperienceListViewModel>();
            foreach (var candidateExperience in candidateExperiences)
            {
                convertedList.Add(ToViewModel(candidateExperience));
            }
            return convertedList;
        }

        private CandidateExperienceListViewModel ToViewModel(CandidateExperience candidateExperience)
        {
            return new CandidateExperienceListViewModel()
            {
                IdCandidate = candidateExperience.IdCandidate,
                IdCandidateExperience = candidateExperience.IdCandidateExperience,
                Company = candidateExperience.Company,
                Job = candidateExperience.Job,
                Description = candidateExperience.Description,
                Salary = candidateExperience?.Salary ?? 0,
                BeginDate = candidateExperience.BeginDate?.ToString("MM/yyyy"),
                EndDate = candidateExperience.EndDate?.ToString("MM/yyyy")
            };
        }
    }
}
