﻿$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);

    if (!urlParams.has('GetStaticContent')) {
        requestDynamicContent();
    }    
});

function requestDynamicContent() {
    var getDynamicContentUrl = window.location.href;
    getDynamicContentUrl = getDynamicContentUrl + (window.location.href.includes('?') ? "&" : "?");
    getDynamicContentUrl = getDynamicContentUrl + "GetDynamicContent=1";

    $.ajax({
        url: getDynamicContentUrl,
        type: "GET",
        success: function (result) {
            injectDynamicContent(result);
        }
    });
}

function injectDynamicContent(dynamicContent) {
    $('[data-renderingIsPersonalised="1"]').each(function () {
        var uid = $(this).attr("data-renderingId");

        var attributeWithUid = "[data-renderingId='" + uid + "']";

        var dynamicElement = $(dynamicContent).filter(attributeWithUid);
        if (dynamicElement) {
            $(this).replaceWith(dynamicElement);
        }
    });
}