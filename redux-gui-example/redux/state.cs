using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;

namespace redux_gui_example.redux
{
    public class CalState : State<CalState>
    {
        public int a;
        public int b;
        public int result;
        public bool has_result = false;
    }
}
