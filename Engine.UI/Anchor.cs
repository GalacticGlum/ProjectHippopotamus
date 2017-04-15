namespace Hippopotamus.Engine.UI
{
    public enum Anchor : byte
    {
        /// <summary> Centre of the parent widget. </summary>
        Centre,
        /// <summary> Centre-left of the parent widget. </summary>
        CentreLeft,
        /// <summary> Centre-right of the parent widget. </summary>
        CentreRight,

        /// <summary> Top-left corner of the parent widget.</summary>
        TopLeft,
        /// <summary> Top-right corner of the parent widget.</summary>
        TopRight,
        /// <summary> Top-centre of the parent widget.</summary>
        TopCentre,

        /// <summary> Bottom-left corner of the parent widget.</summary>
        BottomLeft,
        /// <summary> Bottom-right corner of the parent widget.</summary>
        BottomRight,
        /// <summary> Bottom-centre of the parent widget.</summary>
        BottomCentre

        // TODO: Auto anchoring (auto, auto-inline, auto-centre)
    }
}
