using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace WriteIDTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] comList = WriteInfo.GetComList();
            for (int i = 0; i < comList.Length; i++)
            {
                comboBox1.Items.Add(comList[i]);
            }
            this.comboBox1.Text = getCOM.GetSerialPort();
            GregorianCalendar gc = new GregorianCalendar();
            //获取当前周数
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            string dataStr = DateTime.Now.Year.ToString().Replace("20","");
            if (weekOfYear < 10)
                dataStr = dataStr + "0" + weekOfYear.ToString();
            else
                dataStr = dataStr + weekOfYear.ToString();
            dataStr = dataStr.Trim();
            Console.WriteLine(dataStr);
            Console.WriteLine(getCOM.GetSerialPort());
            textBox6.Text = dataStr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                LogRichTextBox.AppendText("请选择串口\n");
                return;
            }
            WriteInfo req = new WriteInfo();
            string temp = numericUpDown1.Value.ToString().PadLeft(3, '0');
            req.ID = textBox1.Text + textBox6.Text + textBox5.Text+temp;
            req.verson = textBox2.Text;
            LogRichTextBox.AppendText("ID: " + req.ID + ",硬件版本: " + req.verson + "\n开始烧写...\n");

            req.cfgInit(comboBox1.Items[comboBox1.SelectedIndex].ToString(),LogRichTextBox);
            req.WriteInfoFunc(1);
            LogRichTextBox.AppendText("烧写成功\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                LogRichTextBox.AppendText("请选择串口\n");
                return;
            }
            WriteInfo req = new WriteInfo();

            req.cfgInit(comboBox1.Items[comboBox1.SelectedIndex].ToString(), LogRichTextBox);
            string result = req.WriteInfoFunc(2);
            string[] info = result.Split(new char[3] { '/', '|', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (info.Count() == 0)
            {
                LogRichTextBox.AppendText("读取失败\n");
                return;
            }
            MessageBox.Show("ID:" + info[0]); 
            LogRichTextBox.AppendText("ID: " + info[0] + ",硬件版本: " + info[1] + "\n");
        }


    }
}
