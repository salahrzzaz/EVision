using System;
namespace Application.Infrastructure.Data.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreationDate { get; set; }
    }
}
