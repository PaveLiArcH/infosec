using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace DES
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        IDES CurrentCrypt;

        bool isECB;
        public bool IsECB
        {
            get
            {
                return isECB;
            }
            set
            {
                isECB = value;
                OnPropertyChanged("IsECB");
                if (isECB)
                {
                    CurrentCrypt = ecb;
                    SourceText = "";
                }
            }
        }

        bool isCBC;
        public bool IsCBC
        {
            get
            {
                return isCBC;
            }
            set
            {
                isCBC = value;
                OnPropertyChanged("IsCBC");
                if (isCBC)
                {
                    CurrentCrypt = cbc;
                    SourceText = "";
                }
            }
        }

        bool isCFB;
        public bool IsCFB
        {
            get
            {
                return isCFB;
            }
            set
            {
                isCFB = value;
                OnPropertyChanged("IsCFB");
                if (isCFB)
                {
                    CurrentCrypt = cfb;
                    SourceText = "";
                }
            }
        }

        bool isOFB;
        public bool IsOFB
        {
            get
            {
                return isOFB;
            }
            set
            {
                isOFB = value;
                OnPropertyChanged("IsOFB");
                if (isOFB)
                {
                    CurrentCrypt = ofb;
                    SourceText = "";
                }
            }
        }

        bool isEncrypt;
        public bool IsEncrypt
        {
            get
            {
                return isEncrypt;
            }
            set
            {
                isEncrypt = value;
                OnPropertyChanged("IsEncrypt");
                if (isEncrypt)
                {
                    isDecrypt = false;
                    SourceText = ResultText;
                }
            }
        }

        bool isDecrypt;
        public bool IsDecrypt
        {
            get
            {
                return isDecrypt;
            }
            set
            {
                isDecrypt = value;
                OnPropertyChanged("IsDecrypt");
                if (isDecrypt)
                {
                    isEncrypt = false;
                    SourceText = ResultText;
                }
            }
        }

        //static byte[] GetBytes(string str)
        //{
        //    byte[] bytes = new byte[str.Length * sizeof(char)];
        //    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    return bytes;
        //}

        //static string GetString(byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = (byte)str[i];
            }
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = (char)bytes[i];
            }
            return new string(chars);
        }

        string sourceText;
        public string SourceText
        {
            get
            {
                return sourceText;
            }
            set
            {
                sourceText = value;
                OnPropertyChanged("SourceText");
                if (sourceText.Length > 0 && CurrentCrypt!=null)
                {
                    try
                    {
                        if (isEncrypt)
                        {
                            ResultText = GetString(CurrentCrypt.Encrypt(GetBytes(SourceText)));
                        }
                        else
                            if (isDecrypt)
                            {
                                ResultText = GetString(CurrentCrypt.Decrypt(GetBytes(SourceText)));
                            }
                    }
                    catch
                    {
                        ResultText = "";
                    }
                }
                else
                {
                    ResultText = "";
                }
            }
        }
        
        public string ResultText
        {
            get { return (string)GetValue(ResultTextProperty); }
            set { SetValue(ResultTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResultText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultTextProperty =
            DependencyProperty.Register("ResultText", typeof(string), typeof(MainWindow), new UIPropertyMetadata(null));

        ECB ecb = new ECB();
        CBC cbc = new CBC();
        CFB cfb = new CFB();
        OFB ofb = new OFB();

        public MainWindow()
        {
            ecb.KeyRing(0);
            cbc.KeyRing(0);
            cfb.KeyRing(0);
            ofb.KeyRing(0);

            //var _bytes=new byte[] { 0x0, 0xb, 0xa, 0xd, 0xf, 0x0, 0x0, 0xd};
            //var _enc = ofb.Encrypt(_bytes);
            //var _dec = ofb.Decrypt(_enc);
            
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string a_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(a_name));
            }
        }
    }
}
