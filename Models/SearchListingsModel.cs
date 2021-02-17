using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonksList.Models
{
    public class SearchListingsModel
    {
        public List<Listing> listings;
        public IEnumerable<SelectListItem> filters;
        public SelectListItem currentFilter;

        public SearchListingsModel()
        {

        }
    }
}
