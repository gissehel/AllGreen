using AllGreen.Lib.Core.DomainModel.Pipe;
using AllGreen.Lib.DomainModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllGreen.Lib.DomainModel.Pipe
{
    public class PipeResultLine : IPipeResultLine
    {
        public PipeResultLine(int indentLevel)
        {
        }

        public PipeResultLine(int indentLevel, string comment)
        {
            State = TestItemState.None;
            Duration = null;
            PipeLine = new PipeLine(indentLevel, comment);
        }

        public PipeResultLine(int indentLevel, IEnumerable<string> parts)
        {
            State = TestItemState.None;
            Duration = null;
            PipeLine = new PipeLine(indentLevel, parts);
        }

        public IPipeLine PipeLine { get; set; }
        IPipeLine IPipeResultLine.PipeLine => PipeLine;

        public TestItemState State { get; set; }

        public Exception Exception { get; set; }

        public string FailureShortMessage { get; set; }

        public TimeSpan? Duration { get; set; }

        public string StateAsString
        {
            get
            {
                switch (State)
                {
                    case TestItemState.Unknown: return "[??]";
                    case TestItemState.Success: return "[  ]";
                    case TestItemState.Error: return "[KO]";
                    case TestItemState.Failure: return "[**]";

                    case TestItemState.Comment:
                    case TestItemState.None:
                    default:
                        return "    ";
                }
            }
        }

        public string DurationAsString
        {
            get
            {
                var duration = Duration;
                if ((!duration.HasValue) || (State == TestItemState.Comment))
                {
                    return "            ";
                }

                return string.Format(@"{0:hh\:mm\:ss\.fff}", duration.Value);
            }
        }

        public string PipedName => GetPipedName(PipedNameOptions.Default);

        public string GetPipedName(PipedNameOptions options)
        {
            var builder = new StringBuilder();

            builder.Append(StateAsString);
            builder.Append(" ");
            if (options.DisplayDuration)
            {
                builder.Append(DurationAsString);
                builder.Append(" ");
            }
            builder.Append(PipeLine.GetPipedName(options));

            return builder.ToString();
        }

        public IEnumerable<Exception> EnumerateSubExceptions()
        {
            Exception currentException = Exception;
            while (currentException != null)
            {
                yield return currentException;
                currentException = currentException.InnerException;
            }
        }

        public IEnumerable<string> GetStackTracesLines()
        {
            foreach (var exception in EnumerateSubExceptions())
            {
                yield return string.Format("==========> Failure:[{0}]", exception.Message);
                foreach (var line in exception.StackTrace.Split('\n'))
                {
                    yield return line;
                }
            }
            Exception currentException = Exception;
        }

        public IEnumerable<string> GetExceptionPipedNames(PipedNameOptions options)
        {
            if (options.DisplayStackTrace)
            {
                foreach (var line in GetStackTracesLines())
                {
                    var builder = new StringBuilder();
                    builder.Append("     ");
                    if (options.Indent)
                    {
                        for (int indentLevel = 0; indentLevel < PipeLine.IndentLevel; indentLevel++)
                        {
                            builder.Append("    ");
                        }
                    }
                    if (options.DisplayDuration)
                    {
                        builder.Append("             ");
                    }
                    builder.Append("    ! ");
                    builder.Append(line);

                    yield return builder.ToString();
                }
            }
        }
    }
}