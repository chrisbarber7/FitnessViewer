

    var initReports = function () {
        $("#runSummaryInformation").load("/Athlete/GetSportSummary?sport=run&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#rideSummaryInformation").load("/Athlete/GetSportSummary?sport=Ride&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#swimSummaryInformation").load("/Athlete/GetSportSummary?sport=Swim&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#otherSummaryInformation").load("/Athlete/GetSportSummary?sport=Other&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#allSummaryInformation").load("/Athlete/GetSportSummary?sport=All&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));

    };

    var updateWeeklyReports = function () {
        setupWeeklyReport("chart12weekRun", "Run", "#873D48");
        setupWeeklyReport("chart12weekBike", "Ride", "#DC758F");
        setupWeeklyReport("chart12weekSwim", "Swim", "#955E42");
        setupWeightChart();
        setupTimeBySportChart("chartTimeBySport");
        setupTrainingLoadChart();
    };

