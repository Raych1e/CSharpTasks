while (true)
{
    string? input = Console.ReadLine();
    string result;

    if (input == null) return;

    if (input.Length % 2 == 0)
    {
        int position = input.Length / 2;
        string firstSubs = input.Substring(0, position);
        string secondSubs = input.Substring(position);
        firstSubs = new string(firstSubs.Reverse().ToArray());
        secondSubs = new string(secondSubs.Reverse().ToArray());
        result = firstSubs + secondSubs;
    }
    else
    {
        string reversed = new string(input.Reverse().ToArray());
        result = reversed + input;
    };

    Console.WriteLine(result);
}
