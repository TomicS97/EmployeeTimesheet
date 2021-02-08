var EmployeesData = function () {
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
            getEmploeesDataForMonth(data);
        });

    };
    var getEmploeesDataForMonth = function (data) {
        $.ajax({
            url: '/api/Analytic/GetEmployeesTasksBetweenDates',
            data: data,
            success: function (data) {
                Helpers.createEmployeeTemplate(data);  
            },
            error: function (jqXHR, textStatus, errorThrown) {
                toastr.error(textStatus, jqXHR.responseJSON.Message);
            }
        });
    };
    var loadData = function () {
        $.ajax({
            url: '/api/Analytic/EmployeeProjectDataLastMonth',
            success: function (data) {
                Helpers.createEmployeeTemplate(data);              
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
    EmployeesData.init();
});