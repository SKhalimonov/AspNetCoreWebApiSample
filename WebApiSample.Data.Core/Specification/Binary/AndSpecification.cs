﻿using System;
using System.Linq.Expressions;

namespace WebApiSample.Data.Core.Specification.Binary
{
    public class AndSpecification<T> : BinarySpecification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right)
        {
        }

        public override bool SatisfiedBy(T value)
        {
            return LeftSide.SatisfiedBy(value) && RightSide.SatisfiedBy(value);
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return Compose(LeftSide.SatisfiedBy(), RightSide.SatisfiedBy(), Expression.AndAlso);
        }
    }
}
