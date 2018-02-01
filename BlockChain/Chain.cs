using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlockChain.BlockContent;

namespace BlockChain
{
    public class Chain
    {
        public Chain()
        {
            Difficulty = 4;
            Blocks = new List<IBlock>
            {
                CreateGenesisBlock()
            };
        }

        public IList<IBlock> Blocks { get; }

        public int Difficulty { get; set; }

        private IBlock CreateGenesisBlock()
        {
            return new Block<Genesis>
            {
                Content = new Genesis { Header = "GENESIS" }
            };
        }

        public IBlock GetLatestBlock()
        {
            return Blocks.Last();
        }

        public void AddBlock(IBlock newBlock)
        {
            newBlock.Id = GetLatestBlock().Id + 1;
            newBlock.PreviousHash = GetLatestBlock().Hash;
            newBlock.Mine(Difficulty);
            Blocks.Add(newBlock);
        }

        public (bool valid, string message) IsValid()
        {
            for (var i = 1; i < Blocks.Count; i++)
            {
                var currentBlock = Blocks[i];
                var previousBlock = Blocks[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return (false, $"Block #{currentBlock.Id} has invalid hash");
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return (false, $"Block #{currentBlock.Id} previoushash is not the same as previous block hash");
                }
            }
            return (true, string.Empty);
        }

        public string ToJson()
        {
            var sb = new StringBuilder();

            foreach (var block in Blocks)
            {
                sb.Append($"\n{block.ToJson()}");
            }

            return sb.ToString();
        }
    }
}