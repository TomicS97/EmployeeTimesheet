


var ProjectEdit = function () {
    var selectedID;
    var dropdowns = [];

    var rebindEvents = function () {

    };
    var loadDropdowns = function () {
        $.ajax({
            url: '/Projects/GetProjectTypes',
            success: function (data) {
                if (data.Response === "OK") {
                    dropdowns.ProjectTypes = data.items.ProjectTypes;
                    initSelect2();
                    $("#TypeId").select2("val", selectedID);
                }
                else { toastr.error("Error: " + data.Result); }
            }
        });
    };
    var initSelect2 = function () {
        Helpers.elSelect2($("#TypeId"), dropdowns.ProjectTypes);
        Helpers.SelectInSelect2($("#TypeId"), selectedID);
    };

    return {
        init: function (TypeId) {
            rebindEvents();
            selectedID = TypeId;
            loadDropdowns();        
        }
    };
}();

jQuery(document).ready(function (projectType) {
    selectedID = projectType;
    ProjectEdit.init();

});