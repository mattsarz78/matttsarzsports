function WireUpEvents() {
    $("#DropDownTimeZone").on("change", function () {
        var option = $('#DropDownTimeZone option:selected').text();
        var optionVal = $('#DropDownTimeZone option:selected').val();
        var sportYear = $('#sportYear').val();
        $.post('/Schedule/DailyText', { timeZoneValue: option, sportYear: sportYear }, function (data) {
            $("#WeekTextGames").html(data);
            $('#DropDownTimeZone option:eq(' + optionVal + ')').prop('selected', true);
            WireUpEvents();
        });
    });

	CommonTextEvents();
}