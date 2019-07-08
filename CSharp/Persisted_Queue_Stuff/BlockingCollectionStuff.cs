public class WorkItemProducer
{
    private WorkQueue _workQueue;
 
    public WorkItemProducer(WorkQueue workQueue)
    {
        _workQueue = workQueue;
    }
 
    public void ProduceWorkItems()
    {
        while (true)
        {
            Guid jobId = Guid.NewGuid();
            WorkTask wt = new WorkTask(string.Concat("Work with job ID ", jobId), DateTime.UtcNow);
            Debug.WriteLine(string.Format("Thread {0} added work {1} at {2} to the work queue.",
                Thread.CurrentThread.ManagedThreadId, wt.Description, wt.InsertedUtc));
            _workQueue.AddTask(wt);
            Thread.Sleep(2000);
        }
    }
}

public class BlockingCollectionSampleService
{
    public void RunBlockingCollectionCodeSample()
    {
        WorkQueue workQueue = new WorkQueue(new ConcurrentQueue<WorkTask>());
        WorkItemProducer producerOne = new WorkItemProducer(workQueue);
        WorkItemProducer producerTwo = new WorkItemProducer(workQueue);
        WorkItemProducer producerThree = new WorkItemProducer(workQueue);
 
        Task producerTaskOne = Task.Run(() => producerOne.ProduceWorkItems());
        Task producerTaskTwo = Task.Run(() => producerTwo.ProduceWorkItems());
        Task producerTaskThree = Task.Run(() => producerThree.ProduceWorkItems());
 
        Task consumerTaskOne = Task.Run(() => workQueue.MonitorWorkQueue());
        Task consumerTaskTwo = Task.Run(() => workQueue.MonitorWorkQueue());
 
        Console.WriteLine("Tasks started...");
    }
}