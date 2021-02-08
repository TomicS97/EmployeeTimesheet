var TimeSheetNew = function () {
    var newRow = [];
    var newTimesheet = {};
    var rebindEvents = function () {

        $('#StartTime').datetimepicker({
            defaultDate: 'now',
            ignoreReadonly: true,
            format: 'HH:mm'
        });
        $('#EndTime').datetimepicker({
            defaultDate: 'now',
            ignoreReadonly: true,
            format: 'HH:mm'
        });
        $("#StartTime").on("dp.change", function (e) {
            $('#EndTime').data("DateTimePicker").minDate(e.date);
        });
        $("#EndTime").on("dp.change", function (e) {
            $('#StartTime').data("DateTimePicker").maxDate(e.date);
        });
        $('#Date').datetimepicker({
            defaultDate: 'now',
            viewMode: 'days',
            format: 'DD/MM/YYYY'
        }); 
        $('#Date').data("DateTimePicker").disable();
        $("#AddNewTimesheet").unbind().on("click", function () {
            newTimesheet = {};
            debugger
            $(".ff").each(function () {
                var self = $(this);
                var name = self.attr("name");
                newTimesheet[name] = self.val();                
            });
            ajaxNewRow(newTimesheet);
        });
       

    };
    var ajaxNewRow = function (data) {
        $.ajax({
            url: '/Projects/CreateNewTimesheet',
            data: data,
            type: 'POST',
            success: function (data) {
                if (data.Response === "OK") {
                    toastr.success("Info Message", "Success");
                    addNewRowInTable();
                }
                else { toastr.error("Error: " + data.Response); }
            }
        });
    };
   
    var addNewRowInTable = function () {
        $('#tableTimesheets > tbody').find('tr:last').prev().after(
            '<tr><td>'
            + newTimesheet.Details
            + '</td><td>'
            + newTimesheet.Date
            + '</td><td>'
            + newTimesheet.StartTime
            + '</td><td>'
            + newTimesheet.EndTime
            + '</td ></tr >');
              
    };

    return {
        init: function () {
            rebindEvents();
            
        }
    };
}();

jQuery(document).ready(function () {
    TimeSheetNew.init();

});