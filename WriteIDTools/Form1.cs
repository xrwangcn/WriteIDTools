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

        static private string KEY = "10607D51C21D056FD612AC1A8E5D3848";
        responseInfo reinfo = new responseInfo();
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                LogRichTextBox.AppendText("请选择串口\n");
                return;
            }
            WriteInfo req = new WriteInfo();
            writeInfo info = new writeInfo();//写入设备的信息
            string temp = numericUpDown1.Value.ToString().PadLeft(3, '0');



            req.cfgInit(comboBox1.Items[comboBox1.SelectedIndex].ToString(),LogRichTextBox);
            info.action = 3;
            reinfo = req.WriteInfoFunc(info);
            byte[] snData = reinfo.SN;
            info.encKey = AESHelper.AESEncrypt(snData, KEY);
            info.action = 1;
            info.ID = textBox1.Text + textBox6.Text + textBox5.Text + temp;
            info.verson = textBox2.Text;
            LogRichTextBox.AppendText("ID: " + info.ID + ",硬件版本: " + info.verson + "\n开始烧写...\n");
            req.WriteInfoFunc(info);
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
            writeInfo wInfo = new writeInfo();
            req.cfgInit(comboBox1.Items[comboBox1.SelectedIndex].ToString(), LogRichTextBox);
            wInfo.action = 2;
            string result = req.WriteInfoFunc(wInfo).ID; 
            string[] info = result.Split(new char[3] { '/', '|', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (info.Count() == 0)
            {
                LogRichTextBox.AppendText("读取失败\n");
                return;
            }
            MessageBox.Show("ID:" + info[0]); 
            LogRichTextBox.AppendText("ID: " + info[0] + ",硬件版本: " + info[1] + "\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string temp = numericUpDown1.Value.ToString().PadLeft(3, '0');
            string ID = textBox1.Text + textBox6.Text + textBox5.Text + temp;
            LogRichTextBox.AppendText("ID: " + ID +"\n");
            //textBox3.Text = Convert.ToBase64String(AESHelper.AESEncrypt(ID, KEY));
        }


    }
}
