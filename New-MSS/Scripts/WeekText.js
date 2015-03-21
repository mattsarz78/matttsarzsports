$(document).ready(function () {
    $("#DropDownTimeZone").on("change", function () {
        $('#TextForm').submit();
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

});