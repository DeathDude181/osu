﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

namespace osu.Game.Rulesets.Osu.OsuDifficulty.Skills
{
    public class Speed : Skill
    {
        protected override double skillMultiplier => 1400;
        protected override double strainDecayBase => 0.3;

        private const double single_spacing_threshold = 125;
        private const double stream_spacing_threshold = 110;
        private const double almost_diameter = 90;

        protected override double strainValue()
        {
            double distance = current.Distance;

            double speedValue;
            if (distance > single_spacing_threshold)
                speedValue = 2.5;
            else if (distance > stream_spacing_threshold)
                speedValue = 1.6 + 0.9 * (distance - stream_spacing_threshold) / (single_spacing_threshold - stream_spacing_threshold);
            else if (distance > almost_diameter)
                speedValue = 1.2 + 0.4 * (distance - almost_diameter) / (stream_spacing_threshold - almost_diameter);
            else if (distance > almost_diameter / 2)
                speedValue = 0.95 + 0.25 * (distance - almost_diameter / 2) / (almost_diameter / 2);
            else
                speedValue = 0.95;

            return speedValue / current.MS;
        }
    }
}