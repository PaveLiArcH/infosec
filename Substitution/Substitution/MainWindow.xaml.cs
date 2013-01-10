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

namespace Encrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        IBlockCrypt CurrentCrypt;

        bool isSubstitution;
        public bool IsSubstitution
        {
            get
            {
                return isSubstitution;
            }
            set
            {
                isSubstitution = value;
                OnPropertyChanged("IsSubstitution");
                if (isSubstitution)
                {
                    CurrentCrypt = Substitution;
                    SourceText = "";
                }
            }
        }

        bool isPermutation;
        public bool IsPermutation
        {
            get
            {
                return isPermutation;
            }
            set
            {
                isPermutation = value;
                OnPropertyChanged("IsPermutation");
                if (isPermutation)
                {
                    CurrentCrypt = Permutation;
                    SourceText = "";
                }
            }
        }

        bool isXor;
        public bool IsXor
        {
            get
            {
                return isXor;
            }
            set
            {
                isXor = value;
                OnPropertyChanged("IsXor");
                if (isXor)
                {
                    CurrentCrypt = Xor;
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
                            ResultText = CurrentCrypt.Encrypt(SourceText);
                        }
                        else
                            if (isDecrypt)
                            {
                                ResultText = CurrentCrypt.Decrypt(SourceText);
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

        SubstitutionBase Substitution = new SubstitutionBase(2, 'a', 'b', 'c', 'd', 'e');
        PermutationBase Permutation = new PermutationBase(16, 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm');
        xorBase Xor = new xorBase(37, 11, 30, 256);
        //AlphabetBase Alpabet = new AlphabetBase(5, '0', '1');

        public MainWindow()
        {
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
