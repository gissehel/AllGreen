using System.Collections.Generic;

namespace AllGreen.Lib.Engine.Service.CodeGeneration
{
    public class CodeGeneratorContext : ICodeGeneratorContext
    {
        private ICodeGenerator CodeGenerator { get; set; }

        public int IndentLevel { get; private set; }

        private class LineWithIndent
        {
            public LineWithIndent(string line) : this(0, line)
            {
            }

            public LineWithIndent(int surIndentLevel, string line)
            {
                SurIndentLevel = surIndentLevel;
                Line = line;
            }

            public int SurIndentLevel { get; set; }
            public string Line { get; set; }
        }

        private List<LineWithIndent> OnBegin { get; set; } = new List<LineWithIndent>();

        private List<LineWithIndent> OnEnd { get; set; } = new List<LineWithIndent>();

        private List<ICodeGeneratorContext> SubContexts { get; set; } = new List<ICodeGeneratorContext>();

        private bool Initialized { get; set; } = false;
        private bool Disposed { get; set; } = false;

        public CodeGeneratorContext(ICodeGenerator codeGenerator, int indentLevel)
        {
            CodeGenerator = codeGenerator;
            IndentLevel = indentLevel;
        }

        public ICodeGeneratorContext GetSubContext()
        {
            Init();
            var context = new CodeGeneratorContext(CodeGenerator, IndentLevel + 1); ;
            SubContexts.Add(context);
            return context;
        }

        public void Init()
        {
            if (!Initialized)
            {
                foreach (var line in OnBegin)
                {
                    CodeGenerator.WriteLine(IndentLevel + line.SurIndentLevel, line.Line);
                }
                Initialized = true;
            }
        }

        public void Dispose()
        {
            Init();
            if (!Disposed)
            {
                foreach (var line in OnEnd)
                {
                    CodeGenerator.WriteLine(IndentLevel + line.SurIndentLevel, line.Line);
                }
                Disposed = true;
            }
        }

        public void AddBegin(string line)
        {
            AddBegin(0, line);
        }

        public void AddBegin(int indent, string line)
        {
            if (Initialized)
            {
                throw new AllGreenException(string.Format("A problem occured while trying to generate the Begin line code [{0}]", line));
            }
            OnBegin.Add(new LineWithIndent(indent, line));
        }

        public void AddEnd(string line)
        {
            AddEnd(0, line);
        }

        public void AddEnd(int indent, string line)
        {
            if (Disposed)
            {
                throw new AllGreenException(string.Format("A problem occured while trying to generate the End line code [{0}]", line));
            }
            OnEnd.Add(new LineWithIndent(indent, line));
        }

        public void Add(string line)
        {
            Init();
            CodeGenerator.WriteLine(IndentLevel + 1, line);
        }
    }
}