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

namespace DES
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ECB _desECB;

        public MainWindow()
        {
            _desECB = new ECB();
            _desECB.KeyRing(12345678901011);
            var _data = new byte[] { 0, 0xB, 0xA, 0xD, 0xF, 0x0, 0x0, 0xD, 0xF, 0xE, 0xE, 0x1, 0xD, 0xE, 0xA, 0xD, 0x1 };
            var _encrypted=_desECB.Encrypt(_data);
            var _decrypted = _desECB.Decrypt(_encrypted);
            InitializeComponent();
        }
    }
}
