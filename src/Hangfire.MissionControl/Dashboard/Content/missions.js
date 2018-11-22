"use strict";

function createAlert(style, innerContent) {
    return "<div class=\"alert " + style + "\" role=\"alert\">" + innerContent + "</div>";
}

function applyAlert(callerElement, alertsElementId, alertContent) {
    $(alertsElementId).removeClass("hidden");
    $(alertsElementId).append(alertContent);
    callerElement.disabled = false;
}

function onMissionStart(element, jobId) {
    var formElementId = "#" + jobId;
    var alertsElementId = formElementId + "-alerts";
    var data = $(formElementId).serializeArray();
    var launch = !requireConfirmation || confirm("Launch mission?");
    if (!launch) return;

    element.disabled = true;
    $.ajax({
        async: true,
        cache: false,
        timeout: 10000,
        url: baseUrl + "?" + idFieldName + "=" + jobId,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: data,
        type: "post",
        success: function (r) {
            var jobLink = jobLinkBaseUrl + r;
            var alert = createAlert("alert-success", "Mission launched with id: <a href=\"" + jobLink + "\"><strong>" + r + "</strong></a>");
            applyAlert(element, alertsElementId, alert);
        },
        error: function (r) {
            var alert = createAlert("alert-danger", "An error occured during launching: <br/><strong>" + r.responseText + "</strong>");
            applyAlert(element, alertsElementId, alert);
        }
    });
}

$(".datetimepicker").each(function () {
    $(this).datetimepicker({
        format: "YYYY-DD-MM HH:mm"
    });
});
