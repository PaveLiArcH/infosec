using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encrypt
{
    interface IBlockCrypt
    {
        string Encrypt(string a_source);
        string Decrypt(string a_source);
    }
}
