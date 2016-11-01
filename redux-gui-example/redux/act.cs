using redux.libs;
using System;
using System.Collections.Generic;
using System.Text;

namespace redux_gui_example.redux
{
    public class ActSetA : Act
    {
        public int a;
    }
    public class ActSetB : Act
    {
        public int b;
    }
    public class ActPlus : Act { }
    public class ActMinus : Act { }
    public class ActClear : Act { }
}
