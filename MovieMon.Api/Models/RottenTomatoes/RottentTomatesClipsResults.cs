using System.Collections.Generic;

namespace RottenTomatoes.Types.Clips
{
    public class Links
    {
        public string alternate { get; set; }
    }

    public class Clip
    {
        public string title { get; set; }
        public string duration { get; set; }
        public string thumbnail { get; set; }
        public Links links { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
        public string alternate { get; set; }
        public string rel { get; set; }
    }

    public class RootObject
    {
        public List<Clip> clips { get; set; }
        public Links2 links { get; set; }
    }
}