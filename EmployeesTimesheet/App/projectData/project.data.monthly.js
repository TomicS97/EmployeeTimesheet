var ProjectData = function () {
    var rebindEvents = function () {

        $('#DateStart').datetimepicker({
            defaultDate: 'now',
            format: 'L'
        });
        $('#DateEnd').datetimepicker({
            defaultDate: 'now',
            format: 'L'
        });
        $("#DateStart").on("dp.change", function (e) {
            $('#DateEnd').data("DateTimePicker").minDate(e.date);
        });
        $("#DateEnd").on("dp.change", function (e) {
            $('#DateStart').data("DateTimePicker").maxDate(e.date);
        });

        $("#btnGetData").click(function () {
            data = {
                fromDate: $("#DateStart").find('input').val(),
                toDate: $("#DateEnd").find('input').val()
            };
            getProjectDataForMonth(data);
        });

    };
    var getProjectDataForMonth = function (data) {
        $.ajax({
            url: '/api/Analytic/GetProjectDataBetweenDates',
            data: data,
            success: function (data) {
                Helpers.createProjectTemplate(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                toastr.error(textStatus, jqXHR.responseJSON.Message);
            }
        });
    };
    var loadData = function () {
        $.ajax({
            url: '/api/Analytic/GetProjectDataLastMonth',
            success: function (data) {
                Helpers.createProjectTemplate(data);

            },
            error: function (request, error) {

                alert(" Error: " + error);
            }
        });
    };
    
    return {
        init: function () {
            rebindEvents();
            loadData();
        }
    };
}();
$(document).ready(function () {
    ProjectData.init();
});