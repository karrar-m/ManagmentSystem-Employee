using Domain._Common.App;

namespace ManagmentSystem.Domain.Security.User.Db;

public class User : Entity
{

    public string Name { get; set; }
    public string Username { get; set; }
    public string Passwored { get; set; }
    public string PhoneNO { get; set; }
    public string Imagecoed { get; set; }

    public bool Isonline { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? LastLogout { get; set; }

    public bool IsAdmin { get; set; }

}
