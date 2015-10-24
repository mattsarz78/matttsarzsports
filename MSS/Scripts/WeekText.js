$(document).ready(function () {
    WireUpEvents();
});

function WireUpEvents() {
    $("#DropDownTimeZone").on("change", function () {
        var option = $('#DropDownTimeZone option:selected').text();
        var optionVal = $('#DropDownTimeZone option:selected').val();
        var sportYear = $('#sportYear').val();
        var week = $('#week').val();
        $.post('/Schedule/WeeklyText', { timeZoneValue: option, week: week, sportYear: sportYear }, function (data) {
            $("#WeekTextGames").html(data);
            $('#DropDownTimeZone option:eq(' + optionVal + ')').prop('selected', true);
            WireUpEvents();
        });
    });

    $(".checkBoxRow").on("click", function () {
        if ($(this).is(":checked")) {
            $(this).parents("tr").css("background-color", "#CCC").removeClass("DONTPrint").addClass("DOPrint");
        }
        else {
            $(this).parents("tr").css("background-color", "#FFF").addClass("DONTPrint").removeClass("DOPrint");
        }
    });

    $("#ClearAll").on("click", function () {
        $(".checkBoxRow").attr("checked", false);
        $("tr").css("background-color", "#FFF").addClass("DONTPrint").removeClass("DOPrint");
    });

    $("#CheckAll").on("click", function () {
        $(".checkBoxRow").attr("checked", true);
        $("tr").css("background-color", "#CCC").addClass("DOPrint").removeClass("DONTPrint");
    });

}