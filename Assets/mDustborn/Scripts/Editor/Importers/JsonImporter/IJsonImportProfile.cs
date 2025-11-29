namespace JsonImporterToSO
{
    public interface IJsonImportProfile
    {
        public string ResourcesPath { get; }
        public string OutputFolder { get; }
        public void Import(string json);
    }
}