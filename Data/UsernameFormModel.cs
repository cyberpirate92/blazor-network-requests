using System.ComponentModel.DataAnnotations;

public class UsernameFormModel
{
    [Required(AllowEmptyStrings = false)]
    [MinLength(3)]
    public string Username { get; set; }
}