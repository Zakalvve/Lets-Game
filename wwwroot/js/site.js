﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function pinEvent(id) {
    $.ajax({
        type: "POST",
        url: `/Hub/Events/Index?handler=ToggledPinnedAjax&friendID=${id}`,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { eventID: id },
        success: function (IsPinned) {
            console.log(IsPinned);
            if (IsPinned) {
                $(`#${id}`).removeClass("btn-primary").removeClass("btn-outline-primary").addClass("btn-primary");
                $(`#${id}`).html("<i class=\"bi bi-pin-fill\"></i>");
            }
            else {
                $(`#${id}`).removeClass("btn-primary").removeClass("btn-outline-primary").addClass("btn-outline-primary");
                $(`#${id}`).html("<i class=\"bi bi-pin-angle\"></i>");
            }

            $("#pinned-events").load("/Hub/Events/Index?handler=PinnedEventsPartial");
        },
        failure: function () {
            console.log("Failed to pin event: Bad Request");
        }
    });
}

function vote(pollId, voteId, url) {
    $.ajax({
        type: "POST",
        url: `/Hub/Events/Index?handler=CastVoteAjax&pollID=${pollId}&pollOptionID=${voteId}`,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            pollID: pollId,
            pollOptionID: voteId
        },
        success: function () {
            $("#event-poll").load(`/Hub/Events/Index?handler=PollPartial&pollID=${pollId}`);
        },
        failure: function () {
            console.log("Failed to vote on event: Bad Request");
        }
    });
}