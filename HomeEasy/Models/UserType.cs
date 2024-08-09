using System.ComponentModel;

namespace HomeEasy.Models;

public enum UserType
{
    [Description("Cliente")]
    Client,

    [Description("Trabalhador")]
    Worker,

    [Description("Adminstrador")]
    Admin
}
