var ProjectData = function () {

 
    var rebindEvents = function () {
      
        $('#Date').datetimepicker({
            defaultDate: 'now',
            viewMode: 'days',
            format: 'DD/MM/YYYY'
        });

        $('#Date').on('dp.change', function (e) {               
                var self = $(this);
                getProjectDataForDay(self.find('input').val());
        }); 
    };
    var getProjectDataForDay = function (date) {
        $.ajax({
            url: '/api/Analytic/GetProjectDataForDay',
            data: "date=" + date,
            success: function (data) {
                Helpers.createProjectTemplate(data);

            },
            error: function (request, error) {

                alert(" Error: " + error);
            }
        });
    };
    var loadData = function () {
        $.ajax({
            url: '/api/Analytic/GetProjectDataLastDay',           
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


