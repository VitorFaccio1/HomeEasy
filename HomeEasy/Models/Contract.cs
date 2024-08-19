using System.ComponentModel.DataAnnotations;

namespace HomeEasy.Models;

public class Contract
{
    public Contract()
    { }

    public Contract(Ad ad, User? contractor)
    {
        Id = Guid.NewGuid();
        Ad = ad;
        Contractee = ad.User;
        Contractor = contractor;
        Date = DateTime.Now;
    }

    [Key]
    public Guid Id { get; set; }

    public Ad Ad { get; set; }

    public User Contractor { get; set; }

    public User Contractee { get; set; }

    public bool Approved { get; set; }

    public DateTime Date { get; set; }

    public bool Completed { get; set; }

    public bool ContracteeReviewed { get; set; }

    public bool ContractorReviewed { get; set; }
}