$(function () {
    $("div#lang-selector > ul a").on("click", function (e) {
        e.preventDefault();
        var lang = e.target.innerText;
        $.post({
            url: "/home/setlang",
            data: lang,
            contentType: 'text/plain'
        })
            .done(function (response) {
                window.location.reload(false);
                $("div#lang-selector > button").text(lang + " ");
            });
    })
});
