using BlockChain.Interfaces;

namespace BlockChain.Interfaces
{
    public interface IChain
    {
        void AddBlock(IBlock newBlock);

        (bool valid, string message) IsValid();

        string ToJson();
    }
}