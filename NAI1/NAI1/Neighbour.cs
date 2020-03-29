using System;
using System.Collections.Generic;
using System.Text;

namespace NAI1
{
    class Neighbour
    {
        List<double> points = new List<double>();
        public string type { get; }
        public Neighbour(string line, bool withType)
        {
            string[] data = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
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

        public double CalcDisctance(Neighbour n)
        {
            double distance = 0;
            for(int i = 0; i < points.Count; i++)
            {
                distance += Math.Pow(this.points[i] - n.points[i], 2);
            }
            return distance;
        }
    }
}
