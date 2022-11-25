using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models;

public class User
{
    [Key] public int Id { get; set; }
    [StringLength(48)] public string Firstname { get; set; } = "";
    [StringLength(48)] public string Lastname { get; set; } = "";
    [StringLength(48)] public string Email { get; set; } = "";
    [StringLength(32)] public string Password { get; set; } = "";
    public bool IsAdmin { get; set; }
}