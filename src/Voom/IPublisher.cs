using System;
using System.Threading.Tasks;

namespace Voom
{
    public interface IPublisher
    {
        Task PublishAsync(object? data = null);
    }
}

