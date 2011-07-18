﻿namespace Candidate.Areas.Dashboard.Controllers {
    using System.Linq;
    using System.Web.Mvc;
    using Candidate.Areas.Dashboard.Models;
    using Candidate.Core.Settings;
    using Candidate.Core.Settings.Model;

    public class ConfigurationController : Controller {
        private ISettingsManager _settingsManager;

        public ConfigurationController(ISettingsManager settingsManager) {
            _settingsManager = settingsManager;
        }

        [HttpGet]
        public ActionResult Index(string jobName) {
            ViewBag.JobName = jobName;

            return View();
        }

        [HttpGet]
        public ActionResult Github(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            return View(jobConfiguration == null ? null : jobConfiguration.Github);
        }

        [HttpPost]
        public ActionResult Github(string jobName, GithubModel config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new JobConfigurationModel { JobName = jobName, Github = config });
                }
                else {
                    jobConfiguration.Github = config;
                }

                return Json(new { success = true, settings = config });
            }
        }

        [HttpGet]
        public ActionResult Iis(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            return View(jobConfiguration == null ? null : jobConfiguration.Iis);
        }

        [HttpPost]
        public ActionResult Iis(string jobName, IisModel config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new JobConfigurationModel { JobName = jobName, Iis = config });
                }
                else {
                    jobConfiguration.Iis = config;
                }

                return Json(new { success = true, settings = config });
            }
        }

        [HttpGet]
        public ActionResult Solution(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            return View(jobConfiguration == null ? null : jobConfiguration.Solution);
        }

        [HttpPost]
        public ActionResult Solution(string jobName, SolutionModel config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<JobsConfigurationSettingsModel>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new JobConfigurationModel { JobName = jobName, Solution = config });
                }
                else {
                    jobConfiguration.Solution = config;
                }

                return Json(new { success = true, settings = config });
            }
        }

        [HttpGet]
        public ActionResult Delete(string jobName) {
            return View(new DeleteJobModel { JobName = jobName });
        }

        [HttpPost]
        public ActionResult Delete(DeleteJobModel deleteJob) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<JobsSettingsModel>();
                var jobToDelete = currentSettings.Jobs.Where(j => j.Name == deleteJob.JobName).SingleOrDefault();
                var currentJobs = currentSettings.Jobs.Remove(jobToDelete);

                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}
