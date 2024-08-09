using System.ComponentModel;

namespace HomeEasy.Enums;

public enum UserType
{
    [Description("Cliente")]
    Client,

    [Description("Trabalhador")]
    Worker,

    [Description("Adminstrador")]
    Admin
}
