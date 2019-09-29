using BlockChain.Interfaces;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain
{
    public class Block<T> : IBlock where T : IBlockContent
    {
        public Block()
        {
            this.Timestamp = DateTime.Now;
            this.Hash = this.CalculateHash();
            this.PreviousHash = "0";
        }

        public T Content { get; set; }

        public long Id { get; set; }

        public long Nonce { get; set; }

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
            var contentJson = JsonConvert.SerializeObject(this.Content);
            return $"{this.Id}:{this.Nonce}:{contentJson}:{this.PreviousHash}:{this.Timestamp}";
        }

        public string CalculateHash()
        {
            using var hashstring = new SHA256Managed();

            var bytes = Encoding.UTF8.GetBytes(this.ToString());
            var hash = hashstring.ComputeHash(bytes);

            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }

        public void Mine(int difficulty)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (this.Hash.Substring(0, difficulty) != new string('0', difficulty))
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
            stopwatch.Stop();
            Log.Information($"Elapsed time {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}