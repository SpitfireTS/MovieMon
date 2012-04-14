using System.Collections.Generic;

namespace RottenTomatoes.Types.Cast
{
    public class Cast
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> characters { get; set; }
    }

    public class Links
    {
        public string rel { get; set; }
    }

    public class RootObject
    {
        public List<Cast> cast { get; set; }
        public Links links { get; set; }
    }
}