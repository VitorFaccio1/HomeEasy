using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public sealed class Job
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }
}