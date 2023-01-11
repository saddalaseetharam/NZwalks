namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequest
    {
        public string Name { get; set; }
        public Double Length { get; set; }
        public Guid RegionID { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}
