using AllGreen.Lib.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllGreen.Lib.Core.DomainModel
{
    public interface IJson
    {
        string Json { get; }

        string GetJson(JsonOptions options);
    }
}
