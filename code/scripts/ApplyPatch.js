$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);

    if (!urlParams.has('skippatch')) {
        requestDynamicContent();
    }    
});

function requestDynamicContent() {
    var getDynamicContentUrl = window.location.href;
    getDynamicContentUrl = getDynamicContentUrl + (window.location.href.includes('?') ? "&" : "?");
    getDynamicContentUrl = getDynamicContentUrl + "GetDynamicContent=1";

    $.ajax({
        url: getDynamicContentUrl,
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