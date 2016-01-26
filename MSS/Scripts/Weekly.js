function WireUpEvents() {
    $("#DropDownTimeZone").on("change", function () {
        var option = $('#DropDownTimeZone option:selected').text();
        var optionVal = $('#DropDownTimeZone option:selected').val();
        var sportYear = $('#sportYear').val();
        var week = $('#week').val();
        $.post('/Schedule/Weekly', { timeZoneValue: option, week: week, sportYear: sportYear }, function (data) {
            $("#WeeksBase").html(data);
            $('#DropDownTimeZone option:eq(' + optionVal + ')').prop('selected', true);
            UnbindEvents();
            WireUpEvents();
        });
    });

    CommonScheduleEvents();

    $("#btnConferenceGames").on("click", function () {
        $(".slidingNoTVDiv").slideToggle(400, function () {
            showHideButtonText($(".slidingNoTVDiv"), $("#btnConferenceGames"), "Non-Televised Games");
        });
    });
}





 