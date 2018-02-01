using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BlockChain.BlockContent;
using Newtonsoft.Json;

namespace BlockChain
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

    public class Block<T> : IBlock where T : IBlockContent
    {
        public Block()
        {
            Timestamp = DateTime.Now;
            Hash = CalculateHash();
            PreviousHash = "0";
        }

        public T Content { get; set; }

        public long Nonce { get; set; }

        public long Id { get; set; }

        public string Hash { get; set; }

        public string PreviousHash { get; set; }

        public DateTime Timestamp { get; }

        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            return json;
        }

        public override string ToString()
        {
            var contentJson = JsonConvert.SerializeObject(Content);
            return $"{Id}:{Nonce}:{contentJson}:{PreviousHash}:{Timestamp}";
        }

        public string CalculateHash()
        {
            var hashstring = new SHA256Managed();

            var bytes = Encoding.UTF8.GetBytes(ToString());
            var hash = hashstring.ComputeHash(bytes);

            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }

        public void Mine(int difficulty)
        {
            while (Hash.Substring(0, difficulty) != new string('0', difficulty))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}