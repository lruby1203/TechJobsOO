using Microsoft.AspNetCore.Mvc;
using TechJobs.Data;
using TechJobs.ViewModels;
using TechJobs.Models;

namespace TechJobs.Controllers
{
    public class JobController : Controller
    {

        // Our reference to the data store
        private static JobData jobData;

        static JobController()
        {
            jobData = JobData.GetInstance();
        }

        // The detail display for a given Job at URLs like /Job?id=17
        public IActionResult Index(int id)
        {
            Job job = new Job();
            job = jobData.Find(id);
            return View(job);
        }

        public IActionResult New()
        {
            NewJobViewModel newJobViewModel = new NewJobViewModel();
            return View(newJobViewModel);
        }

        [HttpPost]
        public IActionResult New(NewJobViewModel newJobViewModel)
        {
            if (ModelState.IsValid)
            {
                Employer employer = jobData.Employers.Find(newJobViewModel.EmployerID);
                Location location = jobData.Locations.Find(newJobViewModel.LocationID);
                CoreCompetency skill = jobData.CoreCompetencies.Find(newJobViewModel.SkillID);
                PositionType position = jobData.PositionTypes.Find(newJobViewModel.TypeID);
                Job newJob = new Job()
                {
                    Name = newJobViewModel.Name,
                    Employer = employer,
                    Location = location,
                    CoreCompetency = skill,
                    PositionType = position
                };
                jobData.Jobs.Add(newJob);
                int id = newJob.ID;
                string path = string.Format("/Job?id={0}", id);
                return Redirect(path);
            }
            else
            {
                return View(newJobViewModel);
            }
        }
    }
}
