$(document).ready(function () {
   // get_all_games();
    get_game_details(1);
});

//get all games
function get_all_games() {
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: {
            i: 1
        },
        url: "/api/report.ashx",
        success: function (data) {
            console.log("get_all_games");
            console.log(data);
        }
    });
}
//get game details by game id 
function get_game_details(game_id) {
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: {
            i: 2,
            game_id:game_id
        },
        url: "/api/report.ashx",
        success: function (data) {
            console.log("get_game_details");
            console.log(data);
        }
    });
}