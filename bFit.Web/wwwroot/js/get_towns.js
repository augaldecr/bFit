$(document).ready(function () {

    $('#DistrictId').change(function () {

        var url = "https://" + $(location).attr('host') + "/Towns/GetTowns";
        var ddlsource = "#DistrictId";

        $("#TownId").prop("disabled", false);

        $.getJSON(url + "/" + $(ddlsource).val(), function (data) {
            var items = '';

            $("#TownId").empty();

            $.each(data, function (i, state) {
                items += "<option value='" + state.value + "'>" + state.text + "</option>";
            });

            $("#TownId").html(items);
        });

        $('#TownId').change(function () {
            $("#Name").prop("disabled", false);
        });

    });
});