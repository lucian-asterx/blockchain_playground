using BlockChain.Interfaces;

namespace BlockChain.BlockContent
{
    public class Genesis : IBlockContent
    {
        public string Header { get; set; }
    }
}