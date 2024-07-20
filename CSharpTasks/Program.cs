while (true)
{
    string? input = Console.ReadLine();
    string result;
    bool error = false;

    if (input == null) return;

    foreach (char symbol in input)
    {
        if (symbol < 'a'  ||  symbol > 'z')
        {
            Console.WriteLine("{0} - некорректный символ!", symbol);
            error = true;
        }
    }

    if (error) return;


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

    var unique = result.Distinct();

    for (int i = 0; i < unique.Count(); i++)
    {
        int counter = 0;
        for (int j = 0; j < result.Length; j++)
        {
            if (unique.ToList()[i] == result[j])
            {
                counter++;
            }
        }
        Console.WriteLine("Символ \"{0}\" повторялся {1} раз", unique.ToList()[i], counter);
    }
}
