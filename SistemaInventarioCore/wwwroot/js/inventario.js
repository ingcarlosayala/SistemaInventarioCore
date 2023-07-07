
var datatable;

$(document).ready(function () {
    CargarTabla();
});

const CargarTabla = () => {
    datatable = $("#tblDato").DataTable({
        "ajax": {
            "url": "/Inventario/Inventarios/ObtenerTodos"
        },
        "columns": [
            { "data": "bodega.nombre"},
            { "data": "producto.nombre"},
            {
                "data": "producto.costo", "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d;
                }
            },
            { "data": "cantidad", "className": "text-cend" }
        ]
    });
}
