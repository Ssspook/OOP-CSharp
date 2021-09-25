using System;

namespace IsuExtra.Entities
{
    public class Lesson
    {
        private DateTime _startTime;
        private DateTime _endTime;
        private string _professor;
        private string _room;
        public Lesson(DateTime startTime, DateTime endTime, string professor, string room)
        {
            _startTime = startTime;
            _endTime = endTime;
            _room = room;
            _professor = professor;
        }

        public DateTime StartTime => _startTime;
        public DateTime EndTime => _endTime;
    }
}