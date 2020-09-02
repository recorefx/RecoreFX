namespace ReadmeExample
{
    interface IBlob
    {
        string Name { get; }
        byte[] GetContents();
    }
}