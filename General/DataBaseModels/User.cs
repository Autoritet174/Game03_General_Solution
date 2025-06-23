using System;
using System.Text.Json.Serialization;

namespace General.DataBaseModels;

public class User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime Created_at { get; set; }


    [JsonPropertyName("updated_at")]
    public DateTime Updated_at { get; set; }


    [JsonPropertyName("deleted_at")]
    public DateTime? Deleted_at { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }


    [JsonPropertyName("email_verified_at")]
    public DateTime? Email_verified_at { get; set; }

    [JsonPropertyName("user_name")]
    public string? User_name { get; set; }

    [JsonPropertyName("password_hash")]
    public string? Password_hash { get; set; }
}