﻿namespace Candidate.Areas.Dashboard.Controllers {
    using System.Linq;
    using System.Web.Mvc;
    using Candidate.Areas.Dashboard.Models;
    using Candidate.Core.Settings;
    using Candidate.Core.Settings.Model;
using Candidate.Core.Services;

    public class ConfigurationController : Controller {
        private ISettingsManager _settingsManager;
        private IHashService _hashServices;

        public ConfigurationController(ISettingsManager settingsManager, IHashService hashServices) {
            _settingsManager = settingsManager;
            _hashServices = hashServices;
        }

        [HttpGet]
        public ActionResult Index(string jobName) {
            ViewBag.JobName = jobName;

            return View();
        }

        [HttpGet]
        public ActionResult Github(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<SitesConfigurationList>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            if (string.IsNullOrEmpty(jobConfiguration.Github.Hook)) {
                jobConfiguration.Github.Hook = "http:/" + Url.Action("Hook", new { 
                    area = "Dashboard", 
                    controller = "Setup", 
                    jobName = jobName, 
                    token = _hashServices.CreateMD5Hash(jobName) });
            }

            return View(jobConfiguration.Github);
        }

        [HttpPost]
        public ActionResult Github(string jobName, GitHub config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<SitesConfigurationList>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new SiteConfiguration { JobName = jobName, Github = config });
                }
                else {
                    jobConfiguration.Github = config;
                }

                return Json(new { success = true, settings = config });
            }
        }

        [HttpGet]
        public ActionResult Iis(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<SitesConfigurationList>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            return View(jobConfiguration.Iis);
        }

        [HttpPost]
        public ActionResult Iis(string jobName, Iis config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<SitesConfigurationList>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new SiteConfiguration { JobName = jobName, Iis = config });
                }
                else {
                    jobConfiguration.Iis = config;
                }

                return Json(new { success = true, settings = config });
            }
        }

        [HttpGet]
        public ActionResult Solution(string jobName) {
            var currentSettings = _settingsManager.ReadSettings<SitesConfigurationList>();
            var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

            return View(jobConfiguration.Solution);
        }

        [HttpPost]
        public ActionResult Solution(string jobName, Solution config) {
            using (var settingsManager = new TrackableSettingsManager(_settingsManager)) {
                var currentSettings = settingsManager.ReadSettings<SitesConfigurationList>();
                var jobConfiguration = currentSettings.Configurations.Where(c => c.JobName == jobName).SingleOrDefault();

                if (jobConfiguration == null) {
                    currentSettings.Configurations.Add(new SiteConfiguration { JobName = jobName, Solution = config });
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
                var currentSettings = settingsManager.ReadSettings<SitesConfigurationList>();
                var jobToDelete = currentSettings.Configurations.Where(j => j.JobName == deleteJob.JobName).SingleOrDefault();
                var currentJobs = currentSettings.Configurations.Remove(jobToDelete);

                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}
