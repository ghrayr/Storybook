function LoadPage(page, event) {
    event = event || window.event;
    if (!page) page = 1;
    var $this = $(event.target);
    var $pager = $this.parents("div[data-pager='true']:first");
    var $target = $($pager.attr("data-target"));
    var funcName = $pager.attr("data-function");
    if (funcName) {
        var func = window[funcName];

        if (typeof func === "function") {
            func(page);
        }
    } else {
        var url = $pager.attr("data-url") + "?page=" + page;
        var callback = $pager.attr("data-callback");

        $target.load(url, { cache: false }, function () {
            if (callback && callback != '') {
                var fn = window[callback];
                if (typeof fn === "function") {
                    fn();
                }
            }
        });
    }
    return false;
}

function joinGroup(url, id, page) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({ id: id, page: page }),
        contentType: 'application/json; charset=utf-8',
        success: function(result) {
            if (result)
                $("#divGroups").html(result);
        }
    });
}