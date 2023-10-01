namespace Gwizd.Clients;

public class PredictionRequest
    {
        public string Id { get; set; }
    }

public class Bbox
{
    public double Top { get; set; }
    public double Left { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Label
{
    public string Name { get; set; }
    public Bbox Bbox { get; set; }
    public double Confidence { get; set; }
    public string Type { get; set; }
    public object Breed { get; set; }
}

public class Body
{
    public string Image_src { get; set; }
    public List<Label> Labels { get; set; }
}

public class PredictionResponse
{
    public int StatusCode { get; set; }
    public Body Body { get; set; }
}

