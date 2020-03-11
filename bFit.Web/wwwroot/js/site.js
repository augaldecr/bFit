$(document).ready(function () {
    $('#data_table').DataTable({
        language: {
            processing: "Procesando...",
            search: "Buscar&nbsp;:",
            lengthMenu: "Mostrando _MENU_ registros por página",
            zeroRecords: "Ningún registros coincide con los criterios de búsqueda",
            info: "Mostrando página _PAGE_ de _PAGES_",
            infoEmpty: "No hay información",
            infoFiltered: "(filtrado _MAX_ total de registros)",
            loadingRecords: "Carga de datos en curso",
            emptyTable: "No hay registros disponibles",
            paginate: {
                first: "Primera",
                previous: "Anterior",
                next: "Siguiente",
                last: "&Uacuteltima"
            },
            aria: {
                sortAscending: ": orden ascendente",
                sortDescending: ": orden descendente"
            }
        }
    });

    // Delete item
    var subset_to_edit;
    $('.deleteItem').click((e) => {
        subset_to_edit = e.currentTarget.dataset.id;
    });
    $("#btnYesDelete").click(function () {
        window.location.href = '/Owners/DeletePet/' + item_to_delete;
    });

    //Script to append sets to workout
    var subSet = $('#subSet').html();
    $('#btnAddSubset').click((e) => {
        $($('.subSets')).last().after(subSet);
    });

    //Script to add Sets to Workout
    $("#btnAddSet").click(function (e) {
        e.preventDefault(); 
        var post_url = $(this).attr("action");
        var request_method = $(this).attr("method");
        var form_data = $(this).serialize();

        $.ajax({
            url: post_url,
            type: request_method,
            data: form_data
        });
    });
});