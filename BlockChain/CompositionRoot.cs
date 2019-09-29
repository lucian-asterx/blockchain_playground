using BlockChain.BlockContent;
using BlockChain.Interfaces;
using Serilog;

using StructureMap;

namespace BlockChain
{
    public class CompositionRoot : Registry
    {
        public CompositionRoot()
        {
            this.For<ILogger>().Use(o => Log.ForContext(o.ParentType));

            this.For<IBlockContent>().Use<Genesis>();
            this.For<IBlockContent>().Use<Kyc>();
            this.For<IBlockContent>().Use<SmartContract>();
            this.For<IBlockContent>().Use<Transaction>();
        }
    }
}
