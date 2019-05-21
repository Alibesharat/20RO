
public class Navigation
{
    public Route[] routes { get; set; }
    public Waypoint[] waypoints { get; set; }
    public string code { get; set; }
    public string uuid { get; set; }
}

public class Route
{
    public Geometry geometry { get; set; }
    public Leg[] legs { get; set; }
    public string weight_name { get; set; }
    public float weight { get; set; }
    public float duration { get; set; }
    public float distance { get; set; }
}

public class Geometry
{
    public float[][] coordinates { get; set; }
    public string type { get; set; }
}

public class Leg
{
    public string summary { get; set; }
    public float weight { get; set; }
    public float duration { get; set; }
    public object[] steps { get; set; }
    public float distance { get; set; }
}

public class Waypoint
{
    public float distance { get; set; }
    public string name { get; set; }
    public float[] location { get; set; }
}
