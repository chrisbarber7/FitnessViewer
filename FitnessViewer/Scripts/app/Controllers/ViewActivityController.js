$(document).ready(function () {

    var selectedPolyline;

    // user clicks on any of the options in the lap info panel.  We'll load the summary info for that section and highlight the section on the map.
    $('ul.laps li').click(function (e) {
        var startIndex = $(this).attr("data-start-index");
        var endIndex = $(this).attr("data-end-index");
        var activityId = document.getElementById('activityId').value;
        $("#activitySummaryInformation").load("/Activity/GetSummaryInformation?activityId=" + activityId + "&startIndex=" + startIndex + "&endIndex=" + endIndex);

        // if previous selection exists then removeit
        if (selectedPolyline!=undefined)
            mymap.removeLayer(selectedPolyline);

        // get full route coords and just keep the section we are interested in.
        var selectedCoors = getCoords(activityId).slice(startIndex, endIndex);

        selectedPolyline = L.polyline(selectedCoors, { color : 'blue' }).addTo(mymap);

        // un-comment to zoom in on selected polyline.  Undecided if I like it or not as hard to see where on route you are!
        //mymap.fitBounds(selectedPolyline.getBounds());
    });

    var mymap = L.map('mapid');
	var activityId = document.getElementById('activityId').value;

	var routeCoords = getCoords(activityId);
	var polyline = L.polyline(routeCoords, { color: 'red' }).addTo(mymap);
	mymap.fitBounds(polyline.getBounds());

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=sk.eyJ1IjoiY2hyaXNiYXJiZXI3IiwiYSI6ImNpdHlxcDdnOTAwNGUzbm9hMDNueDBla2IifQ.uCc724sMqgSk316I0XuPlA', {
	    maxZoom: 18,
	        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
			'<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
			'Imagery © <a href="http://mapbox.com">Mapbox</a>',
			    id: 'mapbox.streets'
    }).addTo(mymap);

    function getCoords(activityId) {
    var routeCoords = null;
    $.ajax({
        async: false,
        global: false,
        dataType: "json",
        type: "POST",
        url: "/api/Activity/GetMapCoords/" + activityId,
        success: function (data) {
            routeCoords = data;
        },
        error: function () {
            alert("Error getting coords data!");
        }
    });
    return routeCoords;
};

});

//function initActivityMap() {
//    var map = new google.maps.Map(document.getElementById('map'), {
//        //   zoom: 14,
//        //     center: { lat: 53.450128, lng: -2.078496 },
//        mapTypeId: google.maps.MapTypeId.ROADMAP
//    });

//    var activityId = document.getElementById('activityId').value;

//    var routeCoords = (function () {
//        var routeCoords = null;
//        $.ajax({
//            async: false,
//            global: false,
//            dataType: "json",
//            type: "POST",
//            url: "/api/Activity/GetMapCoords/" + activityId,
//            success: function (data) {
//                routeCoords = data;
//            },
//            error: function () {
//                alert("Error getting coords data!");
//            }
//        });
//        return routeCoords;
//    })();

//    // override centre/boundaries
//    var latlngbounds = new google.maps.LatLngBounds();
//    routeCoords.forEach(function (latLng) {
//        latlngbounds.extend(latLng);
//    });

//    map.setCenter(latlngbounds.getCenter());
//    map.fitBounds(latlngbounds);

//    var routePath = new google.maps.Polyline({
//        path: routeCoords,
//        geodesic: true,
//        strokeColor: '#FF0000',
//        strokeOpacity: 1.0,
//        strokeWeight: 2
//    });

//    routePath.setMap(map);
//}