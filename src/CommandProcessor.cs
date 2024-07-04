using System;
using System.Threading.Tasks;
using winctrl.Output;

namespace winctrl
{
    public class CommandProcessor
    {
        private IOutputRenderer _outputRenderer;
        
        public CommandProcessor(IOutputRenderer outputRenderer)
        {
            _outputRenderer = outputRenderer;
        }
        
        public async Task<int> Process<T>(Func<Task<T>> func)
        {
            CommandOutputDTO output;
            
            try
            {
                var result = await func.Invoke();
                output = new CommandOutputDTO()
                {
                    Code = 0,
                    Message = "Success",
                    Data = result,
                };
            }
            catch (Exception e)
            {
                output = new CommandOutputDTO()
                {
                    Code = 1,
                    Message = $"Exception: {e.GetType().Name}",
                    Data = null,
                };
            }
            
            _outputRenderer.Render(output);

            return output.Code;
        }
    }
}