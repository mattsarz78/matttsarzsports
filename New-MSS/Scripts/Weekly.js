$(document).ready(function () {
	$("#DropDownTimeZone").on("change", function () {
		$("#WeekForm").submit();
	});

	$("#RSNLists").children().hide();

	$(".show_hideNoTV").on("click", function () {
		$(".slidingNoTVDiv").slideToggle(400, function () {
			showHideButtonText($(".slidingNoTVDiv"), $("#btnConferenceGames"), "Non-Televised Games");
		});
	});

	$(".show_hideWeb").on("click", function () {
		$(".webGame").slideToggle(400, function () {
			showHideButtonText($(".webGame"), $("#btnWebGames"), "Web Exclusive Games");
		});
		$(".slidingWebDiv").slideToggle(400, function () {
			showHideButtonText($(".slidingWebDiv"), $("#btnWebGames"), "Web Exclusive Games");
		});
	});

	$(".FSNLink").on("click", function () {
		var e = $(this).attr("class").replace("FSNLink ", "");
		var rowPosition;
		if ($(this).parents('.coverage').get(0) != undefined) {
			rowPosition = $(this).parents('.coverage').parents('tr').position().top;
		} else {
			rowPosition = $(this).parents('.coverageppv').parents('tr').position().top;
		}
		if (rowPosition / $(document).height() <= .95) {
			$(window).scrollTop(rowPosition - ($(window).height() / 2));
			showRSNItens(e);
			$("#RSNLists").css("margin-top", rowPosition - ($('#RSNLists').height()));
		} else {
			showRSNItens(e);
			$("#RSNLists").css("margin-top", rowPosition - ($('#RSNLists').height() * 1.2));
		}
		$("#GooglePartialAd").show();
	});

	$(".closer").on("click", function () {
		$("#RSNLists,.overlay").hide();
		$("#RSNLists").children().hide();
	});
});

function showRSNItens(e) {
	$(".overlay").height($(document).height()).show();
	$("#RSNLists,.closer").show();
	$("#" + e).show();
}

function showHideButtonText(e, f, g) {
	if (e.css("display") == "none") {
		f.attr("value", "Show " + g);
	} else {
		f.attr("value", "Hide " + g);
	}
}



 