using System;
using Domain.Parts;
using JetBrains.Annotations;
using SqlServ4r.EntityFramework;
using SqlServ4r.RepGenerationPatten;
using Volo.Abp.DependencyInjection;

namespace SqlServ4r.Repository.Parts
{
    public class PartRepository : GenericRepository<Part,Guid>,ITransientDependency
    {
        public PartRepository([NotNull] DreamContext context) : base(context)
        {
        }
    }
}