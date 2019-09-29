using BlockChain.BlockContent;
using BlockChain.Interfaces;
using Serilog;
using System;

namespace BlockChain
{
    public class Application
    {
        private readonly IChain _chain;
        private readonly ILogger _logger;

        public Application(IChain chain, ILogger logger)
        {
            this._chain = chain;
            this._logger = logger;
        }

        public void Run()
        {
            var newBlock = new Block<Kyc>
            {
                Content = new Kyc
                {
                    Header = "kyc"
                }
            };
            this._chain.AddBlock(newBlock);
            var newBlock1 = new Block<Transaction>
            {
                Content = new Transaction
                {
                    Header = "transaction"
                }
            };
            this._chain.AddBlock(newBlock1);

            for (int i = 0; i < 2; i++)
            {
                var block = new Block<Transaction>
                {
                    Content = new Transaction
                    {
                        Header = $"Transaction {i}"
                    }
                };
                this._chain.AddBlock(block);
            }

            //this._logger.Information(this._chain.ToJson());

            var (valid, message) = this._chain.IsValid();
            if (valid)
            {
                this._logger.Information("Chain is valid");
            }
            else
            {
                this._logger.Error($"Chain is not valid {message}");
            }

            Console.ReadLine();
        }
    }
}