// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("div#lang-selector > ul a").on("click", function (e) {
        e.preventDefault();
        var lang = e.target.innerText;
        $.post({
            url: "./Index?handler=setlang",
            data: lang,
            contentType: 'text/plain'
        })
            .done(function (response) {
                window.location.reload(false);
                $("div#lang-selector > button").text(lang + " ");
            });
    })
});
