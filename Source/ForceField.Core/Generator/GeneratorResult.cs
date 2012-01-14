namespace ForceField.Core.Generator
{
    public class GeneratorResult
    {
        public GeneratorResult(string generatedClassName, string code)
        {
            GeneratedClassName = generatedClassName;
            Code = code;
        }

        public string GeneratedClassName { get; private set; }
        public string Code { get; private set; }
    }
}