namespace Excel;

public class Person
{
    public string LastName { get; }

    public string FirstName { get; }

    public int Age { get; }

    public string Phone { get; }

    public Person(string firstName, string lastName, int age, string phone)
    {
        if (firstName is null)
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (lastName is null)
        {
            throw new ArgumentNullException(nameof(lastName));
        }

        if (age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст не может быть меньше или равно нулю");
        }

        if (phone.Length <= 0 || phone.Length >= 100000)
        {
            throw new ArgumentOutOfRangeException(nameof(phone), "Длина номера телефона не должна быть меньше или равной 0, или больше 6 значений");
        }

        Age = age;
        LastName = lastName;
        FirstName = firstName;
        Phone = phone;
    }
}