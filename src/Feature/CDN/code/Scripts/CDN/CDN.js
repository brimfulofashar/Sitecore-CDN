$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);

    if (!urlParams.has('GetStaticContent')) {
        requestDynamicContent();
    }
});

function requestDynamicContent() {
    var dynamicRenderingIds = getDynamicRenderingIds();
    var getDynamicContentUrl = window.location.href.replace(window.location.hostname, $("body").data("original-host"));
    getDynamicContentUrl = getDynamicContentUrl + (window.location.href.includes('?') ? "&" : "?");
    getDynamicContentUrl = getDynamicContentUrl
        + "GetDynamicContent=1"
        + "&sessionId=" + getCookie('sc_ext_session')
        + "&contactId=" + getCookie('sc_ext_contact');

    $.ajax({
        url: getDynamicContentUrl,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('DynamicRenderingIds', dynamicRenderingIds.join("|"));
        },
        type: "GET",
        success: function (result) {
            injectDynamicContent(result);
        }
    });
}

function injectDynamicContent(dynamicContent) {
    $('[data-rendering-is-dynamic="1"]').each(function () {
        var uid = $(this).attr("data-rendering-id");

        var attributeWithUid = "[data-rendering-id='" + uid + "']";

        var dynamicElement = $(dynamicContent).filter(attributeWithUid);
        if (dynamicElement) {
            $(this).replaceWith(dynamicElement[0].innerHTML);
        }
    });
}

function getDynamicRenderingIds() {
    var dynamicRenderingIds = [];
    $('[data-rendering-is-dynamic="1"]').each(function () {
        var uid = $(this).attr("data-rendering-id");
        var placeholder = $(this).attr("data-placeholder");
        dynamicRenderingIds.push(uid + ":" + placeholder);
    });

    return dynamicRenderingIds;
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}