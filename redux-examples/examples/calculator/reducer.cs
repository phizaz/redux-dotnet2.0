using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;

namespace redux_examples.examples.calculator
{
    public class CalculatorReducer : Reducer<CalculatorState, CalculatorStore>
    {
        public CalculatorReducer(CalculatorStore store) : base(store)
        {
        }

        public CalculatorState Reduce(CalculatorState s, ActClear act)
        {
            s = s.Clone();
            s.has_result = false;
            s.result = default(int);
            return s;
        }

        public CalculatorState Reduce(CalculatorState s, ActPlus act)
        {
            s = s.Clone();
            s.has_result = true;
            s.result = act.a + act.b;
            return s;
        }
    }
}
