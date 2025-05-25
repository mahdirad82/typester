using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using TextAnalyzer.Data;

using var dbContext = new SqliteDbContext();

var wordCount = 20;
// Parse command-line args: look for -n followed by a number
for (var j = 0; j < args.Length; j++)
{
    if ((args[j] == "-n" || args[j] == "--number") && j + 1 < args.Length)
    {
        if (int.TryParse(args[j + 1], out var number))
        {
            if (number is > 120 or < 20)
            {
                Console.WriteLine("Number must be between 20 and 120");
                return;
            }

            wordCount = number;
            j++; // Skip the next arg since we already consumed it
        }
        else
        {
            Console.WriteLine("Invalid number after -n.");
            return;
        }
    }
}

Console.CursorVisible = false;

while (true)
{
    var restartRequested = RunTypingTest(dbContext, wordCount);

    if (restartRequested)
    {
        Console.Clear();
        continue; // restart immediately
    }

    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.R && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        continue; // restart on Ctrl+R after test is done

    break;
}

return;

static bool RunTypingTest(SqliteDbContext dbContext, int wordCount)
{
    var wordsPerLine = (int)Math.Ceiling(Math.Sqrt(wordCount));
    var words = dbContext.Words
        .AsNoTracking()
        .Where(w =>
            w.Word.Length >= 3 &&
            w.Word.Length <= 6)
        .OrderBy(w => EF.Functions.Random())
        .Take(wordCount)
        .Select(w => w.Word.ToLower())
        .ToList();


    var text = string.Join(' ',
        words.Select((w, i) =>
            i > 0 && i % wordsPerLine == 0 ? $"\n{w}" : w));


    var charsAndPoss = WriteCenteredAndGetPositions(text);
    var charToCorrectness = new Dictionary<int, bool>();
    for (var j = 0; j < charsAndPoss.Count; j++)
        charToCorrectness[j] = false;

    var stopWatch = new Stopwatch();
    var isStopWatchRunning = false;
    var i = 0;
    var mistakes = 0;

    while (i < charsAndPoss.Count)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition(charsAndPoss[i].Left, charsAndPoss[i].Top);
        Console.Write(charsAndPoss[i].Char);
        ConsoleKeyInfo input;
        while (true)
        {
            input = Console.ReadKey(true);
            if (input.Key == ConsoleKey.R &&
                input.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                return true; // signal to restart
            }

            if (!char.IsLetter(input.KeyChar) && !char.IsWhiteSpace(input.KeyChar) &&
                input.Key != ConsoleKey.Backspace) continue;

            if (!isStopWatchRunning)
            {
                stopWatch = Stopwatch.StartNew();
                isStopWatchRunning = true;
            }

            break; // valid key pressed
        }

        if (input.Key == ConsoleKey.Backspace && i > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(charsAndPoss[i].Left, charsAndPoss[i].Top);
            Console.Write(charsAndPoss[i].Char);
            i--;
            continue;
        }

        Console.SetCursorPosition(charsAndPoss[i].Left, charsAndPoss[i].Top);
        if (input.KeyChar == charsAndPoss[i].Char)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            charToCorrectness[i] = true;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            charToCorrectness[i] = false;
            mistakes++;
        }

        if (char.IsWhiteSpace(charsAndPoss[i].Char))
        {
            if (input.Key != ConsoleKey.Spacebar)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                charToCorrectness[i] = false;
                mistakes++;
            }
            else
            {
                charToCorrectness[i] = true;
            }
        }

        Console.Write(charsAndPoss[i].Char);
        Console.ResetColor();
        i++;
    }

    stopWatch.Stop();
    var corrects = charToCorrectness.Count(w => w.Value);
    
    WriteCenteredAndGetPositions(
        $"""
         Speed: {Math.Round(corrects * 60 / (stopWatch.Elapsed.TotalSeconds * 5), 2)
         }WPM(Word Per Minute)

         Accuracy: {Math.Round((corrects - mistakes) / (double)charsAndPoss.Count * 100, 2)}%

         Mistakes: {mistakes}

         """, animationDelayMs: 20, foregroundColor: ConsoleColor.Cyan);

    return false;
}

static List<(char Char, int Left, int Top)> WriteCenteredAndGetPositions(
    string textToCenter, bool clearConsole = true, bool animate = true,
    int animationDelayMs = 2, ConsoleColor foregroundColor = ConsoleColor.DarkGray)
{
    var positions = new List<(char Char, int Left, int Top)>();

    if (string.IsNullOrWhiteSpace(textToCenter))
        return positions;

    if (clearConsole)
        Console.Clear();

    ShowRestartHint(foregroundColor);

    var lines = textToCenter.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
    var windowWidth = Console.WindowWidth;
    var windowHeight = Console.WindowHeight;
    var startTop = Math.Max(0, (windowHeight - lines.Length) / 2);

    for (var i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        var startLeft = Math.Max(0, (windowWidth - line.Length) / 2);
        var top = startTop + i;

        if (top >= windowHeight)
            break; // No more space

        try
        {
            Console.SetCursorPosition(startLeft, top);

            foreach (var c in line)
            {
                var left = Console.CursorLeft;
                var currentTop = Console.CursorTop;

                if (char.IsLetter(c) || c == ' ')
                    positions.Add((c, left, currentTop));

                Console.Write(c);

                if (animate)
                    Thread.Sleep(animationDelayMs);
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.Error.WriteLine($"\nError writing to console: {ex.Message}");
        }
    }


    // Move cursor below the text
    var finalTop = Math.Min(startTop + lines.Length, windowHeight - 1);
    Console.SetCursorPosition(0, finalTop);

    return positions;
}

static void ShowRestartHint(ConsoleColor foregroundColor)
{
    var bottomRow = Console.WindowHeight - 1;
    Console.SetCursorPosition(0, bottomRow);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write("Press Ctrl + r to restart at any time".PadRight(Console.WindowWidth));
    Console.ForegroundColor = foregroundColor;
}
