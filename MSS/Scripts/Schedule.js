$(document).ready(function () {
    $("#Main").css("padding-top", $(".navbar").height() + 5);
    WireUpEvents();
});

function showRSNItens(e) {
    $(".overlay").height($(document).height()).show();
    $("#RSNLists,.closer").show();
    $("#" + e).show();
    $(window).scrollTop($('html').offset().top);
}

function showHideButtonText(e, f, g) {
    if (e.css("display") == "none") {
        f.attr("value", "Show " + g);
    } else {
        f.attr("value", "Hide " + g);
    }
}

function UnbindEvents() {
    $("#DropDownTimeZone").unbind("change");
    $(".FSNLink,.closer,#btnWebGames,#btnConferenceGames").unbind("click");
}

function CommonScheduleEvents() {
    $("#RSNLists").children().hide();

    $(".FSNLink").on("click", function () {
        var e = $(this).attr("class").replace("FSNLink ", "");
        showRSNItens(e)
        $("#GooglePartialAd").show();
    });

    $(".closer").on("click", function () {
        $("#RSNLists,.overlay").hide();
        $("#RSNLists").children().hide();
    });

    $("#btnWebGames").on("click", function (event) {
        $(".webGame").slideToggle(400, function () {
            showHideButtonText($(".webGame"), $("#btnWebGames"), "Web Exclusive Games");
        });
        $(".slidingWebDiv").slideToggle(400, function () {
            showHideButtonText($(".slidingWebDiv"), $("#btnWebGames"), "Web Exclusive Games");
        });
    });
}





 