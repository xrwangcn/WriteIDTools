﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WriteIDTools
{
    class WriteInfo
    {
        File_Transfer_cfg transfer_cfg;
        File_Transfer_Helper tran_helper;
        public Thread FILE_Trans_Thread;

        public string ID="";
        public string verson="";
        public int action;
        public byte[] encKey;
        public WriteInfo()
        {
            encKey = new byte[20];
        }
        public responseInfo WriteInfoFunc(writeInfo info)
        {
            tran_helper = new File_Transfer_Helper();
            if (!initCheck())
            {
                MessageBox.Show("请选择串口！");
                return null;
            }
            
            //string ID = "";
            //FILE_Trans_Thread = new Thread(new ThreadStart(WriteInfo_func));
            //FILE_Trans_Thread.Priority = ThreadPriority.Highest;
            //FILE_Trans_Thread.IsBackground = true;
            //FILE_Trans_Thread.Start();

            return tran_helper.WriteInfo(transfer_cfg, info); 
        }

        public void cfgInit(string ComPortName,RichTextBox log)
        {
            transfer_cfg = new File_Transfer_cfg();
            transfer_cfg.ComPortName = ComPortName;
            transfer_cfg.RichTextBox = log;
        }
        //private void WriteInfo_func()
        //{
        //    tran_helper.WriteInfo(transfer_cfg, info);
        //}

        public Boolean initCheck()
        {
            if (transfer_cfg.ComPortName == null || transfer_cfg.ComPortName.Trim().Equals(""))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 加载可用串口列表
        /// </summary>
        public static string[] GetComList()
        {
            List<string> res = new List<string>();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    res.Add(sValue);
                }
            }
            return res.ToArray<string>();
        }

    }

    public class responseInfo
    {
        public string ID;
        public byte[] SN = new byte[20];
    }

    public class writeInfo
    {
        public string ID = "";
        public string verson = "";
        public int action;
        public byte[] encKey;
    }
}
