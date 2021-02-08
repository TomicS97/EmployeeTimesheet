var ProjectDelete = function () {


    var rebindEvents = function () {
        $(".deleteProject").on("click", function () {                     
                var self = $(this);
                var tr = $(this).closest('tr');
                var projectId = self.attr("name");
                ajaxDeleteProject(projectId,tr);
        });       
    };
    var ajaxDeleteProject = function (projectId,tr) {
        $.ajax({
            url: '/Projects/DeleteProject',
            data: "projectId=" + projectId,
            success: function (data) {
                if (data.Response === "OK") {
                    toastr.success("Info Message", "Success");  
                    tr.remove();
                 }
                else { toastr.error("Error: " + data.Response); }
            },
            error: function (request, error) {
                
                alert(" Error: " + error);
            }
        });
    };
   
    return {
        init: function () {
            rebindEvents();
        }
    };
}();

jQuery(document).ready(function () {
    ProjectDelete.init();
});