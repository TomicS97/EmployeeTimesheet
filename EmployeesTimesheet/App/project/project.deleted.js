var ProjectsDeleted = function () {


    var rebindEvents = function () {
        $(".btnReturnToActive").on("click", function () {
            var self = $(this);
            var projectId = self.attr("id");
            var tr = $(this).closest('tr');
            ajaxReturnToActive(projectId,tr);
        });

    };
    var ajaxReturnToActive = function (projectId,tr) {
        $.ajax({
            url: '/Projects/ReturnToActive',
            data: "projectId=" + projectId,
            success: function (data) {
                if (data.Response === "OK") {
                    toastr.success("Info Message", "Project Is Active Now");
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
    ProjectsDeleted.init();
});