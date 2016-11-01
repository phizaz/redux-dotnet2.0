using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace redux.libs
{

    public class ReducerException : Exception { }
    public class UncatchedAction : ReducerException { }

    public class ReducerJob
    {
        public Act action;
        public ManualResetEvent finish;
    }

    // note: from http://stackoverflow.com/questions/1656404/c-sharp-producer-consumer

    public class ReduceRunner : IDisposable
    {
        public delegate bool OnReduceBegin(Act a);
        public delegate void OnReduceFinish();

        object locker;
        Thread worker;
        Queue<ReducerJob> jobs;
        OnReduceBegin on_begin;
        OnReduceFinish on_finish;


        public ReduceRunner()
        {

        }

        public void SetUp(OnReduceBegin on_begin, OnReduceFinish on_finish)
        {
            this.on_begin = on_begin;
            this.on_finish = on_finish;

            if (worker != null)
            {
                throw new Exception();
            }

            this.on_begin = on_begin;
            this.on_finish = on_finish;
            locker = new object();
            jobs = new Queue<ReducerJob>();
            worker = new Thread(Consume);
            worker.Start();
        }

        public void Dispose()
        {
            Enqueue(null);
            worker.Join();
            worker = null;
        }

        public void Join()
        {
            worker.Join();
        }

        public ManualResetEvent Enqueue(Act action)
        {
            // note : also return the evet that this action is carried out !
            ReducerJob job = new ReducerJob();
            job.action = action;
            job.finish = new ManualResetEvent(false);

            lock (locker)
            {
                if (action != null)
                {
                    Console.WriteLine("enqueing action: {0}", action.GetType().ToString());
                }
                jobs.Enqueue(job);
                Monitor.PulseAll(locker);
            }

            return job.finish;
        }

        void Consume()
        {
            while (true)
            {
                ReducerJob job;
                lock (locker)
                {
                    while (jobs.Count == 0) Monitor.Wait(locker);
                    job = jobs.Dequeue();
                }


                Act action = job.action;
                ManualResetEvent finish = job.finish;

                if (action == null)
                {
                    Console.WriteLine("reducer dies");
                    finish.Set();
                    return;         // This signals our exit
                }

                Console.WriteLine("reducing: {0}", action.GetType().ToString());

                // reduce here
                bool success = on_begin(action);
                if (!success)
                {
                    throw new UncatchedAction();
                }

                // rerender 
                on_finish();
                finish.Set();
            }
        }
    }
}
