namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// Stage two, part one: the campaign codes become an enum.
/// <para>
/// A real improvement over the strings in <see cref="SwitchingCheckout"/>. A typo is now a
/// compile error, the set of campaigns is written down in one place, and the switch in
/// <see cref="EnumCheckout"/> can be made exhaustive so the compiler names the branch you
/// forgot.
/// </para>
/// <para>
/// The constants keep the Java original's upper-case names rather than C#'s PascalCase,
/// because <see cref="EnumCheckout"/> prints a constant's name on the receipt and the
/// receipt has to read <c>BLACK_FRIDAY</c> in both languages.
/// </para>
/// </summary>
public enum Campaign
{
    NONE,
    STUDENT,
    STAFF,
    BLACK_FRIDAY,
    BUY_TWO_GET_ONE
}
