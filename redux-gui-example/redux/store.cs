using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux_gui_example.redux
{
    public class CalStore : Store<CalState, CalStore, CalReducer, CalStore>
    {
        public CalStore(ReduceRunner runner) : base(runner)
        {
        }

        public override void Dispose()
        {
            return;
        }

        public override bool IsLoading()
        {
            return false;
        }

        public ManualResetEvent SetA(int a)
        {
            return Dispatch(new ActSetA { a = a });
        }

        public ManualResetEvent SetB(int b)
        {
            return Dispatch(new ActSetB { b = b });
        }

        public ManualResetEvent Plus()
        {
            return Dispatch(new ActPlus());
        }

        public ManualResetEvent Minus()
        {
            return Dispatch(new ActMinus());
        }

        public ManualResetEvent Clear()
        {
            return Dispatch(new ActClear());
        }
    }
}
