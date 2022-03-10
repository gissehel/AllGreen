using System;

namespace AllGreen.Lib.Engine.Service.CodeGeneration
{
    public interface ICodeGeneratorContext : IDisposable
    {
        int IndentLevel { get; }

        ICodeGeneratorContext GetSubContext();

        void AddBegin(string line);

        void AddEnd(string line);

        void AddEnd(int indent, string line);

        void Add(string line);

        void AddBegin(int indent, string line);
    }
}