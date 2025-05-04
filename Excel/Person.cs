namespace Excel;

public class Person
{
    public string LastName { get; }

    public string FirstName { get; }

    public int Age { get; }

    public string Phone { get; }

    public Person(string firstName, string lastName, int age, string phone)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNullOrEmpty(phone);

        if (age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), $"Возраст ({age}) не может быть меньше или равен нулю");
        }

        if (phone.Length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(phone), $"Длина номера телефона ({phone}) не должна быть меньше или равной 0");
        }

        Age = age;
        LastName = lastName;
        FirstName = firstName;
        Phone = phone;
    }
}