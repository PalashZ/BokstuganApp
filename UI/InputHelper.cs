using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using BokstuganApp.Classes;

namespace BokstuganApp.UI
{

    // Denna klassen används för att hantera användarinput i konsolen.
    // Den använder Spectre.Console för att skapa snygga prompts och validera input.
    // Varje metod tar en sträng som parameter och returnerar användarens input i rätt typ.
    public static class InputHelper
    {
        // Frågar användaren om en bekräftelse (ja/nej) och returnerar true eller false.
        public static bool Confirm(string question) => AnsiConsole.Confirm(question);
        public static string AskText(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"{prompt}:")
                    .Validate(text => string.IsNullOrWhiteSpace(text)
                        ? ValidationResult.Error("[red]Fältet kan inte vara tomt.[/]")
                        : ValidationResult.Success()));
        }
        // Användarnamn ska innehålla minst tre bokstäver och bara bokstäver eller siffror.
        public static string AskUserName(string prompt)
        {
            string name = AnsiConsole.Prompt(
                new TextPrompt<string>($"{prompt}:")
                    .Validate(text =>
                    {
                        string name = text.Trim();
                        if (!name.All(character => char.IsLetter(character) || char.IsDigit(character)))
                            return ValidationResult.Error("[red]Användarnamnet får bara innehålla bokstäver och siffror.[/]");
                        if (name.Count(char.IsLetter) < 3)
                            return ValidationResult.Error("[red]Användarnamnet måste innehålla minst 3 bokstäver.[/]");
                        return ValidationResult.Success();
                    }));
            return name.Trim();
        }

        // Frågar användaren om ett heltal inom ett visst intervall och returnerar det.
        public static int AskInt(string prompt, int min, int max)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>($"{prompt}:")
                .ValidationErrorMessage("[red]Skriv ett heltal.[/]")
                .Validate(number => number < min || number > max
                    ? ValidationResult.Error($"[red]Värdet måste vara mellan {min} och {max}.[/]")
                    : ValidationResult.Success()));


        }
    }
}
