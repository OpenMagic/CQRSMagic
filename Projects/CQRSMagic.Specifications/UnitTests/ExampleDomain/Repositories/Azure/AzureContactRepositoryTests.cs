using ExampleMVCApplication;

namespace CQRSMagic.Specifications.UnitTests.ExampleDomain.Repositories.Azure
{
    public class AzureContactRepositoryTests : ContactRepositoryTestBase
    {
        public AzureContactRepositoryTests()
            : base(RepositoryFactories.AzureRepositories)
        {
        }
    }
}