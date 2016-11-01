using System;
using System.Collections.Generic;
using System.Text;
using redux.libs;

namespace redux_gui_example.redux
{
    public class God 
    {
        public CalStore store;

        ReduceRunner runner = new ReduceRunner();

        public God(ReduceRunner.OnReduceFinish notify_fn)
        {
            store = new CalStore(runner);
            runner.SetUp(store.InvokeReduce, notify_fn);
        }

        public void Dispose()
        {
            runner.Dispose();
        }
    }
}
