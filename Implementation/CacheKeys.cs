namespace Implementation;

public static class CacheKeys
{
    public static string GenerateConversationOptionsCacheKey(Guid creatorIdentifier)
    {
        return $"conversation-options-{creatorIdentifier}";
    }

    public static string GenerateConversationCacheKey(Guid creatorIdentifier, Guid conversationId)
    {
        return $"conversationdto-{creatorIdentifier}-{conversationId}";
    }

    public static string GenerateDefaultSystemMessageCacheKey(Guid creatorIdentifier)
    {
        return $"default-system-message-{creatorIdentifier}";
    }
}
