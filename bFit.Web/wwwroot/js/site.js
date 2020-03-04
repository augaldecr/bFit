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
});