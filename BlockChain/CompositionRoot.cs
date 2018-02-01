using BlockChain.BlockContent;

using Serilog;

using StructureMap;

namespace BlockChain
{
    public class CompositionRoot : Registry
    {
        public CompositionRoot()
        {
            For<ILogger>().Use(o => Log.ForContext(o.ParentType));

            For<IBlockContent>().Use<Genesis>();
            For<IBlockContent>().Use<Kyc>();
            For<IBlockContent>().Use<SmartContract>();
            For<IBlockContent>().Use<Transaction>();
        }
    }
}
