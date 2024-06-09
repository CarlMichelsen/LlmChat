using Domain.Abstraction;
using Interface.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Pipeline;

public abstract class BaseTransactionPipeline<T, TR> : ITransactionPipeline<T, TR>
    where T : DbContext
{
    protected BaseTransactionPipeline(T context, params ITransactionPipelineStep<T, TR>[] steps)
    {
        this.Context = context;
        this.Steps = steps.ToList();
    }

    protected T Context { get; init; }

    private List<ITransactionPipelineStep<T, TR>> Steps { get; init; }

    public async Task<Result<TR>> Execute(TR data, CancellationToken cancellationToken)
    {
        using var transaction = this.Context.Database.BeginTransaction();
        try
        {
            await this.PrePipelineExecution(data);
            var step = this.Steps.First();

            do
            {
                this.Steps.Remove(step);
                var result = await step.Execute(this.Context, data, cancellationToken);
                if (result.IsError)
                {
                    if (result.Error is OperationCanceledException canceled)
                    {
                        // Note, this does not roll back the transaction.
                        transaction.Commit();
                        return canceled;
                    }

                    transaction.Rollback();
                    return result.Error!;
                }

                data = result.Unwrap();
                step = this.Steps.FirstOrDefault();
            }
            while (step is not null);

            transaction.Commit();
            return data;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return e;
        }
        finally
        {
            await this.PostPipelineExecution(data);
        }
    }

    protected virtual Task PrePipelineExecution(TR data)
    {
        return Task.CompletedTask;
    }

    protected virtual Task PostPipelineExecution(TR data)
    {
        return Task.CompletedTask;
    }
}
