using System;
using System.Linq;
using System.Threading.Tasks;
using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;

namespace winctrl.Modules.Keyboard
{
    class KeyboardService
    {
        private InputSimulator _simulator = new InputSimulator();

        public async Task SendKeys(string keys)
        {
            var splits = keys.Split('+');
            var keyCodes = splits.Select(ParseKeyCode).ToList();
            var modifiers = keyCodes.Take(keyCodes.Count - 1).ToList();
            var key = keyCodes.Last();

            _simulator.Keyboard.ModifiedKeyStroke(modifiers, key);

            Console.WriteLine("Sending keys: " + keyCodes);
        }

        private VirtualKeyCode ParseKeyCode(string text)
        {
            var upper = text.ToUpper();
            
            if (upper == "CTRL")
            {
                return VirtualKeyCode.CONTROL;
            }

            if (upper == "ALT")
            {
                return VirtualKeyCode.MENU;
            }

            if (upper == "SHIFT")
            {
                return VirtualKeyCode.SHIFT;
            }

            try
            {
                return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), text);
            }
            catch
            {
                return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "VK_" + text);
            }
        }
    }
}