using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Ognp
    {
        private readonly List<Stream> _streams = new List<Stream>();
        public Ognp(string megaFaculty)
        {
            MegaFaculty = megaFaculty;
        }

        public string MegaFaculty { get; }

        public List<Stream> GetStreams()
        {
            return !_streams.Any() ? null : new List<Stream>(_streams);
        }

        public Stream GetStream(Stream stream)
        {
            Stream findingStream = _streams.FirstOrDefault(findingStream => findingStream.Id == stream.Id);
            if (findingStream == null)
                throw new IsuExtraException($"No such stream in {MegaFaculty}'s ognp");

            return findingStream;
        }

        public void AddStream(Stream stream)
        {
            if (stream == null)
                throw new IsuExtraException("Stream cannot be null");
            if (_streams.Contains(stream))
                throw new IsuExtraException($"Stream is already registered for {MegaFaculty}'s ognp");
            _streams.Add(stream);
        }
    }
}