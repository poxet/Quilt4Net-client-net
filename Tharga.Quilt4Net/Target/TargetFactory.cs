using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Target
{
    internal static class TargetFactory
    {
        public static ITarget Get()
        {
            if (Configuration.Target.Instance != null) return Configuration.Target.Instance;

            switch (Configuration.Target.Type)
            {
                case Configuration.Target.TargetType.Service:
                    return new ServiceTarget(Configuration.Target.Location, Configuration.Target.Timeout);
                default:
                    throw ExpectedIssues.GetException(ExpectedIssues.UnknownType).AddData("Type", Configuration.Target.Type.ToString());
            }
        }
    }
}