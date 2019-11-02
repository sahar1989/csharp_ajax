using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;  

/// <summary>
/// Summary description for conncetion
/// </summary>
public class FoosBallDB
{
    public FoosBallDB()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    string con_string = "foosball";
    SqlConnection con;

    public void set_con()
    {
        var connectionString = ConfigurationManager.ConnectionStrings[con_string].ConnectionString;
        con = new SqlConnection(connectionString);
    }
    public void open_connection(){
       
        if (con.State == ConnectionState.Closed){
            con.Open();
        }
    }
    public void close_connection(){
        con.Close();
    }
    public List<Game> get_all_games()
    {
        set_con();
        List<Game> games = new List<Game>();
        using (var cmd = new SqlCommand("sp_get_all_games", con))
        {
           cmd.CommandType = CommandType.StoredProcedure;
           open_connection();
           SqlDataReader reader = cmd.ExecuteReader();
           if (reader.HasRows) {
               DataTable data_table = new DataTable();
               data_table.Load(reader);
               games = retrive_games(data_table);
           }
           reader.Close();
           close_connection(); 
        }
        return games;
    }
    private List<Game> retrive_games(DataTable data_table)
    {
        var games = (from rows in data_table.AsEnumerable()
                        select new Game()
                        {
                            id = Convert.ToInt32(rows["game_id"]),
                            team1_name = rows["team1_name"].ToString(),
                            team2_name = rows["team2_name"].ToString(),
                            winner_id = Convert.ToInt32(rows["winner_id"]),
                            start_date = Convert.ToDateTime(rows["start_date"]).ToString(),
                            end_date = rows["end_date"].ToString() == "" ? "has not finished yet" : rows["end_date"].ToString(),
                            team1_id = Convert.ToInt32(rows["team1_id"]),
                            team2_id = Convert.ToInt32(rows["team2_id"]),
                            winner_name = rows["winner_name"].ToString(),
                        }).ToList();
        return games;
    }
    public Game get_game_details(int game_id)
    {
        set_con();
        Game game = new Game();
        using (var cmd = new SqlCommand("sp_get_game_details", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@game_id", game_id);
            open_connection();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    game.id = Convert.ToInt32(reader["game_id"]);
                    game.team1_name = reader["team2_name"].ToString();
                    game.winner_id = Convert.ToInt32(reader["winner_id"]);
                    game.start_date = Convert.ToDateTime(reader["start_date"]).ToString();
                    game.end_date = reader["end_date"].ToString() == "" ? "has not finished yet" : reader["end_date"].ToString();
                    game.team1_id = Convert.ToInt32(reader["team1_id"]);
                    game.team2_id = Convert.ToInt32(reader["team2_id"]);
                    game.winner_name = reader["winner_name"].ToString();
                    game.team1_goal = Convert.ToInt32(reader["team1_goal"]);
                    game.team2_goal = Convert.ToInt32(reader["team2_goal"]);
                    game.team1_set = Convert.ToInt32(reader["team1_set"]);
                    game.team2_set = Convert.ToInt32(reader["team2_set"]);
                }
            }
            reader.Close();
            close_connection();
        }
        return game;
    }
    private Game retrive_game_details(DataTable data_table)
    {
        var game = new Game(){
            id = Convert.ToInt32(data_table.Columns["game_id"]),
            team1_name =  data_table.Columns["team1_name"].ToString(),
            team2_name =  data_table.Columns["team2_name"].ToString(),
            winner_id = Convert.ToInt32( data_table.Columns["winner"]),
            start_date = Convert.ToDateTime( data_table.Columns["start_date"]).ToString(),
            end_date = data_table.Columns["end_date"].ToString() == "" ? "has not finished yet" : data_table.Columns["end_date"].ToString(),
            team1_id = Convert.ToInt32( data_table.Columns["team1_id"]),
            team2_id = Convert.ToInt32(data_table.Columns["team2_id"]),
            winner_name =  data_table.Columns["winner_name"].ToString(),
            team1_goal = Convert.ToInt32(data_table.Columns["team1_goal"]),
            team2_goal = Convert.ToInt32(data_table.Columns["team2_goal"]),
            team1_set = Convert.ToInt32(data_table.Columns["team1_goal"]),
            team2_set = Convert.ToInt32(data_table.Columns["team2_goal"]),
        };
        return game;
    }

}