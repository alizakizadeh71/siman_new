(function ($) {
	if (!$.browser && $.fn.jquery != "1.3.2") {
		$.extend({
			browser: {}
		});
		$.browser.init = function () {
			var a = {};
			try {
				navigator.vendor ?
					/Chrome/.test(navigator.userAgent) ?
						(a.browser = "Chrome", a.version = parseFloat(navigator.userAgent.split("Chrome/")[1].split("Safari")[0])) : /Safari/.test(navigator.userAgent) ? (a.browser = "Safari", a.version = parseFloat(navigator.userAgent.split("Version/")[1].split("Safari")[0])) : /Opera/.test(navigator.userAgent) && (a.Opera = "Safari", a.version = parseFloat(navigator.userAgent.split("Version/")[1])) : /Firefox/.test(navigator.userAgent) ? (a.browser = "mozilla",
							a.version = parseFloat(navigator.userAgent.split("Firefox/")[1])) : (a.browser = "MSIE", /MSIE/.test(navigator.userAgent) ? a.version = parseFloat(navigator.userAgent.split("MSIE")[1]) : a.version = "edge")
			} catch (e) { a = e; }
			$.browser[a.browser.toLowerCase()] = a.browser.toLowerCase();
			$.browser.browser = a.browser;
			$.browser.version = a.version;
			$.browser.chrome = $.browser.browser.toLowerCase() == 'chrome';
			$.browser.safari = $.browser.browser.toLowerCase() == 'safari';
			$.browser.opera = $.browser.browser.toLowerCase() == 'opera';
			$.browser.msie = $.browser.browser.toLowerCase() == 'msie';
			$.browser.mozilla = $.browser.browser.toLowerCase() == 'mozilla';
		};
		$.browser.init();
	}
})(jQuery);

$(document).ready(function () {
	$('#MenuX').kendoMenu();
	$('#MenuX').css('display', '');
	$('#menuHome').kendoMenu();
	docReady();
	$(document).keypress(function (e) {

		var keyCode = e.keyCode ? e.keyCode : e.which;
		var arabicYeCharCode = 1610;
		var persianYeCharCode = 1740;
		var arabicKeCharCode = 1603;
		var persianKeCharCode = 1705;

		if ($.browser.msie) {
			switch (keyCode) {
				case arabicYeCharCode:
					event.keyCode = persianYeCharCode;
					break;
				case arabicKeCharCode:
					event.keyCode = persianKeCharCode;
					break;
			}
		}
		else if ($.browser.mozilla) {
			switch (keyCode) {
				case arabicYeCharCode:
					substituteCharInFireFox(persianYeCharCode, e);
					break;
				case arabicKeCharCode:
					substituteCharInFireFox(persianKeCharCode, e);
					break;
			}
		}
		else {
			switch (keyCode) {
				case arabicYeCharCode:
					insertAtCaret(String.fromCharCode(persianYeCharCode), e);
					break;
				case arabicKeCharCode:
					insertAtCaret(String.fromCharCode(persianKeCharCode), e);
					break;
			}
		}
	});
});

function GetCommonList(pthis) {
	var inputClass = pthis.id;
	$("#" + inputClass + "className").val(pthis[pthis.selectedIndex].innerText);
	var tag = $(pthis);

	var ash_nameLoad = tag.attr("ash_nameLoad");
	var ash_className = tag.attr("ash_className");
	var ash_numberLoad = tag.attr("ash_numberLoad");

	if (ash_nameLoad == null || ash_nameLoad == "" || ash_numberLoad == null || ash_numberLoad == "") {
		return;
	}

	var intPars = parseInt(ash_numberLoad) + 1;

	var pId = ash_className + 'Id';

	var dependFields = $("select[ash_nameLoad='" + ash_nameLoad + "']");

	for (var i = 0; i < dependFields.length; i++) {
		var className = $(dependFields[i]).attr("ash_className");
		if ($(dependFields[i]).attr("ash_numberLoad") == intPars.toString()) {
			var idSelect = dependFields[i].id;

			var dataval = $("#" + idSelect).attr("data-val");
			var requierd = '';
			if (dataval == 'true') {
				requierd = " data-val='true' data-val-required='تکميل فيلد شهر الزامی است!' ";
			}

			jQuery.ajax({
				url: "/Programmers/Common/LoadCommonListForm",
				data: { className: className, keyField: pId, valueField: tag.val() },
				type: 'Post',
				dataType: "json",
				async: false,
				success: function (result) {
					var relCh = '';
					if (result.length > 0) {
						relCh = "chosen";
					}
					var parentNode = $("#" + idSelect)[0].parentNode;
					$(parentNode).empty();
					//$("#" + idSelect + "_chzn").remove();
					//$(city).attr("data-rel", relCh)
					var select = $("<select " + requierd + " ash_nameLoad='" + ash_nameLoad + "' ash_className='" + className +
						"' ash_numberLoad='" + intPars + "' class='form-control' id='" + idSelect +
						"' onchange='GetCommonList(this)' name='" + idSelect + "' data-rel='" + relCh + "'>");

					var option1 = "<option value=''>" + "از لیست انتخاب نمایید" + "</option>";

					$(select).append(option1);

					if (result == null) {
						$('[data-rel="chosen"],[rel="chosen"]').chosen();
						return;
					}

					for (var j = 0; j < result.length; j++) {
						var option = "<option value='" + result[j].Id + "'>" + result[j].Name + "</option>";

						$(select).append(option);
					}
					$(parentNode).append(select);
					var input = '<input type="hidden" id="' + idSelect + 'className" name="' + idSelect + 'className" class="ctrl-commonListClassName form-control" />';
					$(parentNode).append(input);

					$('[data-rel="chosen"],[rel="chosen"]').chosen();
				}, beforeSend: function (e) {
					kendo.ui.progress($("#customerSettingsLoading"), true);
				}
			});

		}
	}
}

function GetCities(pthis) {

	var tag = $(pthis);
	var city = '';

	var same = tag.attr("same");
	$('[same="' + same + '"]').each(function (e) {


		if (this.className == "ctrl-city form-control" || this.className == 'ctrl-city form-control input-validation-error') {
			city = this;
		}
	})

	if (city == '') {
		return;
	}

	jQuery.ajax({
		url: "/Commons/Address/GetCities",
		data: { 'provinceId': tag.val() },
		type: 'Post',
		dataType: "json",
		success: function (result) {
			var relCh = '';
			if (result.length > 0) {
				relCh = "chosen";
			}
			$(city).empty();
			//$(city).attr("data-rel", relCh)
			var select = $("<select class='form-control' id='" + city.id + "' name='" + city.id + "' data-rel='" + relCh + "'>");

			var option1 = "<option value=''>" + "از لیست انتخاب نمایید" + "</option>";

			$(city).append(option1);

			for (var i = 0; i < result.length; i++) {
				var option = "<option value='" + result[i].Id + "'>" + result[i].Name + "</option>";

				$(city).append(option);
			}
			//$("div.@cityId").append(select);

			$('[data-rel="chosen"],[rel="chosen"]').chosen();
		}
	});
}

function popovershow(p) {
	$(p).popover('show');
}

function popoverhide(p) {
	$(p).popover('hide');
}

function substituteCharInFireFox(charCode, e) {
	var keyEvt = document.createEvent("KeyboardEvent");
	keyEvt.initKeyboardEvent("keypress", true, true, null, false, false, false, false, 0, charCode);
	e.target.dispatchEvent(keyEvt);
	e.preventDefault();
}

function substituteCharInChrome(charCode, e) {
	//it does not work yet! /*$.browser.webkit*/
	//https://bugs.webkit.org/show_bug.cgi?id=16735
	var keyEvt = document.createEvent("KeyboardEvent");
	keyEvt.initKeyboardEvent("keypress", true, true, null, false, false, false, false, 0, charCode);
	e.target.dispatchEvent(keyEvt);
	e.preventDefault();
}

function insertAtCaret(myValue, e) {
	var obj = e.target;
	var startPos = obj.selectionStart;
	var endPos = obj.selectionEnd;
	var scrollTop = obj.scrollTop;
	obj.value = obj.value.substring(0, startPos) + myValue + obj.value.substring(endPos, obj.value.length);
	obj.focus();
	obj.selectionStart = startPos + myValue.length;
	obj.selectionEnd = startPos + myValue.length;
	obj.scrollTop = scrollTop;
	e.preventDefault();
}

function docReady() {
	$('a[href="#"][data-top!=true]').click(function (e) {
		e.preventDefault();
	});

	$('.datepicker').datepicker({
		dateFormat: 'yy/mm/dd',
		autoSize: true,
		showyear: true,
		changeYear: true,
		changeMonth: true,
		yearRange: 'c-100:c+100',
		showButtonPanel: true
	});

	$('[data-rel="chosen"],[rel="chosen"]').chosen();

	$('[rel="tooltip"],[data-rel="tooltip"]').tooltip({ "placement": "bottom", delay: { show: 400, hide: 200 } });

	//$('[rel="popover"],[data-rel="popover"]').popover();

}

$(document).ready(function () {
	$('body').find('input').each(function () {
		if (this.type == "number") {
			$(this).val($(this).val().replace("/00", ""))
			$(this).val($(this).val().replace(".00", ""))
			$(this).val($(this).val().replace("/", "."))
		}
	})
	$(document).on("click", ".editDialog", function (e) {
		var pt = $(this);
		var url = $(this).attr('href');
		jQuery.ajax({
			url: url,
			type: 'Get',
			dataType: "html",
			cache: false,
			success: function (result) {
				debugger;
				//$("#popDetails").empty();
				//$("#popDetails").append(result);

				//var _popdet = $('#popDetails').kendoPopup({
				//	anchor: pt,
				//	//toggleTarget: pt
				//});

				//_popdet.data('kendoPopup').open();
				$("#loadUrl").empty();
				$("#loadUrl").append(result);
				var window = $('#loadUrl');
				window.removeAttr('style')
				if (!window.data("kendoWindow")) {
					window.kendoWindow({
						actions: ["Close", "Refresh", "Maximize"],
					});
				}
				window.data("kendoWindow").center();
				window.data("kendoWindow").open();

				//$("#loadUrl").empty();
				//$("#loadUrl").append(result);
				//$('div#myModal').modal({
				//	keyboard: true,
				//	//backdrop: true,
				//	backdrop: 'static'
				//});
			},
			beforeSend: function (e) {
				kendo.ui.progress($("#customerSettingsLoading"), true);
			}, error: function () {
				$("#loadUrl").empty();
				$("#loadUrl").append("<span style='color:red'>مشکل در بازیابی اطلاعات!دوباره امتحان کنید</span>");
			}
		});

		return false;
	});

	$(document).on("click", ".messageDialog", function (e) {
		var pt = $(this);
		var url = $(this).attr('href');

		if (confirm("آیا مطمئن به ادامه عملیات می باشید")) {
			return true;
		}
		return false;
	});

	$(document).on("click", "#btncancel", function (e) {
		$("#dialog-edit").dialog('close');

	});

	$(document).on('mouseenter', '.tooltip', function () {
		$(this).css("display", "none");
		//$(this).css("", "10000111");
	});
	$("#btnFilter").on("click", function (e) {
		var gridData = $("#grid").data("kendoGrid");

		//var currFilterObj = gridData.dataSource.filter();
		//var currentFilters = currFilterObj ? currFilterObj.filters : [];
		var currentFilters = [];

		//for (var i = 0; i < gridData.columns.length; i++) {
		//	var field = $("[name=" + gridData.columns[i].field + "]")
		//	if (field.length > 0) {

		//		if (field.val() == '00000000-0000-0000-0000-000000000000' || field.val() == null || field.val() == '0' || field.val() == 'None' || field.val() == '') {
		//			continue;
		//		}
		//		currentFilters.push({ field: field[0].name, operator: "eq", value: field.val() });
		//	}
		//}

		$('#panelSearch').find('input').each(function () {
			if (this.type != 'hidden' && this.type != 'checkbox' && $(this).val() != null && $(this).val() != '0' && $(this).val() != '') {
				currentFilters.push({ field: this.name, operator: "eq", value: $(this).val() });
			}
		})

		$('#panelSearch').find('select').each(function () {
			if ($(this).val() != '00000000-0000-0000-0000-000000000000' && $(this).val() != null && $(this).val() != '0' && $(this).val() != 'None' && $(this).val() != '') {
				currentFilters.push({ field: this.name, operator: "eq", value: $(this).val() });
			}
		})

		gridData.dataSource.filter({ logic: "and", filters: currentFilters });
		//$("#panelSearch").toggle();
		var window = $('#panelSearch');

		window.data("kendoWindow").close();
	});

	$(document).on("click", ".k-grid-btnSearch", function (e) {
		var pt = $(this);
		$('#panelSearch').show();
		var window = $('#panelSearch');
		if (!window.data("kendoWindow")) {
			window.kendoWindow({
				width: "500px",
				title: "",
				//close: onClose
			});
		}
		window.data("kendoWindow").center();
		window.data("kendoWindow").open();
		//var _popdet = $('#panelSearch').kendoPopup({
		//	anchor: pt,
		//	toggleTarget: pt,
		//	keyboard: true,
		//	backdrop: true,
		//	backdrop: 'static'
		//});

		//_popdet.data('kendoPopup').open();

		return false;
	});
});

//this.$('.js-loading-bar').modal({
//	backdrop: 'static',
//	show: false
//});

$(document).ajaxStart(function () {
	kendo.ui.progress($("#customerSettings"), true);
}).ajaxStop(function () {
	kendo.ui.progress($("#customerSettings"), false);
	kendo.ui.progress($("#customerSettingsLoading"), false);
});


function refresh() {
	var gridData = $("#grid").data("kendoGrid");

	gridData.dataSource.filter({});
}


function togglePanel() {
	$('#panelSearch').toggle()
}
function fn_DateCompare(DateA, DateB) {
	var a = new Date(DateA);
	var b = new Date(DateB);
	var msDateA = Date.UTC(a.getFullYear(), a.getMonth() + 1, a.getDate());
	var msDateB = Date.UTC(b.getFullYear(), b.getMonth() + 1, b.getDate());
	//if (parseFloat(msDateA) < parseFloat(msDateB))
	//   return "تاریخ تولد از تاریخ صدور کوچکتر است";  // less than
	if (parseFloat(msDateA) == parseFloat(msDateB))
		return true;  // equal
	else if (parseFloat(msDateA) > parseFloat(msDateB))
		return true;  // greater than
	else
		return false;  // error
}
function getActionFilter(formId, dossierId, requestId, formType, actionFormType) {

	if (dossierId == null || dossierId == '') {
		return true;
	}

	var tag = false;
	jQuery.ajax({
		url: '/Programmers/Common/GetActionFilter',
		data: {
			formId: formId,
			dossierId: dossierId,
			requestId: requestId,
			formType: formType,
			controllerName: formType,
			actionFormType: actionFormType
		},
		type: 'Post',
		async: false,
		//dataType: "html",
		cache: false,
		success: function (result) {
			tag = result;
		},
	});

	return tag;
}

function Isvalid() {
	var validator = $("form").validate(); // get validator
	var valid = true;
	$("form").find("input").each(function () {
		if (!validator.element(this)) { // validate every input element inside this step
			valid = false;
		}
	});

	$("form").find("select").each(function () {
		if (!validator.element(this)) {
			valid = false;
		}
	});

	return valid;
}