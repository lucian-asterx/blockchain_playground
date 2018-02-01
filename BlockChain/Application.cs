using System;

using BlockChain.BlockContent;

using Serilog;

namespace BlockChain
{
    public class Application
    {
        private readonly IChain chain;

        private readonly ILogger logger;

        public Application(IChain chain, ILogger logger)
        {
            this.chain = chain;
            this.logger = logger;
        }

        public void Run()
        {
            this.logger.Information("Mining block ... ");
            var newBlock = new Block<Kyc>
                               {
                                   Content = new Kyc
                                                 {
                                                     Header = "kyc"
                                                 }
                               };

            chain.AddBlock(newBlock);
            this.logger.Information($"Block mined successfuly with nonce : {newBlock.Nonce}");

            this.logger.Information("Mining block ... ");
            var newBlock1 = new Block<Transaction>
                                {
                                    Content = new Transaction
                                                  {
                                                      Header = "transaction"
                                                  }
                                };

            chain.AddBlock(newBlock1);
            this.logger.Information($"Block mined successfuly with nonce : {newBlock1.Nonce}");

            this.logger.Information(chain.ToJson());

            var isValid = chain.IsValid();
            if (string.IsNullOrEmpty(isValid.message))
            {
                this.logger.Information($"Chain is valid");
            }
            else
            {
                this.logger.Error($"Chain is not valid {isValid.message}");
            }

            Console.ReadLine();
        }
    }
}
