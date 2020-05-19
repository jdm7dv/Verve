//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using Microsoft.Hpc.Scheduler;

namespace MBT.Escience.HpcLib
{
    public class ClusterStatus
    {
        public string Cluster { get; set; }
        public int IdleCores { get; set; }
        public int BusyCores { get; set; }
        public int QueuedTasks { get; set; }

        private IScheduler _scheduler;

        public ClusterStatus(string clusterName)
        {
            Cluster = clusterName;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}/{2}", Cluster, IdleCores, IdleCores + BusyCores);
        }

        public bool Refresh()
        {
            bool changed = false;
            ISchedulerCounters counters;
            using (ParallelOptionsScope.Suspend())
            {
                if (Connect() && null != (counters = GetCounters()))
                {
                    changed =
                       BusyCores != counters.BusyCores ||
                       IdleCores != counters.IdleCores ||
                       QueuedTasks != counters.QueuedTasks;


                    BusyCores = counters.BusyCores;
                    IdleCores = counters.IdleCores;
                    QueuedTasks = counters.QueuedTasks;

                }
                else
                {
                    changed = BusyCores != -1;

                    BusyCores = -1;
                    IdleCores = -1;
                    QueuedTasks = -1;

                }
            }
            return changed;
        }

        private ISchedulerCounters GetCounters()
        {
            try
            {
                ISchedulerCounters counters = _scheduler.GetCounters();
                return counters;
            }
            catch (Exception)
            {
                _scheduler = null;
                return null;
            }
        }

        private bool Connect()
        {
            if (_scheduler != null) return true;

            bool connected = false;

            using (ParallelOptionsScope.Suspend())
            {
                connected = HpcLib.TryConnect(Cluster, out _scheduler);
            }

            return connected;
        }
    }
}
