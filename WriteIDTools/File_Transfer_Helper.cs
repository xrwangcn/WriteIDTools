using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace WriteIDTools
{

    class File_Transfer_Helper
    {
        public static int DEFAULT_BAUDRATE = 115200;

        // 开始
        private static byte SOH = 0x01;
        // 结束
        private static byte EOT = 0x04;
        // 应答
        private static byte ACK = 0x06;
        // 重传
        private static byte NAK = 0x15;
        // 无条件结束
        private static byte CAN = 0x18;

        // 以128字节块的形式传输数据
        private static int SECTOR_SIZE = 128;
        // 最大错误（无应答）包数
        private static int MAX_ERRORS = 10;



        SerialPort FileSerial;


        /// <summary>
        /// 获取设备ID
        /// </summary>
        /// <param name="trancfg"></param>
        /// <returns></returns>
        public responseInfo WriteInfo(File_Transfer_cfg trancfg, writeInfo info)
        {
            FileSerial = new SerialPort();
            responseInfo result = new responseInfo();
            FileSerial.PortName = trancfg.ComPortName;
            try
            {
                //if (KLINE.IsOpen)
                //{
                FileSerial.Close();
                // }

                FileSerial.BaudRate = DEFAULT_BAUDRATE;
                FileSerial.Parity = Parity.None;
                FileSerial.DataBits = 8;
                FileSerial.StopBits = StopBits.One;
                FileSerial.ReadBufferSize = 4096;
                FileSerial.WriteBufferSize = 4096;
                FileSerial.ReadTimeout = 3000;
                FileSerial.WriteTimeout = 1000;
                FileSerial.RtsEnable = true;
                FileSerial.DtrEnable = true;


                result = WriteInfoRequest(trancfg, FileSerial, info);
            }

            catch (System.Exception ex)
            {
                if (FileSerial == null) return null;
                FileSerial.Close();
                FileSerial = null;
                return null;
            }
            if (FileSerial == null) return null;
            FileSerial.Close();
            FileSerial = null;
            return result;
        }
        private responseInfo WriteInfoRequest(File_Transfer_cfg trancfg, SerialPort FileSerial, writeInfo info)
        {
            byte[] w_buf = new byte[128];
            string resultID = "";
            string resultVER = "";
            responseInfo result = new responseInfo();
            for (int i = 0; i < 128; i++)
            {
                w_buf[i] = 0;
            }

            w_buf[0] = 0x30;//Head
            w_buf[1] = 0x41;//动作类型

            //serial id
            if (info.action == 1)
            {
                w_buf[3] = 0x01;
            }
            else
            {
                w_buf[3] = 0x02;
            }

            info.ID = StringToHexString(info.ID, System.Text.Encoding.ASCII);
            byte[] ID_temp = strToToHexByte(info.ID);
            for (int i = 0; i < ID_temp.Count(); i++)
            {
                w_buf[4 + i] = ID_temp[i];
            }

            info.verson = StringToHexString(info.verson, System.Text.Encoding.ASCII);
            byte[] verson_temp = strToToHexByte(info.verson);
            for (int i = 0; i < verson_temp.Count(); i++)
            {
                w_buf[24 + i] = verson_temp[i];
            }
            if (info.encKey != null)
                for (int i = 0; i < 16; i++)
                {
                    w_buf[44 + i] = info.encKey[i];
                }
            FileSerial.Open();

            FileSerial.Write(w_buf, 0, 128);
            //接收反馈
            int nr = FileSerial.BytesToRead;
            //int n_wait_time = 0;
            //int n_wait_count = 5;

            //循环
            int n_wait_count;
            //等待五次
            byte[] r_buf = new byte[128];
            for (n_wait_count = 0; n_wait_count < 5; n_wait_count++)
            {
                Thread.Sleep(200);
                if (FileSerial.BytesToRead < 128)
                {
                    trancfg.RichTextBox_DoWork("未收到30 11反馈，超时" + (n_wait_count + 1) + "s\n");
                }
                else
                {
                    int h = FileSerial.BytesToRead;
                    int i = FileSerial.Read(r_buf, 0, 128);
                    break;
                }

            }
            //收到反馈


            

            if (r_buf[0] == 0x31 && r_buf[1] == 0x41)
            {
                if (r_buf[3] == 0x02)//只有读取到2命令才返回值
                {
                    if (info.action == 3)//只有读取到2命令才返回值
                    {

                        byte[] SN = new byte[20];
                        Array.Copy(r_buf, 64, SN, 0, 20);
                        result.SN = SN;
                        return result;
                    }
                    byte[] IDtemp = new byte[20];
                    byte[] versontemp = new byte[20];
                    Array.Copy(r_buf, 4, IDtemp, 0, 20);
                    resultID = System.Text.Encoding.ASCII.GetString(IDtemp);
                    resultID = resultID.Replace("\0", "");
                    Array.Copy(r_buf, 24, versontemp, 0, 20);
                    resultVER = System.Text.Encoding.ASCII.GetString(versontemp);
                    result.ID = resultID + "/|/" + resultVER.Replace("\0", "");
                }

                
            }
            else
            {
                trancfg.RichTextBox_DoWork("烧写失败\n");
            }
            return result;
        }

        private string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace("%", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }

}

