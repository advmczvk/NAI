using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nai3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj k:");
            int k = int.Parse(Console.ReadLine());
            var groups = new List<List<Iris>>();
            var centroids = new List<Centroid>();
            for (int i = 0; i < k; i++)
            {
                groups.Add(new List<Iris>());
                centroids.Add(new Centroid(i));
            }
            
            var data = File.ReadAllLines("iris_training.txt");
            Random r = new Random();
            for (int i = 0; i < data.Length; i ++) 
                groups[r.Next(0, k)].Add(new Iris(data[i]));
            
            var groupsTmp = new List<List<Iris>>();
            var centroidsTmp = new List<Centroid>();
            for (int i = 0; i < k; i++)
            {
                groupsTmp.Add(new List<Iris>());
                centroidsTmp.Add(new Centroid(i));
            }
            bool run = true;
            while(run)
            {
                int g = 0;
                foreach(List<Iris> c in groups)
                {
                    Console.WriteLine($"--------GROUP {g} POINTS: {centroids[g].points[0]} {centroids[g].points[1]} {centroids[g].points[2]} {centroids[g].points[3]}--------");
                    g++;
                    foreach(Iris i in c)
                    {
                        Console.WriteLine($"{i.points[0]} {i.points[1]} {i.points[2]} {i.points[3]} {i.type}");
                    }
                }
                foreach (List<Iris> c in groups.ToList())
                {
                    foreach(Iris i in c.ToList())
                    {
                        groupsTmp[NearestGroup(centroids, i)].Add(i);
                    }
                }
                centroidsTmp = centroids;
                Recalculate(centroidsTmp, groups);

                if(groupsTmp.Equals(groups) && centroids.Equals(centroidsTmp))
                {
                    run = false;
                }
                else
                {
                    groups = groupsTmp;
                    centroids = centroidsTmp;
                }
            }
            
        }
        public static int NearestGroup(List<Centroid> centroids, Iris iris)
        {
            double distance = 0, smallest = -1;
            int group = -1;
            foreach(Centroid c in centroids)
            {
                for (int i = 0; i < iris.points.Count; i++)
                {
                    distance += Math.Pow((iris.points[i] - c.points[i]), 2);
                }
                if(smallest < 0 || smallest >= distance)
                {
                    smallest = distance;
                    group = c.group;
                }
            }

            return group;
        }
        public static void Recalculate(List<Centroid> centroids, List<List<Iris>> groups)
        {
            foreach(Centroid c in centroids)
            {
                c.Recalculate(groups[c.group]);
            }
        }
    }
    class Iris
    {
        public string type { get; }
        public List<double> points { get; }
        
        public Iris(string line)
        {
            points = new List<double>();
            var data = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
           
            for (int i = 0; i < data.Length - 1; i++)
            {
                points.Add(double.Parse(data[i]));
            }
            type = data[data.Length - 1];
        }
    }
    class Centroid
    {
        public List<double> points { get; }
        public int group { get; }

        public Centroid(int group)
        {
            this.group = group;
            points = new List<double>();
            var r = new Random();
            for (int i = 0; i < 4; i++)
                points.Add(r.NextDouble() * 10);
        }

        public void Recalculate(List<Iris> irises)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < irises.Count; j++)
                {
                    points[i] += irises[j].points[i];
                }
                points[i] /= irises.Count;
            }
        }
    }
}
