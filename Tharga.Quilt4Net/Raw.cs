using Tharga.Quilt4Net.Target;

#if DEBUG
namespace Tharga.Quilt4Net
{
    public static class Raw
    {
        public static string Execute(string jsonData, string controller, string action)
        {
            var target = TargetFactory.Get() as ServiceTarget;
            return target.ExecuteQuery(jsonData, controller, action);
        }
    }
}
#endif