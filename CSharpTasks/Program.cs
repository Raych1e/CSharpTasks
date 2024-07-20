using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;

while (true)
{
    string? input = Console.ReadLine();
    string result;
    bool error = false;

    Console.WriteLine("Введите желаемый метод сортировки: ");
    Console.WriteLine("1 - Быстрая сортировка (QuickSort)");
    Console.WriteLine("2 - Сортировка деревом (Tree sort)");

    int sortMethod;
    try
    {
        sortMethod = int.Parse(Console.ReadLine());
    }
    catch
    {
        Console.WriteLine("Ожидалось: 1 или 2");
        return;
    }

    if (input == null) return;

    foreach (char symbol in input)
    {
        if (symbol < 'a' || symbol > 'z')
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

    int firstIndex = 0;
    int last = 0;

    for (int i = 0; i < result.Length; i++)
    {
        if ("aeiouy".Contains(result[i]))
        {
            firstIndex = i;
            for (int j = i; j < result.Length; j++)
            {
                if ("aeiouy".Contains(result[j]))
                {
                    last = j;
                }
            }
            break;
        }
    }
    string longSubstring = result.Substring(firstIndex, last - firstIndex + 1);
    Console.WriteLine(longSubstring);

    switch (sortMethod)
    {
        case 1:
            {
                Console.WriteLine("Отсортированный результат: {0}", new string(QuickSort(result.ToCharArray(), 0, result.Length - 1)));
                break;
            }
        case 2:
            {
                BinarySearchTree tree = new BinarySearchTree();
                for (int i = 0; i < result.Length; i++)
                {
                    tree.Insert(result[i]);
                }
                Console.Write("Отсортированный результат: ");
                tree.InOrderTraversal();
                break;
            }
    }
    GetRandomNumberAndRemoveSymbol(result.Length, result);
}


static async void GetRandomNumberAndRemoveSymbol(int max, string symbols)
{
    int randomNumber;
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage responseMessage = await client.GetAsync($"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={max}&count=1");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            responseBody = responseBody.Remove(0, 1);
            responseBody = responseBody.Remove(responseBody.Length - 2, 1);
            randomNumber = int.Parse(responseBody);
        }
        else
        {
            Random random = new Random();
            randomNumber = random.Next(0, max);
        }
    }
    symbols = symbols.Remove(randomNumber, 1);
    Console.WriteLine("Удален символ на позиции {0}: {1}", randomNumber, symbols);
}


static void Swap(char[] array, int i, int j)
{
    char temp = array[i];
    array[i] = array[j];
    array[j] = temp;
}

static int Partition(char[] array, int low, int high)
{
    int pivot = array[high];
    int i = low - 1;

    for (int j = low; j < high; j++)
    {
        if (array[j] <= pivot)
        {
            i++;
            Swap(array, i, j);
        }
    }

    Swap(array, i + 1, high);
    return i + 1;
}


static char[] QuickSort(char[] array, int low, int high)
{
    if (low < high)
    {
        int partitionIndex = Partition(array, low, high);
        QuickSort(array, low, partitionIndex - 1);
        QuickSort(array, partitionIndex + 1, high);
    }
    return array;
}

public class BinarySearchTree
{
    private class Node
    {
        public char data;
        public Node left;
        public Node right;

        public Node(char data)
        {
            this.data = data;
            left = null;
            right = null;
        }
    }

    private Node root;

    public BinarySearchTree()
    {
        root = null;
    }

    public void Insert(char data)
    {
        root = Insert(root, data);
    }

    private Node Insert(Node node, char data)
    {
        if (node == null)
        {
            return new Node(data);
        }

        if (data < node.data)
        {
            node.left = Insert(node.left, data);
        }
        else if (data > node.data)
        {
            node.right = Insert(node.right, data);
        }
        else
        {
            node.right = Insert(node.right, data);
        }

        return node;
    }

    public void InOrderTraversal()
    {
        InOrderTraversal(root);
    }

    private void InOrderTraversal(Node node)
    {
        if (node != null)
        {
            InOrderTraversal(node.left);
            Console.Write(node.data + " ");
            InOrderTraversal(node.right);
        }
    }
}