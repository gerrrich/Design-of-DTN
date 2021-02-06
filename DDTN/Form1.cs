using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ПСПД
{
    public partial class Form1 : Form
    {
        public List<Edge> edges;
        public List<Node> nodes;
        Point first = new Point(-1, -1);
        int mode = 0;
        int count = 0;
        Graphics g;
        int[,] grid = new int[30, 61];
        Font font = new Font(DefaultFont.FontFamily, 13);
        Font font2 = new Font(DefaultFont.FontFamily, 9);
        public bool avto;
        public int pos = 0;
        public List<Tuple<List<Node>, List<Edge>>> graphs;
        public int[,] load;

        Form3 form3;
        Form4 form4;
        Form5 form5;
        Form6 form6;

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 30; i++)
                for (int j = 0; j < 61; j++)
                    grid[i, j] = 0;
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.SetClip(new Rectangle(20, 30, 1221, 631));
            menuStrip1.Items[2].Visible = false;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;

            y = y - y % 20 - 60;
            x = x - x % 20 - 20;

            if (!(x <= 1200 && x >= 0 && y >= 0 && y <= 580) && mode < 2)
            {
                return;
            }

            if (mode == 0)
            {
                if (grid[y / 20, x / 20] == 0)
                {
                    count++;
                    grid[y / 20, x / 20] = 1;
                    g.FillEllipse(SB(1), x + 20, y + 60, 20, 20);
                }
                else if (grid[y / 20, x / 20] == 1)
                {
                    grid[y / 20, x / 20] = 2;
                    g.FillEllipse(SB(2), x + 20, y + 60, 20, 20);
                }
                else if (grid[y / 20, x / 20] == 2)
                {
                    grid[y / 20, x / 20] = 3;
                    g.FillEllipse(SB(3), x + 20, y + 60, 20, 20);
                }
                else if (grid[y / 20, x / 20] == 3)
                {
                    count--;
                    grid[y / 20, x / 20] = 0;
                    g.FillEllipse(SB(0), x + 20, y + 60, 20, 20);
                }

                if (count > 2)
                    menuStrip1.Items[2].Visible = true;
                else
                    menuStrip1.Items[2].Visible = false;

            }
            else if (mode == 1)
            {
                if (first.X == -1 && grid[y / 20, x / 20] != 0)
                {
                    g.DrawEllipse(new Pen(Color.Gold, 3), x - 10 + 20, y - 10 + 60, 40, 40);
                    first.X = x;
                    first.Y = y;
                }
                else if (first.X == x && first.Y == y)
                {
                    g.DrawEllipse(new Pen(Color.WhiteSmoke, 3), x - 10 + 20, y - 10 + 60, 40, 40);
                    first.X = -1;
                    first.Y = -1;
                }
                else if (first.X != -1 && grid[y / 20, x / 20] != 0)
                {
                    bool find = false;
                    for (int i = 0; i < edges.Count; i++)
                    {
                        if (edges[i].a.X == x + 10 + 20 && edges[i].a.Y == y + 10 + 60 && edges[i].b.X == first.X + 10 + 20 && edges[i].b.Y == first.Y + 10 + 60 ||
                            edges[i].b.X == x + 10 + 20 && edges[i].b.Y == y + 10 + 60 && edges[i].a.X == first.X + 10 + 20 && edges[i].a.Y == first.Y + 10 + 60)
                        {
                            find = true;
                            if (edges[i].color == 3)
                            {
                                edges.Remove(edges[i]);
                                g.DrawLine(new Pen(Color.WhiteSmoke, 4), x + 20 + 10, y + 60 + 10, first.X + 20 + 10, first.Y + 60 + 10);
                                g.DrawLine(new Pen(Color.Gray, 1), x + 20 + 10, y + 60 + 10, first.X + 20 + 10, first.Y + 60 + 10);
                            }
                            else
                            {
                                g.DrawLine(new Pen(SB(edges[i].color + 1), 4), x + 20 + 10, y + 60 + 10, first.X + 20 + 10, first.Y + 60 + 10);
                                edges[i].color++;
                            }
                        }
                    }
                    if (!find)
                    {
                        int nua = -1, nub = -1;
                        foreach (Node no in nodes)
                        {
                            if (no.point.Equals(new Point(x + 10 + 20, y + 10 + 60)))
                                nua = no.num;
                            if (no.point.Equals(new Point(first.X + 10 + 20, first.Y + 10 + 60)))
                                nub = no.num;
                        }
                        edges.Add(new Edge()
                        {
                            numa = nua,
                            numb = nub,
                            a = new Point(x + 10 + 20, y + 10 + 60),
                            b = new Point(first.X + 10 + 20, first.Y + 10 + 60),
                            color = 1
                        });
                        g.DrawLine(new Pen(SB(1), 4), x + 20 + 10, y + 60 + 10, first.X + 20 + 10, first.Y + 60 + 10);
                    }


                    g.FillEllipse(SB(grid[y / 20, x / 20]), x + 20, y + 60, 20, 20);
                    g.FillEllipse(SB(grid[first.Y / 20, first.X / 20]), first.X + 20, first.Y + 60, 20, 20);
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i].point.Equals(new Point(x + 10 + 20, y + 10 + 60)))
                        {
                            if (nodes[i].num < 10)
                                g.DrawString(nodes[i].num + "", font, new SolidBrush(Color.White), nodes[i].point.X - 7, nodes[i].point.Y - 10);
                            else
                                g.DrawString(nodes[i].num + "", font2, new SolidBrush(Color.White), nodes[i].point.X - 9, nodes[i].point.Y - 8);
                        }
                        if (nodes[i].point.Equals(new Point(first.X + 10 + 20, first.Y + 10 + 60)))
                        {
                            if (nodes[i].num < 10)
                                g.DrawString(nodes[i].num + "", font, new SolidBrush(Color.White), nodes[i].point.X - 7, nodes[i].point.Y - 10);
                            else
                                g.DrawString(nodes[i].num + "", font2, new SolidBrush(Color.White), nodes[i].point.X - 9, nodes[i].point.Y - 8);
                        }
                    }

                    g.DrawEllipse(new Pen(Color.WhiteSmoke, 3), first.X - 10 + 20, first.Y - 10 + 60, 40, 40);

                    first.X = -1;
                    first.Y = -1;
                }
            }
            else if (mode == 2 && !avto)
            {
                x = e.Location.X;
                y = e.Location.Y;

                if (x >= 30 && x <= 260 && y >= 26 && y <= 54 && pos > 0)
                {
                    g.Clear(Color.WhiteSmoke);
                    for (int i = 0; i < 62; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20 + i * 20, 60, 20 + i * 20, 660);

                    for (int i = 0; i < 31; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

                    if (pos > 1)
                        g.FillRectangle(SB(2), 30, 25, 230, 28);
                    else
                        g.FillRectangle(SB(1), 30, 25, 230, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 30, 26, 230, 28);
                    g.DrawString("< Предыдущая топология", font, new SolidBrush(Color.White), 40, 30);


                    g.FillRectangle(SB(2), 280, 25, 210, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 280, 26, 210, 28);
                    g.DrawString("> Следующая топология", font, new SolidBrush(Color.White), 285, 30);
                    pos--;
                    DrawGraph(graphs[pos].Item1, graphs[pos].Item2, g);


                }
                if (x >= 280 && x <= 490 && y >= 26 && y <= 54 && pos < graphs.Count - 1)
                {
                    g.Clear(Color.WhiteSmoke);
                    for (int i = 0; i < 62; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20 + i * 20, 60, 20 + i * 20, 660);

                    for (int i = 0; i < 31; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

                    g.FillRectangle(SB(2), 30, 25, 230, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 30, 26, 230, 28);
                    g.DrawString("< Предыдущая топология", font, new SolidBrush(Color.White), 40, 30);

                    if (pos < graphs.Count - 2)
                        g.FillRectangle(SB(2), 280, 25, 210, 28);
                    else
                        g.FillRectangle(SB(1), 280, 25, 210, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 280, 26, 210, 28);
                    g.DrawString("> Следующая топология", font, new SolidBrush(Color.White), 285, 30);
                    pos++;
                    DrawGraph(graphs[pos].Item1, graphs[pos].Item2, g);

                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void создатьНовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edges = new List<Edge>();
            nodes = new List<Node>();
            first = new Point(-1, -1);
            mode = 0;
            count = 0;

            menuStrip1.Items[2].Text = "Закончить расстановку узлов";
            menuStrip1.Items[2].Visible = false;

            g.Clear(Color.WhiteSmoke);
            for (int i = 0; i < 30; i++)
                for (int j = 0; j < 61; j++)
                    grid[i, j] = 0;

            for (int i = 0; i < 62; i++)
                g.DrawLine(new Pen(Color.Gray, 1), 20 + i * 20, 60, 20 + i * 20, 660);

            for (int i = 0; i < 31; i++)
                g.DrawLine(new Pen(Color.Gray, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

            g.FillEllipse(new SolidBrush(Color.Red), 20, 30, 20, 20);
            g.DrawString(" - Уже существующие узлы", font, new SolidBrush(Color.Black), 40, 30);

            g.FillEllipse(new SolidBrush(Color.Green), 300, 30, 20, 20);
            g.DrawString(" - Обязательные узлы", font, new SolidBrush(Color.Black), 320, 30);

            g.FillEllipse(new SolidBrush(Color.Blue), 540, 30, 20, 20);
            g.DrawString(" - Необязательные узлы", font, new SolidBrush(Color.Black), 560, 30);

            g.DrawString("Для смены цвета, щелкните по узлу еще раз.", font, new SolidBrush(Color.Black), 865, 30);
        }

        private void закончитьРасстановкуУзловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mode == 0)
            {
                menuStrip1.Items[2].Text = "Закончить расстановку рёбер";

                edges = new List<Edge>();
                nodes = new List<Node>();

                mode = 1;
                //menuStrip1.Items[2].Visible = false;

                g.FillRectangle(SB(0), 20, 30, 1300, 20);

                g.FillEllipse(new SolidBrush(Color.Red), 20, 30, 20, 20);
                g.DrawString(" - Уже существующие ребра", font, new SolidBrush(Color.Black), 40, 30);

                g.FillEllipse(new SolidBrush(Color.Green), 300, 30, 20, 20);
                g.DrawString(" - Обязательные ребра", font, new SolidBrush(Color.Black), 320, 30);

                g.FillEllipse(new SolidBrush(Color.Blue), 540, 30, 20, 20);
                g.DrawString(" - Необязательные ребра", font, new SolidBrush(Color.Black), 560, 30);

                g.DrawString("Для смены цвета, выберите ребра еще раз.", font, new SolidBrush(Color.Black), 865, 30);

                for (int i = 0; i < 62; i++)
                    g.DrawLine(new Pen(Color.WhiteSmoke, 1), 20 + i * 20, 60, 20 + i * 20, 660);

                for (int i = 0; i < 31; i++)
                    g.DrawLine(new Pen(Color.WhiteSmoke, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

                Stack<Point> ts = new Stack<Point>();
                for (int i = 0; i < 30; i++)
                    for (int j = 0; j < 61; j++)
                        if (grid[i, j] != 0)
                        {
                            nodes.Add(new Node(nodes.Count, grid[i, j], new Point(j * 20 + 10 + 20, i * 20 + 10 + 60)));
                            ts.Push(new Point(j * 20 + 10, i * 20 + 10));
                        }

                while (ts.Count != 0)
                {
                    Point point = ts.Pop();
                    Point[] points = ts.ToArray();
                    for (int i = 0; i < ts.Count; i++)
                    {
                        g.DrawLine(new Pen(Color.Gray, 1), point.X + 20, point.Y + 60, points[i].X + 20, points[i].Y + 60);
                    }
                }

                for (int i = 0; i < 30; i++)
                    for (int j = 0; j < 61; j++)
                    {
                        if (grid[i, j] != 0)
                            g.FillEllipse(SB(grid[i, j]), j * 20 + 20, i * 20 + 60, 20, 20);
                    }

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].num < 10)
                        g.DrawString(nodes[i].num + "", font, new SolidBrush(Color.White), nodes[i].point.X - 7, nodes[i].point.Y - 10);
                    else
                        g.DrawString(nodes[i].num + "", font2, new SolidBrush(Color.White), nodes[i].point.X - 9, nodes[i].point.Y - 8);
                }
            }
            else if (mode == 1)
            {
                form3 = new Form3(this);
                form3.ShowDialog();
                form4 = new Form4(this);
                form4.ShowDialog();

                graphs = Graphs.GetGraphs(nodes, edges);

                if (graphs.Count == 0)
                {
                    g.Clear(Color.WhiteSmoke);
                    g.DrawString("ERROR:\nВходные данные не удовлетворяют условию нормального постороения СПД", font, new SolidBrush(Color.Red), 50, 50);
                    return;
                }

                form5 = new Form5(this);
                form5.ShowDialog();
                mode = 2;
                if (avto)
                {
                    long cost, mincost = long.MaxValue;
                    Tuple<List<Node>, List<Edge>> mingraph = new Tuple<List<Node>, List<Edge>>(new List<Node>(), new List<Edge>());
                    foreach (Tuple<List<Node>, List<Edge>> graph in graphs)
                    {
                        cost = 0;
                        foreach (Node node in graph.Item1)
                            cost += node.cost;
                        foreach (Edge edge in graph.Item2)
                            cost += edge.cost;
                        if (cost < mincost)
                        {
                            mincost = cost;
                            mingraph = graph;
                        }
                    }
                    menuStrip1.Items[2].Text = "Выбрать эту топологию";

                    g.Clear(Color.WhiteSmoke);
                    for (int i = 0; i < 62; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20 + i * 20, 60, 20 + i * 20, 660);

                    for (int i = 0; i < 31; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

                    DrawGraph(mingraph.Item1, mingraph.Item2, g);

                    g.DrawString("Стоимость оборудования: " + mincost, font, new SolidBrush(Color.Black), 500, 30);
                }
                else
                {
                    menuStrip1.Items[2].Text = "Выбрать эту топологию";

                    g.Clear(Color.WhiteSmoke);
                    for (int i = 0; i < 62; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20 + i * 20, 60, 20 + i * 20, 660);

                    for (int i = 0; i < 31; i++)
                        g.DrawLine(new Pen(Color.Gray, 1), 20, 60 + i * 20, 1240, 60 + i * 20);

                    g.FillRectangle(SB(1), 30, 25, 230, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 30, 26, 230, 28);
                    g.DrawString("< Предыдущая топология", font, new SolidBrush(Color.White), 40, 30);

                    g.FillRectangle(SB(2), 280, 25, 210, 28);
                    g.DrawRectangle(new Pen(Color.Black, 3), 280, 26, 210, 28);
                    g.DrawString("> Следующая топология", font, new SolidBrush(Color.White), 285, 30);

                    DrawGraph(graphs[0].Item1, graphs[0].Item2, g);

                }
            }
            else if (mode == 2)
            {
                int n = graphs[pos].Item1.Count, nn = nodes.Count;
                for (int j = 0; j < n; j++)
                {
                    int v = graphs[pos].Item1[j].num;
                    graphs[pos].Item1[j].num = j + nn;
                    for (int i = 0; i < graphs[pos].Item2.Count; i++)
                    {
                        if (v == graphs[pos].Item2[i].numa)
                            graphs[pos].Item2[i].numa = j + nn;
                        if (v == graphs[pos].Item2[i].numb)
                            graphs[pos].Item2[i].numb = j + nn;
                    }
                }
                for (int j = 0; j < n; j++)
                    graphs[pos].Item1[j].num -= nn;
                for (int j = 0; j < graphs[pos].Item2.Count; j++)
                {
                    graphs[pos].Item2[j].numa -= nn;
                    graphs[pos].Item2[j].numb -= nn;
                }

                g.Clear(Color.WhiteSmoke);
                //menuStrip1.Items[2].Text = "Выбрать эту топологию";

                load = new int[n, n];
                form6 = new Form6(this);
                form6.ShowDialog();



                int[] grnoload = new int[n];
                int[,] graphload = new int[n, n];

                for (int i = 0; i < n; i++)
                    grnoload[graphs[pos].Item1[i].num] = graphs[pos].Item1[i].speed;

                foreach (Edge edge in graphs[pos].Item2)
                {
                    graphload[edge.numa, edge.numb] = edge.speed;
                    graphload[edge.numb, edge.numa] = edge.speed;
                }


                List<List<byte>[]> all = Paths.allversions<List<byte>>(Paths.allPaths(Paths.Circuit(Paths.Matrix(n, graphs[pos].Item2)), n), n);
                List<byte>[] best;
                int pro;
                (best, pro) = Paths.profitable(all, load, graphload, grnoload, n);

                foreach (Edge edge in graphs[pos].Item2)
                    g.DrawLine(new Pen(Color.RoyalBlue, 3), edge.a.X / 2 + 20, edge.a.Y, edge.b.X / 2 + 20, edge.b.Y);

                foreach (Node node in graphs[pos].Item1)
                {
                    g.FillEllipse(new SolidBrush(Color.Fuchsia), node.point.X / 2 - 10 + 20, node.point.Y - 10, 20, 20);
                    if (node.num < 10)
                        g.DrawString(node.num + "", new Font(DefaultFont.FontFamily, 13), new SolidBrush(Color.Indigo), (node.point.X / 2 - 7) + 20, (node.point.Y - 10));
                    else
                        g.DrawString(node.num + "", new Font(DefaultFont.FontFamily, 9), new SolidBrush(Color.Indigo), (node.point.X / 2 - 9) + 20, (node.point.Y - 8));
                }
                string temp;
                g.DrawString("Файл (Пути.txt) с самым выгодным набором маршрутов для", font, new SolidBrush(Color.Black), 700, 50);
                g.DrawString("данной топологии лежит в той же директории, что и программа.", font, new SolidBrush(Color.Black), 700, 70);
                StreamWriter sw = new StreamWriter("Пути.txt", false);
                for (int i = 0; i < n * (n - 1) / 2; i++)
                {
                    temp = "";
                    for (int j = 0; j < best[i].Count - 1; j++)
                    {
                        temp += best[i][j] + "<->";
                    }
                    temp += best[i][best[i].Count - 1];
                    sw.WriteLine(temp);
                }
                sw.Close();

                int add = 0;
                int[,] loading = new int[n, n];
                int[] nodsload = new int[n];

                for (int i = 0; i < n * (n - 1) / 2; i++)
                {
                    add = load[best[i][0], best[i].Last()];
                    nodsload[best[i][0]] += add;
                    for (int j = 1; j < best[i].Count; j++)
                    {
                        loading[best[i][j - 1], best[i][j]] += add;
                        loading[best[i][j], best[i][j - 1]] += add;
                        nodsload[best[i][j]] += add;
                    }
                }

                g.DrawString("Чтобы не было задежек нужно купить оборудование с", font, new SolidBrush(Color.Black), 700, 110);
                g.DrawString("пропускрой способностью не меньшей рекомендуемой.", font, new SolidBrush(Color.Black), 700, 130);
                g.DrawString("Рекомендуемые пропускные способности", font, new SolidBrush(Color.Black), 700, 150);
                g.DrawString("лежат в файле: \"Рекомендации.txt\"", font, new SolidBrush(Color.Black), 700, 170);
                StreamWriter sw2 = new StreamWriter("Рекомендации.txt", false);

                foreach (Edge edge1 in graphs[pos].Item2)
                    if (graphload[edge1.numa, edge1.numb] == 0)
                        sw2.WriteLine("Для ребра " + edge1.numa + "<->" + edge1.numb + " нагрузка равна: " + loading[edge1.numa, edge1.numb] + "Мбит/с");

                for (int i = 0; i < n; i++)
                    if (graphs[pos].Item1[i].speed == 0)
                        sw2.WriteLine("Для вершины \"" + graphs[pos].Item1[i].num + "\" нагрузка равна: " + nodsload[graphs[pos].Item1[i].num] + "Мбит/с");
                sw2.Close();

                int cost = 0;
                foreach (Edge edge2 in graphs[pos].Item2)
                    cost += edge2.cost;
                foreach (Node node1 in graphs[pos].Item1)
                    cost += node1.cost;


                g.DrawString("Стоимость обслуживания/установки (без учёта", font, new SolidBrush(Color.Black), 700, 210);
                g.DrawString("ещё не купленного оборудования): " + cost, font, new SolidBrush(Color.Black), 700, 230);

                menuStrip1.Items[2].Visible = false;
            }
        }

        public static void DrawGraph(List<Node> nodes, List<Edge> edges, Graphics gg)
        {
            foreach (Edge edge in edges)
                gg.DrawLine(new Pen(Color.RoyalBlue, 3), edge.a, edge.b);

            foreach (Node node in nodes)
            {
                gg.FillEllipse(new SolidBrush(Color.Fuchsia), node.point.X - 10, node.point.Y - 10, 20, 20);
                if (node.num < 10)
                    gg.DrawString(node.num + "", new Font(DefaultFont.FontFamily, 13), new SolidBrush(Color.Indigo), node.point.X - 7, node.point.Y - 10);
                else
                    gg.DrawString(node.num + "", new Font(DefaultFont.FontFamily, 9), new SolidBrush(Color.Indigo), node.point.X - 9, node.point.Y - 8);
            }
        }

        SolidBrush SB(int x)
        {
            if (x == 1)
                return new SolidBrush(Color.Red);
            else if (x == 2)
                return new SolidBrush(Color.Green);
            else if (x == 3)
                return new SolidBrush(Color.Blue);
            else
                return new SolidBrush(Color.WhiteSmoke);
        }
    }
}
