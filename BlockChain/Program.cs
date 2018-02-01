using System;
using BlockChain.BlockContent;

namespace BlockChain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var chain = new Chain();

            Console.Write("Mining block ... ");
            var newBlock = new Block<Kyc>
            {
                Content = new Kyc { Header = "kyc" }
            };

            chain.AddBlock(newBlock);
            Console.WriteLine(newBlock.Nonce);

            Console.Write("Mining block ... ");
            var newBlock1 = new Block<Transaction>
            {
                Content = new Transaction { Header = "transaction" } 
            };

            chain.AddBlock(newBlock1);
            Console.WriteLine(newBlock1.Nonce);

            Console.WriteLine(chain.ToJson());

            var isValid = chain.IsValid();
            if (string.IsNullOrEmpty(isValid.message))
            {
                Console.WriteLine($"Is Valid : {isValid.valid}");
            }
            else
            {
                Console.WriteLine($"Is Valid : {isValid.valid} : {isValid.message}");
            }

            Console.ReadLine();
        }
    }
}