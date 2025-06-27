using AWS_STS_Token_Generator;


class Program
{
    static async Task Main(string[] args)
    {
        await JenkinsTokenGenerator.JenkinsAwsTokenGenerator();
        AmazonSecurityTokenGenerator.AwsTokenGenerator();
    }
}



