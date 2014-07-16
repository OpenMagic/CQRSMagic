using System;
using ExampleDomain.Repositories;
using Ninject;

namespace CQRSMagic.Specifications.UnitTests.ExampleMVCApplication
{
    public abstract class ExampleMvcApplicationTestBase : UnitTestsTestBase
    {
        protected ExampleMvcApplicationTestBase()
        {
        }

        protected ExampleMvcApplicationTestBase(Func<IKernel, IRepositoryFactory> createRepositoryFactory)
            : base(createRepositoryFactory)
        {
        }
    }
}