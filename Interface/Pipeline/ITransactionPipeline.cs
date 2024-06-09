using Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Interface.Pipeline;

public interface ITransactionPipeline<T, TR>
    where T : DbContext
{
    Task<Result<TR>> Execute(TR data, CancellationToken cancellationToken);
}
