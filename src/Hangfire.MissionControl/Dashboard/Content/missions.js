"use strict";

function onMissionStart(e, jobId) {
    e.preventDefault();
    var data = $("#" + jobId).serializeArray();

    $.ajax({
        url: baseUrl + "?" + idFieldName + "=" + jobId,
        data: data,
        type: "post",
        success: function (r) {
            $(jobId + "-alerts").append("<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">×</span></button>Mission launched with id <strong>" + r + "</strong></div>");
        },
        error: function (r) {
            $(jobId + "-alerts").append("<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">×</span></button>An error occured during launching <strong>" + JSON.stringify(r) + "</strong></div>");
        }
    });
};
