$(document).ready(function () {

    $('#CountyId').change(function () {

        var url = "https://" + $(location).attr('host') + "/Districts/GetDistricts";
        var ddlsource = "#CountyId";

        $("#DistrictId").prop("disabled", false);

        $.getJSON(url + "/" + $(ddlsource).val(), function (data) {
            var items = '';

            $("#DistrictId").empty();

            $.each(data, function (i, state) {
                items += "<option value='" + state.value + "'>" + state.text + "</option>";
            });

            $("#DistrictId").html(items);
        });

        $('#DistrictId').change(function () {
            $("#TownId").prop("disabled", false);
        });

    });
});