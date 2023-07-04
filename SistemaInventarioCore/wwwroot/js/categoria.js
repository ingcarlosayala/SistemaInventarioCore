
var datatable;

$(document).ready(function () {
    CargarTabla();
});

const CargarTabla = () => {
    datatable = $("#tblDato").DataTable({
        "ajax": {
            "url": "/Admin/Categorias/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            {
                "data": "estado",
                "render": function (estado) {
                    if (estado) {
                        return "Activo";
                    } else {
                        return "Inactivo";
                    }
                },"width": "15%"
            },
            {
                "data": "id",
                "render": function (id) {
                    return `<div class="text-center">
                                <a href="/Admin/Categorias/Editar/${id}" class="btn btn-secondary btn-sm"> <i class="fa-solid fa-pen-to-square"></i> </a>
                                <a onclick=Delete("/Admin/Categorias/Delete/${id}") class="btn btn-danger btn-sm"> <i class="fa-solid fa-trash"></i> </a>
                            </div>`;
                },"width": "15%"
            }
        ]
    });
}

const Delete = url => {
    Swal.fire({
        title: 'Esta Seguro?',
        text: "No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'SI, Eliminar!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (id) {
                    if (id) {
                        toastr.success(id.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(id.message);
                    }
                }
            });
        }
    })
}