namespace CityOfHiddenApes.Discord.Core.Arguments
{
    using global::Discord.Commands;

    /// <summary>
    /// A marker type extending from Embeddable. Used to denote
    /// types which are initially hydrated from command text and
    /// which are meant to be bounced back to the caller as a
    /// rich Embed.
    /// </summary>
    [NamedArgumentType]
    public class EmbeddableArgument : Embeddable
    {
    }
}