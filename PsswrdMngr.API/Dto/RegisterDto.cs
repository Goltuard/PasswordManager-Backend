using System.ComponentModel.DataAnnotations;

namespace PsswrdMngr.API.Dto;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string PublicKey { get; set; }
}