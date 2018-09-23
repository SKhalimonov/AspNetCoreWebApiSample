using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebApiSample.Data.Core.Specification.Binary;
using WebApiSample.Data.Core.Specification.Unary;

namespace WebApiSample.Data.Core.Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool SatisfiedBy(T value);

        public abstract IEnumerable<T> SatisfyingElementsFrom(IEnumerable<T> candidates);

        public abstract Expression<Func<T, bool>> SatisfiedBy();

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
