using BlockChain.BlockContent;
using BlockChain.Configuration;
using BlockChain.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockChain
{
    public class Chain : IChain
    {
        private readonly ILogger _logger;

        public IList<IBlock> Blocks { get; }

        public int Difficulty { get; set; }

        public Chain(ILogger logger, BlockChainConfiguration configuration)
        {
            this._logger = logger;
            this.Difficulty = configuration.Difficulty;

            this._logger.Information($"Dificulty set to {this.Difficulty}");

            this.Blocks = new List<IBlock>
            {
                this.CreateGenesisBlock()
            };

            this._logger.Information("Genesis block generated successfuly");
        }

        public void AddBlock(IBlock newBlock)
        {
            newBlock.Id = this.GetLatestBlock().Id + 1;
            newBlock.PreviousHash = this.GetLatestBlock().Hash;
            newBlock.Mine(this.Difficulty);
            this.Blocks.Add(newBlock);

            this._logger.Information($"Block {newBlock.Id} added succesfully to the chain");
        }

        public (bool valid, string message) IsValid()
        {
            for (var i = 1; i < this.Blocks.Count; i++)
            {
                var currentBlock = this.Blocks[i];
                var previousBlock = this.Blocks[i - 1];

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

            foreach (var block in this.Blocks)
            {
                sb.Append('\n').Append(block.ToJson());
            }

            return sb.ToString();
        }

        private Block<Genesis> CreateGenesisBlock()
        {
            return new Block<Genesis>
            {
                Content = new Genesis { Header = "GENESIS" }
            };
        }

        public IBlock GetLatestBlock()
        {
            return this.Blocks.Last();
        }
    }
}