using System.Threading.Tasks;

namespace Recore.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static void SyncResult(this Task task) => task.GetAwaiter().GetResult();

        public static T SyncResult<T>(this Task<T> task) => task.GetAwaiter().GetResult();
    }
}