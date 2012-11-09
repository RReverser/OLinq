﻿using System;
using System.Linq.Expressions;

namespace OLinq
{

    class BinaryOperation : Operation<bool>
    {

        private BinaryExpression self;
        private IOperation left;
        private IOperation right;

        public BinaryOperation(OperationContext context, BinaryExpression expression)
            : base(context, expression)
        {
            self = expression;

            left = OperationFactory.FromExpression(context, expression.Left);
            left.Init();
            left.ValueChanged += left_ValueChanged;

            right = OperationFactory.FromExpression(context, expression.Right);
            right.Init();
            right.ValueChanged += right_ValueChanged;

            ResetValue();
        }

        void left_ValueChanged(object sender, ValueChangedEventArgs args)
        {
            ResetValue();
        }

        void right_ValueChanged(object sender, ValueChangedEventArgs args)
        {
            ResetValue();
        }

        bool ResetValue()
        {
            return SetValue(
                Expression.Lambda<Func<bool>>(Expression.MakeBinary(self.NodeType,
                    Expression.Constant(left.Value, self.Left.Type),
                    Expression.Constant(right.Value, self.Right.Type)))
                        .Compile()());
        }

        public override void Dispose()
        {
            if (left != null)
            {
                left.ValueChanged -= left_ValueChanged;
                left.Dispose();
                left = null;
            }

            if (right != null)
            {
                right.ValueChanged -= right_ValueChanged;
                right.Dispose();
                right = null;
            }

            base.Dispose();
        }

    }

}
