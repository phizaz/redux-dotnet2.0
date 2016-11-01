using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;

namespace redux_gui_example.redux
{
    public class CalReducer : Reducer<CalState, CalStore>
    {
        public CalReducer(CalStore store) : base(store)
        {
        }

        public CalState Reduce(CalState s, ActSetA act)
        {
            s = s.Clone();
            s.a = act.a;
            return s;
        }

        public CalState Reduce(CalState s, ActSetB act)
        {
            s = s.Clone();
            s.b = act.b;
            return s;
        }

        public CalState Reduce(CalState s, ActPlus act)
        {
            s = s.Clone();
            s.result = s.a + s.b;
            s.has_result = true;
            return s;
        }

        public CalState Reduce(CalState s, ActMinus act)
        {
            s = s.Clone();
            s.result = s.a - s.b;
            s.has_result = true;
            return s;
        }

        public CalState Reduce(CalState s, ActClear act)
        {
            s = new CalState();
            return s;
        }

    }
}
