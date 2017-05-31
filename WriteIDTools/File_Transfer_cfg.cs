using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WriteIDTools
{
    class File_Transfer_cfg
    {


        public RichTextBox RichTextBox;
        ShowMsg_t ShowMsg_pf;

        string _ComPortName = "";
        string _FolderPathFull = "";


        public string ComPortName
        {
            get
            {
                return _ComPortName;
            }
            set
            {
                _ComPortName = value;
            }
        }

        public string FolderPathFull
        {
            get
            {
                return _FolderPathFull;
            }
            set
            {
                _FolderPathFull = value;
            }
        }

        public File_Transfer_cfg()
        {
            ShowMsg_pf = AppendText;
        }

        delegate void ShowMsg_t(string text);


        

        public int RichTextBox_GetLength()
        {
            return (RichTextBox.TextLength);
        }
        void richTextBox_Select(int start, int len)
        {
            RichTextBox.Select(start, len);
        }
        void AppendText(string text)
        {
            RichTextBox.AppendText(text);
            RichTextBox.Focus();
        }
        void TextSetting(Color SelectColor, Font SelectFont)
        {
            RichTextBox.SelectionColor = SelectColor;
            RichTextBox.SelectionFont = SelectFont;
        }

       


        public void RichTextBox_DoWork(string text)
        {
            if (RichTextBox == null) return;
            RichTextBox.BeginInvoke(ShowMsg_pf, text);
        }
    }
}
