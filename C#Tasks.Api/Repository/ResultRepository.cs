using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace C_Tasks.Api.Repository
{
    public class ResultRepository
    {
        private readonly IConfiguration _configuration;
        public ResultRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> GetBlacklist()
        {
            return _configuration.GetSection("Settings:Blacklist").Get<List<string>>();
        }

        [HttpGet]
        public (string, List<string>) GetResult(string input)
        {
            string result = "";
            List<string> errors = new List<string>();

            var blacklist = GetBlacklist();
            foreach (string item in blacklist)
            {
                if (input == item)
                {
                    errors.Add($"Строка {item} входит в список запрещенных слов");
                }
            }

            foreach (char symbol in input)
            {
                if (symbol < 'a' || symbol > 'z')
                {
                    errors.Add($"Символ {symbol} некорректен");
                }
            } 

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

            return (result, errors);
        }

        [HttpGet]
        public List<string> GetUnique(string result) 
        {
            List<string> uniqueSymbols = new List<string>();

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
                uniqueSymbols.Add(new string($"Символ \"{unique.ToList()[i]}\" повторялся {counter} раз"));
            }


            return uniqueSymbols;
        }

        [HttpGet]
        public string GetSubstring(string result) 
        {
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
            return longSubstring;
        }

        [HttpGet]
        public string Sorted(int method, string result)
        {
            switch (method)
            {
                case 1:
                    {
                        return($"Отсортированный результат: {new string(QuickSort(result.ToCharArray(), 0, result.Length - 1))}");
                    }
                case 2:
                    {
                        BinarySearchTree tree = new BinarySearchTree();
                        for (int i = 0; i < result.Length; i++)
                        {
                            tree.Insert(result[i]);
                        }
                        return ($"Отсортированный результат: {tree.InOrderTraversal()}");
                    }
                default:
                    {
                        return ("Некорректный метод сортировки!");
                    }
            }
        }

        [HttpGet]
        public string GetStringWithDeletedSymbol(string result) 
        {
            return (GetRandomNumberAndRemoveSymbol(result.Length, result).Result);
        }

        static async Task<string> GetRandomNumberAndRemoveSymbol(int max, string symbols)
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
            return($"Удален символ на позиции {randomNumber}: {symbols}");
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

            public string InOrderTraversal()
            {
                return InOrderTraversal(root);
            }

            private string InOrderTraversal(Node node)
            {
                if (node == null)
                {
                    return "";
                }

                string result = "";

                result += InOrderTraversal(node.left);

                result += node.data.ToString();

                result += InOrderTraversal(node.right);

                return result;
            }
        }
    }
}
