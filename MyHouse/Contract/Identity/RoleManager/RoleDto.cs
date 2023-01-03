using System;

namespace Contract.Identity.RoleManager
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set;}
    }
}