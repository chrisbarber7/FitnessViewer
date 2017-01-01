$(document).ready(function () {

    /*
    Code to allow inline editing of activity details (alternative to modal popup).  
    Search for INLINE_EDIT for other commented out code.    

    $('a.edit').click(function () {
        var container = $(this).parent().parent();
        var label = container.find('label');
        label.hide();
        var textBox = container.find('input[type="text"]');
        textBox.val(label.text());
        var saveButton = container.find('#saveDescription');
        saveButton.show();
        textBox.show().focus();
        $(this).hide();
    });
    $('#saveDescription').click(function () {
        var container = $(this).parent();
        var textBox = container.find('input[type="text"]');
        $.post("/api/Activity/UpdateDescription", { '': textBox.val() })
                  .done(function (data) {
                      var label = container.find('label');
                      label.text(data);
                  });
        textBox.hide();
        $(this).hide();
        container.find('label').show();
    });
*/
    var selectedPolyline;
    var fullRouteLatLng;
   // var hasMap = document.getElementById('hasMap').value;
    // user clicks on any of the options in the lap info panel.  We'll load the summary info for that section and highlight the section on the map.
    $('ul.laps li').click(function (e) {
        $("li.selectedLap").removeClass("selectedLap");
        $(this).addClass("selectedLap");
        var startIndex = $(this).attr("data-start-index");
        var endIndex = $(this).attr("data-end-index");
        var streamStep = $(this).attr("data-stream-step");
        var activityId = document.getElementById('activityId').value;
        var selectedText = encodeURIComponent($(this).find(".lapName").text());
        $("#activitySummaryInformation").load("/Activity/GetActivitySummaryInformation?activityId=" + activityId + "&selection=" + selectedText + "&startIndex=" + startIndex + "&endIndex=" + endIndex);

        if (hasMap === "True") {

            // for map and graphs we need to take the steps in data into account
            startIndex = $(this).attr("data-start-stepped-index");;
            endIndex = $(this).attr("data-end-stepped-index");

            // if previous selection exists then removeit
            if (selectedPolyline !== undefined)
                mymap.removeLayer(selectedPolyline);

            // strip the full route coordinated just keeping the section we are interested in (no need for stream step on the map as the map has reduced data points)
            var selectedLatLng = fullRouteLatLng.slice(startIndex, endIndex);

            selectedPolyline = L.polyline(selectedLatLng, { color: 'blue' }).addTo(mymap);

            // un-comment to zoom in on selected polyline.  Undecided if I like it or not as hard to see where on route you are!
            //mymap.fitBounds(selectedPolyline.getBounds());
        }
    });

   
    var activityId = document.getElementById('activityId').value;
    var hasMap = document.getElementById('hasMap').value;

    if (hasMap === "True") {
        var mymap = L.map('mapid');
        fullRouteLatLng = getCoords(activityId);
        var polyline = L.polyline(fullRouteLatLng, { color: 'red' }).addTo(mymap);
        mymap.fitBounds(polyline.getBounds());

        var streets = L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=sk.eyJ1IjoiY2hyaXNiYXJiZXI3IiwiYSI6ImNpdHlxcDdnOTAwNGUzbm9hMDNueDBla2IifQ.uCc724sMqgSk316I0XuPlA', {
            maxZoom: 18,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
            '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets'
        });

        streets.addTo(mymap);

        var outdoors = L.tileLayer('https://api.mapbox.com/styles/v1/mapbox/outdoors-v9/tiles/256/{z}/{x}/{y}?access_token=pk.eyJ1IjoiY2hyaXNiYXJiZXI3IiwiYSI6ImNpdHlxbHo4MDAwM280NnA2eW8yMnlrNGEifQ.tp_Iz6xwf9m9_QXpdC6D8Q', {
            maxZoom: 18,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
            '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.outdoors'
        });

        outdoors.addTo(mymap);

        var satellite = L.tileLayer('https://api.mapbox.com/styles/v1/mapbox/satellite-streets-v9/tiles/256/{z}/{x}/{y}?access_token=pk.eyJ1IjoiY2hyaXNiYXJiZXI3IiwiYSI6ImNpdHlxbHo4MDAwM280NnA2eW8yMnlrNGEifQ.tp_Iz6xwf9m9_QXpdC6D8Q', {
            maxZoom: 18,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
            '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.outdoors'
        });

        var baseLayers = {
            "streets": streets,
            "outdoors": outdoors,
            "satellite": satellite
        };

        L.control.layers(baseLayers).addTo(mymap);
    }

    function getCoords(activityId) {
        var fullRouteLatLng = null;
        $.ajax({
            async: false,
            global: false,
            dataType: "json",
            type: "GET",
            url: "/api/Activity/GetActivityCoords/" + activityId,
            success: function (data) {
                fullRouteLatLng = data;
            },
            error: function () {
                alert("Error getting coords data!");
            }
        });
        return fullRouteLatLng;
    }


    SetupPowerCurveChart("chartPowerCurve");

    function SetupPowerCurveChart(chartName) {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/api/Activity/GetPowerCurve/" + activityId,
            data: JSON,
            success: function (data) {
                PowerCurveChart(data);
            },
            error: function () {
                alert("Error loading activity data!");
            }
        });

        function PowerCurveChart(data) {
            var powerCurveChartData = {
                labels: data.duration,
                datasets: [
                    {
                        label: 'Power',
                        data: data.watts,
                        radius: 0,
                        fill: false,
                        borderColor: 'blue',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth: 1
                    }


                ]
            };



            var ctx = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Line(ctx, {
                data: powerCurveChartData,
                options: {
                    animation: false,
                    //fill: false,
                    //beizierCurve: false,
                    responsive: true,
                    datasetFill: true
                    //,
                    //scales: {
                    //    xAxes: [{
                    //        type: "logarithmic",
                    //        position: "bottom",
                    //        ticks: {
                    //            min:1,
                    //        max:2000
                    //        }
                    //    }]
                    //}
                   
                }
            });
        }


    }




     
        SetupActivityChart("chartActivity");

        function SetupActivityChart(chartName) {
            $.ajax({
                dataType: "json",
                type: "GET",
                url: "/api/Activity/GetActivityStreams/" + activityId,
                data: JSON,
                success: function (data) {
                    ActivityChart(data);
                },
                error: function () {
                    alert("Error loading activity data!");
                }
            });

            function ActivityChart(data) {
                var barChartData = {
                    labels: data.Time,
                    datasets: [
                        {
                            label: 'Power',
                            data: data.Watts,
                            radius: 0,
                            fill: false,
                            borderColor: 'blue',
                            yAxesID: 'y-axis-0',
                            lineTension: 0,
                            pointRadius: 0,
                            borderWidth: 1
                        },
                        {
                            label: 'Heart Rate',
                            data: data.HeartRate,
                            radius: 0,
                            fill: false,
                            borderColor: 'red',
                            yAxesID: 'y-axis-0',
                            lineTension: 0,
                            pointRadius: 0,
                            borderWidth: 1
                        },
                        {
                            label: 'Elevation',
                            data: data.Altitude,
                            radius: 0,
                            fill: false,
                            borderColor: 'green',
                            yAxesID: 'y-axis-1',
                            lineTension: 0,
                            pointRadius: 0,
                            borderWidth: 1
                        }
                    ]
                };

                Chart.defaults.scale.ticks.autoSkipPadding = 100;

                var ctx = document.getElementById(chartName).getContext("2d");

                var myBarChart = Chart.Line(ctx, {
                    data: barChartData,
                    options: {
                        animation: false,
                        fill: false,
                        beizierCurve: false,
                        responsive: true,
                        datasetFill: true,
                        scales: {
                            yAxes: [{
                                position: "left",
                                "id": "y-axis-0"
                            }, {
                                position: "right",
                                "id": "y-axis-1"
                            }]
                        }
                    }
                });
            }


        }
 
        });
  