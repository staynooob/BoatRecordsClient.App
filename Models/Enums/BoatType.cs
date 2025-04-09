using System.ComponentModel;

namespace BoatRecords.Models.Enums;

enum BoatType
{
    [Description("1x")] SingleScull,
    [Description("1x+")] Scull,
    [Description("2x")] DoubleScull,
    [Description("2-")] Pair,
    [Description("2+")] CoxedPair,
    [Description("4x-")] QuadrapleScull,
    [Description("4x+")] CoxedQuadrapleScull,
    [Description("4-")] Four,
    [Description("4+")] CoxedFour,
    [Description("8+")] Eight,
    [Description("8x+")] PairedEight,
}
