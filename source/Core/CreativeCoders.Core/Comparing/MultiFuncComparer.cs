namespace CreativeCoders.Core.Comparing
{
    //public class MultiFuncComparer<T, TKey> : MultiComparer<T>
    //{
    //    public MultiFuncComparer(SortFieldInfo<T, TKey> sortFieldInfo) :
    //        base(new FuncComparer<T, TKey>(sortFieldInfo.KeySelector, sortFieldInfo.SortOrder))
    //    {
    //    }
    //}

    public class MultiFuncComparer<T, TKey1, TKey2> : MultiComparer<T>
    {
        public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2) :
            base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
                new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder))
        {
        }
    }

    public class MultiFuncComparer<T, TKey1, TKey2, TKey3> : MultiComparer<T>
    {
        public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
            SortFieldInfo<T, TKey3> sortFieldInfo3) :
            base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
                new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
                new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder))
        {
        }
    }

    public class MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4> : MultiComparer<T>
    {
        public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
            SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4) :
            base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
                new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
                new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder),
                new FuncComparer<T, TKey4>(sortFieldInfo4.KeySelector, sortFieldInfo4.SortOrder))
        {
        }
    }

    public class MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5> : MultiComparer<T>
    {
        public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
            SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4,
            SortFieldInfo<T, TKey5> sortFieldInfo5) :
            base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
                new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
                new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder),
                new FuncComparer<T, TKey4>(sortFieldInfo4.KeySelector, sortFieldInfo4.SortOrder),
                new FuncComparer<T, TKey5>(sortFieldInfo5.KeySelector, sortFieldInfo5.SortOrder))
        {
        }
    }
}