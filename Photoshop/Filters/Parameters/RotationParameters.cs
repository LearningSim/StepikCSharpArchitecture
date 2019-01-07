using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoshop {
    class RotationParameters : IParameters {
		[ParameterInfo(Name="Угол", MaxValue=360, MinValue=0, Increment=2, DefaultValue=90)]
        public double Angle { get; set; }
    }
}
