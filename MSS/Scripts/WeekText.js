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

	CommonTextEvents();
}