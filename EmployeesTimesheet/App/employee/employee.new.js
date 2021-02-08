var EmployeeNew = function () {
    var dropdowns = [];
    var Role;
    var TeamLeader;

    var rebindEvents = function () {

        $(document.body).on("change", "#Role", function () {
            if (this.value === "2") {
                Helpers.SelectInSelect2($("#TeamLeader"), "");
                $("#TeamLeader").prop("disabled", true);              
            } else {
                $("#TeamLeader").prop("disabled", false);
            }
        });
    };
    var initSelect2 = function () {
        Helpers.elSelect2AllowClear($("#TeamLeader"), dropdowns.UsersList);
        Helpers.elSelect2($("#Role"), dropdowns.UsersRoleList);
        Helpers.SelectInSelect2($("#Role"), Role);
        Helpers.SelectInSelect2($("#TeamLeader"), TeamLeader);
    };

    var loadDropdowns = function () {
        $.ajax({
            url: '/Employee/LoadDropdowns',
            success: function (data) {
                if (data.Response === "OK") {
                    dropdowns.UsersList = data.data.UsersList;
                    dropdowns.UsersRoleList = data.data.UsersRoleList;
                    initSelect2();
                }
                else { toastr.error("Error"); }
            }
        });
    };
    return {
        init: function (RoleId, TeamLeaderId) {
            Role = RoleId; 
            TeamLeader = TeamLeaderId;
            rebindEvents();            
            loadDropdowns();

        }
    };
}();

jQuery(document).ready(function () {
    EmployeeNew.init();   
});