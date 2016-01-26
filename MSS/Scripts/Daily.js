function WireUpEvents() {
    $("#DropDownTimeZone").on("change", function () {
        var option = $('#DropDownTimeZone option:selected').text();
        var optionVal = $('#DropDownTimeZone option:selected').val();
        var sportYear = $('#sportYear').val();
        $.post('/Schedule/Daily', { timeZoneValue: option, sportYear: sportYear }, function (data) {
            $("#WeeksBase").html(data);
            $('#DropDownTimeZone option:eq(' + optionVal + ')').prop('selected', true);
            UnbindEvents();
            WireUpEvents();
        });
    });

	CommonScheduleEvents();
}





 