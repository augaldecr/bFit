$(document).ready(function () {

    $('#StateId').change(function () {

        var url = "https://" + $(location).attr('host') + "/Counties/GetCounties";
        var ddlsource = "#StateId";

        $("#CountyId").prop("disabled", false);

        $.getJSON(url + "/" + $(ddlsource).val(), function (data) {
            var items = '';

            $("#CountyId").empty();

            $.each(data, function (i, state) {
                items += "<option value='" + state.value + "'>" + state.text + "</option>";
            });

            $("#CountyId").html(items);
        });

        $('#CountyId').change(function () {
            $("#DistrictId").prop("disabled", false);
        });

    });
});