using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux_examples.examples.calculator
{
    public class CalculatorStore : Store<CalculatorState, CalculatorStore, CalculatorReducer, CalculatorStore>
    {
        public CalculatorStore(ReduceRunner runner) : base(runner)
        {
             
        }

        public override void Dispose()
        {
            // pass
        }

        public override bool IsLoading()
        {
            return false;
        }

        public ManualResetEvent Plus(int a, int b)
        {
            return Dispatch(new ActPlus { a = a, b = b });
        }

        public ManualResetEvent Clear()
        {
            return Dispatch(new ActClear());
        }

    }
}
