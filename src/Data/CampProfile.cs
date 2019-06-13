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
            this.CreateMap<Camp, CampModel>().ForMember(c => c.Venue, o => o.MapFrom(w => w.Location.VenueName)).ReverseMap();
            this.CreateMap<Talk, TalkModel>().ReverseMap().ForMember(x => x.Camp, opt => opt.Ignore()).ForMember(x => x.Speaker, opt => opt.Ignore());
            this.CreateMap<Speaker, SpeakerModel>().ReverseMap();
        }
    }
}
