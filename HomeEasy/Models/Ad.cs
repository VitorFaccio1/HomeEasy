using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public sealed class Ad
{
    [Key]
    public Guid Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "Título é obrigatório")]
    public string Title { get; set; }

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "Descrição é obrigatório")]
    public string Description { get; set; }

    [Display(Name = "Trabalho")]
    [Required(ErrorMessage = "Selecione pelo menos um trabalho")]
    public string Job { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public User? User { get; set; }
}