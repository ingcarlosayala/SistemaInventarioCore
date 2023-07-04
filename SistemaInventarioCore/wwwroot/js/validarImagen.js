
const ValidarImagen = () => {
    const img = document.querySelector("#imagen");


    if (img.value == "") {

        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Seleccione una Imagen!',
        })
        return false;
    }
    
}