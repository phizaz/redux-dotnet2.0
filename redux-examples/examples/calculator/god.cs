using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux_examples.examples.calculator
{
    public class God
    {
        public CalculatorStore store;
        ReduceRunner runner;
        
        public God()
        {
            runner = new ReduceRunner();
            store = new CalculatorStore(runner);
            runner.SetUp(store.InvokeReduce, ReduceEnd);
        }

        public void ReduceEnd()
        {
            Console.WriteLine("reduce end");
            store.GetState().Dump();
        }
    }
}
