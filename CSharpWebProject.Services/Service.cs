using CSharpWebProject.Data;
using System;

namespace CSharpWebProject.Services
{
    public class Service
    {
        public Service(ApplicationDbContext context)
        {
            this.Context = context;
        }

        protected ApplicationDbContext Context { get; private set; }
    }
}
