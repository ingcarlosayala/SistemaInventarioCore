
var datatable;

$(document).ready(function () {
    CargarTabla();
});

const CargarTabla = () => {
    datatable = $("#tblDato").DataTable({
        "ajax": {
            "url": "/Admin/Usuarios/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre" },
            { "data": "apellido" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "role" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        // Usuario esta Bloqueado
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger btn-sm text-white" style="cursor:pointer">
                                    <i class="fa-solid fa-lock"></i>
                               </a> 
                            </div>
                        `;
                    }
                    else {
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success btn-sm text-white" style="cursor:pointer" >
                                    <i class="fa-solid fa-unlock"></i>
                               </a> 
                            </div>
                        `;
                    }

                }
            }
        ]
    });
}

const BloquearDesbloquear = id => {

    $.ajax({
        type: "POST",
        url: '/Admin/Usuarios/BloquearDesbloquear',
        data: JSON.stringify(id),
        contentType: "application/json",
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