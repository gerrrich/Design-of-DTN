using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ПСПД
{
    public partial class Form6 : Form
    {
        Form1 f;
        public Form6(Form1 form1)
        {
            InitializeComponent();
            f = form1;
        }

        private void Form6_Shown(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("/", " ");
            for (int i = 0; i < (int)Math.Sqrt(f.load.Length); i++)
                dataGridView1.Columns.Add(i + "", f.graphs[f.pos].Item1[i].name + "(" + f.graphs[f.pos].Item1[i].num + ")");
            dataGridView1.Rows.Add((int)Math.Sqrt(f.load.Length));
            for (int i = 0; i < (int)Math.Sqrt(f.load.Length); i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = f.graphs[f.pos].Item1[i].name + "(" + f.graphs[f.pos].Item1[i].num + ")";
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
            for (int i = 0; i < (int)Math.Sqrt(f.load.Length); i++)
            {
                for (int j = i+1; j <= (int)Math.Sqrt(f.load.Length); j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "/";
                    dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int res;
            for (int i = 0; i < (int)Math.Sqrt(f.load.Length); i++)
                for (int j = 1; j <= (int)Math.Sqrt(f.load.Length); j++)
                    if (int.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out res))
                    {
                        f.load[j, i] = res;
                        f.load[i, j] = res;
                    }
            Close();
        }
    }
}
