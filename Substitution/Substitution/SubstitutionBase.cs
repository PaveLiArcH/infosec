using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt
{
    public class SubstitutionBase:IBlockCrypt
    {
        public char[] Alphabet;
        public Dictionary<char, int> ElementIndexes;
        public int WordLength;

        public Dictionary<string, int> CombinationsDictionary;
        public string[] Combinations;

        public int[] DirectSubstitution;
        public int[] InverseSubstitution;

        public SubstitutionBase(int a_wordLength, params char[] a_alphabet)
        {
            //ToDo: add check for adequacy (at least check for positive)
            WordLength = a_wordLength;
            //ToDo: add check for a_elements or just call distinct
            Alphabet = a_alphabet;

            var _elementsCount = Alphabet.Length;
            ElementIndexes = new Dictionary<char, int>();
            for (int i = 0; i < _elementsCount; i++)
            {
                ElementIndexes.Add(Alphabet[i], i);
            }

            //ToDo: add check for adequacy
            int _combinationCount = (int)Math.Pow(_elementsCount, WordLength);
            CombinationsDictionary = new Dictionary<string, int>(_combinationCount);
            Combinations = new string[_combinationCount];

            int[] _combinationIndexes = new int[WordLength];
            for (int i = 0; i < _combinationCount; i++)
            {
                string _combination = "";
                int _add = 1;
                for (int j = WordLength - 1; j >= 0; j--)
                {
                    _combination = Alphabet[_combinationIndexes[j]] + _combination;
                    if (_add != 0)
                    {
                        _combinationIndexes[j] += _add;
                        _add = Math.DivRem(_combinationIndexes[j], _elementsCount, out _combinationIndexes[j]);
                    }
                }
                CombinationsDictionary.Add(_combination, i);
                Combinations[i] = _combination;
            }

            DirectSubstitution = GenerateSubstitution().ToArray();
            InverseSubstitution = new int[DirectSubstitution.Length];
            for (int i=0; i<InverseSubstitution.Length; i++)
            {
                InverseSubstitution[DirectSubstitution[i]] = i;
            };
        }

        public IEnumerable<int> GenerateSubstitution()
        {
            int _num=Combinations.Length;
            IEnumerable<int> _takePart, _skipPart, _startSubstitution = Enumerable.Range(0, _num);
            var _rnd=new Random();
            //int _rounds = _rnd.Next(_num >> 1) + _num >> + 1;
            int _rounds = _rnd.Next(_num) + _num + 1;
            for (int i = 0; i < _rounds; i++)
            {
                int _which=_rnd.Next(_num+1);
                _takePart=_startSubstitution.Take(_which).Reverse();
                _skipPart=_startSubstitution.Skip(_which).Reverse();
                _startSubstitution = _takePart.Concat(_skipPart).ToList();
            }
            return _startSubstitution;
        }

        public string Encrypt(string a_source)
        {
            int _charsToComplete = a_source.Length % WordLength;
            if (_charsToComplete > 0)
            {
                a_source=a_source.PadRight(a_source.Length + WordLength - _charsToComplete, Alphabet[0]);
            }
            string _retStr = "";
            for (int i = 0; i < a_source.Length; i += WordLength)
            {
                _retStr += Combinations[DirectSubstitution[CombinationsDictionary[a_source.Substring(i, WordLength)]]];
            }
            return _retStr;
        }

        public string Decrypt(string a_source)
        {
            int _charsToComplete = a_source.Length % WordLength;
            if (_charsToComplete > 0)
            {
                a_source = a_source.PadRight(a_source.Length + WordLength - _charsToComplete, Alphabet[0]);
            }
            string _retStr = "";
            for (int i = 0; i < a_source.Length; i += WordLength)
            {
                _retStr += Combinations[InverseSubstitution[CombinationsDictionary[a_source.Substring(i, WordLength)]]];
            }
            return _retStr;
        }
    }
}
