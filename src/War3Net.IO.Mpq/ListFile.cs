using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Foole.Mpq
{
    public class ListFile
    {
        public const string Key = "(listfile)";

        private Stream _baseStream;
        private bool _readOnly;

        public ListFile( IEnumerable<string> files )
        {
            _baseStream = new MemoryStream();
            _readOnly = false;

            using ( var writer = GetWriter() )
            {
                foreach ( var fileName in files )
                {
                    writer.WriteLine( fileName );
                }
            }

            _baseStream.Position = 0;
        }

        public Stream BaseStream => _baseStream;

        public void WriteFile( string fileName )
        {
            using ( var writer = GetWriter() )
            {
                writer.WriteLine( fileName );
            }
        }

        public void Finish()
        {
            _baseStream.Position = 0;
            _readOnly = true;
        }

        private StreamWriter GetWriter()
        {
            if ( _readOnly )
            {
                throw new IOException();
            }

            return new StreamWriter( _baseStream, new UTF8Encoding( false, true ), 1024, true );
        }
    }
}