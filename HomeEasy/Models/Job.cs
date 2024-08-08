using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public sealed class Job
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [Display(Name = "Nome")]
    public string Name { get; set; }
}