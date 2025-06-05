using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverpassNet.Tags;

/// <summary>
/// Node specific tags for highways
/// </summary>
public enum HighwayNodes
{
    BusStop = 1 << 30,
    Crossing = 1 << 31,
    CyclistWaitingAid = 1 << 32,
    Elevator = 1 << 33,
    EmergencyBay = 1 << 34,
    EmergencyAccessPoint = 1 << 35,
    GiveWay = 1 << 36,
    Hitchiking = 1 << 37,
    Ladder = 1 << 38,
    Milestone = 1 << 39,
    MiniRoundabout = 1 << 40,
    MotorwayJunction = 1 << 41,
    PassingPlace = 1 << 42,
    Platform = 1 << 43,
    RestArea = 1 << 44,
    Services = 1 << 45,
    SpeedCamera = 1 << 46,
    SpeedDisplay = 1 << 47,
    /// <summary>
    /// A stop sign.	
    /// Node
    /// </summary>
    Stop = 1 << 48,

    /// <summary>
    /// A street light, lamppost, street lamp, light standard, or lamp standard is a raised source of light on the edge of a road, which is turned on or lit at a certain time every night
    /// Node
    /// </summary>
    StreetLamp = 1 << 49,

    /// <summary>
    /// A toll gantry is a gantry suspended over a way, usually a motorway, as part of a system of electronic toll collection. For a toll booth with any kind of barrier or booth see: barrier=toll_booth
    /// Node
    /// </summary>
    TollGantry = 1 << 50,

    /// <summary>
    /// Mirror that reflects the traffic on one road when direct view is blocked.	
    /// Node
    /// </summary>
    TrafficMirror = 1 << 51,

    /// <summary>
    /// Lights that control the traffic	
    /// Node
    /// </summary>
    TrafficSignals = 1 << 52,

    /// <summary>
    /// Designated place to start on a trail or route	
    /// Node
    /// </summary>
    Trailhead = 1 << 53,

    /// <summary>
    /// A turning circle is a rounded, widened area usually, but not necessarily, at the end of a road to facilitate easier turning of a vehicle. Also known as a cul de sac.
    /// Node
    /// </summary>
    TurningCircle = 1 << 54,

    /// <summary>
    /// A widened area of a highway with a non-traversable island for turning around, often circular and at the end of a road.
    /// Node
    /// </summary>
    TurningLoop = 1 << 55,
}
