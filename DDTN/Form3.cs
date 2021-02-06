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
    public partial class Form3 : Form
    {
        Form1 form1;
        public Form3(Form1 f)
        {
            InitializeComponent();
            form1 = f;
            //form1.nodes = new List<Node>();
            //for (int i=0;i<5;i++)
            //    form1.nodes.Add(new Node());
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(form1.nodes.Count);
            dataGridView1.AllowUserToAddRows = false;
            for (int i = 0; i < form1.nodes.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[0].Style.ForeColor = Color.White;
                if (form1.nodes[i].color == 1)
                    dataGridView1.Rows[form1.nodes[i].num].Cells[0].Style.BackColor = Color.Red;
                else if (form1.nodes[i].color == 2)
                    dataGridView1.Rows[form1.nodes[i].num].Cells[0].Style.BackColor = Color.Green;
                else
                {
                    dataGridView1.Rows[form1.nodes[i].num].Cells[2].Value = "подбор";
                    dataGridView1.Rows[form1.nodes[i].num].Cells[2].ReadOnly = true;
                    dataGridView1.Rows[form1.nodes[i].num].Cells[3].ReadOnly = true;
                    dataGridView1.Rows[form1.nodes[i].num].Cells[0].Style.BackColor = Color.Blue;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < form1.nodes.Count; i++)
            {
                try
                {
                    string speed = dataGridView1.Rows[form1.nodes[i].num].Cells[3].Value.ToString();
                    if (speed.Substring(speed.Length - 6, 1) == "M")
                        form1.nodes[i].speed = int.Parse(speed.Substring(0, speed.Length - 7));
                    else
                        form1.nodes[i].speed = int.Parse(speed.Substring(0, speed.Length - 7)) * 1024;
                }
                catch
                {
                    form1.nodes[i].speed = 0;
                }
                try
                {
                    form1.nodes[i].cost = int.Parse(dataGridView1.Rows[form1.nodes[i].num].Cells[2].Value.ToString());
                }
                catch
                {
                    form1.nodes[i].cost = 0;
                }
                try
                {
                    string name = dataGridView1.Rows[form1.nodes[i].num].Cells[1].Value.ToString();
                    form1.nodes[i].name = name;
                }
                catch
                {
                    form1.nodes[i].name = form1.nodes[i].num + "";
                }
            }
            Close();
        }
    }
}
