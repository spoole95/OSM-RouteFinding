using OverpassNet.Extensions;

namespace OverpassNet.Tests.Extensions;
public class EnumExtensionsTests
{
    [Fact]
    public void GetFlags_Or_ReturnsCorrectFlags()
    {
        var flags = TestEnum.Flag1 | TestEnum.Flag2;
        var result = flags.GetFlags().ToArray();
        Assert.Equal(2, result.Length);
        Assert.Contains(TestEnum.Flag1, result);
        Assert.Contains(TestEnum.Flag2, result);
    }

    [Fact]
    public void GetFlags_All_ReturnsCorrectFlags()
    {
        var flags = TestEnum.All;
        var result = flags.GetFlags().ToArray();
        Assert.Equal(3, result.Length);
        Assert.Contains(TestEnum.Flag1, result);
        Assert.Contains(TestEnum.Flag2, result);
        Assert.Contains(TestEnum.Flag3, result);
    }

    [Fact]
    public void GetFlags_And_ReturnsCorrectFlags()
    {
        var flags = TestEnum.Flag1 & TestEnum.Flag2;
        var result = flags.GetFlags().ToArray();
        Assert.Equal(2, result.Length);
        Assert.Contains(TestEnum.Flag1, result);
        Assert.Contains(TestEnum.Flag2, result);
    }

    [Fact]
    public void GetIndividualFlags_ReturnsCorrectIndividualFlags()
    {
        var flags = TestEnum.Flag1 | TestEnum.Flag2;
        var result = flags.GetIndividualFlags().ToArray();
        Assert.Equal(3, result.Length);
        Assert.Contains(TestEnum.Flag1, result);
        Assert.Contains(TestEnum.Flag2, result);
        Assert.Contains(TestEnum.Flag3, result);
    }

    [Fact]
    public void GetIndividualFlags_All_ReturnsCorrectIndividualFlags()
    {
        var flags = TestEnum.All;
        var result = flags.GetIndividualFlags().ToArray();
        Assert.Equal(3, result.Length);
        Assert.Contains(TestEnum.Flag1, result);
        Assert.Contains(TestEnum.Flag2, result);
        Assert.Contains(TestEnum.Flag3, result);
    }

    [Flags]
    private enum TestEnum : ushort
    {
        None = 0,
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4,
        All = ushort.MaxValue - 1
    }
}
