<%@ WebHandler Language="C#" Class="report" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

public class report : IHttpHandler {
    
    FoosBallDB db = new FoosBallDB();
    
    public void ProcessRequest (HttpContext context) {

        string i = context.Request.Form["i"];
        //check path based on index 
        if (i == "1"){
            get_all_games(context);
        }else if (i == "2") {
            get_game_details(context);
        }else{
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }
    }

    private void get_all_games(HttpContext context){

        List<Game> games = db.get_all_games();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize((object)games);
        context.Response.Write(json);
        context.Response.End();
    }

    private void get_game_details(HttpContext context){
        //HttpContext context = HttpContext.Current;
        int game_id = Convert.ToInt32(context.Request.Form["game_id"]);
        Game games = db.get_game_details(game_id);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize((object)games);
        context.Response.Write(json);
        context.Response.End();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}