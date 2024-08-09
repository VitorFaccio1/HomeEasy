using System.ComponentModel.DataAnnotations;
using HomeEasy.Enums;

namespace HomeEasy.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "Email é obrigatório")]
    [StringLength(50, ErrorMessage = "Deve ter entre 5 e 50 caracteres", MinimumLength = 5)]
    [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Deve ser um e-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Senha é obrigatório")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um dígito e um caractere especial.")]
    public string Password { get; set; }

    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; }


    [Display(Name = "Foto de perfil")]
    [Required(ErrorMessage = "Foto é obrigatória")]
    public string Photo { get; set; }

    public UserType Type { get; set; }
}