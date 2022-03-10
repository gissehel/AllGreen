namespace AllGreen.Lib.DomainModel
{
    public struct PipedNameOptions
    {
        /// <summary>
        /// This set of options is best suited for a quick overview of a script or a script result in debug view
        /// </summary>
        public static PipedNameOptions Default = new PipedNameOptions
        {
            DisplayDuration = false,
            ForceDisplayIncludedScripts = false,
            DisplayStackTrace = false,
            Indent = false,
            Compact = false,
            DecorateScriptProperties = false,
        };

        /// <summary>
        /// This set of options is best suited for a canonical representation of a script for storage and commit in VCS
        /// </summary>
        public static PipedNameOptions Canonical = new PipedNameOptions
        {
            DisplayDuration = false,
            ForceDisplayIncludedScripts = false,
            DisplayStackTrace = false,
            Indent = false,
            Compact = true,
            DecorateScriptProperties = true,
        };

        /// <summary>
        /// This set of options is best suited for a readable representation of a script or script result for text only report
        /// </summary>
        public static PipedNameOptions Detailled = new PipedNameOptions
        {
            DisplayDuration = true,
            ForceDisplayIncludedScripts = true,
            DisplayStackTrace = true,
            Indent = true,
            Compact = false,
            DecorateScriptProperties = true,
        };

        public bool ForceDisplayIncludedScripts { get; set; }

        public bool DisplayDuration { get; set; }

        public bool DisplayStackTrace { get; set; }

        public bool Indent { get; set; }

        public bool Compact { get; set; }

        public bool DecorateScriptProperties { get; set; }
    }
}