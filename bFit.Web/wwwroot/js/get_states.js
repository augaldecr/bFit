$(document).ready(function () {

    $('#CountryId').change(function () {

        var url = "https://" + $(location).attr('host') + "/States/GetStates";
        var ddlsource = "#CountryId";

        $("#StateId").prop("disabled", false);

        $.getJSON(url + "/" + $(ddlsource).val(), function (data) {
            var items = '';

            $("#StateId").empty();

            $.each(data, function (i, state) {
                items += "<option value='" + state.value + "'>" + state.text + "</option>";
            });

            $("#StateId").html(items);
        });

        $('#StateId').change(function () {
            $("#Name").prop("disabled", false);
        });

    });

});