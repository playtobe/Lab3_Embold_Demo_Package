namespace Project3.Views;

public static class Menu
{
    public static int GetChoice(object[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        Console.Write($"Your options from 1 - {options.Length}: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int choice))
        {
            return choice;
        }

        return -1;
    }

    public static int getChoice(object[] options) => GetChoice(options);
}
