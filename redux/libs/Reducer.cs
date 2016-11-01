using System;
using System.Collections.Generic;
using System.Text;

namespace redux.libs
{ 
    public interface ReducerCommon { }

    public abstract class Reducer<StateType, StoreType> : ReducerCommon
        where StateType : StateCommon
        where StoreType : StoreCommon
    {
        /* == REDUCER ==
         * - public Reduce<StateType> <-- of a given state
         * - must declare public StateType Reduce(StateType s, <Action> a) 
         */

        protected StoreType store;

        public Reducer(StoreType store) {
            this.store = store; 
        } 
    }

}
