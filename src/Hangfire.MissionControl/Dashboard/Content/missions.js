"use strict";

function createAlert(style, innerContent) {
    return "<div class=\"alert " + style + " alert-dismissible fade in\" role=\"alert\"> " +
        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
            "<span aria-hidden=\"true\">×</span>" +
        "</button>" + innerContent + "</div>";
}

function onMissionStart(e, jobId) {
    e.preventDefault();
    var formElementId = "#" + jobId;
    var alertsElementId = formElementId + "-alerts";
    var data = $(formElementId).serializeArray();

    $.ajax({
        url: baseUrl + "?" + idFieldName + "=" + jobId,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: data,
        type: "post",
        success: function (r) {
            var jobLink = jobLinkBaseUrl + r;
            var alert = createAlert("alert-success", "Mission launched with id: <a href=\"" + jobLink + "\"><strong>" + r + "</strong></a>");
            $(alertsElementId).append(alert);
        },
        error: function (r) {
            var alert = createAlert("alert-danger", "An error occured during launching: <br/><strong>" + r.responseText + "</strong>");
            $(alertsElementId).append(alert);
        }
    });
};
