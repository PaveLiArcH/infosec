using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public interface IDES
    {
        void KeyRing(UInt64 a_source);

        byte[] Encrypt(byte[] a_data);

        byte[] Decrypt(byte[] a_data);
    }
}
