using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>().ForMember(c => c.Venue, o => o.MapFrom(w => w.Location.VenueName));
            this.CreateMap<Talk, TalkModel>();
            this.CreateMap<Speaker, SpeakerModel>();
        }
    }
}
