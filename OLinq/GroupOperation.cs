﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace OLinq
{

    abstract class GroupOperation<TSource, TResult> : EnumerableSourceOperation<TSource, TResult>
    {

        public GroupOperation(OperationContext context, MethodCallExpression expression, Expression sourceExpression)
            : base(context, expression, sourceExpression)
        {
            ResetValue();
        }

        protected override void OnSourceCollectionReset()
        {
            ResetValue();
        }

        protected override void OnSourceCollectionItemsAdded(IEnumerable<TSource> newItems, int startingIndex)
        {
            base.OnSourceCollectionItemsAdded(newItems, startingIndex);
            ResetValue();
        }

        protected override void OnSourceCollectionItemsRemoved(IEnumerable<TSource> oldItems, int startingIndex)
        {
            base.OnSourceCollectionItemsRemoved(oldItems, startingIndex);
            ResetValue();
        }

        /// <summary>
        /// Gets the new value by recalculating the results.
        /// </summary>
        /// <returns></returns>
        protected abstract TResult RecalculateValue();

        /// <summary>
        /// Recalculates the value and sets it.
        /// </summary>
        protected void ResetValue()
        {
            SetValue(RecalculateValue());
        }

    }

    abstract class GroupOperationWithLambda<TSource, TLambdaResult, TResult> : EnumerableSourceWithLambdaOperation<TSource, TLambdaResult, TResult>
    {

        public GroupOperationWithLambda(OperationContext context, MethodCallExpression expression, Expression sourceExpression, Expression<Func<TSource, TLambdaResult>> lambdaExpression)
            : base(context, expression, sourceExpression, lambdaExpression)
        {
            ResetValue();
        }

        protected override void OnLambdaCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            ResetValue();
        }

        protected override void OnLambdaValueChanged(LambdaValueChangedEventArgs<TSource, TLambdaResult> args)
        {
            ResetValue();
        }

        /// <summary>
        /// Gets the new value by recalculating the results.
        /// </summary>
        /// <returns></returns>
        protected abstract TResult RecalculateValue();

        /// <summary>
        /// Recalculates the value and sets it.
        /// </summary>
        protected void ResetValue()
        {
            SetValue(RecalculateValue());
        }

    }

    abstract class GroupOperationWithProjection<TSource, TProjection, TResult> : GroupOperationWithLambda<TSource, TProjection, TResult>
    {

        public GroupOperationWithProjection(OperationContext context, MethodCallExpression expression, Expression sourceExpression, Expression<Func<TSource, TProjection>> projectionExpression)
            : base(context, expression, sourceExpression, projectionExpression)
        {

        }

        public LambdaContainer<TSource, TProjection> Projections
        {
            get { return Lambdas; }
        }

        protected override sealed void OnLambdaCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            OnProjectionCollectionChanged(args);
        }

        /// <summary>
        /// Invoked when the projection collection is changed.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnProjectionCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            ResetValue();
        }

        protected override sealed void OnLambdaValueChanged(LambdaValueChangedEventArgs<TSource, TProjection> args)
        {
            OnProjectionValueChanged(args);
        }

        /// <summary>
        /// Invoked when the value of a projection is changed.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnProjectionValueChanged(LambdaValueChangedEventArgs<TSource, TProjection> args)
        {
            ResetValue();
        }

    }

    abstract class GroupOperationWithPredicate<TSource, TResult> : GroupOperationWithProjection<TSource, bool, TResult>
    {

        public GroupOperationWithPredicate(OperationContext context, MethodCallExpression expression, Expression sourceExpression, Expression<Func<TSource, bool>> predicateExpression)
            : base(context, expression, sourceExpression, predicateExpression)
        {

        }

        public LambdaContainer<TSource, bool> Predicates
        {
            get { return Projections; }
        }

        protected override sealed void OnProjectionCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            OnPredicateCollectionChanged(args);
        }

        /// <summary>
        /// Invoked when the predicate collection is changed.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPredicateCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            ResetValue();
        }

        protected override sealed void OnProjectionValueChanged(LambdaValueChangedEventArgs<TSource, bool> args)
        {
            OnPredicateValueChanged(args);
        }

        /// <summary>
        /// Invoked when a predicate value is changed.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPredicateValueChanged(LambdaValueChangedEventArgs<TSource, bool> args)
        {
            ResetValue();
        }

    }

}
