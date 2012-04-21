using System.Collections.Generic;

namespace RottenTomatoes.Types.Reviews
{
    public class Links
    {
        public string review { get; set; }
    }

    public class Review
    {
        public string critic { get; set; }
        public string date { get; set; }
        public string freshness { get; set; }
        public string publication { get; set; }
        public string quote { get; set; }
        public Links links { get; set; }
        public string original_score { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
        public string alternate { get; set; }
        public string rel { get; set; }
    }

    public class RootObject
    {
        public int total { get; set; }
        public List<Review> reviews { get; set; }
        public Links2 links { get; set; }
        public string link_template { get; set; }
    }
}