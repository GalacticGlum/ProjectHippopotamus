using Hippopotamus.Engine.Bridge;
using MoonSharp.Interpreter;

namespace Hippopotamus.World
{
    [LuaExposeType]
    [MoonSharpUserData]
    public struct SineCurveParameter
    {
        public double MinimumAmplitude { get; set; }
        public double MaximumAmplitude { get; set; }
        
        public double MinimumFrequency { get; set; }
        public double MaximumFrequency { get; set; }

        public SineCurveParameter(double minimumAmplitude, double maximumAmplitude, double minimumFrequency, double maximumFrequency)
        {
            MinimumAmplitude = minimumAmplitude;
            MaximumAmplitude = maximumAmplitude;
            MinimumFrequency = minimumFrequency;
            MaximumFrequency = maximumFrequency;
        }
    }
}
