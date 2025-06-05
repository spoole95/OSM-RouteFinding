namespace OverpassNet.Tags;

/// <summary>
/// Flags for highway way tags
/// </summary>
/// <see cref="https://wiki.openstreetmap.org/wiki/Key:highway"/>
[Flags]
public enum Highway : ulong
{
    /// <summary>
    /// A restricted access major divided highway, normally with 2 or more running lanes plus emergency hard shoulder. Equivalent to the Freeway, Autobahn, etc..
    /// </summary>
    Motorway = 1,

    /// <summary>
    /// The most important roads in a country's system that aren't motorways. (Need not necessarily be a divided highway.)
    /// </summary>
    Trunk = 1 << 1,

    /// <summary>
    /// The next most important roads in a country's system. (Often link larger towns.)
    /// </summary>
    Primary = 1 << 2,

    /// <summary>
    /// The next most important roads in a country's system. (Often link towns.)
    /// </summary>
    Secondary = 1 << 3,

    /// <summary>
    /// The next most important roads in a country's system. (Often link smaller towns and villages)
    /// </summary>
    Tertiary = 1 << 4,

    /// <summary>
    /// The least important through roads in a country's system – i.e. minor roads of a lower classification than tertiary, but which serve a purpose other than access to properties. (Often link villages and hamlets.)
    /// The word 'unclassified' is a historical artefact of the UK road system and does not mean that the classification is unknown; you can use highway = road for that.
    /// </summary>
    Unclassified = 1 << 5,

    /// <summary>
    /// Roads which serve as an access to housing, without function of connecting settlements. Often lined with housing.
    /// </summary>
    Residential = 1 << 6,

    /// <summary>
    /// Any type of road (not <see cref="LinkRoad"/>)
    /// </summary>
    Road = Motorway | Trunk | Primary | Secondary | Tertiary | Unclassified | Residential | RoadUnknown,

    /// <summary>
    /// The link roads (sliproads/ramps) leading to/from a motorway from/to a motorway or lower class highway. Normally with the same motorway restrictions.
    /// </summary>
    MotorwayLink = 1 << 7,

    /// <summary>
    /// The link roads (sliproads/ramps) leading to/from a trunk road from/to a trunk road or lower class highway.
    /// </summary>
    TrunkLink = 1 << 8,

    /// <summary>
    /// The link roads (sliproads/ramps) leading to/from a primary road from/to a primary road or lower class highway.
    /// </summary>
    PrimaryLink = 1 << 9,

    /// <summary>
    /// The link roads (sliproads/ramps) leading to/from a secondary road from/to a secondary road or lower class highway.
    /// </summary>
    SecondaryLink = 1 << 10,

    /// <summary>
    /// The link roads (sliproads/ramps) leading to/from a tertiary road from/to a tertiary road or lower class highway.
    /// </summary>
    TertiaryLink = 1 << 11,

    /// <summary>
    /// Any type of link road (sliproads/ramps)
    /// </summary>
    LinkRoad = MotorwayLink | TrunkLink | PrimaryLink | SecondaryLink | TertiaryLink,

    /// <summary>
    /// For living streets, which are residential streets where pedestrians have legal priority over cars, speeds are kept very low.
    /// </summary>
    LivingStreet = 1 << 12,

    /// <summary>
    /// For access roads to, or within an industrial estate, camp site, business park, car park, alleys, etc. Can be used in conjunction with service=* to indicate the type of usage and with access=* to indicate who can use it and in what circumstances.
    /// </summary>
    Service = 1 << 13,

    /// <summary>
    /// For roads used mainly/exclusively for pedestrians in shopping and some residential areas which may allow access by motorised vehicles only for very limited periods of the day. To create a 'square' or 'plaza' create a closed way and tag as pedestrian and also with area=yes.
    /// </summary>
    Pedestrian = 1 << 14,

    /// <summary>
    /// Roads for mostly agricultural or forestry uses. To describe the quality of a track, see tracktype=*. Note: Although tracks are often rough with unpaved surfaces, this tag is not describing the quality of a road but its use. Consequently, if you want to tag a general use road, use one of the general highway values instead of track.
    /// </summary>
    Track = 1 << 15,

    /// <summary>
    /// A busway where the vehicle guided by the way (though not a railway) and is not suitable for other traffic. Please note: this is not a normal bus lane, use access=no, psv=yes instead!
    /// </summary>
    BusGuideway = 1 << 16,

    /// <summary>
    /// For runaway truck ramps, runaway truck lanes, emergency escape ramps, or truck arrester beds. It enables vehicles with braking failure to safely stop.
    /// </summary>
    Escape = 1 << 17,

    /// <summary>
    /// A course or track for (motor) racing
    /// </summary>
    Raceway = 1 << 18,

    /// <summary>
    /// highway=road - not intended for use
    /// </summary>
    RoadUnknown = 1 << 19,

    /// <summary>
    /// A dedicated roadway for bus rapid transit systems
    /// </summary>
    Busway = 1 << 20,

    /// <summary>
    /// Special roads that are not intended for general use, but are used for specific purposes. This includes roads that are not classified as a normal road type, such as a busway or a raceway.
    /// </summary>
    SpecialRoads = LivingStreet | Service | Pedestrian | Track | BusGuideway | Escape | Raceway | Road | Busway,

    /// <summary>
    /// For designated footpaths; i.e., mainly/exclusively for pedestrians. This includes walking tracks and gravel paths. If bicycles are allowed as well, you can indicate this by adding a bicycle=yes tag. Should not be used for paths where the primary or intended usage is unknown. Use highway=pedestrian for pedestrianised roads in shopping or residential areas and highway=track if it is usable by agricultural or similar vehicles. For ramps (sloped paths without steps), combine this tag with incline=*.
    /// </summary>
    Footway = 1 << 21,

    /// <summary>
    /// For horse riders. Pedestrians are usually also permitted, cyclists may be permitted depending on local rules/laws. Motor vehicles are forbidden.
    /// </summary>
    Bridleway = 1 << 22,

    /// <summary>
    /// For flights of steps (stairs) on footways. Use with step_count=* to indicate the number of steps
    /// </summary>
    Steps = 1 << 23,

    /// <summary>
    /// For a hallway inside of a building.
    /// </summary>
    Corridor = 1 << 24,

    /// <summary>
    /// A non-specific path. Use highway=footway for paths mainly for walkers, highway=cycleway for one also usable by cyclists, highway=bridleway for ones available to horse riders as well as walkers and highway=track for ones which is passable by agriculture or similar vehicles.
    /// </summary>
    Path = 1 << 25,

    /// <summary>
    /// A via ferrata is a route equipped with fixed cables, stemples, ladders, and bridges in order to increase ease and security for climbers. These via ferrata require equipment : climbing harness, shock absorber and two short lengths of rope, but do not require a long rope as for climbing.
    /// </summary>
    ViaFerrata = 1 << 26,

    /// <summary>
    /// Any type of path (not including <see cref="Pedestrian"/>)
    /// </summary>
    Paths = Footway | Bridleway | Steps | Corridor | Path | ViaFerrata,

    /// <summary>
    /// For designated cycleways. Add foot=*, though it may be avoided if default-access-restrictions do apply.
    /// </summary>
    Cicleway = 1 << 27,

    /// <summary>
    /// For planned roads, use with proposed=* and a value of the proposed highway value.
    /// </summary>
    Proposed = 1 << 28,

    /// <summary>
    /// For roads under construction. Use construction=* to hold the value for the completed road.
    /// </summary>
    Construction = 1 << 29,

    /// <summary>
    /// An elevator or lift, used to travel vertically, providing passenger and freight access between pathways at different floor levels.	
    /// </summary>
    Elevator = 1 << 30,

    /// <summary>
    /// An area beside a highway where you can safely stop your car in case of breakdown or emergency.
    /// </summary>
    EmergencyBay = 1UL << 31,

    /// <summary>
    /// A vertical or inclined set of steps or rungs intended for climbing or descending of a person with the help of hands.
    /// </summary>
    Ladder = 1UL << 32,

    /// <summary>
    /// A platform at a bus stop or station.
    /// </summary>
    Platform = 1UL << 33,

    /// <summary>
    /// Place where drivers can leave the road to rest, but not refuel.
    /// </summary>
    RestArea = 1UL << 34,

    /// <summary>
    /// A service station to get food and eat something, often found at motorways
    /// </summary>
    Services = 1UL << 35,


    /// <summary>
    /// All commonly used values according to Taginfo. Node or way.
    /// </summary>
    /// <see cref="https://taginfo.openstreetmap.org/keys/highway#values"/>
    UserDefined = 1UL << 36,

    Any = ~ 0UL
}