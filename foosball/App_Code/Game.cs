using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Game
/// </summary>
public class Game
{
	public Game()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int id{get; set;}
    public string start_date { get; set; }
    public string end_date{get; set;}
    public string team1_name{get; set;}
    public string team2_name{get; set;}
    public int team1_id { get; set; }
    public int team2_id { get; set; }
    public int winner_id { get; set; }
    public string winner_name{ get; set; }
    public int team1_goal { get; set; }
    public int team2_goal { get; set; }
    public int team1_set { get; set; }
    public int team2_set { get; set; }
}