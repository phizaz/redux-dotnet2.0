using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux.libs
{
    public class ReduxStoreException : Exception { }
    public class StateConstructorNotFound : ReduxStoreException { }
    public class ReducerConstructorNotFound : ReduxStoreException { }

    public interface StoreCommon {
        bool InvokeReduce(Act a);
        bool IsLoading();
        void Dispose();
        void SetParent(StoreCommon store);
    }

    public abstract class Store<StateType, StoreType, ReducerType, ParentStoreType> 
        : StoreCommon
        where StateType : StateCommon
        where StoreType : StoreCommon
        where ReducerType : ReducerCommon 
    {
        /* == STORE : The Single Source of Truth ==
         * - have a background worker of ReduceRunner (injected from the outside), that runs all the ruduce functions based on a given action
         */

        public ParentStoreType parent { get; protected set; }
        protected List<StoreCommon> sub_stores = new List<StoreCommon>();
        public StateType s { get; internal set; }
        public StateType prev { get; internal set; }
        protected ReducerType r;
        protected ReduceRunner runner { get; private set; } // used for Dispatching
        protected object locker = new object();

        public Store(
            ReduceRunner runner)
        {
            // init states
            var ctor = typeof(StateType).GetConstructor(new Type[] { });
            if (ctor == null)
            {
                throw new StateConstructorNotFound(); // state constructor not found
            }
            s = (StateType)ctor.Invoke(new object[] { });

            // init reducer
            var reducer_ctor = typeof(ReducerType).GetConstructor(new Type[] { typeof(StoreType) });
            if (reducer_ctor == null)
            {
                throw new ReducerConstructorNotFound(); // reducer ctor not found
            }
            r = (ReducerType)reducer_ctor.Invoke(new object[] { this });

            this.runner = runner;
        }

        public void AddSubstore(StoreCommon substore)
        {
            sub_stores.Add(substore);
            substore.SetParent(this);
        }

        public void ClearSubstore()
        {
            sub_stores = new List<StoreCommon>();
        }

        public void SetParent(StoreCommon store)
        {
            var p = (ParentStoreType)store;
            parent = p;
        }

        public ManualResetEvent Dispatch(Act a)
        {
            return runner.Enqueue(a);
        }

        public object GetLock()
        {
            return locker;
        }

        public StateType GetState()
        {
            return s;
        }

        public StateType GetPreviousState()
        {
            return prev;
        }

        public bool IsChange(string state_path)
        {
            if (s.IsNew()) return true;

            var current = s.GetDynamic(state_path);
            var previous= prev.GetDynamic(state_path);
            
            if (current == null || previous == null)
            {
                if (current == null && previous == null)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return !current.Equals(previous);
            }
        }

        public bool IsChange(string[] paths)
        {
            foreach (var path in paths)
            {
                if (IsChange(path)) return true;
            }
            return false;
        }

        public abstract bool IsLoading();
        public abstract void Dispose();

        public bool InvokeReduce(Act a)
        {
            // called by ReduceRunner
            // keep changes of the current state and previous state

            bool success = false;
            // try to invoke store's function based on its overloading of Reduce function
            // Reduce(State s, Act a) <-- with auto casting
            var method = r.GetType().GetMethod("Reduce", new Type[] { s.GetType(), a.GetType() });
            if (method != null)
            {
                prev = s;
                // if error happens here, the target reduce function has a problem
                s = (StateType) method.Invoke(r, new object[] { s, a });
                success = true;
            }
            else
            {
                prev = s;
            }

            // recursively call the sub-stores if any
            if (sub_stores.Count > 0)
            {
                foreach (var s in sub_stores)
                {
                    success |= s.InvokeReduce(a);
                }    
            }
            return success;
        }

    }
}
