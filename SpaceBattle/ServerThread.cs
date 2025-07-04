﻿using System.Collections.Concurrent;
using Hwdtech;

namespace ShipNamespace
{
    public class ServerThread
    {
        private Action _behaviour;
        private readonly BlockingCollection<IComand> _queue;
        private readonly Thread _thread;
        private bool _stop = false;

        public ServerThread(BlockingCollection<IComand> queue, Action afterThreadStart)
        {
            _queue = queue;

            _behaviour = () =>
            {
                var cmd = _queue.Take();
                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    IoC.Resolve<IComand>("ExceptionHandler", e).Execute();
                }
            };

            _thread = new Thread(() =>
            {
                afterThreadStart();

                while (!_stop)
                {
                    _behaviour();
                }
            });
        }

        internal void Stop()
        {
            _stop = !_stop;
        }

        public BlockingCollection<IComand> GetQueue()
        {
            return _queue;
        }
        public void Wait()
        {
            _thread.Join();
        }

        public Thread GetThread()
        {
            return _thread;
        }

        internal void SetBehaviour(Action newBehaviour)
        {
            _behaviour = newBehaviour;
        }

        public void Start()
        {
            _thread.Start();
        }
    }
}
