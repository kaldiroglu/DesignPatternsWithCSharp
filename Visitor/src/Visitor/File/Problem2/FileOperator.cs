namespace dev.kaldiroglu.Visitor.File.Problem2;

public class FileOperator
{
    public void Operate(IReadOnlyList<File> files)
    {
        foreach (File aFile in files)
        {
            aFile.Open();
            if (aFile is XMLFile)
            {
                XMLFile xmlFile = (XMLFile)aFile;
                bool valid = xmlFile.Validate();
                if (valid)
                {
                    xmlFile.Read();
                }
            }
            else
            {
                TextFile textFile = (TextFile)aFile;
                bool formatted = textFile.CheckFormat();
                if (formatted)
                {
                    textFile.Read();
                }
            }
            aFile.Close();
        }
    }
}
