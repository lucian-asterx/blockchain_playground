using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlockChain.BlockContent;
using BlockChain.Configuration;

using Serilog;

namespace BlockChain
{
    public interface IChain
    {
        void AddBlock(IBlock newBlock);

        (bool valid, string message) IsValid();

        string ToJson();
    }

    public class Chain : IChain
    {
        private readonly ILogger logger;

        public Chain(ILogger logger, BlockChainConfiguration configuration)
        {
            this.logger = logger;
            Difficulty = configuration.Difficulty;

            this.logger.Information($"Dificulty set to {this.Difficulty}");

            Blocks = new List<IBlock>
            {
                CreateGenesisBlock()
            };

            this.logger.Information("Genesis block generated successfuly");
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

            this.logger.Information($"Block {newBlock.Id} added succesfully to the chain");
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