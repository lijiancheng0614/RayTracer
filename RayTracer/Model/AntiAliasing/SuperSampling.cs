using System.Collections.Generic;

namespace RayTracer.Model.AntiAliasing
{
    /// <summary>
    /// Supersampling https://en.wikipedia.org/wiki/Supersampling
    /// </summary>
    class SuperSampling
    {
        public delegate Color RayTracingDelegate(double x, double y);

        List<List<double>> weight = new List<List<double>>();
        Dictionary<int, List<double>> ydict = new Dictionary<int, List<double>>(); //for speedup
        double coefficient;
        double finalCoefficient;
        int level;

        public SuperSampling(int level = 1)
        {
            this.level = level;
            if (level > 0)
                coefficient = 0.5 / level;
            else
                coefficient = 0;
            double weightTot = 0;
            for (int i = -level; i <= level; ++i)
            {
                weight.Add(new List<double>());
                for (int j = -level; j <= level; ++j)
                {
                    double w = 1.0 / (i * i + j * j + 1);
                    weight[i + level].Add(w);
                    weightTot += w;
                }
            }
            finalCoefficient = 1.0 / weightTot;
        }

        // Grid Algorithm
        public Color GridSuperSampling(int x, int y, RayTracingDelegate rayTracingDelegate)
        {
            if (!ydict.ContainsKey(y))
            {
                List<double> ylist = new List<double>();
                for (int j = -level; j <= level; ++j)
                {
                    ylist.Add(y + j * coefficient);
                    ydict[y] = ylist;
                }
            }
            Color color = Color.Black;
            for (int i = -level; i <= level; ++i)
                for (int j = -level; j <= level; ++j)
                {
                    color = color + rayTracingDelegate(x + i * coefficient, ydict[y][j + level]) * weight[i + level][j + level];
                }
            color = color * finalCoefficient;
            return color;
        }
    }
}
