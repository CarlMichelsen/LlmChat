export enum LlmStreamEventType
{
    /// <summary>
    /// Start of a new message.
    /// </summary>
    messageStart = 1000,

    /// <summary>
    /// Start of a content item for a message.
    /// Messages can have multiple content items so there is also an id.
    /// </summary>
    contentStart = 2000,

    /// <summary>
    /// Content chunk of an exsisting content item.
    /// Messages can have multiple content items so there is also an id.
    /// </summary>
    contentDelta = 3000,

    /// <summary>
    /// Indicates that a content item has finnished streaming.
    /// Messages can have multiple content items so there is also an id.
    /// </summary>
    contentStop = 4000,

    /// <summary>
    /// Indicates that a message has finnished streaming.
    /// Contains Stop reason information.
    /// </summary>
    messageStop = 5000,

    /// <summary>
    /// Contains information about token-usage and a prompt identifier.
    /// This event is the last event to be fired.
    /// </summary>
    totalUsage = 6000,

    /// <summary>
    /// An error occured during streaming.
    /// </summary>
    error = 7000,
}