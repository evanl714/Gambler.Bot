namespace KryGamesBot.Ava.Classes
{
    public class RollVerifierRoll
    {
        public string ServerSeed { get; set; }
        public string ClientSeed { get; set; }
        public int Nonce { get; set; }
        public decimal Roll { get; set; }
    }
}