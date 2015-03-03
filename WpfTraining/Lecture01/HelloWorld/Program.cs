using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelloWorld
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new Window();
            window.Content = "Hello, world!";
            var application = new Application();
            application.Run(window);
        }
    }
}
