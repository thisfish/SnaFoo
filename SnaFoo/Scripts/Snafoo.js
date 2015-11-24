
$(document).ready()
{
    var cookieValue = parseInt($.cookie("vote"));
    if (cookieValue == 3) {
        $(".counter").addClass("isHidden")
    }
    else{
        switch (cookieValue) {
            case 2:
                $(".counter_red").removeClass("isHidden");
                break;
            case 1:
                $(".counter_yellow").removeClass("isHidden");
                break;
            default:
                $(".counter_green").removeClass("isHidden");
                break;
        }
    } 
}

$(".vote").click(function () {
    var Id = $(this).attr("data-id");
    var cookieValue =  parseInt($.cookie("vote")); 
    if (cookieValue == 3) {
        $(".voteLimit").removeClass("isHidden")
        $(".counter").addClass("isHidden")
    }
    else {
        $.removeCookie("vote");
        var date = new Date();
        var time = new Date(date.getTime());
        time.setMonth(date.getMonth() + 1);
        time.setDate(0);
        var days = time.getDate() > date.getDate() ? time.getDate() - date.getDate() : 0;
        if (isNaN(cookieValue))
        {
            cookieValue = 1;
        }
        else
        {
            cookieValue++;
        }
        $.cookie("vote", cookieValue, { expires: days });
        $.ajax({
            type: "POST",
            data: Id,
            url: 'Home/Vote/' + Id,
            success: function (result) {
                window.location.href = result.Url
            }
        });
    }
});

$("form").submit(function (e) {
    var valid = false;
    if ($("select").val() == "0") {
        if ($("#Name").val().length > 0 && $("#PurchaseLocations").val().length > 0)
        {
            valid = true;
        }
    }
    else {
        valid = true;
    }
    if(valid){
        var cookieValue = parseInt($.cookie("suggest"));
        if (cookieValue == 1) {
            $(".suggestionMax").removeClass("isHidden")
            e.preventDefault();
        }
        else {
            var date = new Date();
            var time = new Date(date.getTime());
            time.setMonth(date.getMonth() + 1);
            time.setDate(0);
            var days = time.getDate() > date.getDate() ? time.getDate() - date.getDate() : 0;
            $.cookie("suggest", 1, { expires: days });
            $(".inCompleteForm").addClass("isHidden")
            $("form input:submit").click();
        }
    }
    else {
        e.preventDefault();
        $(".inCompleteForm").removeClass("isHidden")
    }
});
