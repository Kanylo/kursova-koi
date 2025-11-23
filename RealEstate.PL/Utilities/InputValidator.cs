using RealEstateApp.BLL.Exceptions;

namespace RealEstateApp.PL.Utilities
{
    public static class InputValidator
    {
        public static string ReadRequiredString(string prompt)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("This field is required. Please try again.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }

            return input.Trim();
        }

        public static decimal ReadDecimal(string prompt, decimal minValue = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal result) && result >= minValue)
                    return result;

                Console.WriteLine($"Please enter a valid number greater than or equal to {minValue}");
            }
        }

        public static int ReadInt(string prompt, int minValue = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result >= minValue)
                    return result;

                Console.WriteLine($"Please enter a valid integer greater than or equal to {minValue}");
            }
        }

        public static T ReadEnum<T>(string prompt) where T : struct, Enum
        {
            while (true)
            {
                Console.WriteLine(prompt);
                var values = Enum.GetValues(typeof(T));

                foreach (var value in values)
                {
                    Console.WriteLine($"{(int)value}. {value}");
                }

                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && Enum.IsDefined(typeof(T), choice))
                    return (T)Enum.ToObject(typeof(T), choice);

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }
}