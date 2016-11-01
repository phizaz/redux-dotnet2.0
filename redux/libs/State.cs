using System;
using System.Collections.Generic;
using System.Text;

namespace redux.libs
{
    public class StateException : Exception { }
    public class ProbAndFieldNotFound : StateException { }

    public interface StateCommon {
        void Dump();
        void SetOld();
        bool IsNew();
        object GetDynamic(string path);
    } 

    public abstract class State<StateType> : StateCommon
        where StateType : StateCommon
    {
        /* == REDUCER ==
         * declare state attributes here using public {get; internal set;}
         */

        private bool is_new = true; // note : this comes in handly when deciding is it new or not (used in store IsChange())

        public StateType Clone()
        {
            var state = (StateType)MemberwiseClone();
            state.SetOld();
            return state;
        }

        public void SetOld()
        {
            is_new = false;
        }

        public bool IsNew()
        {
            return is_new;
        }

        public void Dump()
        {
            var props = typeof(StateType).GetProperties();
            var fields = typeof(StateType).GetFields();
            StringBuilder sb = new StringBuilder();
            foreach (var item in props)
            {
                sb.Append($"{item.Name}:{item.GetValue(this, null)}; ");
            }
            foreach (var item in fields)
            {
                sb.Append($"{item.Name}:{item.GetValue(this)}; ");
            }
            sb.AppendLine();
            Console.WriteLine(sb.ToString());
        }

        private static object GetValue(object obj, string prob)
        {
            // note: get from both field and property 
            var t = obj.GetType();
            //Console.WriteLine("get value type: {0}", t.ToString());
            var f = t.GetField(prob);
            if (f == null)
            {
                var p = t.GetProperty(prob);
                if (p == null)
                {
                    throw new ProbAndFieldNotFound();
                }
                else
                {
                    return p.GetValue(obj, null);
                }
            }
            else
            {
                return f.GetValue(obj);
            }
        }

        public object GetDynamic(string path)
        {
            string[] tokens = path.Split('.');
            object current = this;
            foreach (var token in tokens)
            {
                current = GetValue(current, token);
            }
            return current;
        } 
    }
}
