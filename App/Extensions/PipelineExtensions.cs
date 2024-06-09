using System.Reflection;
using Implementation.Pipeline;
using Interface.Pipeline;

namespace App.Extensions;

public static class PipelineExtensions
{
    public static IServiceCollection RegisterPipelineSteps(this IServiceCollection serviceCollection)
    {
        var pipelineStepInterface = typeof(ITransactionPipelineStep<,>);
        var assembly = Assembly.GetAssembly(typeof(BaseTransactionPipeline<,>))
            ?? throw new Exception("Should be able to find assembly from BaseTransactionPipeline type");

        var pipelineStepTypes = assembly
            .GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType 
                    && i.GetGenericTypeDefinition() == pipelineStepInterface) && t.IsClass)
            .ToList();
        
        foreach (var type in pipelineStepTypes)
        {
            serviceCollection.AddTransient(type);
        }

        return serviceCollection;
    }
}
