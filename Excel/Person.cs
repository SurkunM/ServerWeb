namespace Excel;

public class Person
{
    public string LastName { get; }

    public string FirstName { get; }

    public int Age { get; }

    public int Phone { get; }

    public Person(string firstName, string lastName, int age, int phone)
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
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст не может быть отрицательным числом");
        }

        if (phone <= 0 || phone >= 100000)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Номер телефона не должен быть меньше или равно 0, или больше 100 000");
        }

        Age = age;
        LastName = lastName;
        FirstName = firstName;
        Phone = phone;
    }
}