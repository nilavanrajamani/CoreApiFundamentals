using System;

namespace CoreCodeCamp.Data
{
    public class CampModel
    {
        public string Name { get; set; }
        public string Moniker { get; set; }
        public Location Location { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        public int Length { get; set; } = 1;
    }
}