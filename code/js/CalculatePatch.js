function initialise() {
//    setTimeout(function () { requestDynamicContent() }, 1000);
    requestDynamicContent();
}

function requestDynamicContent() {
    var getDynamicContentUrl = window.location.href + "/?GetDynamicContent=1";    
    $.ajax({
        url: getDynamicContentUrl,
        success: function (result) {
            injectDynamicContent(result);
        }
    });    
}

function injectDynamicContent(content) {
    $('[data-rs="0"]').each(function () {
        var uid = $(this).attr("data-rid");

        var attributeWithUid = "[data-rid='" + uid + "']";

        var dynamicElement = $(content).find(attributeWithUid);
        if (dynamicElement != undefined) {
            $(this).replaceWith(dynamicElement.prevObject);
        }
    });
}