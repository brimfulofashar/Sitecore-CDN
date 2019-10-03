$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);

    if (!urlParams.has('skippatch')) {
        requestDynamicContent();
    }    
});

function requestDynamicContent() {
    var dynamicRenderingIds = getDynamicRenderingIds();
    var getDynamicContentUrl = window.location.href;
    getDynamicContentUrl = getDynamicContentUrl + (window.location.href.includes('?') ? "&" : "?");
    getDynamicContentUrl = getDynamicContentUrl + "GetDynamicContent=1";

    $.ajax({
        url: getDynamicContentUrl,
        type: "GET",
        beforeSend: function (xhr) { xhr.setRequestHeader('DynamicRenderingIds', dynamicRenderingIds.join("|")); },
        success: function (result) {
            injectDynamicContent(result);
        }
    });
}

function injectDynamicContent(dynamicContent) {
    $('[data-rs="0"]').each(function () {
        var uid = $(this).attr("data-rid");

        var attributeWithUid = "[data-rid='" + uid + "']";

        var dynamicElement = $(dynamicContent).filter(attributeWithUid);
        if (dynamicElement) {
            $(this).replaceWith(dynamicElement);
        }
    });
}

function getDynamicRenderingIds() {
    var dynamicRenderingIds = [];
    $('[data-rs="0"]').each(function() {
        var uid = $(this).attr("data-rid");
        dynamicRenderingIds.push(uid);
    });
    return dynamicRenderingIds;
}