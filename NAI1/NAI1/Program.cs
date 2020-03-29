using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NAI1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj k:");
            var k = int.Parse(Console.ReadLine());

            string[] trainingContent = File.ReadAllLines("iris_training.txt");
            string[] testContent = File.ReadAllLines("iris_test.txt");

            var training = new List<Neighbour>();
            var test = new List<Neighbour>();

            for(int i = 0; i < trainingContent.Length; i++)
            {
                training.Add(new Neighbour(trainingContent[i], true));
            }
            for (int i = 0; i < testContent.Length; i++)
            {
                test.Add(new Neighbour(testContent[i], true));
            }
            
            
            int correct = 0;
            foreach(var t in test)
            {
                var distances = new List<Tuple<string, double>>();
                foreach(var n in training)
                {
                    distances.Add(new Tuple<string, double>(n.type, t.CalcDisctance(n)));
                }
                distances = distances.OrderBy(t => t.Item2).ToList();
                    
                var kNearest = new Dictionary<string, int>();

                for (int i = 0; i < k; i++)
                {
                    if (kNearest.ContainsKey(distances[i].Item1))
                    {
                        kNearest[distances[i].Item1]++;
                    }
                    else
                    {
                        kNearest[distances[i].Item1] = 1;
                    }
                }
                    
                string pickedType = kNearest.OrderByDescending(k => k.Value).First().Key;
                int count = kNearest.OrderByDescending(k => k.Value).First().Value;
                
                if (pickedType.Equals(t.type))
                {
                    correct++;
                }
            }
            double percentage = correct / (double) test.Count * 100;
            Console.WriteLine($"Poprawnie dobrano {correct} objektów, co daje {percentage}%");
            Console.WriteLine("Czy chcesz wprowadzić dane? (Y/N):");
            string answer = Console.ReadLine().ToLower().Trim();
            bool loop = answer.Equals("y");
            while (loop)
            {
                Neighbour n = new Neighbour(Console.ReadLine(), false);
                var distances = new List<Tuple<string, double>>();
                foreach (var t in training)
                {
                    distances.Add(new Tuple<string, double>(t.type, t.CalcDisctance(n)));
                }
                var kNearest = new Dictionary<string, int>();

                for (int i = 0; i < k; i++)
                {
                    if (kNearest.ContainsKey(distances[i].Item1))
                    {
                        kNearest[distances[i].Item1]++;
                    }
                    else
                    {
                        kNearest[distances[i].Item1] = 1;
                    }
                }
                string pickedType = kNearest.OrderByDescending(k => k.Value).First().Key;
                int count = kNearest.OrderByDescending(k => k.Value).First().Value;

                Console.WriteLine($"Podany typ to {pickedType}");

                Console.WriteLine("Czy chcesz wprowadzić dane? (Y/N):");
                answer = Console.ReadLine().ToLower().Trim();
                loop = answer.Equals("y");
            }
        }
    }
}
