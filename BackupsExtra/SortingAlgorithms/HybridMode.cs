namespace BackupsExtra.CleaningAlgorithms
{
    public enum HybridMode
    {
        Harsh, // Removing restore point if it doesn't conform to at least one limit
        Soft, // Removing restore point only if it doesn't conform to all limits
    }
}