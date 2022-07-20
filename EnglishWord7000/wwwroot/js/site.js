function playSound(btn) {
    var src_mp3 = btn.attr("data-src-mp3");
    var src_ogg = btn.attr("data-src-ogg");

    if (supportAudioHtml5()) {
        playHtml5(src_mp3, src_ogg);
    } else if (supportAudioFlash()) {
        playFlash(src_mp3, src_ogg);
    } else {
        playRaw(src_mp3, src_ogg);
    }
}

function supportAudioHtml5() {
    var audioTag = document.createElement('audio');
    try {
        return (!!(audioTag.canPlayType)
            && ((audioTag.canPlayType("audio/mpeg") != "no" && audioTag.canPlayType("audio/mpeg") != "")
                || (audioTag.canPlayType("audio/ogg") != "no" && audioTag.canPlayType("audio/ogg") != "")));
    } catch (e) {
        return false;
    }
}

function supportAudioFlash() {
    var flashinstalled = 0;
    var flashversion = 0;
    if (navigator.plugins && navigator.plugins.length) {
        x = navigator.plugins["Shockwave Flash"];
        if (x) {
            flashinstalled = 2;
            if (x.description) {
                y = x.description;
                flashversion = y.charAt(y.indexOf('.') - 1);
            }
        }
        else {
            flashinstalled = 1;
        }
        if (navigator.plugins["Shockwave Flash 2.0"]) {
            flashinstalled = 2;
            flashversion = 2;
        }
    }
    else if (navigator.mimeTypes && navigator.mimeTypes.length) {
        x = navigator.mimeTypes['application/x-shockwave-flash'];
        if (x && x.enabledPlugin)
            flashinstalled = 2;
        else
            flashinstalled = 1;
    }
    else {
        for (var i = 7; i > 0; i--) {
            flashVersion = 0;
            try {
                var flash = new ActiveXObject("ShockwaveFlash.ShockwaveFlash." + i);
                flashVersion = i;
                return (flashVersion > 0)
            }
            catch (e) { }
        }
    }
    return (flashinstalled > 0);
}

function playHtml5(src_mp3, src_ogg) {
    //use appropriate source
    var audio = new Audio("");
    if (audio.canPlayType("audio/mpeg") != "no" && audio.canPlayType("audio/mpeg") != "")
        audio = new Audio(src_mp3);
    else if (audio.canPlayType("audio/ogg") != "no" && audio.canPlayType("audio/ogg") != "")
        audio = new Audio(src_ogg);

    //play
    audio.addEventListener("error", function (e) { alert("Apologies, the sound is not available."); });
    audio.play();
}

function playFlash(src_mp3, src_ogg) {
    var src_flash = "#skAssetUrl('/external/flash/speaker.swf?song_url=" + src_mp3 + "&autoplay=true')";
    if (navigator.plugins && navigator.mimeTypes && navigator.mimeTypes.length) { // netscape plugin architecture
        $("body").append("<embed type='application/x-shockwave-flash' src='" + src_flash + "' width='0' height='0'></embed>");
    } else { // PC I
        $("body").append("<object type='application/x-shockwave-flash' width='0' height='0' codebase='https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0' data='" + src_flash + "'><param name='wmode' value='transparent'/><param name='movie' value='" + src_flash + "'/><embed src='" + src_flash + "' width='0' height='0' ></embed></object>");
    }
}

function playRaw(src_mp3, src_ogg) {
    window.open(src_mp3, "Sound", "menubar=no, status=no, scrollbars=no, menubar=no, width=200, height=100");
}

function bindAudioButtons() {
    $(".audio_play_button").off('click').click(function () {
        playSound($(this));
    });
}

$(function () {
    bindAudioButtons();
});

function HideShowElement(elem) {
    let x = elem.style.display;
    if (elem.style.display == 'none' || elem.style.display == '') elem.style.display = 'block';
    else elem.style.display = 'none';
}

var list = document.getElementById("content_in_russian").childNodes;
let boolFistGap = true;
for (let index = 0; index < list.length; index++) {
    let x = list[index].classList;
    if (x != null && x.contains("pos_item") && (index + 1) < list.length) {
        list[index].onclick = () => { HideShowElement(list[index + 1]) };
    }

    if (x != null && x.contains("gap") && (index + 4) < list.length && list[index + 2].innerHTML == 'Словосочетания') {
        list[index + 2].onclick = () => { HideShowElement(list[index + 4]) };
    }
    if (x != null && x.contains("gap") && (index + 4) < list.length && list[index + 2].innerHTML == 'Возможные однокоренные слова') {
        list[index + 2].onclick = () => { HideShowElement(list[index + 4]) };
    }

}

function MoreElement(elem1, elem2, hideelem) {
    elem2.hidden = false;
    elem1.hidden = true;
    if (hideelem.hidden) {
        hideelem.hidden = false;
    }
    else {
        hideelem.hidden = true;
    }
}
let list2 = document.getElementsByClassName("more_up")[0];
let list3 = document.getElementsByClassName("more_down")[0];
if (list2 != null && list3 != null) {
    list3.hidden = true;
    let hideelem = document.getElementById('hidden_ex');
    hideelem.hidden = true;
    list2.onclick = () => MoreElement(list2, list3, hideelem);
    list3.onclick = () => MoreElement(list3, list2, hideelem);
}