function CreateReport(parameters) {
    const url = parameters.url;

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {

        },
        failure: function () {

        },
        error: function (response) {
            /*alert(response.responseText);*/

        }
    });
};
