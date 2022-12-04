$(document).ready(function () {
    var skip = 3;
    $(document).on('click', '#loadMore', function () {
        $.ajax({
            method: "GET",
            url: "/blog/partial?skip=" + skip,
            success: function (html) {
                $("#blogRows").append(html);
                skip += 4;
                var productCount = $("#blogRows").val();
                if (skip >= productCount)
                    $("#loadMore").remove();
            }
        });
    })
})