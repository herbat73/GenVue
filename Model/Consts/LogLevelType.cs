using System;
using System.Collections.Generic;
using System.Linq;

namespace GenVue.Model.Consts
{
    public class LogLevelType : Enumeration
    {
        public static LogLevelType INFO = new LogLevelType(0, "Information");
        public static LogLevelType SUCCESS = new LogLevelType(1, "Success");
        public static LogLevelType WARNING = new LogLevelType(2, "Warning");
        public static LogLevelType ERROR = new LogLevelType(3, "Error");

        protected LogLevelType() { }

        public LogLevelType(int id, string name)
            : base(id, name)
        {

        }

        public static IEnumerable<LogLevelType> List()
        {
            return new[]
            {
                INFO, SUCCESS, WARNING, ERROR
            };
        }

        public static LogLevelType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ArgumentException($"Possible values for LogLevelType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static LogLevelType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ArgumentException($"Possible values for LogLevelType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
