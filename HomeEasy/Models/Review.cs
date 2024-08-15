using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public sealed class Review
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "A avaliação é obrigatória.")]
    [Range(1, 5, ErrorMessage = "A avaliação deve estar entre 1 e 5.")]
    public int Rating { get; set; }

    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "O comentário é obrigatório.")]
    [StringLength(500, ErrorMessage = "O comentário deve ter no máximo 500 caracteres.")]
    public string Comment { get; set; }

    public User? Reviewer { get; set; }
}