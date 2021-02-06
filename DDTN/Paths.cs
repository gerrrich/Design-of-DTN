using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ПСПД
{
    static class Paths
    {


        public static (List<byte>[], int) profitable(List<List<byte>[]> all, int[,] load, int[,] graphload, int[] graphnodsload, int n)
        {
            int maxp = 0, maxm = int.MinValue, res;
            List<byte>[] maxpath = null, maxmpath = null;
            foreach (List<byte>[] path in all)
            {
                res = advantage(path, load, graphload, graphnodsload, n);
                if (res > 0 && res > maxp)
                {
                    maxp = res;
                    maxpath = path;
                }
                else if (res < 0 && res > maxm)
                {
                    maxm = res;
                    maxmpath = path;
                }
            }
            if (maxpath != null)
                return (maxpath, maxp);
            else
                return (maxmpath, maxm);
        }

        public static int advantage(List<byte>[] path, int[,] load, int[,] graphload, int[] graphnodsload, int n)
        {
            int plus = 0, add = 0, minus = 0;
            int[,] loading = new int[n, n];
            int[] nodsload = new int[n];
            bool ok = true;

            for (int i = 0; i < n * (n - 1) / 2; i++)
            {
                add = load[path[i][0], path[i].Last()];
                nodsload[path[i][0]] += add;
                for (int j = 1; j < path[i].Count; j++)
                {
                    loading[path[i][j - 1], path[i][j]] += add;
                    loading[path[i][j], path[i][j - 1]] += add;
                    nodsload[path[i][j]] += add;
                }
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (graphload[i, j] > 0)
                    {
                        if (graphload[i, j] < loading[i, j])
                        {
                            minus += graphload[i, j] - loading[i, j];
                            ok = false;
                        }
                        else
                            plus += graphload[i, j] - loading[i, j];
                    }
                }
                if (graphnodsload[i] > 0)
                {
                    if (graphnodsload[i] < nodsload[i])
                    {
                        minus += graphnodsload[i] - nodsload[i];
                        ok = false;
                    }
                    else
                        plus += graphnodsload[i] - nodsload[i];
                }
            }

            if (graphnodsload[n - 1] < nodsload[n - 1])
            {
                minus += graphnodsload[n - 1] - nodsload[n - 1];
                ok = false;
            }
            else
                plus += graphnodsload[n - 1] - nodsload[n - 1];

            if (ok)
                return plus;
            else
                return minus;
        }

        public static List<T[]> allversions<T>(List<T>[] paths, int n)//List<byte> - один путь; List<List<byte>> - все пути из i в j; List<List<byte>>[] - все вариации i и j
        {
            List<T[]> ans = new List<T[]>();

            List<Child<T>> end = new List<Child<T>>();
            foreach (T path in paths[0])
            {
                end.Add(new Child<T>(path, null));
            }
            for (int i = 1; i < n * (n - 1) / 2; i++)
            {
                List<Child<T>> preend = new List<Child<T>>();
                foreach (T path in paths[i])
                {
                    preend.Add(new Child<T>(path, end));
                }
                end = preend;
            }
            List<T[]> allpaths = new List<T[]>();
            helper<T>(end, n, 0, new T[n * (n - 1) / 2], allpaths);
            return allpaths;
        }

        public static void helper<T>(List<Child<T>> end, int n, int k, T[] path, List<T[]> allpaths)
        {
            if (end == null)
            {
                allpaths.Add(path);
                return;
            }
            foreach (Child<T> child in end)
            {
                T[] newpath = new T[n * (n - 1) / 2];
                path.CopyTo(newpath, 0);
                newpath[k] = child.info;
                helper<T>(child.children, n, k + 1, newpath, allpaths);
            }
        }

        public class Child<T>
        {
            public T info;
            public List<Child<T>> children;

            public Child(T inf, List<Child<T>> c)
            {
                info = inf;
                children = c;
            }
        }

        public static List<List<byte>>[] allPaths(List<List<byte>>[][,] R, int n)
        {
            List<List<byte>>[] ans = new List<List<byte>>[n * (n - 1) / 2];
            int k = 0;
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                {
                    ans[k] = new List<List<byte>>();
                    for (int p = 0; p < n - 1; p++)
                        foreach (List<byte> list in R[p][i, j])
                            if (list.Count != 0)
                                ans[k].Add(list);
                    k++;
                }
            return ans;
        }

        public static List<List<byte>>[][,] Circuit(List<byte>[,] a)
        {
            int n = (int)Math.Sqrt(a.Length) - 1;
            List<List<byte>>[][,] ans = new List<List<byte>>[n][,];
            ans[0] = new List<List<byte>>[n + 1, n + 1];
            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < n + 1; j++)
                {
                    ans[0][i, j] = new List<List<byte>>();
                    ans[0][i, j].Add(a[i, j]);
                }
            for (int i = 1; i < n; i++)
                ans[i] = Comp(a, ans[i - 1]);
            return ans;
        }

        public static List<byte>[,] Matrix(int numberV, List<Edge> edges)
        {
            List<byte>[,] ans = new List<byte>[numberV, numberV];
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    ans[i, j] = new List<byte>();

            foreach (Edge edge in edges)
            {
                int min = edge.numa < edge.numb ? edge.numa : edge.numb;
                int max = edge.numa > edge.numb ? edge.numa : edge.numb;
                ans[min, max].Add((byte)min);
                ans[min, max].Add((byte)max);
                ans[max, min].Add((byte)max);
                ans[max, min].Add((byte)min);
            }
            return ans;
        }

        public static List<List<byte>>[,] Comp(List<byte>[,] a, List<List<byte>>[,] b = null)
        {
            byte[] same;
            int n = (int)Math.Sqrt(a.Length);
            List<List<byte>>[,] ans = new List<List<byte>>[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    ans[i, j] = new List<List<byte>>();
            if (b == null)
            {
                b = new List<List<byte>>[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        b[i, j] = new List<List<byte>>();
                        b[i, j].Add(a[i, j]);
                    }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        continue;
                    for (int r = 0; r < n; r++)
                    {
                        if (b[i, r].Count == 0 || b[i, r][0].Count == 0 || a[r, j].Count == 0)
                            continue;
                        foreach (List<byte> p in b[i, r])
                        {
                            same = new byte[n];
                            foreach (byte s in p)
                                same[s]++;
                            same[a[r, j][1]]++;
                            if (same.Contains((byte)2))
                                continue;
                            ans[i, j].Add(new List<byte>(p));
                            ans[i, j][ans[i, j].Count - 1].Add((byte)j);
                        }
                    }
                }
            }
            return ans;
        }
    }
}
