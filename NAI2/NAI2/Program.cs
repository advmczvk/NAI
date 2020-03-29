using System;
using System.Collections.Generic;
using System.IO;

namespace NAI2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] trainingContent = File.ReadAllLines("iris_training.txt");
            string[] testContent = File.ReadAllLines("iris_test.txt");

            var training = new List<Point>();
            var test = new List<Point>();

            for (int i = 0; i < trainingContent.Length; i++)
            {
                training.Add(new Point(trainingContent[i], true));
            }
            for (int i = 0; i < testContent.Length; i++)
            {
                test.Add(new Point(testContent[i], true));
            }

            var perceptron = new Perceptron(training[0].points.Count);
            perceptron.StartLearning(training, 10);

            perceptron.RunTest(test);

            Console.WriteLine("Czy chcesz wprowadzić dane? (Y/N):");
            string answer = Console.ReadLine().ToLower().Trim();
            bool loop = answer.Equals("y");
            while (loop)
            {
                Console.WriteLine("Podaj dane: ");
                string input = Console.ReadLine();
                
                var p = new Point(input, false);

                string pickedType = perceptron.Sum(p) == 1 ? "Iris-setosa" : "nie Iris-setosa";

                Console.WriteLine($"Podany typ to {pickedType}");

                Console.WriteLine("Czy chcesz wprowadzić dane? (Y/N):");
                answer = Console.ReadLine().ToLower().Trim();
                loop = answer.Equals("y");
            }
        }
    }
    class Point
    {
        public string type { get; }
        public List<double> points { get; }

        public Point(string line, bool withType)
        {
            points = new List<double>();
            string[] data = line.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (withType)
            {
                for (int i = 0; i < data.Length - 1; i++)
                {
                    points.Add(double.Parse(data[i]));
                }
                type = data[data.Length - 1];
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    points.Add(double.Parse(data[i]));
                }
            }
        }
    }
    class Perceptron
    {
        public double thetha, learningConst;
        public List<double> weights;

        public Perceptron(int pointsCount)
        {
            weights = new List<double>();
            thetha = 1;
            learningConst = 0.8;
            for(int i = 0; i < pointsCount; i++)
            {
                weights.Add(new Random().NextDouble());
            }
        }
        public void StartLearning(List<Point> points, int accuracy)
        {
            for(int i = 0; i < accuracy; i++)
            {
                for(int j = 0; j < points.Count; j++)
                {
                    Delta(points[j]);
                }
            }
        }
        public void Delta(Point point)
        {
            double sum = Sum(point);
            if(point.type == "Iris-setosa")
            {
                
                while(sum == 0)
                {
                    for(int i = 0; i < weights.Count; i++)
                    {
                        weights[i] += (1 - sum) * learningConst * point.points[i];
                    }
                    thetha += (1 - sum) * learningConst;
                    sum = Sum(point);
                }
            }
            else
            {
                while(sum == 1)
                {
                    for(int i = 0; i < weights.Count; i++)
                    {
                        weights[i] += -sum * learningConst * point.points[i];
                    }
                    thetha += - sum * learningConst;
                    sum = Sum(point);
                }
            }
        }
        public double Sum(Point point)
        {
            double sum = 0;
            for(int i = 0; i < point.points.Count; i++)
            {
                sum += point.points[i] * weights[i];
            }
            return sum >= thetha ? 1 : 0;
        }
        public void RunTest(List<Point> test)
        {
            int correct = 0;
            var points = new List<Tuple<string, Point>>();
            foreach(Point p in test)
            {
                points.Add(new Tuple<string, Point>(Sum(p) == 1 ? "Iris-setosa" : "inny", p));
            }
            for(int i = 0; i < test.Count; i++)
            {
                if (test[i].type == points[i].Item1 && test[i].type == "Iris-setosa" || test[i].type != points[i].Item1 && test[i].type != "Iris-setosa") 
                    correct++;
            }
            double percentage = correct / (double)test.Count * 100;
            Console.WriteLine($"Poprawnie dobrano {correct} objektów, co daje {percentage}%");
        }
    }
}
