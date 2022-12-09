using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI.DB;

namespace TerrariaFortress
{
    public class ControlPoint : Region
    {
        public Region Region { get; set; }
        public Team Team { get; set; }
        public double PercentCaptured { get; set; }

        public ControlPoint(Region region, Team owner, double percentCaptured = 0)
        {
            Region = region;
            Team = owner;
            PercentCaptured = percentCaptured;
        }
    }
}
