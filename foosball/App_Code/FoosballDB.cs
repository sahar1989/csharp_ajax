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
    int SET_VALUE = 2;
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
                    game.team1_name = reader["team1_name"].ToString();
                    game.team2_name = reader["team2_name"].ToString();
                    game.start_date = Convert.ToDateTime(reader["start_date"]).ToString();
                    game.end_date = reader["end_date"].ToString() == "" ? "has not finished yet" : reader["end_date"].ToString();
                    game.team1_id = Convert.ToInt32(reader["team1_id"]);
                    game.team2_id = Convert.ToInt32(reader["team2_id"]);
                    game.winner_id = Convert.ToInt32(reader["winner_id"]);
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
            start_date = Convert.ToDateTime( data_table.Columns["start_date"]).ToString(),
            end_date = data_table.Columns["end_date"].ToString() == "" ? "has not finished yet" : data_table.Columns["end_date"].ToString(),
            team1_id = Convert.ToInt32( data_table.Columns["team1_id"]),
            team2_id = Convert.ToInt32(data_table.Columns["team2_id"]),
            winner_id = Convert.ToInt32(data_table.Columns["winner"]),
            winner_name =  data_table.Columns["winner_name"].ToString(),
            team1_goal = Convert.ToInt32(data_table.Columns["team1_goal"]),
            team2_goal = Convert.ToInt32(data_table.Columns["team2_goal"]),
            team1_set = Convert.ToInt32(data_table.Columns["team1_set"]),
            team2_set = Convert.ToInt32(data_table.Columns["team2_set"]),
        };
        return game;
    }

    public string update_set_goal(int team_id,int game_id)
    {
        set_con();
        string result = "update failed";
        using (var cmd = new SqlCommand("sp_update_goal", con))
        {
            cmd.Parameters.AddWithValue("@game_id", game_id);
            cmd.Parameters.AddWithValue("@team_id", team_id);
            cmd.CommandType = CommandType.StoredProcedure;
            open_connection();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                result = "update done";
               
              
            }
            catch { result = "update error"; }
            close_connection();
        }
        //check the game finish or no
        if (check_set(team_id, game_id) == SET_VALUE)
        {
            result = update_games(team_id, game_id);
        }
        return result;
    }
    
    private int check_set(int team_id, int game_id)
    {
        set_con();
        int set_no = 0;
        using (var cmd = new SqlCommand("sp_get_set_no", con))
        {
            cmd.Parameters.AddWithValue("@game_id", game_id);
            cmd.Parameters.AddWithValue("@team_id", team_id);
            cmd.CommandType = CommandType.StoredProcedure;
            open_connection();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    set_no = Convert.ToInt32(reader["set_no"]);
                }
            }
            close_connection();
        }
        return set_no;
    }

    private string update_games(int team_id, int game_id)
    {
        set_con();
        string result = "update winner failed";
        using (var cmd = new SqlCommand("sp_update_winner", con))
        {
            cmd.Parameters.AddWithValue("@game_id", game_id);
            cmd.Parameters.AddWithValue("@team_id", team_id);
            cmd.Parameters.AddWithValue("@end_date", DateTime.Now);
            cmd.CommandType = CommandType.StoredProcedure;
            open_connection();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                result = "update winner done";
            }
            catch { result = "update winner error"; }
            close_connection();

        }
        return result;
    }
}