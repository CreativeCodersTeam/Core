using System.Collections;
using System.Collections.Generic;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class TestDataObject
    {
        public string StrValue { get; set; }

        public int IntValue { get; set; }

        public double FloatValue { get; set; }

        public long LongValue { get; set; }

        public decimal DecimalValue { get; set; }

        public object ObjValue { get; set; }

        public IEnumerable<string> StrItems { get; set; }

        public IList<string> StrList { get; set; }

        public IEnumerable<int> IntItems { get; set; }

        public IEnumerable ObjItems { get; set; }
    }
}