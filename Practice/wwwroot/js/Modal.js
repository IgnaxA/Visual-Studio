function openModal(parameters) {
    const studentId = parameters.studentId;
    const themeId = parameters.themeId;
    const url = parameters.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "studentId": studentId, "themeId": themeId },
        success: function (response) {
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            /*alert(response.responseText);*/
            modal.find(".modal-body").html(response);
            modal.modal('show')
        }
      });
};
