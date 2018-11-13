namespace PremierLeagueAPI.Core.Models
{
    public class Kit
    {
        public int Id { get; set; }
        public KitType KitType { get; set; }
        public string PhotoUrl { get; set; }
        public int SquadId { get; set; }
        public Squad Squad { get; set; }
    }

    public enum KitType
    {
        HomeKit,
        AwayKit,
        ThirdKit
    }
}