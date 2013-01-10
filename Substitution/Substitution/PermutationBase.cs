using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encrypt
{
    public class PermutationBase:IBlockCrypt
    {
        public char[] Alphabet;
        public int SequenceLength;

        public int[] DirectPermutation;
        public int[] InversePermutation;

        public PermutationBase(int a_sequenceLength, params char[] a_alphabet)
        {
            Alphabet = a_alphabet;
            SequenceLength = a_sequenceLength;
            DirectPermutation = GeneratePermutation().ToArray();
            InversePermutation = new int[DirectPermutation.Length];
            for (int i = 0; i < InversePermutation.Length; i++)
            {
                InversePermutation[DirectPermutation[i]] = i;
            };
        }

        public IEnumerable<int> GeneratePermutation()
        {
            var _startPermutation = Enumerable.Range(0, SequenceLength);
            var _rnd = new Random();
            //int _rounds = _rnd.Next(_num >> 1) + _num >> + 1;
            int _rounds = _rnd.Next(SequenceLength) + SequenceLength + 1;
            for (int i = 0; i < _rounds; i++)
            {
                int _which = _rnd.Next(SequenceLength + 1);
                var _takePart = _startPermutation.Take(_which).Reverse();
                var _skipPart = _startPermutation.Skip(_which).Reverse();
                _startPermutation = _takePart.Concat(_skipPart).ToList();
            }
            return _startPermutation;
        }

        public string Encrypt(string a_source)
        {
            string _retStr = "";
            while (a_source.Length > 0)
            {
                string _encryptingStr = new string(a_source.Take(SequenceLength).ToArray()).PadRight(SequenceLength, Alphabet[0]);
                a_source=new string(a_source.Skip(SequenceLength).ToArray());
                for (int i = 0; i < SequenceLength; i++)
                {
                    _retStr += _encryptingStr[DirectPermutation[i]];
                }
            }
            return _retStr;
        }

        public string Decrypt(string a_source)
        {
            string _retStr = "";
            while (a_source.Length > 0)
            {
                string _encryptingStr = new string(a_source.Take(SequenceLength).ToArray()).PadRight(SequenceLength, Alphabet[0]);
                a_source = new string(a_source.Skip(SequenceLength).ToArray());
                for (int i = 0; i < SequenceLength; i++)
                {
                    _retStr += _encryptingStr[InversePermutation[i]];
                }
            }
            return _retStr;
        }
    }
}
