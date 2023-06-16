function openModal(parameters) {

    var requestData;
    if ('consultationId' in parameters) {
        requestData = { 'deadlineId': parameters.deadlineId, 'consultationId': parameters.consultationId }
    }
    else if ('deadlineId' in parameters && !('themeId' in parameters)) {
        requestData = { 'deadlineId': parameters.deadlineId, 'consultationId': 0 };
    }
    else if ('deadlineId' in parameters) {
        requestData = { 'deadlineId': parameters.deadlineId, "themeId": parameters.themeId }
    }
    else if ('studentId' in parameters) {
        requestData = { "studentId": parameters.studentId, "themeId": parameters.themeId };
    }
    else if ('themeId' in parameters) {
        requestData = { "themeId": parameters.themeId };
    }

    const url = parameters.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: requestData,
        success: function (response) {
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            var errorMessage = response.xhr.responseText;
            modal.find(".modal-body").html("Произошла ошибка: " + errorMessage);
        }
      });
};
