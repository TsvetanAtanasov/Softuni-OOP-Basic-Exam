using StorageMaster.Core;
using StorageMaster.Core.IO;
using StorageMaster.Core.IO.Contracts;
using System;

namespace StorageMaster
{
    public class StartUp
    {
        public static void Main()
        {
            IReader reader = new ConsoleReader();
            IWriter writer = new ConsoleWriter();

            var engine = new Engine(reader, writer);
            engine.Run();
        }
    }
}
