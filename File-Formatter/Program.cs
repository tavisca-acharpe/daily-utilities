using System.Text.RegularExpressions;

class AwsCredentialCopier
{
    static void Main(string[] args)
    {
        string sourceFile = "source.txt";
        string destinationFolder = @"C:\Users\I759407\.aws";
        string destinationFile = Path.Combine(destinationFolder, "credentials");
        string marker = "# AWS Credentials Here";  // Marker line in destination file where credentials go

        // Read credentials from source file
        var creds = ExtractCredentials(sourceFile);
        if (creds == null)
        {
            Console.WriteLine("Failed to find AWS credentials in source file.");
            return;
        }

        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }

        // Build the output lines
        var outputLines = new[]
        {
            "[default]",
            $"aws_access_key_id={creds.AccessKeyId}",
            $"aws_secret_access_key={creds.SecretAccessKey}",
            $"aws_session_token={creds.SessionToken}",
            "output=json"
        };

        // Write to file
        File.WriteAllLines(destinationFile, outputLines);

        Console.WriteLine("Credentials written successfully to " + destinationFile);
    }
    static AwsCredentials ExtractCredentials(string filepath)
    {
        string[] lines = File.ReadAllLines(filepath);

        string accessKeyId = null;
        string secretAccessKey = null;
        string sessionToken = null;

        // Regex to match keys like "+ AWS_ACCESS_KEY_ID = value"
        var regex = new Regex(@"^\s*\+\s*(AWS_ACCESS_KEY_ID|AWS_SECRET_ACCESS_KEY|AWS_SESSION_TOKEN)\s*=\s*(.+)\s*$", RegexOptions.IgnoreCase);

        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                string key = match.Groups[1].Value.ToUpper();
                string value = match.Groups[2].Value.Trim().Trim('\'', '"');

                switch (key)
                {
                    case "AWS_ACCESS_KEY_ID":
                        accessKeyId = value;
                        break;
                    case "AWS_SECRET_ACCESS_KEY":
                        secretAccessKey = value;
                        break;
                    case "AWS_SESSION_TOKEN":
                        sessionToken = value;
                        break;
                }
            }
        }

        if (!string.IsNullOrEmpty(accessKeyId) &&
            !string.IsNullOrEmpty(secretAccessKey) &&
            !string.IsNullOrEmpty(sessionToken))
        {
            return new AwsCredentials
            {
                AccessKeyId = accessKeyId,
                SecretAccessKey = secretAccessKey,
                SessionToken = sessionToken
            };
        }
        else
        {
            return null;
        }
    }

    class AwsCredentials
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }
    }
}
