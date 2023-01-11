using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Double Length { get; set; }
        public Guid RegionID { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Property
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
