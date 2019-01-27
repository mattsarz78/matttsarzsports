$(document).ready(function () {
	$("#Main").css("padding-top", $(".navbar").height() + 5);
	WireUpEvents();
});

function CommonTextEvents() {
	$(".checkBoxRow").on("click", function() {
		if ($(this).is(":checked")) {
            $(this).parents("tr").css("background-color", "#CCC").removeClass("DONTPrint").addClass("DOPrint");
		    $(this).prop("checked", true);
		} else {
            $(this).parents("tr").css("background-color", "#FFF").removeClass("DOPrint").addClass("DONTPrint");
			$(this).prop("checked", false);
        }
	});

	$("#ClearAll").on("click", function () {
	    $(".checkBoxRow").each(function () {
	        $(this).prop("checked", false);
	    });
		$("tr.gamerow").css("background-color", "#FFF").addClass("DONTPrint").removeClass("DOPrint");
	});

	$("#CheckAll").on("click", function() {
	    $(".checkBoxRow").each(function () {
	        $(this).prop("checked", true);
	    });
		$("tr.gamerow").css("background-color", "#CCC").addClass("DOPrint").removeClass("DONTPrint");
	});
}
