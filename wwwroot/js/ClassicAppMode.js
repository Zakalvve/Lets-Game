//This script re-styles the layout and creates a simulated app window
//Page scrolling is disabled and "display areas" scroll their overflow locally instead.

$(document).ready(function () {
    $("main").addClass("overflow-hidden");
    $(".app-mode-row").addClass("overflow-auto flex-nowrap");
    $(".app-mode-col").addClass("overflow-auto");
});