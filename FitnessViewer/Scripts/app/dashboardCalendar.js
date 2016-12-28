var DashboardCalendar = function () {
    var init = function () {

        $('#reportrange').daterangepicker({
            startDate: dashboardStart,
            endDate: dashboardEnd,
            "opens": "left",
            ranges: {

                // any ranges added need to also be added to DashboardDateRange.Calculate
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'Last 90 Days': [moment().subtract(89, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'This Year': [moment().startOf('year'), moment()]
            }
        }, DateRangeSelected);

        DateRangeSelected(dashboardStart, dashboardEnd);
    };


    function DateRangeSelected(s, e, label) {
        $('#reportrange span').html(s.format('MMMM D, YYYY') + ' - ' + e.format('MMMM D, YYYY'));
        dashboardStart = s;
        dashboardEnd = e;
        updateWeeklyReports();

        // first time in label will be undefined so skip out populating controls which are populated from the view model.
        if (label !== undefined) {
            initReports();
            UpdateDashboardSettings(s, e, label);
        }
    }

    var UpdateDashboardSettings = function (s, e, l) {
        var model = {
            From: s.utc().format("X"),
            To: e.utc().format("X"),
            DashboardRange: l
        };
        $.post("/settings/UpdateDashboardPeriod", model, function (data) {
            if (!data.success) {
                showError(data.responseText);
                resetIcons(id);
            }
        });
    };

    
    return {
        init: init
    };
}();