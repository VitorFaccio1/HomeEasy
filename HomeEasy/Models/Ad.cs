using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public sealed class Ad
{
    [Key]
    public Guid Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(50, ErrorMessage = "Título deve ter entre 5 e 50 caracteres", MinimumLength = 5)]
    public string Title { get; set; }

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "Descrição é obrigatório")]
    [StringLength(500, ErrorMessage = "Descrição deve ter entre 5 e 500 caracteres", MinimumLength = 5)]
    public string Description { get; set; }

    [Display(Name = "Trabalho")]
    [Required(ErrorMessage = "Selecione pelo menos um trabalho")]
    public string Job { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public User? User { get; set; }
}