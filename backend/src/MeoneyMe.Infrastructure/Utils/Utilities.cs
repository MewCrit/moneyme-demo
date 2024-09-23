


namespace MeoneyMe.Infrastructure.Utils;


public static class Utilities
{
    public static string GenerateUrl(string id) => $"http://localhost:3000/confirm/{id}";

    public static bool MustBe18(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue)
        {
            return false;
        }

        var age = DateTime.Today.Year - dateOfBirth.Value.Year;

        if (dateOfBirth.Value > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }
}