using Newtonsoft.Json;

namespace Cube_Playground.Models
{
    internal abstract class CubeModel
    {
        override public string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
