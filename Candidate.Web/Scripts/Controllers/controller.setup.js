﻿/// <reference path="../jquery-1.4.4-vsdoc.js" />

$(function () {

    var Run = function () {
        // Global config
        $.blockUI.defaults.message = null;

        var setup = $('#setup');
        var startSetupMethod = setup.data('start-setup');
        var cloneRepositoryMethod = setup.data('clone-repo');
        var pullRepositoryMethod = setup.data('pull-repo');
        var runBuildMethod = setup.data('run-build');
        var runTestMethod = setup.data('run-test');
        var runDeployMethod = setup.data('run-deploy');
        var stopWebSiteMethod = setup.data('site-stop');
        var startWebSiteMethod = setup.data('site-start');

        start();

        function start() {
            $.blockUI();

            $('#current-step').html('Stopping web site...');
            stopWebSite();
        }

        function stopWebSite() {

            $.ajax({
                url: stopWebSiteMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    startSetup();
                }
            });

        }

        function startSetup() {
            $.ajax({
                url: startSetupMethod,
                type: 'GET',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        setupStarted(r.setupInitInfo)
                    }
                }
            });

        }

        function setupStarted(setupInitInfo) {
            if (!setupInitInfo.isRepoCloned) {
                startRepositoryClone();
            } else {
                startPullRepository();
            }
        }

        function startPullRepository() {
            $('#current-step').html('Pulling changes from origin repository...');
            pullRepository();
        }

        function pullRepository() {
            $.ajax({
                url: pullRepositoryMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        cloneRepositoryDone();
                    }
                }
            });
        }

        function startRepositoryClone() {
            $('#current-step').html('Cloning repository...');
            cloneRepository();
        }

        function cloneRepository() {
            $.ajax({
                url: cloneRepositoryMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        cloneRepositoryDone();
                    }
                }
            });
        }

        function cloneRepositoryDone() {
            startBuild();
        }

        function startBuild() {
            $('#current-step').html('Running application build...');

            runBuild();
        }

        function runBuild() {
            $.ajax({
                url: runBuildMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        runBuildDone();
                    }
                }
            });

        }

        function runBuildDone() {
            startTests();
        }

        function startTests() {
            $('#current-step').html('Testing up application...');

            runTest();
        }

        function runTest() {
            $.ajax({
                url: runTestMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        runTestDone();
                    }
                }
            });
        }

        function runTestDone() {
            startDeployment();
        }

        function startDeployment() {
            $('#current-step').html('And now deploy application...');

            runDeploy();
        }

        function runDeploy() {
            $.ajax({
                url: runDeployMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    if (r.success) {
                        runDeployDone();
                    }
                }
            });
        }

        function runDeployDone() {

            startWebSite();
        }


        function startWebSite() {
            $('#current-step').html('Starting web site...');

            $.ajax({
                url: startWebSiteMethod,
                type: 'POST',
                processData: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (r) {
                    finish();
                }
            });

        }

        function finish() {
            $('#current-step').html('Application successfully launched!');
            $('.preloader').hide();
            $.unblockUI();
        }

        function updateConsole(line) {
            var currentConsoleContent = $('#run-console-log').html();
            currentConsoleContent += line;
            $('#run-console-log').html(currentConsoleContent);
        }
    } ();
});