using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Filter
{

    public class UseQueueFromParameterAttribute : JobFilterAttribute, IElectStateFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueAttribute"/> class
        /// using the specified queue name.
        /// </summary>
        /// <param name="queue">Queue name.</param>
        public UseQueueFromParameterAttribute(string queueName)
        {
            this.QueueName = queueName;
        }

        public string QueueName { get; private set; }

        public void OnStateElection(ElectStateContext context)
        {
            ((EnqueuedState)context.CandidateState).Queue = String.Format(QueueName, context.BackgroundJob.Job.Args.ToArray());
        }
    }
}
