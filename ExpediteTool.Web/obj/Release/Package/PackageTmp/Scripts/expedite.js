$(document).ready(function () {
    $('#popupAddData').on('shown.bs.modal', function () {
        var lotId = $('#txtLotId');
        lotId.focus();
        lotId.select();
    });
    $('#modalUpdateComment').on('shown.bs.modal', function () {
        $('#txtUpdateComment').focus();
        $('#txtUpdateComment').select();
        $('#validateLengthComment').attr('style', 'display:none');
    });
    $('#modalUpdateComment').on('hide.bs.modal', function () {
        $('#validateLengthComment').attr('style', 'display:none');
        $('#validateLengthComment').text('');
    });

    $('input[type="file"]').change(function () {
        $(this).next().removeAttr('disabled');
    }).next().attr('disabled', 'disabled');

    var optionCalendar =
        {
            showOn: 'both',
            autoSize: true,
            constrainInput: true,
            buttonImageOnly: true,
            buttonImage: "../Images/calendar.png",
            buttonText: "Calendar",
            dateFormat: 'mm/dd/yy',
            minDate: -0,
        };

    $('#cboBu').selectedIndex = 0;
    $('#dpkRequestOutDate').datepicker(optionCalendar);

    $("[id*=imgOrdersShow]").each(function () {
        if ($(this)[0].src.indexOf("minus") != -1) {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
            $(this).next().remove();
        }
    });
    $("[id*=imgProductsShow]").each(function () {
        if ($(this)[0].src.indexOf("minus") != -1) {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
            $(this).next().remove();
        }
    });
    $('#pnWaitingProcess').dialog(
            {
                autoOpen: false,
                dialogClass: "loadingScreenWindow",
                closeOnEscape: false,
                draggable: false,
                width: 260,
                minHeight: 30,
                modal: true,
                buttons: {},
                resizable: false,
                open: function () {
                    // scrollbar fix for IE
                    $('body').css('overflow', 'hidden');
                },
                close: function () {
                    // reset overflow
                    $('body').css('overflow', 'auto');
                }
            });
    var text = $('#txtSearchAll').val();
    if (text != '') {
        $('#btnSearchAll').removeAttr('disabled');
    }
    else {
        $('#btnSearchAll').attr('disabled', 'disabled');
    }
    $('#txtSearchAll').keyup(function (event) {
        if ($(this).val() != '') {
            $('#btnSearchAll').removeAttr('disabled');
        }
        else {
            $('#btnSearchAll').attr('disabled', 'disabled');
        }
    });
});
function onKeySearch(id) {
    var $this = $('#txtSearchBu' + id);
    var trSearch = $this.parent().//td
          parent().//tr
          parent().//tbody
          parent().//table
          parent().//td
          parent()//tr[class=row]
    var trGridView = trSearch.closest('tr').next().next();
    var grdChildActived = trGridView.find('td').children('div').children('div').children('div').children('table');
    searchTable($this.val(), grdChildActived);
};
function searchTable(inputVal, panel) {
    var table = panel;
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found == true)
                $(row).show();
            else
                $(row).hide();
        }
    });
};
function resetControlPopup() {
    $('#hdnHoLotId').val(0);
    $('#txtLotId').val('');
    $('#txtReason').val('');
    $('#dpkRequestOutDate').val('');
    $('#cboStatus').val(0);
    $('#txtOwner').val('');
    $('#txtComment').val('');
    $('#txtPlatform').val('');
    $('#txtCurrentOperation').val('');
    $('#txtDevice').val('');
}
function bindDataPopup(id, lotId, reason, requestOutDate, bu, owner, comment, platform, currentOperation, device) {
    $('#hdnHoLotId').val(id);
    $('#txtLotId').val(lotId);
    $('#txtReason').val(reason);
    $('#dpkRequestOutDate').val(requestOutDate);
    $('#cboBu').val(bu);
    $('#txtOwner').val(owner);
    $('#txtComment').val(comment);
    $('#txtPlatform').val(platform);
    $('#txtCurrentOperation').val(currentOperation);
    $('#txtDevice').val(device);
    //showPopup
    var options = {
        show: true,
        backdrop: false
    };
    $('#popupAddData').modal(options);
    return true;
}
function checkSave() {
    var result = true;
    var firstControl = null;
    var message = "";
    var txtLotId = document.getElementById('txtLotId');
    if (txtLotId != null) {
        if (jQuery.trim(txtLotId.value) == "") {
            result = false;
            message = "The LotId field is required.\n";
            if (firstControl == null)
                firstControl = txtLotId;
        }
        else
            if (jQuery.trim(txtLotId.value).length > 255) {
                result = false;
                message = "The LotId field max length is 50 chars.\n";
                if (firstControl == null)
                    firstControl = txtLotId;
            }
    }

    var txtReason = document.getElementById('txtReason');
    if (txtReason != null) {
        if (jQuery.trim(txtReason.value) == "") {
            message += "The Reason field is required.\n";
            if (firstControl == null)
                firstControl = txtReason;
            result = false;
        }
        else
            if (jQuery.trim(txtReason.value).length > 255) {
                message += "The Reason field max length is 255 chars.\n";
                if (firstControl == null)
                    firstControl = txtReason;
                result = false;
            }
    }

    var dpkRequestOutDate = document.getElementById('dpkRequestOutDate');
    if (dpkRequestOutDate != null) {
        if (jQuery.trim(dpkRequestOutDate.value) == "") {
            message += "The Request Out Date field is required.\n";
            if (firstControl == null)
                firstControl = dpkRequestOutDate;
            result = false;
        }
    }

    var txtOwner = document.getElementById('txtOwner');
    if (txtOwner != null) {
        if (jQuery.trim(txtOwner.value) == "") {
            message += "The Owner field is required.\n";
            if (firstControl == null)
                firstControl = txtOwner;
            result = false;
        }
        else
            if (jQuery.trim(txtOwner.value).length > 50) {
                message += "The Owner field max length is 50 chars.\n";
                if (firstControl == null)
                    firstControl = txtOwner;
                result = false;
            }
    }

    var txtComment = document.getElementById('txtComment');
    if (txtComment != null) {
        if (jQuery.trim(txtComment.value).length > 100) {
            message += "The Comment field max length is 100 chars.\n";
            if (firstControl == null)
                firstControl = txtComment;
            result = false;
        }
    }

    var txtPlatform = document.getElementById('txtPlatform');
    if (txtPlatform != null) {
        if (jQuery.trim(txtPlatform.value).length > 40) {
            message += "The Platform field max length is 40 chars.\n";
            if (firstControl == null)
                firstControl = txtPlatform;
            result = false;
        }
    }

    var txtCurrentOperation = document.getElementById('txtCurrentOperation');
    if (txtCurrentOperation != null) {
        if (jQuery.trim(txtCurrentOperation.value) == "") {
            message += "The Current Operation field is required.\n";
            if (firstControl == null)
                firstControl = txtCurrentOperation;
            result = false;
        }
        else
            if (jQuery.trim(txtCurrentOperation.value).length > 255) {
                message += "The Current Operation field max length is 255 chars.\n";
                if (firstControl == null)
                    firstControl = txtCurrentOperation;
                result = false;
            }
    }

    var txtDevice = document.getElementById('txtDevice');
    if (txtDevice != null) {
        if (jQuery.trim(txtDevice.value) == "") {
            message += "The Device field is required.\n";
            if (firstControl == null)
                firstControl = txtDevice;
            result = false;
        }
        else
            if (jQuery.trim(txtDevice.value).length > 50) {
                message += "The Device field max length is 50 chars.\n";
                if (firstControl == null)
                    firstControl = txtDevice;
                result = false;
            }
    }

    if (firstControl != null)
        firstControl.focus();
    if (result === false) {
        alert(message);
    }

    return result;
}
function showPopup() {
    //reset popup
    resetControlPopup();
    var options = {
        show: true,
        backdrop: false
    };
    $('#popupAddData').modal(options);
}
function hidePopup() {
    var options = {
        show: false,
    };
    $('#popupAddData').modal('hide');
    resetControlPopup();
}
function addDataSuccess() {
    hidePopup();
    $.toaster({
        priority: 'success',
        title: 'Success',
        timer: 3000,
        message: 'Add HotLots success'
    });
}
function updateDataSuccess() {
    hidePopup();
    $.toaster({
        priority: 'success',
        title: 'Success',
        timer: 3000,
        message: 'Update HotLots success'
    });
}
function addDataFail() {
    $.toaster({
        priority: 'danger',
        title: 'Fail',
        timer: 3000,
        message: 'Add HotLots fail'
    });
}
function updateDataFail() {
    $.toaster({
        priority: 'danger',
        title: 'Fail',
        timer: 3000,
        message: 'Update HotLots fail'
    });
}
function confirmDelete() {
    return confirm('Are you sure to delete lot?');
}
function confirmCloseStatus() {
    return confirm('Are you sure to close lot?');
}
function confirmPendingStatus() {
    return confirm('Are you sure to pending this lot?');
}
function confirmActiveStatus() {
    return confirm('Are you sure to make active this lot?');
}
function waitingDialog() {
    $("#pnWaitingProcess").dialog('option', 'title', 'Exporting to Excel...');
    $("#pnWaitingProcess").dialog('open');
}
function watingImportDialog() {
    $("#pnWaitingProcess").dialog('option', 'title', 'Import Lots...');
    $("#pnWaitingProcess").dialog('open');
    $('#panelImportExcel').hide();
}
function closeWaitingDialog() {
    $("loadingScreenWindow").dialog('close');
}
function closeImportDialog(type, numberRecord) {
    //Close dialog processing
    $("loadingScreenWindow").dialog('close');
    /*0:SUCCESS; 1:FAILURE; 2:DUPLICATE*/
    switch (type) {
        case 0:
            $('#txtNumberRecord').text(numberRecord);
            $('#modalPopupSuccess').modal({ show: true, backdrop: false });
            break;
        case 1:
            $('#modalPopupFailure').modal({ show: true, backdrop: false });
            break;
        case 2:
            $('#txtDuplicationLot').text(numberRecord);
            $('#modalPopupDuplicate').modal({ show: true, backdrop: false });
            break;
    }
}

function reloadPage() {
    $('#modalPopupSuccess').modal({ show: true, backdrop: false })
}

function getfile(filename) {
    document.location.href = "../DownloadFile.ashx?filename=" + filename; //+ "&urlback="+url;
    $("#pnWaitingProcess").dialog('close');
}
function setModalUpdateComment(id, comment) {
    $('#hdfHotLotDataId').val(id);
    $('#txtUpdateComment').val(comment);
}

var isSubmitted = false;
var inited = false;

function checkLength(txtUpdateComment) {
    if (!isSubmitted) {
        var item = $('#' + txtUpdateComment)
        if (item != null && item.val().trim().trimStart().trimEnd().length <= 100) {
            isSubmitted = true;
            return true;
        }
        else {
            var validate = $('#validateLengthComment')
            validate.removeAttr('style');
            validate.css('color', 'red');
            validate.css('padding-left', '15px');
            validate.text('Max length of Comment is 100 chars');
            return false;
        }
    }
    else
        return false;
}
