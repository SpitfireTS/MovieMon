using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MovieMon.Api.Models
{
    public class MovieSearchCriteria
    {
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Location Location { get; set; }
        public string Genre { get; set; }
        public MovieKey Key { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var field = Title != null ?string.Format("Title={0}?", Title): "Title=?";
            builder.Append(field);

            field = ReleaseDate != null ? string.Format("ReleaseDate={0}?", ReleaseDate) : "ReleaseDate=/";
            builder.Append(field);
            
            field = Genre != null ?string.Format("Genre={0}", Title): "Genre=/";
            builder.Append(field);
            return builder.ToString();
        }
    }
}

  