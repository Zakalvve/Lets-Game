﻿//This script re-styles the layout and creates a simulated app window
//Page scrolling is disabled and "display areas" scroll their overflow locally instead.

$(document).ready(function () {
    $("main").addClass("overflow-hidden");
    $(".app-mode-row").addClass("overflow-auto flex-nowrap");
    $(".app-mode-col").addClass("overflow-auto");
});

function removeActive(filter) {
	$(filter).removeClass("active");
}
function toggleActive(filter, e) {
	$(filter).removeClass("active");
	$(e).addClass("active");
}
function displayFriends(filter, e) {
	toggleActive(filter, e);
	//ajax to load list of friends and display
	$("#display").html("");
	$("#page-list").load(`/Hub/Friends/Index?handler=FriendsListPartial`);
}
function displayFriendRequests(filter, e) {
	toggleActive(filter, e);
	//ajax to display friend requests
	$("#display").html("");
	$("#page-list").load(`/Hub/Friends/Index?handler=RequestsListPartial`);
}
function displaySendRequests(filter, e) {
	toggleActive(filter, e);
	//ajax to display friend requests
	$("#display").html("");
	$("#page-list").load(`/Hub/Friends/Index?handler=SentRequestsListPartial`);
}

function displayEventRequests(filter, e) {
	toggleActive(filter, e);
	//ajax to display friend requests
	$("#display").html("");
	$("#page-list").load(`/Hub/Friends/Index?handler=EventRequestsListPartial`);
}

function displayAcceptEventRequest(filter, e, eventId) {
	toggleActive(filter, e);
	$("#display").addClass("flex-column-reverse");
	$("#display").load(`/Hub/Friends/Index?handler=AcceptEventRequestPartial&eventId=${eventId}`);
}

function displayChat(filter, e, id) {
	toggleActive(filter, e);
	//ajax to recover chat for friend with id
	$("#display").addClass("flex-column-reverse");
	$("#display").load(`/Hub/Friends/Index?handler=FriendChatPartial&userId=${id}`);
	$("#friend-controls").load(`/Hub/Friends/Index?handler=FriendsListControlsPartial&friendId=${id}`)
}
function displayFriendRequest(filter, e, id) {
	toggleActive(filter, e);
	$("#display").removeClass("flex-column-reverse");
	$("#display").load(`/Hub/Friends/Index?handler=FriendRequestPartial&friendId=${id}`);
}

function displayAddFriend() {
	removeActive(".list-group-item");
	$("#display").load(`/Hub/Friends/Index?handler=AddFriendPartial`);
}

function submitFriendRequest() {
	value = $("#selection").val();
	$("#id-value").val($('#users [value="' + value + '"]').data('value'));
	$("#page-form").submit();
}