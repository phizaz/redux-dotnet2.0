using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;

namespace redux_examples.examples.calculator
{
    public class CalculatorState : State<CalculatorState>
    {
        public bool has_result = false;
        public int result;
    }
}
