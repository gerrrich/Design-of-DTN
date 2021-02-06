using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПСПД
{
    public partial class Form4 : Form
    {
        Form1 f;
        public Form4(Form1 form1)
        {
            InitializeComponent();
            f = form1;
        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(f.edges.Count);
            dataGridView1.AllowUserToAddRows = false;
            for (int i = 0; i < f.edges.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = f.edges[i].numa + "<->" + f.edges[i].numb;
                dataGridView1.Rows[i].Cells[0].Style.ForeColor = Color.White;
                if (f.edges[i].color == 1)
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
                else if (f.edges[i].color == 2)
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Green;
                else
                {
                    dataGridView1.Rows[i].Cells[2].Value = "подбор";
                    dataGridView1.Rows[i].Cells[3].ReadOnly = true;
                    dataGridView1.Rows[i].Cells[2].ReadOnly = true;
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Blue;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < f.edges.Count; i++)
            {
                try
                {
                    string speed = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    if (speed.Substring(speed.Length - 6, 1) == "M")
                        f.edges[i].speed = int.Parse(speed.Substring(0, speed.Length - 7));
                    else
                        f.edges[i].speed = int.Parse(speed.Substring(0, speed.Length - 7)) * 1024;
                }
                catch
                {
                    f.edges[i].speed = 0;
                }
                try
                {
                    f.edges[i].cost = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                }
                catch
                {
                    f.edges[i].cost = 0;
                }
                try
                {
                    string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    f.edges[i].name = name;
                }
                catch
                {
                    f.edges[i].name = f.edges[i].numa + "<->" + f.edges[i].numb;
                }
            }
            Close();
        }
    }
}
