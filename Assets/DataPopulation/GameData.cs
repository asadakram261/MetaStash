using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locations
{
    public int  id {set; get;}
    public string reward_type { set; get; }
    public float reward_amount{ set; get; }
    public bool is_active { set; get; }
    public double longitude { set; get; }
    public double latitude { set; get; }
    public string time_to_active { set; get; }
    public string created_at { set; get; }
    public string updated_at { set; get; }

}
public class VoteData
{
    public int ID;
    public int Votess;
}
[System.Serializable]
public class Sign_In
{
    public userSignIN user;

}
[System.Serializable]
public class userSignIN
{
    public string email;
    public string password;
}

[System.Serializable]
public class SignUP
{
    public userData user;

}
[System.Serializable]
public class userData
{
    public string name;
    public string email;
    public string password;
    public string password_confirmation;
    public string city;
    public string country;
}
[System.Serializable]
public class CastVote
{
    public int player_id;
    public int ngo_id;
}

public class NGO_Data
{
    public int id { get; set; }
    public string name { get; set; }
}

public class ActivePoll
{
    public int Ngo_id { get; set; }
    public string Name { get; set; }
    public int Votes { get; set; }
}






[System.Serializable]
public class PlayerData
{
    public Player_Data data;
}
[System.Serializable]
public class Player_Data
{
    public string message;
    public Player_data user;
    public int status;
}

[System.Serializable]
public class Player_data
{

    public int id;
    public string created_at;
    public string updated_at;
    public string email;
    public string name;
    public string role;
    public string city;
    public string country;
    public bool payment_status;
    public string payment_date;
    public bool vote_casted;
    public float total_collection;
}

[System.Serializable]
public class Signup_Info
{
    public string message;
    public User_value user;
    public int status;
}
[System.Serializable]

public class User_value
{ 
    public int id;
    public string created_at;
    public string updated_at;
    public string email;
    public string name;
    public string role;
    public string city;
    public string country;
    public bool payment_status;
    public string payment_date;
    public bool vote_casted;
    public float total_collection;
    

}
[System.Serializable]
public class PollData
{
    public int user_id;
    public int ngo_id_1;
    public int ngo_id_2;
    public int ngo_id_3;
    public int ngo_id_4;

}


public class PaymentData
{
    public int id;
    public bool payment_status;

}
[System.Serializable]
public class RootNgo

{
    public int id;
    public string name;

}

[System.Serializable]
public class RootUser
{
    public UserData user;

}
[System.Serializable]
public class UserData
{

    public int id;
    public string email;
    public string created_at;
    public string updated_at;
    public string name;
    public string role;
    public string city;
    public string country;
    public bool payment_status;
    public string payment_date;
    public bool vote_casted;
    public float total_collection;

}

[System.Serializable]

public class Roots
{
    public FeedbackData data;
}
public class FeedbackData
{
    public int id;
    public string message_heading;
    public string message_details;

}
public class collectDrop
{
    public int player_id;
    public int drop_id;

}

[System.Serializable]
public class DropData
{
    public string message;
    public float balance;
    public int status;
}










