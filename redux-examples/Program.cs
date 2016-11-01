using redux_examples.examples.calculator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux_examples
{
    class Program
    {
        static void Main(string[] args)
        {
            God god = new God();
            god.store.Plus(1, 2).WaitOne();
            god.store.Clear();
        }
    }
}
