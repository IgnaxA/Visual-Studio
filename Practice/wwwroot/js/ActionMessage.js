function SendAlert(parameters) {
    const fN = parameters.formName
    const data = $(fN).serialize()
    $.ajax({
        type: 'POST',
        url: parameters.url,
        data: parameters.formName,
        success: function (data) {

            Swal.fire({
                title: 'Изменение сохранено',
                text: data.description,
                icon: 'success',
                confirmButtonText: 'Окей'
            })
            $('#modal').modal('hide');


        },
        error: function (response) {
            Swal.fire({
                title: 'Ошибка',
                text: response.error,
                icon: 'error',
                confirmButtonText: 'Окей'
            })
        }
    })
};


