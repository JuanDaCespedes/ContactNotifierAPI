using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Contact
{
    private Contact()
    {
    }

    public Contact(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    [Key]
    public int Id { get; }
    public string Name { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
}