using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ПСПД
{
    class IncidenceTable
    {

    }

    public class Node
    {
        public int num;
        public int color;
        public Point point;
        public int cost;
        public int speed;
        public string name;

        public Node(int nu, int col, Point po)
        {
            num = nu;
            color = col;
            point = po;
        }
    }

    public class Edge
    {
        public Point a;
        public Point b;
        public int numa;
        public int numb;
        public int color;
        public int cost;
        public int speed;
        public string name;
    }

    public static class Graphs
    {
        public static List<Tuple<List<Node>, List<Edge>>> GetGraphs(List<Node> nodes, List<Edge> edges)
        {
            List<Tuple<List<Node>, List<Edge>>> output = new List<Tuple<List<Node>, List<Edge>>>();

            List<Node> originNodes = new List<Node>();
            List<Node> blueNodes = new List<Node>();
            List<List<Node>> allNodes = new List<List<Node>>();
            allNodes.Add(new List<Node>());

            foreach (Node node in nodes)
            {
                if (node.color == 1 || node.color == 2)
                    originNodes.Add(node);
                else
                    blueNodes.Add(node);
            }

            if (blueNodes.Count > 1)
            {
                allNodes.Add(new List<Node>(blueNodes));
                GetBlues(blueNodes, 0, allNodes);
            }
            if (blueNodes.Count == 1)
            {
                allNodes.Add(new List<Node>(blueNodes));
            }
            //
            //
            List<Edge> originEdges = new List<Edge>();
            List<Edge> blueEdges = new List<Edge>();
            List<List<Edge>> allEdges = new List<List<Edge>>();
            allEdges.Add(new List<Edge>());

            foreach (Edge edge in edges)
            {
                if (edge.color == 1 || edge.color == 2)
                    originEdges.Add(edge);
                else
                    blueEdges.Add(edge);
            }
            if (blueEdges.Count > 1)
            {
                allEdges.Add(new List<Edge>(blueEdges));
                GetBlues(blueEdges, 0, allEdges);
            }
            if (blueEdges.Count == 1)
            {
                allEdges.Add(new List<Edge>(blueEdges));
            }
            //
            //
            for (int i = 0; i < allNodes.Count; i++)
                allNodes[i].AddRange(originNodes);
            for (int i = 0; i < allEdges.Count; i++)
                allEdges[i].AddRange(originEdges);

            for (int i = 0; i < allNodes.Count; i++)
                for (int j = 0; j < allEdges.Count; j++)
                    if (GoodGraph(allNodes[i], allEdges[j]))
                        output.Add(new Tuple<List<Node>, List<Edge>>(allNodes[i], allEdges[j]));
            //
            //
            return output;
        }

        private static void GetBlues<T>(List<T> blues, int j, List<List<T>> all)
        {
            int n = blues.Count;
            if (n == 2)
            {
                if (j == 0)
                {
                    List<T> newBlues1 = new List<T>();
                    newBlues1.Add(blues[0]);
                    all.Add(newBlues1);
                }
                List<T> newBlues2 = new List<T>();
                newBlues2.Add(blues[1]);
                all.Add(newBlues2);
                return;
            }
            for (int i = j; i < n - 1; i++)
            {
                List<T> newBlues = new List<T>(blues);
                newBlues.RemoveAt(n - 1 - i);
                all.Add(newBlues);
                GetBlues(newBlues, i, all);
            }
            List<T> newBlues0 = new List<T>(blues);
            newBlues0.RemoveAt(0);
            all.Add(newBlues0);
        }


        private static bool GoodGraph(List<Node> nodes, List<Edge> edges)
        {
            List<Tuple<int, int>> a = new List<Tuple<int, int>>();
            for (int i = 0; i < nodes.Count; i++)
                a.Add(new Tuple<int, int>(nodes[i].num, i));

            List<Tuple<int, int>> b = new List<Tuple<int, int>>();
            for (int i = 0; i < nodes.Count; i++)
                b.Add(new Tuple<int, int>(nodes[i].num, 0));
            int cur = -1, cur2 = -1;

            foreach (Edge edge in edges)
            {
                bool temp = false;
                foreach (Node node in nodes)
                {
                    if (node.num == edge.numa)
                        temp = true;
                }
                if (!temp)
                    return false;

                temp = false;
                foreach (Node node in nodes)
                {
                    if (node.num == edge.numb)
                        temp = true;
                }
                if (!temp)
                    return false;

                for (int i = 0; i < b.Count; i++)
                {
                    if (b[i].Item1 == edge.numa)
                        b[i] = new Tuple<int, int>(b[i].Item1, b[i].Item2 + 1);
                    if (b[i].Item1 == edge.numb)
                        b[i] = new Tuple<int, int>(b[i].Item1, b[i].Item2 + 1);
                }

                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].Item1 == edge.numb)
                        cur = a[i].Item2;
                    if (a[i].Item1 == edge.numa)
                        cur2 = a[i].Item2;
                }
                for (int i = 0; i < a.Count; i++)
                    if (cur == a[i].Item2)
                        a[i] = new Tuple<int, int>(a[i].Item1, cur2);
            }
            cur = a[0].Item2;
            for (int i = 1; i < a.Count; i++)
                if (a[i].Item2 != cur)
                    return false;
            for (int i = 0; i < b.Count; i++)
                if (b[i].Item2 < 2)
                    return false;
            return true;
        }
    }
}
