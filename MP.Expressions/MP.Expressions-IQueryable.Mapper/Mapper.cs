using System;

namespace MP.Expressions_IQueryable.Mapper
{
    public class Mapper<TInputType, TOutputType>
    {
        private readonly Func<TInputType, TOutputType> _mapFunction;

        internal Mapper(Func<TInputType, TOutputType> func)
        {
            _mapFunction = func;
        }

        public TOutputType Map(TInputType source)
        {
            return _mapFunction(source);
        }
    }
}
