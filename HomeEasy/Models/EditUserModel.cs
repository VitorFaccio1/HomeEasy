﻿using HomeEasy.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public class EditUserModel
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "Email é obrigatório")]
    [StringLength(50, ErrorMessage = "Deve ter entre 5 e 50 caracteres", MinimumLength = 5)]
    [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Deve ser um e-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; }

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(19, ErrorMessage = "O telefone deve ter no máximo 19 caracteres")]
    [RegularExpression(@"^\+55 \(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "O número de telefone deve estar no formato +55 (11) 99999-9999.")]
    public string Phone { get; set; }

    [Display(Name = "País")]
    [Required(ErrorMessage = "País é obrigatório")]
    [StringLength(50, ErrorMessage = "O país deve ter no máximo 50 caracteres")]
    public string Country { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "Estado é obrigatório")]
    [StringLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres")]
    public string State { get; set; }

    [Display(Name = "Gênero")]
    [Required(ErrorMessage = "Gênero é obrigatório")]
    [StringLength(10, ErrorMessage = "O gênero deve ter no máximo 10 caracteres")]
    public string Gender { get; set; }

    [Display(Name = "Cidade")]
    [Required(ErrorMessage = "Cidade é obrigatória")]
    [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres")]
    public string City { get; set; }

    [Display(Name = "Data de nascimento")]
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    [DataType(DataType.Date, ErrorMessage = "Data inválida")]
    public DateOnly DateOfBirth { get; set; }
}
