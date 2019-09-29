using System;

namespace BlockChain.Interfaces
{
    public interface IBlock
    {
        long Id { get; set; }

        string Hash { get; set; }

        string PreviousHash { get; set; }

        DateTime Timestamp { get; }

        void Mine(int difficulty);

        string CalculateHash();

        string ToJson();
    }
}