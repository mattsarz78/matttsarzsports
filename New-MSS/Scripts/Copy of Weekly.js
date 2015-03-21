$(document).ready(function () {
    $("#DropDownTimeZone").on("change", function () {
        $('#WeekForm').submit();
    });

    $('.show_hideEligibility').on("click", function () {
        $(".slidingBowlDiv").slideToggle("fast", function () {
            if ($('.slidingBowlDiv').css("display") == "none") {
                $('#btnEligibility').attr("value", "Show Bowl Eligibility");
            }
            else {
                $('#btnEligibility').attr("value", "Hide Bowl Eligibility");
            }
        });
    });

    if (window.innerWidth < 641) {
        $("#Partial").attr("style", "display:inline-block;width:125px;height:125px");
        $("#Partial").attr("data-ad-slot", "1186564671");
    }
    else {
        $("#Partial").attr("style", "display:inline-block;width:234px;height:60px");
        $("#Partial").attr("data-ad-slot", "0805896429");
    }

    $("#RSNLists").children().hide();

    $('.show_hideNoTV').on("click", function () {
        $(".slidingNoTVDiv").slideToggle(400, function () {
            if ($(".slidingNoTVDiv").css("display") == "none") {
                $('#btnConferenceGames').attr("value", "Show Non-Televised Games");
            }
            else {
                $('#btnConferenceGames').attr("value", "Hide Non-Televised Games");
            }
        });
    });

    $('.show_hideWeb').on("click", function () {
        $(".webGame").slideToggle(400, function () {
            if ($(".webGame").css("display") == "none") {
                $('#btnWebGames').attr("value", "Show Web Exclusive Games");
            }
            else {
                $('#btnWebGames').attr("value", "Hide Web Exclusive Games");
            }
        });
        $(".slidingWebDiv").slideToggle(400, function () {
            if ($(".slidingWebDiv").css("display") == "none") {
                $('#btnWebGames').attr("value", "Show Web Exclusive Games");
            }
            else {
                $('#btnWebGames').attr("value", "Hide Web Exclusive Games");
            }
        });

    });

    $('.FSNLink').on("click", function () {
        var divLink = $(this).attr("class");
        divLink = divLink.replace("FSNLink ", "");
        $('.overlay').height($(document).height());
        $('.overlay').show();
        $('#RSNLists').show();
        $('.closer').show();
        $('#' + divLink).show();
        $('#GooglePartialAd').show();
        $(window).scrollTop(0, 0);
    });

    $('.closer').on("click", function () {
        $('#RSNLists').hide();
        $(".overlay").hide();
        $('#RSNLists').children().hide();
    });

    $('body').bind('contextmenu', function () {
        alert("Please link directly to this page instead of stealing the code. Some of the links won't work. Thank you.");
        return false;
    });
});

