using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class ReaderFile : IReaderFile
    {
        private readonly CoreContext _context;

        public ReaderFile(CoreContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ReadFile(string path)
        {
            Console.WriteLine("Hello");
        }
    }
}
