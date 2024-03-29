﻿using System.Text.Json.Serialization;
using OnlineMarket.Domain.Enums;

namespace OnlineMarket.WebAPI.Data.Models;

public class Person
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    [JsonIgnore] public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? City { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhotoName { get; set; } = null!;

    [JsonIgnore] public RoleType RoleId { get; set; }

    [JsonIgnore] public ICollection<Ad> Ads { get; set; } = new List<Ad>();

    [JsonIgnore] public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    [JsonIgnore] public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public Role Role { get; set; } = null!;
}