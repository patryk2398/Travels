using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travels.Models.ViewModel
{
    public class PlacesAndUserViewModel
    {
        public ApplicationUser UserObj { get; set; }
        public IEnumerable<Place> Place { get; set; }
        public List<TravelType> TravelTypesList { get; set; }
        public List<Place> PlacesList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
