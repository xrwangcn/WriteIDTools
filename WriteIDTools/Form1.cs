using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WriteIDTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] comList = WriteInfo.GetComList();
            this.comboBox1.Text = "                          请选择串口";
            for (int i = 0; i < comList.Length; i++)
            {
                comboBox1.Items.Add(comList[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WriteInfo req = new WriteInfo();
            req.ID = textBox1.Text;
            req.verson = textBox2.Text;
            req.WriteInfoFunc(comboBox1.Items[comboBox1.SelectedIndex].ToString(),1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriteInfo req = new WriteInfo();
            string result = req.WriteInfoFunc(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 2);
        }


    }
}
