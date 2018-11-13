namespace PremierLeagueAPI.Dtos.Stadium
{
    public class StadiumUpdateDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int BuiltYear { get; set; }
        public string PitchSize { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string MapPhotoUrl { get; set; }
    }
}