using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace winctrl.Output
{
    class JsonOutputRenderer : IOutputRenderer
    {
        private JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };
        
        public JsonOutputRenderer()
        {
            _options.Converters.Add(new JsonStringEnumConverter());
        }
        
        public void Render(CommandOutputDTO output)
        {
            Console.WriteLine(JsonSerializer.Serialize(output, _options));
        }
    }
}