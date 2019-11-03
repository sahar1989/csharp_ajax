$(document).ready(function () {
    //get_all_games();
});

String.prototype.replace_all = function (strTarget, strSubString) {
    let strText = this;
    let intIndexOfMatch = strText.indexOf(strTarget);
    while (intIndexOfMatch != -1) {
        strText = strText.replace(strTarget, strSubString);
        intIndexOfMatch = strText.indexOf(strTarget);
    }
    return (strText);
}

//get all games
function get_all_games() {

    let header = "<table class='table table-striped table-bordered text-center' > " +
       "<tr class='record text-center' >" +
       "<td>row</td>" +
       "<td>game</td>" +
       "<td>team1 name</td>" +
       "<td>team2 name</td>" +
       "<td>start date</td>" +
       "<td>end date</td>" +
       "<td>winner</td>" +
       "<td>details</td></tr>";

    let main_row = "<tr id='record{game_id}' class='record text-center' >" +
        "<td>{row}</td>" +
        "<td>{game_id}</td>" +
        "<td>{team1_name}</td>" +
        "<td>{team2_name}</td>" +
        "<td>{start_date}</td>" +
        "<td>{end_date}</td>" +
        "<td>{winner_name}</td>" +
        "<td><input type='button' id='btn_details' value='view details' onclick='get_game_details({game_id});' /></td></tr>";
    let footer = "</table></td></tr><tr><td>";
    let rows = "";
    let counter = 0;
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: {
            i: 1
        },
        url: "/api/public_api.ashx",
        success: function (data) {
            $.each(data, function (index) {
                row = main_row;
                row = main_row.replace_all("{row}", counter++);
                row = row.replace_all("{game_id}", this['id']);
                row = row.replace_all("{id}", this['Id']);
                row = row.replace_all("{team1_name}", this['team1_name']);
                row = row.replace_all("{team2_name}", this['team2_name']);
                row = row.replace_all("{start_date}", this['start_date']);
                row = row.replace_all("{end_date}", this['end_date']);
                row = row.replace_all("{winner_name}", this['winner_name']);
                rows = rows + row;
            });

            $("#div_report").html(header + rows + footer);
        }
    });
}
//get game details by game id 
function get_game_details(game_id) {

    let details_panel =
        "<div class='center'><label>game : {game_id}</label></div>"+
        "<div class='row'>" +
        "<div class='col-sm'><label>team1 name : {team1_name}</label></div>" +
        "<div class='col-sm'><label>team2 name : {team2_name}</label></div></div>" +
        "<div class='row'>" +
        "<div class='col-sm'><label>team1 goal : {team1_goal}</label></div>" +
        "<div class='col-sm'><label>team2 goal : {team2_goal}</label></div></div>" +
        "<div class='row'>" +
        "<div class='col-sm'><label>team1 set : {team1_set}</label></div>" +
        "<div class='col-sm'><label>team2 set : {team2_set}</label></div></div>" +
        "<div class='row'>" +
        "<div class='col-sm'><input type='button' id='btn_update_t1' value='update' onclick='update_set_goal({game_id},{team1_id});' /></div>" +
        "<div class='col-sm'><input type='button' id='btn_update_t2' value='update' onclick='update_set_goal({game_id},{team2_id});' /></div></div>" +
        "<div class='center'><label>winner : {winner}</label></div>";

    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: {
            i: 2,
            game_id: game_id
        },
        url: "/api/public_api.ashx",
        success: function (data) {
            row = details_panel;
            row = row.replace_all("{game_id}", data['id']);
            row = row.replace_all("{team1_id}", data['team1_id']);
            row = row.replace_all("{team2_id}", data['team2_id']);
            row = row.replace_all("{team1_name}", data['team1_name']);
            row = row.replace_all("{team2_name}", data['team2_name']);
            row = row.replace_all("{team1_goal}", data['team1_goal']);
            row = row.replace_all("{team2_goal}", data['team2_goal']);
            row = row.replace_all("{team1_set}", data['team1_set']);
            row = row.replace_all("{team2_set}", data['team2_set']);
            row = row.replace_all("{winner}", data['winner_name']);
            $("#div_details").css('display', 'block');
            $("#div_details").html(row);
        }
    });
}
//update goals winner 
function update_set_goal(game_id,team_id) {

    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: {
            i: 3,
            game_id: game_id,
            team_id: team_id
        },
        url: "/api/public_api.ashx",

        success: function (data) {
            $("#div_message").css('display', 'block');
            $("#div_message").html(data);
            $("#div_details").css('display', 'none');
            get_all_games();
            get_game_details(game_id);
        }
    });
}