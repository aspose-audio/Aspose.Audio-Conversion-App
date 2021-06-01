const VIEWABLE_EXTENSIONS = [
    'VSD', 'VSDX', 'VSX', 'VTX', 'VDX', 'VSSX', 'VSTX', 'VSDM', 'VSSM', 'VSTM'
];

const FILE_STATE_INIT = 0;
const FILE_STATE_SET = 1;
const FILE_STATE_UPLOADING = 2;
const FILE_STATE_PROCESSING = 3;
const FILE_STATE_DONE = 4;
const FILE_STATE_FAIL = 5;

const FILE_TYPE_FILE = 0;
const FILE_TYPE_URL = 1;

const AUDIO_CODECS = {
    'aac': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'AAC (Advanced Audio Coding)',
            'value': 'aac'
        }
    ],
    'aiff': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'PCM A-law, G.711(Uncompressed)',
            'value': 'pcm_alaw'
        },
        {
            'name': 'PCM mu-law, G.711(Uncompressed)',
            'value': 'pcm_mulaw'
        },
        {
            'name': 'PCM signed 16-bit little-endian(Uncompressed)',
            'value': 'pcm_s16le'
        },
        {
            'name': 'PCM signed 16-bit big-endian(Uncompressed)',
            'value': 'pcm_s16be'
        }
    ],   
    'flac': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'FLAC (Free Lossless Audio Codec)',
            'value': 'flac'
        }
    ], 
    'm4a': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'AAC (Advanced Audio Coding)',
            'value': 'aac'
        }
    ],
    'mp3': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'MP3 (MPEG audio layer 3)',
            'value': 'libmp3lame'
        }
    ],
    'wav': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'PCM A-law, G.711(Uncompressed)',
            'value': 'pcm_alaw'
        },
        {
            'name': 'PCM mu-law, G.711(Uncompressed)',
            'value': 'pcm_mulaw'
        },
        {
            'name': 'PCM signed 16-bit little-endian(Uncompressed)',
            'value': 'pcm_s16le'
        },
        {
            'name': 'PCM signed 16-bit big-endian(Uncompressed)',
            'value': 'pcm_s16be'
        }
    ], 
    'wma': [
        {
            'name': 'Copy',
            'value': 'copy'
        },
        {
            'name': 'Windows Media Audio 1',
            'value': 'wmav1'
        }
        ,
        {
            'name': 'Windows Media Audio 2',
            'value': 'wmav2'
        }
    ],
    'ac3': [
        {
            'name': 'AC3(Dolby AC-3)',
            'value': 'ac3'
        }
    ],
    'amr': [
        {
            'name': 'AMR NarrowBand',
            'value': 'libopencore_amrnb'
        },
        {
            'name': 'AMR WideBand',
            'value': 'libvo_amrwbenc'
        }
    ],
    'caf': [
        {
            'name': 'PCM A-law, G.711(Uncompressed)',
            'value': 'pcm_alaw'
        },
        {
            'name': 'PCM mu-law, G.711(Uncompressed)',
            'value': 'pcm_mulaw'
        },
        {
            'name': 'PCM signed 16-bit little-endian(Uncompressed)',
            'value': 'pcm_s16le'
        },
        {
            'name': 'PCM signed 16-bit big-endian(Uncompressed)',
            'value': 'pcm_s16be'
        },
        {
            'name': 'AC3(Dolby AC-3)',
            'value': 'ac3'
        },
        {
            'name': 'MP3 (MPEG audio layer 3)',
            'value': 'libmp3lame'
        }
    ], 
    'ogg': [
        {
            'name': 'Ogg Vorbis',
            'value': 'libvorbis'
        }
    ],
}

// filedrop component
var fileDrop = {};
var fileDrop2 = {};

$.extend($.expr[':'], {
    isEmpty: function (e) {
        return e.value === '';
    }
});

var dataList = [];

var localData = { page: 1, total: 0, records: "0", rows: dataList };
localData.rows = dataList;
localData.records = dataList.length;
localData.total = (dataList.length % 5 == 0) ? (dataList.length / 5) : (Math.floor(dataList.length / 5) + 1);
var reader = {
    root: function (obj) {
        return localData.rows;
    },
    page: function (obj) {
        return localData.page;
    },
    total: function (obj) {
        return localData.total;
    },
    records: function (obj) {
        return localData.records;
    },
    repeatitems: false
};

// Restricts input for the set of matched elements to the given inputFilter function.
(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
    //$(window).resize(function(){
    //    $("#rawFileTable").setGridWidth(document.body.clientWidth * 0.8);
    //});
}(jQuery));


function clickUpLoad() {
    document.getElementById("file" + curFileIndex).click();
}

var curFileIndex = 1;

function fileSelected() {
    //var fileObj = $('#file' + curFileIndex)[0];
    var fileObj = document.getElementById('file' + curFileIndex).files[0];
    var gridItem = {};
    //gridItem.filePath = getFileName(fileObj.value);
    gridItem.filePath = fileObj.name;
    gridItem.fileObj = fileObj;
    gridItem.fileType = FILE_TYPE_FILE;
    
    if (o.DefaultAudioFormat) {
        gridItem.audioFormat = o.DefaultAudioFormat.toUpperCase();
        gridItem.state = FILE_STATE_SET;
    } else {
        gridItem.state = FILE_STATE_INIT;
    }
    dataList.push(gridItem);
    curFileIndex++;
    $('#fileDiv').append('<input id="file' + curFileIndex + '" type="file" style="display: none;" accept="*" onchange="fileSelected()">');
    $('#rawFileTable').jsGrid("refresh");
    $('#uploadDiv').removeClass('hidden');
}

function getFileName(path) {
    var pos1 = path.lastIndexOf('/');
    var pos2 = path.lastIndexOf('\\');
    var pos = Math.max(pos1, pos2)
    if (pos < 0)
        return path;
    else
        return path.substring(pos + 1);
}

function AudioFormatSelect(rowId, audioFormat) {
    var rowObject = dataList[rowId-1];
    rowObject.audioFormat = audioFormat;
    rowObject.state = FILE_STATE_SET;
    $("#rawFileTable").jsGrid("refresh");
}

function removeItem(rowId) {
    dataList.splice(rowId - 1, 1);
    //$("#rawFileTable").clearGridData();
    $("#rawFileTable").jsGrid("refresh");
    if (dataList.length == 0) {
        $('#uploadDiv').addClass('hidden');
    }
}

var curAudioSettingIndex = -1;


function initAddFileDialog() {
    $('#addfileUrl').val('');
}

function openAddFileDialogModel() {
    initAddFileDialog();
    $('#addFileDialog').modal('show');
}

function urlInputed() {
    var url = $('#addfileUrl').val();
    if (!fIsUrL(url)) {
        alert('Please enter the correct format URL.');
        return false;
    }
    var gridItem = {};
    gridItem.filePath = getFileNameByUrl(url);
    gridItem.fileObj = url;
    gridItem.fileType = FILE_TYPE_URL;
    
    if (o.DefaultAudioFormat) {
        gridItem.audioFormat = o.DefaultAudioFormat.toUpperCase();
        gridItem.state = FILE_STATE_SET;
    } else {
        gridItem.state = FILE_STATE_INIT;
    }
    dataList.push(gridItem);
    $('#rawFileTable').jsGrid("refresh");
    $('#uploadDiv').removeClass('hidden');
    $('#addFileDialog').modal('hide');
}

function getFileNameByUrl(path) {
    var pos = path.lastIndexOf('/');
    if (pos < 0 || pos == path.length-1)
        return path;
    else
        return path.substring(pos + 1);
}

function fIsUrL(sUrl) {
    var sRegex = '^((https|http)?://)' + '?(([0-9a-z_!~*\'().&=+$%-]+: )?[0-9a-z_!~*\'().&=+$%-]+@)?' //ftp的user@ 
        + '(([0-9]{1,3}.){3}[0-9]{1,3}'
        + '|'
        + '([0-9a-z_!~*\'()-]+.)*'
        + '([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].'
        + '[a-z]{2,6})'
        + '(:[0-9]{1,4})?'
        + '((/?)|'
        + '(/[0-9a-z_!~*\'().;?:@&=+$,%#-]+)+/?)$';
    var re = new RegExp(sRegex);
    if (re.test(sUrl)) {
        return true;
    }
    return false;
}

function initAudioSettingModel() {
    $('#bitrateInput').val('');
    $('#volumeSelect').val('0');
    $('#startTime').val('');
    $('#endTime').val('');
    $('#vbrSelect').val('-1');
    if (!$('#VBRSelectDiv').hasClass('hidden')) {
        $('#VBRSelectDiv').addClass('hidden');
    }
    if ($('#bitrateDiv').hasClass('hidden')) {
        $('#bitrateDiv').removeClass('hidden');
    }
}
function openAudioSettingModel(rowId) {
    initAudioSettingModel();
    curAudioSettingIndex = rowId - 1;
    var rowObject = dataList[rowId - 1];

    $('#audioCodecSelect').empty();
    var curCodecs = AUDIO_CODECS[rowObject.audioFormat.toLowerCase()];
    if (curCodecs) {
        curCodecs.forEach((item, index)=>{
            $('#audioCodecSelect').append('<option value="' + item['value'] + '">' + item['name'] + '</option>');
        });
    }
    if (rowObject.AdjustVolumn) {
        $('#volumeSelect').val(rowObject.AdjustVolumn);
    }
    if (rowObject.AudioBitRate) {
        $('#bitrateInput').val(rowObject.VideoBitRate);
    }
    if (rowObject.AudioCodec) {
        $('#audioCodecSelect').val(rowObject.AudioCodec);
    }

    if (rowObject.audioFormat.toLowerCase() == 'mp3') {
        if ($('#VBRSelectDiv').hasClass('hidden')) {
            $('#VBRSelectDiv').removeClass('hidden');
        }
        if (rowObject.AudioQuality) {
            $('#vbrSelect').val(rowObject.AudioQuality);
            if (!$('#bitrateDiv').hasClass('hidden')) {
                $('#bitrateDiv').addClass('hidden');
            }
        }
        else {
            if ($('#bitrateDiv').hasClass('hidden')) {
                $('#bitrateDiv').removeClass('hidden');
            }
        }
    }

    if (rowObject.StartTime) {
        $('#startTime').val(rowObject.StartTime);
    }
    if (rowObject.EndTime) {
        $('#endTime').val(rowObject.EndTime);
    }
    $('#audioSetting').modal('show');
}

function validateAudioSetting(){
    var bitrateSetting = $('#bitrateInput').val();
    if (bitrateSetting && !(/(^[0-9]*[1-9][0-9]*$)/.test(bitrateSetting))) {
        alert('Audio bitrate should be positive integer.');
        return false;
    }
    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    var startHour, startMinute, startSecond;
    var endHour, endMinute, endSecond;
    if (startTime) {
        var dateArr = startTime.split(':');
        if (dateArr.length != 3 || dateArr[0].length != 2 || dateArr[1].length != 2 || dateArr[2].length != 2) {
            alert('Start time has incorrect format.');
            return false;
        }
        startHour = parseInt(dateArr[0]);
        if (isNaN(startHour) || startHour < 0) {
            alert('Start time has incorrect format.');
            return false;
        }
        startMinute = parseInt(dateArr[1]);
        if (isNaN(startMinute) || startHour > 59 || startHour < 0) {
            alert('Start time has incorrect format.');
            return false;
        }
        startSecond = parseInt(dateArr[2]);
        if (isNaN(startSecond) || startSecond > 59 || startSecond < 0) {
            alert('Start time has incorrect format.');
            return false;
        }
    }
    if (endTime) {
        var dateArr = endTime.split(':');
        if (dateArr.length != 3 || dateArr[0].length != 2 || dateArr[1].length != 2 || dateArr[2].length != 2) {
            alert('End time has incorrect format.');
            return false;
        }
        endHour = parseInt(dateArr[0]);
        if (isNaN(endHour) || endHour < 0) {
            alert('End time has incorrect format.');
            return false;
        }
        endMinute = parseInt(dateArr[1]);
        if (isNaN(endMinute) || endMinute > 59 || endMinute < 0) {
            alert('End time has incorrect format.');
            return false;
        }
        endSecond = parseInt(dateArr[2]);
        if (isNaN(endSecond) || endSecond > 59 || endSecond < 0) {
            alert('End time has incorrect format.');
            return false;
        }
    }
    if (startTime && endTime) {
        if (startHour > endHour) {
            alert('The end time should be greater than the start time.');
            return false;
        }
        else if (startHour == endHour) {
            if (startMinute > endMinute) {
                alert('The end time should be greater than the start time.');
                return false;
            }
            else if (startMinute == endMinute) {
                if (startSecond >= endSecond) {
                    alert('The end time should be greater than the start time.');
                    return false;
                }
            }
        }
    }
    return true;
}

function VBRSelectChange() {
    var vbrSelect = $('#vbrSelect').val();
    if (vbrSelect == '-1') {
        if ($('#bitrateDiv').hasClass('hidden')) {
            $('#bitrateDiv').removeClass('hidden');
        }
    }
    else {
        if (!$('#bitrateDiv').hasClass('hidden')) {
            $('#bitrateDiv').addClass('hidden');
        }
    }
}

function saveAudioSetting() {
    if (!validateAudioSetting()) {
        return;
    }
    var rowObject = dataList[curAudioSettingIndex];

    var bitrateSetting = $('#bitrateInput').val();
    if (bitrateSetting) {
        rowObject.AudioBitRate = bitrateSetting;
    }

    if (rowObject.audioFormat.toLowerCase() == 'mp3') {
        var vbrSelectSetting = $('#vbrSelect').val();
        if (vbrSelectSetting != '-1') {
            rowObject.AudioQuality = vbrSelectSetting;
        }
        else {
            if (rowObject.AudioQuality) {
                delete rowObject.AudioQuality;
            }
        }
    }

    var volumnSetting = $('#volumeSelect').val();
    if (volumnSetting != '0') {
        rowObject.AdjustVolumn = volumnSetting;
    }

    var codecSetting = $('#audioCodecSelect').val();
    if (codecSetting) {
        rowObject.AudioCodec = codecSetting;
    }

    var startTime = $('#startTime').val();
    if (startTime) {
        rowObject.StartTime = startTime;
    }

    var endTime = $('#endTime').val();
    if (endTime) {
        rowObject.EndTime = endTime;
    }

    $('#audioSetting').modal('hide');
}

function clickConvert() {
    dataList.forEach((item, index) => {
        if (item.state == FILE_STATE_SET) {
            var convertOption = {};
            convertOption.audioFormat = item.audioFormat.toLowerCase();

            if (item.AdjustVolumn) {
                convertOption.adjustVolumn = item.AdjustVolumn;
            }
            if (item.AudioCodec) {
                convertOption.audioCodec = item.AudioCodec;
            }
            if (item.audioFormat.toLowerCase() == 'mp3') {
                if (item.AudioQuality) {
                    convertOption.audioQuality = item.AudioQuality;
                }
                else {
                    if (item.AudioBitRate) {
                        convertOption.audioBitRate = item.AudioBitRate;
                    }
                }
            } else {
                if (item.AudioBitRate) {
                    convertOption.audioBitRate = item.AudioBitRate;
                }
            }
            if (item.StartTime) {
                convertOption.startTime = item.StartTime;
            }
            if (item.EndTime) {
                convertOption.endTime = item.EndTime;
            }

            var formData = new FormData();

            formData.append("fileType", item.fileType);

            if (item.fileType == FILE_TYPE_URL) {
                formData.append("fileUrl", item.fileObj);
            }
            else {
                formData.append("file", item.fileObj);
            }
    
            formData.append("convertOption", JSON.stringify(convertOption));
            $.ajax({
                method: "POST",
                url: o.APIBasePath + 'conversion',
                data: formData,
                dataType: "json",
                processData: false,
                contentType: false,
                success: (data) => {
                    item.state = FILE_STATE_PROCESSING;
                    item.fileRequestId = data.Data.FileRequestId;
                    $('#rawFileTable').jsGrid("refresh");
                },
                error: (data) => {
                    item.state = FILE_STATE_FAIL;
                    $('#rawFileTable').jsGrid("refresh");
                    if (data.responseJSON.message) {
                        showAlert(data.responseJSON.message);
                    }
                }
            });
            item.state = FILE_STATE_UPLOADING;
        }
    });
    $('#rawFileTable').jsGrid("refresh");
}

var loopCheckStatusTimer = null;
function loopCheckStatus() {
    dataList.forEach((item, index) => {
        if (item.state == FILE_STATE_PROCESSING && item.fileRequestId) {
            $.ajax({
                method: "GET",
                url: o.APIBasePath + 'conversion/HandleStatus?fileRequestId=' + encodeURI(item.fileRequestId),
                dataType: "json",
                success: (data) => {
                    if (data.Data.Status == 0) {
                        item.state = FILE_STATE_DONE;
                        item.DownloadLink = data.Data.DownloadLink;
                        $('#rawFileTable').jsGrid("refresh");
                    }
                    else if (data.Data.Status == 1) {
                        item.state = FILE_STATE_FAIL;
                        $('#rawFileTable').jsGrid("refresh");
                    }

                },
                error: (data) => {
                    showAlert(data.responseJSON.message);
                }
            });
        }
    });
}

function showLoader() {
    $('.progress > .progress-bar').html('15%');
    $('.progress > .progress-bar').css('width', '15%');
    $('#loader').removeClass("hidden");
    hideAlert();
}

function hideLoader() {
    $('#loader').addClass("hidden");
}

function generateViewerLink(res) {
    var data = res.Data;
    return encodeURI(o.ViewerPathWF + '?FolderName='+data.FolderName) + '&FileName=' + encodeURIComponent(data.FileName);
}

function generateEditorLink(res) {
    var data = res.Data;
    return encodeURI(o.EditorPathWF + '?FolderName=' + data.FolderName + '&FileName=') + encodeURIComponent(data.FileName);
}
function generateNewEditorLink() {
    return encodeURI(o.EditorPathWF);
}

function sendPageView(url) {
    if ('ga' in window)
        try {
            var tracker = ga.getAll()[0];
            if (tracker !== undefined) {
                tracker.send('pageview', url);
            }
        } catch (e) {
            /**/
        }
}

function workSuccess(res, textStatus, xhr) {
    hideLoader();

    if (res.Code === 200) {
        var data = res.Data;
        $('#WorkPlaceHolder').addClass('hidden');
        $('.appfeaturesectionlist').addClass('hidden');
        $('.howtolist').addClass('hidden');
        $('.app-features-section').addClass('hidden');
        $('.app-product-section').addClass('hidden');
        $('#TextPlaceHolder').addClass('hidden');

        $('#DownloadPlaceHolder').removeClass('hidden');
        $('#OtherApps').removeClass('hidden');

        if (o.ReturnFromViewer === undefined) {
            const pos = o.AppDownloadURL.indexOf('?');
            const url = pos === -1 ? o.AppDownloadURL : o.AppDownloadURL.substring(0, pos);
            sendPageView(url);
        }

        //var url = encodeURI(o.APIBasePath + o.AppName.toLowerCase()+`/Storage/Download`) + `?file=${encodeURIComponent(data.FileName)}` + `&folder=${encodeURIComponent(data.FolderName)}`;
        var url = data.DownloadLink;
        $('#DownloadButton').attr('href', url);
        //$('#DownloadButton').attr('download', data.FileName);
        o.DownloadUrl = url;

        if (o.ShowViewerButton) {
            let viewerlink = $('#ViewerLink');
            let dotPos = data.FileName.lastIndexOf('.');
            let ext = dotPos >= 0 ? data.FileName.substring(dotPos + 1).toUpperCase() : null;
            if (ext !== null && viewerlink.length && VIEWABLE_EXTENSIONS.indexOf(ext) !== -1) {
                viewerlink.on('click', function (evt) {
                    evt.preventDefault();
                    evt.stopPropagation();
                    openIframe(generateViewerLink(data), '/viewer', '/diagram/view');
                });
            } else {
                viewerlink.hide();
                $(viewerlink[0].parentNode.previousElementSibling).hide(); // div.clearfix
            }
        }
    } else {
        showAlert(res.Status);
        ShowReportModal(res);
    }
}

function sendEmail() {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var email = $('#EmailToInput').val();
    if (!email || !re.test(String(email).toLowerCase())) {
        window.alert('Please specify the valid email address!');
        return;
    }

    var data = {
        appname: o.AppName,
        email: email,
        url: o.DownloadUrl,
        title: $('#ProductTitle')[0].innerText
    };

    $('#sendEmailModal').modal('hide');
    $('#sendEmailButton').addClass('hidden');

    $.ajax({
        method: 'POST',
        url: o.APIBasePath + o.AppName.toLowerCase() + +'/email/send-file',
        data: data,
        dataType: 'json',
        success: (res) => {
            showMessage(res.Message);
        },
        complete: () => {
            $('#sendEmailButton').removeClass('hidden');
            hideLoader();
        },
        error: (res) => {
            showAlert(res.responseJSON.Message);
        }
    });
}

function sendFeedback(text) {
    var msg = (typeof text === 'string' ? text : $('#feedbackText').val());
    if (!msg || msg.match(/^\s+$/) || msg.length > 1000) {
        return;
    }

    var data = {
        appname: o.AppName,
        text: msg
    };

    if (!text) {
        if ('ga' in window) {
            try {
                var tracker = window.ga.getAll()[0];
                if (tracker !== undefined) {
                    tracker.send('event', {
                        'eventCategory': 'Social',
                        'eventAction': 'feedback-in-download'
                    });
                }
            } catch (e) {
            }
        }
    }

    $.ajax({
        method: "POST",
        url: o.APIBasePath + o.AppName.toLowerCase() + '/email/feedback',
        data: data,
        dataType: "json",
        success: (data) => {
            showMessage(data.Message);
            $('#feedback').hide();
        },
        error: (data) => {
            showAlert(data.responseJSON.message);
        }
    });
}

function hideAlert() {
    $('#alertMessage').addClass("hidden");
    $('#alertMessage').text("");
    $('#alertSuccess').addClass("hidden");
    $('#alertSuccess').text("");
}

function showAlert(msg) {
    hideLoader();
    $('#alertMessage').html(msg);
    $('#alertMessage').removeClass("hidden");
    $('#alertMessage').fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);
}

function showMessage(msg) {
    hideLoader();
    $('#alertSuccess').text(msg);
    $('#alertSuccess').removeClass("hidden");
}

(function ($) {
    $.QueryString = (function (paramsArray) {
        let params = {};

        for (let i = 0; i < paramsArray.length; ++i) {
            let param = paramsArray[i]
                .split('=', 2);

            if (param.length !== 2)
                continue;

            params[param[0]] = decodeURIComponent(param[1].replace(/\+/g, " "));
        }

        return params;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

function progress(evt) {
    if (evt.lengthComputable) {
        var max = evt.total;
        var current = evt.loaded;

        var percentage = Math.round((current * 100) / max);
        percentage = (percentage < 15 ? 15 : percentage) + '%';

        $('.progress > .progress-bar').html(percentage);
        $('.progress > .progress-bar').css('width', percentage);
    }
}

function removeAllFileBlocks() {
    fileDrop.droppedFiles.forEach(function (item) {
        $('#fileupload-' + item.id).remove();
    });
    fileDrop.droppedFiles = [];
    hideLoader();
}

function openIframe(url, fakeUrl, pageViewUrl) {
    // push fake state to prevent from going back
    window.history.pushState(null, null, fakeUrl);

    // remove body scrollbar
    $('body').css('overflow-y', 'hidden');

    // create iframe and add it into body
    var div = $('<div id="iframe-wrap"></div>');
    $('<iframe>', {
        src: url,
        id: 'iframe-document',
        frameborder: 0,
        scrolling: 'yes'
    }).appendTo(div);
    div.appendTo('body');
    sendPageView(pageViewUrl);
}

function closeIframe() {
    removeAllFileBlocks();
    $('div#iframe-wrap').remove();
    $('body').css('overflow-y', 'auto');
}

function request(url, data) {
    showLoader();
    $.ajax({
        method: 'POST',
        url: url,
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        success: workSuccess,
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload)
                myXhr.upload.addEventListener('progress', progress, false);
            return myXhr;
        },
        error: function (err) {
            showAlert(err.responseJSON.Message);
            ShowReportModal(err.responseJSON);
        }
    });
}

function ShowReportModal(data) {
    $('#errorTitle').text(data.Message);
    $('#errorModal').modal('show');
    $("#errorFolderName").val(data.SessionId);
    $("#errorAction").val(data.Action);
    $("#errorMessage").val(data.Message);
}

function requestErrorReport() {
    if ($("#errorEmail").val().trim() == "") {
        return;
    }
    var reg = new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$");
    if (!reg.test($("#errorEmail").val())) {
        alert("email is invalid");
        return;
    }
    if ($('#forumPrivate').prop('checked')) {
        $("#errorPrivate").val("true");
    } else {
        $("#errorPrivate").val("false");
    }
    var url = o.APIBasePath + o.AppName.toLowerCase()+ "/report/error";
    $.ajax({
        method: 'POST',
        url: url,
        data: $("#errorForm").serialize(),
        contentType: 'application/x-www-form-urlencoded',
        cache: false,
        timeout: 600000,
        success: (d) => {
            if (d.StatusCode == 200) {
                $('#errorModal').modal('hide');
                $('#lnkForums').attr("href", d.ForumLink);
                $('#reportResultModal').modal('show');
            } else {
                alert(d.Status);
            }
        },
        error: (err) => {
            //alert("Internal Server Error");
        }
    });
}

function requestConversion() {
    let data = fileDrop.prepareFormData();
    if (data === null)
        return;

    let url = o.APIBasePath + 'conversion?outputType=' + $('#saveAs').val();
    request(url, data);
}

function requestViewer(data) {
    var url = generateViewerLink(data);
    console.log(url);
    openIframe(url, '/diagram/viewer', '/diagram/view');
}

function requestEditor(data) {
    var url = generateEditorLink(data);
    console.log(url);
    openIframe(url, '/diagram/editor', '/diagram/edit');
}

function requestNewEditor() {
    $("#newButton").val("Loading");
    var url = generateNewEditorLink();
    openIframe(url, '/diagram/editor', '/diagram/edit');
}

function requestMerger() {
    let data = fileDrop.prepareFormData(2, o.MaximumUploadFiles);
    if (data === null)
        return;

    let url = o.APIBasePath + 'merger?outputType=' + $('#saveAs').val();
    request(url, data);
}

function prepareDownloadUrl() {
    o.AppDownloadURL = o.AppURL;
    var pos = o.AppDownloadURL.indexOf(':');
    if (pos > 0)
        o.AppDownloadURL = (pos > 0 ? o.AppDownloadURL.substring(pos + 3) : o.AppURL) + '/download';
    else
        o.AppDownloadURL += '/download';
    pos = o.AppDownloadURL.indexOf('/');
    o.AppDownloadURL = o.AppDownloadURL.substring(pos);
}

function checkReturnFromViewer() {
    var query = window.location.search;
    if (query.length > 0) {
        o.ReturnFromViewer = true;
        var data = {
            StatusCode: 200,
            FolderName: $.QueryString['id'],
            FileName: $.QueryString['FileName'],
            FileProcessingErrorCode: 0
        };
        var beforeQueryString = window.location.href.split("?")[0];
        window.history.pushState({}, document.title, beforeQueryString);

        if (!o.UploadAndRedirect)
            workSuccess(data);
    }
}

function shareApp(type) {
    if (['facebook', 'twitter', 'linkedin', 'cloud', 'feedback', 'otherapp', 'bookmark'].indexOf(type) !== -1) {
        var gaEvent = function (action, category) {
            if (!category) {
                category = 'Social';
            }
            if ('ga' in window) {
                try {
                    var tracker = window.ga.getAll()[0];
                    if (tracker !== undefined) {
                        tracker.send('event', {
                            'eventCategory': category,
                            'eventAction': action
                        });
                    }
                } catch (err) {
                }
            }
        };
        var appPath = window.location.pathname.split('/');
        var appURL = 'https://' + window.location.hostname + "/" + appPath[1] + "/" + appPath[2];
        var title = document.title.replace('&', 'and');
        // Google Analytics event
        gaEvent(type.charAt(0).toUpperCase() + type.slice(1));

        // perform an action
        switch (type) {
            case 'facebook':
                var a = document.createElement('a');
                a.href = 'https://www.facebook.com/sharer/sharer.php?u=#' + encodeURI(appURL);
                a.setAttribute('target', '_blank');
                a.click();
                break;
            case 'twitter':
                var a = document.createElement('a');
                a.href = 'https://twitter.com/intent/tweet?text=' + encodeURI(title) + '&url=' + encodeURI(appURL);
                a.setAttribute('target', '_blank');
                a.click();
                break;
            case 'linkedin':
                var a = document.createElement('a');
                a.href = 'https://www.linkedin.com/sharing/share-offsite/?url=' + encodeURI(appURL);
                a.setAttribute('target', '_blank');
                a.click();
                break;
            case 'feedback':
                $('#feedbackModal').modal({
                    keyboard: true
                });
                break;
            case 'otherapp':
                document.location.href = 'https://products.aspose.app/audio/family';
                break;
            case 'cloud':
                var e = e || window.event;
                e = e.target || e.srcElement;
                if (e.tagName !== "A") {
                    var a = document.createElement('a');
                    a.href = 'https://products.aspose.cloud/Diagram/family';
                    a.setAttribute('target', '_blank');
                    a.click();
                }
                break;
            case 'bookmark':
                $('#bookmarkModal').modal({
                    keyboard: true
                });
                break;
            default:
            // nothing
        }
    }
}

function sendFeedbackExtended() {
    var text = $('#feedbackBody').val();
    if (text && !text.match(/^.s+$/)) {
        $('#feedbackModal').modal('hide');
        sendFeedback(text);
    }
}

function otherAppClick(name, left = false) {
    if ('ga' in window) {
        try {
            var tracker = window.ga.getAll()[0];
            if (tracker !== undefined) {
                tracker.send('event', {
                    'eventCategory': 'Other App Click' + (left ? ' Left' : ''),
                    'eventAction': name
                });
            }
        } catch (e) {
        }
    }
}

function findRowId(item) {
    var ret = -1;
    for (var i = 0; i < dataList.length; i++) {
        if (dataList[i] == item) {
            ret = i + 1;
            break;
        }
    }
    return ret;
}

$(document).ready(function () {
    try {
        prepareDownloadUrl();
        checkReturnFromViewer();

        if (o.AppName == "Conversion" && o.Accept == ".excel") {
            o.Accept = ".xls,.xlsx";
            o.UploadOptions = ".XLS,.XLSX";
        }
        

        if (!o.ShowHelpButton) {
            $('#showHelpButton').on('click', function () {
                // const helpTemplate = o.getTrustedResourceUrl('/assembly/' + ASPOSE_PRODUCTNAME + 'HelpTemplate.html');
                alert("showHelpButton");
                const helpTemplate = "/assembly/diagramHelpTemplate.html";
                $("#help-dialog-template > .modal-dialog > .modal-content").html(helpTemplate).contents();
            });
        }

        // social network modal
        $('#bookmarkModal').on('show.bs.modal', function (e) {
            $('#bookmarkModal').css('display', 'flex');
            $('#bookmarkModal').on('keydown', function (evt) {
                if ((evt.metaKey || evt.ctrlKey) && String.fromCharCode(evt.which).toLowerCase() === 'd') {
                    $('#bookmarkModal').modal('hide');
                }
            });
        });
        $('#bookmarkModal').on('hidden.bs.modal', function (e) {
            $('#bookmarkModal').off('keydown');
        });

        // send email modal
        $('#sendEmailButton').on('click', function () {
            $('#sendEmailModal').modal({
                keyboard: true
            });
        });
        $('#sendEmailModal').on('show.bs.modal', function () {
            $('#sendEmailModal').css('display', 'flex');
        });
        $('#sendEmailModal').on('shown.bs.modal', function () {
            $('#EmailToInput').focus();
        });

        // send feedback modal
        $('#feedbackModal').on('show.bs.modal', function (e) {
            $('#feedbackModal').css('display', 'flex');
        });
        $('#feedbackModal').on('shown.bs.modal', function () {
            $('#feedbackBody').focus();
        });

        //$('#sendFeedbackBtn').on('click', sendFeedback);

        // detect Ctrl + D keypress
        $(document).on('keydown', function (evt) {
            if (evt.originalEvent.code === 'KeyD' && evt.originalEvent.ctrlKey && !evt.originalEvent.altKey && !evt.originalEvent.shiftKey && !evt.originalEvent.metaKey) {
                if ('ga' in window) {
                    try {
                        var tracker = window.ga.getAll()[0];
                        if (tracker !== undefined) {
                            tracker.send('event', {
                                'eventCategory': '[Ctrl + D] keypress',
                                'eventAction': 'Target: ' + window.location.pathname
                            });
                        }
                    } catch (e) {
                    }
                }
            }
        });
        $('#rawFileTable').jsGrid(
            {
                data: dataList,
                height: 'auto',
                width: '100%',
                heading: false,
                noDataContent: '',
                fields: [
                    {
                        name: 'filePath',
                        width: '30%',
                        align: "left",
                        itemTemplate: function (value, rowObject) {
                            return '<span class="glyphicon glyphicon-file"></span>' + '<span>' + rowObject.filePath + '</span>';
                        }
                    },
                    {
                        name: 'convertSetting',
                        width: '30%',
                        css: "bootstrap_dropdown",
                        align: "left",
                        itemTemplate: function (value, rowObject) {
                            var rowId = findRowId(rowObject);
                            if (rowObject.state == FILE_STATE_PROCESSING || rowObject.state == FILE_STATE_DONE) {
                                return '';
                            }
                            var audioFormat = '...';
                            if (rowObject.audioFormat) {
                                audioFormat = rowObject.audioFormat;
                            }
                            var ret = 'Convert to<div class="btn-group btn-select"><button type="button" class="btn btn-default dropdown-toggle btn-select2" data-toggle="dropdown"><span id="selectFormat">'
                                +
                                audioFormat
                                + '</span> <span class="caret"></span></button><ul class="dropdown-menu dropdown-menu2" role="menu"><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'AAC\')">AAC</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'AIFF\')">AIFF</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'FLAC\')">FLAC</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'M4A\')">M4A</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'MP3\')">MP3</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'WAV\')">WAV</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'WMA\')">WMA</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'AC3\')">AC3</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'CAF\')">CAF</a><a class="dm-msg" onClick="AudioFormatSelect(' + rowId + ',\'OGG\')">OGG</a></ul></div>';
                            if (rowObject.audioFormat) {
                                ret += '<button class="btn set-btn" onclick="openAudioSettingModel(' + rowId + ')">set</button>'
                            }
                            return ret;
                        }
                    },
                    {
                        name: 'operation',
                        width: '40%',
                        align: "left",
                        itemTemplate: function (value, rowObject) {
                            var rowId = findRowId(rowObject);
                            var ret = '';
                            if (rowObject.state == FILE_STATE_UPLOADING) {
                                ret += '<span class="file_status">uploading</span><i style="margin-left:5px" class="fa fa-circle-o-notch fa-spin"></i>';
                            }
                            else if (rowObject.state == FILE_STATE_FAIL) {
                                ret += '<span class="file_status_error">failed</span>';
                                ret += '<span class="glyphicon glyphicon-remove-sign pull-right" onclick="removeItem(' + rowId + ')"></span>'
                            }
                            else if (rowObject.state == FILE_STATE_PROCESSING) {
                                ret += '<span class="file_status">processing</span><i style="margin-left:5px" class="fa fa-circle-o-notch fa-spin"></i>';
                            }
                            else if (rowObject.state == FILE_STATE_DONE) {
                                ret += '<span class="file_status">finished</span>';
                                ret += '<a class="file_download pull-right" href="' + rowObject.DownloadLink + '">Download</a>';
                            }
                            else {
                                ret += '<span class="glyphicon glyphicon-remove-sign pull-right" onclick="removeItem(' + rowId + ')"></span>'
                            }
                            
                            return ret;
                        }
                    }
                ],
                onRefreshing: function (grid) {
                    if (dataList.length == 0) {
                        $('#rawTableDiv').hide();
                    } else {
                        $('#rawTableDiv').show();
                    }
                    
                }

            });
        $('#rawFileTable').jsGrid("refresh");

        loopCheckStatusTimer = setInterval(function () { loopCheckStatus(); }, 5000); 
    } catch{

    }
});
