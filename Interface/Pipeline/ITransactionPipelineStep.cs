using Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Interface.Pipeline;

public interface ITransactionPipelineStep<T, TR>
    where T : DbContext
{
    Task<Result<TR>> Execute(T context, TR data, CancellationToken cancellationToken);
}
