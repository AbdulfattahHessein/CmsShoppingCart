// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/// <reference path="../lib/jquery/dist/jquery.js" />

$(function () {
    if ($("a.confirmDeletion").Length != 0) {

        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm Deletion")) return false;
        });

    }
    if ($("div.notification").Length != 0) {

        setTimeout(() => $("div.notification").fadeOut(), 2000);

    }
});

function readUrl(input) {
    /*    console.log("HIHIHHI");*/
    console.log(input.files);
    if (input.files != null && input.files[0] != null) {
        console.log("Look here")
        let reader = new FileReader();
        reader.onload = (e) => $("img#imgpreview").attr("src", e.target.result).width(400);
        reader.readAsDataURL(input.files[0]);
    }

}




